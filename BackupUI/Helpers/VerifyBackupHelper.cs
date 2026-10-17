using SecureServerBackup.Enums;
using SecureServerBackup.Models;
using SecureServerBackup.Services;
using SecureServerBackup.WinForms;

using SecureServerBackupCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Forms;

namespace SecureServerBackup.Helpers
{
	public class VerifyBackupHelper
	{
		AvailableBackupInfo? backup;
		public VerifyBackupHelper(AvailableBackupInfo? _backup)
		{
			backup = _backup;
		}
		public async Task VerifyBackup(Form owner, Label verifyStatusLabel)
		{
			if (backup != null)
			{
				try
				{
					if (verifyStatusLabel != null)
					{
						verifyStatusLabel.Text = $"Verifying backup: {backup.BackupName}\nPath: {backup.BackupPath}\n\nStarting verification process...";
						verifyStatusLabel.ForeColor = System.Drawing.Color.Blue;
					}

					// Determine which image to verify; for multi-volume backups, ask the user.
					int selectedImageIndex = 1;
					var (countSuccess, imageCount, countError) = NativeBackupMountManager.GetImageCount(backup.BackupPath);
					if (!countSuccess)
					{
						CustomDialogService.ShowError(owner, $"Failed to read backup images:\n{countError}", "Error");
						return;
					}

					if (imageCount > 1)
					{
						var (infoSuccess, images, infoError) = NativeBackupMountManager.GetImageInfo(backup.BackupPath);
						if (!infoSuccess || images.Count == 0)
						{
							CustomDialogService.ShowError(owner, $"Failed to get image details:\n{infoError}", "Error");
							return;
						}

						var imageDialog = new ImageSelectionDialog(
							images,
							"Verify Selected",
							"This backup contains multiple restore points. Select which point to verify:")
						{
							Owner = owner
						};

						if (imageDialog.ShowDialog(owner) != DialogResult.OK)
						{
							return;
						}

						selectedImageIndex = imageDialog.SelectedImageIndex;
					}

					var progressWindow = new MountProgressForm(owner,$"Verify Backup");

					progressWindow.SetBackupName($"Verifying {backup.BackupName}");
					progressWindow.SetStatus("Preparing verification...", -1);
					progressWindow.Show();

					var verificationResult = new StringBuilder();
					var verifyJobName = $"{backup.BackupName} [Verify]";
					verificationResult.AppendLine($"Verification started at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
					verificationResult.AppendLine($"Backup: {backup.BackupName}");
					verificationResult.AppendLine($"Type: {backup.BackupType}");
					verificationResult.AppendLine($"Path: {backup.BackupPath}");
					if (imageCount > 1)
					{
						verificationResult.AppendLine($"Image: {selectedImageIndex} of {imageCount}");
					}
					verificationResult.AppendLine(new string('-', 60));

					BackupLogger.LogInfo(verifyJobName,
						$"Verification started for {backup.BackupName}",
						$"Path: {backup.BackupPath}" + (imageCount > 1 ? $" | Image {selectedImageIndex} of {imageCount}" : string.Empty));

					void UpdateVerificationLog(string message)
					{
						verificationResult.AppendLine(message);
						BackupLogger.LogInfo(verifyJobName, message);

						if (verifyStatusLabel != null)
						{
							verifyStatusLabel.Text = verificationResult.ToString();
						}
					}

					try
					{
						var repairChoice = CustomDialogResult.No;

						int verificationResultCode = await Task.Run(() =>
						{
							using var preparedBackup = EncryptedBackupFileService.PrepareForRead(
								owner,
								backup.BackupPath,
								backup.BackupName,
								backup.ProtectedEncryptionPassword);

							var healthMessage = new StringBuilder(1024);

							int checkResult = SecureServerBackupCommon.BackupEngineInterop.CheckBackupImageStatusWithProgress(
								preparedBackup.WorkingPath,
								selectedImageIndex,
								true,
								healthMessage,
								healthMessage.Capacity,
								(percentage, message) =>
								{
									owner.Invoke(() =>
									{
										progressWindow.SetStatus(message, percentage);
										verificationResult.AppendLine($"[{percentage}%] {message}");
										if (verifyStatusLabel != null)
										{
											verifyStatusLabel.Text = verificationResult.ToString();
										}
									});
								});

							owner.Invoke(() =>
							{
								progressWindow.SetStatus(healthMessage.ToString(), checkResult == 0 ? 100 : -1);
								UpdateVerificationLog($"SSB health check result: {checkResult}");
								UpdateVerificationLog($"Health details: {healthMessage}");
							});

							if (checkResult == 0)
							{
								return 0;
							}

							owner.Invoke(() => progressWindow.SetStatus("Verification failed.", -1));
							return checkResult;
						});

						if (verificationResultCode != 0)
						{
							repairChoice = CustomDialogService.ShowQuestion(
								owner,
								"Verification failed. Attempt to repair corrupted backup file?",
								"Repair Backup File");

							if (repairChoice == CustomDialogResult.Yes)
							{
								progressWindow.SetStatus("Attempting repair of corrupted backup file...", -1);

								int repairResultCode = await Task.Run(() =>
								{
									using var preparedBackup = EncryptedBackupFileService.PrepareForRead(
										owner,
										backup.BackupPath,
										backup.BackupName,
										backup.ProtectedEncryptionPassword);

									var repairMessage = new StringBuilder(1024);

									int repairResult = SecureServerBackupCommon.BackupEngineInterop.RepairBackupImageStatusWithProgress(
										preparedBackup.WorkingPath,
										selectedImageIndex,
										null,
										0,
										false,
										repairMessage,
										repairMessage.Capacity,
										(percentage, message) =>
										{
											owner.Invoke(() =>
											{
												progressWindow.SetStatus(message, percentage);
												verificationResult.AppendLine($"[{percentage}%] {message}");
												if (verifyStatusLabel != null)
												{
													verifyStatusLabel.Text = verificationResult.ToString();
												}
											});
										});

									owner.Invoke(() =>
									{
										UpdateVerificationLog($"SSB repair result: {repairResult}");
										UpdateVerificationLog($"Repair details: {repairMessage}");
										progressWindow.SetStatus(
											repairResult == 0 ? "Repair completed successfully." : "Repair failed.",
											repairResult == 0 ? 100 : -1);
									});

									return repairResult;
								});

								verificationResultCode = repairResultCode;
							}

							if (repairChoice != CustomDialogResult.Yes)
							{
								UpdateVerificationLog("Repair declined by user.");
							}
						}

						verificationResult.AppendLine(new string('-', 60));

						if (verificationResultCode == 0)
						{
							verificationResult.AppendLine($"Verification completed successfully at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
							verificationResult.AppendLine("RESULT: BACKUP IS VALID");

							if (verifyStatusLabel != null)
							{
								verifyStatusLabel.Text = verificationResult.ToString();
								verifyStatusLabel.ForeColor = System.Drawing.Color.Green;
							}

							BackupLogger.LogSuccess(verifyJobName,
								$"Verification passed for {backup.BackupName}",
								backup.BackupPath);

							CustomDialogService.ShowSuccess(owner,
								$"Backup verification completed successfully!\n\nBackup: {backup.BackupName}\n\nThe backup integrity has been verified and is valid.",
								"Verification Complete");
						}
						else
						{
							verificationResult.AppendLine($"Verification completed with errors at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
							verificationResult.AppendLine($"RESULT: BACKUP FAILED VERIFICATION (CODE {verificationResultCode})");

							if (verifyStatusLabel != null)
							{
								verifyStatusLabel.Text = verificationResult.ToString();
								verifyStatusLabel.ForeColor = System.Drawing.Color.Red;
							}

							BackupLogger.LogError(verifyJobName,
								$"Verification failed for {backup.BackupName} (code {verificationResultCode})",
								backup.BackupPath);

							CustomDialogService.ShowError(owner,
								$"Backup verification failed!\n\nBackup: {backup.BackupName}\n\nThe backup may be corrupted or inaccessible.",
								"Verification Failed");
						}
					}
					catch (Exception verifyEx)
					{
						verificationResult.AppendLine(new string('-', 60));
						verificationResult.AppendLine($"ERROR: {verifyEx.Message}");
						verificationResult.AppendLine($"Verification failed at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

						if (verifyStatusLabel != null)
						{
							verifyStatusLabel.Text = verificationResult.ToString();
							verifyStatusLabel.ForeColor = System.Drawing.Color.Red;
						}

						BackupLogger.LogError(verifyJobName,
							$"Verification exception for {backup.BackupName}: {verifyEx.Message}",
							verifyEx.ToString());

						CustomDialogService.ShowError(owner,
							$"Backup verification failed:\n{verifyEx.Message}",
							"Verification Error");
					}
					finally
					{
						progressWindow.CloseProgress();
					}
				}
				catch (Exception ex)
				{
					if (verifyStatusLabel != null)
					{
						verifyStatusLabel.Text = $"Error initializing verification:\n{ex.Message}";
						verifyStatusLabel.ForeColor = System.Drawing.Color.Red;
					}

					BackupLogger.LogError($"{backup.BackupName} [Verify]",
						$"Failed to start verification for {backup.BackupName}: {ex.Message}",
						ex.ToString());

					CustomDialogService.ShowError($"Error initializing verification:\n{ex.Message}",
								  "Error");
				}
			}
		}

	}
}

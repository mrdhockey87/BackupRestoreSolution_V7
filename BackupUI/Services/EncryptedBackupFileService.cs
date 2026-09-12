using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using SecureServerBackupCommon;
using SecureServerBackup.WinForms;
using Forms = System.Windows.Forms;

namespace SecureServerBackup.Services
{
    public sealed class PreparedBackupFile : IDisposable
    {
        private bool _disposed;

        public string OriginalPath { get; init; } = string.Empty;
        public string WorkingPath { get; init; } = string.Empty;
        public bool IsTemporary { get; init; }

        ~PreparedBackupFile()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (IsTemporary)
            {
                BackupEncryptionService.DeleteTemporaryFile(WorkingPath);
            }

            _disposed = true;
        }
    }

    public static class EncryptedBackupFileService
    {
        public static PreparedBackupFile PrepareForRead(Window? owner, string backupPath, string backupName, string? protectedPassword = null)
        {
            Forms.IWin32Window? formsOwner = null;
            if (owner != null)
            {
                var helper = new WindowInteropHelper(owner);
                if (helper.Handle != IntPtr.Zero)
                {
                    formsOwner = new NativeWindowOwner(helper.Handle);
                }
            }

            return PrepareForReadForWinForms(formsOwner, backupPath, backupName, protectedPassword);
        }

        public static PreparedBackupFile PrepareForReadForWinForms(Forms.IWin32Window? owner, string backupPath, string backupName, string? protectedPassword = null)
        {
            if (!BackupEncryptionService.IsEncryptedBackupFile(backupPath))
            {
                return new PreparedBackupFile
                {
                    OriginalPath = backupPath,
                    WorkingPath = backupPath,
                    IsTemporary = false
                };
            }

            string? promptError = null;
            bool allowStoredPassword = !string.IsNullOrWhiteSpace(protectedPassword);

            while (true)
            {
                try
                {
                    string password;
                    if (allowStoredPassword)
                    {
                        password = BackupEncryptionService.UnprotectPassword(protectedPassword!);
                        allowStoredPassword = false;
                    }
                    else
                    {
                        using var prompt = new BackupPasswordPromptForm(backupName);

                        if (!string.IsNullOrWhiteSpace(promptError))
                        {
                            prompt.SetError(promptError);
                        }

                        if (prompt.ShowDialog(owner) != Forms.DialogResult.OK)
                        {
                            throw new OperationCanceledException("The encrypted backup password prompt was cancelled.");
                        }

                        password = prompt.EnteredPassword;
                    }

                    string tempPath = BackupEncryptionService.DecryptFileToTemporaryLocation(backupPath, password);
                    return new PreparedBackupFile
                    {
                        OriginalPath = backupPath,
                        WorkingPath = tempPath,
                        IsTemporary = true
                    };
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    promptError = ex.Message;
                    if (owner == null)
                    {
                        throw;
                    }
                }
            }
        }

        private sealed class NativeWindowOwner : Forms.IWin32Window
        {
            public NativeWindowOwner(IntPtr handle)
            {
                Handle = handle;
            }

            public IntPtr Handle { get; }
        }
    }
}

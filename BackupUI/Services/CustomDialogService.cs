using System;
using System.Diagnostics;
using System.Windows.Forms;
using SecureServerBackup.Enums;
using SecureServerBackup.WinForms;

namespace SecureServerBackup.Services
{
	/// <summary>
	/// Service for showing custom themed dialogs throughout the WinForms application.
	/// </summary>
	public static class CustomDialogService
	{
		public static void ShowInfo(string message, string title = "Information")
		{
			Show(message, title, DialogButtons.OK, DialogIcon.Information);
		}

		public static void ShowInfo(IWin32Window owner, string message, string title = "Information")
		{
			Show(owner, message, title, DialogButtons.OK, DialogIcon.Information);
		}

		public static void ShowSuccess(string message, string title = "Success")
		{
			Show(message, title, DialogButtons.OK, DialogIcon.Success);
		}

		public static void ShowSuccess(IWin32Window owner, string message, string title = "Success")
		{
			Show(owner, message, title, DialogButtons.OK, DialogIcon.Success);
		}

		public static void ShowWarning(string message, string title = "Warning")
		{
			Show(message, title, DialogButtons.OK, DialogIcon.Warning);
		}

		public static void ShowWarning(IWin32Window owner, string message, string title = "Warning")
		{
			Show(owner, message, title, DialogButtons.OK, DialogIcon.Warning);
		}

		public static void ShowError(string message, string title = "Error")
		{
			Show(message, title, DialogButtons.OK, DialogIcon.Error);
		}

		public static void ShowError(IWin32Window owner, string message, string title = "Error")
		{
			Show(owner, message, title, DialogButtons.OK, DialogIcon.Error);
		}

		public static CustomDialogResult ShowQuestion(string message, string title = "Question")
		{
			return Show(message, title, DialogButtons.YesNo, DialogIcon.Question);
		}

		public static CustomDialogResult ShowQuestion(IWin32Window owner, string message, string title = "Question")
		{
			return Show(owner, message, title, DialogButtons.YesNo, DialogIcon.Question);
		}

		public static CustomDialogResult ShowConfirmation(string message, string title = "Confirm")
		{
			return Show(message, title, DialogButtons.YesNoCancel, DialogIcon.Question);
		}

		public static CustomDialogResult ShowConfirmation(IWin32Window owner, string message, string title = "Confirm")
		{
			return Show(owner, message, title, DialogButtons.YesNoCancel, DialogIcon.Question);
		}

		public static CustomDialogResult ShowOKCancel(string message, string title = "Confirm", DialogIcon icon = DialogIcon.Question)
		{
			return Show(message, title, DialogButtons.OKCancel, icon);
		}

		public static CustomDialogResult ShowOKCancel(IWin32Window owner, string message, string title = "Confirm", DialogIcon icon = DialogIcon.Question)
		{
			return Show(owner, message, title, DialogButtons.OKCancel, icon);
		}

		public static CustomDialogResult Show(string message, string title, DialogButtons buttons, DialogIcon icon)
		{
			var owner = GetOwnerWindow();
			return owner is null
				? ShowInternal(null, message, title, buttons, icon)
				: ShowInternal(owner, message, title, buttons, icon);
		}

		public static CustomDialogResult Show(IWin32Window owner, string message, string title, DialogButtons buttons, DialogIcon icon)
		{
			return ShowInternal(owner, message, title, buttons, icon);
		}

		private static IWin32Window? GetOwnerWindow()
		{
			var mainForm = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;
			return mainForm is not null && mainForm.IsHandleCreated ? mainForm : null;
		}

		private static CustomDialogResult ShowInternal(IWin32Window? owner, string message, string title, DialogButtons buttons, DialogIcon icon)
		{
			try
			{
				using var dialog = new CustomDialogForm();
				dialog.Configure(message, title, buttons, icon);

				if (owner is not null)
				{
					dialog.ShowDialog(owner);
				}
				else
				{
					dialog.ShowDialog();
				}

				return dialog.Result;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"CustomDialog failed: {ex.Message}");
				if (owner is not null)
				{
					MessageBox.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				return CustomDialogResult.OK;
			}
		}
	}
}

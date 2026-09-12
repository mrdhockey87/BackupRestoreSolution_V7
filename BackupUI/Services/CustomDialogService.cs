using System;
using System.Windows;
using Forms = System.Windows.Forms;

namespace SecureServerBackup
{
    /// <summary>
    /// Service for showing custom themed dialogs throughout the application
    /// Replaces MessageBox with turquoise-themed dialogs
    /// </summary>
    public static class CustomDialogService
    {
        /// <summary>
        /// Shows an information dialog
        /// </summary>
        public static void ShowInfo(string message, string title = "Information")
        {
            Show(message, title, DialogButtons.OK, DialogIcon.Information);
        }

        /// <summary>
        /// Shows an information dialog with specified owner window
        /// </summary>
        public static void ShowInfo(Window owner, string message, string title = "Information")
        {
            Show(owner, message, title, DialogButtons.OK, DialogIcon.Information);
        }

        public static void ShowInfo(Forms.IWin32Window owner, string message, string title = "Information")
        {
            Show(owner, message, title, DialogButtons.OK, DialogIcon.Information);
        }

        /// <summary>
        /// Shows a success dialog
        /// </summary>
        public static void ShowSuccess(string message, string title = "Success")
        {
            Show(message, title, DialogButtons.OK, DialogIcon.Success);
        }

        /// <summary>
        /// Shows a success dialog with specified owner window
        /// </summary>
        public static void ShowSuccess(Window owner, string message, string title = "Success")
        {
            Show(owner, message, title, DialogButtons.OK, DialogIcon.Success);
        }

        public static void ShowSuccess(Forms.IWin32Window owner, string message, string title = "Success")
        {
            Show(owner, message, title, DialogButtons.OK, DialogIcon.Success);
        }

        /// <summary>
        /// Shows a warning dialog
        /// </summary>
        public static void ShowWarning(string message, string title = "Warning")
        {
            Show(message, title, DialogButtons.OK, DialogIcon.Warning);
        }

        /// <summary>
        /// Shows a warning dialog with specified owner window
        /// </summary>
        public static void ShowWarning(Window owner, string message, string title = "Warning")
        {
            Show(owner, message, title, DialogButtons.OK, DialogIcon.Warning);
        }

        public static void ShowWarning(Forms.IWin32Window owner, string message, string title = "Warning")
        {
            Show(owner, message, title, DialogButtons.OK, DialogIcon.Warning);
        }

        /// <summary>
        /// Shows an error dialog
        /// </summary>
        public static void ShowError(string message, string title = "Error")
        {
            Show(message, title, DialogButtons.OK, DialogIcon.Error);
        }

        /// <summary>
        /// Shows an error dialog with specified owner window
        /// </summary>
        public static void ShowError(Window owner, string message, string title = "Error")
        {
            Show(owner, message, title, DialogButtons.OK, DialogIcon.Error);
        }

        public static void ShowError(Forms.IWin32Window owner, string message, string title = "Error")
        {
            Show(owner, message, title, DialogButtons.OK, DialogIcon.Error);
        }

        /// <summary>
        /// Shows a question dialog with Yes/No buttons
        /// </summary>
        public static CustomDialogResult ShowQuestion(string message, string title = "Question")
        {
            return Show(message, title, DialogButtons.YesNo, DialogIcon.Question);
        }

        /// <summary>
        /// Shows a question dialog with Yes/No buttons and specified owner window
        /// </summary>
        public static CustomDialogResult ShowQuestion(Window owner, string message, string title = "Question")
        {
            return Show(owner, message, title, DialogButtons.YesNo, DialogIcon.Question);
        }

        public static CustomDialogResult ShowQuestion(Forms.IWin32Window owner, string message, string title = "Question")
        {
            return Show(owner, message, title, DialogButtons.YesNo, DialogIcon.Question);
        }

        /// <summary>
        /// Shows a confirmation dialog with Yes/No/Cancel buttons
        /// </summary>
        public static CustomDialogResult ShowConfirmation(string message, string title = "Confirm")
        {
            return Show(message, title, DialogButtons.YesNoCancel, DialogIcon.Question);
        }

        /// <summary>
        /// Shows a confirmation dialog with Yes/No/Cancel buttons and specified owner window
        /// </summary>
        public static CustomDialogResult ShowConfirmation(Window owner, string message, string title = "Confirm")
        {
            return Show(owner, message, title, DialogButtons.YesNoCancel, DialogIcon.Question);
        }

        public static CustomDialogResult ShowConfirmation(Forms.IWin32Window owner, string message, string title = "Confirm")
        {
            return Show(owner, message, title, DialogButtons.YesNoCancel, DialogIcon.Question);
        }

        /// <summary>
        /// Shows a dialog with OK/Cancel buttons
        /// </summary>
        public static CustomDialogResult ShowOKCancel(string message, string title = "Confirm", DialogIcon icon = DialogIcon.Question)
        {
            return Show(message, title, DialogButtons.OKCancel, icon);
        }

        /// <summary>
        /// Shows a dialog with OK/Cancel buttons and specified owner window
        /// </summary>
        public static CustomDialogResult ShowOKCancel(Window owner, string message, string title = "Confirm", DialogIcon icon = DialogIcon.Question)
        {
            return Show(owner, message, title, DialogButtons.OKCancel, icon);
        }

        public static CustomDialogResult ShowOKCancel(Forms.IWin32Window owner, string message, string title = "Confirm", DialogIcon icon = DialogIcon.Question)
        {
            return Show(owner, message, title, DialogButtons.OKCancel, icon);
        }

        /// <summary>
        /// Shows a custom dialog with specified parameters
        /// </summary>
        public static CustomDialogResult Show(string message, string title, DialogButtons buttons, DialogIcon icon)
        {
            if (!HasActiveWpfApplication())
            {
                Forms.DialogResult result = Forms.MessageBox.Show(
                    message,
                    title,
                    ToFormsButtons(buttons),
                    ToFormsIcon(icon));

                return FromFormsDialogResult(result);
            }

            try
            {
                var dialog = new CustomDialog();
                
                // Set owner to main window if available
                if (Application.Current?.MainWindow != null && Application.Current.MainWindow.IsLoaded)
                {
                    dialog.Owner = Application.Current.MainWindow;
                }

                dialog.Configure(message, title, buttons, icon);

                // Show dialog and return result
                dialog.ShowDialog();
                return dialog.Result;
            }
            catch (Exception ex)
            {
                // Fallback to MessageBox if custom dialog fails
                System.Diagnostics.Debug.WriteLine($"CustomDialog failed: {ex.Message}");
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                return CustomDialogResult.OK;
            }
        }

        private static bool HasActiveWpfApplication()
        {
            return Application.Current != null;
        }

        public static CustomDialogResult Show(Forms.IWin32Window owner, string message, string title, DialogButtons buttons, DialogIcon icon)
        {
            try
            {
                using var dialog = new WinForms.CustomDialogForm();
                dialog.Configure(message, title, buttons, icon);
                dialog.ShowDialog(owner);
                return dialog.Result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"WinForms dialog failed: {ex.Message}");
                var result = Forms.MessageBox.Show(
                    owner,
                    message,
                    title,
                    ToFormsButtons(buttons),
                    ToFormsIcon(icon));

                return FromFormsDialogResult(result);
            }
        }

        /// <summary>
        /// Shows dialog with owner window specified
        /// </summary>
        public static CustomDialogResult Show(Window owner, string message, string title, DialogButtons buttons, DialogIcon icon)
        {
            try
            {
                var dialog = new CustomDialog
                {
                    Owner = owner
                };

                dialog.Configure(message, title, buttons, icon);

                // Show dialog and return result
                dialog.ShowDialog();
                return dialog.Result;
            }
            catch (Exception ex)
            {
                // Fallback to MessageBox if custom dialog fails
                System.Diagnostics.Debug.WriteLine($"CustomDialog failed: {ex.Message}");
                MessageBox.Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                return CustomDialogResult.OK;
            }
        }

        /// <summary>
        /// Converts MessageBoxResult to CustomDialogResult for compatibility
        /// </summary>
        public static CustomDialogResult FromMessageBoxResult(MessageBoxResult result)
        {
            return result switch
            {
                MessageBoxResult.OK => CustomDialogResult.OK,
                MessageBoxResult.Cancel => CustomDialogResult.Cancel,
                MessageBoxResult.Yes => CustomDialogResult.Yes,
                MessageBoxResult.No => CustomDialogResult.No,
                _ => CustomDialogResult.None
            };
        }

        /// <summary>
        /// Converts CustomDialogResult to MessageBoxResult for compatibility
        /// </summary>
        public static MessageBoxResult ToMessageBoxResult(CustomDialogResult result)
        {
            return result switch
            {
                CustomDialogResult.OK => MessageBoxResult.OK,
                CustomDialogResult.Cancel => MessageBoxResult.Cancel,
                CustomDialogResult.Yes => MessageBoxResult.Yes,
                CustomDialogResult.No => MessageBoxResult.No,
                _ => MessageBoxResult.None
            };
        }

        private static Forms.MessageBoxButtons ToFormsButtons(DialogButtons buttons)
        {
            return buttons switch
            {
                DialogButtons.OK => Forms.MessageBoxButtons.OK,
                DialogButtons.OKCancel => Forms.MessageBoxButtons.OKCancel,
                DialogButtons.YesNo => Forms.MessageBoxButtons.YesNo,
                DialogButtons.YesNoCancel => Forms.MessageBoxButtons.YesNoCancel,
                _ => Forms.MessageBoxButtons.OK
            };
        }

        private static Forms.MessageBoxIcon ToFormsIcon(DialogIcon icon)
        {
            return icon switch
            {
                DialogIcon.Information => Forms.MessageBoxIcon.Information,
                DialogIcon.Success => Forms.MessageBoxIcon.Information,
                DialogIcon.Warning => Forms.MessageBoxIcon.Warning,
                DialogIcon.Error => Forms.MessageBoxIcon.Error,
                DialogIcon.Question => Forms.MessageBoxIcon.Question,
                _ => Forms.MessageBoxIcon.None
            };
        }

        private static CustomDialogResult FromFormsDialogResult(Forms.DialogResult result)
        {
            return result switch
            {
                Forms.DialogResult.OK => CustomDialogResult.OK,
                Forms.DialogResult.Cancel => CustomDialogResult.Cancel,
                Forms.DialogResult.Yes => CustomDialogResult.Yes,
                Forms.DialogResult.No => CustomDialogResult.No,
                _ => CustomDialogResult.None
            };
        }
    }
}

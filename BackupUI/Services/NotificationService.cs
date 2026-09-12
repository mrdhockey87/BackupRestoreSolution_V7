using System;
using System.Threading;
using Forms = System.Windows.Forms;

namespace SecureServerBackup.Services
{
    public static class NotificationService
    {
        private static bool notificationsEnabled = true;
        private static SynchronizationContext? uiSynchronizationContext;
        private static Action? showActivityAction;

        public static void Initialize()
        {
            uiSynchronizationContext ??= SynchronizationContext.Current;
            // Notification system initialized
        }

        public static void ConfigureUiContext(SynchronizationContext synchronizationContext, Action? showActivityHandler = null)
        {
            uiSynchronizationContext = synchronizationContext ?? throw new ArgumentNullException(nameof(synchronizationContext));
            showActivityAction = showActivityHandler;
        }

        public static void ShowBackupFailureNotification(string jobName, string message)
        {
            if (!notificationsEnabled)
                return;

            try
            {
                // Show tray balloon notification (fallback for now)
                InvokeOnUiThread(() =>
                {
                    Forms.MessageBox.Show(
                        $"Backup Failed!\n\nJob: {jobName}\n\n{message}\n\nCheck the Activity tab for details.",
                        "Backup Failed",
                        Forms.MessageBoxButtons.OK,
                        Forms.MessageBoxIcon.Warning);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Notification failed: {ex.Message}");
            }
        }

        public static void ShowValidationFailureNotification(string jobName, string backupPath)
        {
            if (!notificationsEnabled)
                return;

            try
            {
                InvokeOnUiThread(() =>
                {
                    var result = Forms.MessageBox.Show(
                        $"Backup Validation Failed!\n\nJob: {jobName}\n\nAuto-recovery initiated.\nFailed backup renamed and new full backup will be created.\n\nView Activity tab for details?",
                        "Backup Validation Failed",
                        Forms.MessageBoxButtons.YesNo,
                        Forms.MessageBoxIcon.Warning);

                    if (result == Forms.DialogResult.Yes)
                    {
                        if (showActivityAction != null)
                        {
                            showActivityAction();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Notification failed: {ex.Message}");
            }
        }

        public static void ShowBackupSuccessNotification(string jobName)
        {
            if (!notificationsEnabled)
                return;

            try
            {
                // Success notifications are less intrusive - just log
                System.Diagnostics.Debug.WriteLine($"Backup completed successfully: {jobName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Notification failed: {ex.Message}");
            }
        }

        public static void SetNotificationsEnabled(bool enabled)
        {
            notificationsEnabled = enabled;
        }

        private static void InvokeOnUiThread(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (uiSynchronizationContext != null)
            {
                uiSynchronizationContext.Send(_ => action(), null);
                return;
            }

            action();
        }
    }
}


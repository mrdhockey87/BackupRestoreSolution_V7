using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace SecureServerBackup.Services
{
    /// <summary>
    /// Manages window position persistence for the main window
    /// </summary>
    public static class WindowPositionManager
    {
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BackupRestoreApp",
            "window-position.json");

        /// <summary>
        /// Saves the main window's position and size
        /// </summary>
        public static void SaveMainWindowPosition(Window window)
        {
            try
            {
                var position = new WindowPosition
                {
                    Left = window.Left,
                    Top = window.Top,
                    Width = window.Width,
                    Height = window.Height,
                    WindowState = window.WindowState,
                    IsMaximized = window.WindowState == WindowState.Maximized
                };

                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);

                // Save to JSON
                var json = JsonSerializer.Serialize(position, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save window position: {ex.Message}");
            }
        }

        /// <summary>
        /// Restores the main window's position and size
        /// </summary>
        public static void RestoreMainWindowPosition(Window window)
        {
            try
            {
                if (!File.Exists(SettingsPath))
                {
                    // First run - center window on screen
                    CenterWindow(window);
                    return;
                }

                var json = File.ReadAllText(SettingsPath);
                var position = JsonSerializer.Deserialize<WindowPosition>(json);

                if (position != null && IsPositionValid(position))
                {
                    window.Left = position.Left;
                    window.Top = position.Top;
                    window.Width = position.Width;
                    window.Height = position.Height;
                    window.WindowState = position.IsMaximized ? WindowState.Maximized : position.WindowState;
                }
                else
                {
                    // Invalid position - center window
                    CenterWindow(window);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to restore window position: {ex.Message}");
                // Fall back to centering window
                CenterWindow(window);
            }
        }

        /// <summary>
        /// Validates that the saved position is visible on current screen configuration
        /// </summary>
        private static bool IsPositionValid(WindowPosition position)
        {
            // Check if window is within any screen bounds
            var rect = new Rect(position.Left, position.Top, position.Width, position.Height);

            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                var workingArea = new Rect(
                    screen.WorkingArea.Left,
                    screen.WorkingArea.Top,
                    screen.WorkingArea.Width,
                    screen.WorkingArea.Height);

                // Check if at least part of the window is visible
                if (workingArea.IntersectsWith(rect))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Centers the window on the primary screen
        /// </summary>
        private static void CenterWindow(Window window)
        {
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Configures a child window to open relative to the main window
        /// </summary>
        public static void SetChildWindowPosition(Window childWindow, Window mainWindow)
        {
            childWindow.Owner = mainWindow;
            childWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        /// <summary>
        /// Saves the main form's position and size.
        /// </summary>
        public static void SaveMainWindowPosition(global::System.Windows.Forms.Form form)
        {
            try
            {
                var bounds = form.WindowState == global::System.Windows.Forms.FormWindowState.Normal
                    ? form.Bounds
                    : form.RestoreBounds;

                var position = new WindowPosition
                {
                    Left = bounds.Left,
                    Top = bounds.Top,
                    Width = bounds.Width,
                    Height = bounds.Height,
                    IsMaximized = form.WindowState == global::System.Windows.Forms.FormWindowState.Maximized
                };

                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);

                var json = JsonSerializer.Serialize(position, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save form position: {ex.Message}");
            }
        }

        /// <summary>
        /// Restores the main form's position and size.
        /// </summary>
        public static void RestoreMainWindowPosition(global::System.Windows.Forms.Form form)
        {
            try
            {
                if (!File.Exists(SettingsPath))
                {
                    CenterWindow(form);
                    return;
                }

                var json = File.ReadAllText(SettingsPath);
                var position = JsonSerializer.Deserialize<WindowPosition>(json);

                if (position != null && IsPositionValid(position))
                {
                    form.StartPosition = global::System.Windows.Forms.FormStartPosition.Manual;
                    form.SetBounds((int)position.Left, (int)position.Top, (int)position.Width, (int)position.Height);
                    form.WindowState = position.IsMaximized
                        ? global::System.Windows.Forms.FormWindowState.Maximized
                        : global::System.Windows.Forms.FormWindowState.Normal;
                }
                else
                {
                    CenterWindow(form);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to restore form position: {ex.Message}");
                CenterWindow(form);
            }
        }

        /// <summary>
        /// Configures a child form to open relative to its parent form.
        /// </summary>
        public static void SetChildWindowPosition(global::System.Windows.Forms.Form childForm, global::System.Windows.Forms.Form mainForm)
        {
            childForm.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
            childForm.Owner = mainForm;
        }

        private class WindowPosition
        {
            public double Left { get; set; }
            public double Top { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public WindowState WindowState { get; set; }
            public bool IsMaximized { get; set; }
        }

        private static void CenterWindow(global::System.Windows.Forms.Form form)
        {
            form.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
        }
    }
}

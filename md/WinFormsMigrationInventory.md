# BackupUI WPF Surface Inventory

## Summary
- XAML files: 34
- WPF windows: 31
- WPF user controls: 1
- XAML code-behind files: 33
- Theme dictionaries: 1 (`Themes/TurquoiseTheme.xaml`)
- WPF-specific converter classes: 1 (`Converters/StringToVisibilityConverter.cs`)

## WPF Application Bootstrap
- `App.xaml`
- `App.xaml.cs`
- Current startup responsibilities:
  - single-instance mutex
  - elevation / restart-as-admin
  - splash screen startup sequence
  - main window creation
  - startup error dialogs

## Main Shell
- `MainWindow.xaml`
- `MainWindow.xaml.cs`
- Major responsibilities:
  - tabbed shell for Backup / Restore / Activity / Service workflows
  - menu and status bar
  - job list rendering using WPF templating
  - dispatcher timers for running-backup monitoring and activity warning refresh
  - modeless child progress window orchestration via `Application.Current.Windows`
  - dynamic WPF control creation in code-behind

## WPF Windows To Convert
- `Windows/AboutWindow`
- `Windows/ActivityDetailWindow`
- `Windows/ActivityManagementWindow`
- `Windows/BackupCompletionDialog`
- `Windows/BackupPasswordPromptWindow`
- `Windows/BackupProgressWindow`
- `Windows/BackupWindow`
- `Windows/BackupWindowNew`
- `Windows/CustomDialog`
- `Windows/DiskSelectionWindow`
- `Windows/ExclusionsManagementWindow`
- `Windows/ExportOptionsDialog`
- `Windows/ImageSelectionDialog`
- `Windows/ImportBackupWindow`
- `Windows/MountProgressWindow`
- `Windows/NetworkPathDialog`
- `Windows/NextRunTimeEditWindow`
- `Windows/RecoveryEnvironmentWindow`
- `Windows/RestoreItemSelectionWindow`
- `Windows/RestorePointSelectionWindow`
- `Windows/RestoreProgressWindow`
- `Windows/RestoreVolumeSelectionDialog`
- `Windows/RestoreWindow`
- `Windows/RestoreWindowNew`
- `Windows/ScheduleManagementWindow`
- `Windows/ServiceManagementWindow`
- `Windows/SplashScreen`
- `Windows/TempPathSelectionDialog`
- `Windows/VolumeConfigurationWindow`
- `Windows/VolumeSelectionWindow`

## Custom WPF Control
- `Controls/VolumeResizeControl.xaml`
- `Controls/VolumeResizeControl.xaml.cs`
- Uses `Canvas`, `Border`, `TextBlock`, `Path`, `Rectangle`, `Line`, `Thumb`, WPF brushes, and mouse-drag behavior.
- Will need a custom WinForms `UserControl` with manual drawing and resize-hit handling.

## Shared WPF Infrastructure To Replace
- `Services/WindowPositionManager.cs`
  - currently typed around `System.Windows.Window`
- `Services/CustomDialogService.cs`
  - currently typed around `System.Windows.Window`, `Application.Current`, and WPF `MessageBox`
- `Services/NotificationService.cs`
  - currently uses `Application.Current.Dispatcher` and WPF `MessageBox`
- `Converters/StringToVisibilityConverter.cs`
  - WPF-only visibility converter
- `Themes/TurquoiseTheme.xaml`
  - WPF-only resource dictionary

## High-Risk / High-Complexity Areas
1. `MainWindow.xaml(.cs)`
2. `Windows/BackupWindowNew.xaml(.cs)`
3. `Windows/RestoreWindowNew.xaml(.cs)`
4. `Windows/VolumeConfigurationWindow.xaml(.cs)`
5. `Controls/VolumeResizeControl.xaml(.cs)`
6. `Windows/SplashScreen.xaml(.cs)`
7. `Windows/CustomDialog.xaml(.cs)`

## Key WPF Constructs In Use
- `System.Windows.Window`
- `System.Windows.Controls.*`
- `System.Windows.Media.*`
- `System.Windows.Shapes.*`
- `System.Windows.Threading.DispatcherTimer`
- `Application.Current.Windows`
- `Visibility`
- `MessageBoxButton`, `MessageBoxImage`, `MessageBoxResult`
- XAML resources / merged dictionaries / data templates
- dynamic control creation in code-behind

## Migration Implication
This is a full UI-platform rewrite inside the existing project, not a project property flip. The safest path is to keep the project buildable while converting bootstrap and shared infrastructure first, then migrate forms and controls in priority order until WPF-specific code can be removed completely.
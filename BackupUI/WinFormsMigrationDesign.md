# BackupUI WinForms Migration Design

## Goal
Replace the WPF application surface in `BackupUI` with WinForms while preserving behavior and keeping the repository buildable during the migration.

## Core Strategy
Use an in-place staged migration:
1. Keep `BackupUI` as the single UI project.
2. Introduce WinForms startup and WinForms base infrastructure first.
3. Replace the main shell with a WinForms form.
4. Convert dialogs and complex workflow forms in priority order.
5. Remove WPF assets and `UseWPF` only after no WPF dependencies remain.

This keeps the project compiling during intermediate phases instead of attempting a single cutover.

## Application Structure
### Startup
- Replace WPF `App.xaml` startup with `Program.cs` using WinForms application startup.
- Preserve these behaviors from `App.xaml.cs`:
  - single-instance mutex
  - elevate to administrator when needed
  - backup engine DLL presence validation
  - splash-screen startup sequence
  - startup exception handling

### Main Shell
- Replace `MainWindow.xaml` with a WinForms form-based shell.
- Keep the class name `MainWindow` if possible to minimize code churn.
- Use WinForms controls that map closely to the current shell:
  - `MenuStrip`
  - `StatusStrip`
  - `TabControl`
  - `FlowLayoutPanel` / `Panel` / `TableLayoutPanel`
  - `DataGridView`
  - `ListView` or `Panel`-based card rendering where WPF item templates are currently used
  - `System.Windows.Forms.Timer` for UI polling

### Dialogs and Child Windows
- Convert each WPF window to a WinForms `Form`.
- Preserve modal behavior with `ShowDialog(IWin32Window)`.
- Preserve owned/modeless behavior with `Owner` and tracked open-form collections.

### Shared UI Infrastructure
- Update `WindowPositionManager` to use WinForms `Form`.
- Update `CustomDialogService` to use WinForms forms and `MessageBox` fallback.
- Update `NotificationService` to use WinForms marshaling and form activation.
- Remove the WPF visibility converter and handle control visibility directly in code.
- Replace WPF resource-dictionary styling with reusable WinForms helper methods and shared color/font constants.

### Custom Controls
- Rebuild `VolumeResizeControl` as a WinForms `UserControl`.
- Render bars, arrows, and free space in `OnPaint`.
- Implement resize handles via mouse hit-testing and drag state rather than WPF `Thumb`.

## Intermediate Buildability Rules
- Keep `UseWPF` enabled temporarily until all WPF files are replaced.
- Add WinForms entry and infrastructure first.
- Convert files in slices large enough to avoid split ownership of the same workflow.
- Remove XAML and WPF references only after the last dependent code is migrated.

## Priority Order
1. Bootstrap and startup (`App.xaml`, `App.xaml.cs`, `Program.cs`)
2. Shared infrastructure (`CustomDialogService`, `WindowPositionManager`, `NotificationService`)
3. Main shell (`MainWindow`)
4. Splash and custom dialog forms
5. Backup / restore workflow forms
6. Volume configuration and custom resize control
7. Remaining utility dialogs
8. WPF cleanup and package/resource removal

## Compatibility Decisions
- Reuse existing namespaces and public class names where practical to reduce downstream edits.
- Reuse business logic and service classes unchanged unless they directly depend on WPF types.
- Prefer hand-authored WinForms partial classes for early migration rather than waiting on designer-generated files.

## Immediate Execution Target
The next executable step is to convert bootstrap and configuration so the project can start from WinForms while still allowing temporary coexistence with remaining WPF screens during the migration.
# BackupWindowNew WinForms Conversion Boundary

## Goal
Replace the `BackupWindowNew` entry flow with a native WinForms form without regressing advanced backup functionality during the transition.

## Observed Complexity
`BackupWindowNew` combines several large concerns in one WPF window:
- source tree discovery and lazy loading for disks, volumes, folders, files, network paths, Hyper-V systems, guest disks, and guest volumes
- backup-type switching across full, incremental, differential, selected files/folders, clone-to-disk, clone-to-virtual-disk, clone Hyper-V, and export Hyper-V
- encryption UI and password persistence rules
- scheduling UI and validation
- exclusions management
- clone/Hyper-V rename and retention options
- immediate run (`StartBackup_Click`) and saved-job creation (`SaveJob_Click`)

## Safe First Native Boundary
The first native WinForms boundary should be:
1. Native WinForms shell/form for the New/Edit Backup entry point.
2. Native common settings surface:
   - backup name
   - backup destination
   - backup type selection for standard backup modes
   - verify/compress/encrypt toggles
   - schedule controls
3. Explicit fallback path for advanced source/clone/Hyper-V editing while native parity is incomplete.

## Preserve-Functionality Rule
Until native source-tree parity exists, any flow that depends on:
- disk/volume source-tree selection
- Hyper-V system or guest selection
- clone/export destination logic
- advanced selected-files replay behavior
should remain reachable through the existing WPF editor.

## Immediate Implementation Direction
Create a native WinForms `BackupWindowNewForm` that acts as the new entry shell and clearly exposes:
- standard/native settings
- a button to open the full legacy WPF editor for advanced source selection and unsupported backup modes

This avoids a functionality regression while replacing the user entry point with WinForms and gives a place to migrate sections incrementally.

## Follow-on Native Migration Order
1. native form shell and settings
2. exclusions/encryption/schedule sections
3. source-tree model and WinForms tree rendering
4. standard disk/volume/file selection
5. Hyper-V and clone flows
6. direct save/start execution from native form
7. remove WPF `BackupWindowNew` bridge usage
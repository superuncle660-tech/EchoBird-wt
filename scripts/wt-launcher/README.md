# Windows Terminal launcher

EchoBird launches CLI tools through `startCommand` when that field is present.
On Windows, using a `.bat` wrapper causes `cmd.exe` to flash briefly before
Windows Terminal opens.

This launcher is compiled as a Windows GUI executable (`/target:winexe`), so it
does not create its own console window. It opens `wt.exe` directly and starts
the matching CLI inside a new Windows Terminal tab:

- `echobird-wt-claudecode.exe` runs `claude`
- `echobird-wt-codex.exe` runs `codex`

The public repository does not contain the private `echobird_core` process
manager, so the corresponding `tools/*/paths.json` files intentionally remove
`startCommand` and put these GUI launchers first in the Windows path list. When
the launchers exist in `%USERPROFILE%/.echobird/wrappers`, EchoBird treats them
as the detected executable path instead of running a command string through
`cmd.exe`.

Build on Windows:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/wt-launcher/build-wt-launchers.ps1
```

# Project Handoff

## Overview

This repository contains a WinForms line-scan camera capture application targeting `.NET Framework 4.7.2` and `x64`.
The project is designed so the WinForms UI remains editable by a person in the Designer, while camera control, settings persistence, and image viewing logic are separated from `MainForm`.

Repository: `https://github.com/falcomu-lang/my-app-2`

## Current State

- Branch: `main`
- Solution: `CameraCaptureApp.sln`
- Main project: `CameraCaptureApp/CameraCaptureApp.csproj`
- Target framework: `.NET Framework 4.7.2`
- Target platform: `x64`
- Current handoff date: `2026-08-06`
- Latest pushed commit before this handoff update: `13111e1 Avoid locked Sapera parameter writes`

Latest verified local build command:

```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64\MSBuild.exe" ".\CameraCaptureApp.sln" /t:Build /p:Configuration=Debug
```

Latest local build result: `0 warning / 0 error`

## What Is Ready

- Main WinForms application layout for a `1280 x 720` program window.
- `MainForm` hosts the viewer at runtime to avoid Designer load issues.
- `CameraSettingsForm` remains Designer-editable.
- `settings.ini` persistence is implemented and generated beside the built executable.
- Sapera connection flow is present through:
  - `SapAcquisition`
  - `SapBufferWithTrash`
  - `SapAcqToBuf`
  - `SignalNotify`
  - `XferNotify`
- Manual connection, disconnection, preview, stop, and snap capture actions exist.
- Preview frames are throttled to about `5 Hz` to keep the UI responsive.
- Large image loading and viewing supports:
  - very large image dimensions
  - async loading
  - zoom and pan
  - reduced flicker
  - tiled / pyramid-style rendering work from earlier iterations
- Snapshot save on manual capture has been added.

## Current Camera Parameter Status

The current stable behavior prioritizes avoiding Sapera error popups and keeping connection/preview stable.

- `Exposure Time`: acquisition-side writes have previously shown successful readback, but current stable flow applies acquisition parameters during reconnect rather than while already connected.
- `Internal Line Rate`: acquisition-side writes have previously shown successful readback for:
  - `INT_LINE_TRIGGER_ENABLE`
  - `INT_LINE_TRIGGER_FREQ`
- `Length`: some acquisition length paths work depending on camera / CCF state, but `Acquisition length parameter not supported` may still appear for unsupported Sapera parameters.
- `Gain`: not currently working through this application.
  - The attempted `SapAcqDevice` path is not available for the current acquisition connection.
  - Sapera reported errors such as `CorAcqDeviceGetHandle not implemented()` when probing that path.
  - The current stable version disables the risky `SapAcqDevice` probing to avoid error popups.

## Important Sapera Notes

- Parameters can become locked after buffer / transfer objects are created.
- To avoid `CorAcqSetPrmEx parameters locked()`, acquisition parameters are currently applied early in the connection flow.
- Already-connected `Apply` currently saves settings and expects reconnect for hardware application.
- The current Sapera DLL reference still points to a local SDK/demo DLL path, so another computer must have a compatible Sapera SDK/runtime setup.

## Known Issues

- `Gain` cannot currently be written from the app.
- `Exposure Time` and `Internal Line Rate` need a cleaner, confirmed user workflow for when values are written:
  - before connect
  - during reconnect
  - while preview is stopped
- Online parameter changes are intentionally conservative right now to avoid locked-parameter errors.
- Some Sapera official dialogs may still show SDK-level message boxes if invalid paths are probed.
- No automated tests.
- No installer or deployment package.

## Next Version Goals

The next version should focus on reliable camera parameter control:

1. Make `Exposure Time` controllable from the app with a clear apply timing.
2. Make `Gain` controllable from the app by finding the same parameter path used by the official camera settings tool.
3. Make `Internal Line Rate` controllable and verify that it affects line-scan acquisition behavior.

Recommended approach for the next version:

- Do not re-enable broad `SapAcqDevice` probing until the exact official feature path is known.
- Investigate how the official Sapera / camera configuration window obtains the writable `Gain` handle.
- Add readback display for every parameter write that remains enabled.
- Consider a two-mode apply workflow:
  - `Save Only`
  - `Apply On Reconnect`
- Keep online writes disabled or guarded until the lock behavior is fully mapped.

## Notes For The Next Person

- If WinForms Designer fails, check whether `CameraDisplayControl` was inserted directly into Designer files.
- Avoid writing Sapera acquisition parameters after transfer/buffer creation unless the transfer is safely stopped and the parameter is confirmed writable.
- Avoid calling `SapAcqDevice` creation paths blindly; some paths trigger official Sapera message boxes.
- Keep the Camera Settings UI clean. Temporary diagnostic fields should not remain in the layout if they block normal use.

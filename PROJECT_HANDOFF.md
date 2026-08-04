# Project Handoff

## Overview

This repository contains a WinForms-based line-scan camera application scaffold targeting `.NET Framework 4.7.2`.
The current codebase includes:

- a Designer-friendly main dashboard
- a runtime-mounted large-image viewer control
- a camera settings dialog
- a Sapera SDK integration baseline for connect / grab / freeze / snap

The project is intended to become a practical line-scan acquisition tool with large-image viewing and future long-image accumulation support.

## Current State

- Repository branch: `main`
- Main solution: `CameraCaptureApp.sln`
- Main project: `CameraCaptureApp/CameraCaptureApp.csproj`
- Target framework: `.NET Framework 4.7.2`
- Target platform: `x64`
- Current date context: `2026-08-04`
- Latest verified local build command:

```powershell
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe' CameraCaptureApp.sln /t:Build /p:Configuration=Debug
```

## What Is Ready

- Main WinForms application structure under `CameraCaptureApp/`
- `MainForm` split from services and models
- `CameraDisplayControl` with:
  - async image loading
  - zoom with mouse wheel
  - drag-to-pan
  - large-image viewing behavior where zoom can exceed the initial fit rectangle
  - double-buffered render surface to reduce flicker
- `CameraSettingsForm` for editable camera settings
- `CameraService` baseline Sapera integration using:
  - `SapAcquisition`
  - `SapBufferWithTrash`
  - `SapAcqToBuf`
  - `SignalNotify`
  - `XferNotify`
- Sapera acquisition dialog files copied locally under `CameraCaptureApp/Sapera/`
- README updated to reflect the current project direction

## Important Implementation Notes

- The custom viewer is no longer placed directly in the WinForms Designer tree for `MainForm`.
  - To avoid Designer failures, `MainForm` now hosts a plain `Panel` in Designer and creates `CameraDisplayControl` at runtime.
- Sapera SDK is currently referenced from a local DLL path:
  - `C:\Users\falcomu\Documents\Codex\程式撰寫 專案資料夾\攝影機影像擷取\原廠攝影機取像程式\GrabDemo\CSharp\bin\Debug\DALSA.SaperaLT.SapClassBasic.dll`
- This means GitHub alone is not yet sufficient for a clean build on another machine unless that dependency is also installed or relocated into the repository in a compliant way.

## What Is Not Finished

- Live image/frame data is not yet pushed from Sapera buffers into `CameraDisplayControl`
- The line-scan accumulation pipeline is not implemented yet
- No persistent settings file storage yet
- No save-image/export workflow yet
- Several UI strings still need cleanup from prior encoding damage in older edits
- No tests
- No installer or deployment packaging

## Recommended Next Steps

1. Replace the current local Sapera DLL dependency with a more portable setup if redistribution is allowed.
2. Add a dedicated adapter layer between `CameraService` and raw Sapera objects.
3. Implement frame extraction or preview bridging from `SapBuffer` into the custom viewer.
4. Design the real line-scan accumulation model:
   - line buffer
   - long-image growth
   - update throttling
   - memory strategy
5. Add persistent settings storage to disk.
6. Clean remaining UI labels and strings.

## Notes For The Next Person

- If WinForms Designer reports it cannot load `CameraDisplayControl`, check whether someone reintroduced the control directly into the Designer file.
- The large-image viewer currently behaves like a real zoom/pan viewer rather than always fitting the image inside the original frame.
- The build has been verified locally on `2026-08-04`, but portability to another machine is still blocked by the Sapera dependency path.

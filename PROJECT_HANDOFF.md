# Project Handoff

## Overview

This repository contains a WinForms-based line-scan camera application scaffold targeting `.NET Framework 4.7.2`.
The code is organized so the UI remains editable with the WinForms Designer while camera logic, settings logic, and viewer behavior stay separated from `MainForm`.

The project is now beyond a pure UI scaffold:

- the main dashboard is in place
- the custom large-image viewer is working
- Sapera SDK objects can connect / grab / snap
- preview frames are now pushed from `CameraService` into the viewer
- camera settings now persist to `settings.ini`

## Current State

- Repository branch: `main`
- Main solution: `CameraCaptureApp.sln`
- Main project: `CameraCaptureApp/CameraCaptureApp.csproj`
- Target framework: `.NET Framework 4.7.2`
- Target platform: `x64`
- Current date context: `2026-08-04`
- Latest pushed commit before this handoff update: `2b56f1f Add Sapera preview pipeline and ini settings persistence`
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
- `CameraService` Sapera integration using:
  - `SapAcquisition`
  - `SapBufferWithTrash`
  - `SapAcqToBuf`
  - `SignalNotify`
  - `XferNotify`
- Preview event pipeline:
  - `CameraService` raises `FrameReady`
  - `MainForm` receives preview frames without blocking the UI thread
  - preview updates are throttled to about `5 Hz`
- Persistent settings storage:
  - stored as `settings.ini`
  - loaded on startup
  - saved when the settings dialog is confirmed
- Sapera acquisition dialog files copied locally under `CameraCaptureApp/Sapera/`

## Important Implementation Notes

- The custom viewer is not placed directly in the WinForms Designer tree for `MainForm`.
  - To avoid Designer failures, `MainForm` hosts a plain `Panel` in Designer and creates `CameraDisplayControl` at runtime.
- Sapera SDK is currently referenced from a local DLL path:
  - `..\..\原廠攝影機取像程式\GrabDemo\CSharp\bin\Debug\DALSA.SaperaLT.SapClassBasic.dll`
- This means GitHub alone is still not enough for a clean build on another machine unless Sapera is installed there too, or the dependency strategy is changed.
- `settings.ini` is written beside the built executable.
  - In a local Debug run, this means `CameraCaptureApp\bin\Debug\settings.ini`.
- The current Sapera preview conversion path reads buffer data into a managed byte array and converts it into a `Bitmap`.
  - It currently assumes common `Mono8` or `24bpp` style preview output.
  - This is a first working bridge, not the final line-scan-specific preview design.

## What Is Not Finished

- The true line-scan accumulation pipeline is not implemented yet
- Very large continuous image stitching / append strategy is not implemented yet
- No save-image/export workflow yet
- The current preview bridge may still need adjustment for the actual camera pixel format or pitch behavior
- Some older UI text or source strings may still contain legacy encoding damage, even if not all of it is visible in the running UI
- No tests
- No installer or deployment packaging

## Recommended Next Steps

1. Replace the current local Sapera DLL dependency with a more portable setup if redistribution is allowed.
2. Validate the preview conversion against the actual line-scan camera output format.
3. Design the real line-scan accumulation model:
   - line buffer
   - long-image growth
   - update throttling
   - memory strategy
4. Add save and export flow for captured images.
5. Clean any remaining UI labels and source strings that came from earlier encoding-damaged edits.
6. Decide whether `settings.ini` should be checked into source as a sample template or stay runtime-generated only.

## Notes For The Next Person

- If WinForms Designer reports it cannot load `CameraDisplayControl`, check whether someone reintroduced the control directly into the Designer file.
- The large-image viewer behaves like a real zoom/pan viewer rather than always fitting the image inside the original frame.
- The project has been verified to build locally on `2026-08-04`.
- Portability to another machine is still mainly blocked by the Sapera dependency path and target machine SDK availability.

# Project Handoff

## Overview

This repository contains a WinForms line-scan camera capture application targeting `.NET Framework 4.7.2` and `x64`.
The project uses Teledyne DALSA Sapera LT / SapClassBasic APIs for Camera Link acquisition and camera feature control.

Repository: `https://github.com/falcomu-lang/my-app-2`

## Current State

- Branch: `main`
- Solution: `CameraCaptureApp.sln`
- Main project: `CameraCaptureApp/CameraCaptureApp.csproj`
- Target framework: `.NET Framework 4.7.2`
- Target platform: `x64`
- Current handoff date: `2026-08-07`
- Latest pushed commit before this handoff update: `838ec0d Restore exposure feature candidates`

Latest verified local build command:

```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64\MSBuild.exe" ".\CameraCaptureApp.sln" /t:Build /p:Configuration=Debug
```

Latest local build result: `0 warning / 0 error`

## What Is Working Now

- Camera connection, disconnection, preview, stop, and snap capture.
- WinForms Designer-editable `CameraSettingsForm`.
- Runtime-hosted camera viewer in `MainForm`.
- `settings.ini` persistence beside the built executable.
- Large image viewing support with zoom / pan and reduced UI flicker.
- Camera parameters currently confirmed by user as working in this version:
  - `Exposure Time`
  - `Gain`
  - `Length`
- Current confirmed user feedback:
  - Exposure, gain, and length can be operated normally in the current version.
  - The app still contains diagnostic / probing code that was added while finding the correct Sapera feature path.

## Parameter Apply Workflow

- Pressing `Apply` in the settings window saves the requested values locally.
- Hardware parameter writes are intentionally performed on the next offline reconnect path, not immediately while live/connected.
- Expected workflow:
  1. Disconnect / stay offline.
  2. Edit settings.
  3. Press `Apply`.
  4. Connect again.
- This workflow is deliberate because some Sapera acquisition parameters become locked after buffers/transfers are created.

## Current Parameter Implementation Notes

### Exposure

- The currently useful exposure path is the camera-side `SapAcqDevice` feature write path.
- The app tries several exposure-related feature names to discover the one the installed camera accepts.
- This probing is still present because the exact final production feature name has not been fully narrowed down.
- The app also still writes/readbacks acquisition-side line integrate diagnostics:
  - `LINE_INTEGRATE_METHOD`
  - `LINE_INTEGRATE_ENABLE`
  - `LINE_INTEGRATE_DURATION`
- These acquisition-side values may read back successfully but are not necessarily the parameter that changes actual brightness for this camera.

### Gain

- `Gain` is currently working through the camera-side `SapAcqDevice.SetFeatureValue("Gain", valueString)` path.
- The value is sent as a string, matching the user's reference notebook snippet.

### Length

- Length is currently working for the user's setup.
- It is still routed through Sapera acquisition parameter logic where supported by the loaded CCF / camera path.

### Internal Line Rate

- Internal line rate is not yet confirmed working.
- The next version should focus on this feature.
- Existing acquisition-side attempts include:
  - `INT_LINE_TRIGGER_ENABLE`
  - `INT_LINE_TRIGGER_FREQ`
- These may write/read back but have not yet been confirmed to affect line-scan behavior.

## Diagnostic Code Still Present

Some diagnostic / probing code remains intentionally. It was added while finding the correct camera parameter path and should be cleaned up once final feature names are known.

Important diagnostic outputs:

- `CameraCaptureApp\bin\Debug\logs\last_requested_settings.txt`
  - Written when `ApplySettings()` receives UI settings.
  - Shows requested exposure/gain/length/internal line rate before hardware writes.
- `CameraCaptureApp\bin\Debug\logs\last_apply_params.txt`
  - Written when parameters are applied during connect/reconnect.
  - Shows Sapera write attempts, readbacks, and notebook feature results.
- `live_features_*.txt`
  - Exported by `Live Features`.
  - Used to inspect camera-side feature names and access modes.
- `live_features_failed_*.txt`
  - Exported when `SapAcqDevice` live feature probing is unavailable.

Keep these diagnostics for now because they are still useful for mapping the camera-specific line rate parameter.

## Important Sapera Notes

- The camera is a line-scan camera.
- Sapera acquisition parameters can become locked after buffer / transfer creation.
- Avoid writing acquisition parameters late in the live preview path unless transfer is safely stopped and the parameter is confirmed writable.
- The working camera-side path uses `SapAcqDevice` bound to the selected or auto-selected AcqDevice location.
- If no user-selected DeviceFeature path exists and only one AcqDevice is found, the app auto-selects that unique path.
- The Sapera DLL reference still points to a local SDK/demo DLL path, so another computer needs a compatible Sapera SDK/runtime setup.

## Known Issues

- `Internal Line Rate` is not yet functional.
- Some exploratory exposure feature writes are still present and should be trimmed once the exact working feature is confirmed from `last_apply_params.txt`.
- No automated tests.
- No installer or deployment package.
- The downloaded PDF `20161221032935911.pdf` is currently untracked and should not be committed unless intentionally needed.

## Next Version Goals

1. Add reliable `Internal Line Rate` control.
2. Identify the exact Sapera feature or acquisition parameter that controls line rate for this line-scan setup.
3. Reduce exposure probing code after the final working exposure feature name is confirmed.
4. Keep exposure, gain, and length behavior stable while adding line rate.

Recommended next-version approach:

- First compare `last_apply_params.txt` and `live_features_*.txt` before/after changing line rate in the official tool.
- Look for feature names containing:
  - `LineRate`
  - `AcquisitionLineRate`
  - `DeviceLineRate`
  - `LinePeriod`
  - `AcquisitionLinePeriod`
  - `Encoder`
  - `Trigger`
- Prefer a single confirmed feature write over broad candidate probing.
- Preserve the current reconnect-based apply timing unless live write safety is proven.

## Notes For The Next Person

- Do not remove the diagnostic reports until internal line rate is working.
- Avoid broad `SapAcqDevice` probing that creates SDK message boxes.
- Keep `CameraSettingsForm` Designer-editable.
- Treat exposure/gain/length as currently stable behavior; test them after any line rate changes.

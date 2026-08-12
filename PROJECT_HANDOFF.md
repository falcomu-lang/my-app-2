# Project Handoff

## Overview

This repository contains a WinForms line-scan camera capture application targeting `.NET Framework 4.7.2` and `x64`.
The project uses Teledyne DALSA Sapera LT / SapClassBasic APIs for Camera Link acquisition and attached-camera feature control.

Repository: `https://github.com/falcomu-lang/my-app-2`

## Current State

- Branch: `main`
- Solution: `CameraCaptureApp.sln`
- Main project: `CameraCaptureApp/CameraCaptureApp.csproj`
- Target framework: `.NET Framework 4.7.2`
- Target platform: `x64`
- Current handoff date: `2026-08-12`
- Latest pushed commit at this handoff update: `6235311 Add uncompressed TIFF save option`

Latest verified local build command:

```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64\MSBuild.exe" ".\CameraCaptureApp.sln" /t:Build /p:Configuration=Debug
```

Latest local build result: `0 warning / 0 error`

## What Is Working Now

- Camera connection and disconnection.
- Preview start, preview stop, and capture.
- WinForms Designer-editable `CameraSettingsForm`.
- Runtime-hosted camera viewer in `MainForm`.
- `settings.ini` persistence beside the built executable.
- Large image viewing support with zoom / pan and reduced UI flicker.
- Main command button enable/disable state follows connection and preview state.
- Camera apply result popup has been removed; apply result remains in the settings form status label.
- Live preview is downscaled before display to reduce UI stalls.
- Capture data and UI preview are now partially separated:
  - `FrameRecorder` stores the latest full-resolution frame.
  - UI preview may drop older frames and use downscaled preview frames.
  - Capture saves from `FrameRecorder`, not from the preview bitmap.
- Manual snapshot saving is available from the viewer toolbar when allowed by the current acquisition state.
- Manual snapshots save under `Application.StartupPath\snapshot`.
- Capture-triggered image saves still use the configured capture folder/pattern fallback path.
- Saved image format is configurable in `Camera Settings -> Saving`:
  - `PNG`
  - `TIF`
  - `TIF (uncompressed)`
- `BMP` was removed because very large 8-bit BMP files were slow/unreliable to open in common viewers.
- Snapshot saving now writes through WIC grayscale encoding and uses a temporary `.tmp` file before moving to the final file name.
- Snapshot save jobs are queued through one shared progress window.
- Up to 5 snapshot save jobs may run in parallel; additional jobs remain queued.
- The progress window reports active, waiting, done, and failed job counts.
- Camera parameters confirmed by user as working in this version:
  - `Exposure Time`
  - `Gain`
  - `Length`
  - `Internal Line Rate`

## Rolling Capture

- Rolling capture is implemented from frame-based acquisition output.
- Settings are in `Camera Settings -> Image` under the rolling capture group.
- Persisted rolling settings:
  - `RollingCaptureEnabled`
  - `RollingCaptureFrameCount`
  - `RollingCaptureDirection`
- Rolling frame count maximum is currently `100`.
- Rolling direction choices:
  - Top-to-bottom: newest frame is inserted at the top and older frames move down.
  - Bottom-to-top: newest frame is inserted at the bottom and older frames move up.
- Rolling preview supports zoom/pan after acquisition stops and can be saved as the full rolling image.
- Rolling save uses the same selected image save format as single-frame snapshots.
- The save operation snapshots the current rolling frames before background processing so later incoming frames do not mutate the save job.

## Parameter Apply Workflow

- Pressing `Apply` in the settings window saves requested values locally.
- Hardware parameter writes are performed on the next offline reconnect path, not immediately while live/connected.
- Expected workflow:
  1. Disconnect / stay offline.
  2. Edit settings.
  3. Press `Apply`.
  4. Connect again.
- This workflow remains deliberate because Sapera acquisition parameters and attached-camera features can become locked after acquisition/buffer/transfer objects are created.

## Current Parameter Implementation Notes

### Internal Line Rate

- Internal line rate is now confirmed working.
- The effective CamExpert field is:
  - Category/location: attached camera -> Camera control
  - Feature display name: `Internal Line Rate`
  - Feature name: `AcquisitionLineRate`
  - Type: `IInteger` / `SapFeature::TypeInt64`
  - Description: camera line rate in Hz
- The working application path is early attached-camera feature write:
  - Create `SapAcqDevice` before `SapAcquisition.Create()`.
  - Write `AcquisitionLineRate` as `Int64`.
  - Call `UpdateFeaturesToDevice()`.
- Acquisition-side line trigger parameters are still set/read back as supporting trigger configuration:
  - `INT_LINE_TRIGGER_ENABLE`
  - `INT_LINE_TRIGGER_FREQ`
  - `EXT_LINE_TRIGGER_ENABLE = 0`
  - `SHAFT_ENCODER_ENABLE = 0`
- Important: `INT_LINE_TRIGGER_FREQ` can read back correctly but was not the feature that made the camera line rate change by itself. The key working feature is attached-camera `AcquisitionLineRate`.

### Exposure

- Exposure is currently working after line rate setup.
- The app writes camera-side exposure features through `SapAcqDevice`, after early line-rate setup, so line rate setup does not override exposure behavior.
- Avoid configuring `ExposureStart` trigger source for internal line rate; doing so previously interfered with exposure behavior.

### Gain

- `Gain` is currently working through camera-side `SapAcqDevice.SetFeatureValue("Gain", valueString)`.
- The value is sent as a string, matching the user's reference notebook snippet.

### Length

- Length is currently working for the user's setup.
- It is routed through Sapera acquisition parameter logic using `SapAcquisition.Prm.CROP_HEIGHT` where supported by the loaded CCF / camera path.

## Diagnostics Still Present

Important diagnostic outputs:

- `CameraCaptureApp\bin\Debug\logs\last_requested_settings.txt`
  - Written when `ApplySettings()` receives UI settings.
  - Shows requested exposure/gain/length/internal line rate before hardware writes.
- `CameraCaptureApp\bin\Debug\logs\last_apply_params.txt`
  - Written when parameters are applied during connect/reconnect.
  - Shows Sapera write attempts, readbacks, and attached-camera feature results.
- `live_features_*.txt`
  - Exported by `Live Features`.
  - Used to inspect attached-camera feature names and access modes.
- `live_features_failed_*.txt`
  - Exported when `SapAcqDevice` live feature probing is unavailable.

Diagnostic/probing cleanup already performed:

- Removed CCF internal-line-rate text rewriting.
- Removed unused line-rate feature candidate probing such as `LineRateAbs`, `LineRateRaw`, `DeviceLineRate`, line-period, and timer fallbacks.
- Removed late AcqDevice line-rate retry path. The intended path is now the early `AcquisitionLineRate(Int64)` write.

Keep the remaining diagnostics for now. They are low-friction and useful if the Sapera target machine or attached-camera path changes.

## Preview And Capture Architecture

- Current preview event is still `SapXferPair.XferEventType.EndOfFrame`.
- Current behavior is frame-based:
  - Sapera produces a complete frame.
  - The full frame is copied into `FrameRecorder`.
  - A downscaled frame is used for UI display.
  - UI preview may drop old frames to stay responsive.
  - Capture saves the latest full frame held by `FrameRecorder`.
- Optional rolling capture builds a bounded vertical composite from recent full frames, but it is still frame-based.
- Live preview resolution text displays the source frame size, not the downscaled preview bitmap size.

Important limitation:

- If the desired future behavior is a complete continuous long-image scan, the next architectural step is a real recorder queue/chunk writer:
  - Sapera frame/chunk callback -> recorder queue -> append to long image buffer/file.
  - UI preview remains independent and can drop frames.
  - Save/export uses recorder output, not preview output.

## Important Sapera Notes

- The camera is a line-scan camera.
- Some Sapera acquisition parameters and camera features become locked after acquisition/buffer/transfer creation.
- The working attached-camera path uses `SapAcqDevice` bound to the selected or auto-selected AcqDevice location.
- If no user-selected DeviceFeature path exists and exactly one AcqDevice is found, the app auto-selects that unique path.
- If line rate ever stops working on another machine, first verify that `DeviceFeatureServerName` / `DeviceFeatureResourceIndex` point to the same attached-camera path shown by CamExpert.
- The Sapera DLL reference still points to a local SDK/demo DLL path, so another computer needs a compatible Sapera SDK/runtime setup.

## Known Issues / Current Limits

- No automated tests.
- No installer or deployment package.
- Preview responsiveness has improved, but true in-progress line-scan display would require chunk/line-based acquisition events instead of only `EndOfFrame`.
- `FrameRecorder` stores the latest full frame and, when rolling capture is enabled, a bounded rolling frame list. It does not append all frames/chunks into an unlimited continuous long image.
- Very large image saves are still expensive. Example user case: `16384 x 50000` 8-bit image is about 819 MB raw data and may encode to roughly 400 MB depending on content/format.
- PNG/TIF save time may be dominated by encoder CPU work, not just disk throughput.
- `TIF (uncompressed)` exists as a speed-oriented option, trading larger files for less compression work.
- Parallel save limit is currently hard-coded in `MainForm` as `MaxConcurrentSnapshotSaves = 5`.
- The downloaded PDF `20161221032935911.pdf` is currently untracked and should not be committed unless intentionally needed.

## Recommended Next Steps

1. Test the latest `main` on the remote camera machine after this handoff update.
2. Compare `PNG`, `TIF`, and `TIF (uncompressed)` save time on the real camera machine for the target image size.
3. If save speed remains too slow, profile time spent in:
   - copying frame bytes into the grayscale buffer,
   - WIC encoding,
   - final disk write / antivirus / sync folder overhead.
4. If many huge images are saved at once, test whether `MaxConcurrentSnapshotSaves = 5` is actually optimal. Large uncompressed/TIFF writes may perform better at 2 or 3 concurrent jobs on some disks.
5. If preview still feels delayed, determine whether delay comes from waiting for `EndOfFrame`:
   - Large `Length` values naturally delay UI updates because the app waits for a full frame.
   - Consider `EndOfNLines` or other Sapera line/chunk callbacks if supported.
6. If full continuous long-image capture is required, implement a recorder queue:
   - Background worker owns the full-resolution data path.
   - UI preview remains downscaled and droppable.
   - Capture/export reads from recorder output.
7. Keep exposure/gain/length/internal line rate behavior stable when changing preview or recorder architecture.

## Notes For The Next Person

- Do not reintroduce broad line-rate probing. The confirmed line-rate feature is attached-camera `AcquisitionLineRate` written as `Int64`.
- Do not rely on CCF file edits for internal line rate; CamExpert changed runtime/attached-camera state without updating CCF.
- Do not set `ExposureStart` trigger source while configuring internal line rate; this previously broke exposure behavior.
- Keep `CameraSettingsForm` Designer-editable.
- Treat exposure/gain/length/internal line rate as currently stable behavior and test all four after camera pipeline changes.
- Preserve the separation between UI preview and full-resolution save data. The save path should continue using `FrameRecorder` snapshots, not the downscaled display bitmap.
- For save pipeline changes, keep the temporary `.tmp` write then final move behavior to avoid users opening incomplete output files.

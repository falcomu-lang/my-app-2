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
- Current handoff date: `2026-08-27`
- Latest pushed feature commit referenced by this handoff: `62725b4 Fix waveform selection at high zoom`
- Latest verified local changes not yet pushed:
  - Shift-assisted gray waveform line snapping.
  - Progressive large-image preview refinement while loading.

Latest verified local build command:

```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64\MSBuild.exe" ".\CameraCaptureApp.sln" /p:Configuration=Debug /p:Platform="Any CPU" /p:GenerateResourceMSBuildArchitecture=CurrentArchitecture /m:1
```

Latest local build result: `0 warning / 0 error`

## What Is Working Now

- Camera connection and disconnection.
- Preview start, preview stop, and capture.
- WinForms Designer-editable `CameraSettingsForm`.
- Runtime-hosted camera viewer in `MainForm`.
- `settings.ini` persistence beside the built executable.
- Large image viewing support with zoom / pan and reduced UI flicker.
- Large image viewing supports tiled rendering for huge images such as the user's `16384 x 50000` case.
- Large-image pan behavior was tuned so dragging uses a lightweight preview layer, then high-resolution tiles are refreshed after mouse-up.
- Large-image tile rendering was adjusted to avoid visible seams between tiles at fractional zoom levels.
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
- Recent user-confirmed behavior:
  - Meter wheel parameters now persist across restart as expected.
  - External-trigger/meter-wheel automatic compare action currently behaves as expected.
  - Main image pan smoothness is now acceptable after the preview-during-drag change.
  - Gray waveform generation speed for large loaded images is now acceptable after batched tile sampling.
- Shutdown/connect-failure cleanup now guards Sapera cleanup calls so a Sapera DLL load failure does not bubble directly to the UI thread during cleanup.

## Gray Waveform Feature

- Main viewer toolbar now includes `取得灰階波形` to create a grayscale waveform from the currently displayed image.
- The feature can be used without connecting to the camera:
  - load an image from file,
  - press `取得灰階波形`,
  - draw a line directly on the main image view.
- It also works after stopping camera preview:
  - the app locks the current displayed frame for line selection,
  - new camera frames do not replace the selection image while selection is active.
- Selection behavior:
  - left-click drag on the main image defines the waveform line,
  - holding `Shift` while drawing snaps the line angle using the user's requested buckets:
    - `0-30` degrees snaps to horizontal,
    - `31-59` degrees snaps to `45` degrees,
    - `60-90` degrees snaps to vertical,
    - the same rule repeats in the other quadrants,
  - confirmation dialog shows start/end/length,
  - `Retry` restarts line selection,
  - cancel exits selection mode.
- For camera frames, waveform sampling uses the full-resolution frame stored in `FrameRecorder`, not the downscaled display preview.
- For loaded large images, waveform sampling uses `LargeImageSource` tile data so it can sample real source pixels without creating a huge full-size bitmap.
- Important implementation files:
  - `CameraCaptureApp/Controls/CameraDisplayControl.cs`
  - `CameraCaptureApp/Controls/LargeImageSource.cs`
  - `CameraCaptureApp/Forms/GrayScaleWaveformForm.cs`
  - `CameraCaptureApp/Forms/GrayScaleWaveformForm.Designer.cs`
  - `CameraCaptureApp/Forms/GrayWaveformConfirmForm.cs`
  - `CameraCaptureApp/Services/IGrayPixelSource.cs`
  - `CameraCaptureApp/Services/GrayWaveformSelectionEventArgs.cs`
- `IGrayPixelSource` exposes both:
  - `GetGrayAt(int x, int y)`
  - `GetGrayValues(Point[] points, int[] destination)`
- `GetGrayValues` was added for large-image performance. Do not regress the waveform form back to per-point `GetGrayAt` loops for large images.
- `LargeImageSource.GetGrayValues` groups line points by `1024 x 1024` tile, creates/loads each tile once, locks it once, and reads all points in that tile as a batch.
- Earlier user performance datapoint:
  - source image: `16384 x 50000`,
  - line length: `11078` points,
  - pre-batch time: about `34 seconds`,
  - post-batch speed was user-confirmed as much faster / acceptable.
- Waveform result window behavior:
  - initial Y range is fixed at `0-255`,
  - mouse wheel zooms in/out,
  - left mouse drag selects a zoom region,
  - right mouse button resets the view,
  - horizontal guide lines are drawn every `16` gray levels plus `255`,
  - the chart panel is double-buffered to reduce selection flicker,
  - the window is resizable and has the normal Windows maximize button,
  - the chart expands with the window and the close button stays anchored at bottom-right.

## Large Image Display Notes

- `LargeImageSource` is the main abstraction for huge loaded images.
- It builds preview levels at max dimensions:
  - `512`
  - `1024`
  - `2048`
  - `4096`
  - `8192`
- Large-image file loading now uses progressive preview refinement:
  - `LargeImageSource` creates only the fastest/smallest full-image preview during construction.
  - Larger preview levels are generated in the background after the image is displayed.
  - Each completed preview level schedules a viewer refresh.
  - This means the user can see a blurry full-image overview quickly, and the same overview becomes clearer as background preview levels complete.
  - Zooming into an area may initially show the available preview, then high-resolution tiles replace that area as tile decoding completes.
- It renders high-resolution detail as `1024 x 1024` tiles.
- Tile cache limit is currently `MaxTileCacheCount = 96`.
- Pan / zoom performance changes:
  - cached preview and tile bitmaps are reused for painting instead of cloned on every paint,
  - preview drawing clips to only the currently visible source rectangle,
  - while `_isPanning` is true, the viewer renders only the preview layer and skips tile drawing/scheduling,
  - mouse-up invalidates the viewer so high-resolution tiles are drawn/refreshed after dragging stops,
  - tile creation now happens outside the shared `_sync` lock, reducing UI stalls while background tile decode is active.
- Tile seam fix:
  - tile destination rectangles use `Floor` for left/top and `Ceiling` for right/bottom to prevent 1-pixel gaps at fractional zoom,
  - tile drawing uses `ImageAttributes.SetWrapMode(WrapMode.TileFlipXY)` to reduce interpolation artifacts at tile edges.
- User-reported zoom levels that previously felt less smooth:
  - `0.19x`
  - `0.23x`
  - `0.29x`
  - `0.36x`
  - `0.45x`
  - `0.57x`
  - `0.71x`
- Preserve the current panning strategy unless intentionally changing the interaction model; user said the latest smoothness is what they wanted.

### Trigger Page Updates

- A new checkbox has been added under `Camera Settings -> Trigger`:
  - `External Trigger One Frame`
- This option maps to Sapera acquisition parameter `CORACQ_PRM_EXT_FRAME_TRIGGER_ENABLE`.
- The option is intentionally independent from `Trigger Mode`:
  - It can be used with `Continuous`.
  - It can be used with `ExternalTrigger`.
- The app persists this setting in `settings.ini` as `ExternalFrameTriggerOneFrame`.
- Additional trigger-page options:
  - `Compare Set follows current encoder value`
  - `Also apply Encoder Set on external trigger`
- `External Trigger One Frame` remains available in both:
  - `Continuous`
  - `External Trigger`
- `Compare Set follows current encoder value` is available only when:
  - `Trigger Mode = External Trigger`
  - `External Trigger One Frame` is checked
- When `Trigger Mode = Continuous`, `Compare Set follows current encoder value` is automatically unchecked and disabled.
- `Also apply Encoder Set on external trigger` is available only when `Compare Set follows current encoder value` is checked.
- The app persists these settings in `settings.ini` as:
  - `ExternalFrameTriggerOneFrameCompareFromEncoder`
  - `ExternalFrameTriggerOneFrameSetEncoderOnTrigger`
- The `External Trigger` preview-arm check now considers both:
  - `EXT_LINE_TRIGGER_ENABLE`
  - `EXT_FRAME_TRIGGER_ENABLE`
- Important behavior note:
  - This checkbox only toggles the external frame trigger enable state.
  - It does not replace the existing line trigger mode or internal line rate behavior.
  - The automatic compare/encoder actions are app-side LSI-8181 operations triggered by Sapera external-trigger notification; they do not change Sapera trigger parameters by themselves.

## LSI-8181 Meter Wheel Status

- The attempted integration of the vendor's full LSI-8181 VB.NET test program has been removed from this repository.
- Removed from the current project:
  - `Lsi8181Official/`
  - the `LSI8181-64` VB project entry in `CameraCaptureApp.sln`
  - the C# project reference to `Lsi8181Official/LSI8181-64.vbproj`
  - copied LSI8181 vendor DLL content entries from `CameraCaptureApp/CameraCaptureApp.csproj`
  - `CameraCaptureApp/LSI8181.dll`
  - `CameraCaptureApp/LSI8181_64.dll`
  - `CameraCaptureApp/WindowsControlLibrary1.dll`
  - the `米輪控制` button and all startup/open/close code that referenced `LSI8181.Main_Form`
- New LSI-8181 support is now being rebuilt as a small purpose-built C# layer instead of embedding the vendor test program.
- Current implementation files:
  - `CameraCaptureApp/Native/Lsi8181Native.cs`
  - `CameraCaptureApp/Services/Lsi8181MeterWheelService.cs`
  - `CameraCaptureApp/Forms/MeterWheelControlForm.cs`
  - `CameraCaptureApp/Forms/MeterWheelControlForm.Designer.cs`
  - `CameraCaptureApp/Forms/MeterWheelControlForm.resx`
  - `CameraCaptureApp/Forms/MeterWheelExtensionCompareForm.cs`
  - `CameraCaptureApp/Forms/MeterWheelExtensionCompareForm.Designer.cs`
  - `CameraCaptureApp/Forms/MeterWheelExtensionCompareForm.resx`
- `MainForm` now has a `Meter Wheel` button.
- `MainForm` owns the active `Lsi8181MeterWheelService` instance.
- After `MainForm` is shown, the app waits `1 second` and attempts to auto-connect the meter wheel using persisted meter wheel settings.
  - The saved `MeterWheelCardId` selects which LSI-8181 card is connected during this startup auto-connect path.
- Meter wheel auto-connect failure is logged and shown in the footer message; it does not block startup with an error popup.
- The meter wheel control window is modeless:
  - The camera app remains usable while the meter wheel window is open.
  - Pressing `Meter Wheel` again focuses the existing window instead of opening duplicates.
  - Closing the meter wheel window does not disconnect the meter wheel card.
  - The meter wheel card disconnects only when the user presses `Disconnect` in the meter wheel window or when the main application closes.
- `MeterWheelControlForm` is Designer-editable. Keep UI layout edits in the Designer file so future maintainers can drag controls in Visual Studio.
- `MeterWheelExtensionCompareForm` is Designer-editable:
  - It has a public parameterless constructor for Visual Studio Designer.
  - The runtime constructor receives `Lsi8181MeterWheelService`, `CameraSettings`, and `ISettingsService`.
  - Do not add custom helper method calls inside `MeterWheelExtensionCompareForm.Designer.cs`; Visual Studio Designer previously failed to load when helper methods such as `ConfigureHeader` were used.
- Current meter wheel UI supports:
  - Card ID selection (`0` to `15`).
  - Connect / Disconnect.
  - Live encoder value refresh every `200 ms` while connected.
  - Encoder clear and custom encoder preset.
  - Compare value clear and custom compare value.
  - Auto-increment value input and `Apply`.
  - Encoder input multiple rate selection in vendor order: `X4`, `X2`, `X1`.
  - `Reverse Direction` software setting for reversing meter wheel count direction without rewiring.
  - `CMP Out Width` input and `Set`.
  - `Extension` button below `CMP Out Width`.
- The meter wheel form now protects settings while loading saved values into UI controls:
  - UI initialization no longer fires value-change handlers that overwrite saved settings with default values.
  - This fixed the observed issue where `Multiple Rate`, `Reverse Direction`, and `CMP Out Width` appeared to revert after restart.
- `Multiple Rate`, `Reverse Direction`, and `CMP Out Width` persist through `settings.ini`.
  - `Multiple Rate` saves when the combo-box selection changes or when `Set` is pressed.
  - `Reverse Direction` saves when the checkbox changes.
  - `CMP Out Width` saves when the numeric value changes or when `Set` is pressed.
- The extension compare window supports `CMP0` through `CMP7`:
  - Mask.
  - Offset Compare.
  - Pulse Width.
  - Output state.
  - Live Status refresh every `200 ms`.
- Extension compare behavior follows the vendor test program:
  - When a channel's mask is enabled, its manual output state checkbox is cleared and disabled.
  - `Apply` / `OK` writes all 8 extension channels to hardware.
  - `Apply` / `OK` also persists extension compare settings to `settings.ini`.
- Current persisted meter wheel settings in `settings.ini`:
  - `MeterWheelCardId`
  - `MeterWheelCompareIncrement`
  - `MeterWheelMultipleRate`
  - `MeterWheelReverseDirection`
  - `MeterWheelCmpOutWidth`
  - `MeterWheelExtensionCompareMask`
  - `MeterWheelExtensionCompareOffsets`
  - `MeterWheelExtensionComparePulseWidths`
  - `MeterWheelExtensionCompareOutputStates`
  - `MeterWheelEncoderValue`
  - `MeterWheelCompareValue`
- When opening the meter wheel window, the persisted values are loaded into the UI, including the saved Card ID selection.
- When pressing meter wheel `Apply` / `Set` for increment, multiple rate, or CMP out width, the values are saved back through `SettingsService`.
- Changing multiple rate or CMP out width in the UI also saves immediately.
- The encoder and compare custom numeric entry fields now persist as settings values, but they are not automatically pushed to hardware when the form opens.
- The `Set` buttons remain the only path that writes those entry values to the meter wheel card.
- Important compare/encoder behavior:
  - The Compare custom numeric value is user-defined.
  - Pressing Compare `Set` writes the current Compare numeric value to hardware and must not auto-replace the value from the live encoder.
  - Pressing Encoder `Set` writes the current Encoder numeric value to hardware.
- When connecting to the meter wheel card, the app applies persisted meter wheel settings immediately:
  - Encoder input mode is forced to quadrature mode.
  - Multiple rate uses the saved `X4` / `X2` / `X1` selection.
  - Count direction uses the saved `Reverse Direction` setting.
  - Compare mode is forced to auto increment.
  - Compare auto-increment value is applied.
  - CMP OUT is forced to pulse output through `LSI8181_compare_CMP_OUT_set`.
  - CMP OUT is forced enabled through `LSI8181_toggle_preset(CardID, 1)`.
  - Extension compare settings for `CMP0` through `CMP7` are applied from `settings.ini`.
  - Counter starts in compare output mode.
- `MeterWheelCardId` is saved when the user connects from the meter wheel control window:
  - The selected Card ID combo-box index is written to `_settings.MeterWheelCardId`.
  - `SettingsService.Save()` persists it to `settings.ini`.
  - The next application startup uses the saved value for the 1-second delayed auto-connect.
- `MeterWheelReverseDirection` is implemented in software by reading and writing the LSI-8181 CIO polarity register:
  - The app calls `LSI8181_CIO_polarity_read` and `LSI8181_CIO_polarity_set`.
  - It toggles A phase polarity bit `0` only, preserving the other CIO polarity bits.
  - Default `false` keeps the original wiring direction.
  - `true` reverses quadrature count direction so the previous negative direction becomes positive.
- External-trigger meter-wheel automation:
  - `CameraService` now exposes `ExternalTriggerReceived`.
  - It is raised when Sapera reports `ExternalTrigger` or `ExternalTrigger2`.
  - `MainForm` listens for this event.
  - If `Trigger Mode = External Trigger`, `External Trigger One Frame` is checked, and `Compare Set follows current encoder value` is checked, the app calls `_meterWheelService.SetCompare(_settings.MeterWheelCompareValue)`.
  - If `Also apply Encoder Set on external trigger` is also checked, the app then calls `_meterWheelService.SetEncoder(_settings.MeterWheelEncoderValue)`.
  - This is intended to let the external trigger automatically perform the same logical action as the Meter Wheel Control form's Compare `Set`, with optional Encoder `Set` back to the stored origin/preset value.
  - These automatic actions do not mutate the numeric entry fields; they write the persisted values to hardware.
- If the vendor program changes the LSI-8181 hardware state, this app restores its persisted meter wheel settings on the next startup auto-connect or manual connect. It cannot prevent another process with DLL access from changing hardware registers while both programs are running.
- Important API correction:
  - `LSI8181_CO_read` reads the instantaneous physical `CMP_OUT` output state, not whether CMP OUT functionality is enabled.
  - In pulse mode, `CO_read` may return `0` when no compare pulse is active.
  - Do not validate CMP OUT enable by requiring `LSI8181_CO_read == 1`; that caused false failures and was removed in commit `9006c8b`.
- The vendor Compare form's `CMP out` checkbox is not a reliable enable-state display:
  - The vendor main form reads `LSI8181_CO_read`.
  - The lines that would update `Compare_Form.CompareOut_CheckBox.Checked` are commented out in the vendor source.
  - Use actual pulse behavior / wiring or vendor main I/O status as the practical hardware check.
- Difference between compare outputs:
  - Compare operation `CMP_OUT` is the main compare output line.
  - Extension compare `Pulse Width` applies to position-offset compare outputs `CMP0_OUT` through `CMP7_OUT`.
  - `CMP0_OUT` through `CMP7_OUT` are separate differential physical outputs, not just virtual signals.
- The original vendor source remains outside this repository at the user-provided reference folder:
  - `C:\Users\falcomu\Documents\Codex\程式撰寫 專案資料夾\攝影機影像擷取\LSI8181`
- The installed vendor API/manual reference is also available at:
  - `C:\Program Files (x86)\JS Automation\LSI8181\API\sw8181.pdf`
  - `C:\Program Files (x86)\JS Automation\LSI8181\API\x64\LSI8181_64.cs`
- The last verified build after the meter wheel updates succeeded with `0 warning / 0 error`.
- The last verified build after the trigger-page and meter-wheel entry-value updates also succeeded with `0 warning / 0 error`.
- The latest verified build after external-trigger compare/encoder automation and meter-wheel persistence fixes also succeeded with `0 warning / 0 error`.

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

### External Trigger / Meter Wheel Line Scan

- The external trigger work is still under active tuning.
- The intended user flow is:
  1. Meter wheel rotates and outputs TTL pulses.
  2. Each pulse triggers one line.
  3. The app continues collecting lines until the requested image length is reached.
  4. Only then should the image be displayed as a completed frame.
- The most useful user-confirmed UI mapping from CamExpert is:
  - `Board -> Advanced control -> Line Sync Source = External Line Trigger`
  - `Line integration method setting = Method 3`
  - `Camera Control Method Selected = Method 3`
  - `Exposure = 40`
  - `Integration pulse #0 = High`
  - `Integration pulse #1 = Low`
  - `CC1 = Pulse #1`
  - `Attached camera -> I/O controls -> Trigger mode = On`
- The user also confirmed the freerun switch-back rule:
  - `Board -> Advanced control -> Line Sync Source = None`
  - This is the required setting to return to freerun mode in the official program.
- The current Sapera names that matter for this path are:
  - `CIRACQ_PRM_EXT_LINE_TRIGGER_ENABLE`
  - `CIRACQ_PRM_LINE_INTEGRATE_METHOD`
  - `CORACQ_PRM_LINE_INTEGRATE_ENABLE`
  - `CORACQ_ORM_CAM_IO_CONTROL`
  - `TriggerMode`
- Treat `Line Sync Source = None` as the explicit freerun state, not just "external trigger disabled".
- Current behavior to remember:
  - Free-run / continuous mode must keep its internal line rate path intact.
  - External-trigger writes must not leak into free-run, or Sapera starts showing warning dialogs and the app can fall back into unexpected continuous acquisition behavior.
  - Some Sapera writes can fail with `parameter invalid value`, `parameter not available`, or `outofrange` if the camera / board is not in the right state.
- The current code path was adjusted several times to avoid unavailable acquisition writes and to preserve continuous mode. Treat external-trigger changes carefully and verify on real hardware after each edit.

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
- Important deployment finding from `2026-08-26`:
  - The development machine uses Sapera SDK/runtime `9.12`.
  - The field machine was reported as Sapera SDK/runtime `8.6`.
  - The observed field error was `System.IO.FileLoadException: cannot load a procedure imported by DALSA.SaperaLT.SapClassBasic.dll`.
  - This strongly suggests a Sapera managed/native DLL version mismatch, not necessarily missing frame-grabber hardware.
  - A machine can have a capture card installed but still fail if the runtime DLLs are older than the managed `DALSA.SaperaLT.SapClassBasic.dll` used by the app build.
  - Preferred resolution: keep the development and field Sapera runtime versions aligned.
  - If the field machine cannot be upgraded to `9.12`, rebuild this app against the field machine's `8.6` Sapera DLLs.
- Commit `d0ed6fb` added safer Sapera cleanup:
  - connect-failure cleanup now uses guarded per-object destroy/dispose calls,
  - disconnect cleanup uses the same guarded cleanup path,
  - cleanup failures are logged through `AppLogger` instead of surfacing directly as a UI thread exception.
- This guarded cleanup does not fix Sapera version incompatibility by itself; it only prevents cleanup-time failures from interrupting app shutdown or connect failure handling.

## Known Issues / Current Limits

- No automated tests.
- No installer or deployment package.
- Deployment currently depends on a compatible Sapera LT runtime. The app was built on a Sapera `9.12` development machine, while the field machine was reported as `8.6`; this mismatch can cause `FileLoadException` / imported procedure load failures even when a capture card is present.
- LSI-8181 meter wheel support is now partially implemented, but it still requires real hardware validation.
- The app depends on `LSI8181_64.dll` and the LSI-8181 driver being available at runtime. The vendor DLL is not committed to this repository.
- The current meter wheel layer only wraps the needed APIs for basic counter/compare/CMP OUT setup. It does not embed or expose the full vendor test program.
- The current meter wheel layer also wraps the needed position-offset compare APIs for `CMP0_OUT` through `CMP7_OUT`; it still does not expose every vendor API.
- The app cannot prevent the vendor LSI-8181 program from changing hardware state if both programs can access the same card. Avoid running both as active controllers at the same time.
- External-trigger support is not yet considered finished. The current goal is line-by-line triggering from the meter wheel output, not a full-frame software trigger.
- Preview responsiveness has improved, but true in-progress line-scan display would require chunk/line-based acquisition events instead of only `EndOfFrame`.
- `FrameRecorder` stores the latest full frame and, when rolling capture is enabled, a bounded rolling frame list. It does not append all frames/chunks into an unlimited continuous long image.
- Very large image saves are still expensive. Example user case: `16384 x 50000` 8-bit image is about 819 MB raw data and may encode to roughly 400 MB depending on content/format.
- PNG/TIF save time may be dominated by encoder CPU work, not just disk throughput.
- `TIF (uncompressed)` exists as a speed-oriented option, trading larger files for less compression work.
- Parallel save limit is currently hard-coded in `MainForm` as `MaxConcurrentSnapshotSaves = 5`.
- Large-image viewing is now tuned for smooth pan by showing preview while dragging and restoring high-resolution tiles after mouse-up. This is intentional; do not force high-resolution tile drawing during mouse drag unless replacing it with a faster rendering engine.
- Large-image tile seams were addressed in commit `9522f55`; if seams reappear, first inspect tile destination rounding and interpolation/wrap mode before changing the tile cache architecture.
- The downloaded PDF `20161221032935911.pdf` is currently untracked and should not be committed unless intentionally needed.

## Recommended Next Steps

1. Test the latest `main` on the real LSI-8181/camera machine after this handoff update.
2. Align Sapera runtime versions before deeper camera debugging:
   - Option A: install Sapera `9.12` runtime/SDK on the field machine to match the development machine.
   - Option B: rebuild the app against Sapera `8.6` DLLs if the field machine must remain on `8.6`.
   - Confirm `CameraCaptureApp.exe` is running as `x64` and loads matching 64-bit Sapera native DLLs.
   - Do not treat capture-card presence alone as proof that Sapera DLL loading is correct.
3. Verify the meter wheel and camera wiring before chasing software bugs:
   - meter wheel TTL output
   - camera trigger input
   - expected physical output from the LSI-8181 card
4. Confirm the desired capture mode on hardware:
   - one pulse equals one line
   - the app keeps stacking lines until the requested image length is reached
   - display happens only after the full line count is collected
5. Verify the current free-run path still works after the external-trigger changes:
   - continuous mode should not inherit external-trigger parameters
   - no Sapera warning dialogs should appear in normal free-run
6. Verify meter wheel card connection behavior with `LSI8181_64.dll` available beside the app executable or in the DLL search path.
7. Verify startup behavior:
   - Start the app.
   - Wait at least `1 second`.
   - Confirm the meter wheel auto-connects to the persisted `MeterWheelCardId`.
   - Open and close `Meter Wheel Control`; confirm closing the window does not disconnect the card.
8. Verify physical output wiring:
   - Main compare output: `CMP_OUT`.
   - Extension offset outputs: `CMP0_OUT` through `CMP7_OUT`.
9. Verify actual `CMP_OUT` pulse behavior with the configured `CMP Out Width`; do not rely on the vendor Compare form checkbox alone.
10. Confirm the persisted meter wheel settings are restored and applied after app restart:
   - `MeterWheelCardId`
   - `MeterWheelCompareIncrement`
   - `MeterWheelMultipleRate`
   - `MeterWheelReverseDirection`
   - `MeterWheelCmpOutWidth`
   - `MeterWheelEncoderValue`
   - `MeterWheelCompareValue`
   - `MeterWheelExtensionCompareMask`
   - `MeterWheelExtensionCompareOffsets`
   - `MeterWheelExtensionComparePulseWidths`
   - `MeterWheelExtensionCompareOutputStates`
11. Verify external-trigger automation on real hardware:
   - `Continuous` mode disables `Compare Set follows current encoder value`.
   - `External Trigger` mode allows `Compare Set follows current encoder value`.
   - External trigger causes Compare Set to write the saved `MeterWheelCompareValue`.
   - With `Also apply Encoder Set on external trigger`, external trigger also writes the saved `MeterWheelEncoderValue`.
12. Verify extension compare output behavior for `CMP0_OUT` through `CMP7_OUT` on real hardware.
13. Compare `PNG`, `TIF`, and `TIF (uncompressed)` save time on the real camera machine for the target image size.
14. Re-test large-image pan behavior on the target `16384 x 50000` files at the known sensitive zoom levels:
   - `0.19x`
   - `0.23x`
   - `0.29x`
   - `0.36x`
   - `0.45x`
   - `0.57x`
   - `0.71x`
15. Re-test large-image tile seam visibility after zooming and panning at fractional zoom levels.
16. Re-test gray waveform sampling on loaded large images and camera frames:
   - loaded large image should sample real source pixels through `LargeImageSource`,
   - camera frame should sample the full-resolution frame from `FrameRecorder`, not the UI preview bitmap.
17. If save speed remains too slow, profile time spent in:
   - copying frame bytes into the grayscale buffer,
   - WIC encoding,
   - final disk write / antivirus / sync folder overhead.
18. If many huge images are saved at once, test whether `MaxConcurrentSnapshotSaves = 5` is actually optimal. Large uncompressed/TIFF writes may perform better at 2 or 3 concurrent jobs on some disks.
19. If preview still feels delayed, determine whether delay comes from waiting for `EndOfFrame`:
   - Large `Length` values naturally delay UI updates because the app waits for a full frame.
   - Consider `EndOfNLines` or other Sapera line/chunk callbacks if supported.
20. If full continuous long-image capture is required, implement a recorder queue:
   - Background worker owns the full-resolution data path.
   - UI preview remains downscaled and droppable.
   - Capture/export reads from recorder output.
21. Keep exposure/gain/length/internal line rate behavior stable when changing preview or recorder architecture.

## Notes For The Next Person

- Do not reintroduce broad line-rate probing. The confirmed line-rate feature is attached-camera `AcquisitionLineRate` written as `Int64`.
- Do not rely on CCF file edits for internal line rate; CamExpert changed runtime/attached-camera state without updating CCF.
- Do not set `ExposureStart` trigger source while configuring internal line rate; this previously broke exposure behavior.
- Keep `CameraSettingsForm` Designer-editable.
- Do not assume the vendor LSI-8181 program is still embedded. It was intentionally removed in commit `2700831`.
- Keep LSI-8181 support as a minimal C# service/form that directly wraps only the needed LSI-8181 DLL APIs and persists/applies settings explicitly.
- Do not use `LSI8181_CO_read == 1` as proof that CMP OUT is enabled. It is an instantaneous output-state readback and may be `0` between pulses.
- Keep `MeterWheelControlForm` Designer-editable.
- Keep `MeterWheelExtensionCompareForm` Designer-editable.
- Keep parameterless constructors on Designer-editable forms so Visual Studio can instantiate them.
- Keep runtime-only dependencies such as `Lsi8181MeterWheelService` out of Designer constructors.
- Do not add custom helper method calls inside `.Designer.cs` files; Visual Studio Designer can fail to load the form even when MSBuild succeeds.
- Do not move meter wheel service ownership back into `MeterWheelControlForm`; closing that window must not disconnect the meter wheel.
- `MainForm` owns the meter wheel service and is responsible for app-start auto-connect and final app-close disposal.
- Keep meter wheel setting load guarded so UI control-change events do not overwrite persisted values during form initialization.
- Keep Compare numeric entry user-defined. Do not auto-fill Compare from live encoder when the user presses Compare `Set`.
- `Compare Set follows current encoder value` is intentionally disabled outside `External Trigger` mode.
- The optional external-trigger Encoder Set writes the saved `MeterWheelEncoderValue`, not the current live encoder value.
- Treat exposure/gain/length/internal line rate as currently stable behavior and test all four after camera pipeline changes.
- Preserve the separation between UI preview and full-resolution save data. The save path should continue using `FrameRecorder` snapshots, not the downscaled display bitmap.
- Preserve the separation between UI preview and gray-waveform sampling data. Camera waveform sampling should continue using `FrameRecorder.SnapshotLatest()` when the viewer is displaying a preview bitmap.
- Preserve `IGrayPixelSource.GetGrayValues` batch sampling. Large-image waveform speed depends on tile grouping and batch reads.
- Preserve large-image pan behavior: preview while dragging, high-resolution tiles after mouse-up.
- For save pipeline changes, keep the temporary `.tmp` write then final move behavior to avoid users opening incomplete output files.
- Do not assume `FileLoadException` from `DALSA.SaperaLT.SapClassBasic.dll` means no capture card. It can mean the card exists but Sapera managed/native DLL versions are mismatched.

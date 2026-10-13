# Camera Image Capture

A Windows Forms application for Teledyne DALSA line-scan cameras and Camera Link frame grabbers. The application targets .NET Framework 4.7.2 and runs as a 64-bit process.

## Project

- Solution: `CameraCaptureApp.sln`
- Main project: `CameraCaptureApp/CameraCaptureApp.csproj`
- Current branch: `main`
- Latest pushed commit: `8e6b375` (`Prefer no-config device for matched camera`)
- Detailed implementation notes and handoff: [`PROJECT_HANDOFF.md`](PROJECT_HANDOFF.md)

## Features

- Camera connection, preview, frame capture, and Exposure / Gain controls.
- Continuous, External Trigger, and Software Trigger workflows.
- Image saving as PNG, TIF, or uncompressed TIF.
- Large-image viewing with zoom, pan, tiled detail rendering, and grayscale waveform selection.
- LSI-8181 meter-wheel controls and compare-output configuration.

## Build Requirements

- Windows and Visual Studio/MSBuild with the .NET Framework 4.7.2 targeting pack.
- A compatible 64-bit Sapera LT SDK/runtime. The managed Sapera assembly and installed native runtime must be version-compatible.
- The LSI-8181 driver and `LSI8181_64.dll` for meter-wheel functionality. The vendor DLL is not included in this repository.

Build command previously used successfully on the development machine:

```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64\MSBuild.exe" ".\CameraCaptureApp.sln" /p:Configuration=Debug /p:Platform="Any CPU" /p:GenerateResourceMSBuildArchitecture=CurrentArchitecture /m:1
```

The latest AcqDevice constructor-order change has not yet been rebuilt or validated on the target camera hardware.

## Two-Frame-Grabber Setup

The application can be run as two separate copies, each configured for one acquisition server:

| Acquisition server | Attached-camera feature path | Reported AcqDevice resource |
| --- | --- | --- |
| `Xtium-CL_MX4_1` (reported) | `CameraLink_1#0` | `Xtium-CL_MX4_1_Serial_0` |
| `Xtium-CL_MX4_2` (reported) | `CameraLink_2#0` | `Xtium-CL_MX4_2_Serial_0` |

The user confirmed that Exposure and Gain are independently controllable when each application copy selects the corresponding card. Resource-name matching is used to associate each CameraLink AcqDevice with its Xtium acquisition server.

For a resource-name-matched camera, the latest code tries the no-config `SapAcqDevice` constructor first and retains config-file and boolean constructor fallbacks. This ordering was selected after finding a slow, failed config-file `Create()` on card 1. The change is pushed in commit `8e6b375`; its effect has not yet been measured on hardware. The user considers the current connection behavior acceptable and has paused further performance tuning for now.

## Logs

Each running copy writes to its own `logs\app.log` beside the executable. Useful camera initialization records include:

- `AcqDevice variant`: constructor and `Create()` timing for each constructor path.
- `AcqDevice probe`: total time spent trying a candidate AcqDevice location.
- `Notebook feature timing`: AcqDevice preparation and line-rate, exposure, gain, trigger, and feature-update timings.
- `Connect timing`: connection-stage totals.

Other diagnostic reports, including `last_requested_settings.txt` and `last_apply_params.txt`, are also written under the application's `logs` directory.

## Notes

- Use matching 64-bit Sapera managed/native components; capture-card presence alone does not guarantee runtime compatibility.
- The project has no automated test suite or installer. Camera and trigger behavior require validation on the target hardware.
- `20161221032935911.pdf` may be present as an untracked local file; it is not part of the application source.

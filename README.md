# my-app-2

Camera image capture application based on WinForms and .NET Framework 4.7.2.

## Current State

The repository now contains a runnable desktop application scaffold with:

- a modular WinForms project
- a line-scan oriented main dashboard
- a camera settings dialog
- service and model layers separated from `MainForm`
- placeholder line-scan camera service ready for Sapera SDK integration

## Project Structure

- `CameraCaptureApp.sln`: Visual Studio solution
- `CameraCaptureApp/Forms`: main window and settings dialog
- `CameraCaptureApp/Controls`: reusable UI controls
- `CameraCaptureApp/Services`: camera and settings service abstractions
- `CameraCaptureApp/Models`: shared data models

## Build

This project targets `.NET Framework 4.7.2`.

Example build command on this machine:

```powershell
& 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe' CameraCaptureApp.sln /t:Build /p:Configuration=Debug
```

## Next Steps

1. Replace the placeholder `CameraService` with a real line-scan Sapera integration.
2. Stream incoming scan lines into a long-image buffer instead of replacing full frames.
3. Implement save flow for long images and intermediate scan snapshots.
4. Add config persistence to disk.
5. Refine the Designer layout around scan progress and diagnostics.

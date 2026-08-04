using CameraCaptureApp.Models;

namespace CameraCaptureApp.Services
{
    public interface ICameraService
    {
        CameraStatus Status { get; }

        void ApplySettings(CameraSettings settings);

        bool Connect();

        void Disconnect();

        bool StartPreview();

        void StopPreview();

        bool CaptureFrame();
    }
}

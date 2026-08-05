using CameraCaptureApp.Models;

namespace CameraCaptureApp.Services
{
    public interface ICameraService
    {
        event System.EventHandler<CameraFrameEventArgs> FrameReady;

        CameraSettings CurrentSettings { get; }

        CameraStatus Status { get; }

        void ApplySettings(CameraSettings settings);

        bool Connect();

        bool SelectConnectionSettings(System.Windows.Forms.IWin32Window owner);

        string ExportLiveFeatureReport();

        void Disconnect();

        bool StartPreview();

        void StopPreview();

        bool CaptureFrame();
    }
}

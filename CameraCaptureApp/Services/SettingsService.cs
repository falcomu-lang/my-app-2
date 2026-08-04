using CameraCaptureApp.Models;

namespace CameraCaptureApp.Services
{
    public class SettingsService : ISettingsService
    {
        private CameraSettings _settings;

        public SettingsService()
        {
            _settings = CameraSettings.CreateDefault();
        }

        public CameraSettings Load()
        {
            return _settings.Clone();
        }

        public void Save(CameraSettings settings)
        {
            _settings = settings.Clone();
        }
    }
}

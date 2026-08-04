namespace CameraCaptureApp.Models
{
    public class CameraSettings
    {
        public string CameraName { get; set; }

        public string ConfigFilePath { get; set; }

        public string ServerName { get; set; }

        public int ResourceIndex { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public decimal ExposureTime { get; set; }

        public decimal Gain { get; set; }

        public decimal FrameRate { get; set; }

        public string PixelFormat { get; set; }

        public TriggerMode TriggerMode { get; set; }

        public bool AutoConnect { get; set; }

        public bool AutoSave { get; set; }

        public string SaveFolder { get; set; }

        public string FileNamePattern { get; set; }

        public static CameraSettings CreateDefault()
        {
            return new CameraSettings
            {
                CameraName = "Default Camera",
                ConfigFilePath = string.Empty,
                ServerName = string.Empty,
                ResourceIndex = 0,
                Width = 1280,
                Height = 720,
                ExposureTime = 1200,
                Gain = 1,
                FrameRate = 30,
                PixelFormat = "Mono8",
                TriggerMode = TriggerMode.Continuous,
                AutoConnect = false,
                AutoSave = false,
                SaveFolder = string.Empty,
                FileNamePattern = "capture_{yyyyMMdd_HHmmss}"
            };
        }

        public CameraSettings Clone()
        {
            return (CameraSettings)MemberwiseClone();
        }
    }
}

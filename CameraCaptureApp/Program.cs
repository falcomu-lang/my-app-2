using System;
using System.Windows.Forms;
using CameraCaptureApp.Forms;
using CameraCaptureApp.Services;

namespace CameraCaptureApp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UiExceptionHandler.Register();
            AppLogger.Log("Application started. BaseDirectory=" + AppDomain.CurrentDomain.BaseDirectory);

            ISettingsService settingsService = new SettingsService();
            ICameraService cameraService = new CameraService();
            ILsi8181Service lsi8181Service = new Lsi8181Service();

            Application.Run(new MainForm(cameraService, settingsService, lsi8181Service));
        }
    }
}

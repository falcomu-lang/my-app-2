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

            ISettingsService settingsService = new SettingsService();
            ICameraService cameraService = new CameraService();

            Application.Run(new MainForm(cameraService, settingsService));
        }
    }
}

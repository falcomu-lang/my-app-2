using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using CameraCaptureApp.Models;
using DALSA.SaperaLT.SapClassBasic;
using DALSA.SaperaLT.SapClassGui;

namespace CameraCaptureApp.Services
{
    public class CameraService : ICameraService
    {
        private const int PreviewIntervalMilliseconds = 200;

        private readonly CameraStatus _status;
        private readonly object _frameSync = new object();
        private CameraSettings _settings;
        private SapLocation _serverLocation;
        private string _configFileName;
        private SapAcquisition _acquisition;
        private SapBuffer _buffers;
        private SapAcqToBuf _transfer;
        private DateTime _lastPreviewFrameUtc;

        public CameraService()
        {
            _settings = CameraSettings.CreateDefault();
            _status = new CameraStatus
            {
                FrameWidth = _settings.Width,
                FrameHeight = _settings.Height,
                CameraName = _settings.CameraName,
                UpdateRateHz = 5,
                FollowLatestLine = true,
                ScanStateText = "Idle",
                LastMessage = "Sapera camera service is ready."
            };
        }

        public event EventHandler<CameraFrameEventArgs> FrameReady;

        public CameraSettings CurrentSettings
        {
            get { return _settings.Clone(); }
        }

        public CameraStatus Status
        {
            get { return _status; }
        }

        public void ApplySettings(CameraSettings settings)
        {
            _settings = settings.Clone();
            _status.FrameWidth = _settings.Width;
            _status.FrameHeight = _settings.Height;
            _status.CameraName = _settings.CameraName;
            _status.LastMessage = "Camera settings applied.";
        }

        public bool Connect()
        {
            var attemptedStoredSettings = false;
            try
            {
                attemptedStoredSettings = HasStoredConnectionSettings();
                if (!TryPrepareConnectionSettings())
                {
                    _status.LastMessage = "Camera connection was cancelled.";
                    return false;
                }

                return OpenCurrentConnection();
            }
            catch (Exception ex)
            {
                if (attemptedStoredSettings && SelectConnectionSettings(null))
                {
                    try
                    {
                        return OpenCurrentConnection();
                    }
                    catch (Exception retryEx)
                    {
                        ex = retryEx;
                    }
                }

                DestroySdkObjects();
                DisposeSdkObjects();
                _status.IsConnected = false;
                _status.HasSignal = false;
                _status.ScanStateText = "Error";
                _status.LastMessage = "Camera connect failed: " + ex.Message;
                return false;
            }
        }

        public void Disconnect()
        {
            DestroySdkObjects();
            DisposeSdkObjects();
            _status.IsPreviewing = false;
            _status.IsConnected = false;
            _status.HasSignal = false;
            _status.ScanStateText = "Disconnected";
            _status.LastMessage = "Camera disconnected.";
        }

        public bool StartPreview()
        {
            if (!_status.IsConnected || _transfer == null)
            {
                _status.LastMessage = "Connect the camera before starting preview.";
                return false;
            }

            if (_transfer.Grab())
            {
                _status.IsPreviewing = true;
                _status.ScanStateText = "Preview";
                _status.LastMessage = "Preview started.";
                return true;
            }

            _status.LastMessage = "Preview could not be started.";
            return false;
        }

        public void StopPreview()
        {
            if (_transfer != null && _transfer.Initialized)
            {
                _transfer.Freeze();
            }

            _status.IsPreviewing = false;
            _status.ScanStateText = "Stopped";
            _status.LastMessage = "Preview stopped.";
        }

        public bool CaptureFrame()
        {
            if (!_status.IsConnected || _transfer == null)
            {
                _status.LastMessage = "Connect the camera before grabbing a frame.";
                return false;
            }

            if (_transfer.Snap())
            {
                _status.ScanStateText = "Snap";
                _status.LastMessage = "Single frame capture requested.";
                return true;
            }

            _status.LastMessage = "Single frame capture could not be started.";
            return false;
        }

        public bool SelectConnectionSettings(System.Windows.Forms.IWin32Window owner)
        {
            using (var dialog = new AcqConfigDlg(null, _settings.ConfigFilePath ?? string.Empty, AcqConfigDlg.ServerCategory.ServerAcq))
            {
                if (dialog.ShowDialog(owner) != System.Windows.Forms.DialogResult.OK)
                {
                    return false;
                }

                ApplyDialogSelection(dialog);
                _status.LastMessage = "Connection settings updated from Sapera.";
                return true;
            }
        }

        private bool TryPrepareConnectionSettings()
        {
            if (HasStoredConnectionSettings())
            {
                BuildLocationFromSettings();
                return true;
            }

            return SelectConnectionSettings(null);
        }

        private bool HasStoredConnectionSettings()
        {
            return !string.IsNullOrWhiteSpace(_settings.ConfigFilePath)
                && File.Exists(_settings.ConfigFilePath)
                && (!string.IsNullOrWhiteSpace(_settings.ServerName) || _settings.ServerIndex >= 0)
                && _settings.ResourceIndex >= 0;
        }

        private void BuildLocationFromSettings()
        {
            if (!string.IsNullOrWhiteSpace(_settings.ServerName))
            {
                _serverLocation = new SapLocation(_settings.ServerName, _settings.ResourceIndex);
            }
            else
            {
                _serverLocation = new SapLocation(_settings.ServerIndex, _settings.ResourceIndex);
            }

            _configFileName = _settings.ConfigFilePath;
        }

        private void ApplyDialogSelection(AcqConfigDlg dialog)
        {
            _serverLocation = dialog.ServerLocation;
            _configFileName = dialog.ConfigFile;
            _settings.ServerName = _serverLocation.ServerName;
            _settings.ServerIndex = _serverLocation.ServerIndex;
            _settings.ResourceIndex = _serverLocation.ResourceIndex;
            _settings.ConfigFilePath = _configFileName;
            _settings.CameraName = _serverLocation.ServerName;
        }

        private bool CreateSdkObjects()
        {
            if (_acquisition != null && !_acquisition.Initialized && !_acquisition.Create())
            {
                return false;
            }

            if (_buffers != null && !_buffers.Initialized)
            {
                if (!_buffers.Create())
                {
                    return false;
                }

                _buffers.Clear();
            }

            if (_transfer != null && !_transfer.Initialized && !_transfer.Create())
            {
                return false;
            }

            _acquisition.SignalNotifyEnable = true;
            return true;
        }

        private void DestroySdkObjects()
        {
            if (_transfer != null && _transfer.Initialized)
            {
                _transfer.Destroy();
            }

            if (_buffers != null && _buffers.Initialized)
            {
                _buffers.Destroy();
            }

            if (_acquisition != null && _acquisition.Initialized)
            {
                _acquisition.Destroy();
            }
        }

        private void DisposeSdkObjects()
        {
            if (_transfer != null)
            {
                _transfer.XferNotify -= OnTransferNotify;
                _transfer.Dispose();
                _transfer = null;
            }

            if (_buffers != null)
            {
                _buffers.Dispose();
                _buffers = null;
            }

            if (_acquisition != null)
            {
                _acquisition.SignalNotify -= OnSignalNotify;
                _acquisition.Dispose();
                _acquisition = null;
            }
        }

        private void OnTransferNotify(object sender, SapXferNotifyEventArgs argsNotify)
        {
            _status.ScannedLineCount = argsNotify.EventCount;
            _status.ScanStateText = argsNotify.Trash ? "Trash" : "Receiving";
            _status.LastMessage = argsNotify.Trash
                ? "Frame landed in trash buffer."
                : "Frame received from Sapera.";

            if (!argsNotify.Trash)
            {
                PublishPreviewFrame();
            }
        }

        private void OnSignalNotify(object sender, SapSignalNotifyEventArgs argsSignal)
        {
            _status.HasSignal = argsSignal.SignalStatus != SapAcquisition.AcqSignalStatus.None;
            if (!_status.HasSignal)
            {
                _status.IsConnected = false;
                _status.LastMessage = "No camera signal detected.";
            }
            else
            {
                _status.IsConnected = true;
            }
        }

        private void PublishPreviewFrame()
        {
            lock (_frameSync)
            {
                var elapsed = DateTime.UtcNow - _lastPreviewFrameUtc;
                if (elapsed.TotalMilliseconds < PreviewIntervalMilliseconds)
                {
                    return;
                }

                _lastPreviewFrameUtc = DateTime.UtcNow;
            }

            Bitmap previewFrame = null;
            try
            {
                previewFrame = TryCreatePreviewBitmap();
                if (previewFrame == null)
                {
                    return;
                }

                var handler = FrameReady;
                if (handler != null)
                {
                    handler(this, new CameraFrameEventArgs(previewFrame));
                    previewFrame = null;
                }
            }
            catch (Exception ex)
            {
                _status.LastMessage = "Preview conversion failed: " + ex.Message;
            }
            finally
            {
                if (previewFrame != null)
                {
                    previewFrame.Dispose();
                }
            }
        }

        private Bitmap TryCreatePreviewBitmap()
        {
            if (_buffers == null || !_buffers.Initialized)
            {
                return null;
            }

            var width = _buffers.Width;
            var height = _buffers.Height;
            if (width <= 0 || height <= 0)
            {
                return null;
            }

            int pixelDepth;
            int pitch;
            if (!_buffers.GetParameter(SapBuffer.Prm.PIXEL_DEPTH, out pixelDepth) ||
                !_buffers.GetParameter(SapBuffer.Prm.PITCH, out pitch))
            {
                return null;
            }

            var bytesPerPixel = Math.Max(1, (pixelDepth + 7) / 8);
            if (bytesPerPixel != 1 && bytesPerPixel != 3)
            {
                bytesPerPixel = 1;
            }

            var expectedStride = width * bytesPerPixel;
            if (pitch < expectedStride)
            {
                pitch = expectedStride;
            }

            var rawBytes = new byte[pitch * height];
            var handle = GCHandle.Alloc(rawBytes, GCHandleType.Pinned);
            try
            {
                if (!_buffers.ReadRect(0, 0, width, height, handle.AddrOfPinnedObject()))
                {
                    return null;
                }
            }
            finally
            {
                handle.Free();
            }

            return bytesPerPixel == 3
                ? CreateRgbBitmap(rawBytes, width, height, pitch)
                : CreateMonoBitmap(rawBytes, width, height, pitch);
        }

        private static Bitmap CreateMonoBitmap(byte[] rawBytes, int width, int height, int sourceStride)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            var palette = bitmap.Palette;
            for (var i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }

            bitmap.Palette = palette;
            var data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            try
            {
                for (var y = 0; y < height; y++)
                {
                    Marshal.Copy(rawBytes, y * sourceStride, data.Scan0 + (y * data.Stride), width);
                }
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return bitmap;
        }

        private static Bitmap CreateRgbBitmap(byte[] rawBytes, int width, int height, int sourceStride)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            var data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            try
            {
                var rowLength = width * 3;
                for (var y = 0; y < height; y++)
                {
                    Marshal.Copy(rawBytes, y * sourceStride, data.Scan0 + (y * data.Stride), rowLength);
                }
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return bitmap;
        }

        private bool OpenCurrentConnection()
        {
            DestroySdkObjects();
            DisposeSdkObjects();

            _acquisition = new SapAcquisition(_serverLocation, _configFileName);
            if (SapBuffer.IsBufferTypeSupported(_serverLocation, SapBuffer.MemoryType.ScatterGather))
            {
                _buffers = new SapBufferWithTrash(2, _acquisition, SapBuffer.MemoryType.ScatterGather);
            }
            else
            {
                _buffers = new SapBufferWithTrash(2, _acquisition, SapBuffer.MemoryType.ScatterGatherPhysical);
            }

            _transfer = new SapAcqToBuf(_acquisition, _buffers);
            _transfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;
            _transfer.XferNotify += OnTransferNotify;
            _transfer.XferNotifyContext = this;

            _acquisition.SignalNotify += OnSignalNotify;
            _acquisition.SignalNotifyContext = this;

            if (!CreateSdkObjects())
            {
                throw new InvalidOperationException("Sapera objects could not be created.");
            }

            _status.IsConnected = true;
            _status.HasSignal = _acquisition.SignalStatus != SapAcquisition.AcqSignalStatus.None;
            _status.CameraName = _serverLocation.ServerName;
            _status.FrameWidth = _buffers.Width;
            _status.FrameHeight = _buffers.Height;
            if (!_status.HasSignal)
            {
                _status.IsConnected = false;
                _status.LastMessage = "Connected objects were created, but no camera signal was detected.";
            }
            else
            {
                _status.LastMessage = "Camera connected successfully.";
            }
            _status.ScanStateText = "Connected";
            return true;
        }
    }
}

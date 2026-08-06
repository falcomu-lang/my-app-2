using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
        private SapAcqDevice _acqDevice;
        private SapAcquisition _acquisition;
        private SapBuffer _buffers;
        private SapAcqToBuf _transfer;
        private DateTime _lastPreviewFrameUtc;
        private bool _deviceFeaturesAvailable;
        private string _acqDevicePathSummary;

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
            _status.FrameHeight = _settings.Length > 0 ? _settings.Length : _settings.Height;
            _status.CameraName = _settings.CameraName;
            if (_status.IsConnected)
            {
                ApplyWritableCameraSettings(true);
                return;
            }

            _status.LastMessage = HasStoredConnectionSettings()
                ? "Camera settings saved locally. They will be applied on the next connect."
                : "Camera settings saved locally. Select connection settings before applying to hardware.";
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

        public string ExportLiveFeatureReport()
        {
            if (!_status.IsConnected)
            {
                throw new InvalidOperationException("Connect the camera before probing live features.");
            }

            EnsureAcqDeviceAvailable();
            if (!_deviceFeaturesAvailable || _acqDevice == null || !_acqDevice.Initialized)
            {
                throw new InvalidOperationException("SapAcqDevice is not available for this connection path.");
            }

            var reportBuilder = new StringBuilder();
            reportBuilder.AppendLine("CameraCaptureApp Live Feature Report");
            reportBuilder.AppendLine("GeneratedAt=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            reportBuilder.AppendLine("CameraName=" + _status.CameraName);
            reportBuilder.AppendLine("Server=" + _serverLocation.ServerName);
            reportBuilder.AppendLine("ResourceIndex=" + _serverLocation.ResourceIndex);
            reportBuilder.AppendLine("ConfigFile=" + _configFileName);
            reportBuilder.AppendLine();

            for (var i = 0; i < _acqDevice.FeatureCount; i++)
            {
                var feature = new SapFeature();
                if (!_acqDevice.GetFeatureInfo(i, feature))
                {
                    continue;
                }

                var name = feature.Name ?? string.Empty;
                if (name.Length == 0)
                {
                    continue;
                }

                reportBuilder.AppendLine("[Feature] " + name);
                reportBuilder.AppendLine("DisplayName=" + SafeString(feature.DisplayName));
                reportBuilder.AppendLine("Category=" + SafeString(feature.Category));
                reportBuilder.AppendLine("Type=" + feature.DataType);
                reportBuilder.AppendLine("AccessMode=" + feature.DataAccessMode);
                reportBuilder.AppendLine("Visibility=" + feature.UserVisibility);
                reportBuilder.AppendLine("Description=" + SafeString(feature.Description));
                reportBuilder.AppendLine("Value=" + ReadFeatureValue(name, feature.DataType));
                reportBuilder.AppendLine();
            }

            var filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "live_features_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            File.WriteAllText(filePath, reportBuilder.ToString(), Encoding.UTF8);
            _status.LastMessage = "Live feature report exported: " + Path.GetFileName(filePath);
            return filePath;
        }

        public string ExportAcquisitionParameterReport()
        {
            if (_acquisition == null || !_acquisition.Initialized)
            {
                throw new InvalidOperationException("Connect the camera before probing acquisition parameters.");
            }

            var reportBuilder = new StringBuilder();
            reportBuilder.AppendLine("CameraCaptureApp Acquisition Parameter Report");
            reportBuilder.AppendLine("GeneratedAt=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            reportBuilder.AppendLine("CameraName=" + _status.CameraName);
            reportBuilder.AppendLine("Server=" + _serverLocation.ServerName);
            reportBuilder.AppendLine("ResourceIndex=" + _serverLocation.ResourceIndex);
            reportBuilder.AppendLine("ConfigFile=" + _configFileName);
            reportBuilder.AppendLine();

            foreach (SapAcquisition.Prm parameter in Enum.GetValues(typeof(SapAcquisition.Prm)))
            {
                if (!_acquisition.IsParameterAvailable(parameter))
                {
                    continue;
                }

                reportBuilder.AppendLine("[Parameter] " + parameter);
                reportBuilder.AppendLine("Type=" + SapAcquisition.GetParameterType(parameter));
                reportBuilder.AppendLine("Value=" + ReadAcquisitionParameterValue(parameter));
                reportBuilder.AppendLine();
            }

            var filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "acquisition_params_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            File.WriteAllText(filePath, reportBuilder.ToString(), Encoding.UTF8);
            _status.LastMessage = "Acquisition parameter report exported: " + Path.GetFileName(filePath);
            return filePath;
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
            if (_acqDevice != null && !_acqDevice.Initialized && !TryCreateAcqDevice())
            {
                _acqDevice.Dispose();
                _acqDevice = null;
            }

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
            _deviceFeaturesAvailable = false;
            _acqDevicePathSummary = string.Empty;
            if (_acqDevice != null)
            {
                _acqDevice.Dispose();
                _acqDevice = null;
            }

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
            _acquisition.SignalNotify += OnSignalNotify;
            _acquisition.SignalNotifyContext = this;

            if (!_acquisition.Create())
            {
                throw new InvalidOperationException("Sapera objects could not be created.");
            }

            ApplyWritableCameraSettings(false);

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

            if (!CreateSdkObjects())
            {
                throw new InvalidOperationException("Sapera buffer or transfer objects could not be created.");
            }

            ApplyWritableCameraSettings(true);

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
                if (string.IsNullOrWhiteSpace(_status.LastMessage) || _status.LastMessage == "Camera parameters written to Sapera device.")
                {
                    _status.LastMessage = "Camera connected successfully.";
                }
            }
            _status.ScanStateText = "Connected";
            return true;
        }

        private void ApplyWritableCameraSettings(bool includeDeviceFeatures)
        {
            var applied = false;
            var acquisitionLengthApplied = false;
            var notes = new System.Collections.Generic.List<string>();

            if (TrySetInternalLineRate(notes))
            {
                applied = true;
            }

            if (TrySetExposureParameters(notes))
            {
                applied = true;
            }

            if (TrySetLengthParameters(notes))
            {
                _status.FrameHeight = _settings.Length;
                acquisitionLengthApplied = true;
                applied = true;
            }

            if (TryApplyAcquisitionTriggerMode(notes))
            {
                applied = true;
            }

            if (includeDeviceFeatures)
            {
                EnsureAcqDeviceAvailable();

                if (!_deviceFeaturesAvailable || _acqDevice == null || !_acqDevice.Initialized)
                {
                    notes.Add("Device feature path unavailable: Gain cannot be written on this connection");
                }
                else
                {
                    if (TrySetNumericFeature(_settings.ExposureTime, notes, "ExposureTime", "ExposureTimeAbs", "Exposure"))
                    {
                        applied = true;
                    }

                    var gainApplied = false;
                    if (TrySetNumericFeature(_settings.Gain, notes, "Gain", "GainRaw", "AnalogGain"))
                    {
                        gainApplied = true;
                        applied = true;
                    }

                    if (!gainApplied && TrySetNumericFeature(_settings.Gain, notes, "GainAbs", "SensorGain", "DigitalGain", "AllGain", "MasterGain"))
                    {
                        applied = true;
                    }

                    if (!acquisitionLengthApplied && TrySetIntegralFeature(_settings.Length, notes, "Height", "AcquisitionLineCount", "FrameLength", "ImageHeight", "ROIHeight", "LineCount"))
                    {
                        _status.FrameHeight = _settings.Length;
                        applied = true;
                    }

                    if (ApplyTriggerMode(notes))
                    {
                        applied = true;
                    }

                    if (!string.IsNullOrWhiteSpace(_acqDevicePathSummary))
                    {
                        notes.Add("Gain path " + _acqDevicePathSummary);
                    }
                }
            }

            if (notes.Count > 0)
            {
                _status.LastMessage = string.Join(" | ", notes.ToArray());
            }
            else if (applied)
            {
                _status.LastMessage = "Camera parameters written to Sapera device.";
            }
        }

        private bool TrySetNumericFeature(decimal value, System.Collections.Generic.List<string> notes, params string[] featureNames)
        {
            var appliedFeature = string.Empty;
            if (TrySetDeviceFeature(featureNames, (double)value, out appliedFeature))
            {
                notes.Add(appliedFeature + " applied requested=" + value + " readback=" + ReadNumericFeatureValue(appliedFeature));
                return true;
            }

            notes.Add(featureNames[0] + " not supported");
            return false;
        }

        private void TryInitializeAcqDevice()
        {
            _deviceFeaturesAvailable = false;
            _acqDevicePathSummary = string.Empty;
            DisposeAcqDeviceOnly();

            foreach (var candidate in BuildCandidateAcqDeviceLocations())
            {
                var createdDevice = TryBuildAndCreateAcqDevice(candidate);
                if (createdDevice != null)
                {
                    _acqDevice = createdDevice;
                    _deviceFeaturesAvailable = true;
                    _acqDevicePathSummary = candidate.ServerName + "#" + candidate.ResourceIndex;
                    return;
                }
            }
        }

        private void EnsureAcqDeviceAvailable()
        {
            if (_deviceFeaturesAvailable)
            {
                return;
            }

            if (_acqDevice == null)
            {
                TryInitializeAcqDevice();
            }

            if (_deviceFeaturesAvailable)
            {
                return;
            }

            if (_acqDevice != null && !_acqDevice.Initialized)
            {
                if (!TryCreateAcqDevice())
                {
                    _acqDevice.Dispose();
                    _acqDevice = null;
                }
            }
        }

        private bool TryCreateAcqDevice()
        {
            if (_acqDevice == null)
            {
                return false;
            }

            try
            {
                _deviceFeaturesAvailable = _acqDevice.Create();
                return _deviceFeaturesAvailable;
            }
            catch
            {
                _deviceFeaturesAvailable = false;
                return false;
            }
        }

        private void DisposeAcqDeviceOnly()
        {
            if (_acqDevice != null)
            {
                try
                {
                    _acqDevice.Dispose();
                }
                catch
                {
                }

                _acqDevice = null;
            }
        }

        private IEnumerable<SapLocation> BuildCandidateAcqDeviceLocations()
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var location in EnumerateDirectAcqDeviceLocations(_serverLocation.ServerName))
            {
                var key = location.ServerName + "|" + location.ResourceIndex.ToString();
                if (seen.Add(key))
                {
                    yield return location;
                }
            }

            if (_serverLocation != null && !string.IsNullOrWhiteSpace(_serverLocation.ServerName))
            {
                for (var serverIndex = 0; serverIndex < SapManager.GetServerCount(); serverIndex++)
                {
                    var serverName = SapManager.GetServerName(serverIndex);
                    if (!IsRelatedServerName(_serverLocation.ServerName, serverName))
                    {
                        continue;
                    }

                    foreach (var location in EnumerateDirectAcqDeviceLocations(serverName))
                    {
                        var key = location.ServerName + "|" + location.ResourceIndex.ToString();
                        if (seen.Add(key))
                        {
                            yield return location;
                        }
                    }
                }
            }
        }

        private static IEnumerable<SapLocation> EnumerateDirectAcqDeviceLocations(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
            {
                yield break;
            }

            var count = SapManager.GetResourceCount(serverName, SapManager.ResourceType.AcqDevice);
            for (var resourceIndex = 0; resourceIndex < count; resourceIndex++)
            {
                yield return new SapLocation(serverName, resourceIndex);
            }
        }

        private SapAcqDevice TryBuildAndCreateAcqDevice(SapLocation location)
        {
            foreach (var device in BuildAcqDeviceVariants(location))
            {
                if (device == null)
                {
                    continue;
                }

                try
                {
                    if (device.Initialized || device.Create())
                    {
                        return device;
                    }
                }
                catch
                {
                }

                try
                {
                    device.Dispose();
                }
                catch
                {
                }
            }

            return null;
        }

        private IEnumerable<SapAcqDevice> BuildAcqDeviceVariants(SapLocation location)
        {
            SapAcqDevice device;

            device = TryBuildAcqDevice(() => new SapAcqDevice(location, _configFileName));
            if (device != null)
            {
                yield return device;
            }

            device = TryBuildAcqDevice(() => new SapAcqDevice(location));
            if (device != null)
            {
                yield return device;
            }

            device = TryBuildAcqDevice(() => new SapAcqDevice(location, true));
            if (device != null)
            {
                yield return device;
            }

            device = TryBuildAcqDevice(() => new SapAcqDevice(location, false));
            if (device != null)
            {
                yield return device;
            }
        }

        private static SapAcqDevice TryBuildAcqDevice(Func<SapAcqDevice> factory)
        {
            try
            {
                return factory();
            }
            catch
            {
                return null;
            }
        }

        private static bool IsRelatedServerName(string primaryServerName, string candidateServerName)
        {
            if (string.IsNullOrWhiteSpace(primaryServerName) || string.IsNullOrWhiteSpace(candidateServerName))
            {
                return false;
            }

            if (string.Equals(primaryServerName, candidateServerName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var normalizedPrimary = NormalizeServerName(primaryServerName);
            var normalizedCandidate = NormalizeServerName(candidateServerName);
            return string.Equals(normalizedPrimary, normalizedCandidate, StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeServerName(string serverName)
        {
            var trimmed = serverName.Trim();
            if (trimmed.Length > 2 && char.IsWhiteSpace(trimmed[trimmed.Length - 2]) && char.IsDigit(trimmed[trimmed.Length - 1]))
            {
                return trimmed.Substring(0, trimmed.Length - 2).TrimEnd();
            }

            return trimmed;
        }

        private bool TrySetIntegralFeature(int value, System.Collections.Generic.List<string> notes, params string[] featureNames)
        {
            var appliedFeature = string.Empty;
            if (TrySetDeviceFeature(featureNames, value, out appliedFeature))
            {
                notes.Add(appliedFeature + " applied requested=" + value + " readback=" + ReadNumericFeatureValue(appliedFeature));
                return true;
            }

            notes.Add(featureNames[0] + " not supported");
            return false;
        }

        private bool ApplyTriggerMode(System.Collections.Generic.List<string> notes)
        {
            switch (_settings.TriggerMode)
            {
                case TriggerMode.Continuous:
                    if (TryConfigureTriggerSelector("FrameStart", false, null) |
                        TryConfigureTriggerSelector("LineStart", false, null) |
                        TrySetDeviceFeature(new[] { "TriggerMode" }, "Off"))
                    {
                        notes.Add("TriggerMode continuous applied");
                        return true;
                    }

                    notes.Add("TriggerMode continuous not supported");
                    return false;

                case TriggerMode.SoftwareTrigger:
                    if (TryConfigureTriggerSelector("FrameStart", true, "Software") |
                        TryConfigureTriggerSelector("LineStart", true, "Software"))
                    {
                        notes.Add("TriggerMode software applied");
                        return true;
                    }

                    notes.Add("TriggerMode software not supported");
                    return false;

                case TriggerMode.ExternalTrigger:
                    if (TryConfigureTriggerSelector("LineStart", true, "Line1") |
                        TryConfigureTriggerSelector("FrameStart", true, "Line1") |
                        TryConfigureTriggerSelector("FrameStart", true, "Input1"))
                    {
                        notes.Add("TriggerMode external applied");
                        return true;
                    }

                    notes.Add("TriggerMode external not supported");
                    return false;

                case TriggerMode.SingleFrame:
                    if (TryConfigureTriggerSelector("FrameStart", true, "Software") |
                        TryConfigureTriggerSelector("AcquisitionStart", true, "Software"))
                    {
                        notes.Add("TriggerMode single-frame applied");
                        return true;
                    }

                    notes.Add("TriggerMode single-frame not supported");
                    return false;

                default:
                    return false;
            }
        }

        private bool TrySetInternalLineRate(System.Collections.Generic.List<string> notes)
        {
            var acquisitionRate = decimal.ToInt32(decimal.Truncate(_settings.InternalLineRate));
            TrySetAcquisitionIntParameter(notes, 1, SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE);
            if (TrySetAcquisitionIntParameter(notes, acquisitionRate, SapAcquisition.Prm.INT_LINE_TRIGGER_FREQ))
            {
                return true;
            }

            var appliedFeature = string.Empty;
            if (TrySetDeviceFeature(
                new[] { "AcquisitionLineRate", "LineRate", "DeviceLineRate", "InternalLineRate" },
                (double)_settings.InternalLineRate,
                out appliedFeature))
            {
                notes.Add(appliedFeature + " applied");
                return true;
            }

            notes.Add("InternalLineRate not supported");
            return false;
        }

        private bool TrySetExposureParameters(System.Collections.Generic.List<string> notes)
        {
            var exposureValue = decimal.ToInt32(decimal.Truncate(_settings.ExposureTime));
            if (TrySetAcquisitionIntParameter(
                notes,
                exposureValue,
                SapAcquisition.Prm.LINE_INTEGRATE_DURATION,
                SapAcquisition.Prm.CAM_TRIGGER_DURATION))
            {
                return true;
            }

            if (TrySetAcquisitionIntParameter(
                notes,
                exposureValue,
                SapAcquisition.Prm.TIME_INTEGRATE_DURATION))
            {
                return true;
            }

            notes.Add("Acquisition exposure parameter not supported");
            return false;
        }

        private bool TrySetLengthParameters(System.Collections.Generic.List<string> notes)
        {
            if (TrySetAcquisitionIntParameter(
                notes,
                _settings.Length,
                SapAcquisition.Prm.CROP_HEIGHT))
            {
                return true;
            }

            notes.Add("Acquisition length parameter not supported");
            return false;
        }

        private bool TryApplyAcquisitionTriggerMode(System.Collections.Generic.List<string> notes)
        {
            switch (_settings.TriggerMode)
            {
                case TriggerMode.Continuous:
                    var useInternalLineTrigger = _settings.InternalLineRate > 0;
                    if (TrySetAcquisitionBoolPattern(
                        notes,
                        new[]
                        {
                            new ParameterWrite(SapAcquisition.Prm.CAM_TRIGGER_ENABLE, 0),
                            new ParameterWrite(SapAcquisition.Prm.LINE_TRIGGER_ENABLE, 0),
                            new ParameterWrite(SapAcquisition.Prm.EXT_TRIGGER_ENABLE, 0),
                            new ParameterWrite(SapAcquisition.Prm.EXT_FRAME_TRIGGER_ENABLE, 0),
                            new ParameterWrite(SapAcquisition.Prm.EXT_LINE_TRIGGER_ENABLE, 0),
                            new ParameterWrite(SapAcquisition.Prm.INT_FRAME_TRIGGER_ENABLE, 0),
                            new ParameterWrite(SapAcquisition.Prm.INT_LINE_TRIGGER_ENABLE, useInternalLineTrigger ? 1 : 0)
                        }))
                    {
                        notes.Add(useInternalLineTrigger
                            ? "Acquisition trigger continuous with internal line rate applied"
                            : "Acquisition trigger continuous applied");
                        return true;
                    }
                    break;

                case TriggerMode.SoftwareTrigger:
                    if (TrySetAcquisitionBoolPattern(
                        notes,
                        new[]
                        {
                            new ParameterWrite(SapAcquisition.Prm.CAM_TRIGGER_ENABLE, 1),
                            new ParameterWrite(SapAcquisition.Prm.LINE_TRIGGER_ENABLE, 1)
                        }))
                    {
                        notes.Add("Acquisition trigger software applied");
                        return true;
                    }
                    break;

                case TriggerMode.ExternalTrigger:
                    if (TrySetAcquisitionBoolPattern(
                        notes,
                        new[]
                        {
                            new ParameterWrite(SapAcquisition.Prm.EXT_LINE_TRIGGER_ENABLE, 1),
                            new ParameterWrite(SapAcquisition.Prm.EXT_TRIGGER_ENABLE, 1),
                            new ParameterWrite(SapAcquisition.Prm.EXT_FRAME_TRIGGER_ENABLE, 1)
                        }))
                    {
                        notes.Add("Acquisition trigger external applied");
                        return true;
                    }
                    break;

                case TriggerMode.SingleFrame:
                    if (TrySetAcquisitionBoolPattern(
                        notes,
                        new[]
                        {
                            new ParameterWrite(SapAcquisition.Prm.INT_FRAME_TRIGGER_ENABLE, 1),
                            new ParameterWrite(SapAcquisition.Prm.CAM_TRIGGER_ENABLE, 1)
                        }))
                    {
                        notes.Add("Acquisition trigger single-frame applied");
                        return true;
                    }
                    break;
            }

            notes.Add("Acquisition trigger parameter not supported");
            return false;
        }

        private bool TryConfigureTriggerSelector(string selector, bool enabled, string source)
        {
            var selectorApplied = TrySetDeviceFeature(new[] { "TriggerSelector" }, selector);
            var modeApplied = TrySetDeviceFeature(new[] { "TriggerMode" }, enabled ? "On" : "Off");
            var sourceApplied = !enabled || string.IsNullOrWhiteSpace(source) || TrySetDeviceFeature(new[] { "TriggerSource" }, source);
            return selectorApplied && modeApplied && sourceApplied;
        }

        private bool TrySetDeviceFeature(string[] featureNames, double value, out string appliedFeature)
        {
            appliedFeature = string.Empty;
            if (!_deviceFeaturesAvailable || _acqDevice == null || !_acqDevice.Initialized)
            {
                return false;
            }

            foreach (var featureName in featureNames)
            {
                if (!_acqDevice.IsFeatureAvailable(featureName))
                {
                    continue;
                }

                if (TrySetFeatureValue(featureName, value))
                {
                    appliedFeature = featureName;
                    return true;
                }
            }

            return false;
        }

        private bool TrySetDeviceFeature(string[] featureNames, int value, out string appliedFeature)
        {
            appliedFeature = string.Empty;
            if (!_deviceFeaturesAvailable || _acqDevice == null || !_acqDevice.Initialized)
            {
                return false;
            }

            foreach (var featureName in featureNames)
            {
                if (!_acqDevice.IsFeatureAvailable(featureName))
                {
                    continue;
                }

                if (TrySetFeatureValue(featureName, value))
                {
                    appliedFeature = featureName;
                    return true;
                }
            }

            return false;
        }

        private bool TrySetAcquisitionIntParameter(System.Collections.Generic.List<string> notes, int value, params SapAcquisition.Prm[] parameters)
        {
            if (_acquisition == null || !_acquisition.Initialized)
            {
                return false;
            }

            var wasGrabbing = _transfer != null && _transfer.Initialized && _status.IsPreviewing;
            try
            {
                if (wasGrabbing)
                {
                    try
                    {
                        _transfer.Freeze();
                    }
                    catch
                    {
                    }
                }

                foreach (var parameter in parameters)
                {
                    try
                    {
                        if (_acquisition.SetParameter(parameter, value, true))
                        {
                            notes.Add(parameter + " applied requested=" + value + " readback=" + ReadAcquisitionIntParameter(parameter));
                            return true;
                        }
                    }
                    catch
                    {
                    }
                }

                return false;
            }
            finally
            {
                if (wasGrabbing)
                {
                    try
                    {
                        _transfer.Grab();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private bool TrySetAcquisitionBoolPattern(System.Collections.Generic.List<string> notes, ParameterWrite[] writes)
        {
            foreach (var write in writes)
            {
                if (TrySetAcquisitionIntParameter(notes, write.Value, write.Parameter))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TrySetDeviceFeature(string[] featureNames, string value)
        {
            if (!_deviceFeaturesAvailable || _acqDevice == null || !_acqDevice.Initialized)
            {
                return false;
            }

            foreach (var featureName in featureNames)
            {
                if (!CanWriteDeviceFeature(featureName))
                {
                    continue;
                }

                if (TrySetFeatureValue(featureName, value))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TrySetFeatureValue(string featureName, double value)
        {
            try
            {
                if (_acqDevice.SetFeatureValue(featureName, value))
                {
                    _acqDevice.UpdateFeaturesToDevice();
                    return true;
                }
            }
            catch
            {
            }

            try
            {
                if (_acqDevice.SetFeatureValue(featureName, (float)value))
                {
                    _acqDevice.UpdateFeaturesToDevice();
                    return true;
                }
            }
            catch
            {
            }

            if (Math.Abs(value - Math.Round(value)) < 0.000001)
            {
                var integralValue = Convert.ToInt64(Math.Round(value));
                if (integralValue >= int.MinValue && integralValue <= int.MaxValue)
                {
                    try
                    {
                        if (_acqDevice.SetFeatureValue(featureName, Convert.ToInt32(integralValue)))
                        {
                            _acqDevice.UpdateFeaturesToDevice();
                            return true;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return false;
        }

        private bool TrySetFeatureValue(string featureName, int value)
        {
            try
            {
                if (_acqDevice.SetFeatureValue(featureName, value))
                {
                    _acqDevice.UpdateFeaturesToDevice();
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        private bool TrySetFeatureValue(string featureName, string value)
        {
            try
            {
                if (_acqDevice.SetFeatureValue(featureName, value))
                {
                    _acqDevice.UpdateFeaturesToDevice();
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        private bool CanWriteDeviceFeature(string featureName)
        {
            if (string.IsNullOrWhiteSpace(featureName) || !_acqDevice.IsFeatureAvailable(featureName))
            {
                return false;
            }

            SapFeature feature = null;
            try
            {
                feature = new SapFeature(_acqDevice.Location);
                feature.Create();
                if (_acqDevice.GetFeatureInfo(featureName, feature))
                {
                    var accessMode = feature.DataAccessMode.ToString();
                    if (accessMode.IndexOf("Write", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        return false;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (feature != null)
                {
                    try
                    {
                        if (feature.Initialized)
                        {
                            feature.Destroy();
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        feature.Dispose();
                    }
                    catch
                    {
                    }
                }
            }

            return true;
        }

        private string ReadNumericFeatureValue(string featureName)
        {
            if (_acqDevice == null || !_acqDevice.Initialized || string.IsNullOrWhiteSpace(featureName))
            {
                return "<unavailable>";
            }

            try
            {
                double doubleValue;
                if (_acqDevice.GetFeatureValue(featureName, out doubleValue))
                {
                    return doubleValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch
            {
            }

            try
            {
                float floatValue;
                if (_acqDevice.GetFeatureValue(featureName, out floatValue))
                {
                    return floatValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch
            {
            }

            try
            {
                int intValue;
                if (_acqDevice.GetFeatureValue(featureName, out intValue))
                {
                    return intValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch
            {
            }

            return "<unreadable>";
        }

        private string ReadAcquisitionIntParameter(SapAcquisition.Prm parameter)
        {
            if (_acquisition == null || !_acquisition.Initialized)
            {
                return "<unavailable>";
            }

            try
            {
                int intValue;
                if (_acquisition.GetParameter(parameter, out intValue))
                {
                    return intValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch
            {
            }

            return "<unreadable>";
        }

        private string ReadFeatureValue(string featureName, SapFeature.Type dataType)
        {
            try
            {
                switch (dataType)
                {
                    case SapFeature.Type.Bool:
                        bool boolValue;
                        return _acqDevice.GetFeatureValue(featureName, out boolValue) ? boolValue.ToString() : "<unreadable>";
                    case SapFeature.Type.Int32:
                        int int32Value;
                        return _acqDevice.GetFeatureValue(featureName, out int32Value) ? int32Value.ToString() : "<unreadable>";
                    case SapFeature.Type.Int64:
                        long intValue;
                        return _acqDevice.GetFeatureValue(featureName, out intValue) ? intValue.ToString() : "<unreadable>";
                    case SapFeature.Type.Float:
                        float floatValue;
                        return _acqDevice.GetFeatureValue(featureName, out floatValue) ? floatValue.ToString() : "<unreadable>";
                    case SapFeature.Type.Double:
                        double doubleValue;
                        return _acqDevice.GetFeatureValue(featureName, out doubleValue) ? doubleValue.ToString() : "<unreadable>";
                    case SapFeature.Type.String:
                    case SapFeature.Type.Enum:
                        string textValue;
                        return _acqDevice.GetFeatureValue(featureName, out textValue) ? SafeString(textValue) : "<unreadable>";
                    default:
                        string fallbackValue;
                        return _acqDevice.GetFeatureValue(featureName, out fallbackValue) ? SafeString(fallbackValue) : "<unsupported>";
                }
            }
            catch (Exception ex)
            {
                return "<error: " + ex.Message + ">";
            }
        }

        private static string SafeString(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace("\r", " ").Replace("\n", " ");
        }

        private string ReadAcquisitionParameterValue(SapAcquisition.Prm parameter)
        {
            try
            {
                var parameterType = SapAcquisition.GetParameterType(parameter);
                switch (parameterType.ToString())
                {
                    case "Int32":
                    case "RangeInt32":
                    case "Index":
                    case "Enum":
                        int intValue;
                        return _acquisition.GetParameter(parameter, out intValue) ? intValue.ToString() : "<unreadable>";
                    case "Int64":
                    case "RangeInt64":
                        long longValue;
                        return _acquisition.GetParameter(parameter, out longValue) ? longValue.ToString() : "<unreadable>";
                    case "String":
                        string stringValue;
                        return _acquisition.GetParameter(parameter, out stringValue) ? SafeString(stringValue) : "<unreadable>";
                    default:
                        SapAcquisition.Val value;
                        return _acquisition.GetParameter(parameter, out value) ? value.ToString() : "<unreadable>";
                }
            }
            catch (Exception ex)
            {
                return "<error: " + ex.Message + ">";
            }
        }

        private struct ParameterWrite
        {
            public ParameterWrite(SapAcquisition.Prm parameter, int value)
            {
                Parameter = parameter;
                Value = value;
            }

            public SapAcquisition.Prm Parameter { get; private set; }

            public int Value { get; private set; }
        }
    }
}

using CameraCaptureApp.Models;
using DALSA.SaperaLT.SapClassBasic;
using DALSA.SaperaLT.SapClassGui;

namespace CameraCaptureApp.Services
{
    public class CameraService : ICameraService
    {
        private CameraSettings _settings;
        private readonly CameraStatus _status;
        private SapLocation _serverLocation;
        private string _configFileName;
        private SapAcquisition _acquisition;
        private SapBuffer _buffers;
        private SapAcqToBuf _transfer;

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
                ScanStateText = "待命",
                LastMessage = "Line-scan camera scaffold ready. SDK integration pending."
            };
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
            _status.LastMessage = "線掃描設定已套用。";
        }

        public bool Connect()
        {
            try
            {
                if (!EnsureConnectionSettings())
                {
                    _status.LastMessage = "已取消 Sapera 相機設定。";
                    return false;
                }

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
                    DestroySdkObjects();
                    DisposeSdkObjects();
                    _status.LastMessage = "Sapera 取像物件建立失敗。";
                    return false;
                }

                _status.IsConnected = true;
                _status.HasSignal = _acquisition.SignalStatus != SapAcquisition.AcqSignalStatus.None;
                _status.CameraName = _serverLocation.ServerName;
                _status.FrameWidth = _buffers.Width;
                _status.FrameHeight = _buffers.Height;
                _status.ScanStateText = "已連線";
                _status.LastMessage = "Sapera 線掃描相機已連線。";
                return true;
            }
            catch (System.Exception ex)
            {
                DestroySdkObjects();
                DisposeSdkObjects();
                _status.IsConnected = false;
                _status.HasSignal = false;
                _status.ScanStateText = "連線失敗";
                _status.LastMessage = "Sapera 連線失敗: " + ex.Message;
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
            _status.ScanStateText = "已斷線";
            _status.LastMessage = "線掃描相機已斷線。";
        }

        public bool StartPreview()
        {
            if (!_status.IsConnected || _transfer == null)
            {
                _status.LastMessage = "請先連線，再開始掃描。";
                return false;
            }

            if (_transfer.Grab())
            {
                _status.IsPreviewing = true;
                _status.ScanStateText = "掃描中";
                _status.LastMessage = "Sapera 掃描已開始。";
                return true;
            }

            _status.LastMessage = "Sapera 無法開始掃描。";
            return false;
        }

        public void StopPreview()
        {
            if (_transfer != null && _transfer.Initialized)
            {
                _transfer.Freeze();
            }

            _status.IsPreviewing = false;
            _status.ScanStateText = "已停止";
            _status.LastMessage = "Sapera 掃描已停止。";
        }

        public bool CaptureFrame()
        {
            if (!_status.IsConnected || _transfer == null)
            {
                _status.LastMessage = "請先連線，再執行單次擷取。";
                return false;
            }

            if (_transfer.Snap())
            {
                _status.ScanStateText = "單次擷取";
                _status.LastMessage = "Sapera 已觸發單次擷取。";
                return true;
            }

            _status.LastMessage = "Sapera 無法執行單次擷取。";
            return false;
        }

        private bool EnsureConnectionSettings()
        {
            if (!string.IsNullOrWhiteSpace(_settings.ServerName))
            {
                _serverLocation = new SapLocation(_settings.ServerName, _settings.ResourceIndex);
                _configFileName = _settings.ConfigFilePath;
                return true;
            }

            using (var dialog = new AcqConfigDlg(null, string.Empty, AcqConfigDlg.ServerCategory.ServerAcq))
            {
                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return false;
                }

                _serverLocation = dialog.ServerLocation;
                _configFileName = dialog.ConfigFile;
                _settings.ServerName = _serverLocation.ServerName;
                _settings.ResourceIndex = _serverLocation.ServerIndex;
                _settings.ConfigFilePath = _configFileName;
                _settings.CameraName = _serverLocation.ServerName;
                return true;
            }
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
            _status.ScanStateText = argsNotify.Trash ? "背景累積" : "更新中";
            _status.LastMessage = argsNotify.Trash
                ? "Sapera 正在背景累積線掃描資料。"
                : "Sapera 已收到新的取像更新。";
        }

        private void OnSignalNotify(object sender, SapSignalNotifyEventArgs argsSignal)
        {
            _status.HasSignal = argsSignal.SignalStatus != SapAcquisition.AcqSignalStatus.None;
            if (!_status.HasSignal)
            {
                _status.LastMessage = "Sapera 已連線，但目前沒有相機訊號。";
            }
        }
    }
}

using System;
using System.Drawing;

namespace CameraCaptureApp.Services
{
    public sealed class FrameRecorder : IDisposable
    {
        private readonly object _sync = new object();
        private Bitmap _latestFrame;

        public void StoreLatest(Bitmap frame)
        {
            if (frame == null)
            {
                return;
            }

            var copy = (Bitmap)frame.Clone();
            lock (_sync)
            {
                if (_latestFrame != null)
                {
                    _latestFrame.Dispose();
                }

                _latestFrame = copy;
            }
        }

        public Bitmap SnapshotLatest()
        {
            lock (_sync)
            {
                return _latestFrame == null ? null : (Bitmap)_latestFrame.Clone();
            }
        }

        public void Dispose()
        {
            lock (_sync)
            {
                if (_latestFrame != null)
                {
                    _latestFrame.Dispose();
                    _latestFrame = null;
                }
            }
        }
    }
}

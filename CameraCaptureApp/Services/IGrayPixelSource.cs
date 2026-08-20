using System;

namespace CameraCaptureApp.Services
{
    public interface IGrayPixelSource : IDisposable
    {
        int Width { get; }

        int Height { get; }

        int GetGrayAt(int x, int y);
    }
}

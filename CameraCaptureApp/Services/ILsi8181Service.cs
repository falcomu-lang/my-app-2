using System;
using System.Collections.Generic;

namespace CameraCaptureApp.Services
{
    public interface ILsi8181Service : IDisposable
    {
        bool IsInitialized { get; }

        int LastStatusCode { get; }

        string LastMessage { get; }

        IReadOnlyList<Lsi8181CardInfo> InitializeAndScanCards();

        int ReadCounter(byte cardId);

        void ClearCounter(byte cardId);

        void Close();
    }
}

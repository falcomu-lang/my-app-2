using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace CameraCaptureApp.Services
{
    public sealed class Lsi8181Service : ILsi8181Service
    {
        private bool _disposed;

        public bool IsInitialized { get; private set; }

        public int LastStatusCode { get; private set; }

        public string LastMessage { get; private set; }

        public IReadOnlyList<Lsi8181CardInfo> InitializeAndScanCards()
        {
            ThrowIfDisposed();

            if (!IsInitialized)
            {
                SetStatus(SafeCall(Lsi8181Native.LSI8181_initial), "LSI-8181 initialized.");
                EnsureSuccess("Initialize LSI-8181");
                IsInitialized = true;
            }

            var cards = new List<Lsi8181CardInfo>();
            for (byte cardId = 0; cardId <= Lsi8181Native.CardIdMax; cardId++)
            {
                ulong ioAddress = 0;
                ulong timerCounterAddress = 0;
                var status = SafeCall(() => Lsi8181Native.LSI8181_info(cardId, ref ioAddress, ref timerCounterAddress));
                if (status == Lsi8181Native.NoError)
                {
                    cards.Add(new Lsi8181CardInfo
                    {
                        CardId = cardId,
                        IoAddress = ioAddress,
                        TimerCounterAddress = timerCounterAddress
                    });
                }
            }

            SetStatus(Lsi8181Native.NoError, cards.Count + " LSI-8181 card(s) found.");
            return cards;
        }

        public int ReadCounter(byte cardId)
        {
            ThrowIfDisposed();
            EnsureInitialized();

            var value = 0;
            SetStatus(SafeCall(() => Lsi8181Native.LSI8181_counter_read(cardId, ref value)), "Counter read.");
            EnsureSuccess("Read LSI-8181 counter");
            return value;
        }

        public void ClearCounter(byte cardId)
        {
            ThrowIfDisposed();
            EnsureInitialized();

            SetStatus(SafeCall(() => Lsi8181Native.LSI8181_counter_set(cardId, 0)), "Counter cleared.");
            EnsureSuccess("Clear LSI-8181 counter");
        }

        public Lsi8181CounterInputMode ReadCounterInputMode(byte cardId)
        {
            ThrowIfDisposed();
            EnsureInitialized();

            byte inputMode = 0;
            byte debounceTime = 0;
            byte multipleRate = 0;
            SetStatus(
                SafeCall(() => Lsi8181Native.LSI8181_CI_mode_read(cardId, ref inputMode, ref debounceTime, ref multipleRate)),
                "Counter input mode read.");
            EnsureSuccess("Read LSI-8181 counter input mode");

            return new Lsi8181CounterInputMode
            {
                InputMode = inputMode,
                DebounceTime = debounceTime,
                MultipleRate = multipleRate
            };
        }

        public void SetMultipleRate(byte cardId, byte multipleRate)
        {
            ThrowIfDisposed();
            EnsureInitialized();

            var currentMode = ReadCounterInputMode(cardId);
            SetStatus(
                SafeCall(() => Lsi8181Native.LSI8181_CI_mode_set(
                    cardId,
                    currentMode.InputMode,
                    currentMode.DebounceTime,
                    multipleRate)),
                "Multiple rate set.");
            EnsureSuccess("Set LSI-8181 multiple rate");
        }

        public int ReadAutoIncrement(byte cardId)
        {
            ThrowIfDisposed();
            EnsureInitialized();

            var value = 0;
            SetStatus(
                SafeCall(() => Lsi8181Native.LSI8181_compare_increment_read(cardId, ref value)),
                "Auto increment read.");
            EnsureSuccess("Read LSI-8181 auto increment");
            return value;
        }

        public void ApplyAutoIncrementMode(byte cardId, int incrementValue)
        {
            ThrowIfDisposed();
            EnsureInitialized();

            SetStatus(
                SafeCall(() => Lsi8181Native.LSI8181_compare_increment_set(cardId, incrementValue)),
                "Auto increment value set.");
            EnsureSuccess("Set LSI-8181 auto increment value");

            SetStatus(
                SafeCall(() => Lsi8181Native.LSI8181_compare_mode_set(cardId, 2)),
                "Compare mode set to auto increment.");
            EnsureSuccess("Set LSI-8181 compare mode to auto increment");

            SetStatus(
                SafeCall(() => Lsi8181Native.LSI8181_counter_start(cardId, 2)),
                "Auto increment compare mode enabled.");
            EnsureSuccess("Enable LSI-8181 auto increment compare mode");
        }

        public void Close()
        {
            if (_disposed || !IsInitialized)
            {
                return;
            }

            SetStatus(SafeCall(Lsi8181Native.LSI8181_close), "LSI-8181 closed.");
            IsInitialized = false;
        }

        public void Dispose()
        {
            Close();
            _disposed = true;
        }

        private void EnsureInitialized()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("LSI-8181 is not initialized. Click Open / Scan first.");
            }
        }

        private void EnsureSuccess(string action)
        {
            if (LastStatusCode != Lsi8181Native.NoError)
            {
                throw new InvalidOperationException(action + " failed. Code #" + LastStatusCode + ".");
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private int SafeCall(Func<int> action)
        {
            try
            {
                return action();
            }
            catch (DllNotFoundException ex)
            {
                throw new FileNotFoundException("LSI8181_64.dll was not found beside the application executable.", ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new BadImageFormatException("LSI8181_64.dll does not match the application platform. The app must run as x64.", ex);
            }
            catch (Win32Exception ex)
            {
                throw new InvalidOperationException("LSI-8181 driver call failed: " + ex.Message, ex);
            }
        }

        private void SetStatus(int statusCode, string successMessage)
        {
            LastStatusCode = statusCode;
            LastMessage = statusCode == Lsi8181Native.NoError
                ? successMessage
                : "LSI-8181 Error (Code #" + statusCode + ")";
        }
    }
}

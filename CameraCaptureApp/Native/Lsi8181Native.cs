using System;
using System.Runtime.InteropServices;

namespace CameraCaptureApp.Native
{
    internal static class Lsi8181Native
    {
        public const uint Success = 0;
        public const byte CardIdMax = 15;
        public const byte QuadratureMode = 0;
        public const byte DebounceTime1Us = 1;
        public const byte Multiple4 = 0;
        public const byte CounterRun = 1;

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_initial();

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_close();

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_info(byte CardID, ref ulong IO_address, ref ulong TC_address);

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_CI_mode_set(byte CardID, byte in_mode, byte debounce_time, byte multiple_rate);

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_counter_set(byte CardID, int counter_value);

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_counter_read(byte CardID, ref int counter_value);

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_compare_value_set(byte CardID, int compare_value);

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_compare_value_read(byte CardID, ref int compare_value);

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_counter_start(byte CardID, byte mode);

        [DllImport("LSI8181_64.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern uint LSI8181_counter_stop(byte CardID);
    }
}

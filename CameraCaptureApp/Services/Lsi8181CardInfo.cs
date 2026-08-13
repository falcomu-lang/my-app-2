namespace CameraCaptureApp.Services
{
    public sealed class Lsi8181CardInfo
    {
        public byte CardId { get; set; }

        public ulong IoAddress { get; set; }

        public ulong TimerCounterAddress { get; set; }

        public override string ToString()
        {
            return "Card " + CardId + "  IO=0x" + IoAddress.ToString("X") + "  TC=0x" + TimerCounterAddress.ToString("X");
        }
    }
}

using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace NetUtils
{
    [PublicAPI]
    public static class ConsoleActions
    {
        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
        public static extern bool SetSignalHandler(SignalHandler handler, bool addHandler);
    }

    public delegate void SignalHandler(int consoleSignal);
}
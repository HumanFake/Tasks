using System;
using System.Runtime.InteropServices;
using NetUtils;

namespace TcpListener
{
    internal static class CliEntryPoint
    {
        private delegate void SignalHandler(int consoleSignal);

        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
        private static extern bool SetSignalHandler(SignalHandler handler, bool addHandler);

        private static void Main()
        {
            var port = Port.ReadPort();
            var address = NetIO.FindLocalIpAddressOrNull();

            try
            {
                if (address == null)
                {
                    throw new ArgumentException("локальный IP не найден");
                }
                Console.Clear();
                var server = new Server(port, address);

                SignalHandler signalHandler = unused =>
                {
                    server.StopListen();
                };
                SetSignalHandler(signalHandler, true);

                server.Listen();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

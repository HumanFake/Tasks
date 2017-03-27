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

        private static SignalHandler _signalHandler;

        private static void Main()
        {
            var port = NetIO.ReadPort();
            var address = NetIO.FindLocalIpAddress();

            try
            {
                var server = new Server(port, address);

                _signalHandler += unused =>
                {
                    server.StopListen();
                };
                SetSignalHandler(_signalHandler, true);

                server.Listen();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

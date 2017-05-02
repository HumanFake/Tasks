using System;
using System.Runtime.InteropServices;
using NetUtils;

namespace TudpServer
{
    static class ServerEntryPoint
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
                    throw new ArgumentException("local IP noy found");
                }
                Console.Clear();
                var server = new Server(port, address);

                void SignalHandler(int unused)
                {
                    server.StopListen();
                }
                SetSignalHandler(SignalHandler, true);

                server.Listen();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

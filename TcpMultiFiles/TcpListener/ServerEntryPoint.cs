using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal static class ServerEntryPoint
    {
        private delegate void SignalHandler(int consoleSignal);

        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
        private static extern bool SetSignalHandler(SignalHandler handler, bool addHandler);

        private static void Main()
        {
            var port = Port.ReadPort();
            var address = NetIo.FindLocalIpAddressOrNull();

            try
            {
                if (address == null)
                {
                    throw new Exception("Can't get local IP");
                }
                Console.Clear();
                using (var server = new Server(port, address))
                {
                    // Ситуация с тем, что у объекта уже вызвали Dispose обраатывается внутри метода
                    // ReSharper disable once AccessToDisposedClosure
                    SignalHandler signalHandler = unused => CloseServer(server);
                    SetSignalHandler(signalHandler, true);

                    server.Listen();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void CloseServer([NotNull] Server server)
        {
            try
            {
                server.ListenStop();
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}

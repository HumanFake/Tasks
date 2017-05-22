using System;
using System.Runtime.InteropServices;
using NetUtils;

namespace UdpSelfCounter
{
    internal static class CliEntryPoint
    {
        private delegate void SignalHandler(int consoleSignal);

        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
        private static extern bool SetSignalHandler(SignalHandler handler, bool addHandler);


        private static void Main()
        {
            var localPort = Port.ReadPort();
            try
            {
                using (var sender = new Sender(localPort))
                {
                    sender.Send(ProgramData.EntryMessage);

                    using (var receiver = new Receiver(sender, localPort))
                    {
                        // Ситуация с тем, что у объекта уже вызвали Dispose обраатывается внутри метода
                        // ReSharper disable once AccessToDisposedClosure
                        SignalHandler handler = unused => CloseServer(receiver);
                        SetSignalHandler(handler, true);

                        receiver.Listen();
                    }
                }
            }
            catch (ReceiveException)
            {
                Console.WriteLine("error when message is received");
            }
            catch (SenderException)
            {
                Console.WriteLine("error when message is sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CloseServer(Receiver receiver)
        {
            try
            {
                receiver.StopListen();
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}

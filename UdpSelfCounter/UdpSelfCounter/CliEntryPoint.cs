﻿using System;
using System.Runtime.InteropServices;
using NetUtils;

namespace UdpSelfCounter
{
    internal static class CliEntryPoint
    {
        private delegate void SignalHandler(int consoleSignal);

        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
        private static extern bool SetSignalHandler(SignalHandler handler, bool addHandler);

        private static SignalHandler _signalHandler;

        private static void Main()
        {
            var localPort = NetIO.ReadPort();

            try
            {
                using (var sender = new Sender(localPort))
                {
                    sender.Send(ProgramData.EntryMessage);

                    var receiver = new Receiver(sender, localPort);
                    _signalHandler += unused =>
                    {
                        receiver.StopListen();
                    };
                    SetSignalHandler(_signalHandler, true);

                    receiver.Listen();
                }
            }
            catch (ReceiveException)
            {
                Console.WriteLine("error when message is received");
            }
            catch (SenderException)
            {
                Console.WriteLine("error when message is sended");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;
using Timer = System.Timers.Timer;

namespace UdpSelfCounter
{
    internal sealed class Receiver : Disposable
    {
        private delegate void SignalHandler(int consoleSignal);

        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
        private static extern bool SetSignalHandler(SignalHandler handler, bool addHandler);

        private const int RecountTime = 2000;

        private readonly object _locker = new object();
        private readonly UdpClient _receiveClient;
        private readonly Sender _sendClient;
        private readonly List<IPAddress> _currentClients = new List<IPAddress>();
        private readonly Timer _recountTimer = new Timer(RecountTime);

        internal Receiver([NotNull] Sender sender, [NotNull] Port port)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            if (port == null)
            {
                throw new ArgumentNullException(nameof(port));
            }

            _currentClients.Add(NetIo.FindLocalIpAddressOrNull());
            _sendClient = sender;
            try
            {
                _receiveClient = new UdpClient(port.AtInt);
            }
            catch (Exception)
            {
                Console.WriteLine($"Can't star at {port.AtInt} port");
                try
                {
                    _receiveClient = new UdpClient(0);
                    var ipEndPoint = _receiveClient.Client.LocalEndPoint as IPEndPoint;
                    var currentPort = ipEndPoint?.Port;
                    Console.WriteLine($"Automatically chosen port: {currentPort}");
                }
                catch (Exception ex)
                {
                    throw new ReceiveException(ex.Message, ex);
                }
            }
            _receiveClient.JoinMulticastGroup(ProgramData.BroadcastAddress);

            _recountTimer.Elapsed += RecountDuplicates;
            _recountTimer.AutoReset = true;


            void Handler(int unused) => StopListen();
            SetSignalHandler(Handler, true);
        }

        internal void Listen()
        {
            try
            {
                _recountTimer.Start();
                lock (_locker)
                {
                    DisplayCopyCount(_currentClients.Count);
                }
                while (true)
                {
                    IPEndPoint remoteIpEndPoint = null;
                    var receiveBytes = _receiveClient.Receive(ref remoteIpEndPoint);
                    var message = new ReceiveMessage(receiveBytes, remoteIpEndPoint);
                    lock (_locker)
                    {
                        var currentCopyCount = _currentClients.Count;
                        AnalyzeMessage(message);
                        if (currentCopyCount != _currentClients.Count)
                        {
                            DisplayCopyCount(_currentClients.Count);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ReceiveException(ex.Message, ex.InnerException);
            }
        }

        private void StopListen()
        {
            _recountTimer.Stop();
            _sendClient.Send(ProgramData.OutMessage);
            _receiveClient.Client.Shutdown(SocketShutdown.Both);
        }

        private void RecountDuplicates([NotNull] object unused, [NotNull] ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                lock (_locker)
                {
                    _currentClients.Clear();
                    _currentClients.Add(NetIo.FindLocalIpAddressOrNull());
                }
                _sendClient.Send(ProgramData.EntryMessage);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error in recounting clients");
            }
        }

        private void DisplayCopyCount(int copyCount)
        {
            Console.Clear();
            Console.WriteLine($"{nameof(UdpSelfCounter)} count: " + copyCount);
        }

        private void AnalyzeMessage([NotNull] ReceiveMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            #if DEBUG
            Console.WriteLine($"New message {message.Body} from {message.Address}");
            #endif
            if (ProgramData.AnswerMessage == message.Body)
            {
                if (false == _currentClients.Contains(message.Address))
                {
                    _currentClients.Add(message.Address);
                }
            }
            if (ProgramData.EntryMessage == message.Body)
            {
                if (false == _currentClients.Contains(message.Address))
                {
                    _currentClients.Add(message.Address);
                }
                _sendClient.Send(ProgramData.AnswerMessage);
            }
            if (ProgramData.OutMessage == message.Body)
            {
                _currentClients.Remove(message.Address);
            }
        }

        protected override void FreeManagedResources()
        {
            _recountTimer.Close();
            _receiveClient.Close();
        }
    }
}
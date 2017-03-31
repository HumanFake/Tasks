using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;
using Timer = System.Timers.Timer;

namespace UdpSelfCounter
{
    internal class Receiver
    {
        private const int RecountTime = 2000;

        private readonly object _locker = new object();
        private readonly UdpClient _receiveClient;
        private readonly Sender _sendClient;
        private readonly List<IPAddress> _currentClients = new List<IPAddress>();
        private readonly Timer _recountTimer = new Timer(RecountTime);

        internal Receiver([NotNull] Sender sender, Port port)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            _currentClients.Add(NetIO.FindLocalIpAddressOrNull());
            _sendClient = sender;
            try
            {
                _receiveClient = new UdpClient(port.GetPort);
            }
            catch (Exception)
            {
                Console.WriteLine($"Не удалось запустить приложение под портом: {port.GetPort}");
                try
                {
                    _receiveClient = new UdpClient(0);
                    var ipEndPoint = _receiveClient.Client.LocalEndPoint as IPEndPoint;
                    var currentPort = ipEndPoint?.Port;
                    Console.WriteLine($"Автоматически выбран порт: {currentPort}");
                }
                catch (Exception ex)
                {
                    throw new ReceiveException(ex.Message, ex);
                }
            }
            _receiveClient.JoinMulticastGroup(ProgramData.BroadcasAddress);

            _recountTimer.Elapsed += RecountDuplicates;
            _recountTimer.AutoReset = true;
        }

        internal void Listen()
        {
            try
            {
                _recountTimer.Start();

                DisplayCopyCount(_currentClients.Count);
                while (true)
                {
                    IPEndPoint remoteIpEndPoint = null;
                    var receiveBytes = _receiveClient.Receive(ref remoteIpEndPoint);
                    var message = new ReceiveMessage(receiveBytes, remoteIpEndPoint);

                    var currentCopyCount = _currentClients.Count;
                    AnalyzeMessage(message);
                    if (currentCopyCount != _currentClients.Count)
                    {
                        DisplayCopyCount(_currentClients.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ReceiveException(ex.Message, ex.InnerException);
            }
            finally
            {
                _receiveClient?.Close();
                _recountTimer?.Close();
            }
        }

        internal void StopListen()
        {
            _sendClient?.Send(ProgramData.OutMessage);
            _receiveClient?.Close();
        }

        private void RecountDuplicates(object unused, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                lock (_locker)
                {
                    _currentClients.Clear();
                    _currentClients.Add(NetIO.FindLocalIpAddressOrNull());
                    _sendClient.Send(ProgramData.EntryMessage);
                }
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
    }
}
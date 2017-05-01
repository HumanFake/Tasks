using System;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    public class TudpListener : Disposable
    {
        private const int LastMessageCount = 5;
        private const int ConnectionMessageCount = 5;
        private readonly UdpClient _udpClient;
        private byte[] _currentIdentifier;

        public TudpListener([NotNull]Port port, [NotNull] IPAddress address)
        {
            if (port == null)
            {
                throw new ArgumentNullException(nameof(port));
            }
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            _udpClient = new UdpClient(new IPEndPoint(address, port.AtInt));
        }

        public IPAddress WaitConnection()
        {
            IPEndPoint remoteIpEndPoint = null;

            while (true)
            {
                var receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
                var tudpData = new TudpData(receiveBytes);
                if (tudpData.IsConnectionMessage())
                {
                    for (int i = 0; i < ConnectionMessageCount; i++)
                    {
                        _udpClient.Send(TudpData.ConnectionMessage, TudpData.ConnectionMessage.Length, remoteIpEndPoint);
                    }
                    return remoteIpEndPoint.Address;
                }
            }
        }

        public byte[] Read()
        {
            try
            {
                while (true)
                {
                    IPEndPoint remoteIpEndPoint = null;

                    var receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
                    var tudpData = new TudpData(receiveBytes);
                    var receivedDatagramIdentifier = tudpData.GetIdentifier();

                    if (tudpData.IsLastMessage())
                    {
                        for (int i = 0; i < LastMessageCount; i++)
                        {
                            _udpClient.Send(TudpData.LastMessage, TudpData.LastMessage.Length, remoteIpEndPoint);
                        }
                        break;
                    }

                    _udpClient.Send(receivedDatagramIdentifier, receivedDatagramIdentifier.Length, remoteIpEndPoint);

                    if (_currentIdentifier == null || false == tudpData.CompareIdentifier(_currentIdentifier))
                    {
                        _currentIdentifier = receivedDatagramIdentifier;
                        return tudpData.GetDatagram();
                    }
                }
                return new byte[0];
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw new Exception();
            }
        }

        protected override void FreeManagedResources()
        {
            _udpClient.Close();
        }
    }
}
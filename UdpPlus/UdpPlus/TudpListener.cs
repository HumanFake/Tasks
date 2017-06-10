using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        private const int ClientReceiveTimeout = 50;

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
            _udpClient.Client.ReceiveTimeout = ClientReceiveTimeout;
        }

        public IPAddress GetConnectionAddressOrNull(CancellationToken cancellationToken)
        {
            IPEndPoint remoteIpEndPoint = null;

            while (true)
            {
                byte[] receiveBytes = null;
                while (false == cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
                        if (receiveBytes.Length != 0)
                        {
                            break;
                        }
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode != SocketError.TimedOut)
                        {
                            throw new SocketException(e.ErrorCode);
                        }
                    }
                }
                if (receiveBytes == null)
                {
                    return null;
                }
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
            ThrowIfDisposed();
            Console.Out.WriteLine("1");
            _udpClient.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    public class TudpListener : Disposable
    {
        private readonly UdpClient _udpClient;
        private byte[] _currentIdentifer;

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
                if (tudpData.IsConnectionMessege())
                {
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
                    var reveivedDategrammIdentifer = tudpData.GetIdentifer();

                    if (tudpData.IsLastMessege())
                    {
                        _udpClient.Send(TudpData.LastMessege, TudpData.LastMessege.Length, remoteIpEndPoint);
                        break;
                    }
                    _udpClient.Send(reveivedDategrammIdentifer, reveivedDategrammIdentifer.Length, remoteIpEndPoint);
                    if (_currentIdentifer == null || false == tudpData.CompareIdentifer(_currentIdentifer))
                    {
                        _currentIdentifer = reveivedDategrammIdentifer;
                        return tudpData.GetDategramm();
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
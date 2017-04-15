using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    public class UdpPListener : Disposable
    {
        private readonly UdpClient _udpClient;

        public UdpPListener([NotNull]Port port, [NotNull] IPAddress address)
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

        public byte[] Receive()
        {
            byte[] dategrammIdentifer = null;
            var reciveData = new List<byte>();
            while (true)
            {
                IPEndPoint remoteIpEndPoint = null;
                var receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
                var ud = new TudpData(receiveBytes);
                var reveivedategrammIdentifer = ud.GetIdentifer();
                if (false == reveivedategrammIdentifer.Equals(dategrammIdentifer))
                {
                    reciveData.AddRange(ud.GetDategramm());
                    dategrammIdentifer = reveivedategrammIdentifer;
                }
                if (reveivedategrammIdentifer.Equals(TudpData.LastMessege))
                {
                    break;
                }
                _udpClient.Send(reveivedategrammIdentifer, reveivedategrammIdentifer.Length, remoteIpEndPoint);
            }

            return reciveData.ToArray();
        }

        protected override void FreeManagedResources()
        {
            _udpClient?.Close();
        }
    }
}
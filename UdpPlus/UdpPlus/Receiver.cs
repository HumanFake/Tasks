using System;
using System.Net;
using System.Net.Sockets;

namespace UdpPlus
{
    internal class Receiver
    {
        internal delegate void ReceiveHendler(TudpData bytes);

        internal event ReceiveHendler NewMessege;

        private readonly UdpClient _client;

        internal Receiver(UdpClient client)
        {
            _client = client;
        }

        internal void Listen()
        {
            while (true)
            {
                try
                {
                    IPEndPoint remoteIpEndPoint = null;
                    var receiveBytes = _client.Receive(ref remoteIpEndPoint);
                    var tudpData = new TudpData(receiveBytes);
                    NewMessege?.Invoke(tudpData);
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
    }
}
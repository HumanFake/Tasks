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
                IPEndPoint remoteIpEndPoint = null;
                var receiveBytes = _client.Receive(ref remoteIpEndPoint);
                var ud = new TudpData(receiveBytes);
                NewMessege?.Invoke(ud);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    public sealed class TudpClient : Disposable
    {
        private readonly UdpClient _udpClient;

        public TudpClient([NotNull] Port port, [NotNull] IPAddress address)
        {
            if (port == null)
            {
                throw new ArgumentNullException(nameof(port));
            }
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            _udpClient = new UdpClient(new IPEndPoint(address, port.GetPort));
        }

        public void Send([NotNull] byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            var receiver = new Receiver(_udpClient);
            var task = new Task(receiver.Listen);
            task.Start();

            var index = 1;
            var currentPosition = 0;

            while (currentPosition != bytes.Length - 1)
            {
                var identifer = BitConverter.GetBytes(index);
                var buffer = new List<byte>();
                buffer.AddRange(identifer);
                for (int i = 0; i < TudpUtils.MaxDaitagrammBytes; i++)
                {
                    if (currentPosition == bytes.Length - 1)
                    {
                        break;
                    }
                    buffer.Add(bytes[currentPosition]);
                    currentPosition++;
                }
                _udpClient.Send(buffer.ToArray(), buffer.Count);
                receiver.NewMessege += udpPData =>
                {
                    if (udpPData.GetIdentifer().Equals(identifer))
                    {
                        index++;
                    }
                };
            }
        }

        protected override void FreeManagedResources()
        {
            _udpClient?.Close();
        }
    }

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
            _udpClient = new UdpClient(new IPEndPoint(address, port.GetPort));
        }

        public void Receive()
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
        }

        protected override void FreeManagedResources()
        {
            _udpClient?.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    public class UdpPClient : Disposable
    {
        private readonly UdpClient _udpClient;

        public UdpPClient([NotNull] Port port, [NotNull] IPAddress address)
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
            var index = 1;
            var currentPosition = 0;

            var identifer = BitConverter.GetBytes(index);
            var buffer = new List<byte>();
            buffer.AddRange(identifer);
            for (int i = 0; i < UdpPUtils.MaxDaitagrammBytes; i++)
            {
                if (currentPosition == bytes.Length - 1)
                {
                    break;
                }
                buffer.Add(bytes[currentPosition]);
                currentPosition++;
            }
            _udpClient.Send(buffer.ToArray(), buffer.Count);

            IPEndPoint remoteIpEndPoint = null;
            var receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
            var ud = new UdpPData(receiveBytes);
            var receiveIdentifer = ud.GetIdentifer();

        }

        protected override void FreeManagedResources()
        {
            _udpClient?.Close();
        }
    }

    internal class Receiver
    {
        internal delegate void ReceiveHendler(byte[] bytes);

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
                var ud = new UdpPData(receiveBytes);
                var receiveIdentifer = ud.GetIdentifer();
                NewMessege?.Invoke(receiveIdentifer);
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
            byte[]  dategrammIdentifer = null;
            var reciveData = new List<byte>();
            while (true)
            {
                IPEndPoint remoteIpEndPoint = null;
                var receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
                var ud = new UdpPData(receiveBytes);
                var reveivedategrammIdentifer = ud.GetIdentifer();
                if (false == reveivedategrammIdentifer.Equals(dategrammIdentifer))
                {
                    reciveData.AddRange(ud.GetDategramm());
                    dategrammIdentifer = reveivedategrammIdentifer;
                }
                _udpClient.Send(reveivedategrammIdentifer, reveivedategrammIdentifer.Length, remoteIpEndPoint);
            }
        }

        protected override void FreeManagedResources()
        {
            _udpClient?.Close();
        }
    }

    internal class UdpPData
    {
        private readonly byte[] _identifer;
        private readonly List<byte> _data = new List<byte>();

        internal UdpPData([NotNull] byte[] dataWithIdentifer)
        {
            var identiferBytes = new byte[UdpPUtils.IdentiferByteCount];
            for (int i = 0; i < identiferBytes.Length; i++)
            {
                identiferBytes[i] = dataWithIdentifer[i];
            }
            _identifer = identiferBytes;

            for (int i = UdpPUtils.IdentiferByteCount; i < dataWithIdentifer.Length; i++)
            {
                _data.Add(dataWithIdentifer[i]);
            }
        }

        internal byte[] GetDategramm()
        {
            return _data.ToArray();
        }

        internal byte[] GetIdentifer()
        {
            return _identifer;
        }
    }
}

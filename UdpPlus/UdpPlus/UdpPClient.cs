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
        }

        protected override void FreeManagedResources()
        {
            _udpClient?.Close();
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
            while (true)
            {
                IPEndPoint remoteIpEndPoint = null;
                var receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
                var ud = new UdpPData(receiveBytes);
            }
        }

        protected override void FreeManagedResources()
        {
            _udpClient?.Close();
        }
    }

    internal class UdpPData
    {
        private readonly int _identifer;
        private readonly List<byte> _data = new List<byte>();

        internal UdpPData([NotNull] byte[] dataWithIdentifer)
        {
            var identiferBytes = new byte[UdpPUtils.IdentiferByteCount];
            for (int i = 0; i < identiferBytes.Length; i++)
            {
                identiferBytes[i] = dataWithIdentifer[i];
            }
            _identifer = BitConverter.ToInt32(identiferBytes, 0);

            for (int i = UdpPUtils.IdentiferByteCount; i < dataWithIdentifer.Length; i++)
            {
                _data.Add(dataWithIdentifer[i]);
            }
        }

        internal byte[] GetDategramm([NotNull] UdpPData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return data._data.ToArray();
        }

        internal int GetIdentifer([NotNull] UdpPData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return data._identifer;
        }
    }
}

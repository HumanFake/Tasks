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

        public TudpClient([NotNull] IPEndPoint address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            _udpClient = new UdpClient(address);
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
                for (int i = 0; i < TudpUtils.MaxDaitagramByteCount; i++)
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
}

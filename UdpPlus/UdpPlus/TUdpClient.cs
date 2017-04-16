using System;
using System.IO;
using System.Linq;
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
            _udpClient = new UdpClient();
            _udpClient.Connect(address);
        }

        public void SendFile([NotNull] Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            var receiver = new Receiver(_udpClient);
            var task = new Task(receiver.Listen);
            task.Start();

            var index = 1;
            receiver.NewMessege += udpPData =>
            {
                var receivedIndetifer = BitConverter.ToInt32(udpPData.GetIdentifer(), 0);
                if (receivedIndetifer.Equals(index))
                {
                    index++;
                }
                if (receivedIndetifer.Equals(TudpData.LastMessegeIndetifer))
                {
                    index = 0;
                }
            };

            while (true)
            {
                var currentIndex = index;
                var identifer = BitConverter.GetBytes(currentIndex);
                var buffer = new byte[TudpUtils.MaxDaitagramByteCount];
                var readBytes = stream.Read(buffer, 0, buffer.Length);

                var messege = new byte[identifer.Length + readBytes];
                for (int i = 0; i < identifer.Length; i++)
                {
                    messege[i] = identifer[i];
                }
                for (int i = 0; i < readBytes; i++)
                {
                    messege[i + identifer.Length] = buffer[i];
                }

                if (readBytes == 0)
                {
                    while (index != 0)
                    {
                        _udpClient.Send(TudpData.LastMessege, TudpData.LastMessege.Length);
                    }
                    return;
                }
                while (currentIndex == index)
                {
                    _udpClient.Send(messege, messege.Length);
                }
            }
        }

        protected override void FreeManagedResources()
        {
            _udpClient.Close();
        }
    }
}

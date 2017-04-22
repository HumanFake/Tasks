using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    public sealed class TudpClient : Disposable
    {
        private readonly UdpClient _udpClient;
        private const int ResendMessegeTime = 1;

        public TudpClient([NotNull] IPEndPoint address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            _udpClient = new UdpClient();
            _udpClient.Connect(address);
        }

        public void Connect()
        {
            _udpClient.Send(TudpData.ConnectionMessege, TudpData.ConnectionMessege.Length);
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
                if (udpPData.CompareIdentifer(index))
                {
                    index++;
                }
                if (udpPData.IsLastMessege())
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
                        Thread.Sleep(ResendMessegeTime);
                    }
                    return;
                }
                while (currentIndex == index)
                {
                    _udpClient.Send(messege, messege.Length);
                    Thread.Sleep(ResendMessegeTime);
                }
            }
        }

        protected override void FreeManagedResources()
        {
            _udpClient.Close();
        }
    }
}

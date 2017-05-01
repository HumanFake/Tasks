using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    public sealed class TudpClient : Disposable
    {
        private readonly UdpClient _udpClient;
        private const int MaxSendTryCount = 10;
        private const int ReceiveTimeout = 1000;

        public TudpClient([NotNull] IPEndPoint address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            _udpClient = new UdpClient
            {
                Client = {ReceiveTimeout = ReceiveTimeout}
            };
            _udpClient.Connect(address);
        }

        public void Connect()
        {
            var answer = Send(TudpData.ConnectionMessage, TudpData.ConnectionMessage.Length);
            if (false == answer.IsConnectionMessage())
            {
                throw new InvalidDatagremException();
            }
        }

        public void SendFile([NotNull] Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var index = 1;
            while (true)
            {
                var identifier = BitConverter.GetBytes(index);
                var buffer = new byte[TudpUtils.MaxDatagramByteCount];
                var readBytes = stream.Read(buffer, 0, buffer.Length);

                var message = new byte[identifier.Length + readBytes];
                for (int i = 0; i < identifier.Length; i++)
                {
                    message[i] = identifier[i];
                }
                for (int i = 0; i < readBytes; i++)
                {
                    message[i + identifier.Length] = buffer[i];
                }

                if (readBytes == 0)
                {
                    var sendTryCount = 0;
                    while (true)
                    {
                        _udpClient.Send(TudpData.LastMessage, TudpData.LastMessage.Length);
                        sendTryCount++;
                        var answer = WaitAnswer();
                        if (answer != null && answer.IsLastMessage())
                        {
                            break;
                        }
                        if (sendTryCount == MaxSendTryCount)
                        {
                            throw new TimeoutReceiveException();
                        }
                    }
                    return;
                }
                while (true)
                {
                    var answer = Send(message, message.Length);
                    if (answer.CompareIdentifier(index))
                    {
                        index++;
                        break;
                    }
                }
            }
        }

        private TudpData Send(byte[] bytes, int bytesCount)
        {
            var sendTryCount = 0;
            while (true)
            {
                _udpClient.Send(bytes, bytesCount);
                sendTryCount++;

                var answer = WaitAnswer();
                if (answer != null)
                {
                    return answer;
                }

                if (sendTryCount == MaxSendTryCount)
                {
                    throw new TimeoutReceiveException();
                }
            }
        }

        [CanBeNull]
        private TudpData WaitAnswer()
        {
            while (true)
            {
                try
                {
                    TudpData tudpData = null;
                    IPEndPoint remoteIpEndPoint = null;
                    var receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
                    if (receiveBytes.Length != 0)
                    {
                        tudpData = new TudpData(receiveBytes);
                    }
                    return tudpData;
                }
                catch (Exception)
                {
                    break;
                }
            }
            return null;
        }

        protected override void FreeManagedResources()
        {
            _udpClient.Close();
        }
    }
}

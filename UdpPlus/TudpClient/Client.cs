using System;
using System.IO;
using System.Net;
using JetBrains.Annotations;
using NetUtils;

using net = System.Net.Sockets;
namespace TudpClient
{
    internal sealed class Client : Disposable
    {
        private readonly net.TcpClient _client;
        private readonly UdpPlus.TudpClient _clientN;
        private readonly net.NetworkStream _stream;

        internal Client([NotNull] IPEndPoint address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            try
            {
                _client = new net.TcpClient();
                _client.Connect(address);
                _stream = _client.GetStream();

                _clientN = new UdpPlus.TudpClient(address);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error when creating a {nameof(Client)}", exception.InnerException);
            }
        }

        internal void Send([NotNull] Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            try
            {
                var buffer = new byte[1024*1024];
                while (true)
                {
                    var data = stream.Read(buffer, 0, buffer.Length);
                    if (data == 0)
                    {
                        break;
                    }
                    _clientN.Send(buffer);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error while sending stream", exception.InnerException);
            }
        }

        protected override void FreeManagedResources()
        {
            _stream?.Close();
            _client?.Close();
        }
    }
}
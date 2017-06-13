using System;
using System.IO;
using System.Net;
using JetBrains.Annotations;
using NetUtils;
using TcpClient.Exceptions;
using net = System.Net.Sockets;

namespace TcpClient
{
    internal sealed class Client : Disposable
    {
        private readonly net.TcpClient _client;
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
            }
            catch (Exception exception)
            {
                throw new CreateClientException($"Error when creating a {nameof(Client)}", exception.InnerException);
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
                stream.CopyTo(_stream);
            }
            catch (Exception exception)
            {
                throw new SendDataException("Error while sending stream", exception.InnerException);
            }
        }

        protected override void FreeManagedResources()
        {
            _stream.Close();
            _client.Close();
        }
    }
}
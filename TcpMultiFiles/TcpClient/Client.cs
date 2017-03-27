using System;
using System.Net;
using JetBrains.Annotations;
using TcpClient.Exceptions;
using net = System.Net.Sockets;

namespace TcpClient
{
    public class Client : IDisposable
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

        internal void Send([NotNull] byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            try
            {
                _stream.Write(data, 0, data.Length);
            }
            catch (Exception exception)
            {
                throw new SendDataException("Error while sending data", exception.InnerException);
            }
        }

        public void Dispose()
        {
            _stream.Close();
            _client.Close();
        }
    }
}
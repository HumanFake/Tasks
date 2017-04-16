using System;
using System.IO;
using System.Net;
using JetBrains.Annotations;
using NetUtils;

namespace TudpClient
{
    internal sealed class Client : Disposable
    {
        private readonly UdpPlus.TudpClient _client;

        internal Client([NotNull] IPEndPoint address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            try
            {
                _client = new UdpPlus.TudpClient(address);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error when creating a {nameof(Client)}", exception.InnerException);
            }
        }

        internal void Send([NotNull] FileStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            try
            {
                _client.SendFile(stream);
            }
            catch (Exception exception)
            {
                throw new Exception("Error while sending stream", exception.InnerException);
            }
        }

        protected override void FreeManagedResources()
        {
            _client.Dispose();
        }
    }
}
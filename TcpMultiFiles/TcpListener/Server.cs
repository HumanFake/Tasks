using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal sealed class Server : Disposable
    {
        private readonly System.Net.Sockets.TcpListener _server;

        internal Server([NotNull] Port port, [NotNull] IPAddress address)
        {
            if (port == null)
            {
                throw new ArgumentNullException(nameof(port));
            }
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            try
            {
                _server = new System.Net.Sockets.TcpListener(address, port.AtInt);
            }
            catch (Exception)
            {
                Console.WriteLine($"Can't star at: {port.AtInt} port");
                try
                {
                    _server = new System.Net.Sockets.TcpListener(address, 0);
                    var ipEndPoint = _server.Server.LocalEndPoint as IPEndPoint;
                    Console.WriteLine($"Automatically chosen port: {ipEndPoint?.Port}");
                }
                catch (Exception e)
                {
                    throw new ServerException(e);
                }
            }
        }
        
        internal void Listen()
        {
            try
            {
                _server.Start();
                while (true)
                {
                    Console.WriteLine("Connection waiting... ");
                    var tcpClient = _server.AcceptTcpClient();
                    var responseClient = new Receiver(tcpClient);

                    var clientThread = new Thread(() => GetResponse(responseClient));
                    clientThread.Start();
                }
            }
            catch (SocketException e)
            {
                #if DEBUG
                Console.Out.WriteLine(e);
                #endif
            }
            catch (Exception e)
            {
                throw new ServerException(e);
            }
        }

        internal void ListenStop()
        {
            _server.Stop();
        }
        
        private static void GetResponse([NotNull] Receiver receiver)
        {
            try
            {
                using (receiver)
                {
                    receiver.ReceiveData();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error data receive.");
            }
        }

        protected override void FreeManagedResources()
        {
            _server.Stop();
        }
    }
}
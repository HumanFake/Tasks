using System;
using System.Net;
using System.Threading;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal class Server
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
                _server = new System.Net.Sockets.TcpListener(address, port.GetPort);
            }
            catch (Exception)
            {
                Console.WriteLine($"Can't star at: {port.GetPort} port");
                try
                {
                    _server = new System.Net.Sockets.TcpListener(address, 0);
                    var ipEndPoint = _server.Server.LocalEndPoint as IPEndPoint;
                    Console.WriteLine($"Automatically chousen port: {ipEndPoint?.Port}");
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
                    var responseClient = new ResponseClient(tcpClient);

                    var clientThread = new Thread(() => GetResponse(responseClient));
                    clientThread.Start();
                }
            }
            catch (Exception e)
            {
                throw new ServerException(e);
            }
            finally
            {
                _server?.Stop();
            }
        }

        internal void ListenStop()
        {
            try
            {
                _server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private static void GetResponse([NotNull] ResponseClient responseClient)
        {
            try
            {
                using (responseClient)
                {
                    responseClient.ResponceMessage();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error data recive.");
            }
        }
    }
}
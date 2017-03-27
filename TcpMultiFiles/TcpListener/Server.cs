using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal class Server : Disposable
    {
        private const int BufferSize = 1024 * 1024;
        private readonly System.Net.Sockets.TcpListener _server;

        internal Server(int port, [NotNull] IPAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            _server = new System.Net.Sockets.TcpListener(address, port);
        }
        
        internal void Listen()
        {
            try
            {
                _server.Start();
                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");
                    var tcpClient = _server.AcceptTcpClient();

                    var clientThread = new Thread(() => GetResponse(tcpClient));
                    clientThread.Start();
                }
            }
            catch (Exception e)
            {
                throw new ServerException(e);
            }
            finally
            {
                Dispose();
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
        
        private static void GetResponse(TcpClient tcpClient)
        {
            try
            {
                using (tcpClient)
                {
                    long byteCount;
                    using (var networkStream = tcpClient.GetStream())
                    {
                        Console.WriteLine("Подключен клиент. Выполнение запроса...");

                        byteCount = 0;
                        var buffer = new byte[BufferSize];
                        while (true)
                        {
                            var readedBytes = networkStream.Read(buffer, 0, buffer.Length);
                            if (readedBytes == 0)
                            {
                                break;
                            }
                            byteCount += readedBytes;
                        }
                    }
                    Console.WriteLine($"Получено {byteCount}");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка во время получения данных");
            }
        }

        protected override void FreeManagedResources()
        {
            _server?.Stop();
        }
    }
}
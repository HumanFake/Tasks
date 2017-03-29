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

        internal Server(Port port, [NotNull] IPAddress address)
        {
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
                Console.WriteLine($"Не удаётся запустить сервер с протом: {port.GetPort}");
                try
                {
                    _server = new System.Net.Sockets.TcpListener(address, 0);
                    var ipEndPoint = _server.Server.LocalEndPoint as IPEndPoint;
                    Console.WriteLine($"Порт выбран автоматически: {ipEndPoint?.Port}");
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
                    Console.WriteLine("Ожидание подключений... ");
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
        
        private static void GetResponse(ResponseClient responseClient)
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
                Console.WriteLine("Ошибка во время получения данных");
            }
        }

        protected override void FreeManagedResources()
        {
            _server?.Stop();
        }

        private class ResponseClient : Disposable
        {
            private readonly TcpClient _tcpClient;

            internal ResponseClient(TcpClient tcpClient)
            {
                _tcpClient = tcpClient;
            }

            internal void ResponceMessage()
            {
                long byteCount;
                var ipEndPoint = (IPEndPoint) _tcpClient.Client.LocalEndPoint;
                using (var networkStream = _tcpClient.GetStream())
                {
                    Console.WriteLine($"Подключен клиент {ipEndPoint.Address}.\nВыполнение запроса...");

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
                Console.WriteLine($"Приём данных с {ipEndPoint.Address} завершён. Получено {byteCount} байт.");
            }

            void ClearLine(int line)
            {
                Console.MoveBufferArea(0, line, Console.BufferWidth, 1, Console.BufferWidth, line, ' ', Console.ForegroundColor, Console.BackgroundColor);
            }

            protected override void FreeManagedResources()
            {
                _tcpClient?.Close();
            }
        }
    }
}
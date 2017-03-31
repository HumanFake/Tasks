using System;
using System.Diagnostics;
using System.Net;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal class Server : Disposable
    {
        private const int BufferSize = 1024;
        private const int Time = 500;

        private readonly System.Net.Sockets.TcpListener _server;
        private readonly Timer _timer = new Timer(Time);

        internal Server(int port, [NotNull] IPAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            _server = new System.Net.Sockets.TcpListener(address, port);
            _timer.Enabled = true;
        }

        internal void Listen()
        {
            try
            {
                _server.Start();

                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");

                    using (var client = _server.AcceptTcpClient())
                    {
                        Console.WriteLine("Подключен клиент. Выполнение запроса...");

                        long momentByteCount = 0;
                        var stream = client.GetStream();

                        var time = Stopwatch.StartNew();

                        long byteCount = 0;
                        var buffer = new byte[BufferSize];
                        _timer.Elapsed += (sender, args) =>
                        {
                            Console.Out.WriteLine($"{momentByteCount.BytesToMegaBytes() * 2}");
                            momentByteCount = 0;
                        };
                        _timer.Start();

                        while (true)
                        {
                            var readedBytes = stream.Read(buffer, 0, buffer.Length);
                            if (readedBytes == 0)
                            {
                                break;
                            }
                            byteCount += readedBytes;
                            momentByteCount += readedBytes;
                        }

                        DisplayResult(byteCount, time.ElapsedMilliseconds);
                        _timer.Stop();
                    }
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

        internal void StopListen()
        {
            _server.Stop();
        }

        private static void DisplayResult(long byteCount, long milliseconds)
        {
            Console.WriteLine($"Байт принято: {byteCount}");
            Console.WriteLine($"Общее время мс: {milliseconds}");
            Console.WriteLine(milliseconds <= 0
                ? "Данные пряняты быстрее чем за 1ну миллисекунду."
                : $"Средняя скорость: {(byteCount.BytesToMegaBytes() / milliseconds.MillisecondToSecond())} MB/c");
        }

        protected override void FreeManagedResources()
        {
            _server?.Stop();
        }
    }
}
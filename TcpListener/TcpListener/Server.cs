using System;
using System.Diagnostics;
using System.Net;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal class Server
    {
        private const int BufferSize = 1024;
        private const long TimerRestartTime = 500;
        
        private readonly System.Net.Sockets.TcpListener _server;
        private readonly Timer _timer = new Timer(TimerRestartTime);

        private long _totalRecivedBytesCount;
        private long _lastDisplayedRecivedBytesCount;
        
        internal Server(int port, [NotNull] IPAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            try
            {
                _server = new System.Net.Sockets.TcpListener(address, port);
            }
            catch (Exception e)
            {
                throw new ServerException(e);
            }
            _timer.Elapsed += DisplayCurrentSpeed;
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
                        _totalRecivedBytesCount = 0;
                        _lastDisplayedRecivedBytesCount = 0;
                        _timer.Start();
                        try
                        {
                            var clientIpEndPoint = client.Client.LocalEndPoint as IPEndPoint;
                            Console.WriteLine($"Подключен клиент {clientIpEndPoint?.Address}. Выполнение запроса...");
                            var stream = client.GetStream();
                            var buffer = new byte[BufferSize];

                            var time = Stopwatch.StartNew();
                            while (true)
                            {
                                var readedBytes = stream.Read(buffer, 0, buffer.Length);
                                if (readedBytes == 0)
                                {
                                    break;
                                }
                                _totalRecivedBytesCount += readedBytes;
                            }
                            DisplayResult(_totalRecivedBytesCount, time.ElapsedMilliseconds);
                        }
                        catch (Exception)
                        {
                            Console.Error.WriteLine("Ошибка при получении данных.");
                        }
                        finally
                        {
                            _timer.Stop();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new ServerException(e);
            }
            finally
            {
                _timer?.Close();
                _server?.Stop();
            }
        }

        internal void StopListen()
        {
            _server.Stop();
        }

        private void DisplayCurrentSpeed(object source, ElapsedEventArgs e)
        {
            var cursorPosition = Console.CursorTop;
            var recivedBytes = _totalRecivedBytesCount - _lastDisplayedRecivedBytesCount;
            _lastDisplayedRecivedBytesCount = _lastDisplayedRecivedBytesCount + recivedBytes;

            var averedgeSpeed = recivedBytes.BytesToMegaBytes() / TimerRestartTime.MillisecondToSecond();
            NetIO.ConsoleWrite(cursorPosition, "Текущая скорость: " + averedgeSpeed.ToString("F") + "МБ/с");
        }

        private static void DisplayResult(long byteCount, long milliseconds)
        {
            Console.WriteLine($"Байт принято: {byteCount}");
            Console.WriteLine($"Общее время: {milliseconds.MillisecondToSecond().ToString("F")}c");
            Console.WriteLine(milliseconds <= 0
                ? "Данные пряняты быстрее чем за 1ну миллисекунду."
                : $"Средняя скорость: {(byteCount.BytesToMegaBytes() / milliseconds.MillisecondToSecond()).ToString("F")} MB/c");
            Console.WriteLine();
        }
    }
}
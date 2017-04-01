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
        private const long TimerDelay = 500;
        
        private readonly System.Net.Sockets.TcpListener _server;
        private readonly Timer _speedometer = new Timer(TimerDelay);

        private long _totalReceivedBytesCount;
        private long _lastDisplayedReceivedBytesCount;
        
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
                Console.WriteLine($"Не удалось запустить приложение под портом: {port.GetPort}");
                try
                {
                    _server = new System.Net.Sockets.TcpListener(address, 0);
                    var ipEndPoint = _server.LocalEndpoint as IPEndPoint;
                    var currentPort = ipEndPoint?.Port;
                    Console.WriteLine($"Автоматически выбран порт: {currentPort}");
                }
                catch (Exception ex)
                {
                    throw new ServerException(ex);
                }
            }
            _speedometer.Elapsed += DisplayCurrentSpeed;
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
                        _totalReceivedBytesCount = 0;
                        _lastDisplayedReceivedBytesCount = 0;
                        _speedometer.Start();
                        try
                        {
                            var clientIpEndPoint = client.Client.LocalEndPoint as IPEndPoint;
                            Console.WriteLine($"Подключен клиент {clientIpEndPoint?.Address}. Выполнение запроса...");

                            var time = Stopwatch.StartNew();
                            using (var stream = client.GetStream())
                            {
                                var buffer = new byte[BufferSize];
                                
                                while (true)
                                {
                                    var readedBytes = stream.Read(buffer, 0, buffer.Length);
                                    if (readedBytes == 0)
                                    {
                                        break;
                                    }
                                    _totalReceivedBytesCount += readedBytes;
                                }
                            }
                            DisplayResult(_totalReceivedBytesCount, time.ElapsedMilliseconds);
                        }
                        catch (Exception)
                        {
                            Console.Error.WriteLine("Ошибка при получении данных.");
                        }
                        finally
                        {
                            _speedometer.Stop();
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
                _speedometer?.Close();
                _server?.Stop();
            }
        }

        internal void StopListen()
        {
            _server.Stop();
        }

        private void DisplayCurrentSpeed([NotNull] object source, [NotNull] ElapsedEventArgs e)
        {
            var cursorPosition = Console.CursorTop;
            var receivedBytes = _totalReceivedBytesCount - _lastDisplayedReceivedBytesCount;
            _lastDisplayedReceivedBytesCount = _lastDisplayedReceivedBytesCount + receivedBytes;

            var averedgeSpeed = receivedBytes.BytesToMegaBytes() / TimerDelay.MillisecondToSecond();
            NetIO.ConsoleWrite(cursorPosition, "Текущая скорость: " + averedgeSpeed.ToString("F") + "МБ/с");
        }

        private static void DisplayResult(long byteCount, long milliseconds)
        {
            var cursorPosition = Console.CursorTop;
            NetIO.ConsoleWrite(cursorPosition, "                                                               ");

            Console.WriteLine($"Байт принято: {byteCount}");
            Console.WriteLine($"Общее время: {milliseconds.MillisecondToSecond():F} c");
            Console.WriteLine(milliseconds <= 0
                ? "Данные пряняты быстрее чем за 1ну миллисекунду."
                : $"Средняя скорость: {(byteCount.BytesToMegaBytes() / milliseconds.MillisecondToSecond()):F} MB/c");
            Console.WriteLine();
        }
    }
}
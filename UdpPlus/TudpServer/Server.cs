using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;
using UdpPlus;

namespace TudpServer
{
    internal class Server : Disposable
    {
        private const long TimerDelay = 500;
        
        private readonly TudpListener _server;
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
                _server = new TudpListener(port, address);
            }
            catch (Exception ex)
            {
                throw new ServerException(ex);
            }
            _speedometer.Elapsed += DisplayCurrentSpeed;
        }

        internal void Listen()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");
                    _totalReceivedBytesCount = 0;
                    _lastDisplayedReceivedBytesCount = 0;
                    var remoteIp = _server.WaitConnection();
                    Console.Out.WriteLine($"{remoteIp} was connected...");
                    try
                    {
                        _speedometer.Start();
                        var time = Stopwatch.StartNew();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestReceived");
                        using (var file = new FileStream(filePath, FileMode.Create))
                        {
                            while (true)
                            {
                                var readedBytes = _server.Read();
                                if (readedBytes.Length == 0)
                                {
                                    break;
                                }
                                file.Write(readedBytes, 0, readedBytes.Length);
                                _totalReceivedBytesCount += readedBytes.Length;
                            }
                        }
                        DisplayResult(_totalReceivedBytesCount, time.ElapsedMilliseconds);
                        break;
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Ошибка при получении данных.");
                        break;
                    }
                    finally
                    {
                        _speedometer.Stop();
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
            }
        }

        internal void StopListen()
        {
            Console.Out.WriteLine("Over");
        }

        private void DisplayCurrentSpeed([NotNull] object source, [NotNull] ElapsedEventArgs e)
        {
            var totalReceivedBytesCount = _totalReceivedBytesCount;
            var receivedBytes = totalReceivedBytesCount - _lastDisplayedReceivedBytesCount;
            _lastDisplayedReceivedBytesCount = _lastDisplayedReceivedBytesCount + receivedBytes;

            var averedgeSpeed = receivedBytes.BytesToMegaBytes() / TimerDelay.MillisecondToSecond();
            var cursorPosition = Console.CursorTop;
            NetIO.ConsoleWrite(cursorPosition, "Current speed: " + averedgeSpeed.ToString("F4") + "MB/s");
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

        protected override void FreeManagedResources()
        {
            _server.Dispose();
        }
    }
}
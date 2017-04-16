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
    internal class Server
    {
        private const long TimerDelay = 500;
        
        private readonly object _counterMutex = new object();

        private readonly System.Net.Sockets.TcpListener _server;
        private readonly TudpListener _serverN;
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
                _server = new System.Net.Sockets.TcpListener(address, port.AtInt);
                _serverN = new TudpListener(port, address);
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
                _server.Start();

                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");
                    _totalReceivedBytesCount = 0;
                    _lastDisplayedReceivedBytesCount = 0;
                    _speedometer.Start();
                    try
                    {
                        var time = Stopwatch.StartNew();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestReceived");
                        using (var file = new FileStream(filePath, FileMode.OpenOrCreate))
                        {
                            while (true)
                            {
                                var readedBytes = _serverN.Receive();
                                if (readedBytes.Length == 0)
                                {
                                    break;
                                }
                                file.Write(readedBytes, 0, readedBytes.Length);
                                lock (_counterMutex)
                                {
                                    _totalReceivedBytesCount += readedBytes.Length;
                                }
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
                _server?.Stop();
            }
        }

        internal void StopListen()
        {
            _server.Stop();
        }

        private void DisplayCurrentSpeed([NotNull] object source, [NotNull] ElapsedEventArgs e)
        {

            long receivedBytes;
            lock (_counterMutex)
            {
                receivedBytes = _totalReceivedBytesCount - _lastDisplayedReceivedBytesCount;
            }
            _lastDisplayedReceivedBytesCount = _lastDisplayedReceivedBytesCount + receivedBytes;

            var averedgeSpeed = receivedBytes.BytesToMegaBytes() / TimerDelay.MillisecondToSecond();
            var cursorPosition = Console.CursorTop;
            NetIO.ConsoleWrite(cursorPosition, "Current speed: " + averedgeSpeed.ToString("F") + "MB/s");
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
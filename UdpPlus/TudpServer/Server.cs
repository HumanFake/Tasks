using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;
using UdpPlus;
using Timer = System.Timers.Timer;

namespace TudpServer
{
    internal sealed class Server : Disposable
    {
        private const long TimerDelay = 500;

        private readonly TudpListener _server;
        private readonly Timer _speedometer = new Timer(TimerDelay);
        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

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
            _cancellationToken = _cancelTokenSource.Token;

            _speedometer.Elapsed += DisplayCurrentSpeed;
        }

        private void SignalHandler(int unused)
        {
            StopListen();
        }

        internal void Listen()
        {
            try
            {
                ConsoleActions.SetSignalHandler(SignalHandler, true);

                while (true)
                {
                    Console.WriteLine("Connection waiting... ");
                    _totalReceivedBytesCount = 0;
                    _lastDisplayedReceivedBytesCount = 0;
                    var remoteIp = _server.GetConnectionAddressOrNull(_cancellationToken);
                    if (remoteIp == null)
                    {
                        break;
                    }
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
                                var readBytes = _server.Read();
                                if (readBytes.Length == 0)
                                {
                                    break;
                                }
                                file.Write(readBytes, 0, readBytes.Length);
                                _totalReceivedBytesCount += readBytes.Length;
                            }
                        }
                        DisplayResult(_totalReceivedBytesCount, time.ElapsedMilliseconds);
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Error data receive.");
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

        private void StopListen()
        {
            _cancelTokenSource.Cancel();
        }

        private void DisplayCurrentSpeed([NotNull] object source, [NotNull] ElapsedEventArgs e)
        {
            var totalReceivedBytesCount = _totalReceivedBytesCount;
            var receivedBytes = totalReceivedBytesCount - _lastDisplayedReceivedBytesCount;
            _lastDisplayedReceivedBytesCount = _lastDisplayedReceivedBytesCount + receivedBytes;

            var averageSpeed = receivedBytes.BytesToMegaBytes() / TimerDelay.MillisecondToSecond();
            var cursorPosition = Console.CursorTop;
            NetIo.ConsoleWrite(cursorPosition, "Current speed: " + averageSpeed.ToString("F4") + "MB/s");
        }

        private static void DisplayResult(long byteCount, long milliseconds)
        {
            var cursorPosition = Console.CursorTop;
            NetIo.ConsoleWrite(cursorPosition, "                                                               ");
            
            Console.WriteLine($"Total bytes: {byteCount}");
            Console.WriteLine($"Total time: {milliseconds.MillisecondToSecond():F} s");
            Console.WriteLine(milliseconds <= 0
                ? "Receive faster when one second."
                : $"Average speed: {byteCount.BytesToMegaBytes() / milliseconds.MillisecondToSecond():F} MB/s");
            Console.WriteLine();
        }

        protected override void FreeManagedResources()
        {
            //_server.Dispose();
        }
    }
}
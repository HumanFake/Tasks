using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal sealed class Receiver : Disposable
    {
        private const int BufferSize = 1024 * 1024;
        private const long TimerDelay = 500;

        private readonly TcpClient _tcpClient;
        private readonly Timer _speedometer = new Timer(TimerDelay);
        private readonly int _cursorPosition;
        private readonly object _locker = new object();

        private long _totalReceivedBytesCount;
        private long _lastDisplayedReceivedBytesCount;

        internal Receiver([NotNull] TcpClient tcpClient)
        {
            if (tcpClient == null)
            {
                throw new ArgumentNullException(nameof(tcpClient));
            }
            _cursorPosition = Console.CursorTop;
            _tcpClient = tcpClient;
            _speedometer.Elapsed += DisplayCurrentSpeed;
        }

        internal void ReceiveData()
        {
            _speedometer.Start();
            try
            {
                var clientIpEndPoint = _tcpClient.Client.LocalEndPoint as IPEndPoint;
                Console.WriteLine($"{clientIpEndPoint?.Address} connect. Data receiving...");
                Console.WriteLine();
                var time = Stopwatch.StartNew();
                using (var stream = _tcpClient.GetStream())
                {
                    var buffer = new byte[BufferSize];

                    while (true)
                    {
                        var readBytes = stream.Read(buffer, 0, buffer.Length);
                        if (readBytes == 0)
                        {
                            break;
                        }
                        lock (_locker)
                        {
                            _totalReceivedBytesCount += readBytes;
                        }
                    }
                }
                DisplayResult(_totalReceivedBytesCount, time.ElapsedMilliseconds, clientIpEndPoint?.Address.ToString());
            }
            finally
            {
                _speedometer.Stop();
            }
        }

        private void DisplayCurrentSpeed([NotNull] object source, [NotNull]  ElapsedEventArgs e)
        {
            lock (_locker)
            {
                var receivedBytes = _totalReceivedBytesCount - _lastDisplayedReceivedBytesCount;
                _lastDisplayedReceivedBytesCount = _lastDisplayedReceivedBytesCount + receivedBytes;

                var averageSpeed = receivedBytes.BytesToMegaBytes() / TimerDelay.MillisecondToSecond();
                NetIo.ConsoleWrite(_cursorPosition, "Current speed: " + averageSpeed.ToString("F") + "MB/s");
            }
        }

        private void DisplayResult(long byteCount, long milliseconds, [CanBeNull] string clientIp)
        {
            lock (_locker)
            {
                NetIo.ConsoleWrite(_cursorPosition, "                                                               ");
                Console.WriteLine();
                Console.WriteLine($"Data receiving complete from: {clientIp}");
                Console.WriteLine($"Total bytes: {byteCount}");
                Console.WriteLine($"Total time: {milliseconds.MillisecondToSecond():F} s");
                Console.WriteLine(milliseconds <= 0
                    ? "Receive faster when one second."
                    : $"Average speed: {(byteCount.BytesToMegaBytes() / milliseconds.MillisecondToSecond()):F} MB/s");
                Console.WriteLine();
            }
        }

        protected override void FreeManagedResources()
        {
            _speedometer.Close();
            _tcpClient.Close();
        }
    }
}
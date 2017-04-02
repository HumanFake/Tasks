using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal class ResponseClient : Disposable
    {
        private const int BufferSize = 1024 * 1024;
        private const long TimerDelay = 500;

        private readonly TcpClient _tcpClient;
        private readonly Timer _speedometer = new Timer(TimerDelay);
        private readonly int _cursorPosition;
        private readonly object _outputLocker = new object(); 

        private long _totalReceivedBytesCount;
        private long _lastDisplayedReceivedBytesCount;

        internal ResponseClient([NotNull] TcpClient tcpClient)
        {
            if (tcpClient == null)
            {
                throw new ArgumentNullException(nameof(tcpClient));
            }
            _cursorPosition = Console.CursorTop;
            _tcpClient = tcpClient;
            _speedometer.Elapsed += DisplayCurrentSpeed;
        }

        internal void ResponceMessage()
        {
            _totalReceivedBytesCount = 0;
            _lastDisplayedReceivedBytesCount = 0;
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
                        var readedBytes = stream.Read(buffer, 0, buffer.Length);
                        if (readedBytes == 0)
                        {
                            break;
                        }
                        _totalReceivedBytesCount += readedBytes;
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
            lock (_outputLocker)
            {
                var receivedBytes = _totalReceivedBytesCount - _lastDisplayedReceivedBytesCount;
                _lastDisplayedReceivedBytesCount = _lastDisplayedReceivedBytesCount + receivedBytes;

                var averedgeSpeed = receivedBytes.BytesToMegaBytes() / TimerDelay.MillisecondToSecond();
                NetIO.ConsoleWrite(_cursorPosition, "Current speed: " + averedgeSpeed.ToString("F") + "MB/s");
            }
        }

        private void DisplayResult(long byteCount, long milliseconds, [CanBeNull] string clientIp)
        {
            lock (_outputLocker)
            {
                NetIO.ConsoleWrite(_cursorPosition, "                                                               ");
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
            _tcpClient?.Close();
        }
    }
}
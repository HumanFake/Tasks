using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using JetBrains.Annotations;
using NetUtils;
using Timer = System.Timers.Timer;

namespace TcpListener
{
    internal sealed class Receiver : Disposable
    {
        private const int BufferSize = 1024 * 1024;
        private const long TimerDelay = 500;

        private readonly TcpClient _tcpClient;
        private readonly Timer _speedometer = new Timer(TimerDelay);
        private readonly int _cursorPosition;
        private readonly object _monitor = new object();
        private readonly object _outputMonitor = new object();

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

        internal void ReceiveData(CancellationToken cancellationToken)
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
                        if (cancellationToken.IsCancellationRequested)
                        {

                            Console.WriteLine($"data receive from {clientIpEndPoint?.Address} was interrupt");
                            break;
                        }
                        var readBytes = stream.Read(buffer, 0, buffer.Length);
                        if (readBytes == 0)
                        {
                            break;
                        }
                        lock (_monitor)
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
            long receivedBytes;
            lock (_monitor)
            {
                receivedBytes = _totalReceivedBytesCount - _lastDisplayedReceivedBytesCount;
                _lastDisplayedReceivedBytesCount = _lastDisplayedReceivedBytesCount + receivedBytes;
            }
            lock (_outputMonitor)
            {
                var averageSpeed = receivedBytes.BytesToMegaBytes() / TimerDelay.MillisecondToSecond();
                NetIo.ConsoleWrite(_cursorPosition, "Current speed: " + averageSpeed.ToString("F") + "MB/s");
            }
        }

        private void DisplayResult(long byteCount, long milliseconds, [CanBeNull] string clientIp)
        {
            lock (_outputMonitor)
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
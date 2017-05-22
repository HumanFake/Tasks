using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal sealed class Server : Disposable
    {
        private delegate void SignalHandler(int consoleSignal);

        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
        private static extern bool SetSignalHandler(SignalHandler handler, bool addHandler);

        private readonly System.Net.Sockets.TcpListener _server;
        private readonly List<Thread> _threads = new List<Thread>();
        private readonly CancellationTokenSource CancelTokenSource = new CancellationTokenSource();

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
            }
            catch (Exception)
            {
                Console.WriteLine($"Can't star at: {port.AtInt} port");
                try
                {
                    _server = new System.Net.Sockets.TcpListener(address, 0);
                    var ipEndPoint = _server.Server.LocalEndPoint as IPEndPoint;
                    Console.WriteLine($"Automatically chosen port: {ipEndPoint?.Port}");
                }
                catch (Exception e)
                {
                    throw new ServerException(e);
                }
            }
            void SignalHandler(int unused) => ListenStop();
            SetSignalHandler(SignalHandler, true);
        }

        internal void Listen()
        {
            try
            {
                _server.Start();
                while (true)
                {
                    Console.WriteLine("Connection waiting... ");
                    var tcpClient = _server.AcceptTcpClient();
                    var responseClient = new Receiver(tcpClient);

                    var token = CancelTokenSource.Token;
                    var clientThread = new Thread(() => GetResponse(responseClient, token));
                    clientThread.Start();
                    clientThread.Interrupt();
                }
            }
            catch (SocketException e)
            {
#if DEBUG
                Console.Out.WriteLine(e);
#endif
            }
            catch (Exception e)
            {
                throw new ServerException(e);
            }
        }

        private void ListenStop()
        {
            CancelTokenSource.Cancel();
            _server.Stop();
        }

        private static void GetResponse([NotNull] Receiver receiver, CancellationToken token)
        {
            try
            {
                using (receiver)
                {
                    receiver.ReceiveData(token);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error data receive.");
            }
        }

        protected override void FreeManagedResources()
        {
            _server.Stop();
        }
    }
}
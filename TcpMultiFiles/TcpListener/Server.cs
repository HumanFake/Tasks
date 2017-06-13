using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal sealed class Server : Disposable
    {
        private readonly System.Net.Sockets.TcpListener _server;
        private readonly List<Thread> _threads = new List<Thread>();
        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

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
        }

        private void SignalHandler(int unused)
        {
            ListenStop();
        }

        internal void Listen()
        {
            try
            {
                ConsoleActions.SetSignalHandler(SignalHandler, true);

                _server.Start();
                while (true)
                {
                    Console.WriteLine("Connection waiting... ");
                    var tcpClient = _server.AcceptTcpClient();
                    var responseClient = new Receiver(tcpClient);

                    var token = _cancelTokenSource.Token;
                    var clientThread = new Thread(() => GetResponse(responseClient, token));
                    clientThread.Start();
                    _threads.Add(clientThread);

                    if (0 == _threads.Count % 2)
                    {
                        RemoveCompletedTreads();
                    }
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

        private void RemoveCompletedTreads()
        {
            for (int i = 0; i < _threads.Count; i++)
            {
                if (false == _threads[i].IsAlive)
                {
                    _threads.RemoveAt(i);
                }
            }
        }

        private void ListenStop()
        {
            ThrowIfDisposed();

            _cancelTokenSource.Cancel();
            _server.Stop();
            RemoveCompletedTreads();
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
            _cancelTokenSource.Dispose();
            _server.Stop();
        }
    }
}
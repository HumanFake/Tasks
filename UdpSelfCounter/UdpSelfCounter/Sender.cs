using System;
using System.Net;
using System.Net.Sockets;
using NetUtils;

namespace UdpSelfCounter
{
    internal class Sender : Disposable
    {
        private readonly UdpClient _sender;
        private readonly IPEndPoint _endPoint;

        internal Sender(Port port)
        {
            _sender = new UdpClient();
            _endPoint = new IPEndPoint(ProgramData.RemoteIpAddress, port.GetPort);
        }

        internal void Send(short datagram)
        {
            try
            {
                var bytes = BitConverter.GetBytes(datagram);

                _sender.Send(bytes, bytes.Length, _endPoint);
            }
            catch (Exception ex)
            {
                throw new SenderException(ex.Message, ex.InnerException);
            }
        }

        protected override void FreeManagedResources()
        {
            _sender?.Close();
        }
    }
}
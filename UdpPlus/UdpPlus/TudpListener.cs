using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using NetUtils;

namespace UdpPlus
{
    public class TudpListener : Disposable
    {
        private readonly UdpClient _udpClient;

        public TudpListener([NotNull]Port port, [NotNull] IPAddress address)
        {
            if (port == null)
            {
                throw new ArgumentNullException(nameof(port));
            }
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            _udpClient = new UdpClient(new IPEndPoint(address, port.AtInt));
        }

        public byte[] Receive()
        {
            try
            {
                byte[] dategrammIdentifer = null;
                var reciveData = new List<byte>();

                while (true)
                {
                    IPEndPoint remoteIpEndPoint = null;

                    var receiveBytes = _udpClient.Receive(ref remoteIpEndPoint);
                    var tudpData = new TudpData(receiveBytes);
                    var reveivedategrammIdentifer = tudpData.GetIdentifer();

                    if (BitConverter.ToInt32(reveivedategrammIdentifer, 0) == TudpData.LastMessegeIndetifer)
                    {
                        _udpClient.Send(TudpData.LastMessege, TudpData.LastMessege.Length, remoteIpEndPoint);
                        break;
                    }
                    if (dategrammIdentifer == null ||
                        BitConverter.ToInt32(reveivedategrammIdentifer, 0) != BitConverter.ToInt32(dategrammIdentifer, 0))
                    {
                        reciveData.AddRange(tudpData.GetDategramm());
                        dategrammIdentifer = reveivedategrammIdentifer;
                        
                        var i = BitConverter.ToInt32(reveivedategrammIdentifer, 0);
                        Console.Out.WriteLine($"получен пакет {i}");
                        Console.Out.WriteLine($"total count b {reciveData.Count}");
                    }
                    _udpClient.Send(reveivedategrammIdentifer, reveivedategrammIdentifer.Length, remoteIpEndPoint);
                }

                return reciveData.ToArray();
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw new Exception();
            }
        }

        protected override void FreeManagedResources()
        {
            _udpClient?.Close();
        }
    }
}
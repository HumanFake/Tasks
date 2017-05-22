using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using NetUtils;

namespace TcpListener
{
    internal static class ServerEntryPoint
    {

        private static void Main()
        {
            var port = Port.ReadPort();
            var address = NetIo.FindLocalIpAddressOrNull();

            try
            {
                if (address == null)
                {
                    throw new Exception("Can't get local IP");
                }
                Console.Clear();
                using (var server = new Server(port, address))
                {
                    server.Listen();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

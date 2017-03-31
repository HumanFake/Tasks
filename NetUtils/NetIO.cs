using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace NetUtils
{
    public static class NetIO
    {
        private const string InvalidIp = "Недопустимый IP.";
        private const double BytesInMegaBytes = 1024.0;
        private const double MilliseconsInSecond = 1000.0;

        public static IPAddress ReadAddress()
        {
            IPAddress address;
            while (true)
            {
                Console.Write("Введите IP: ");
                try
                {
                    var input = Console.ReadLine();
                    if (input != null)
                    {
                        address = IPAddress.Parse(input);
                        break;
                    }
                    Console.WriteLine(InvalidIp);
                }
                catch (Exception)
                {
                    Console.WriteLine(InvalidIp);
                }
            }
            return address;
        }

        [CanBeNull]
        public static IPAddress FindLocalIpAddressOrNull()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public static double BytesToMegaBytes(this long sourse)
        {
            return sourse / BytesInMegaBytes / BytesInMegaBytes;
        }

        public static double MillisecondToSecond(this long sourse)
        {
            return sourse / MilliseconsInSecond;
        }
    }
}

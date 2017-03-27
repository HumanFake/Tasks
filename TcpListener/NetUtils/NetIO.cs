using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NetUtils
{
    public static class NetIO
    {
        private const string InvalidIp = "Недопустимый IP.";
        private const string InvalidPort = "Недопустимое значение порта.";
        private const int MaximumPort = 65535;
        private const int MinimumPort = 1024;
        private const double BytesInMegaBytes = 1024.0;

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

        public static Port ReadPort()
        {
            int port;
            while (true)
            {
                Console.Write("Введите порт: ");
                var input = Console.ReadLine();
                if (input != null && int.TryParse(input, out port))
                {
                    if (port > MinimumPort || port < MaximumPort)
                    {
                        break;
                    }
                }
                Console.WriteLine(InvalidPort);
            }
            return new Port(port);
        }

        public static IPAddress FindLocalIpAddress()
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
            return sourse / 1000.0;
        }
    }
}

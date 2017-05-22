using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace NetUtils
{
    public static class NetIo
    {
        private const string InvalidIp = "Invalid IP.";
        private const double BytesInMegaBytes = 1024.0;
        private const double MillisecondsInSecond = 1000.0;

        [PublicAPI]
        public static IPAddress ReadAddress()
        {
            IPAddress address;
            while (true)
            {
                Console.Write("Enter IP: ");
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

        [PublicAPI]
        public static void ConsoleWrite(int line, [NotNull] string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            var currentLeftCursorPosition = Console.CursorLeft;
            var currentTopCursorPosition = Console.CursorTop;

            Write(line, text);

            Console.SetCursorPosition(currentLeftCursorPosition, currentTopCursorPosition);
        }

        [PublicAPI]
        public static void ConsoleWriteLine(int line, [NotNull] string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (line < 0)
            {
                throw new ArgumentException($"{nameof(line)} must be greater than zero");
            }
            var currentLeftCursorPosition = Console.CursorLeft;
            var currentTopCursorPosition = Console.CursorTop;

            Write(line, text);
            Console.WriteLine();

            Console.SetCursorPosition(currentLeftCursorPosition, currentTopCursorPosition);
        }

        private static void Write(int line, [NotNull] string text)
        {
            Console.SetCursorPosition(0, line);
            Console.Write(" ");
            Console.SetCursorPosition(0, line);
            Console.Write(text);
        }

        [CanBeNull]
        public static IPAddress FindLocalIpAddressOrNull()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        [CanBeNull]
        public static IPAddress FindLocalIpAddressOrNull(NetworkInterfaceType _type)
        {
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address;
                        }
                    }
                }
            }
            return null;
        }

        [PublicAPI]
        public static double BytesToMegaBytes(this long source)
        {
            return source / BytesInMegaBytes / BytesInMegaBytes;
        }

        [PublicAPI]
        public static double MillisecondToSecond(this long source)
        {
            return source / MillisecondsInSecond;
        }
    }
}

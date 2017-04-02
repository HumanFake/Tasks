﻿using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using JetBrains.Annotations;
// ReSharper disable InconsistentNaming

namespace NetUtils
{
    public static class NetIO
    {
        private const string InvalidIp = "Invalid IP.";
        private const double BytesInMegaBytes = 1024.0;
        private const double MillisecondsInSecond = 1000.0;

        [UsedImplicitly]
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

        [UsedImplicitly]
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

        [UsedImplicitly]
        public static void ConsoleWriteLine(int line, [NotNull] string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
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
            Console.Write(" ", 0, Console.WindowWidth);
            Console.SetCursorPosition(0, line);
            Console.Write(text);
        }

        [CanBeNull]
        public static IPAddress FindLocalIpAddressOrNull()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        [UsedImplicitly]
        public static double BytesToMegaBytes(this long sourse)
        {
            return sourse / BytesInMegaBytes / BytesInMegaBytes;
        }

        [UsedImplicitly]
        public static double MillisecondToSecond(this long sourse)
        {
            return sourse / MillisecondsInSecond;
        }
    }
}

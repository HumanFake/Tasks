using System;
using System.IO;
using System.Net;
using NetUtils;

namespace TudpClient
{
    internal static class EntryPoint
    {
        private static void Main()
        {
            var port = Port.ReadPort();
            var ipAddress = NetIo.ReadAddress();

            var ipEndPoint = new IPEndPoint(ipAddress, port.AtInt);

            Console.WriteLine("Enter file name:");
            var fileName = Console.ReadLine();
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    using (var client = new Client(ipEndPoint))
                    {
                        Console.WriteLine("Sending data...");
                        client.Send(stream);
                        Console.WriteLine("Data sending complete...");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                #if DEBUG
                Console.Write(exception.Message);
                #endif
            }
            Console.Read();
        }
    }
}

using System;
using System.IO;
using System.Net;
using NetUtils;

namespace TcpClient
{
    internal static class CliEntryPoint
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
                        client.Send(stream);
                        Console.WriteLine("Data sending complete...");
                    }
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.Read();
        }
    }
}

using System;
using System.IO;
using System.Net;
using NetUtils;

namespace TudpClient
{
    static class EntryPoint
    {
        static void Main()
        {
            var port = Port.ReadPort();
            var ipAddress = NetIO.ReadAddress();

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

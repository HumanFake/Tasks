using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetUtils;

namespace TudpClient
{
    static class EntryPoint
    {
        static void Main(string[] args)
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
                        client.Send(stream);
                        Console.WriteLine("Data sending complete...");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.Read();
        }
    }
}

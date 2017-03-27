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
            var port = NetIO.ReadPort();
            var ipAddress = NetIO.ReadAddress();

            var ipEndPoint = new IPEndPoint(ipAddress, port);

            Console.WriteLine("Введите имя файла:");
            var fileName = Console.ReadLine();

            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    using (var client = new Client(ipEndPoint))
                    {
                        client.Send(stream);
                        Console.WriteLine("Отправка данных завершена...");
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

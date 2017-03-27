using System;
using System.IO;
using System.Net;
using NetUtils;

namespace TcpClient
{
    static class ClientEntryPoint
    {
        private const int BufferSize = 1024 * 1024;

        static void Main()
        {
            var port = NetIO.ReadPort();
            var ipAddress = NetIO.ReadAddress();

            var ipEndPoint = new IPEndPoint(ipAddress, port);

            Console.WriteLine("Введите имя файла:");
            var fileName = Console.ReadLine();

            try
            {
                using (var reader = new FileStream(fileName, FileMode.Open))
                {
                    using (var client = new Client(ipEndPoint))
                    {
                        var buffer = new byte[BufferSize];

                        Console.WriteLine("Отправка данных началась...");
                        while (true)
                        {
                            var readBytesCount = reader.Read(buffer, 0, buffer.Length);
                            if (readBytesCount == 0)
                            {
                                break;
                            }
                            client.Send(buffer);
                        }
                    }
                }
                Console.WriteLine("Отправка данных завершена...");
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.Read();
        }
    }
}

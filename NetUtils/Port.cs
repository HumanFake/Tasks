using System;

namespace NetUtils
{
    public sealed class Port
    {
        private const string InvalidPort = "Недопустимое значение порта.";
        private const int MaximumPort = 65535;
        private const int MinimumPort = 1024;

        private Port(int port)
        {
            GetPort = port;
        }

        public int GetPort { get; }

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
    }
}
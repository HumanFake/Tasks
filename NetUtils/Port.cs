using System;

namespace NetUtils
{
    public sealed class Port
    {
        private const string InvalidPort = "Invalid port.";
        private const int MaximumPort = 65535;
        private const int MinimumPort = 1024;

        private Port(int port)
        {
            AtInt = port;
        }

        public int AtInt { get; }

        public static Port ReadPort()
        {
            int port;
            while (true)
            {
                Console.Write("Enter port: ");
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
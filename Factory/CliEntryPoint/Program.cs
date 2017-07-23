using System;
using Model;

namespace CliEntryPoint
{
    static class Program
    {
        static void Main(string[] args)
        {
            var fabric = new Fabric();
            fabric.Start();

            Console.Read();
            fabric.Stop();
        }
    }
}

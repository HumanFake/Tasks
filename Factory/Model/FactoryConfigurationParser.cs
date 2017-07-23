using System.IO;

namespace Model
{
    internal static class FactoryConfigurationParser
    {
        private const string Path = @"C:\Users\HumanFake\Desktop\Tasks-master\Factory\CliEntryPoint\FactoryConfiguration.txt";

        internal static FactoryConfiguration Parse()
        {
            var factoryConfiguration = new FactoryConfiguration();
            using (var reader = new StreamReader(Path))
            {
                while (false == reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var splitedLine = line?.Split('=');

                    if(splitedLine?.Length != 2)
                    {
                        continue;
                    }

                    if(splitedLine[0] == "CarStorageCapacity")
                    {
                        if (false == uint.TryParse(splitedLine[1], out uint carStorageCapacity))
                        {
                            continue;
                        }
                        factoryConfiguration.CarStorageCapacity = carStorageCapacity;
                    }
                    if (splitedLine[0] == "MotorStorageCapacity")
                    {
                        if (false == uint.TryParse(splitedLine[1], out uint motorStorageCapacity))
                        {
                            continue;
                        }
                        factoryConfiguration.MotorStorageCapacity = motorStorageCapacity;
                    }
                    if (splitedLine[0] == "BodyStorageCapacity")
                    {
                        if (false == uint.TryParse(splitedLine[1], out uint bodyStorageCapacity))
                        {
                            continue;
                        }
                        factoryConfiguration.BodyStorageCapacity = bodyStorageCapacity;
                    }
                    if (splitedLine[0] == "AccessoryStorageCapacity")
                    {
                        if (false == uint.TryParse(splitedLine[1], out uint accessoryStorageCapacity))
                        {
                            continue;
                        }
                        factoryConfiguration.AccessoryStorageCapacity = accessoryStorageCapacity;
                    }
                    if (splitedLine[0] == "AccessorySupplier")
                    {
                        if (false == uint.TryParse(splitedLine[1], out uint accessorySupplier))
                        {
                            continue;
                        }
                        factoryConfiguration.AccessorySupplier = accessorySupplier;
                    }
                    if (splitedLine[0] == "Workers")
                    {
                        if (false == uint.TryParse(splitedLine[1], out uint workers))
                        {
                            continue;
                        }
                        factoryConfiguration.Workers = workers;
                    }
                    if (splitedLine[0] == "Dealers")
                    {
                        if (false == uint.TryParse(splitedLine[1], out uint dealers))
                        {
                            continue;
                        }
                        factoryConfiguration.Dealers = dealers;
                    }
                    if (splitedLine[0] == "WriteToLog")
                    {
                        if (false == bool.TryParse(splitedLine[1], out bool writeToLog))
                        {
                            continue;
                        }
                        factoryConfiguration.WriteToLog = writeToLog;
                    }
                }
            }

            return factoryConfiguration;
        }
    }

    internal struct FactoryConfiguration
    {
        internal uint CarStorageCapacity { get; set; }
        internal uint MotorStorageCapacity { get; set; }
        internal uint BodyStorageCapacity { get; set; }
        internal uint AccessoryStorageCapacity { get; set; }
        internal uint AccessorySupplier { get; set; }
        internal uint Workers { get; set; }
        internal uint Dealers { get; set; }

        internal bool WriteToLog { get; set; }
    }
}

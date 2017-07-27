namespace Model
{
    internal static class FactoryConfigurationParser
    {
        internal static FactoryConfiguration Parse()
        {
            var factoryConfiguration = new FactoryConfiguration();
            var configurationLines = Properties.Resources.FactoryConfiguration.Split('\n');

            foreach (var line in configurationLines)
            {
                var splittedLine = line?.Split('=');

                if (splittedLine?.Length != 2)
                {
                    continue;
                }

                if (splittedLine[0] == "CarStorageCapacity")
                {
                    if (false == uint.TryParse(splittedLine[1], out uint carStorageCapacity))
                    {
                        continue;
                    }
                    factoryConfiguration.CarStorageCapacity = carStorageCapacity;
                }
                if (splittedLine[0] == "MotorStorageCapacity")
                {
                    if (false == uint.TryParse(splittedLine[1], out uint motorStorageCapacity))
                    {
                        continue;
                    }
                    factoryConfiguration.MotorStorageCapacity = motorStorageCapacity;
                }
                if (splittedLine[0] == "BodyStorageCapacity")
                {
                    if (false == uint.TryParse(splittedLine[1], out uint bodyStorageCapacity))
                    {
                        continue;
                    }
                    factoryConfiguration.BodyStorageCapacity = bodyStorageCapacity;
                }
                if (splittedLine[0] == "AccessoryStorageCapacity")
                {
                    if (false == uint.TryParse(splittedLine[1], out uint accessoryStorageCapacity))
                    {
                        continue;
                    }
                    factoryConfiguration.AccessoryStorageCapacity = accessoryStorageCapacity;
                }
                if (splittedLine[0] == "AccessorySupplier")
                {
                    if (false == uint.TryParse(splittedLine[1], out uint accessorySupplier))
                    {
                        continue;
                    }
                    factoryConfiguration.AccessorySupplier = accessorySupplier;
                }
                if (splittedLine[0] == "Workers")
                {
                    if (false == uint.TryParse(splittedLine[1], out uint workers))
                    {
                        continue;
                    }
                    factoryConfiguration.Workers = workers;
                }
                if (splittedLine[0] == "Dealers")
                {
                    if (false == uint.TryParse(splittedLine[1], out uint dealers))
                    {
                        continue;
                    }
                    factoryConfiguration.Dealers = dealers;
                }
                if (splittedLine[0] == "WriteToLog")
                {
                    if (false == bool.TryParse(splittedLine[1], out bool writeToLog))
                    {
                        continue;
                    }
                    factoryConfiguration.WriteToLog = writeToLog;
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

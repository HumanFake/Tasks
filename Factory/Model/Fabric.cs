using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace Model
{
    public class Fabric
    {
        private MotorStorage _motorStorage;
        private CarStorage _carStorage;
        private List<MotorSupplier> _motorMotorSuppliers = new List<MotorSupplier>();
        private List<Dealer> _dealers = new List<Dealer>();

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly FactoryConfiguration _factoryConfiguration;
        private readonly Utils.ThreadPool _threadPool;

        public Fabric()
        {
            _factoryConfiguration = FactoryConfigurationParser.Parse();
            _threadPool = new Utils.ThreadPool(_factoryConfiguration.Workers, nameof(Fabric));
        }

        public void Start()
        {
            _motorStorage = new MotorStorage(_factoryConfiguration.MotorStorageCapacity);
            _carStorage = new CarStorage(_factoryConfiguration.CarStorageCapacity);

            for (int i = 0; i < _factoryConfiguration.Dealers; i++)
            {
                _dealers.Add(new Dealer(_carStorage, _cancellationTokenSource.Token));
            }

            _motorMotorSuppliers.Add(new MotorSupplier(_motorStorage, _cancellationTokenSource.Token));

            _carStorage.StorageChanged += OnStorageChange;
            RealWork();
        }

        private void RealWork()
        {
            var popMotor = _motorStorage.PopMotor();

            var car = new Car(popMotor, Guid.NewGuid().ToString());
            _carStorage.AddCar(car);
            Console.WriteLine("new car");
        }

        private void OnStorageChange([NotNull] object sender, [NotNull] NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                _threadPool.Dispatch(RealWork);
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
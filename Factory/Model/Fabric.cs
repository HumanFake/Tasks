using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace Model
{
    public class Fabric
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly List<Dealer> _dealers = new List<Dealer>();
        private readonly FactoryConfiguration _factoryConfiguration;
        private readonly Utils.ThreadPool _threadPool;

        private Storage<Motor> _motorStorage;
        private Supplier<Motor> _motorSupplier;
        private Storage<Body> _bodyStorage;
        private Supplier<Body> _bodySupplier;

        private CarStorage _carStorage;

        public Fabric()
        {
            _factoryConfiguration = FactoryConfigurationParser.Parse();
            _threadPool = new Utils.ThreadPool(_factoryConfiguration.Workers, nameof(Fabric));
        }

        public void Start()
        {
            _motorStorage = new Storage<Motor>(_factoryConfiguration.MotorStorageCapacity);
            _motorSupplier = new Supplier<Motor>(_motorStorage, _cancellationTokenSource.Token);
            _motorSupplier.SetSupplyTime(200);

            _bodyStorage = new Storage<Body>(_factoryConfiguration.BodyStorageCapacity);
            _bodySupplier = new Supplier<Body>(_bodyStorage, _cancellationTokenSource.Token);
            _bodySupplier.SetSupplyTime(500);

            _carStorage = new CarStorage(_factoryConfiguration.CarStorageCapacity);

            for (int i = 0; i < _factoryConfiguration.Dealers; i++)
            {
                _dealers.Add(new Dealer(_carStorage, _cancellationTokenSource.Token));
            }

            _carStorage.StorageChanged += OnStorageChange;
            RealWork();
        }

        private void RealWork()
        {
            var motor = _motorStorage.Pop();
            var body = _bodyStorage.Pop();

            var car = new Car(motor, body, Guid.NewGuid().ToString());
            _carStorage.AddCar(car);
            Console.WriteLine(@"new car");
        }

        private void OnStorageChange(NotifyStorageChangedAction action)
        {
            if (action == NotifyStorageChangedAction.Remove)
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
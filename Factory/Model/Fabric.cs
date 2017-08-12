using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Model.Observers;

namespace Model
{
    public class Fabric
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly List<Dealer> _dealers = new List<Dealer>();
        private readonly FactoryConfiguration _factoryConfiguration;
        private readonly Utils.ThreadPool _threadPool;
        private readonly CarStorageController _storageController;
        private readonly Storage<Motor> _motorStorage;
        private readonly Storage<Body> _bodyStorage;
        private readonly Storage<Accessory> _accessorStorage;
        private readonly CarStorage _carStorage;

        private readonly List<Supplier<Accessory>> _accessorSuppliers = new List<Supplier<Accessory>>();

        private Supplier<Motor> _motorSupplier;
        private Supplier<Body> _bodySupplier;

        public Fabric([NotNull] IStorageObserver observer)
        {
            _factoryConfiguration = FactoryConfigurationParser.Parse();
            _threadPool = new Utils.ThreadPool(_factoryConfiguration.Workers, nameof(Fabric));

            _motorStorage = new Storage<Motor>(_factoryConfiguration.MotorStorageCapacity, observer);
            _bodyStorage = new Storage<Body>(_factoryConfiguration.BodyStorageCapacity, observer);
            _carStorage = new CarStorage(_factoryConfiguration.CarStorageCapacity, observer);
            _accessorStorage = new Storage<Accessory>(_factoryConfiguration.AccessoryStorageCapacity, observer);

            _storageController = new CarStorageController(this, _carStorage);
        }

        public void Start()
        {
            _motorSupplier = new Supplier<Motor>(_motorStorage, _cancellationTokenSource.Token);

            _bodySupplier = new Supplier<Body>(_bodyStorage, _cancellationTokenSource.Token);

            for (int i = 0; i < _factoryConfiguration.AccessorySupplier; i++)
            {
                var accessorSupplier = new Supplier<Accessory>(_accessorStorage, _cancellationTokenSource.Token);
                _accessorSuppliers.Add(accessorSupplier);
            }

            for (int i = 0; i < _factoryConfiguration.Dealers; i++)
            {
                _dealers.Add(new Dealer(_carStorage, _cancellationTokenSource.Token));
            }

            CreateNewCar();
        }

        public int GetCarsInStorage()
        {
            return _carStorage.Capacity;
        }

        public StorageState GetMotorStorageSate()
        {
            var state = new StorageState(_motorStorage.InStock, _motorStorage.ProductsInStorageForAllTime);
            return state;
        }

        public StorageState GetBodyStorageState()
        {
            var state = new StorageState(_bodyStorage.InStock, _bodyStorage.ProductsInStorageForAllTime);
            return state;
        }

        public StorageState GetAccessoryStorageState()
        {
            var state = new StorageState(_accessorStorage.InStock, _accessorStorage.ProductsInStorageForAllTime);
            return state;
        }

        public void SetMotorSupplyTime(uint time)
        {
            _motorSupplier.SetSupplyTime(time);
        }

        public void SetBodySupplyTime(uint time)
        {
            _bodySupplier.SetSupplyTime(time);
        }

        public void SetAccessorySupplyTime(uint time)
        {
            foreach (var accessorSupplier in _accessorSuppliers)
            {
                accessorSupplier.SetSupplyTime(time);
            }
        }

        public void SetDealerSupplyTime(uint time)
        {
            foreach (var dealer in _dealers)
            {
                dealer.SetReleaseTimeInMillisecond(time);
            }
        }

        private void RealWork()
        {
            var motor = _motorStorage.Pop();
            var body = _bodyStorage.Pop();
            var accessory = _accessorStorage.Pop();

            var car = new Car(motor, body, accessory, Guid.NewGuid().ToString());
            _carStorage.AddCar(car);
        }

        internal void CreateNewCar()
        {
            _threadPool.Dispatch(RealWork);
        }
        
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _threadPool.Dispose();
        }
    }
}
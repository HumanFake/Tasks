using System;
using System.Collections.Generic;
using System.Threading;
using Model.Observers;

namespace Model
{
    public class Fabric
    {
        private readonly IStorageObserver _observer;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly List<Dealer> _dealers = new List<Dealer>();
        private readonly FactoryConfiguration _factoryConfiguration;
        private readonly Utils.ThreadPool _threadPool;
        private readonly CarStorageController _storageController;

        private Storage<Motor> _motorStorage;
        private Supplier<Motor> _motorSupplier;
        private Storage<Body> _bodyStorage;
        private Supplier<Body> _bodySupplier;

        private CarStorage _carStorage;

        public Fabric(IStorageObserver observer)
        {
            _observer = observer;

            _factoryConfiguration = FactoryConfigurationParser.Parse();
            _threadPool = new Utils.ThreadPool(_factoryConfiguration.Workers, nameof(Fabric));

            _motorStorage = new Storage<Motor>(_factoryConfiguration.MotorStorageCapacity, _observer);
            _bodyStorage = new Storage<Body>(_factoryConfiguration.BodyStorageCapacity, _observer);
            _carStorage = new CarStorage(_factoryConfiguration.CarStorageCapacity, _observer);

            _storageController = new CarStorageController(this, _carStorage);
        }

        public void Start()
        {
            _motorSupplier = new Supplier<Motor>(_motorStorage, _cancellationTokenSource.Token);
            _motorSupplier.SetSupplyTime(200);

            _bodySupplier = new Supplier<Body>(_bodyStorage, _cancellationTokenSource.Token);
            _bodySupplier.SetSupplyTime(500);

            for (int i = 0; i < _factoryConfiguration.Dealers; i++)
            {
                _dealers.Add(new Dealer(_carStorage, _cancellationTokenSource.Token));
            }

            CreateNewCar();
        }

        public int CarsInStorage()
        {
            return _carStorage.Capacity;
        }

        public Storage<Motor> GetMotorStorage()
        {
            return _motorStorage;
        }

        public Storage<Body> GetBodyStorage()
        {
            return _bodyStorage;
        }

        public void SetMotorSupplyTime(uint time)
        {
            _motorSupplier.SetSupplyTime(time);
        }

        public void SetBodySupplyTime(uint time)
        {
            _bodySupplier.SetSupplyTime(time);
        }

        private void RealWork()
        {
            var motor = _motorStorage.Pop();
            var body = _bodyStorage.Pop();

            var car = new Car(motor, body, Guid.NewGuid().ToString());
            _carStorage.AddCar(car);
            Console.WriteLine(@"new car");
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
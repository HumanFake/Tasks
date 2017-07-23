using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

namespace Model
{
    public class Fabric
    {
        private const int DefaultWorkerCount = 5;
        private const int DefaultMotorSupplierCount = 2;
        private const int DefaultDialerCount = 2;

        private const int DefaultMotorStorageCapacity = 5;
        private const int DefaultCarStorageCapacity = 10;

        private MotorStorage _motorStorage;
        private CarStorage _carStorage;
        private List<MotorSupplier> _motorMotorSuppliers = new List<MotorSupplier>();
        private List<Dealer> _dealers = new List<Dealer>();

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Fabric()
        {
            var t = FactoryConfigurationParser.Parse();
        }

        public void Start()
        {
            _motorStorage = new MotorStorage(DefaultMotorStorageCapacity);
            _carStorage = new CarStorage(DefaultCarStorageCapacity);

            for (int i = 0; i < DefaultDialerCount; i++)
            {
                _dealers.Add(new Dealer(_carStorage, _cancellationTokenSource.Token));
            }
            for (int i = 0; i < DefaultMotorSupplierCount; i++)
            {
                _motorMotorSuppliers.Add(new MotorSupplier(_motorStorage, _cancellationTokenSource.Token));
            }

            _carStorage.StorageChanged += OnStorageChange;
            RealWork(null);
        }

        internal void RealWork(object state)
        {
            var popMotor = _motorStorage.PopMotor();
            Console.WriteLine(popMotor.Id + " : get from storage");

            var car = new Car(popMotor, Guid.NewGuid().ToString());
            _carStorage.AddCar(car);
        }

        internal void OnStorageChange([NotNull] object sender, [NotNull] NotifyCollectionChangedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(RealWork);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
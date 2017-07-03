using System;
using System.Collections.Generic;
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

            var emulateWork = new Task(() => EmulateWorker(_cancellationTokenSource.Token));
            emulateWork.Start();
        }

        private void EmulateWorker(CancellationToken cancellationToken)
        {
            const int maxSleepTime = 2000;
            const int sleepIteration = 500;

            var sleepTime = 0;

            while (false == cancellationToken.IsCancellationRequested)
            {
                var popMotor = _motorStorage.PopMotor();

                Console.WriteLine(popMotor.Id + " : get from storage");
                
                Thread.Sleep(sleepTime);

                sleepTime += sleepIteration;
                if (sleepTime > maxSleepTime)
                {
                    sleepTime = 0;
                }
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
using System.Threading;

namespace Model
{
    public sealed class MotorSupplier
    {
        private const int DefaultSupplyTimeInMillisecond = 3000;
        
        private readonly string _supplierId;
        private readonly object _monitor = new object();
        private readonly MotorStorage _storage;

        private int _currentMotorId;
        private Motor _motor;

        public MotorSupplier(MotorStorage storage, CancellationToken cancellationToken)
        {
            _supplierId = System.Guid.NewGuid().ToString();
            _storage = storage;

            var thread = new Thread(() => { Start(cancellationToken); });
            thread.Start();
        }

        private void Start(CancellationToken cancellationToken)
        {
            lock (_monitor)
            {
                while (false == cancellationToken.IsCancellationRequested)
                {
                    CreateNewMotor();

                    var motorId = _supplierId + "__" + _currentMotorId;
                    _currentMotorId++;

                    _motor = new Motor(motorId);

                    _storage.AddMotor(_motor);
                }
            }
        }

        private static void CreateNewMotor()
        {
            Thread.Sleep(DefaultSupplyTimeInMillisecond);
        }

        public void SupplyToStorage(MotorStorage motorStorage)
        {
            lock (_monitor)
            {
                Monitor.PulseAll(_monitor);
                if (_motor == null)
                {
                    return;
                }

                motorStorage.AddMotor(_motor);
                _motor = null;
            }
        }
    }
}
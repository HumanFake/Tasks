using System.Threading;

namespace Model
{
    public class Dealer
    {
        private const int DefaultReleaseTimeInMillisecond = 3000;

        private readonly string _dealerId;
        private CarStorage _storage;
        private readonly object _monitor = new object();

        private int _currentCarId;

        public Dealer(CarStorage storage, CancellationToken cancellationToken)
        {
            _dealerId = System.Guid.NewGuid().ToString();
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

                    var motorId = _dealerId + "__" + _currentCarId;
                    _currentCarId++;

                    _storage.PopCar();
                }
            }
        }

        private static void CreateNewMotor()
        {
            Thread.Sleep(DefaultReleaseTimeInMillisecond);
        }
    }
}
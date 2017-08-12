using System.Threading;
using System;

namespace Model
{
    public class Dealer
    {
        private const int DefaultReleaseTimeInMillisecond = 2000;

        private readonly string _dealerId;
        private readonly CarStorage _storage;
        private readonly object _monitor = new object();

        private uint _releaseTimeInMillisecond = DefaultReleaseTimeInMillisecond;

        public Dealer(CarStorage storage, CancellationToken cancellationToken)
        {
            _dealerId = Guid.NewGuid().ToString();
            _storage = storage;

            var thread = new Thread(() => { Start(cancellationToken); });
            thread.Start();
        }

        internal void SetReleaseTimeInMillisecond(uint time)
        {
            _releaseTimeInMillisecond = time;
        }

        private void Start(CancellationToken cancellationToken)
        {
            lock (_monitor)
            {
                while (false == cancellationToken.IsCancellationRequested)
                {
                    ReleaseCar();
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    var car = _storage.PopCar();
                    Console.WriteLine(GenerateCarInformation(car));
                }
            }
        }

        private string GenerateCarInformation(Car car)
        {
            var result = $"{DateTime.Now}: Dealer {_dealerId}:" +
                $" Auto {car.Id} (Body: <{car.Body.Id}>, Motor: {car.Motor.Id}, Accessory: <ID>)";

            return result;
        }

        private void ReleaseCar()
        {
            Thread.Sleep((int) _releaseTimeInMillisecond);
        }
    }
}
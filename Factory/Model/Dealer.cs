using System.Threading;
using System;

namespace Model
{
    public class Dealer
    {
        private const int DefaultReleaseTimeInMillisecond = 3000;

        private readonly string _dealerId;
        private readonly CarStorage _storage;
        private readonly object _monitor = new object();
        
        public Dealer(CarStorage storage, CancellationToken cancellationToken)
        {
            _dealerId = Guid.NewGuid().ToString();
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
                    RealeseCar();
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    Console.WriteLine("Need car");
                    var car = _storage.PopCar();
                }
            }
        }

        private string GenerateCarInforamtion(Car car)
        {
            var result = $"{DateTime.Now.ToString()}: Dealer {_dealerId}:" +
                $" Auto {car.Id} (Body: <ID>, Motor: {car.Motor.Id}, Accessory: <ID>)";

            return result;
        }

        private static void RealeseCar()
        {
            Thread.Sleep(DefaultReleaseTimeInMillisecond);
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Model
{
    public class CarStorage
    {
        private readonly int _maxCapacity;
        private readonly ObservableCollection<Car> _cars = new ObservableCollection<Car>();

        public CarStorage(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
            _cars.CollectionChanged += OnStorageChange;
        }

        internal NotifyCollectionChangedEventHandler StorageChanged;

        private void OnStorageChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            StorageChanged.Invoke(sender, e);
        }

        internal void AddCar([NotNull] Car motor)
        {
            lock (_cars)
            {
                Monitor.PulseAll(_cars);
                while (_cars.Count >= _maxCapacity)
                {
                    Monitor.Wait(_cars);
                }

                Console.WriteLine(motor.Id + $" : add to CarStorage. Current capacity: {_cars.Count + 1}");
                _cars.Add(motor);
            }
        }

        public Car PopCar()
        {
            lock (_cars)
            {
                Monitor.PulseAll(_cars);
                while (_cars.Count == 0)
                {
                    Monitor.Wait(_cars);
                }

                var result = _cars.First();
                _cars.Remove(result);

                return result;
            }
        }

        private int Capacity => _cars.Count;
    }
}
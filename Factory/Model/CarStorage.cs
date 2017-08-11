using JetBrains.Annotations;
using System.Collections.Specialized;
using Model.Observers;

namespace Model
{
    public class CarStorage
    {
        private readonly Storage<Car> _storage;

        public CarStorage(uint maxCapacity, IStorageObserver observer)
        {
            _storage = new Storage<Car>(maxCapacity, observer);
        }

        internal NotifyStorageChanged StorageChanged;

        internal void AddCar([NotNull] Car car)
        {
            _storage.Add(car);
            NotifyAboutAdding();
        }

        public Car PopCar()
        {
            var car = _storage.Pop();
            NotifyAboutRemoving();

            return car;
        }

        private void NotifyAboutAdding()
        {
            StorageChanged?.Invoke(NotifyStorageChangedAction.Add);
        }

        private void NotifyAboutRemoving()
        {
            StorageChanged?.Invoke(NotifyStorageChangedAction.Remove);
        }

        public int Capacity => _storage.Capacity;
    }

    public delegate void NotifyStorageChanged(NotifyStorageChangedAction action);

    public enum NotifyStorageChangedAction
    {
        Add,
        Remove
    }
}
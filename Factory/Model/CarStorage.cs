﻿using JetBrains.Annotations;
using Model.Observers;

namespace Model
{
    public class CarStorage
    {
        private readonly Storage<Car> _storage;

        public CarStorage(uint maxCapacity, IStorageObserver observer)
        {
            MaxCapacity = maxCapacity;
            _storage = new Storage<Car>(maxCapacity, observer);
        }

        public int Capacity => _storage.InStock;
        internal uint MaxCapacity { get; }

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
    }

    public delegate void NotifyStorageChanged(NotifyStorageChangedAction action);
}
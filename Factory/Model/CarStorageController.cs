using JetBrains.Annotations;
using System.Collections.Specialized;
using System.Threading;

namespace Model
{
    internal class CarStorageController
    {
        private readonly Fabric _fabric;
        private readonly CarStorage _carStorage;

        internal CarStorageController([NotNull] Fabric fabric, CarStorage carStorage)
        {
            _fabric = fabric;
            _carStorage = carStorage;

            _carStorage.StorageChanged += OnStorageChange;
        }
        
        private void OnStorageChange(NotifyStorageChangedAction action)
        {
            _fabric.CreateNewCar();
        }
    }
}

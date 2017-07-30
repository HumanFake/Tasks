using JetBrains.Annotations;
using System.Collections.Specialized;

namespace Model
{
    internal class CarStorageController
    {
        private readonly Fabric _fabric;
        
        internal CarStorageController([NotNull] Fabric fabric)
        {
            _fabric = fabric;
        }

        internal void OnStorageChange([NotNull] object sender, [NotNull] NotifyCollectionChangedEventArgs e)
        {

        }
    }
}

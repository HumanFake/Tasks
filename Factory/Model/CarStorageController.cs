using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

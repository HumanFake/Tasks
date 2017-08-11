using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using Model.Observers;

namespace Model
{
    public class Storage<T> where T : IProduct
    {
        private readonly uint _maxCapacity;
        private readonly IStorageObserver _observer;
        private readonly List<T> _products = new List<T>();

        private int _totalProductsWasInStorag = 0;

        public Storage(uint maxCapacity, IStorageObserver observer)
        {
            _maxCapacity = maxCapacity;
            _observer = observer;
        }

        internal void Add([NotNull] T motor)
        {
            lock (_products)
            {
                Monitor.PulseAll(_products);
                while (_products.Count >= _maxCapacity)
                {
                    Monitor.Wait(_products);
                }

                Console.WriteLine(typeof(T).Name + _products.Count);
                _products.Add(motor);

                _totalProductsWasInStorag++;
                _observer.OnStorageChange();
            }
        }

        internal T Pop()
        {
            lock (_products)
            {
                Monitor.PulseAll(_products);
                while (_products.Count == 0)
                {
                    Monitor.Wait(_products);
                }

                var result = _products.First();
                _products.Remove(result);
                _observer.OnStorageChange();

                return result;
            }
        }

        public int ProductsInStorageForAllTime => _totalProductsWasInStorag;
        public int Capacity => _products.Count;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace Model
{
    internal class Storage<T> where T : IProduct
    {
        private readonly uint _maxCapacity;
        private readonly List<T> _products = new List<T>();

        public Storage(uint maxCapacity)
        {
            _maxCapacity = maxCapacity;
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
            }
        }

        public T Pop()
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

                return result;
            }
        }

        public int Capacity => _products.Count;
    }
}
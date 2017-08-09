using System;
using System.Threading;

namespace Model
{
    internal class Supplier<T> where T : IProduct
    {
        private const uint DefaultSupplyTimeInMillisecond = 100;

        private readonly string _supplierId;
        private readonly object _monitor = new object();
        private readonly Storage<T> _storage;

        private uint _supplyTimeInMillisecond = DefaultSupplyTimeInMillisecond;
        private int _currentProductId;
        private T _product;

        public Supplier(Storage<T> storage, CancellationToken cancellationToken)
        {
            _supplierId = Guid.NewGuid().ToString();
            _storage = storage;

            var thread = new Thread(() => { Start(cancellationToken); });
            thread.Start();
        }

        public void SetSupplyTime(uint supplyTime)
        {
            _supplyTimeInMillisecond = supplyTime;
        }

        private void Start(CancellationToken cancellationToken)
        {
            lock (_monitor)
            {
                while (false == cancellationToken.IsCancellationRequested)
                {
                    _product = CreateNewProduct(); 

                    _storage.Add(_product);
                }
            }
        }

        private T CreateNewProduct()
        {
            var id = _supplierId + "__" + _currentProductId;
            _currentProductId++;
            try
            {
                var product = (T)Activator.CreateInstance(typeof(T), id);

                Creation();

                return product;
            }
            catch
            {
                throw new Exception($"can't instance with the ({id.GetType().Name}:{nameof(id)}) argument");
            }
        }

        private void Creation()
        {
            Thread.Sleep((int) _supplyTimeInMillisecond);
        }
    }
}
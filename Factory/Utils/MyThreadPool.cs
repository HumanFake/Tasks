using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Utils
{
    internal sealed class MyThreadPool : Disposable
    {
        private readonly Queue<Action> _actions = new Queue<Action>();
        private readonly List<Thread> _tasks = new List<Thread>();
        private readonly object _monitor = new object();

        private bool _interrupted = true;

        internal MyThreadPool(int threadCount)
        {
            if (threadCount <= 0)
            {
                throw new Exception($"{threadCount} must be greater than zero");
            }
            for (int i = 0; i < threadCount; i++)
            {
                _tasks.Add(new Thread(InternalAction));
                _tasks.First().Start();
            }
        }

        internal void Execute(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            lock (_monitor)
            {
                _actions.Enqueue(action);
                Monitor.PulseAll(_monitor);
            }
        }


        internal void Interrupt()
        {
            _interrupted = true;

            lock (_monitor)
            {
                Monitor.PulseAll(_monitor);
            }
            foreach (var task in _tasks)
            {
                task.Join();
                task.Interrupt();
            }
        }

        private void InternalAction()
        {
            lock (_monitor)
            {
                while (true)
                {
                    while (_actions.Count == 0 && _interrupted == false)
                    {
                        Monitor.Wait(_monitor);
                    }
                    if (_interrupted)
                    {
                        break;
                    }

                    var action = _actions.Dequeue();
                    action.Invoke();
                }
            }
        }
    }

}
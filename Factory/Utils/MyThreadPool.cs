using System;
using System.Collections.Concurrent;
using System.Threading;
using JetBrains.Annotations;

namespace Utils
{
    public sealed class ThreadPool : Disposable
    {
        private const int MaxQueueSize = 1024 * 1024;
        private const int JoinTimeoutMilliseconds = 500;

        private readonly Thread[] _executors;
        private readonly BlockingCollection<Action> _tasks;

        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        public ThreadPool(uint threadCount, [NotNull] string threadPoolName)
        {
            if (threadPoolName == null)
            {
                throw new ArgumentNullException(nameof(threadPoolName));
            }

            _tasks = new BlockingCollection<Action>(MaxQueueSize);
            _executors = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(ThreadLoop) {Name = $"{threadPoolName} Thread {i}"};
                thread.Start(_cancellation.Token);

                _executors[i] = thread;
            }
        }

        public void Dispatch([NotNull] Action job)
        {
            ThrowIfDisposed();

            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            _tasks.Add(job, _cancellation.Token);
        }

        private void ThreadLoop([CanBeNull] object state)
        {
            ThrowIfDisposed();

            var token = state as CancellationToken? ?? CancellationToken.None;

            try
            {
                while (false == token.IsCancellationRequested)
                {
                    _tasks.Take(token).Invoke();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        protected override void FreeManagedResources()
        {
            _cancellation.Cancel();

            foreach (var executor in _executors)
            {
                if (false == executor.Join(JoinTimeoutMilliseconds))
                {
                    executor.Abort();
                }
            }

            _tasks.Dispose();
            _cancellation.Dispose();
        }

    }
}
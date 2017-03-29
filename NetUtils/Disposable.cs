using System;

namespace NetUtils
{
    public abstract class Disposable : IDisposable
    {
        private bool _disposed;

        ~Disposable()
        {
            Dispose(false);
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                FreeManagedResources();
            }

            FreeUnmanagedResources();

            _disposed = true;
        }

        // может вызываться в финализаторе
        protected virtual void FreeUnmanagedResources()
        {
        }

        // гарантированно не будет вызываться в финализаторе
        protected virtual void FreeManagedResources()
        {
        }
    }
}
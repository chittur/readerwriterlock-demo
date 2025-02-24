/******************************************************************************
 * Filename    = ReaderWriterLock.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = ReaderWriterSynchronization
 * 
 * Project     = Synchronization
 *
 * Description = Defines a simple reader-writer lock implementation.
 *****************************************************************************/

using System.Diagnostics;

namespace Synchronization
{
    /// <summary>
    /// A simple reader-writer lock implementation.
    /// </summary>
    public class ReaderWriterLock
    {
        private readonly object _readerLock;
        private readonly object _writerLock;
        private int _readerCount;
        private readonly AutoResetEvent _noReaders;

        private readonly LockPolicy _lockPolicy;
        private readonly int _maxReaders;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderWriterLock"/> class.
        /// </summary>
        public ReaderWriterLock(LockPolicy lockPolicy = LockPolicy.Basic, int maxReaders = 5 /* A reasonable balance to avoid writer starvation */)
        {
            _readerLock = new object();
            _writerLock = new object();
            _readerCount = 0;
            _noReaders = new AutoResetEvent(false);
            _lockPolicy = lockPolicy;
            _maxReaders = maxReaders;
        }

        /// <summary>
        /// Enters the read lock.
        /// </summary>
        public void EnterReadLock()
        {
            if (_lockPolicy == LockPolicy.MaxReadersBeforeWriter)
            {
                Monitor.Enter(_readerLock);
                if (_readerCount >= _maxReaders)
                {
                    Monitor.Exit(_readerLock);
                    EnterWriteLock();
                    ExitWriteLock();
                }
                else
                {
                    Monitor.Exit(_readerLock);
                }
            }

            Monitor.Enter(_writerLock);
            Monitor.Enter(_readerLock);
            ++_readerCount;
            Monitor.Exit(_readerLock);
            Monitor.Exit(_writerLock);
        }

        /// <summary>
        /// Enters the write lock.
        /// </summary>
        public void EnterWriteLock()
        {
            Monitor.Enter(_writerLock);
            Monitor.Enter(_readerLock);
            while (_readerCount > 0)
            {
                Monitor.Exit(_readerLock);
                _noReaders.WaitOne();
                Monitor.Enter(_readerLock);
            }
            Monitor.Exit(_readerLock);
        }

        /// <summary>
        /// Exits the read lock.
        /// </summary>
        public void ExitReadLock()
        {
            Monitor.Enter(_readerLock);
            Debug.Assert(_readerCount > 0);
            --_readerCount;
            if (_readerCount == 0)
            {
                _noReaders.Set();
            }
            Monitor.Exit(_readerLock);
        }

        /// <summary>
        /// Exits the write lock.
        /// </summary>
        public void ExitWriteLock()
        {
            Debug.Assert(Monitor.IsEntered(_writerLock));
            Monitor.Exit(_writerLock);
        }
    }

    public enum LockPolicy
    {
        Basic,
        MaxReadersBeforeWriter
    }
}

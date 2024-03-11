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

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderWriterLock"/> class.
        /// </summary>
        public ReaderWriterLock()
        {
            _readerLock = new object();
            _writerLock = new object();
            _readerCount = 0;
            _noReaders = new AutoResetEvent(false);
        }

        /// <summary>
        /// Enters the read lock.
        /// </summary>
        public void EnterReadLock()
        {
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
}

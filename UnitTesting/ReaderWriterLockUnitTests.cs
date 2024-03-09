/******************************************************************************
 * Filename    = ReaderWriterLockUnitTests.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = ReaderWriterSynchronization
 * 
 * Project     = UnitTesting
 *
 * Description = Defines the unit tests for ReaderWriterLock.
 *****************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using ReaderWriterLock = Synchronization.ReaderWriterLock;

namespace UnitTesting
{
    /// <summary>
    /// Unit tests for ReaderWriterLock.
    /// </summary>
    [TestClass]
    public class ReaderWriterLockUnitTests
    {
        /// <summary>
        /// Basic unit test for ReaderWriterLock.
        /// </summary>
        [TestMethod]
        public void BasicTest()
        {
            ReaderWriterLock readerWriterLock = new();
            Logger.LogMessage("Entering read lock.");
            readerWriterLock.EnterReadLock();
            Logger.LogMessage("Entered read lock.");
            Logger.LogMessage("Exiting read lock.");
            readerWriterLock.ExitReadLock();
            Logger.LogMessage("Exited read lock.");
            Logger.LogMessage("Entering write lock.");
            readerWriterLock.EnterWriteLock();
            Logger.LogMessage("Entered write lock.");
            Logger.LogMessage("Exiting write lock.");
            readerWriterLock.ExitWriteLock();
            Logger.LogMessage("Exited write lock.");
        }

        /// <summary>
        /// Basic unit test for ReaderWriterLock using multiple threads.
        /// </summary>
        [TestMethod]
        public void BasicTestMultipleThreads()
        {
            ReaderWriterLock readerWriterLock = new();
            Thread thread1 = new(() =>
            {
                Logger.LogMessage("Thread1: Entering read lock.");
                readerWriterLock.EnterReadLock();
                Logger.LogMessage("Thread1: Entered read lock.");
                Thread.Sleep(GetRandomDelay());
                Logger.LogMessage("Thread1: Exiting read lock.");
                readerWriterLock.ExitReadLock();
                Logger.LogMessage("Thread1: Exited read lock.");
            });
            Thread thread2 = new(() =>
            {
                Logger.LogMessage("Thread2: Entering read lock.");
                readerWriterLock.EnterReadLock();
                Logger.LogMessage("Thread2: Entered read lock.");
                Thread.Sleep(GetRandomDelay());
                Logger.LogMessage("Thread2: Exiting read lock.");
                readerWriterLock.ExitReadLock();
                Logger.LogMessage("Thread2: Exited read lock.");
            });
            Thread thread3 = new(() =>
            {
                Logger.LogMessage("Thread3: Entering write lock.");
                readerWriterLock.EnterWriteLock();
                Logger.LogMessage("Thread3: Entered write lock.");
                Thread.Sleep(GetRandomDelay());
                Logger.LogMessage("Thread3: Exiting write lock.");
                readerWriterLock.ExitWriteLock();
                Logger.LogMessage("Thread3: Exited write lock.");
            });
            Thread thread4 = new(() =>
            {
                Logger.LogMessage("Thread4: Entering write lock.");
                readerWriterLock.EnterWriteLock();
                Logger.LogMessage("Thread4: Entered write lock.");
                Thread.Sleep(GetRandomDelay());
                Logger.LogMessage("Thread4: Exiting write lock.");
                readerWriterLock.ExitWriteLock();
                Logger.LogMessage("Thread4: Exited write lock.");
            });

            Logger.LogMessage("Starting threads.");
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();

            Logger.LogMessage("Waiting for threads to complete.");
            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

            Logger.LogMessage("Threads completed.");
        }

        /// <summary>
        /// Gets a random delay.
        /// </summary>
        /// <returns>Gets a random delay.</returns>
        private int GetRandomDelay()
        {
            return new Random().Next(1000);
        }
    }
}
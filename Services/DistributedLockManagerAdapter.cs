using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.FileSystems.LockFile;
using Piedone.HelpfulLibraries.Tasks.Locking;

namespace Lombiq.Hosting.Azure.Lucene.Services
{
    /// <summary>
    /// For usage in <see cref="Lombiq.Hosting.Azure.Indexing.Services.IndexingTaskExecutorDecorator"/> so it doesn't use the
    /// file system.
    /// </summary>
    internal class DistributedLockManagerAdapter : ILockFileManager
    {
        private readonly IDistributedLockManager _distributedLockManager;


        public DistributedLockManagerAdapter(IDistributedLockManager distributedLockManager)
        {
            _distributedLockManager = distributedLockManager;
        }
        
		
        public bool TryAcquireLock(string path, ref ILockFile lockFile)
        {
            var locker = _distributedLockManager.TryAcquireLock(SanitizePath(path));
            if (locker == null) return false;

            lockFile = new DistributedLockAdapter(locker);
            return true;
        }

        public bool IsLocked(string path)
        {
            throw new NotImplementedException();
        }


        private static string SanitizePath(string path)
        {
            return path.Replace('/', '-');
        }


        private class DistributedLockAdapter : ILockFile
        {
            private readonly IDistributedLock _distributedLock;


            public DistributedLockAdapter(IDistributedLock distributedLock)
            {
                _distributedLock = distributedLock;
            }
        
		
            public void Release()
            {
                _distributedLock.Dispose();
            }

            public void Dispose()
            {
                _distributedLock.Dispose();
            }
        }
    }
}
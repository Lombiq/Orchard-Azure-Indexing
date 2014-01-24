using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac.Core;
using Orchard.Indexing.Services;
using Autofac;
using Orchard.FileSystems.AppData;
using Orchard.Data;
using Lombiq.Hosting.Azure.Lucene.Models;
using Piedone.HelpfulLibraries.Tasks.Locking;
using Orchard.FileSystems.LockFile;

namespace Lombiq.Hosting.Azure.Lucene.Services
{
    public class IndexingTaskExecutorModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += (sender, e) =>
                {
                    if (e.Component.Activator.LimitType != typeof(IndexingTaskExecutor)) return;

                    // Changing the IAppDataFolder implementation to our own.
                    var dbAppDataFolder = new DbAppDataFolder(e.Context.Resolve<IRepository<AppDataFileRecord>>());
                    var lockFileManager = new DistributedLockManagerAdapter(e.Context.Resolve<IDistributedLockManager>());
                    e.Parameters = e.Parameters.Concat(new[]
                    {
                        new TypedParameter(typeof(IAppDataFolder), dbAppDataFolder),
                        new TypedParameter(typeof(ILockFileManager), lockFileManager)
                    });
                };
        }
    }
}
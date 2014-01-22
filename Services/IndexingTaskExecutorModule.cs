﻿using System;
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

namespace Lombiq.Hosting.Azure.Lucene.Services
{
    public class IndexingTaskExecutorModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += (sender, e) =>
                {
                    if (e.Component.Activator.LimitType.Name != typeof(IndexingTaskExecutor).Name) return;

                    // Changing the IAppDataFolder implementation to our own.
                    var dbAppDataFolder = new DbAppDataFolder(e.Context.Resolve<IRepository<AppDataFileRecord>>());
                    e.Parameters = e.Parameters.Concat(new[] { new TypedParameter(typeof(IAppDataFolder), dbAppDataFolder) });
                };
        }
    }
}
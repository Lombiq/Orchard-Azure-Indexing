using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Hosting.Azure.Indexing;
using Orchard;
using Orchard.Azure.Services.Environment.Configuration;
using Orchard.Azure.Services.FileSystems;
using Orchard.Environment.Extensions;

namespace Lombiq.Hosting.Azure.Lucene.Services
{
    public interface ILuceneAzureFileSystemFactory : IDependency
    {
        AzureFileSystem Create(string shellName);
    }


    [OrchardFeature("Lombiq.Hosting.Azure.Indexing.Lucene")]
    public class LuceneAzureFileSystemFactory : ILuceneAzureFileSystemFactory
    {
        public AzureFileSystem Create(string shellName)
        {
            var storageConnectionString = PlatformConfiguration.GetSetting(Constants.LuceneStorageStorageConnectionStringSettingName, shellName) ?? "UseDevelopmentStorage=true";
            return new AzureFileSystem(storageConnectionString, "lucene", shellName, true, null);
        }
    }
}
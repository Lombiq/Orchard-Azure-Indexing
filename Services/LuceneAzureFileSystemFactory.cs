using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Hosting.Azure.Indexing;
using Orchard;
using Orchard.Azure.Services.Environment.Configuration;
using Orchard.Azure.Services.FileSystems;
using Orchard.Environment.Extensions;

namespace Lombiq.Hosting.Azure.Indexing.Services
{
    public interface ILuceneAzureFileSystemFactory : IDependency
    {
        AzureFileSystem Create(string shellName);
    }


    [OrchardFeature("Lombiq.Hosting.Azure.Indexing.Lucene")]
    public class LuceneAzureFileSystemFactory : ILuceneAzureFileSystemFactory
    {
        private readonly IPlatformConfigurationAccessor _pca;


        public LuceneAzureFileSystemFactory(IPlatformConfigurationAccessor pca)
        {
            _pca = pca;
        }


        public AzureFileSystem Create(string shellName)
        {
            var storageConnectionString = _pca.GetSetting(Constants.LuceneStorageStorageConnectionStringSettingName, shellName, null) ?? "UseDevelopmentStorage=true";
            return new AzureFileSystem(storageConnectionString, "lucene", shellName, true, null);
        }
    }
}
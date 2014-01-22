using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Store.Azure;
using Lucene.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Orchard.Azure.Services.Environment.Configuration;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.AppData;
using Orchard.Indexing;

namespace Lombiq.Hosting.Azure.Lucene.Services
{
    [OrchardSuppressDependency("Lucene.Services.LuceneIndexProvider")]
    public class AzureLuceneIndexProvider : LuceneIndexProvider, IIndexProvider
    {
        private readonly CloudStorageAccount _storageAccount;

        private readonly IAppDataFolder _appDataFolder;
        private readonly ShellSettings _shellSettings;


        public AzureLuceneIndexProvider(IAppDataFolder appDataFolder, ShellSettings shellSettings)
            : base(new StubAppData(appDataFolder), shellSettings)
        {
            _appDataFolder = appDataFolder;
            _shellSettings = shellSettings;

            // Not nice in the ctor but since this is a singleton it's simpler than caring about locking and happens once anyway.
            var storageConnectionString = PlatformConfiguration.GetSetting(Constants.LuceneStorageStorageConnectionStringSettingName, _shellSettings.Name) ?? "UseDevelopmentStorage=true";
            _storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            var blobClient = _storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("search-indices");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Off);
        }


        bool IIndexProvider.Exists(string name)
        {
            return false;
            throw new NotImplementedException();
        }

        IEnumerable<string> IIndexProvider.List()
        {
            return Enumerable.Empty<string>();
        }

        void IIndexProvider.DeleteIndex(string name)
        {
            throw new NotImplementedException();
        }

        bool IIndexProvider.IsEmpty(string indexName)
        {
            if (!((IIndexProvider)this).Exists(indexName))
            {
                return true;
            }

            using (var reader = IndexReader.Open(GetDirectory(indexName), true))
            {
                return reader.NumDocs() == 0;
            }
        }

        int IIndexProvider.NumDocs(string indexName)
        {
            if (!((IIndexProvider)this).Exists(indexName))
            {
                return 0;
            }

            using (var reader = IndexReader.Open(GetDirectory(indexName), true))
            {
                return reader.NumDocs();
            }
        }


        protected override Directory GetDirectory(string indexName)
        {
            if (_storageAccount == null)
            {
                //var blobClient = _storageAccount.CreateCloudBlobClient();
                //var container = blobClient.GetContainerReference("search-indices");
                //container.CreateIfNotExists(BlobContainerPublicAccessType.Off);
            }

            var cacheDirectoryPath = _appDataFolder.Combine("Sites", _shellSettings.Name, "IndexCaches", indexName);
            var cacheDirectoryInfo = new System.IO.DirectoryInfo(_appDataFolder.MapPath(cacheDirectoryPath));

            return new AzureDirectory(_storageAccount, "search-indices", FSDirectory.Open(cacheDirectoryInfo));
        }
    }
}
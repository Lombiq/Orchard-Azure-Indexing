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
using Orchard.Azure.Services.FileSystems;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.AppData;
using Orchard.Indexing;

namespace Lombiq.Hosting.Azure.Indexing.Services
{
    [OrchardSuppressDependency("Lucene.Services.LuceneIndexProvider")]
    [OrchardFeature("Lombiq.Hosting.Azure.Indexing.Lucene")]
    public class AzureLuceneIndexProvider : LuceneIndexProvider, IIndexProvider
    {
        private readonly AzureFileSystem _fileSystem;
        private readonly IAppDataFolder _appDataFolder;
        private readonly ShellSettings _shellSettings;


        public AzureLuceneIndexProvider(IAppDataFolder appDataFolder, ShellSettings shellSettings)
            : base(new StubAppData(appDataFolder), shellSettings)
        {
            _appDataFolder = appDataFolder;
            _shellSettings = shellSettings;

            var storageConnectionString = PlatformConfiguration.GetSetting(Constants.LuceneStorageStorageConnectionStringSettingName, _shellSettings.Name) ?? "UseDevelopmentStorage=true";
            _fileSystem = new AzureFileSystem(storageConnectionString, "lucene", _shellSettings.Name, true, null);
        }


        bool IIndexProvider.Exists(string name)
        {
            return _fileSystem.FolderExists(name);
        }

        IEnumerable<string> IIndexProvider.List()
        {
            // Will list the pseudo-folders of the indices.
            if (!_fileSystem.FolderExists(string.Empty)) return Enumerable.Empty<string>();
            return _fileSystem.ListFolders(string.Empty).Select(file => file.GetName());
        }

        void IIndexProvider.DeleteIndex(string name)
        {
            _fileSystem.DeleteFolder(name);
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
            var cacheDirectoryPath = _appDataFolder.Combine("Sites", _shellSettings.Name, "IndexingCache", indexName);
            var cacheDirectoryInfo = new System.IO.DirectoryInfo(_appDataFolder.MapPath(cacheDirectoryPath));

            return new AzureDirectory(
                _fileSystem.Container,
                _shellSettings.Name + "/" + indexName,
                FSDirectory.Open(cacheDirectoryInfo));
        }
    }
}
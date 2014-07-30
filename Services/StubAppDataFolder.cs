using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.FileSystems.AppData;

namespace Lombiq.Hosting.Azure.Indexing.Services
{
    /// <summary>
    /// Serves only to be fed to <see cref="Lucene.Services.LuceneIndexProvider"/>. Any method in the class that would use 
    /// <see cref="Orchard.FileSystems.AppData.IAppDataFolder"/> is overridden from <see cref="Lombiq.Hosting.Azure.Indexing.Services.AzureLuceneIndexProvider"/>.
    /// </summary>
    internal class StubAppDataFolder : IAppDataFolder
    {
        private readonly IAppDataFolder _appDataFolder;


        public StubAppDataFolder(IAppDataFolder appDataFolder)
        {
            _appDataFolder = appDataFolder;
        }


        public IEnumerable<string> ListFiles(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ListDirectories(string path)
        {
            throw new NotImplementedException();
        }

        public string Combine(params string[] paths)
        {
            return string.Empty;
        }

        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public void CreateFile(string path, string content)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public string ReadFile(string path)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public void StoreFile(string sourceFileName, string destinationPath)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public DateTime GetFileLastWriteTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public Orchard.Caching.IVolatileToken WhenPathChanges(string path)
        {
            throw new NotImplementedException();
        }

        public string MapPath(string path)
        {
            return _appDataFolder.MapPath(path);
        }

        public string GetVirtualPath(string path)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Lombiq.Hosting.Azure.Lucene.Models;
using Orchard.Data;
using Orchard.FileSystems.AppData;

namespace Lombiq.Hosting.Azure.Lucene.Services
{
    /// <summary>
    /// Serves only to be fed to <see cref="Lombiq.Hosting.Azure.Indexing.Services.IndexingTaskExecutorDecorator"/> so it doesn't use the
    /// file system.
    /// </summary>
    public class DbAppDataFolder : IAppDataFolder
    {
        private readonly IRepository<AppDataFileRecord> _fileRecordRepository;


        public DbAppDataFolder(IRepository<AppDataFileRecord> fileRecordRepository)
        {
            _fileRecordRepository = fileRecordRepository;
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
            return Path.Combine(paths).Replace(Path.DirectorySeparatorChar, '/');
        }

        public bool FileExists(string path)
        {
            return _fileRecordRepository.Table.Any(fileRecord => fileRecord.Path == path);
        }

        public void CreateFile(string path, string content)
        {
            var record = GetRecord(path);
            if (record == null)
            {
                record = new AppDataFileRecord { Path = path };
                _fileRecordRepository.Create(record);
            }
            record.Content = content;
        }

        public System.IO.Stream CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public string ReadFile(string path)
        {
            var record = GetRecord(path);
            if (record == null) return null;
            return record.Content;
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
            var record = GetRecord(path);
            if (record != null) _fileRecordRepository.Delete(record);
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
            throw new NotImplementedException();
        }

        public string GetVirtualPath(string path)
        {
            throw new NotImplementedException();
        }


        private AppDataFileRecord GetRecord(string path)
        {
            return _fileRecordRepository.Table.Where(fileRecord => fileRecord.Path == path).SingleOrDefault();
        }
    }
}
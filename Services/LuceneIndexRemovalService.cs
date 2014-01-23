using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Environment.Extensions;

namespace Lombiq.Hosting.Azure.Lucene.Services
{
    public interface ILuceneIndexRemovalService : IDependency
    {
        void RemoveIndices(string shellName);
    }


    [OrchardFeature("Lombiq.Hosting.Azure.Indexing.Lucene")]
    public class LuceneIndexRemovalService : ILuceneIndexRemovalService
    {
        private readonly ILuceneAzureFileSystemFactory _fileSystemFactory;


        public LuceneIndexRemovalService(ILuceneAzureFileSystemFactory fileSystemFactory)
        {
            _fileSystemFactory = fileSystemFactory;
        }


        public void RemoveIndices(string shellName)
        {
            var fileSystem = _fileSystemFactory.Create(shellName);

            if (!fileSystem.FolderExists(string.Empty)) return;

            fileSystem.DeleteFolder(string.Empty);
        }
    }
}
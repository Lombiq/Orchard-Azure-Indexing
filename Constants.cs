using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.Hosting.Azure.Indexing
{
    public static class Constants
    {
        /// <summary>
        /// Configuration key for the connection string of the Azure storage account that will be used to store the Lucene indices.
        /// </summary>
        public const string LuceneStorageStorageConnectionStringSettingName = "Lombiq.Hosting.Azure.IndexingStorageConnectionString";
    }
}
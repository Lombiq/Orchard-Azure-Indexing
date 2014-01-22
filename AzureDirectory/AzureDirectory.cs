﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Lucene.Net.Store.Azure
{
    public class AzureDirectory : Directory
    {
        private Directory _cacheDirectory;
        private readonly string _root;

        private CloudBlobContainer _blobContainer;
        public CloudBlobContainer BlobContainer { get { return _blobContainer; } }


        public AzureDirectory(CloudBlobContainer blobContainer, string root, Directory cacheDirectory)
        {
            if (blobContainer == null)
                throw new ArgumentNullException("blobContainer");
            if (cacheDirectory == null)
                throw new ArgumentNullException("cacheDirectory");

            _blobContainer = blobContainer;
            _root = string.IsNullOrEmpty(root) ? string.Empty : root + "/";
            _cacheDirectory = cacheDirectory;
        }


        public bool CompressBlobs
        {
            get;
            set;
        }
      
        public void ClearCache()
        {
            foreach (string file in _cacheDirectory.ListAll())
            {
                _cacheDirectory.DeleteFile(file);
            }
        }

        public Directory CacheDirectory
        {
            get
            {
                return _cacheDirectory;
            }
            set
            {
                _cacheDirectory = value;
            }
        }

        /// <summary>Returns an array of strings, one for each file in the directory. </summary>
        public override String[] ListAll()
        {
            var results = from blob in _blobContainer.ListBlobs(_root)
                          select blob.Uri.AbsolutePath.Substring(blob.Uri.AbsolutePath.LastIndexOf('/') + 1);
            return results.ToArray<string>();
        }

        /// <summary>Returns true if a file with the given name exists. </summary>
        public override bool FileExists(String name)
        {
            // this always comes from the server
            try
            {
                var blob = _blobContainer.GetBlockBlobReference(_root + name);
                blob.FetchAttributes();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>Returns the time the named file was last modified. </summary>
        public override long FileModified(String name)
        {
            // this always has to come from the server
            try
            {
                var blob = _blobContainer.GetBlockBlobReference(_root + name);
                blob.FetchAttributes();
                return blob.Properties.LastModified.Value.UtcDateTime.ToFileTimeUtc();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Set the modified time of an existing file to now. </summary>
        public override void TouchFile(System.String name)
        {
            //BlobProperties props = _blobContainer.GetBlobProperties(_root + name);
            //_blobContainer.UpdateBlobMetadata(props);
            // I have no idea what the semantics of this should be...hmmmm...
            // we never seem to get called
            _cacheDirectory.TouchFile(name);
            //SetCachedBlobProperties(props);
        }

        /// <summary>Removes an existing file in the directory. </summary>
        public override void DeleteFile(System.String name)
        {
            var blob = _blobContainer.GetBlockBlobReference(_root + name);
            blob.DeleteIfExists();
            Debug.WriteLine(String.Format("DELETE {0}/{1}", _blobContainer.Uri.ToString(), name));

            if (_cacheDirectory.FileExists(name + ".blob"))
                _cacheDirectory.DeleteFile(name + ".blob");

            if (_cacheDirectory.FileExists(name))
                _cacheDirectory.DeleteFile(name);
        }

       
        /// <summary>Returns the length of a file in the directory. </summary>
        public override long FileLength(String name)
        {
            var blob = _blobContainer.GetBlockBlobReference(_root + name);
            blob.FetchAttributes();

            // index files may be compressed so the actual length is stored in metatdata
            string blobLegthMetadata;
            bool hasMetadataValue = blob.Metadata.TryGetValue("CachedLength", out blobLegthMetadata);

            long blobLength;
            if (hasMetadataValue && long.TryParse(blobLegthMetadata, out blobLength))
                return blobLength;
            else
                return blob.Properties.Length; // fall back to actual blob size
        }

        /// <summary>Creates a new, empty file in the directory with the given name.
        /// Returns a stream writing this file. 
        /// </summary>
        public override IndexOutput CreateOutput(System.String name)
        {
            var blob = _blobContainer.GetBlockBlobReference(_root + name);
            return new AzureIndexOutput(this, blob);
        }

        /// <summary>Returns a stream reading an existing file. </summary>
        public override IndexInput OpenInput(System.String name)
        {
            try
            {
                var blob = _blobContainer.GetBlockBlobReference(_root + name);
                blob.FetchAttributes();
                return new AzureIndexInput(this, blob);
            }
            catch (Exception err)
            {
                throw new FileNotFoundException(name, err);
            }
        }

        private Dictionary<string, AzureLock> _locks = new Dictionary<string, AzureLock>();

        /// <summary>Construct a {@link Lock}.</summary>
        /// <param name="name">the name of the lock file
        /// </param>
        public override Lock MakeLock(System.String name)
        {
            lock (_locks)
            {
                if (!_locks.ContainsKey(name))
                    _locks.Add(name, new AzureLock(_root + name, this));
                return _locks[name];
            }
        }

        public override void ClearLock(string name)
        {
            lock (_locks)
            {
                if (_locks.ContainsKey(name))
                {
                    _locks[name].BreakLock();
                }
            }
            _cacheDirectory.ClearLock(name);
        }

        /// <summary>Closes the store. </summary>
        protected override void Dispose(bool disposing)
        {
            _blobContainer = null;
        }

        public virtual bool ShouldCompressFile(string path)
        {
            if (!CompressBlobs)
                return false;

            var ext = System.IO.Path.GetExtension(path);
            switch (ext)
            {
                case ".cfs":
                case ".fdt":
                case ".fdx":
                case ".frq":
                case ".tis":
                case ".tii":
                case ".nrm":
                case ".tvx":
                case ".tvd":
                case ".tvf":
                case ".prx":
                    return true;
                default:
                    return false;
            };
        }
        public StreamInput OpenCachedInputAsStream(string name)
        {
            return new StreamInput(CacheDirectory.OpenInput(name));
        }

        public StreamOutput CreateCachedOutputAsStream(string name)
        {
            return new StreamOutput(CacheDirectory.CreateOutput(name));
        }

    }

}

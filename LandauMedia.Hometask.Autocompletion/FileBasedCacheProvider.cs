using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using LandauMedia.Hometask.Abstractions;
using Newtonsoft.Json;

namespace LandauMedia.Hometask.Autocompletion
{
    public class FileBasedCacheProvider : ICacheProvider
    {
        private readonly IFileSystem fs;
        private readonly IConfig config;
        private string CacheFolderName => string.Concat(config.PathToFilesFolder, "\\", "cache");
        public FileBasedCacheProvider(IFileSystem fs, IConfig config)
        {
            this.config = config;
            this.fs = fs;
        }
        public void CacheIndex(IEnumerable<string> filesToCache, IDictionary<string, IDictionary<string, int>> index)
        {
            if (!fs.Directory.Exists(CacheFolderName))
            {
                fs.Directory.CreateDirectory(CacheFolderName);
            }
            string cacheFileName = string.Concat(CacheFolderName, "\\", "cache.json");
            var indexToCache = new CachedIndex()
            {
                Meta = filesToCache.ToDictionary(file => file, file => fs.File.GetLastWriteTime(file)),
                Value = index
            };
            fs.File.WriteAllText(cacheFileName, JsonConvert.SerializeObject(indexToCache));            
        }

        public bool TryGetCachedIndex(IEnumerable<string> filesToCache, out IDictionary<string, IDictionary<string, int>> index)
        {
            string cacheFileName = string.Concat(CacheFolderName, "\\", "cache.json");
            if (!fs.File.Exists(cacheFileName))
            {
                index = null;
                return false;
            }
            else
            {
                string json = fs.File.ReadAllText(cacheFileName);
                var cache = JsonConvert.DeserializeObject<CachedIndex>(json);
                var currentFiles = filesToCache.ToDictionary(file => file, file => fs.File.GetLastWriteTime(file));
                bool cacheIsValid = cache.Meta.Count == currentFiles.Count && !currentFiles.Except(cache.Meta).Any();
                if (cacheIsValid)
                {
                    index = cache.Value;
                    return true;
                }
                index = null;
                return false;
            }            
        }

        internal class CachedIndex
        {
            public IDictionary<string, DateTime> Meta { get; set; }
            public IDictionary<string, IDictionary<string, int>> Value { get; set; }
        }
    }
}
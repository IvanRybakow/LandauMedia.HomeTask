using System.Collections.Generic;

namespace LandauMedia.Hometask.Abstractions
{
    public interface ICacheProvider
    {
        bool TryGetCachedIndex(IEnumerable<string> filesToCache, out IDictionary<string, IDictionary<string, int>> index);
        void CacheIndex(IEnumerable<string> filesToCache, IDictionary<string, IDictionary<string, int>> index);
    }
}
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Xml.Linq;
using LandauMedia.Hometask.Abstractions;

namespace LandauMedia.Hometask.Autocompletion
{
    public class XmlBasedAutocompleteService : InMemoryAutocompleteServiceBase
    {
        private readonly IConfig config;
        private readonly IFileSystem fileSystem;
        private readonly ICacheProvider cacheProvider;

        public XmlBasedAutocompleteService(IConfig config, IFileSystem fileSystem, ICacheProvider cacheProvider)
        {
            this.config = config;
            this.fileSystem = fileSystem;
            this.cacheProvider = cacheProvider;
            Initialize();
        }
        private void Initialize()
        {
            var files = fileSystem.Directory.EnumerateFiles(config.PathToFilesFolder, "*.xml");
            if(cacheProvider.TryGetCachedIndex(files, out IDictionary<string, IDictionary<string, int>> cachedIndex))
            {
                MergeIndex(cachedIndex);
            }
            else
            {
                foreach (var xmlFile in files)
                {
                    System.IO.Stream fileStream = fileSystem.File.OpenRead(xmlFile);
                    XDocument document = XDocument.Load(fileStream);
                    var elements = document.Descendants("text");
                    foreach (var element in elements)  
                    {
                        AddTokensToIndex(GetTokensFromText(element.Value));            
                    }      
                }
                cacheProvider.CacheIndex(files, index);
            }
             
        }
    }
}
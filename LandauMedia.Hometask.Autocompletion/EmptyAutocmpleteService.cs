using LandauMedia.Hometask.Abstractions;
using Microsoft.Extensions.Logging;

namespace LandauMedia.Hometask.Autocompletion
{
    public class EmptyAutocmpleteService : InMemoryAutocompleteServiceBase, IAddableAutocompleteService
    {
        private readonly ILogger logger;
        
        public EmptyAutocmpleteService(ILogger logger)
        {
            this.logger = logger;
        }
        public void AddTextToSource(string text)
        {
            var tokens = GetTokensFromText(text);
            AddTokensToIndex(tokens);
            logger.LogInformation($"Added text {text} to index");
        }
    }
}
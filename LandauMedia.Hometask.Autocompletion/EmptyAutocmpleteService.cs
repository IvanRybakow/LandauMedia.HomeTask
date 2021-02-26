using LandauMedia.Hometask.Abstractions;
using Microsoft.Extensions.Logging;

namespace LandauMedia.Hometask.Autocompletion
{
    public class EmptyAutocmpleteService : InMemoryAutocompleteServiceBase, IAddableAutocompleteService
    {
        public void AddTextToSource(string text)
        {
            var tokens = GetTokensFromText(text);
            AddTokensToIndex(tokens);
        }
    }
}
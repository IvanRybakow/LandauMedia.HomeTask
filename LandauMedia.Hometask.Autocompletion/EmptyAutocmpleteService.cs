using LandauMedia.Hometask.Abstractions;
using Microsoft.Extensions.Logging;

namespace LandauMedia.Hometask.Autocompletion
{
    public class EmptyAutocmpleteService : InMemoryAutocompleteServiceBase, IAddableAutocompleteService
    {
        private readonly ITokenService tokenService;
        private readonly ILogger logger;

        public EmptyAutocmpleteService(ITokenService tokenService, ILogger logger)
        {
            this.logger = logger;
            this.tokenService = tokenService;
        }
        public void AddTextToSource(string text)
        {
            var tokens = tokenService.GetTokensFromText(text);
            AddTokensToIndex(tokens);
        }
    }
}
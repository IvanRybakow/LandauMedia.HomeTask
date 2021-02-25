using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandauMedia.Hometask.Abstractions;

namespace LandauMedia.Hometask.Autocompletion
{
    public abstract class InMemoryAutocompleteServiceBase : IAutoCompleteService
    {
        private readonly IDictionary<string, IDictionary<string, int>> index = new Dictionary<string, IDictionary<string, int>>();
        public IEnumerable<(string word, int frequency)> GetNextWords(string word)
        {
            if (word != null && index.TryGetValue(word, out var value))            
                return value.OrderByDescending(kvp => kvp.Value).Select(kvp => (kvp.Key, kvp.Value));            
            return Enumerable.Empty<(string word, int frequency)>();
        }

        public Task<IEnumerable<(string word, int frequency)>> GetNextWordsAsync(string word)
        {
            if (word != null && index.TryGetValue(word, out var value))            
                return  Task.FromResult(value.OrderByDescending(kvp => kvp.Value).Select(kvp => (kvp.Key, kvp.Value)));            
            return Task.FromResult<IEnumerable<(string word, int frequency)>>(Enumerable.Empty<(string word, int frequency)>());
        }

        protected void AddTokensToIndex(IEnumerable<string> tokenList)
        {
            string currentToken = null;
            foreach (var token in tokenList)
            {
                if (string.IsNullOrEmpty(currentToken))                    
                    currentToken = token;
                else
                {
                    if (index.TryGetValue(currentToken, out var value))
                    {
                        if (!value.TryAdd(token, 1))
                            value[token] += 1;
                    }                                                                                      
                    else                        
                        index.Add(currentToken, new Dictionary<string, int>(){ { token, 1 } });
                    currentToken = token;
                }
            }
        }
    }
}
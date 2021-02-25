using System.Collections.Generic;
using System.Threading.Tasks;

namespace LandauMedia.Hometask.Abstractions
{
    public interface IAutoCompleteService
    {
        Task<IEnumerable<(string word, int frequency)>> GetNextWordsAsync(string word);
        IEnumerable<(string word, int frequency)> GetNextWords(string word); 
    }
}
namespace LandauMedia.Hometask.Abstractions
{
    public interface IAutoCompleteService
    {
        Task<IEnumerable<(string word, int frequency)>> GetNextWordsAsync(string word);
        IEnumerable<(string word, int frequency)> GetNextWords(string word); 
        Task<IEnumerable<(string word, int frequency)>> GetNextWordsAsync(string word, int limit);
        IEnumerable<(string word, int frequency)> GetNextWords(string word, int limit); 
    }
}
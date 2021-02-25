using System.Collections.Generic;

namespace LandauMedia.Hometask.Abstractions
{
    public interface ITokenService
    {
         IEnumerable<string> GetTokensFromText(string text);
    }
}
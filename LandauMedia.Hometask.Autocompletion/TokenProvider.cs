using System;
using System.Collections.Generic;

namespace LandauMedia.Hometask.Autocompletion
{
    public class TokenProvider
    {
        private readonly char[] splitChars = new char[] { ' ', '-', ',', ';', '.', '\t', '\n', '[', ']', '(', ')', '+',
         '-', '\"', '\'', '!', '?', '\\', '/', '&', '$', '=', '#', '<', '>', ':', '_', '@', '{', '}', '%', '*', '~', '^', '|' };

        public IEnumerable<string> GetTokensFromText(string text)
        {
            if (string.IsNullOrEmpty(text)) return new List<string>();
            return text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
    }
}
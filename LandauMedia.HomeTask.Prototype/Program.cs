using System;
using System.IO.Abstractions;
using System.Linq;
using LandauMedia.Hometask.Abstractions;
using LandauMedia.Hometask.Autocompletion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LandauMedia.HomeTask.Prototype
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider provider = BuildServiceProvider();
            IAutoCompleteService autocompletionService = provider.GetService<IAutoCompleteService>();
            Console.WriteLine("Please enter the example text:");
            string text = Console.ReadLine();
            if (autocompletionService is IAddableAutocompleteService service)
                service.AddTextToSource(text);                
            Console.WriteLine("Please enter the word to search in text, type \"exit\" to exit the program");
            string word = Console.ReadLine();
            while (word != "exit")
            {
                var suggestions = autocompletionService.GetNextWords(word);
                if (suggestions != null)
                {
                    foreach (var suggestion in suggestions.Take(10))
                        Console.WriteLine($"{suggestion.frequency} x {suggestion.word}");
                }
                else
                    Console.WriteLine("No matches");
                
                word = Console.ReadLine();
            }           
        }

        static IServiceProvider BuildServiceProvider()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appSettings.json", false).Build();
            IConfig config = configuration.Get<Config>();

            IServiceCollection collection = new ServiceCollection();
            collection.AddSingleton<IConfig>(config);
            collection.AddSingleton<IAutoCompleteService, XmlBasedAutocompleteService>();
            collection.AddScoped<IFileSystem, FileSystem>();

            return collection.BuildServiceProvider();
        }
    }
}
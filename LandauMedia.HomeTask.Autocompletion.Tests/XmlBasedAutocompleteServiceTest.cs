using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using LandauMedia.Hometask.Abstractions;
using LandauMedia.Hometask.Autocompletion;
using Moq;
using Xunit;

namespace LandauMedia.HomeTask.Autocompletion.Tests
{
    public class XmlBasedAutocompleteServiceTest
    {
        private XmlBasedAutocompleteService xmlBasedAutocompletionService;
        private readonly Mock<IFileSystem> fileSystem;
        private readonly Mock<IConfig> config;
        private readonly Mock<ICacheProvider> cacheProvider;

        public XmlBasedAutocompleteServiceTest()
        {
            fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(f => f.Directory.EnumerateFiles(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<string>(){"file"});

            cacheProvider = new Mock<ICacheProvider>();
            IDictionary<string, IDictionary<string, int>> param;
            cacheProvider.Setup(c => c.TryGetCachedIndex(It.IsAny<IEnumerable<string>>(), out param)).Returns(false);

            config = new Mock<IConfig>();
            config.Setup(c => c.PathToFilesFolder).Returns("test");
        }

        [Fact]
        public void GetNextWords_ReturnsCorrectReslult()
        {
            //Given
            string xmlText = "<text>Dies ist ein Beispieltext, der ein Problem demonstrieren soll.</text>";
            Stream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xmlText));
            fileSystem.Setup(f => f.File.OpenRead(It.IsAny<string>())).Returns(memoryStream);
            xmlBasedAutocompletionService = new XmlBasedAutocompleteService(config.Object, fileSystem.Object, cacheProvider.Object);
            //When
            var firstRequest = xmlBasedAutocompletionService.GetNextWords("ein").ToList();
            var secondRequest = xmlBasedAutocompletionService.GetNextWords("der").ToList();
            //Then
            Assert.True(firstRequest.Count() == 2);
            Assert.Single(secondRequest);
            Assert.Contains(("beispieltext", 1), firstRequest);
            Assert.Contains(("problem", 1), firstRequest);
            Assert.Contains(("ein", 1), secondRequest);
        }

        [Fact]
        public async void GetNextWordsAsync_WithLimit_ReturnsCorrectReslult()
        {
            //Given
            string xmlText = "<text>Dies ist ein Beispieltext, der ein Problem demonstrieren soll. Dies ist ein Beispieltext, der noch was demonstriert.Der der, der noch, der ein</text>";
            Stream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xmlText));
            fileSystem.Setup(f => f.File.OpenRead(It.IsAny<string>())).Returns(memoryStream);
            xmlBasedAutocompletionService = new XmlBasedAutocompleteService(config.Object, fileSystem.Object, cacheProvider.Object);
            //When
            var firstRequest = (await xmlBasedAutocompletionService.GetNextWordsAsync("ein", 1)).ToList();
            var secondRequest = (await xmlBasedAutocompletionService.GetNextWordsAsync("der", 2)).ToList();
            //Then
            Assert.Single(firstRequest);
            Assert.True(secondRequest.Count() == 2);
            Assert.Contains(("beispieltext", 2), firstRequest);
            Assert.Contains(("ein", 2), secondRequest);
            Assert.Contains(("noch", 2), secondRequest);
        }
    }
}
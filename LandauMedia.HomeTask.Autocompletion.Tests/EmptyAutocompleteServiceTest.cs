using Xunit;
using LandauMedia.Hometask.Autocompletion;
using System.Linq;

namespace LandauMedia.HomeTask.Autocompletion.Tests
{
    public class EmptyAutocompleteServiceTest
    {
        private readonly EmptyAutocmpleteService service;
        public EmptyAutocompleteServiceTest()
        {
            service = new EmptyAutocmpleteService();
        }
        [Fact]
        public void AddTextToSource_WithOneInput_GetNextWords_ReturnsValidResult()
        {
            //Given
            var text = "Dies ist ein Beispieltext, der ein Problem demonstrieren soll.";
            service.AddTextToSource(text);
            //When
            var firstRequest = service.GetNextWords("ein").ToList();
            var secondRequest = service.GetNextWords("der").ToList();
            //Then
            Assert.True(firstRequest.Count() == 2);
            Assert.Single(secondRequest);
            Assert.Contains(("Beispieltext", 1), firstRequest);
            Assert.Contains(("Problem", 1), firstRequest);
            Assert.Contains(("ein", 1), secondRequest);
        }
        [Fact]
        public void AddTextToSource_DoubleCall_GetNextWords_ReturnsValidResult()
        {
            //Given
            service.AddTextToSource("Dies ist ein Beispieltext, der ein Problem demonstrieren soll.");
            service.AddTextToSource("Dies ist ein Beispieltext, der noch was demonstriert");
            //When
            var firstRequest = service.GetNextWords("ein").ToList();
            var secondRequest = service.GetNextWords("der").ToList();
            //Then
            Assert.True(firstRequest.Count() == 2);
            Assert.True(secondRequest.Count() == 2);
            Assert.Contains(("Beispieltext", 2), firstRequest);
            Assert.Contains(("Problem", 1), firstRequest);
            Assert.Contains(("ein", 1), secondRequest);
            Assert.Contains(("noch", 1), secondRequest);
        }
        [Fact]
        public void GetNextWords_WithoutAddedText_ReturnsEmptyCollection()
        {
            //Given

            //When
            var request = service.GetNextWords("ein").ToList();
            //Then
            Assert.Empty(request);
        }
    }
    
}
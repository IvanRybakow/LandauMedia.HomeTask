using Xunit;
using LandauMedia.Hometask.Autocompletion;
using System.Linq;

namespace LandauMedia.HomeTask.Autocompletion.Tests
{
    public class EmptyAutocompleteServiceTest
    {
        private EmptyAutocmpleteService service;

        [Fact]
        public void AddTextToSource_WithOneInput_GetNextWords_ReturnsValidResult()
        {
            //Given
            service = new EmptyAutocmpleteService();
            var text = "Dies ist ein Beispieltext, der ein Problem demonstrieren soll.";
            service.AddTextToSource(text);
            //When
            var firstRequest = service.GetNextWords("ein").ToList();
            var secondRequest = service.GetNextWords("der").ToList();
            //Then
            Assert.True(firstRequest.Count() == 2);
            Assert.Single(secondRequest);
            Assert.Contains(("beispieltext", 1), firstRequest);
            Assert.Contains(("problem", 1), firstRequest);
            Assert.Contains(("ein", 1), secondRequest);
        }
        [Fact]
        public void AddTextToSource_DoubleCall_GetNextWords_ReturnsValidResult()
        {
            //Given
            service = new EmptyAutocmpleteService();
            service.AddTextToSource("Dies ist ein Beispieltext, der ein Problem demonstrieren soll.");
            service.AddTextToSource("Dies ist ein Beispieltext, der noch was demonstriert");
            //When
            var firstRequest = service.GetNextWords("ein").ToList();
            var secondRequest = service.GetNextWords("der").ToList();
            //Then
            Assert.True(firstRequest.Count() == 2);
            Assert.True(secondRequest.Count() == 2);
            Assert.Contains(("beispieltext", 2), firstRequest);
            Assert.Contains(("problem", 1), firstRequest);
            Assert.Contains(("ein", 1), secondRequest);
            Assert.Contains(("noch", 1), secondRequest);
        }
        [Fact]
        public void GetNextWords_WithoutAddedText_ReturnsEmptyCollection()
        {
            //Given
            service = new EmptyAutocmpleteService();
            //When
            var request = service.GetNextWords("ein").ToList();
            //Then
            Assert.Empty(request);
        }
    }
    
}
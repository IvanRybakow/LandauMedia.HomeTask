using Xunit;
using LandauMedia.Hometask.Abstractions;
using LandauMedia.Hometask.Autocompletion;
using Moq;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace LandauMedia.HomeTask.Autocompletion.Tests
{
    public class EmptyAutocompleteServiceTest
    {
        private readonly EmptyAutocmpleteService service;
        private readonly Mock<ILogger> loggerMock;
        public EmptyAutocompleteServiceTest()
        {
            loggerMock = new Mock<ILogger>();
            service = new EmptyAutocmpleteService(loggerMock.Object);
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
            loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
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
            loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Exactly(2));
        }
        [Fact]
        public void GetNextWords_WithoutAddedText_ReturnsEmptyCollection()
        {
            //Given

            //When
            var request = service.GetNextWords("ein").ToList();
            //Then
            Assert.Empty(request);
            loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
    
}
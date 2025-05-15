using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using STIN_News_Module.Logic.AIStuff;
using System.Collections.Generic;

namespace STIN_News_Module_Test
{
    public class AITests
    {
        [Fact]
        public async Task GetClasification_PositiveHigher_Returns1()
        {
            // Arrange
            var sentimentResponse = new List<List<Sentiment>>
        {
            new List<Sentiment>
            {
                new Sentiment { label = "positive", score = 0.8 },
                new Sentiment { label = "negative", score = 0.2 }
            }
        };

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(sentimentResponse);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(jsonResponse),
               });

            var client = new HttpClient(handlerMock.Object);
            var ai = new AI(client, "fake-key", "http://fake-url");

            // Act
            var result = await ai.GetClasification("some text");

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetClasification_NegativeHigher_Returns0()
        {
            var sentimentResponse = new List<List<Sentiment>>
        {
            new List<Sentiment>
            {
                new Sentiment { label = "positive", score = 0.3 },
                new Sentiment { label = "negative", score = 0.6 }
            }
        };

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(sentimentResponse);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(jsonResponse),
               });

            var client = new HttpClient(handlerMock.Object);
            var ai = new AI(client, "fake-key", "http://fake-url");

            var result = await ai.GetClasification("some negative text");

            Assert.Equal(-1, result);
        }
    }
}


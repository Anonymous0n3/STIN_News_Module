using Xunit;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using STIN_News_Module.Logic;
using STIN_News_Module.Logic.AIStuff;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.News;
using NewsAPI.Models;
using System.Linq;
using STIN_News_Module;

namespace STIN_News_Module_Test
{
    public class UtilsTests
    {
        [Fact]
        public async Task DoAllLogic_ReturnsExpectedResult()
        {
            // Arrange
            var mockNews = new Mock<INewsGetting>();
            var mockAI = new Mock<IAI>();
            var mockFilter = new Mock<IFilterManager>();
            var mockJson = new Mock<IJSONLogic>();

            var articles = new List<Article>
        {
            new Article { Description = "Good news" },
            new Article { Description = "Bad news" }
        };

            var dataModels = new List<DataModel>
        {
            new DataModel { Name = "Apple", Rating = 0, Sale = 0 }
        };

            mockNews.Setup(n => n.returnNews("Apple", 3)).Returns(articles);
            mockAI.Setup(a => a.GetClasification("Good news")).ReturnsAsync(1);
            mockAI.Setup(a => a.GetClasification("Bad news")).ReturnsAsync(0);

            mockFilter.Setup(f => f.ExecuteAllFilters(It.IsAny<List<DataModel>>()))
                      .Returns<List<DataModel>>(d => d);

            mockJson.Setup(j => j.deserializeJSON(It.IsAny<string>()))
                    .Returns<List<DataModel>>(null);

            var httpClient = new HttpClient(new FakeHttpMessageHandler());

            var utils = new Utils(mockNews.Object, mockAI.Object, mockFilter.Object, mockJson.Object, httpClient);

            // Act
            var result = await utils.doAllLogic(dataModels, 3);

            // Assert
            Assert.Single(result);
            Assert.Equal(0, result[0].Rating); // 1 (Good) + 0 (Bad)
        }
    }

    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(new List<DataModel>
        {
            new DataModel { Name = "Apple", Rating = 1, Sale = 0 }
        });

            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        }
    }
}

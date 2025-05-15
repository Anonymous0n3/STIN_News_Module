using Xunit;
using Moq;
using NewsAPI.Models;
using NewsAPI.Constants;
using System;
using System.Collections.Generic;
using STIN_News_Module.Logic.News;
using STIN_News_Module;

namespace STIN_News_Module_Test
{
    public class NewsGettingTests
    {
        [Fact]
        public void ReturnNews_WithValidQuery_ReturnsArticles()
        {
            // Arrange
            var mockClient = new Mock<INewsApiClient>();

            var expectedArticles = new List<Article>
        {
            new Article { Title = "Test Article 1" },
            new Article { Title = "Test Article 2" }
        };

            mockClient.Setup(c => c.GetEverything(It.IsAny<EverythingRequest>()))
                .Returns(new ArticlesResult
                {
                    Status = Statuses.Ok,
                    Articles = expectedArticles,
                    TotalResults = expectedArticles.Count
                });

            var newsGetting = new News_Getting(mockClient.Object);

            // Act
            var result = newsGetting.returnNews("AI", 3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Article 1", result[0].Title);
        }

        [Fact]
        public void ReturnNews_WithApiFailure_ReturnsNull()
        {
            var mockClient = new Mock<INewsApiClient>();
            mockClient.Setup(c => c.GetEverything(It.IsAny<EverythingRequest>()))
                .Returns(new ArticlesResult { Status = Statuses.Error });

            var newsGetting = new News_Getting(mockClient.Object);

            var result = newsGetting.returnNews("invalid", 3);

            Assert.Null(result);
        }
    }
}

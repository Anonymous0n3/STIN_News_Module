using System;
using Xunit;
using STIN_News_Module.Pages;
using System.Linq;

namespace STIN_News_Module.Tests
{
    public class UserDashboardModelTests
    {
        [Fact]
        public void OnGet_ShouldPopulateStocksWithExpectedData()
        {
            // Arrange
            var model = new UserDashboardModel();

            // Act
            model.OnGet();

            // Assert
            Assert.NotNull(model.Stocks);
            Assert.Equal(3, model.Stocks.Count);

            var microsoft = model.Stocks.FirstOrDefault(s => s.Name == "Microsoft");
            Assert.NotNull(microsoft);
            Assert.Equal(8, microsoft.Rating);
            Assert.Equal(0, microsoft.Sale);

            var google = model.Stocks.FirstOrDefault(s => s.Name == "Google");
            Assert.NotNull(google);
            Assert.Equal(-3, google.Rating);
            Assert.Equal(1, google.Sale);

            var openAI = model.Stocks.FirstOrDefault(s => s.Name == "OpenAI");
            Assert.NotNull(openAI);
            Assert.Equal(5, openAI.Rating);
            Assert.Equal(0, openAI.Sale);
        }
    }
}

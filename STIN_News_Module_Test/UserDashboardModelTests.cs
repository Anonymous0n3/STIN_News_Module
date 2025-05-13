using System;
using System.Linq;
using STIN_News_Module.Pages;
using Xunit;

namespace STIN_News_Module_Test
{
    public class UserDashboardModelTests
    {
        [Fact]
        public void OnGet_InitializesStocksList()
        {
            // Arrange
            var model = new UserDashboardModel();

            // Act
            model.OnGet();

            // Assert
            Assert.NotNull(model.Stocks);
            Assert.Equal(3, model.Stocks.Count);
        }

        [Fact]
        public void OnGet_StockItemsContainExpectedData()
        {
            // Arrange
            var model = new UserDashboardModel();

            // Act
            model.OnGet();
            var stocks = model.Stocks;

            // Assert
            Assert.Contains(stocks, s => s.Name == "Microsoft" && s.Rating == 8 && s.Sell == 0);
            Assert.Contains(stocks, s => s.Name == "Google" && s.Rating == -3 && s.Sell == 1);
            Assert.Contains(stocks, s => s.Name == "OpenAI" && s.Rating == 5 && s.Sell == 0);
        }

        [Fact]
        public void OnGet_DatesAreSetRelativeToNow()
        {
            // Arrange
            var model = new UserDashboardModel();
            var now = DateTime.Now;

            // Act
            model.OnGet();

            // Assert
            Assert.Contains(model.Stocks, s => (now - s.Date).TotalDays <= 2);
        }
    }
}

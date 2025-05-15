using Xunit;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Filtering.Filters;
using System.Collections.Generic;

namespace STIN_News_Module_Test
{
    public class NegativeRatingFilterTests
    {
        [Fact]
        public void Execute_RemovesNonPositiveRatings()
        {
            // Arrange
            var data = new List<DataModel>
            {
                new DataModel { Name = "Positive", Rating = 3 },
                new DataModel { Name = "Zero", Rating = -1 },
                new DataModel { Name = "Negative", Rating = -2 }
            };

            var filter = new NegativeRatingFilter();

            // Act
            var result = filter.Execute(data);

            // Assert
            Assert.Single(result);
            Assert.Equal("Positive", result[0].Name);
        }
    }
}
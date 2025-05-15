using Xunit;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
using System.Collections.Generic;
using STIN_News_Module.Logic.Filtering.Filters;
using System.Reflection;

namespace STIN_News_Module_Test
{
    public class FilterManagerTests
    {
        [Fact]
        public void ExecuteAllFilters_AppliesAllFiltersCorrectly()
        {
            // Arrange
            var testData = new List<DataModel>
        {
            new DataModel { Name = "X", Rating = 0 },
            new DataModel { Name = "Y", Rating = 1 },
            new DataModel { Name = "Z", Rating = 1, Sale = 1 }
        };

            var manager = FilterManager.Instance;

            // Act
            var result = manager.ExecuteAllFilters(testData);

            // Assert
            Assert.Equal(0, result.Count); // Očekáváme 0 položek, protože nic nemá articles
        }

        [Fact]
        public void AutoRegisterFilters_ShouldIncludeDummyFilter()
        {
            // Pouze ověří, že DummyFilter je mezi zaregistrovanými filtry
            var filtersField = typeof(FilterManager)
                .GetField("filters", BindingFlags.NonPublic | BindingFlags.Instance);

            var instance = FilterManager.Instance;
            var filters = filtersField.GetValue(instance) as List<FilterBase>;

            Assert.Contains(filters, f => f.GetType().Name == "NumOfArticles");
        }

    }
}

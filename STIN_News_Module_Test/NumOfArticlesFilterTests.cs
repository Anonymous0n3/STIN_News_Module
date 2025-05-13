using Xunit;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Filtering.Filters;
using System.Collections.Generic;

namespace STIN_News_Module_Test
{
    public class NumOfArticlesFilterTests
    {
        [Fact]
        public void Execute_OnlyIncludesItemsWithAtLeast10Articles()
        {
            // Arrange
            var item1 = new DataModel { Name = "FewArticles" };
            item1.setarticleNum(5);

            var item2 = new DataModel { Name = "EnoughArticles" };
            item2.setarticleNum(15);

            var item3 = new DataModel { Name = "ExactlyTen" };
            item3.setarticleNum(10);

            var data = new List<DataModel> { item1, item2, item3 };
            var filter = new NumOfArticles();

            // Act
            var result = filter.Execute(data);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Name == "EnoughArticles");
            Assert.Contains(result, x => x.Name == "ExactlyTen");
            Assert.DoesNotContain(result, x => x.Name == "FewArticles");
        }
    }
}
using Moq;
using NewsAPI.Models;
using STIN_News_Module.Logic;
using STIN_News_Module.Logic.AIStuff;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.News;

namespace STIN_News_Module_Test
{
    public class UtilsTests
    {
        [Fact]
        public void LimitToRange_ValueBelowMin_ReturnsMin()
        {
            var utils = new Utils();
            int result = utils.LimitToRange(-15, -10, 10);
            Assert.Equal(-10, result);
        }

        [Fact]
        public void LimitToRange_ValueAboveMax_ReturnsMax()
        {
            var utils = new Utils();
            int result = utils.LimitToRange(20, -10, 10);
            Assert.Equal(10, result);
        }

        [Fact]
        public void LimitToRange_ValueWithinRange_ReturnsSame()
        {
            var utils = new Utils();
            int result = utils.LimitToRange(5, -10, 10);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Sell_RemovesItemsWithRatingGreaterOrEqualToMin()
        {
            var utils = new Utils();
            var data = new List<DataModel>
    {
        new DataModel { Name = "A", Rating = 5 },
        new DataModel { Name = "B", Rating = 8 },
        new DataModel { Name = "C", Rating = 10 }
    };

            var result = utils.sell(data);

            Assert.Equal(1, result.Count);
            Assert.DoesNotContain(result, item => item.Rating >= 5);
        }

        [Fact]
        public void Sell_KeepsItemsWithRatingBelowMin()
        {
            var utils = new Utils();
            var data = new List<DataModel>
    {
        new DataModel { Name = "X", Rating = 3 },
        new DataModel { Name = "Y", Rating = 6 }
    };

            var result = utils.sell(data);

            Assert.Equal(2, result.Count);
        }

    }
}

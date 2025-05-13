using System.Text.Json;
using STIN_News_Module.Logic.JsonModel;
using Xunit;

namespace STIN_News_Module_Test
{
    public class DataModelTests
    {
        [Fact]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var model = new DataModel
            {
                Name = "Example",
                Date = 2025,
                Rating = 8,
                Sale = 1
            };

            // Act & Assert
            Assert.Equal("Example", model.Name);
            Assert.Equal(2025, model.Date);
            Assert.Equal(8, model.Rating);
            Assert.Equal(1, model.Sale);
        }

        [Fact]
        public void ArticleNum_GetSet_WorksCorrectly()
        {
            // Arrange
            var model = new DataModel();

            // Act
            model.setarticleNum(42);

            // Assert
            Assert.Equal(42, model.getarticleNum());
        }

        [Fact]
        public void JsonSerialization_UsesCorrectPropertyNames()
        {
            // Arrange
            var model = new DataModel
            {
                Name = "SerializedItem",
                Date = 2023,
                Rating = 5,
                Sale = 1
            };

            // Act
            string json = JsonSerializer.Serialize(model);

            // Assert – kontrola správných názvů v JSON
            Assert.Contains("\"name\":\"SerializedItem\"", json);
            Assert.Contains("\"date\":2023", json);
            Assert.Contains("\"rating\":5", json);
            Assert.Contains("\"sale\":1", json);
        }

        [Fact]
        public void JsonDeserialization_MapsToCorrectProperties()
        {
            // Arrange
            string json = "{\"name\":\"DeserializedItem\",\"date\":2022,\"rating\":-2,\"sale\":0}";

            // Act
            var model = JsonSerializer.Deserialize<DataModel>(json);

            // Assert
            Assert.Equal("DeserializedItem", model.Name);
            Assert.Equal(2022, model.Date);
            Assert.Equal(-2, model.Rating);
            Assert.Equal(0, model.Sale);
        }
    }
}

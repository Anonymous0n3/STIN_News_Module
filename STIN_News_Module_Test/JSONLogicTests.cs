using System;
using System.Collections.Generic;
using STIN_News_Module.Logic.JsonModel;
using Xunit;

namespace STIN_News_Module_Test
{
    public class JSONLogicTests
    {
        [Fact]
        public void SerializeJSON_ReturnsValidJsonString()
        {
            // Arrange
            var data = new List<DataModel>
        {
            new DataModel { Name = "Item1", Date = 2024, Rating = 5, Sale = 1 },
            new DataModel { Name = "Item2", Date = 2025, Rating = -3, Sale = 0 }
        };

            // Act
            string json = JSONLogic.Instance.serializeJSON(data);

            // Assert
            Assert.Contains("\"name\":\"Item1\"", json);
            Assert.Contains("\"rating\":-3", json);
        }

        [Fact]
        public void DeserializeJSON_ReturnsValidObjectList()
        {
            // Arrange
            string json = "[{\"name\":\"Test\",\"date\":2023,\"rating\":7,\"sale\":1}]";

            // Act
            var result = JSONLogic.Instance.deserializeJSON(json);

            // Assert
            Assert.Single(result);
            Assert.Equal("Test", result[0].Name);
            Assert.Equal(2023, result[0].Date);
            Assert.Equal(7, result[0].Rating);
            Assert.Equal(1, result[0].Sale);
        }

        [Fact]
        public void SerializeAndDeserialize_RoundTripPreservesData()
        {
            // Arrange
            var original = new List<DataModel>
        {
            new DataModel { Name = "RoundTrip", Date = 2022, Rating = 10, Sale = 0 }
        };

            // Act
            var json = JSONLogic.Instance.serializeJSON(original);
            var deserialized = JSONLogic.Instance.deserializeJSON(json);

            // Assert
            Assert.Single(deserialized);
            Assert.Equal(original[0].Name, deserialized[0].Name);
            Assert.Equal(original[0].Date, deserialized[0].Date);
            Assert.Equal(original[0].Rating, deserialized[0].Rating);
            Assert.Equal(original[0].Sale, deserialized[0].Sale);
        }

        [Fact]
        public void Deserialize_InvalidJson_ThrowsException()
        {
            // Arrange
            string invalidJson = "{ invalid json }";

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => JSONLogic.Instance.deserializeJSON(invalidJson));
        }
    }
}

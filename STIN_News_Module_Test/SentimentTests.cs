using System.Text.Json;
using STIN_News_Module.Logic.AIStuff;
using Xunit;

namespace STIN_News_Module_Test
{
    public class SentimentTests
    {
        [Fact]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var sentiment = new Sentiment
            {
                label = "positive",
                score = 0.85
            };

            // Act & Assert
            Assert.Equal("positive", sentiment.label);
            Assert.Equal(0.85, sentiment.score, 3); // tolerance na desetinná čísla
        }

        [Fact]
        public void CanBeSerializedToJson()
        {
            // Arrange
            var sentiment = new Sentiment { label = "neutral", score = 0.5 };

            // Act
            string json = JsonSerializer.Serialize(sentiment);

            // Assert
            Assert.Contains("\"label\":\"neutral\"", json);
            Assert.Contains("\"score\":0.5", json);
        }

        [Fact]
        public void CanBeDeserializedFromJson()
        {
            // Arrange
            string json = "{\"label\":\"negative\",\"score\":0.1}";

            // Act
            var sentiment = JsonSerializer.Deserialize<Sentiment>(json);

            // Assert
            Assert.Equal("negative", sentiment.label);
            Assert.Equal(0.1, sentiment.score, 3);
        }
    }
}

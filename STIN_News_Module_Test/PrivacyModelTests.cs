using Microsoft.Extensions.Logging;
using Moq;
using STIN_News_Module.Pages;
using Xunit;

namespace STIN_News_Module_Test
{
    public class PrivacyModelTests
    {
        [Fact]
        public void Constructor_SetsLogger()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PrivacyModel>>();

            // Act
            var model = new PrivacyModel(loggerMock.Object);

            // Assert
            Assert.NotNull(model);
        }

        [Fact]
        public void OnGet_DoesNotThrowException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PrivacyModel>>();
            var model = new PrivacyModel(loggerMock.Object);

            // Act & Assert
            var exception = Record.Exception(() => model.OnGet());
            Assert.Null(exception);
        }
    }
}

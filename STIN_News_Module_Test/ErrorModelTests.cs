using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using STIN_News_Module.Pages;
using Xunit;

namespace STIN_News_Module_Test
{
    public class ErrorModelTests
    {
        [Fact]
        public void ShowRequestId_ReturnsFalse_WhenRequestIdIsNullOrEmpty()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorModel>>();
            var model = new ErrorModel(loggerMock.Object)
            {
                RequestId = null
            };

            // Act
            var result = model.ShowRequestId;

            // Assert
            Assert.False(result);

            model.RequestId = "";
            Assert.False(model.ShowRequestId);
        }

        [Fact]
        public void ShowRequestId_ReturnsTrue_WhenRequestIdIsNotEmpty()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorModel>>();
            var model = new ErrorModel(loggerMock.Object)
            {
                RequestId = "12345"
            };

            // Act
            var result = model.ShowRequestId;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OnGet_SetsRequestId_FromActivityOrTraceIdentifier()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorModel>>();
            var model = new ErrorModel(loggerMock.Object);

            // Simulace HttpContext a TraceIdentifier
            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "test-trace-id";
            model.PageContext = new PageContext
            {
                HttpContext = httpContext
            };

            // Act
            model.OnGet();

            // Assert
            Assert.Equal("test-trace-id", model.RequestId);
        }

        [Fact]
        public void OnGet_UsesActivityId_WhenAvailable()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorModel>>();
            var model = new ErrorModel(loggerMock.Object);

            // Simulace Activity
            var activity = new Activity("TestActivity");
            activity.Start();
            var expectedId = activity.Id;

            var httpContext = new DefaultHttpContext();
            model.PageContext = new PageContext
            {
                HttpContext = httpContext
            };

            // Act
            model.OnGet();

            // Assert
            Assert.Equal(expectedId, model.RequestId);

            activity.Stop();
        }
    }
}

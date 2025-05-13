using System;
using System.IO;
using Xunit;
using STIN_News_Module.Logic;

namespace STIN_News_Module_Test
{
    public class EnvLoadTests
    {
        private readonly string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

        [Fact]
        public void Load_SetsEnvironmentVariablesFromFile()
        {
            // Arrange
            File.WriteAllText(envFilePath, "TEST_KEY=TestValue\nANOTHER_KEY=AnotherValue");

            // Act
            EnvLoad.Load();

            // Assert
            Assert.Equal("TestValue", Environment.GetEnvironmentVariable("TEST_KEY"));
            Assert.Equal("AnotherValue", Environment.GetEnvironmentVariable("ANOTHER_KEY"));
        }

        [Fact]
        public void Load_IgnoresMalformedLines()
        {
            // Arrange
            File.WriteAllText(envFilePath, "MALFORMED_LINE\nKEY1=Good");

            // Act
            EnvLoad.Load();

            // Assert
            Assert.Equal("Good", Environment.GetEnvironmentVariable("KEY1"));
            Assert.Null(Environment.GetEnvironmentVariable("MALFORMED_LINE")); // nebude nastavena
        }

        [Fact]
        public void Load_DoesNothingWhenFileDoesNotExist()
        {
            // Arrange
            if (File.Exists(envFilePath))
                File.Delete(envFilePath);

            // Clear env var if it exists
            Environment.SetEnvironmentVariable("SHOULD_NOT_EXIST", null);

            // Act
            EnvLoad.Load();

            // Assert
            Assert.Null(Environment.GetEnvironmentVariable("SHOULD_NOT_EXIST"));
        }

        // Cleanup
        ~EnvLoadTests()
        {
            if (File.Exists(envFilePath))
                File.Delete(envFilePath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using STIN_News_Module.Logic.Logging;
using Xunit;

namespace STIN_News_Module_Test
{
    public class LoggingServiceTests
    {
        // Pomocná metoda pro reset logů mezi testy
        private void ResetLogs()
        {
            var field = typeof(LoggingService)
                .GetField("logs", BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(null, new List<string>());
            }
        }

        [Fact]
        public void AddLog_AddsMessageToLogs()
        {
            // Arrange
            ResetLogs();

            // Act
            LoggingService.AddLog("Test message");
            var logs = LoggingService.GetLogs();

            // Assert
            Assert.Single(logs);
            Assert.Contains("Test message", logs[0]);
        }

        [Fact]
        public void GetLogs_ReturnsCopy_NotReference()
        {
            // Arrange
            ResetLogs();
            LoggingService.AddLog("First");

            // Act
            var logs1 = LoggingService.GetLogs();
            logs1.Add("Fake log"); // změna v kopii
            var logs2 = LoggingService.GetLogs();

            // Assert
            Assert.DoesNotContain("Fake log", logs2);
        }

        [Fact]
        public void AddLog_RespectsMaxLimitOf1000()
        {
            // Arrange
            ResetLogs();
            for (int i = 0; i < 1050; i++)
            {
                LoggingService.AddLog($"Log {i}");
            }

            // Act
            var logs = LoggingService.GetLogs();

            // Assert
            Assert.Equal(1000, logs.Count);
            Assert.Contains("Log 50", logs[0]); // prvních 50 bylo smazáno
            Assert.Contains("Log 1049", logs.Last());
        }

        [Fact]
        public void Logs_AreThreadSafe()
        {
            // Arrange
            ResetLogs();
            int threads = 10;
            int logsPerThread = 100;

            // Act
            Parallel.For(0, threads, i =>
            {
                for (int j = 0; j < logsPerThread; j++)
                {
                    LoggingService.AddLog($"Thread {i} - log {j}");
                }
            });

            var logs = LoggingService.GetLogs();

            // Assert
            Assert.Equal(threads * logsPerThread, logs.Count);
        }
    }
}

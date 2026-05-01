// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Logging;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
internal class AppLoggerProxyWithLog4LoggerTests
{
    private readonly IAppLoggerProxy _logger;

    private readonly LogDataFactory _logDataFactory = new();

    private const string LogFile = "C:\\ProgramData\\ConsoleApp1\\apptest.log";

    public AppLoggerProxyWithLog4LoggerTests()
    {
        var factory = new Log4NetLoggerFactory();
        _logger = new AppLoggerProxy(factory, _logDataFactory);

        if (File.Exists(LogFile))
        {
            File.Delete(LogFile);
        }
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _logger.StopLogging();
        _logger.Dispose();
    }

    [Test]
    public void LogInformation_StringMessage_EntryWritten()
    {
        // Arrange 
        const string message = "Information";

        // Act  
        _logger.LogInformation(message);

        // Assert
        CheckLogFile(message);
    }

    [Test]
    public void LogWarning_StringMessage_EntryWritten()
    {
        // Arrange 
        const string message = "Warning";

        // Act  
        _logger.LogWarning(message);

        // Assert

        CheckLogFile(message);
    }

    [Test]
    public void LogDebug_StringMessage_EntryWritten()
    {
        // Arrange 
        const string message = "Debug";

        // Act  
        _logger.LogDebug(message);

        // Assert

        CheckLogFile(message);
    }

    [Test]
    public void LogDebug_MultipleMessages_EntryWritten()
    {
        // Arrange 
        const string message = "Debug";

        // Act  
        Assert.DoesNotThrow(() =>
        {
            for (var i = 0; i < 10000; i++)
            {
                AsyncHelper.FireAndForget(() =>
                {
                    _logger.LogDebug($"{message}{i}");
                });
            }
        });

        // Assert
        Assert.Pass();
    }

    [Test]
    public void LogError_StringMessage_EntryWritten()
    {
        // Arrange 
        const string message = "Error";

        // Act  
        _logger.LogError(message);

        // Assert
        CheckLogFile(message);
    }

    [Test]
    public void LogCritical_StringMessage_EntryWritten()
    {
        // Arrange 
        const string message = "Critical";

        // Act  
        _logger.LogCritical(message);

        // Assert

        CheckLogFile(message);
    }

    private void CheckLogFile(string message)
    {
        message = $"- {message} -";

        Wait.Until(() => false, 1000);

        var content = File.ReadAllText(LogFile);

        Assert.That(content.Contains(message), Is.EqualTo(true));
    }
}
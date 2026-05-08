// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using Bodoconsult.App.Logging;
using Bodoconsult.App.Test.App;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
internal class AppLoggerExtensionsTests
{
    [Test]
    public void GetDefaultLogger_ValidLogConfig_ReturnsLogger()
    {
        // Arrange 
        var logConfig = Globals.Instance.LoggingConfig;

        // Act  
        var logger = AppLoggerExtensions.GetDefaultLogger(logConfig);

        // Assert
        Assert.That(logger, Is.Not.Null);
    }

    [Test]
    public void GetMonitorLogger_ValidLogConfig_ReturnsLogger()
    {
        // Arrange 
        var fileName = Path.GetTempFileName();
        var logConfig = Globals.Instance.LoggingConfig;

        // Act  
        var logger = AppLoggerExtensions.GetMonitorLogger(logConfig, fileName);

        // Assert
        Assert.That(logger, Is.Not.Null);
    }

    [Test]
    public void GetDefaultAppLoggerProxy_ValidLogConfig_ReturnsLogger()
    {
        // Arrange 
        var logConfig = Globals.Instance.LoggingConfig;

        // Act  
        var logger = AppLoggerExtensions.GetDefaultAppLoggerProxy(logConfig);

        // Assert
        Assert.That(logger, Is.Not.Null);
    }

    [Test]
    public void GetMonitorAppLoggerProxy_ValidLogConfig_ReturnsLogger()
    {
        // Arrange 
        var fileName = Path.GetTempFileName();
        var logConfig = Globals.Instance.LoggingConfig;

        // Act  
        var logger = AppLoggerExtensions.GetMonitorAppLoggerProxy(logConfig, fileName);
        logger.LogInformation("Test");
        logger.LogError("TestError");

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(logger, Is.Not.Null);
            logger.Dispose();
            Task.Delay(200);

            var fi = new FileInfo(fileName);

            Wait.Until(() => fi.Exists);
            Assert.That(fi.Exists, Is.True);

            Assert.That(fi.Length, Is.Not.Zero);

            fi.Delete();
        }
    }
}
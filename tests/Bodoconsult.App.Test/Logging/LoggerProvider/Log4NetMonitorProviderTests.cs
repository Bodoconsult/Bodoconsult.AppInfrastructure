// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Logging;

namespace Bodoconsult.App.Test.Logging.LoggerProvider;

[TestFixture]
internal class Log4NetMonitorProviderTests
{
    [Test]
    public void CreateLogger_DefaultCtor_ReturnsLogger()
    {
        // Arrange 
        var logFilePath = Path.GetTempFileName();
        var provider = new Log4NetMonitorProvider(logFilePath);

        // Act  
        var logger = provider.CreateLogger("Default");

        // Assert
        Assert.That(logger, Is.Not.Null);
    }

    [Test]
    public void CreateLogger_CtorProvidingConfigFile_ReturnsLogger()
    {
        // Arrange 
        var logFilePath = Path.GetTempFileName();

        var s = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(s);

        var dir = new FileInfo(s).DirectoryName;
        ArgumentNullException.ThrowIfNull(dir);

        // ReSharper disable once AssignNullToNotNullAttribute
        s = Path.Combine(dir, "log4net.config");

        var provider = new Log4NetMonitorProvider(logFilePath, s);

        // Act  
        var logger = provider.CreateLogger("Default");

        // Assert
        Assert.That(logger, Is.Not.Null);
    }
}
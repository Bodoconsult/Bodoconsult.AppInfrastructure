// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using Bodoconsult.App.Logging;
using Bodoconsult.App.Test.App;
using System.Diagnostics;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
internal class MonitorLoggerFactoryTests
{
    [Test]
    public void CreateLogger_ValidSetup_LoggerCreated()
    {
        // Arrange 
        const string deviceName = "999999";
        var filePath = Path.Combine(Globals.Instance.DataPath, $"{deviceName}.log");

        DeleteFile(filePath);

        var factory = new MonitorLoggerFactory(filePath);
        factory.LoggingConfig = Globals.Instance.LoggingConfig;

        // Act  
        var logger = factory.CreateLogger("Hallo");

        // Assert
        Assert.That(logger, Is.Not.Null);
    }

    [Test]
    public void LogMessages_ValidSetup_MessagesLogged()
    {
        // Arrange 
        const string deviceName = "999999";
        var filePath = Path.Combine(Globals.Instance.DataPath, $"{deviceName}.log");

        DeleteFile(filePath);

        var factory = new MonitorLoggerFactory(filePath);
        factory.LoggingConfig = Globals.Instance.LoggingConfig;

        var loggerProxy = new AppLoggerProxy(factory, Globals.Instance.LogDataFactory);

        // Act  
        loggerProxy.LogInformation("Testinfo");
        loggerProxy.LogDebug("Testdebug");
        loggerProxy.LogError("Testerror");

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(loggerProxy, Is.Not.Null);
            loggerProxy.Dispose();
            Task.Delay(200);

            var fi = new FileInfo(filePath);

            Wait.Until(() => fi.Exists);
            Assert.That(fi.Exists, Is.True);

            Assert.That(fi.Length, Is.Not.Zero);
        }

        DeleteFile(filePath);
    }

    [Test]
    public void LogMessages_2Loggers_MessagesLogged()
    {
        // Arrange 
        const string deviceName = "999999";
        var filePath = Path.Combine(Globals.Instance.DataPath, $"{deviceName}1.log");
        var filePath2 = Path.Combine(Globals.Instance.DataPath, $"{deviceName}2.log");

        DeleteFile(filePath);
        DeleteFile(filePath2);

        var factory = new MonitorLoggerFactory(filePath);
        factory.LoggingConfig = Globals.Instance.LoggingConfig;

        var factory2 = new MonitorLoggerFactory(filePath2);
        factory2.LoggingConfig = Globals.Instance.LoggingConfig;

        var loggerProxy = new AppLoggerProxy(factory, Globals.Instance.LogDataFactory);
        var loggerProxy2 = new AppLoggerProxy(factory2, Globals.Instance.LogDataFactory);

        // Act  
        loggerProxy.LogInformation("Testinfo");
        loggerProxy2.LogInformation("Testinfo2");
        loggerProxy.LogDebug("Testdebug");
        loggerProxy2.LogDebug("Testdebug2");
        loggerProxy.LogError("Testerror");
        loggerProxy2.LogError("Testerror2");

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(loggerProxy, Is.Not.Null);
            loggerProxy.Dispose();
            Task.Delay(200);

            Assert.That(loggerProxy2, Is.Not.Null);
            loggerProxy2.Dispose();
            Task.Delay(200);

            var fi = new FileInfo(filePath);

            Wait.Until(() => fi.Exists);
            Assert.That(fi.Exists, Is.True);

            Assert.That(fi.Length, Is.Not.Zero);

            var fi2 = new FileInfo(filePath2);

            Wait.Until(() => fi2.Exists);
            Assert.That(fi2.Exists, Is.True);

            Assert.That(fi2.Length, Is.Not.Zero);

            Assert.That(fi2.Length, Is.GreaterThan(fi.Length));
        }

        DeleteFile(filePath);
        DeleteFile(filePath2);
    }

    private static void DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception e)
        {
            Debug.Print(e.Message);
            throw;
        }
    }
}
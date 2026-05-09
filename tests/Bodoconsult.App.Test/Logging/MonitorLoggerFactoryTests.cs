// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using Bodoconsult.App.Logging;
using Bodoconsult.App.Test.App;
using Bodoconsult.App.Test.DataExportServices;
using NUnit.Framework.Legacy;
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
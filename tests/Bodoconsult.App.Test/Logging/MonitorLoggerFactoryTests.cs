// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Logging;
using Bodoconsult.App.Test.App;
using NUnit.Framework.Legacy;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
public class MonitorLoggerFactoryTests
{
    [Test]
    public void TestCreateLogger()
    {
        // Arrange 
        const string towerSn = "999999";
        var filePath = Path.Combine(Globals.Instance.DataPath, $"{towerSn}.log");

        DeleteFile(filePath);

        var factory = new MonitorLoggerFactory(filePath);

        // Act  
        var logger = factory.CreateLogger("Hallo");

        // Assert
        Assert.That(logger, Is.Not.Null);

    }


    [Test]
    public void TestCheckQueue()
    {
        // Arrange 
        const string towerSn = "999999";
        var filePath = Path.Combine(Globals.Instance.DataPath, $"{towerSn}.log");

        DeleteFile(filePath);

        var factory = new MonitorLoggerFactory(filePath);
        var loggerProxy = new AppLoggerProxy(factory, Globals.Instance.LogDataFactory);
        loggerProxy.LogError("Testerror");

        // Act  

        // Assert
        FileAssert.Exists(filePath);
        loggerProxy.Dispose();
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
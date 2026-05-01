// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Logging;
using Bodoconsult.App.Test.App;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
internal class MonitorLoggerFactoryFactoryTests
{
    [Test]
    public void CreateInstance_DeviceName_FactoryCreated()
    {
        // Arrange 
        const string deviceName = "999999";

        var factory = new MonitorLoggerFactoryFactory(Globals.Instance);

        // Act  
        var logger = factory.CreateInstance(deviceName);

        // Assert
        Assert.That(logger, Is.Not.Null);
        Assert.That(logger.FileName, Is.Not.Null);
    }

    [Test]
    public void CreateInstance_TypeAndIP_FactoryCreated()
    {
        // Arrange 
        const string deviceName = "999999";
        const string ip = "127.0.0.1";

        var factory = new MonitorLoggerFactoryFactory(Globals.Instance);

        // Act  
        var logger = factory.CreateInstance(deviceName, ip);

        // Assert
        Assert.That(logger, Is.Not.Null);
        Assert.That(logger.FileName, Is.Not.Null);
    }
}
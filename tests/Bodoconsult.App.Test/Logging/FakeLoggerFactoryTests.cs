// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Logging;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
internal class FakeLoggerFactoryTests: BaseFakeLoggerTests
{

    [SetUp]
    public void Setup()
    {
        LoggedMessages.Clear();
    }

    [Test]
    public void TestCreateLogger()
    {
        // Arrange 
        var factory = new FakeLoggerFactory
        {
            FakeLogDelegate = FakeLogDelegate
        };

        // Act  
        var fake = (FakeLogger)factory.CreateLogger("TestCategrory");
            
        Assert.That(fake, Is.Not.Null);
        Assert.That(LoggedMessages.Count == 0);

        var logger = (ILogger)fake;
        logger.Log(LogLevel.Critical, "Hallo");

        // Assert
        Assert.That(LoggedMessages.Count == 1);

        factory.Dispose();
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFileTransferCore.Business.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test.Logging;

[TestFixture]
internal class UnitTestsFakeLoggerFactory: BaseFakeLoggerTests
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

    }
}
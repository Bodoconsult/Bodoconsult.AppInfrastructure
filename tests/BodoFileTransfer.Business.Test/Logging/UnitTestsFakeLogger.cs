// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using BodoFileTransferCore.Business.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test.Logging;

[TestFixture]
internal class UnitTestsFakeLogger: BaseFakeLoggerTests
{

    [SetUp]
    public void Setup()
    {
        LoggedMessages.Clear();
    }


    [Test]
    public void TestLog()
    {
        // Arrange 
        var fake = new FakeLogger("TestCategrory")
        {
            FakeLogDelegate = FakeLogDelegate
        };

        logger = fake;

        // Act  
        logger.Log(LogLevel.Critical, "Hallo");

        // Assert
        Assert.That(LoggedMessages.Count == 1);

    }


}
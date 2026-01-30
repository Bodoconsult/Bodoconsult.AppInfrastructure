// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Logging;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
internal class FakeLoggerTests: BaseFakeLoggerTests
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

        Logger = fake;

        // Act  
        Logger.Log(LogLevel.Critical, "Hallo");

        // Assert
        Assert.That(LoggedMessages.Count == 1);

    }


}
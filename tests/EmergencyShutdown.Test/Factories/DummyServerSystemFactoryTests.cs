// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using EmergencyShutdown.Business.Factories;
using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;

namespace EmergencyShutdown.Test.Factories;

[TestFixture]
internal class DummyServerSystemFactoryTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void CreateInstance_validSetup_InstanceCreated()
    {
        // Arrange
        var factory = new DummyServerSystemFactory();
        var server = new DummyServer();

        var config = new ShutdownConfig();

        // Act
        var result = factory.CreateInstance(server, config);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ShutdownConfig, Is.EqualTo(config));
        Assert.That(result.Server, Is.EqualTo(server));
    }
}
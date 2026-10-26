// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Windows.ShellTools;
using EmergencyShutdown.Business.Factories;
using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;

namespace EmergencyShutdown.Test.Factories;

[TestFixture]
internal class MasterServerSystemFactoryTests
{
    [Test]
    public void CreateInstance_ValidDummyServer_InstanceCreated()
    {
        // Arrange
        var shellProcessManager = new WinShellProcessManager();
        var factory = new MasterServerSystemFactory(shellProcessManager);
        var server = new DummyServer();

        var config = new ShutdownConfig();

        // Act
        var result = factory.CreateInstance(server, config);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ShutdownConfig, Is.EqualTo(config));
        Assert.That(result.Server, Is.EqualTo(server));
    }

    [Test]
    public void CreateInstance_Valid_WindowsServer_InstanceCreated()
    {
        // Arrange
        var shellProcessManager = new WinShellProcessManager();
        var factory = new MasterServerSystemFactory(shellProcessManager);
        var server = new WindowsServer();

        var config = new ShutdownConfig();

        // Act
        var result = factory.CreateInstance(server, config);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ShutdownConfig, Is.EqualTo(config));
        Assert.That(result.Server, Is.EqualTo(server));
    }
}

[TestFixture]
internal class WindowsServerSystemFactoryTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void CreateInstance_validSetup_InstanceCreated()
    {
        // Arrange
        var shellProcessManager = new WinShellProcessManager();
        var factory = new WindowsServerSystemFactory(shellProcessManager);
        var server = new WindowsServer();

        var config = new ShutdownConfig();
            
        // Act
        var result = factory.CreateInstance(server, config);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ShutdownConfig, Is.EqualTo(config));
        Assert.That(result.Server, Is.EqualTo(server));
    }
}
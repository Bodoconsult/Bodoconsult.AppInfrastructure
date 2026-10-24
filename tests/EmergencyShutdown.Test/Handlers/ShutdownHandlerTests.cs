// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using EmergencyShutdown.Business.Factories;
using EmergencyShutdown.Business.Handler;
using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;

namespace EmergencyShutdown.Test.Handlers;

[TestFixture]
internal class ShutdownHandlerTests
{
    private readonly ShutdownConfig _shutdownConfig = new();
    private readonly MasterServerSystemFactory _masterServerSystemFactory = new();

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorrectly()
    {
        // Arrange
        var workload = new Workload();
        workload.Servers.Add(new DummyServer());

        // Act
        var h = new ShutdownHandler(_masterServerSystemFactory, _shutdownConfig);

        // Assert
        Assert.That(h.Servers.Count, Is.Zero);
    }

    [Test]
    public void LoadWorkload_ValidWorkload_ServersLoaded()
    {
        // Arrange
        var workload = new Workload();
        workload.Servers.Add(new DummyServer());

        var h = new ShutdownHandler(_masterServerSystemFactory, _shutdownConfig);

        // Act
        h.LoadWorkload(workload);

        // Assert
        Assert.That(h.Servers.Count, Is.Not.Zero);
    }

    [Test]
    public void ProcessWorkload_ValidWorkload_ServersShutdown()
    {
        // Arrange
        var workload = new Workload();
        workload.Servers.Add(new DummyServer());

        var h = new ShutdownHandler(_masterServerSystemFactory, _shutdownConfig);
        h.LoadWorkload(workload);

        // Act
        h.ProcessWorkload();

        // Assert
        Assert.That(h.Servers.Count, Is.Not.Zero);
        Assert.That(h.Processed, Is.True);
    }
}
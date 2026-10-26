// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using EmergencyShutdown.Business.Helpers;
using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;

namespace EmergencyShutdown.Test.Handlers;

[TestFixture]
internal class WorkloadTests
{
    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorrectly()
    {
        // Arrange

        // Act
        var workload = new Workload();
        workload.Servers.Add(new WindowsServer());
        workload.Servers.Add(new WindowsServer());

        // Assert
        Assert.That(workload.Servers.Count, Is.Not.Zero);

        var fileName = Path.Combine(Path.GetTempPath(), "cred.json");
        JsonHelper.SerializeAsFile(fileName, workload);

        FileSystemHelper.RunInDebugMode(fileName);
    }
    [Test]
    public void LoadJson_ValidDefaultSetup_SerializedAndDeserializedCorrectly()
    {
        // Arrange
        var workload = new Workload();
        workload.Servers.Add(new WindowsServer());
        workload.Servers.Add(new WindowsServer());

        Assert.That(workload.Servers.Count, Is.Not.Zero);

        var json = JsonHelper.Serialize(workload);

        // Act
        var result = JsonHelper.LoadJsonString<Workload>(json);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Servers.Count, Is.EqualTo(workload.Servers.Count));
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using EmergencyShutdown.Business.Helpers;
using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;

namespace EmergencyShutdown.Test.Handlers;

[TestFixture]
internal class EmergencyShutdownConsoleStarterTests
{
    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var result = new EmergencyShutdownConsoleStarter([], true);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.Workload, Is.Null);
            Assert.That(result.DataProtectionService, Is.Not.Null);
            Assert.That(result.CredentialsFileName, Is.Not.Null);
            Assert.That(result.CredentialsImportFileName, Is.Not.Null);
        }
    }

    [Test]
    public void LoadWorkload_ValidImportFile_WorkloadLoaded()
    {
        // Arrange 
        var result = new EmergencyShutdownConsoleStarter([], true);

        FileSystemHelper.DeleteFile(result.CredentialsFileName);

        var workload = new Workload();
        workload.Servers.Add(new WindowsServer());
        workload.Servers.Add(new WindowsServer());

        // Assert
        Assert.That(workload.Servers.Count, Is.Not.Zero);

        var fileName = result.CredentialsImportFileName;
        JsonHelper.SerializeAsFile(fileName, workload);

        Assert.That(File.Exists(fileName));

        // Act  
        result.LoadWorkload();
        
        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Workload, Is.Not.Null);
            Assert.That(result.DataProtectionService, Is.Not.Null);
            Assert.That(result.CredentialsFileName, Is.Not.Null);
            Assert.That(result.CredentialsImportFileName, Is.Not.Null);
        }
    }

    [Test]
    public void LoadWorkload_ValidImportFileReimport_WorkloadLoaded()
    {
        // Arrange 
        var result = new EmergencyShutdownConsoleStarter([], true);

        FileSystemHelper.DeleteFile(result.CredentialsFileName);

        var workload = new Workload();
        workload.Servers.Add(new WindowsServer());
        workload.Servers.Add(new WindowsServer());

        // Assert
        Assert.That(workload.Servers.Count, Is.Not.Zero);

        var fileName = result.CredentialsImportFileName;
        JsonHelper.SerializeAsFile(fileName, workload);

        Assert.That(File.Exists(fileName));

        // Act  
        result.LoadWorkload();

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Workload, Is.Not.Null);
            Assert.That(result.DataProtectionService, Is.Not.Null);
            Assert.That(result.CredentialsFileName, Is.Not.Null);
            Assert.That(result.CredentialsImportFileName, Is.Not.Null);
            Assert.That(File.Exists(result.CredentialsFileName));
        }

        // Act 2
        result = new EmergencyShutdownConsoleStarter([], true);
        result.LoadWorkload();

        // Assert2
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Workload, Is.Not.Null);
            Assert.That(result.DataProtectionService, Is.Not.Null);
            Assert.That(result.CredentialsFileName, Is.Not.Null);
            Assert.That(result.CredentialsImportFileName, Is.Not.Null);
        }
    }

    [Test]
    public void Start_ValidImportFile_WorkloadProcessed()
    {
        // Arrange 
        var starter = new EmergencyShutdownConsoleStarter([], true);

        FileSystemHelper.DeleteFile(starter.CredentialsFileName);

        var workload = new Workload();
        workload.Servers.Add(new DummyServer());
        workload.Servers.Add(new DummyServer());

        // Assert
        Assert.That(workload.Servers.Count, Is.Not.Zero);

        var fileName = starter.CredentialsImportFileName;
        JsonHelper.SerializeAsFile(fileName, workload);

        Assert.That(File.Exists(fileName));

        starter.LoadWorkload();

        // Act  
        var result = starter.Start();

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Zero);
        }
    }
}
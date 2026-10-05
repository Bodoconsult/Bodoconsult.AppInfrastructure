// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.ShellTools;
using Bodoconsult.App.Backup;
using Bodoconsult.App.Windows.Backup;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.Backup;

[TestFixture]
internal class BackupManagerTests
{
    private readonly string _target = Path.GetTempPath();

    [TearDown]
    public void CleanUp()
    {
        var targetDir = Path.Combine(_target, "Blubb");
        if (Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir, true);
        }

        targetDir = Path.Combine(_target, "Blabb");
        if (Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir, true);
        }
    }

    [Test]
    public void StartBackup_SimpleBackupMode_BackupExecuted()
    {
        // Arrange 
        var newDir = Path.Combine(_target, "Blubb");
        if (!Directory.Exists(newDir))
        {
            Directory.CreateDirectory(newDir);
        }

        Assert.That(Directory.Exists(newDir), Is.True);

        var targetDir = Path.Combine(_target, "Blabb");

        if (Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir);
        }

        var spm = new FakeShellProcessManager();

        var factory = new RobocopyBackupTargetFactory(spm);


        List<Exception> errors = new();
        var jobTask = new BasicBackupConfig
        {
            Errors = errors,
            StatusChanged = StatusChanged
        };

        jobTask.BackupTargets.Add(new BackupTargetSettings
        {
            BackupMode = BackupModeEnum.Simple,
            Count = 5,
            Source = newDir,
            Target =    targetDir,
            Thread = 1
        });
        
        var bm = new BackupManager(jobTask, factory);

        // Act
        bm.StartBackup();

        // Assert
        Assert.That(spm.Commands.Count, Is.EqualTo(1));
    }

    [Test]
    public void ActivateShadowing_SimpleBackupMode_BackupExecuted()
    {
        // Arrange 
        var newDir = Path.Combine(_target, "Blubb");
        if (!Directory.Exists(newDir))
        {
            Directory.CreateDirectory(newDir);
        }

        Assert.That(Directory.Exists(newDir), Is.True);

        var targetDir = Path.Combine(_target, "Blabb");

        if (Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir);
        }

        var spm = new FakeShellProcessManager();

        var factory = new RobocopyBackupTargetFactory(spm);


        List<Exception> errors = new();
        var jobTask = new BasicBackupConfig
        {
            GetAliveFolder = "C:\\temp",
            ShadowingSourceDrive = "C:",
            ShadowingTargetDrive = "H:",
            Errors = errors,
            StatusChanged = StatusChanged
        };

        jobTask.BackupTargets.Add(new BackupTargetSettings
        {
            BackupMode = BackupModeEnum.Simple,
            Count = 5,
            Source = newDir,
            Target = targetDir,
            Thread = 1
        });

        var bm = new BackupManager(jobTask, factory);

        // Act
        bm.CheckStorageMedia();
        bm.ActivateShadowing();
        bm.StartBackup();
        bm.DeactivateShadowing();

        // Assert
        Assert.That(spm.Commands.Count, Is.EqualTo(1));
    }

    [Test]
    public void CheckStorageMedia_SimpleBackupMode_BackupExecuted()
    {
        // Arrange 
        var newDir = Path.Combine(_target, "Blubb");
        if (!Directory.Exists(newDir))
        {
            Directory.CreateDirectory(newDir);
        }

        Assert.That(Directory.Exists(newDir), Is.True);

        var targetDir = Path.Combine(_target, "Blabb");

        if (Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir);
        }

        var spm = new FakeShellProcessManager();

        var factory = new RobocopyBackupTargetFactory(spm);


        List<Exception> errors = new();
        var jobTask = new BasicBackupConfig
        {
            GetAliveFolder = "C:\\temp",
            Errors = errors,
            StatusChanged = StatusChanged
        };

        jobTask.BackupTargets.Add(new BackupTargetSettings
        {
            BackupMode = BackupModeEnum.Simple,
            Count = 5,
            Source = newDir,
            Target = targetDir,
            Thread = 1
        });

        var bm = new BackupManager( jobTask, factory);

        // Act
        bm.CheckStorageMedia();
        bm.StartBackup();

        // Assert
        Assert.That(spm.Commands.Count, Is.EqualTo(1));
    }





    private void StatusChanged(string message)
    {
        Debug.Print(message);
    }
}
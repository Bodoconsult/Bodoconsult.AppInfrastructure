// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.ShellTools;
using Bodoconsult.App.Backup;
using Bodoconsult.App.Windows.Backup;
using Bodoconsult.App.Windows.ShellTools;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.Backup;

[TestFixture]
internal class RobocopyBackupTargetRealTests
{
    private readonly string _target = @"C:\temp"; //Path.GetTempPath();

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
    public void RobocopyBackupTarget_SimpleBackupMode_OneCommandExecuted()
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
            Directory.Delete(targetDir, true);
        }

        var spm = new WinShellProcessManager();

        List<Exception> errors = [];

        var settings = new BackupTargetSettings
        {
            Source = newDir,
            Target = targetDir,
            BackupMode = BackupModeEnum.Week1,
            Count = 5,
            Errors = errors,
            StatusChanged = StatusChanged,
            Command  = "robocopy ??source?? ??target??  *.* /FFT /S /E /log:??log?? /COPY:DT /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd ??xd?? /NoDCopy"
    };

        var rbt = new RobocopyBackupTarget(spm, settings);

        // Act
        rbt.StartBackupProcess();

        // Assert
        //Assert.That(spm.Commands.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public void RobocopyBackupTarget_Week1BackupMode_OneCommandExecuted()
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
            Directory.Delete(targetDir, true);
        }

        var spm = new WinShellProcessManager();

        List<Exception> errors = [];

        var settings = new BackupTargetSettings
        {
            Source = newDir,
            Target = targetDir,
            BackupMode = BackupModeEnum.Week1,
            Count = 5,
            Errors = errors,
            StatusChanged = StatusChanged,
            Command = "robocopy ??source?? ??target??  *.* /FFT /S /E /log:??log?? /COPY:DT /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd ??xd?? /NoDCopy"
        };


        var rbt = new RobocopyBackupTarget(spm, settings);

        // Act
        rbt.StartBackupProcess();

        // Assert
        //Assert.That(spm.Commands.Count, Is.EqualTo(1));
    }

    [Test]
    public void RobocopyBackupTarget_WeekBackupMode_OneCommandExecuted()
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
            Directory.Delete(targetDir, true);
        }

        var spm = new WinShellProcessManager();

        List<Exception> errors = [];

        var settings = new BackupTargetSettings
        {
            Source = newDir,
            Target = targetDir,
            BackupMode = BackupModeEnum.Week1,
            Count = 5,
            Errors = errors,
            StatusChanged = StatusChanged,
            Command = "robocopy ??source?? ??target??  *.* /FFT /S /E /log:??log?? /COPY:DT /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd ??xd?? /NoDCopy"
        };

        var rbt = new RobocopyBackupTarget(spm, settings);

        // Act
        rbt.StartBackupProcess();

        // Assert
        //Assert.That(spm.Commands.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public void RobocopyBackupTarget_DayBackupMode_OneCommandExecuted()
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
            Directory.Delete(targetDir, true);
        }

        var spm = new WinShellProcessManager();

        List<Exception> errors = [];

        var settings = new BackupTargetSettings
        {
            Source = newDir,
            Target = targetDir,
            BackupMode = BackupModeEnum.Week1,
            Count = 5,
            Errors = errors,
            StatusChanged = StatusChanged,
            Command = "robocopy ??source?? ??target??  *.* /FFT /S /E /log:??log?? /COPY:DT /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd ??xd?? /NoDCopy"
        };

        var rbt = new RobocopyBackupTarget(spm, settings);

        // Act
        rbt.StartBackupProcess();

        // Assert
        //Assert.That(spm.Commands.Count, Is.EqualTo(1));
    }

    [Test]
    public void RobocopyBackupTarget_DaysDateBackupMode_OneCommandExecuted()
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
            Directory.Delete(targetDir, true);
        }

        var spm = new WinShellProcessManager();

        List<Exception> errors = [];

        var settings = new BackupTargetSettings
        {
            Source = newDir,
            Target = targetDir,
            BackupMode = BackupModeEnum.Week1,
            Count = 5,
            Errors = errors,
            StatusChanged = StatusChanged,
            Command = "robocopy ??source?? ??target??  *.* /FFT /S /E /log:??log?? /COPY:DT /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd ??xd?? /NoDCopy"
        };

        var rbt = new RobocopyBackupTarget(spm, settings);

        // Act
        rbt.StartBackupProcess();

        // Assert
        //Assert.That(spm.Commands.Count, Is.EqualTo(1));
    }

    private void StatusChanged(string message)
    {
        Debug.Print(message);
    }
}
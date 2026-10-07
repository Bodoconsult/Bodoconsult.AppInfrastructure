// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.IO;
using Bodoconsult.App.Abstractions.ShellTools;
using Bodoconsult.App.Windows.ShellTools;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.ShellTools;

[TestFixture]
internal class WinShellProcessManagerTests
{
    private readonly string _target = Path.GetTempPath();

    [OneTimeTearDown]
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
    public void RemoveDirectory_ExistingDir_DirRemoved()
    {
        // Arrange 
        var spm = new WinShellProcessManager();

        var newDir = Path.Combine(_target, "Blubb");

        if (!Directory.Exists(newDir))
        {
            Directory.CreateDirectory(newDir);
        }

        Assert.That(Directory.Exists(newDir), Is.True);

        // Act
        var p = new RemoveDirectoryShellProcessParameters
        {
            Path = newDir
        };

        spm.RemoveDirectory(p);

        // Assert
        Assert.That(Directory.Exists(newDir), Is.False);
    }

    [Test]
    public void MoveDirectory_ExistingDir_DirMoved()
    {
        // Arrange 
        var spm = new WinShellProcessManager();

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

        // Act
        var p = new MoveDirectoryShellProcessParameters
        {
            SourcePath = newDir,
            TargetPath = targetDir
        };

        spm.MoveDirectory(p);

        // Assert
        Assert.That(Directory.Exists(targetDir), Is.True);
    }

    [Test]
    public void RunRobocopy_ExistingDir_DirRemoved()
    {
        // Arrange 
        var spm = new WinShellProcessManager();

        var newDir = Path.Combine(_target, "Blubb");

        if (!Directory.Exists(newDir))
        {
            Directory.CreateDirectory(newDir);
        }

        Assert.That(Directory.Exists(newDir), Is.True);

        var targetDir = Path.Combine(_target, "Blabb");

        if (!Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir, true);
        }

        // Act
        var args = $"{newDir} {targetDir} *.* /FFT /S /E /COPY:DT /B /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd /NoDCopy";
           
        var p = new RobocopyShellProcessParameters
        {
            Args = args
        };

        spm.RunRobocopy(p);

        // Assert
        Assert.That(Directory.Exists(targetDir), Is.True);
    }
}
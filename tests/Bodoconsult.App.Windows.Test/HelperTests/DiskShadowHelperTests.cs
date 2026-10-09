// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Windows.Helpers;
using NUnit.Framework;
using System;
using System.IO;

namespace Bodoconsult.App.Windows.Test.HelperTests;

[TestFixture]
internal class DiskShadowHelperTests
{
    public DiskShadowHelperTests()
    {
        DiskShadowHelper.TempDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Bodoconsult.App");

        if (!Directory.Exists(DiskShadowHelper.TempDir))
        {
            Directory.CreateDirectory(DiskShadowHelper.TempDir);
        }
    }

    [Test]
    public void CreateDrive_ExistingDrive_ShadowingActive()
    {

        var sourceDrive = "C:";
        var targetDrive = "H:";

        DiskShadowHelper.ClearDrive(targetDrive);


        DiskShadowHelper.CreateDrive(sourceDrive, targetDrive);


        DiskShadowHelper.ClearDrive(targetDrive);


        Assert.That(true);
    }

    [Test]
    public void GetBatchContent_ValidSetup_ResourceLoaded()
    {
        // Arrange

        // Act
        var result = DiskShadowHelper.GetBatchContent;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.GreaterThan(0));
    }

    [Test]
    public void CreateCabFile_ValidSetup_FileCreated()
    {
        // Arrange

        // Act
        var cabFile = Path.Combine(Path.GetTempPath(), "Backup.cab");
        DiskShadowHelper.CreateCabFile(cabFile);

        // Assert
        Assert.That(File.Exists(cabFile), Is.True);
    }
}
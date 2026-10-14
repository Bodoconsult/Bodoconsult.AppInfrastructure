// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.IO;
using System.Runtime.Versioning;
using Bodoconsult.Drawing.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Drawing.Test;

[SupportedOSPlatform("windows")]
[TestFixture]

public class ImageToolsTests
{


    //[SetUp]
    //public void Setup()
    //{

    //}


    //[TearDown]
    //public void Cleanup()
    //{
    //}


    [Test]
    public void TestGetImageSize()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");

        // Act
        var result = ImageTools.GetImageSize(source);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Width > 0);
        Assert.That(result.Height > 0);

    }

    [Test]
    public void TestGenerateThumb()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");
        var fi = new FileInfo(source);

        const int width = 500;

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

        if (File.Exists(target))
        {
            File.Delete(target);
        }
        Assert.That(!File.Exists(target));

        // Act
        ImageTools.GenerateThumb(fi, target, width, width);

        // Assert
        Assert.That(File.Exists(target));

        var result = ImageTools.GetImageSize(target);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Width > 0);
        Assert.That(result.Height > 0);
        Assert.That(result.Width == width);

        //TestHelper.StartFile(target);

    }


    [Test]
    public void TestGenerateWebImage()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");


        const int width = 500;
        const int maxSize = 1000000;

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

        if (File.Exists(target))
        {
            File.Delete(target);
        }
        Assert.That(!File.Exists(target));

        File.Copy(source, target);

        var fi = new FileInfo(target);

        // Act
        ImageTools.GenerateWebImage(fi, maxSize, width, width);

        // Assert
        Assert.That(File.Exists(target));

        var result = ImageTools.GetImageSize(target);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Width > 0);
        Assert.That(result.Height > 0);
        Assert.That(result.Width == width);

        //TestHelper.StartFile(target);

    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using Bodoconsult.Drawing.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Drawing.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
public class BitmapServiceTests
{
    private BitmapService _service;

    [SetUp]
    public void Setup()
    {
        _service = new BitmapService();
    }


    [TearDown]
    public void Cleanup()
    {
    }

    [Test]
    public void TestResizeImage()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

        if (File.Exists(target))
        {
            File.Delete(target);
        }
        Assert.That(!File.Exists(target));

        _service.LoadBitmap(source);

        // Act
        _service.ResizeImage(400, 400);
            
        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }

    [Test]
    public void TestResizeImageAdjustBcg()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

        if (File.Exists(target))
        {
            File.Delete(target);
        }
        Assert.That(!File.Exists(target));

        var brightness = 1.4f;
        var contrast = 0.8f;
        var gamma = 1f;




        _service.LoadBitmap(source);
        _service.ResizeImage(400, 400);

        // Act
        _service.AdjustBcg(brightness, contrast, gamma);

        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }


    [Test]
    public void TestResizeImageAdjustSaturation()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

        if (File.Exists(target))
        {
            File.Delete(target);
        }
        Assert.That(!File.Exists(target));

        var saturation = -1F;

        _service.LoadBitmap(source);
        _service.ResizeImage(400, 400);

        // Act
        _service.AdjustSaturation(saturation);

        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }


    [Test]
    public void TestCaseWebReporting()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");

        var target = Path.Combine( TestHelper.TargetFolder, "DSC_0187.jpg");

        if (File.Exists(target))
        {
            File.Delete(target);
        }
        Assert.That(!File.Exists(target));

        var brightness = 1.45f; // no change in brightness
        var contrast = 0.9f; // twice the contrast
        var gamma = 1f; // no change in gamma




        _service.LoadBitmap(source);
        _service.ResizeImage(400, 400);

        // Act
        _service.AdjustSaturation(-1F);
        _service.AdjustBcg(brightness, contrast, gamma);

        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }



    [Test]
    public void TestRoundedCorners()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

        if (File.Exists(target))
        {
            File.Delete(target);
        }
        Assert.That(!File.Exists(target));

        _service.LoadBitmap(source);
        _service.ResizeImage(400, 400);

        // Act
        _service.RoundCorners(15, Color.White,  2, Color.Black, true, 10);

        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }

}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Drawing.SkiaSharp.Services;
using Bodoconsult.Drawing.SkiaSharp.Tests.Helpers;
using SkiaSharp;

namespace Bodoconsult.Drawing.SkiaSharp.Tests;

//[SupportedOSPlatform("windows")]
[TestFixture]
internal class BitmapServiceTests
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
        _service.Dispose();
    }


    [Test]
    public void DrawText_Rectangle_TextWrittenLeftBound()
    {
        // Arrange 
        var target = Path.Combine(TestHelper.TargetFolder, "test.jpg");
        TestHelper.DeleteFile(target);

        _service.NewBitmap(600, 800);
        _service.DrawRectangle(0,0, 600, 800, SKColors.White, SKColors.White);

        var rect = new SKRect(100, 100, 500, 700);

        const string text = TestHelper.MassText;

        // Act  
        _service.DrawText(text, rect, "Arial", 11, SKColors.Black, SKTextAlign.Left);
        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }

    [Test]
    public void DrawText_Rectangle_TextWrittenRightBound()
    {
        // Arrange 
        var target = Path.Combine(TestHelper.TargetFolder, "test.jpg");
        TestHelper.DeleteFile(target);

        _service.NewBitmap(600, 800);
        _service.DrawRectangle(0, 0, 600, 800, SKColors.White, SKColors.White);

        var rect = new SKRect(100, 100, 500, 700);

        const string text = TestHelper.MassText;

        // Act  
        _service.DrawText(text, rect, "Arial", 11, SKColors.Black, SKTextAlign.Right);
        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }

    [Test]
    public void DrawText_Rectangle_TextWrittenCentered()
    {
        // Arrange 
        var target = Path.Combine(TestHelper.TargetFolder, "test.jpg");
        TestHelper.DeleteFile(target);

        _service.NewBitmap(600, 800);
        _service.DrawRectangle(0, 0, 600, 800, SKColors.White, SKColors.White);

        var rect = new SKRect(100, 100, 500, 700);

        const string text = TestHelper.MassText;

        // Act  
        _service.DrawText(text, rect, "Arial", 11, SKColors.Black, SKTextAlign.Center);
        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }

    [Test]
    public void ResizeImage_ValidImage_ImageResized()
    {
        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder, "DSC_0187.JPG");

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");
        TestHelper.DeleteFile(target);

        Assert.That(!File.Exists(target));

        _service.LoadBitmap(source);

        // Act
        _service.ResizeImage(400, 400);
            
        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }

    //[Test]
    //public void TestResizeImageAdjustBcg()
    //{

    //    // Arrange
    //    var source = Path.Combine(TestHelper.GetTestDataFolder, "DSC_0187.JPG");

    //    var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

    //    TestHelper.DeleteFile(target);
    //    Assert.That(!File.Exists(target));

    //    var brightness = 1.4f;
    //    var contrast = 0.8f;
    //    var gamma = 1f;




    //    _service.LoadBitmap(source);
    //    _service.ResizeImage(400, 400);

    //    // Act
    //    _service.AdjustBcg(brightness, contrast, gamma);

    //    _service.SaveAsJpeg(target);

    //    // Assert
    //    Assert.That(File.Exists(target));

    //    TestHelper.StartFile(target);
    //}

    [Test]
    public void AdjustSaturation_ValidImage_ImageResizedAndSaturationChanged()
    {
        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder, "DSC_0187.JPG");

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

        TestHelper.DeleteFile(target);
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
    public void AdjustSaturation_ValidImageBodoWebReporting_ImageResizedAndSaturationChanged()
    {
        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder, "DSC_0187.JPG");

        var target = Path.Combine( TestHelper.TargetFolder, "DSC_0187.jpg");

        TestHelper.DeleteFile(target);
        Assert.That(!File.Exists(target));

        var brightness = 1.45f; // no change in brightness
        var contrast = 0.9f; // twice the contrast
        var gamma = 1f; // no change in gamma




        _service.LoadBitmap(source);
        _service.ResizeImage(400, 400);

        // Act
        _service.AdjustSaturation(-1F);
        //_service.AdjustBcg(brightness, contrast, gamma);

        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }

    [Test]
    public void TestRoundedCorners()
    {

        // Arrange
        var source = Path.Combine(TestHelper.GetTestDataFolder, "DSC_0187.JPG");

        var target = Path.Combine(TestHelper.TargetFolder, "DSC_0187.jpg");

        TestHelper.DeleteFile(target);
        Assert.That(!File.Exists(target));

        _service.LoadBitmap(source);
        _service.ResizeImage(400, 400);

        // Act
        //_service.RoundCorners(15, Color.White,  2, Color.Black, true, 10);

        _service.SaveAsJpeg(target);

        // Assert
        Assert.That(File.Exists(target));

        TestHelper.StartFile(target);
    }
}
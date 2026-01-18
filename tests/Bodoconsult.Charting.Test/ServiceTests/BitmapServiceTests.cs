// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.IO;
using Bodoconsult.Charting.Test.Helpers;
using Bodoconsult.Drawing.SkiaSharp.Services;
using NUnit.Framework;
using SkiaSharp;

namespace Bodoconsult.Charting.Test.ServiceTests;

[TestFixture]
internal class BitmapServiceTests
{

    private string _imagePath =  Path.Combine(TestHelper.TestDataPath, "DSC_0126.JPG");


    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var service = new BitmapService();

        // Assert
        Assert.That(service.CurrentBitmap, Is.Null);
        Assert.That(service.CurrentCanvas, Is.Null);
    }

    [Test]
    public void NewBitmap_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        const int width = 600;
        const int height = 400;
        var service = new BitmapService();

        // Act  
        service.NewBitmap(width, height);

        // Assert
        Assert.That(service.CurrentBitmap, Is.Not.Null);
        Assert.That(service.CurrentCanvas, Is.Not.Null);
        Assert.That(service.CurrentBitmap.Width, Is.EqualTo(width));
        Assert.That(service.CurrentBitmap.Height, Is.EqualTo(height));
    }

    [Test]
    public void LoadBitmap_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        const int width = 600;
        const int height = 400;

        var bitmap = new SKBitmap(width, height);

        var service = new BitmapService();

        // Act  
        service.LoadBitmap(bitmap);

        // Assert
        Assert.That(service.CurrentBitmap, Is.Not.Null);
        Assert.That(service.CurrentBitmap, Is.SameAs(bitmap));
        Assert.That(service.CurrentCanvas, Is.Not.Null);
        Assert.That(service.CurrentBitmap.Width, Is.EqualTo(width));
        Assert.That(service.CurrentBitmap.Height, Is.EqualTo(height));
    }

    [Test]
    public void LoadBitmap_FromExistingFile_PropsSetCorrectly()
    {
        // Arrange 
        var service = new BitmapService();
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        // Act  
        service.LoadBitmap(_imagePath);

        // Assert
        Assert.That(service.CurrentBitmap, Is.Not.Null);
        Assert.That(service.CurrentCanvas, Is.Not.Null);

        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void SaveAsMemoryStream_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        const int width = 600;
        const int height = 400;

        var fileName = Path.Combine(TestHelper.TestResultPath, "test.png");
        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.NewBitmap(width, height);
        service.DrawRectangle(0, 0, width, height, SKColors.Red, SKColors.Blue, 4);

        // Act  
        var ms = service.SaveAsMemoryStream();

        using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
            ms.Position = 0;
            ms.CopyTo(file);
            //var bytes = new byte[ms.Length];
            //ms.ReadExactly(bytes, 0, (int)ms.Length);
            //file.Write(bytes, 0, bytes.Length);
            ms.Close();
        }

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void SaveAsPng_ValidSetup_PngSaved()
    {
        // Arrange 
        const int width = 600;
        const int height = 400;
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.NewBitmap(width, height);
        service.DrawRectangle(0, 0, width, height, SKColors.Blue, SKColors.Blue, 4);

        // Act  
        service.SaveAsPng(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void SaveAsJpeg_ValidSetup_JpegSaved()
    {
        // Arrange 
        const int width = 600;
        const int height = 400;
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.NewBitmap(width, height);
        service.DrawRectangle(0, 0, width, height, SKColors.Blue, SKColors.Blue, 4);

        // Act  
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void ApplyPaddingToImage_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.ApplyPaddingToImage(SKColors.Aqua);
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void Grayscale_Black_ReturnsInvertedColorWhite()
    {
        // Arrange 
        var color = SKColors.Blue;
        short volume = 25;

        // Act  
        var result = BitmapService.Grayscale((uint)color, volume);

        // Assert
        Assert.That(result, Is.Not.EqualTo(SKColors.Blue));
    }

    [Test]
    public void InvertArgbColor_Black_ReturnsInvertedColorWhite()
    {
        // Arrange 
        var color = SKColors.Black;

        // Act  
        var result = BitmapService.ToInvertedArgbColor((uint)color);

        // Assert
        Assert.That(result, Is.EqualTo(SKColors.White));
    }

    [Test]
    public void InvertColors_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.ToImageWithInvertedColors();
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void GreyScale_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.ToGreyscaleImage();
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void ConvertBlackAndWhite2_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.ToBlackAndWhiteImage2();
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void ConvertBlackAndWhiteFloydSteinberg_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.ToBlackAndWhiteImageFloydSteinberg();
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void ResizeImage_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.ResizeImage(800, 600, true, (uint)SKColors.Aqua);
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void AdjustSaturation_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.AdjustSaturation(-1F);
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void AdjustBrightness_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.AdjustBrightness(-0.5F);
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void AdjustContrast_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.AdjustContrast(0.1F);
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void AdjustGamma_ValidSetup_JpegSaved()
    {
        // Arrange 
        var fileName = Path.Combine(TestHelper.TestResultPath, "test.jpg");

        if (File.Exists(fileName)) File.Delete(fileName);

        var service = new BitmapService();
        service.LoadBitmap(_imagePath);

        // Act  
        service.AdjustGamma(-99F);
        service.SaveAsJpeg(fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);

        TestHelper.StartFile(fileName);
    }
}
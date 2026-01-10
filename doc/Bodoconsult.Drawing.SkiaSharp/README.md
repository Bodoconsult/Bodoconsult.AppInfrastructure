Bodoconsult.Drawing.SkiaSharp
===========================================

# Overview

## What does Bodoconsult.Drawing.SkiaSharp library

Bodoconsult.Drawing.SkiaSharp is library to simplify bitmap handling. The main class is BitmapService. 

## How to use the library

The source code contains NUnit test classes the following source code is extracted from. The samples below show the most helpful use cases for the library.

# Manipulating bitmaps with BitmapService

BitmapService delivers easy to use basic functionality to manipulate bitmaps like drawing rectangle, rectangles with rounded corners and writing text.

Please see the interface IBitmapService below for available services provided by BitmapService class.

``` csharp
/// <summary>
/// Interface for a SkiaSharp based bitmap service handling a bitmap and its canvas
/// </summary>
public interface IBitmapService : IDisposable
{
    /// <summary>
    /// Current bitmap
    /// </summary>
    SKBitmap CurrentBitmap { get; }

    /// <summary>
    /// Current canvas
    /// </summary>
    SKCanvas CurrentCanvas { get; }

    /// <summary>
    /// Load a bitmap from a file
    /// </summary>
    /// <param name="fullPath">full file name of the source file to load</param>
    void LoadBitmap(string fullPath);

    /// <summary>
    /// Load a bitmap from a file
    /// </summary>
    /// <param name="bitmap">The bitmap to process</param>
    void LoadBitmap(SKBitmap bitmap);

    /// <summary>
    /// Create a new bitmap
    /// </summary>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    void NewBitmap(int width, int height);

    /// <summary>
    /// Save the bitmap as PNG file
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    void SaveAsPng(string fullPath);

    /// <summary>
    /// Save the bitmap as JPEG file (Quality 100)
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    void SaveAsJpeg(string fullPath);

    /// <summary>
    /// Save the bitmap as JPEG file
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    /// <param name="quality">Quality between 1 and 100</param>
    void SaveAsJpeg(string fullPath, int quality);

    /// <summary>
    /// Clear the current canvas
    /// </summary>
    /// <param name="color">Color to clear the canvas with</param>
    void Clear(SKColor color);

    /// <summary>
    /// Draw a rectangle
    /// </summary>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="right">Right position</param>
    /// <param name="bottom">Bottom position</param>
    /// <param name="fillColor">Fill color</param>
    /// <param name="borderColor">Border color</param>
    /// <param name="borderWidth">Border width. Default = 2</param>
    void DrawRectangle(int left, int top, int right, int bottom, SKColor fillColor, SKColor borderColor, int borderWidth = 2);

    /// <summary>
    /// Draw a rectangle
    /// </summary>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="right">Right position</param>
    /// <param name="bottom">Bottom position</param>
    /// <param name="fillPaint">Fill paint. Use it to add gradients</param>
    /// <param name="borderColor">Border color</param>
    /// <param name="borderWidth">Border width. Default = 2</param>
    void DrawRectangle(int left, int top, int right, int bottom, SKPaint fillPaint, SKColor borderColor, int borderWidth = 2);

    /// <summary>
    /// Draw a rectangle filled with a vertical gradient
    /// </summary>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="right">Right position</param>
    /// <param name="bottom">Bottom position</param>
    /// <param name="topColor">Top color pf the vertical gradient</param>
    /// <param name="bottomColor">Bottom color pf the vertical gradient</param>
    /// <param name="borderColor">Border color</param>
    /// <param name="borderWidth">Border width. Default = 2</param>
    void DrawRectangleWithVerticalGradient(int left, int top, int right, int bottom, SKColor topColor, SKColor bottomColor, SKColor borderColor, int borderWidth = 2);

    /// <summary>
    /// Draw a runded rectangle
    /// </summary>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="right">Right position</param>
    /// <param name="bottom">Bottom position</param>
    /// <param name="fillColor">Fill color</param>
    /// <param name="borderColor">Border color</param>
    /// <param name="radius">Corner radius</param>
    /// <param name="borderWidth">Border width. Default = 2</param>
    void DrawRoundedRectangle(int left, int top, int right, int bottom, SKColor fillColor, SKColor borderColor, int radius, int borderWidth = 2);

    /// <summary>
    /// Draw a runded rectangle
    /// </summary>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="right">Right position</param>
    /// <param name="bottom">Bottom position</param>
    /// <param name="fillPaint">Fill paint to add gradients</param>
    /// <param name="borderColor">Border color</param>
    /// <param name="radius">Corner radius</param>
    /// <param name="borderWidth">Border width. Default = 2</param>
    void DrawRoundedRectangle(int left, int top, int right, int bottom, SKPaint fillPaint, SKColor borderColor, int radius, int borderWidth = 2);

    /// <summary>
    /// Draw a rectangle filled with a vertical gradient
    /// </summary>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="right">Right position</param>
    /// <param name="bottom">Bottom position</param>
    /// <param name="topColor">Top color pf the vertical gradient</param>
    /// <param name="bottomColor">Bottom color pf the vertical gradient</param>
    /// <param name="borderColor">Border color</param>
    /// <param name="radius">Corner radius</param>
    /// <param name="borderWidth">Border width. Default = 2</param>
    void DrawRoundedRectangleWithVerticalGradient(int left, int top, int right, int bottom, SKColor topColor, SKColor bottomColor, SKColor borderColor, int radius, int borderWidth = 2);

    /// <summary>
    /// Draw a PNG image as array to the canvas
    /// </summary>
    /// <param name="bytes">Byte arry of an PNG image</param>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="right">Right position</param>
    /// <param name="bottom">Bottom position</param>
    void DrawPng(byte[] bytes, int left, int top, int right, int bottom);
    
    /// <summary>
    /// Draw a text
    /// </summary>
    /// <param name="text">Text to draw</param>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <param name="font">Current font</param>
    /// <param name="color">Current foreground color</param>
    /// <param name="alignment">Alignment relative to x and y position</param>
    void DrawText(string text, int x, int y, SKFont font, SKColor color, SKTextAlign alignment);

    /// <summary>
    /// Draw a text
    /// </summary>
    /// <param name="text">Text to draw</param>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <param name="fontName">Current font name</param>
    /// <param name="fontSize">Current font size in pt</param>
    /// <param name="color">Current foreground color</param>
    /// <param name="alignment">Alignment relative to x and y position</param>
    void DrawText(string text, int x, int y, string fontName, float fontSize, SKColor color, SKTextAlign alignment);

    #region Resize image

    /// <summary>
    /// Resize the image
    /// </summary>
    /// <param name="maxWidth">Maximum with of the new image</param>
    /// <param name="maxHeight">Maximum height of the new image</param>
    /// <param name="padImage">Make the image square with padding</param>
    /// <param name="padImageColorArgb">ARGB color value of the color used for padding</param>
    void ResizeImage(int maxWidth, int maxHeight, bool padImage = false, uint padImageColorArgb = 0);

    /// <summary>
    /// Add padding in a certain color to the image if it is not a square
    /// </summary>
    /// <param name="backColor">Background color</param>
    void ApplyPaddingToImage(SKColor backColor);

    #endregion

    /// <summary>
    /// Invert the image
    /// </summary>
    void ToImageWithInvertedColors();

    /// <summary>
    /// Make the image a grey scale image
    /// </summary>
    /// <exception cref="Exception"></exception>
    void ToGreyscaleImage();

    /// <summary>
    /// Convert to black and white image (version 2)
    /// </summary>
    void ToBlackAndWhiteImage2();

    /// <summary>
    /// Convert to black and white image (version Floyd Steinberg)
    /// </summary>
    void ToBlackAndWhiteImageFloydSteinberg();

    /// <summary>
    /// Adjusts the contrast of an image. amount is the adjustment level. Negative values decrease contrast, positive values increase contrast, and 0 means no change.
    /// </summary>
    /// <param name="amount">Amout to reduce the contrast</param>
    void AdjustContrast(float amount);

    /// <summary>
    /// Adjust the brightness of an image.
    /// </summary>
    /// <param name="brightness">Brightness factor from -1F til +1F. -1F means complement colors. Around 0.5F means a faded image.</param>
    void AdjustBrightness(float brightness);

    /// <summary>
    /// Adjust the saturation of an image.
    /// </summary>
    /// <param name="saturation">Satuation factor from -1F til +1F. -1F means complement colors. Around 0.5F means a faded image.</param>
    void AdjustSaturation(float saturation);

    /// <summary>
    /// Adjust the gamma for the bitmap
    /// </summary>
    /// <param name="gamma">Gamma factor to use</param>
    void AdjustGamma(float gamma);
}
```

The following test code shows to create a bitmap, draw in it and then save it as PNG or JPEG file.

``` csharp
[Test]
public void SaveAsPng_ValidSetup_PngSaved()
{
    // Arrange 
    var width = 600;
    var height = 400;
    var fileName = Path.Combine(TestHelper.TestResultPath, $"test.png");

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
    var width = 600;
    var height = 400;
    var fileName = Path.Combine(TestHelper.TestResultPath, $"test.jpg");

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
```

If you need direct access to the canvas of the bitmap you can use BitmapService.CurrentCanvas which is loaded automatically when the image is created or loaded.

# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.
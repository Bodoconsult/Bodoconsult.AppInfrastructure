// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using SkiaSharp;

namespace Bodoconsult.Drawing.SkiaSharp.Services;

/// <summary>
/// Current implementation of <see cref="IBitmapService"/>
/// </summary>
public class BitmapService : IBitmapService
{
    private string _fullPath;

    /// <summary>
    /// Current bitmap
    /// </summary>
    public SKBitmap CurrentBitmap { get; private set; }

    /// <summary>
    /// Current canvas
    /// </summary>
    public SKCanvas CurrentCanvas { get; private set; }

    /// <summary>
    /// Load a bitmap from a file
    /// </summary>
    /// <param name="fullPath">full file name of the source file to load</param>
    public void LoadBitmap(string fullPath)
    {
        _fullPath = fullPath;

        var image = SKImage.FromEncodedData(fullPath);
        CurrentBitmap = SKBitmap.FromImage(image);
        image.Dispose();
        CurrentCanvas = new SKCanvas(CurrentBitmap);
    }

    /// <summary>
    /// Load a bitmap from a file
    /// </summary>
    /// <param name="bitmap">The bitmap to process</param>
    public void LoadBitmap(SKBitmap bitmap)
    {
        CurrentBitmap = bitmap;
        CurrentCanvas = new SKCanvas(CurrentBitmap);
    }

    /// <summary>
    /// Create a new bitmap
    /// </summary>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    public void NewBitmap(int width, int height)
    {
        CurrentBitmap = new SKBitmap(width, height);
        CurrentCanvas = new SKCanvas(CurrentBitmap);
        CurrentCanvas.Clear(SKColors.Transparent);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        CurrentCanvas.Dispose();
        CurrentBitmap.Dispose();
    }


    /// <summary>
    /// Save the bitmap as PNG file
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    public void SaveAsPng(string fullPath)
    {
        using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        using var image = SKImage.FromBitmap(CurrentBitmap);
        using var encodedImage = image.Encode();
        encodedImage.SaveTo(stream);
    }

    /// <summary>
    /// Save the bitmap as JPEG file (Quality 100)
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    public void SaveAsJpeg(string fullPath)
    {
        SaveAsJpeg(fullPath, 100);
    }

    /// <summary>
    /// Save the bitmap as JPEG file
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    /// <param name="quality">Quality between 1 and 100</param>
    public void SaveAsJpeg(string fullPath, int quality)
    {
        if (quality is < 1 or > 100)
        {
            quality = 100;  // no compression
        }

        using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        using var image = SKImage.FromBitmap(CurrentBitmap);
        using var encodedImage = image.Encode(SKEncodedImageFormat.Jpeg, quality);
        encodedImage.SaveTo(stream);
    }

    /// <summary>
    /// Clear the current canvas
    /// </summary>
    /// <param name="color">Color to clear the canvas with</param>
    public void Clear(SKColor color)
    {
        CurrentCanvas.Clear(color);
    }


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
    public void DrawRectangle(int left, int top, int right, int bottom, SKColor fillColor, SKColor borderColor, int borderWidth = 2)
    {
        using var paint = new SKPaint();
        paint.Color = fillColor;
        paint.Style = SKPaintStyle.Fill;

        var rect = new SKRect(left, top, right, bottom);

        CurrentCanvas.DrawRect(rect, paint);

        using var paint2 = new SKPaint();
        paint2.Color = borderColor;
        paint2.StrokeWidth = borderWidth;
        paint2.Style = SKPaintStyle.Stroke;

        CurrentCanvas.DrawRect(rect, paint2);
    }

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
    public void DrawRectangle(int left, int top, int right, int bottom, SKPaint fillPaint, SKColor borderColor, int borderWidth = 2)
    {
        var rect = new SKRect(left, top, right, bottom);

        CurrentCanvas.DrawRect(rect, fillPaint);

        using var paint2 = new SKPaint();
        paint2.Color = borderColor;
        paint2.StrokeWidth = borderWidth;
        paint2.Style = SKPaintStyle.Stroke;

        CurrentCanvas.DrawRect(rect, paint2);
    }

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
    public void DrawRectangleWithVerticalGradient(int left, int top, int right, int bottom, SKColor topColor, SKColor bottomColor, SKColor borderColor, int borderWidth = 2)
    {
        using var paint = new SKPaint();
        paint.Shader = SKShader.CreateLinearGradient(
            new SKPoint(left, top),
            new SKPoint(left, bottom),
            [topColor, bottomColor],
            [0, 1],
            SKShaderTileMode.Repeat);
        DrawRectangle(left, top, right, bottom, paint, borderColor, borderWidth);
    }

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
    public void DrawRoundedRectangle(int left, int top, int right, int bottom, SKColor fillColor, SKColor borderColor, int radius, int borderWidth = 2)
    {
        using var paint = new SKPaint();
        paint.Color = fillColor;
        paint.Style = SKPaintStyle.Fill;

        using var roundedRect = new SKRoundRect(new SKRect(left, top, right, bottom), radius);

        CurrentCanvas.DrawRoundRect(roundedRect, paint);

        using var paint2 = new SKPaint();
        paint2.Color = borderColor;
        paint2.StrokeWidth = borderWidth;
        paint2.Style = SKPaintStyle.Stroke;

        CurrentCanvas.DrawRoundRect(roundedRect, paint2);
    }

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
    public void DrawRoundedRectangle(int left, int top, int right, int bottom, SKPaint fillPaint, SKColor borderColor, int radius, int borderWidth = 2)
    {
        using var roundedRect = new SKRoundRect(new SKRect(left, top, right, bottom), radius);

        CurrentCanvas.DrawRoundRect(roundedRect, fillPaint);

        using var paint2 = new SKPaint();
        paint2.Color = borderColor;
        paint2.StrokeWidth = borderWidth;
        paint2.Style = SKPaintStyle.Stroke;

        CurrentCanvas.DrawRoundRect(roundedRect, paint2);
    }


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
    public void DrawRoundedRectangleWithVerticalGradient(int left, int top, int right, int bottom, SKColor topColor, SKColor bottomColor, SKColor borderColor, int radius, int borderWidth = 2)
    {
        using var paint = new SKPaint();
        paint.Shader = SKShader.CreateLinearGradient(
            new SKPoint(left, top),
            new SKPoint(left, bottom),
            [topColor, bottomColor],
            [0, 1],
            SKShaderTileMode.Repeat);
        DrawRoundedRectangle(left, top, right, bottom, paint, borderColor, radius, borderWidth);
    }

    /// <summary>
    /// Draw a PNG image as array to the canvas
    /// </summary>
    /// <param name="bytes">Byte arry of an PNG image</param>
    /// <param name="left">Left position</param>
    /// <param name="top">Top position</param>
    /// <param name="right">Right position</param>
    /// <param name="bottom">Bottom position</param>
    public void DrawPng(byte[] bytes, int left, int top, int right, int bottom)
    {
        using var b = SKBitmap.Decode(bytes);
        CurrentCanvas.DrawBitmap(b, new SKRect(left, top, right, bottom));
    }

    /// <summary>
    /// Draw a text
    /// </summary>
    /// <param name="text">Text to draw</param>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <param name="font">Current font</param>
    /// <param name="color">Current foreground color</param>
    /// <param name="alignment">Alignment relative to x and y position</param>
    public void DrawText(string text, int x, int y, SKFont font, SKColor color, SKTextAlign alignment)
    {
        var yKorr = y - font.Size / 2;
        var xKorr = x;

        // draw left-aligned text, solid
        using var paint = new SKPaint();
        paint.IsAntialias = true;
        paint.Color = color;
        paint.IsStroke = false;

        xKorr = alignment switch
        {
            SKTextAlign.Center => x - (int)(font.MeasureText(text, paint) / 2),
            SKTextAlign.Right => x - (int)font.MeasureText(text, paint),
            _ => xKorr
        };

        CurrentCanvas.DrawText(text, xKorr, yKorr, font, paint);
    }

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
    public void DrawText(string text, int x, int y, string fontName, float fontSize, SKColor color, SKTextAlign alignment)
    {
        var typeFace = SKTypeface.FromFamilyName(fontName);

        if (fontSize < 0)
        {
            fontSize = 12;
        }

        var font = new SKFont();
        font.Size = fontSize;
        font.Typeface = typeFace;
        DrawText(text, x, y, font, color, alignment);
    }

    /// <summary>
    /// Draw a square on the canvas
    /// </summary>
    /// <param name="squareSize">Square size</param>
    /// <param name="color">Color to use for square</param>
    /// <param name="startX">Start x-coordinate</param>
    /// <param name="startY">Start y-coordinate</param>
    /// <exception cref="ArgumentException">Thrown if startX or startY or squareSize are lower or equal to 0</exception>
    public void DrawSquareOnImage(int squareSize, SKColor color, int startX, int startY)
    {
        if (squareSize <= 0 || startX <= 0 || startY <= 0)
        {
            throw new ArgumentException("Square size and coordinates must be greater than zero.");
        }

        using var paint = new SKPaint();
        paint.Color = color;
        var square = new SKRect(startX, startY, startX + squareSize, startY + squareSize);
        CurrentCanvas.DrawRect(square, paint);
    }

    /// <summary>
    /// Draw a square on the canvas
    /// </summary>
    /// <param name="squareSize">Square size</param>
    /// <param name="paint">Paint to use for square</param>
    /// <param name="startX">Start x-coordinate</param>
    /// <param name="startY">Start y-coordinate</param>
    /// <exception cref="ArgumentException">Thrown if startX or startY or squareSize are lower or equal to 0</exception>
    public void DrawSquareOnImage(int squareSize, SKPaint paint, int startX, int startY)
    {
        if (squareSize <= 0 || startX <= 0 || startY <= 0)
        {
            throw new ArgumentException("Square size and coordinates must be greater than zero.");
        }

        var square = new SKRect(startX, startY, startX + squareSize, startY + squareSize);
        CurrentCanvas.DrawRect(square, paint);
    }

    #region Resize image

    /// <summary>
    /// Resize the image
    /// </summary>
    /// <param name="maxWidth">Maximum with of the new image</param>
    /// <param name="maxHeight">Maximum height of the new image</param>
    /// <param name="padImage">Make the image square with padding</param>
    /// <param name="padImageColorArgb">ARGB color value of the color used for padding</param>
    public void ResizeImage(int maxWidth, int maxHeight, bool padImage = false, uint padImageColorArgb = 0)
    {
        int newWidth;
        int newHeight;

        ////first we check if the image needs rotating (eg phone held vertical when taking a picture for example)
        //foreach (var prop in CurrentBitmap.PropertyItems)
        //{
        //    if (prop.Id != 0x0112)
        //    {
        //        continue;
        //    }
        //    int orientationValue = CurrentBitmap.GetPropertyItem(prop.Id).Value[0];
        //    var rotateFlipType = GetRotateFlipType(orientationValue);
        //    CurrentBitmap.RotateFlip(rotateFlipType);
        //    break;
        //}


        //check if the with or height of the image exceeds the maximum specified, if so calculate the new dimensions
        if (CurrentBitmap.Width > maxWidth || CurrentBitmap.Height > maxHeight)
        {
            var ratioX = (double)maxWidth / CurrentBitmap.Width;
            var ratioY = (double)maxHeight / CurrentBitmap.Height;
            var ratio = Math.Min(ratioX, ratioY);

            newWidth = (int)(CurrentBitmap.Width * ratio);
            newHeight = (int)(CurrentBitmap.Height * ratio);
        }
        else
        {
            newWidth = CurrentBitmap.Width;
            newHeight = CurrentBitmap.Height;
        }

        //apply the padding to make a square image
        if (padImage)
        {
            ApplyPaddingToImage(new SKColor(padImageColorArgb));
        }

        //start the resizing
        var newImage = CurrentBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKSamplingOptions.Default);
        var canvas = new SKCanvas(newImage);

        CurrentBitmap.Dispose();
        CurrentBitmap = newImage;
        CurrentCanvas = canvas;
    }

    /// <summary>
    /// Add padding in a certain color to the image
    /// </summary>
    /// <param name="backColor">Background color</param>
    public void ApplyPaddingToImage(SKColor backColor)
    {
        //get the maximum size of the image dimensions
        var maxSize = Math.Max(CurrentBitmap.Height, CurrentBitmap.Width);

        //create a new square image
        var squareImage = new SKBitmap(maxSize, maxSize);

        var canvas = new SKCanvas(squareImage);

        //fill the new square with a color
        var rect = new SKRect(0, 0, maxSize, maxSize);
        using var paint = new SKPaint();
        paint.Color = backColor;
        paint.Style = SKPaintStyle.Fill;

        canvas.DrawRect(rect, paint);

        using var paint2 = new SKPaint();
        paint2.Color = backColor;
        paint2.StrokeWidth = 1;
        paint2.Style = SKPaintStyle.Stroke;

        canvas.DrawRect(rect, paint2);

        //put the original image on top of the new square
        var left = (maxSize - CurrentBitmap.Width) / 2f;
        var top = (maxSize - CurrentBitmap.Height) / 2f;

        rect = new SKRect(left, top, left + CurrentBitmap.Width, top + CurrentBitmap.Height);
        canvas.DrawBitmap(CurrentBitmap, rect);

        //return the image
        CurrentBitmap.Dispose();
        CurrentBitmap = squareImage;
        CurrentCanvas = canvas;
    }

    #endregion

    /// <summary>
    /// Invert the image
    /// </summary>
    public void ToImageWithInvertedColors()
    {
        using var paint = new SKPaint();
        paint.BlendMode = SKBlendMode.Src;

        for (var x = 0; x < CurrentBitmap.Width; x++)
        {
            for (var y = 0; y < CurrentBitmap.Height; y++)
            {
                var c = CurrentBitmap.GetPixel(x, y);
                if (c.Alpha <= 0)
                {
                    continue;
                }
                paint.Color = ToInvertedArgbColor((uint)c);
                CurrentCanvas.DrawPoint(x, y, paint);
            }
        }

    }

    /// <summary>
    /// Make the image a grey scale image
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void ToGreyscaleImage()
    {
        try
        {
            // https://github.com/mono/SkiaSharp/issues/2405

            using var paint = new SKPaint();

            // Define a grayscale color filter to apply to the image
            paint.ColorFilter = SKColorFilter.CreateColorMatrix(new[]
            {
                0.2126f, 0.7152f, 0.0722f, 0, 0,  // red channel weights
                0.2126f, 0.7152f, 0.0722f, 0, 0,  // green channel weights
                0.2126f, 0.7152f, 0.0722f, 0, 0,  // blue channel weights
                0,       0,       0,       1, 0   // alpha channel weights
            });

            // redraw the image using the color filter
            CurrentCanvas.DrawBitmap(CurrentBitmap, 0, 0, paint);

            //// ToDo :check value
            //const short volume = 100;

            //for (var x = 0; x < CurrentBitmap.Width; x++)
            //{
            //    for (var y = 0; y < CurrentBitmap.Height; y++)
            //    {
            //        var c = CurrentBitmap.GetPixel(x, y);
            //        if (c.Alpha <= 0)
            //        {
            //            continue;
            //        }

            //        var newColor = Grayscale(c, volume);
            //        CurrentBitmap.SetPixel(x, y, newColor);
            //    }
            //}
        }
        catch (Exception ex)
        {
            var msg = $"Core.GreyScale:Error:{_fullPath}:";
            Trace.TraceError(msg + ex.Message);
            throw new Exception(msg, ex);
        }
    }

    /// <summary>
    /// Change the RGB color to the Grayscale version
    /// </summary>
    /// <param name="color">The source color</param>
    /// <param name="volume">Gray scale volume between -255 - 255</param>
    /// <returns></returns>
    public static SKColor Grayscale(SKColor color, short volume = 0)
    {
        if (volume == 0)
        {
            return color;
        }
        var r = color.Red;
        var g = color.Green;
        var b = color.Blue;
        var mean = (r + g + b) / 3F;
        var n = volume / 255F;
        var o = 1 - n;
        return new SKColor(Convert.ToByte(r * o + mean * n), Convert.ToByte(g * o + mean * n), Convert.ToByte(b * o + mean * n), color.Alpha);
    }

    /// <summary>
    /// Convert to black and white image (version 2)
    /// </summary>
    public void ToBlackAndWhiteImage2()
    {
        using var paint = new SKPaint();
        paint.BlendMode = SKBlendMode.Src;

        for (var x = 0; x < CurrentBitmap.Width; x++)
        {
            for (var y = 0; y < CurrentBitmap.Height; y++)
            {
                var c = CurrentBitmap.GetPixel(x, y);
                if (c.Alpha <= 0)
                {
                    continue;
                }
                paint.Color = ToBlackAndWhiteColor(c);
                CurrentCanvas.DrawPoint(x, y, paint);
            }
        }

    }

    /// <summary>
    /// Convert to black and white image (version Floyd Steinberg)
    /// </summary>
    public void ToBlackAndWhiteImageFloydSteinberg()
    {
        using var paint = new SKPaint();
        paint.BlendMode = SKBlendMode.Src;

        for (var x = 0; x < CurrentBitmap.Width; x++)
        {
            for (var y = 0; y < CurrentBitmap.Height; y++)
            {
                var oldColor = CurrentBitmap.GetPixel(x, y);
                if (oldColor.Alpha <= 0)
                {
                    continue;
                }

                var newColor = ToBlackAndWhiteColor(oldColor);
                ApplyFloydSteinbergAlgo(oldColor, newColor, x, y);
                paint.Color = newColor;
                CurrentCanvas.DrawPoint(x, y, paint);
            }
        }
    }

    private void ApplyFloydSteinbergAlgo(SKColor oldColor, SKColor newColor, int x, int y)
    {

        //return newColor;

        using var paint = new SKPaint();
        paint.BlendMode = SKBlendMode.Src;

        SKColor offsetPixel;

        var redError = oldColor.Red - newColor.Red;
        var blueError = oldColor.Green - newColor.Green;
        var greenError = oldColor.Blue - newColor.Blue;

        if (x + 1 < CurrentBitmap.Width)
        {
            // right
            offsetPixel = CurrentBitmap.GetPixel(x + 1, y);

            offsetPixel = new SKColor((byte)(offsetPixel.Red + ((redError * 7) >> 4)),
                (byte)(offsetPixel.Green + ((greenError * 7) >> 4)), (byte)(offsetPixel.Blue + ((blueError * 7) >> 4)), offsetPixel.Alpha);
            paint.Color = offsetPixel;
            CurrentCanvas.DrawPoint(x + 1, y, paint);
        }

        if (y + 1 >= CurrentBitmap.Height)
        {
            return;
        }

        if (x - 1 > 0)
        {
            // left and down
            offsetPixel = CurrentBitmap.GetPixel(x - 1, y + 1);
            offsetPixel = new SKColor((byte)(offsetPixel.Red + ((redError * 3) >> 4)),
                (byte)(offsetPixel.Green + ((greenError * 3) >> 4)), (byte)(offsetPixel.Blue + ((blueError * 3) >> 4)), offsetPixel.Alpha);
            paint.Color = offsetPixel;
            CurrentCanvas.DrawPoint(x - 1, y + 1, paint);
        }

        // down
        offsetPixel = CurrentBitmap.GetPixel(x, y + 1);
        offsetPixel = new SKColor((byte)(offsetPixel.Red + ((redError * 5) >> 4)),
            (byte)(offsetPixel.Green + ((greenError * 5) >> 4)),
            (byte)(offsetPixel.Blue + ((blueError * 5) >> 4)), offsetPixel.Alpha);
        paint.Color = offsetPixel;
        CurrentCanvas.DrawPoint(x, y + 1, paint);

        if (x + 1 >= CurrentBitmap.Width)
        {
            return;
        }
        // right and down
        offsetPixel = CurrentBitmap.GetPixel(x + 1, y + 1);
        offsetPixel = new SKColor((byte)(offsetPixel.Red + ((redError * 1) >> 4)),
            (byte)(offsetPixel.Green + ((greenError * 1) >> 4)), (byte)(offsetPixel.Blue + ((blueError * 1) >> 4)), offsetPixel.Alpha);
        paint.Color = offsetPixel;
        CurrentCanvas.DrawPoint(x + 1, y + 1, paint);
    }

    /// <summary>
    /// Adjust the saturation of an image.
    /// </summary>
    /// <param name="saturation">Satuation factor from -1F til +1F. -1F means complement colors. Around 0.5F means a faded image.</param>
    public void AdjustSaturation(float saturation)
    {

        using var paint = new SKPaint();
        paint.BlendMode = SKBlendMode.Src;

        for (var x = 0; x < CurrentBitmap.Width; x++)
        {
            for (var y = 0; y < CurrentBitmap.Height; y++)
            {
                var c = CurrentBitmap.GetPixel(x, y);
                if (c.Alpha <= 0)
                {
                    continue;
                }

                c.ToHsl(out var h, out var s, out var l);

                paint.Color = SKColor.FromHsl(h, s + saturation * s, l);
                CurrentCanvas.DrawPoint(x, y, paint);
            }
        }
    }

    /// <summary>
    /// Adjust the brightness of an image.
    /// </summary>
    /// <param name="brightness">Brightness factor from -1F til +1F. -1F means complement colors. Around 0.5F means a faded image.</param>
    public void AdjustBrightness(float brightness)
    {

        using var paint = new SKPaint();
        paint.BlendMode = SKBlendMode.Src;

        for (var x = 0; x < CurrentBitmap.Width; x++)
        {
            for (var y = 0; y < CurrentBitmap.Height; y++)
            {
                var c = CurrentBitmap.GetPixel(x, y);
                if (c.Alpha <= 0)
                {
                    continue;
                }

                c.ToHsv(out var h, out var s, out var b);

                paint.Color = SKColor.FromHsl(h, s, b + b * brightness);
                CurrentCanvas.DrawPoint(x, y, paint);
            }
        }
    }

    /// <summary>
    /// Adjust the gamma for the bitmap
    /// </summary>
    /// <param name="gamma">Gamma factor to use</param>
    public void AdjustGamma(float gamma)
    {
        using var paint = new SKPaint();
        paint.BlendMode = SKBlendMode.Src;

        // Define a gamma filter
        paint.MaskFilter = SKMaskFilter.CreateGamma(gamma);

        // ToDo: check if gamma is applied correctly

        // redraw the image using the color filter
        CurrentCanvas.DrawBitmap(CurrentBitmap, 0, 0, paint);
    }

    /// <summary>
    ///  Save the bitmap as <see cref="MemoryStream"/>
    /// </summary>
    /// <returns>Open <see cref="MemoryStream"/> instance</returns>
    public MemoryStream SaveAsMemoryStream()
    {
        var ms = new MemoryStream();
        CurrentBitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
        ms.Position = 0;
        return ms;
    }

    /// <summary>
    /// Adjusts the contrast of an image. amount is the adjustment level. Negative values decrease contrast, positive values increase contrast, and 0 means no change.
    /// </summary>
    /// <param name="amount">Amout to reduce the contrast</param>
    public void AdjustContrast(float amount)
    {
        try
        {
            if (Math.Abs(amount) < 0.0000000000001)
            {
                return;
            }

            using var paint = new SKPaint();

            // Define a grayscale color filter to apply to the image
            paint.ColorFilter = ContrastFilter(amount);

            // redraw the image using the color filter
            CurrentCanvas.DrawBitmap(CurrentBitmap, 0, 0, paint);
        }
        catch (Exception ex)
        {
            var msg = $"Core.GreyScale:Error:{_fullPath}:";
            Trace.TraceError(msg + ex.Message);
            throw new Exception(msg, ex);
        }
    }

    /// <summary>
    /// Convert color to black or white color
    /// </summary>
    /// <param name="color">Current color</param>
    /// <returns>Black or white color</returns>
    public static SKColor ToBlackAndWhiteColor(SKColor color)
    {
        var gray = (byte)(0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue);

        //return gray < 128 ? new SKColor(color.Alpha, 0, 0, 0) : new SKColor(color.Alpha, 255, 255, 255);
        return gray < 128 ? SKColors.Black : SKColors.White;
    }

    /// <summary>
    /// Invert the RGB color
    /// </summary>
    /// <param name="argbColor">ARGB color value</param>
    /// <remarks>https://stackoverflow.com/questions/1165107/how-do-i-invert-a-colour</remarks>
    /// <returns>Inverted color</returns>
    public static SKColor ToInvertedArgbColor(uint argbColor)
    {
        return new SKColor(argbColor ^ 0xFFFFFF);
    }

    // https://stackoverflow.com/questions/76831547/apply-contrast-to-images-using-c-sharp-and-skiasharp

    /// <summary>
    /// Adjusts the contrast of an image. amount is the adjustment level. Negative values decrease contrast, positive values increase contrast, and 0 means no change.
    /// </summary>
    /// <param name="amount">Amout to reduce the contrast</param>
    /// <returns></returns>
    private static SKColorFilter ContrastFilter(float amount)
    {
        if (amount is < -1 or > 1)
        {
            amount = 0;
        }

        var translatedContrast = amount + 1;
        var averageLuminance = 0.2f * (1 - amount);

        return SKColorFilter.CreateColorMatrix(new[]
        {
            translatedContrast, 0, 0, 0, averageLuminance,
            0, translatedContrast, 0, 0, averageLuminance,
            0, 0, translatedContrast, 0, averageLuminance,
            0, 0, 0, 1, 0
        });
    }


    // https://medium.com/@robert.cichielo/dithering-color-images-with-skiasharp-69de53207f8d

    ///// <summary>
    ///// Converts the bitmap image to a bi-tonal (black/white) image using a simple threshold or
    ///// dither based on the Bayer 16x16 matrix. The resulting buffer returned is a bi-tonal image
    ///// buffer with the same width and height of the original image.
    ///// </summary>
    //public static unsafe byte[] BitonalFromBitmap(SKBitmap bitmap, bool dither = false)
    //{
    //    // compute stride, allocate workspace
    //    var stride = (bitmap.Width + 7) / 8;
    //    var buffer = new byte[stride * bitmap.Height];

    //    // get pointer to image pixels
    //    var src = (byte*)bitmap.GetPixels().ToPointer();

    //    // process all image rows
    //    for (var y = 0; y < bitmap.Height; y++)
    //    {
    //        var dst = y * stride;
    //        byte mask = 0x80;
    //        byte b = 0;

    //        // process raster line pixels
    //        var p = src;
    //        for (var x = 0; x < bitmap.Width; x++)
    //        {
    //            // compute pixel average
    //            var c = (*p + *(p + 1) + *(p + 2)) / 3;
    //            p += 4;

    //            // dither or threshold
    //            var t = dither ? Matrix16X16[y & 0x0f, x & 0x0f] : 128;
    //            if (c < t)
    //                b |= mask;

    //            // adjust output mask
    //            if ((mask >>= 1) == 0)
    //            {
    //                buffer[dst++] = b;
    //                mask = 0x80;
    //                b = 0;
    //            }
    //        }

    //        // flush remaining byte
    //        if (mask != 0x80)
    //            buffer[dst] = b;

    //        // point to next row
    //        src += bitmap.RowBytes;
    //    }

    //    // return bi-tonal image buffer
    //    return buffer;
    //}

    //private static readonly int[,] Matrix16X16 =
    //{
    //    { 0, 191, 48, 239, 12, 203, 60, 251, 3, 194, 51, 242, 15, 206, 63, 254 },
    //    { 127, 64, 175, 112, 139, 76, 187, 124, 130, 67, 178, 115, 142, 79, 190, 127 },
    //    { 32, 223, 16, 207, 44, 235, 28, 219, 35, 226, 19, 210, 47, 238, 31, 222 },
    //    { 159, 96, 143, 80, 171, 108, 155, 92, 162, 99, 146, 83, 174, 111, 158, 95 },
    //    { 8, 199, 56, 247, 4, 195, 52, 243, 11, 202, 59, 250, 7, 198, 55, 246 },
    //    { 135, 72, 183, 120, 131, 68, 179, 116, 138, 75, 186, 123, 134, 71, 182, 119 },
    //    { 40, 231, 24, 215, 36, 227, 20, 211, 43, 234, 27, 218, 39, 230, 23, 214 },
    //    { 167, 104, 151, 88, 163, 100, 147, 84, 170, 107, 154, 91, 166, 103, 150, 87 },
    //    { 2, 193, 50, 241, 14, 205, 62, 253, 1, 192, 49, 240, 13, 204, 61, 252 },
    //    { 129, 66, 177, 114, 141, 78, 189, 126, 128, 65, 176, 113, 140, 77, 188, 125 },
    //    { 34, 225, 18, 209, 46, 237, 30, 221, 33, 224, 17, 208, 45, 236, 29, 220 },
    //    { 161, 98, 145, 82, 173, 110, 157, 94, 160, 97, 144, 81, 172, 109, 156, 93 },
    //    { 10, 201, 58, 249, 6, 197, 54, 245, 9, 200, 57, 248, 5, 196, 53, 244 },
    //    { 137, 74, 185, 122, 133, 70, 181, 118, 136, 73, 184, 121, 132, 69, 180, 117 },
    //    { 42, 233, 26, 217, 38, 229, 22, 213, 41, 232, 25, 216, 37, 228, 21, 212 },
    //    { 169, 106, 153, 90, 165, 102, 149, 86, 168, 105, 152, 89, 164, 101, 148, 85 }
    //};

    // real world
    //private static readonly int[,] Matrix16X16 =
    //{
    //    { 5, 191, 48, 239, 12, 203, 60, 251, 3, 194, 51, 242, 15, 206, 63, 250 },
    //    { 127, 64, 175, 112, 139, 76, 187, 124, 130, 67, 178, 115, 142, 79, 190, 127 },
    //    { 32, 223, 16, 207, 44, 235, 28, 219, 35, 226, 19, 210, 47, 238, 31, 222 },
    //    { 159, 96, 143, 80, 171, 108, 155, 92, 162, 99, 146, 83, 174, 111, 158, 95 },
    //    { 8, 199, 56, 247, 4, 195, 52, 243, 11, 202, 59, 250, 7, 198, 55, 246 },
    //    { 135, 72, 183, 120, 131, 68, 179, 116, 138, 75, 186, 123, 134, 71, 182, 119 },
    //    { 40, 231, 24, 215, 36, 227, 20, 211, 43, 234, 27, 218, 39, 230, 23, 214 },
    //    { 167, 104, 151, 88, 163, 100, 147, 84, 170, 107, 154, 91, 166, 103, 150, 87 },
    //    { 2, 193, 50, 241, 14, 205, 62, 253, 1, 192, 49, 240, 13, 204, 61, 252 },
    //    { 129, 66, 177, 114, 141, 78, 189, 126, 128, 65, 176, 113, 140, 77, 188, 125 },
    //    { 34, 225, 18, 209, 46, 237, 30, 221, 33, 224, 17, 208, 45, 236, 29, 220 },
    //    { 161, 98, 145, 82, 173, 110, 157, 94, 160, 97, 144, 81, 172, 109, 156, 93 },
    //    { 10, 201, 58, 249, 6, 197, 54, 245, 9, 200, 57, 248, 5, 196, 53, 244 },
    //    { 137, 74, 185, 122, 133, 70, 181, 118, 136, 73, 184, 121, 132, 69, 180, 117 },
    //    { 42, 233, 26, 217, 38, 229, 22, 213, 41, 232, 25, 216, 37, 228, 21, 212 },
    //    { 169, 106, 153, 90, 165, 102, 149, 86, 168, 105, 152, 89, 164, 101, 148, 85 }
    //};
}

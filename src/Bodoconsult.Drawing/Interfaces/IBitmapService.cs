// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Drawing;

namespace Bodoconsult.Drawing.Interfaces;

/// <summary>
/// Interface for service handling bitmap manipulation
/// </summary>
public interface IBitmapService: IDisposable
{
    /// <summary>
    /// Current bitmap
    /// </summary>
    Bitmap CurrentBitmap { get; }

    /// <summary>
    /// Load a bitmap from a file
    /// </summary>
    /// <param name="fullPath">full file name of the source file to load</param>
    void LoadBitmap(string fullPath);

    /// <summary>
    /// Load a bitmap from a file
    /// </summary>
    /// <param name="bitmap">The bitmap to process</param>
    void LoadBitmap(Bitmap bitmap);

    /// <summary>
    /// Create a new bitmap
    /// </summary>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    void NewBitmap(int width, int height);

    /// <summary>
    /// Create a new bitmap
    /// </summary>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    /// <param name="color">Background color</param>
    public void NewBitmap(int width, int height, Color color);

    /// <summary>
    /// Resize the image
    /// </summary>
    /// <param name="maxWidth">Maximum with of the new image</param>
    /// <param name="maxHeight">Maximum height of the new image</param>
    /// <param name="imageResolution">Target resolution for the image</param>
    /// <param name="padImage">Make the image square with padding</param>
    /// <param name="padImageColorArgb">ARGB color value of the color used for padding</param>
    void ResizeImage(int maxWidth, int maxHeight, float imageResolution = 72, bool padImage = false, int padImageColorArgb = 0);

    /// <summary>
    /// Add padding in a certain color to the image
    /// </summary>
    /// <param name="backColor"></param>
    void ApplyPaddingToImage(Color backColor);

    /// <summary>
    /// Adjust brightness, contrast and gamma
    /// </summary>
    /// <param name="brightness">Brightness factor: default 1F means not change </param>
    /// <param name="contrast">Contrast factor: default 1F means not change</param>
    /// <param name="gamma">Gamma factor: default 1F means not change</param>
    void AdjustBcg(float brightness = 1F, float contrast = 1F, float gamma = 1F);

    /// <summary>
    /// Adjust the saturation of an image.
    /// </summary>
    /// <remarks>Based on https://www.codeproject.com/Tips/78995/Image-colour-manipulation-with-ColorMatrix</remarks>
    /// <param name="saturation">Satuation factor from -1F til +1F. -1F means complement colors. Around 0.5F means a faded image.</param>
    void AdjustSaturation(float saturation);

    /// <summary>
    /// Create rectangle with rounded corners
    /// </summary>
    /// <param name="cornerRadius">Corner radius</param>
    /// <param name="backcolor">Background color</param>
    /// <param name="borderWidth">Border width</param>
    /// <param name="borderColor">Border color</param>
    /// <param name="shadow">Draw shadow?</param>
    /// <param name="shadowOffset">Offset for the shadow</param>
    void RoundCorners(int cornerRadius, Color backcolor, int borderWidth, Color borderColor, bool shadow = false, int shadowOffset = 15);

    /// <summary>
    /// Make the image a grey scale image
    /// </summary>
    /// <exception cref="Exception"></exception>
    void GreyScale();

    /// <summary>
    /// Invert the image
    /// </summary>
    void InvertColors();

    /// <summary>
    /// Converts an image to black and white
    /// </summary>
    /// <remarks>https://stackoverflow.com/questions/6155864/c-sharp-convert-image-to-complete-blackwhite</remarks>
    void ConvertBlackAndWhite();

    /// <summary>
    /// Convert color to back and white (Version 2)
    /// </summary>
    void ConvertBlackAndWhite2();

    /// <summary>
    /// Convert color to back and white (Version Floyd-Steinberg)
    /// </summary>
    void ConvertBlackAndWhiteFloydSteinberg();

    /// <summary>
    /// Draw a text left adjusted
    /// </summary>
    /// <param name="text">test to left adjust</param>
    /// <param name="font">font to use</param>
    /// <param name="brush">brush to use</param>
    /// <param name="x1">left corner x-coordinate</param>
    /// <param name="y1">left corner y-coordinate</param>
    /// <param name="x2">right corner x-coordinate</param>
    /// <param name="height">right corner y-coordinate</param>
    /// <param name="xDistance">distance on the left end from left corner x-coordinate x1</param>
    void LeftText(string text, Font font, Brush brush, float x1, float y1, float x2, float height,
        float xDistance = 1F);

    /// <summary>
    /// Draw a text right adjusted
    /// </summary>
    /// <param name="text">test to right adjust</param>
    /// <param name="font">font to use</param>
    /// <param name="brush">brush to use</param>
    /// <param name="x1">left corner x-coordinate</param>
    /// <param name="y1">left corner y-coordinate</param>
    /// <param name="x2">right corner x-coordinate</param>
    /// <param name="height">right corner y-coordinate</param>
    /// <param name="xDistance">distance on the right end from right corner x-coordinate x2</param>
    void RightText(string text, Font font, Brush brush, float x1, float y1, float x2, float height,
        float xDistance = 1F);

    /// <summary>
    /// Draw a text right adjusted
    /// </summary>
    /// <param name="text">test to draw centered</param>
    /// <param name="font">font to use</param>
    /// <param name="brush">brush to use</param>
    /// <param name="x1">left corner x-coordinate</param>
    /// <param name="y1">left corner y-coordinate</param>
    /// <param name="x2">right corner x-coordinate</param>
    /// <param name="height">height of the rectangle to draw in</param>
    void CenteredText(string text, Font font, Brush brush, float x1, float y1, float x2, float height);

    /// <summary>
    /// Draw a rectangle
    /// </summary>
    /// <param name="pen">pen to use</param>
    /// <param name="x1">left corner x-coordinate</param>
    /// <param name="y1">left corner y-coordinate</param>
    /// <param name="x2">right corner x-coordinate</param>
    /// <param name="height">height of the rectangle to draw in</param>
    void DrawRectangle(Pen pen, float x1, float y1, float x2, float height);

    /// <summary>
    /// Draw an image with a frame around it
    /// </summary>
    /// <param name="img">Image to draw</param>
    /// <param name="framePen">Pen to use for the frame around the image</param>
    /// <param name="x">left corner x-coordinate</param>
    /// <param name="y">left corner y-coordinate</param>
    /// <param name="width">Width of the image to draw</param>
    /// <param name="height">Height of the image to draw</param>
    void DrawImageWithFrame(Image img, Pen framePen, float x, float y, float width, float height);

    /// <summary>
    /// Save the bitmap as PNG file
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    void SaveAsPng(string fullPath);

    /// <summary>
    /// Save the bitmap as JPEG file
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    /// <param name="compressionLevel">Compression level from 1 to 100 with 1 lowest quality and 100 highest quality</param>
    void SaveAsJpeg(string fullPath, uint compressionLevel = 80);
}
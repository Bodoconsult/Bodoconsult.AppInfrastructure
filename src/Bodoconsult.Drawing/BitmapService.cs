// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using Bodoconsult.Drawing.Helpers;

namespace Bodoconsult.Drawing;

/// <summary>
/// Service class for working with bitmap images
/// </summary>
[SupportedOSPlatform("windows")]
public class BitmapService : IDisposable
{
    private string _fullPath;

    /// <summary>
    /// Current bitmap
    /// </summary>
    public Bitmap CurrentBitmap { get; private set; }

    /// <summary>
    /// Load a bitmap from a file
    /// </summary>
    /// <param name="fullPath">full file name of the source file to load</param>
    public void LoadBitmap(string fullPath)
    {
        _fullPath = fullPath;
        CurrentBitmap = new Bitmap(fullPath);
    }

    /// <summary>
    /// Load a bitmap from a file
    /// </summary>
    /// <param name="bitmap">The bitmap to process</param>
    public void LoadBitmap(Bitmap bitmap)
    {
        CurrentBitmap = bitmap;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        CurrentBitmap?.Dispose();
    }

    #region Resize image

    /// <summary>
    /// Resize the image
    /// </summary>
    /// <param name="maxWidth">Maximum with of the new image</param>
    /// <param name="maxHeight">Maximum height of the new image</param>
    /// <param name="imageResolution">Target resolution for the image</param>
    /// <param name="padImage">Make the image square with padding</param>
    /// <param name="padImageColorArgb">ARGB color value of the color used for padding</param>
    public void ResizeImage(int maxWidth, int maxHeight, float imageResolution = 72, bool padImage = false, int padImageColorArgb = 0)
    {
        int newWidth;
        int newHeight;

        //first we check if the image needs rotating (eg phone held vertical when taking a picture for example)
        foreach (var prop in CurrentBitmap.PropertyItems)
        {
            if (prop.Id != 0x0112)
            {
                continue;
            }
            int orientationValue = CurrentBitmap.GetPropertyItem(prop.Id).Value[0];
            var rotateFlipType = GetRotateFlipType(orientationValue);
            CurrentBitmap.RotateFlip(rotateFlipType);
            break;
        }

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
            ApplyPaddingToImage(Color.FromArgb(padImageColorArgb));
        }

        //start the resize with a new image
        var newImage = new Bitmap(newWidth, newHeight);

        //set the new resolution
        //newImage.SetResolution(imageResolution, imageResolution);

        //start the resizing
        using (var graphics = Graphics.FromImage(newImage))
        {
            //set some encoding specs
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            graphics.DrawImage(CurrentBitmap, 0, 0, newWidth, newHeight);
        }

        CurrentBitmap.Dispose();
        CurrentBitmap = newImage;

    }

    /// <summary>
    /// Add padding in a certain color to the image
    /// </summary>
    /// <param name="backColor"></param>
    public void ApplyPaddingToImage(Color backColor)
    {
        //get the maximum size of the image dimensions
        var maxSize = Math.Max(CurrentBitmap.Height, CurrentBitmap.Width);
        var squareSize = new Size(maxSize, maxSize);

        //create a new square image
        var squareImage = new Bitmap(squareSize.Width, squareSize.Height);

        using (var graphics = Graphics.FromImage(squareImage))
        {
            //fill the new square with a color
            graphics.FillRectangle(new SolidBrush(backColor), 0, 0, squareSize.Width, squareSize.Height);

            //put the original image on top of the new square
            graphics.DrawImage(CurrentBitmap, squareSize.Width / 2 - CurrentBitmap.Width / 2, squareSize.Height / 2 - CurrentBitmap.Height / 2, CurrentBitmap.Width, CurrentBitmap.Height);
        }

        //return the image
        CurrentBitmap.Dispose();
        CurrentBitmap = squareImage;
    }





    //=== determine image rotation
    private RotateFlipType GetRotateFlipType(int rotateValue)
    {
        var flipType = rotateValue switch
        {
            1 => RotateFlipType.RotateNoneFlipNone,
            2 => RotateFlipType.RotateNoneFlipX,
            3 => RotateFlipType.Rotate180FlipNone,
            4 => RotateFlipType.Rotate180FlipX,
            5 => RotateFlipType.Rotate90FlipX,
            6 => RotateFlipType.Rotate90FlipNone,
            7 => RotateFlipType.Rotate270FlipX,
            8 => RotateFlipType.Rotate270FlipNone,
            _ => RotateFlipType.RotateNoneFlipNone
        };

        return flipType;
    }


    ////== convert image to base64
    //public string ConvertImageToBase64(Image image)
    //{
    //    using (var ms = new MemoryStream())
    //    {
    //        //convert the image to byte array
    //        image.Save(ms, ImageFormat.Jpeg);
    //        var bin = ms.ToArray();

    //        //convert byte array to base64 string
    //        return Convert.ToBase64String(bin);
    //    }
    //}

    #endregion

    /// <summary>
    /// Adjust brightness, contrast and gamma
    /// </summary>
    /// <param name="brightness">Brightness factor: default 1F means not change </param>
    /// <param name="contrast">Contrast factor: default 1F means not change</param>
    /// <param name="gamma">Gamma factor: default 1F means not change</param>
    public void AdjustBcg(float brightness = 1F, float contrast = 1F, float gamma = 1F)
    {

        var adjustedImage = new Bitmap(CurrentBitmap.Width, CurrentBitmap.Height);


        var adjustedBrightness = brightness - 1.0f;


        // create matrix that will brighten and contrast the image
        float[][] ptsArray =
        [
            [contrast, 0, 0, 0, 0], // scale red
            [0, contrast, 0, 0, 0], // scale green
            [0, 0, contrast, 0, 0], // scale blue
            [0, 0, 0, 1.0f, 0], // don't scale alpha
            [adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1]
        ];

        var imageAttributes = new ImageAttributes();
        imageAttributes.ClearColorMatrix();
        imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
        using (var g = Graphics.FromImage(adjustedImage))
        {
            g.DrawImage(CurrentBitmap, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height)
                , 0, 0, CurrentBitmap.Width, CurrentBitmap.Height,
                GraphicsUnit.Pixel, imageAttributes);
        }

        CurrentBitmap.Dispose();
        CurrentBitmap = adjustedImage;
    }

    /// <summary>
    /// Adjust the saturation of an image.
    /// </summary>
    /// <remarks>Based on https://www.codeproject.com/Tips/78995/Image-colour-manipulation-with-ColorMatrix</remarks>
    /// <param name="saturation">Satuation factor from -1F til +1F. -1F means complement colors. Around 0.5F means a faded image.</param>
    public void AdjustSaturation(float saturation)
    {
        const float rWeight = 0.3086f;
        const float gWeight = 0.6094f;
        const float bWeight = 0.0820f;

        var a = (1.0f - saturation) * rWeight + saturation;
        var b = (1.0f - saturation) * rWeight;
        var c = (1.0f - saturation) * rWeight;
        var d = (1.0f - saturation) * gWeight;
        var e = (1.0f - saturation) * gWeight + saturation;
        var f = (1.0f - saturation) * gWeight;
        var g = (1.0f - saturation) * bWeight;
        var h = (1.0f - saturation) * bWeight;
        var i = (1.0f - saturation) * bWeight + saturation;

        // Create a Graphics

        var bmp = (Bitmap)CurrentBitmap.Clone();


        using (var gr = Graphics.FromImage(CurrentBitmap))
        {
            // ColorMatrix elements
            float[][] ptsArray =
            [
                [a,  b,  c,  0, 0],
                [d,  e,  f,  0, 0],
                [g,  h,  i,  0, 0],
                [0,  0,  0,  1, 0],
                [0, 0, 0, 0, 1]
            ];
            // Create ColorMatrix
            var clrMatrix = new ColorMatrix(ptsArray);
            // Create ImageAttributes
            var imgAttribs = new ImageAttributes();
            // Set color matrix
            imgAttribs.SetColorMatrix(clrMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Default);
            // Draw Image with no effects
            gr.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
            // Draw Image with image attributes
            gr.DrawImage(bmp,
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                0, 0, bmp.Width, bmp.Height,
                GraphicsUnit.Pixel, imgAttribs);
        }

        bmp.Dispose();
    }

    /// <summary>
    /// Create rectangle with rounded corners
    /// </summary>
    /// <param name="cornerRadius">Corner radius</param>
    /// <param name="backcolor">Background color</param>
    /// <param name="borderWidth">Border width</param>
    /// <param name="borderColor">Border color</param>
    /// <param name="shadow">Draw shadow?</param>
    /// <param name="shadowOffset">Offset for the shadow</param>
    public void RoundCorners(int cornerRadius, Color backcolor, int borderWidth, Color borderColor, bool shadow = false, int shadowOffset = 15)
    {
        cornerRadius *= 2;

        //cornerRadius = 0;

        //var roundedImage = new Bitmap(Bitmap.Width, Bitmap.Height);
        var roundedImage = (Bitmap)CurrentBitmap.Clone();

        GraphicsPath gp = null;

        if (cornerRadius > 0)
        {
            gp = new GraphicsPath();
            gp.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            gp.AddArc(0 + roundedImage.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            gp.AddArc(0 + roundedImage.Width - cornerRadius, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            gp.AddArc(0, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            gp.CloseFigure();
        }


        using (var g = Graphics.FromImage(roundedImage))
        {

            // Rounded corners
            g.SmoothingMode = SmoothingMode.HighQuality;

            if (cornerRadius > 0)
            {
                g.SetClip(gp);
            }


            //if (BorderWidth > 0)
            //{
            //    if (cornerRadius > 0)
            //    {
            var gp1 = new GraphicsPath();
            gp1.AddArc(1, 1, cornerRadius, cornerRadius, 180, 90);
            gp1.AddArc(1 + roundedImage.Width - cornerRadius - 2, 1, cornerRadius, cornerRadius, 270, 90);
            gp1.AddArc(1 + roundedImage.Width - cornerRadius - 2, 1 + roundedImage.Height - cornerRadius - 2, cornerRadius, cornerRadius, 0, 90);
            gp1.AddArc(1, 1 + roundedImage.Height - cornerRadius - 2, cornerRadius, cornerRadius, 90, 90);
            gp1.CloseFigure();
            g.DrawPath(new Pen(borderColor, borderWidth), gp1);
            //}
            //else
            //{
            //    var gp1 = new GraphicsPath();
            //    gp1.AddRectangle(new Rectangle(1, 1, roundedImage.Width - 2, roundedImage.Height - 4));
            //    gp1.CloseFigure();
            //    g.DrawPath(new Pen(BorderColor, BorderWidth), gp1);
            //}

            //}
        }

        var roundedImageEnd = new Bitmap(CurrentBitmap.Width, CurrentBitmap.Height);

        using (var g = Graphics.FromImage(roundedImageEnd))
        {
            g.FillRectangle(new SolidBrush(backcolor), new Rectangle(0, 0, CurrentBitmap.Width, CurrentBitmap.Height));



            if (cornerRadius > 0) g.SetClip(gp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(roundedImage, new Rectangle(0, 0, CurrentBitmap.Width, CurrentBitmap.Height),
                new Rectangle(0, 0, CurrentBitmap.Width, CurrentBitmap.Height), GraphicsUnit.Pixel);

        }

        //roundedImageEnd.MakeTransparent(Color.LightGray);
        roundedImage.Dispose();

        if (!shadow)
        {

            CurrentBitmap.Dispose();
            CurrentBitmap = roundedImageEnd;
            return;
        }

        var shadowedImageEnd = new Bitmap(roundedImageEnd.Width + shadowOffset,
            roundedImageEnd.Height + shadowOffset);
        using (var g = Graphics.FromImage(shadowedImageEnd))
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            // Fill Background
            g.FillRectangle(new SolidBrush(backcolor), new Rectangle(0, 0, shadowedImageEnd.Width, shadowedImageEnd.Height));

            var left = shadowOffset;
            var top = shadowOffset;

            var gp1 = new GraphicsPath();
            gp1.AddArc(left, top, cornerRadius, cornerRadius, 180, 90);
            gp1.AddArc(left + roundedImageEnd.Width - cornerRadius, top, cornerRadius, cornerRadius, 270, 90);
            gp1.AddArc(left + roundedImageEnd.Width - cornerRadius, top + roundedImageEnd.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            gp1.AddArc(left, top + roundedImageEnd.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            gp1.CloseFigure();


            //var brush = new PathGradientBrush(gp1)
            //{
            //    CenterPoint = new PointF(left + roundedImage.Width / 2, top + roundedImage.Height / 2),
            //    CenterColor = Color.Black,
            //    SurroundColors = new[] { Color.White }
            //};


            //g.FillPath(brush, gp1);
            //g.SetClip(gp);


            // this is where we create the shadow effect, so we will use a 
            // pathgradientbursh and assign our GraphicsPath that we created of a 
            // Rounded Rectangle
            using (var brush = new PathGradientBrush(gp1))
            {
                // set the wrapmode so that the colors will layer themselves
                // from the outer edge in
                brush.WrapMode = WrapMode.Clamp;

                // Create a color blend to manage our colors and positions and
                // since we need 3 colors set the default length to 3
                var colorBlend = new ColorBlend(3)
                {

                    // here is the important part of the shadow making process, remember
                    // the clamp mode on the colorblend object layers the colors from
                    // the outside to the center so we want our transparent color first
                    // followed by the actual shadow color. Set the shadow color to a 
                    // slightly transparent DimGray, I find that it works best.|
                    Colors =
                    [
                        Color.Transparent,
                        Color.FromArgb(180, Color.DarkGray),
                        Color.FromArgb(180, Color.DarkGray)
                    ],
                    Positions = [0f, .1f, 1f]
                };



                // our color blend will control the distance of each color layer
                // we want to set our transparent color to 0 indicating that the 
                // transparent color should be the outer most color drawn, then
                // our Dimgray color at about 10% of the distance from the edge

                // assign the color blend to the pathgradientbrush
                brush.InterpolationColors = colorBlend;

                // fill the shadow with our pathgradientbrush

                g.FillPath(brush, gp1);
            }

            if (cornerRadius > 0)
            {
                g.SetClip(gp);
            }
            // g.DrawImage(roundedImageEnd, new Rectangle(0, 0, roundedImageEnd.Width, roundedImageEnd.Height - ChartData.ChartStyle.CorrectiveFactor), new Rectangle(0, 0, roundedImageEnd.Width, roundedImage.HeightEnd - ChartData.ChartStyle.CorrectiveFactor), GraphicsUnit.Pixel);


            g.DrawImage(roundedImageEnd, new Rectangle(0, 0, roundedImageEnd.Width, roundedImageEnd.Height), new Rectangle(0, 0, roundedImageEnd.Width, roundedImageEnd.Height), GraphicsUnit.Pixel);
        }

        roundedImageEnd.Dispose();
        CurrentBitmap.Dispose();
        CurrentBitmap = shadowedImageEnd;

    }

    /// <summary>
    /// Make the image a grey scale image
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void GreyScale()
    {
        try
        {
            // ToDo :check value
            const short volume = 100;

            for (var x = 0; x < CurrentBitmap.Width; x++)
            {
                for (var y = 0; y < CurrentBitmap.Height; y++)
                {
                    var c = CurrentBitmap.GetPixel(x, y);
                    if (c.A > 0)
                    {
                        CurrentBitmap.SetPixel(x, y, Grayscale(c, volume));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var msg = $"Core.GreyScale:Error:{_fullPath}:";
            Trace.TraceError(msg + ex.Message);
            throw new Exception(msg, ex);
        }
    }

    /// <summary>
    /// Invert the image
    /// </summary>
    public void InvertColors()
    {

        for (var x = 0; x < CurrentBitmap.Width; x++)
        {
            for (var y = 0; y < CurrentBitmap.Height; y++)
            {
                var c = CurrentBitmap.GetPixel(x, y);
                if (c.A > 0)
                {
                    CurrentBitmap.SetPixel(x, y, InvertArgbColor(c.ToArgb()));
                }
            }
        }

    }


    /// <summary>
    /// Invert the RGB color
    /// </summary>
    /// <param name="argbColor">ARGB color value</param>
    /// <remarks>https://stackoverflow.com/questions/1165107/how-do-i-invert-a-colour</remarks>
    /// <returns></returns>
    public static Color InvertArgbColor(int argbColor)
    {
        return Color.FromArgb(argbColor ^ 0xFFFFFF);
    }

    /// <summary>
    /// Change the RGB color to the Grayscale version
    /// </summary>
    /// <param name="color">The source color</param>
    /// <param name="volume">Gray scale volume between -255 - 255</param>
    /// <returns></returns>
    public static Color Grayscale(Color color, short volume = 0)
    {
        if (volume == 0)
        {
            return color;
        }
        var r = color.R;
        var g = color.G;
        var b = color.B;
        var mean = (r + g + b) / 3F;
        var n = volume / 255F;
        var o = 1 - n;
        return Color.FromArgb(color.A, Convert.ToInt32(r * o + mean * n), Convert.ToInt32(g * o + mean * n), Convert.ToInt32(b * o + mean * n));
    }

    /// <summary>
    /// Converts an image to black and white
    /// </summary>
    /// <remarks>https://stackoverflow.com/questions/6155864/c-sharp-convert-image-to-complete-blackwhite</remarks>
    public void ConvertBlackAndWhite()
    {
        var colorMatrix = new ColorMatrix([
            [.3f, .3f, .3f, 0, 0],
            [.59f, .59f, .59f, 0, 0],
            [.11f, .11f, .11f, 0, 0],
            [0, 0, 0, 1, 0],
            [0, 0, 0, 0, 1]
        ]);

        var imageAttributes = new ImageAttributes();
        imageAttributes.ClearColorMatrix();
        imageAttributes.SetColorMatrix(colorMatrix);

        using (var g = Graphics.FromImage(CurrentBitmap))
        {
            // Change brightness, contrast and gamma
            g.DrawImage(CurrentBitmap, new Rectangle(0, 0, CurrentBitmap.Width, CurrentBitmap.Height)
                , 0, 0, CurrentBitmap.Width, CurrentBitmap.Height,
                GraphicsUnit.Pixel, imageAttributes);
        }

    }

    /// <summary>
    /// Convert color to back and white (Version 2)
    /// </summary>
    public void ConvertBlackAndWhite2()
    {

        for (var x = 0; x < CurrentBitmap.Width; x++)
        {
            for (var y = 0; y < CurrentBitmap.Height; y++)
            {
                var c = CurrentBitmap.GetPixel(x, y);
                if (c.A > 0)
                {
                    CurrentBitmap.SetPixel(x, y, BlackAndWhiteColor(c));
                }
            }
        }

    }

    /// <summary>
    /// Convert color to back and white (Version Floyd-Steinberg)
    /// </summary>
    public void ConvertBlackAndWhiteFloydSteinberg()
    {

        for (var x = 0; x < CurrentBitmap.Width; x++)
        {
            for (var y = 0; y < CurrentBitmap.Height; y++)
            {
                var oldColor = CurrentBitmap.GetPixel(x, y);
                if (oldColor.A <= 0)
                {
                    continue;
                }

                var newColor = BlackAndWhiteColor(oldColor);
                ApplyFloydSteinbergAlgo(oldColor, newColor, x, y);
                CurrentBitmap.SetPixel(x, y, newColor);
            }
        }

    }

    private void ApplyFloydSteinbergAlgo(Color oldColor, Color newColor, int x, int y)
    {
        Color offsetPixel;

        var redError = oldColor.R - newColor.R;
        var blueError = oldColor.G - newColor.G;
        var greenError = oldColor.B - newColor.B;

        if (x + 1 <= CurrentBitmap.Width)
        {
            // right
            offsetPixel = CurrentBitmap.GetPixel(x + 1, y);

            offsetPixel = Color.FromArgb(offsetPixel.A, offsetPixel.R + ((redError * 7) >> 4),
                offsetPixel.G + ((greenError * 7) >> 4), offsetPixel.B + ((blueError * 7) >> 4));

            CurrentBitmap.SetPixel(x + 1, y, offsetPixel);
        }

        if (y + 1 >= CurrentBitmap.Height)
        {
            return;
        }

        if (x - 1 > 0)
        {
            // left and down
            offsetPixel = CurrentBitmap.GetPixel(x - 1, y + 1);
            offsetPixel = Color.FromArgb(offsetPixel.A, offsetPixel.R + ((redError * 3) >> 4),
                offsetPixel.G + ((greenError * 3) >> 4), offsetPixel.B + ((blueError * 3) >> 4));
            CurrentBitmap.SetPixel(x - 1, y + 1, offsetPixel);
        }

        // down
        offsetPixel = CurrentBitmap.GetPixel(x, y + 1);
        offsetPixel = Color.FromArgb(offsetPixel.A, offsetPixel.R + ((redError * 5) >> 4),
            offsetPixel.G + ((greenError * 5) >> 4),
            offsetPixel.B + ((blueError * 5) >> 4));
        CurrentBitmap.SetPixel(x, y + 1, offsetPixel);

        if (x + 1 > CurrentBitmap.Width)
        {
            return;
        }
        // right and down
        offsetPixel = CurrentBitmap.GetPixel(x + 1, y + 1);
        offsetPixel = Color.FromArgb(offsetPixel.A, offsetPixel.R + ((redError * 1) >> 4),
            offsetPixel.G + ((greenError * 1) >> 4), offsetPixel.B + ((blueError * 1) >> 4));
        CurrentBitmap.SetPixel(x + 1, y + 1, offsetPixel);
    }

    private Color BlackAndWhiteColor(Color color)
    {
        var gray = (byte)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);

        return gray < 128 ? Color.FromArgb(color.A, 0, 0, 0) : Color.FromArgb(color.A, 255, 255, 255);
    }


    #region Save image

    /// <summary>
    /// Save the bitmap as PNG file
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    public void SaveAsPng(string fullPath)
    {
        ImageHelper.SaveAsPng(CurrentBitmap, fullPath);
    }

    /// <summary>
    /// Save the bitmap as JPEG file
    /// </summary>
    /// <param name="fullPath">full file name of the target file</param>
    /// <param name="compressionLevel">Compression level from 1 to 100 with 1 lowest quality and 100 highest quality</param>
    public void SaveAsJpeg(string fullPath, uint compressionLevel = 80)
    {


        ImageHelper.SaveAsJpeg(CurrentBitmap, fullPath, compressionLevel);


        ////save the image to a memorystream to apply the compression level
        //var jpgEncoder = ImageHelper.GetEncoderInfo("image/jpeg");
        //using (var ms = new MemoryStream())
        //{
        //    //var encoderParameters = new EncoderParameters(1);
        //    //encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, (byte)compressionLevel);

        //    var encoderParameters = new EncoderParameters(1)
        //    {
        //        Param = {[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compressionLevel)}
        //    };

        //    Bitmap.Save(ms, jpgEncoder, encoderParameters);

        //    //save the image as byte array here if you want the return type to be a Byte Array instead of Image
        //    //byte[] imageAsByteArray = ms.ToArray();

        //    //write to file
        //    var file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        //    ms.WriteTo(file);
        //    file.Close();
        //    ms.Close();
        //    file.Dispose();
        //    ms.Dispose();
        //}

    }

    #endregion
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Drawing;
using System.Runtime.Versioning;
using Bodoconsult.Drawing.Model;

namespace Bodoconsult.Drawing;

/// <summary>
/// Helper class for simple image manipulation
/// </summary>
[SupportedOSPlatform("windows")]
public static class ImageTools
{


    /// <summary>
    /// Get the image size from a image file
    /// </summary>
    /// <param name="fileName">Full image file name</param>
    /// <returns></returns>
    public static ImageSize GetImageSize(string fileName)
    {
        using Stream stream = File.OpenRead(fileName);
        using var sourceImage = Image.FromStream(stream, false, false);
        var x = new ImageSize
        {
            Width = sourceImage.Width,
            Height = sourceImage.Height,
        };


        return x;
    }

    /// <summary>
    /// Generate a thumb for a image file
    /// </summary>
    /// <param name="fi">File to create a thumb for</param>
    /// <param name="thumbFileName">File name for the thumb file</param>
    /// <param name="thumbWidth">Width of the thumb in pixels</param>
    /// <param name="thumbHeight">Height of the thumb in pixels</param>
    public static void GenerateThumb(FileSystemInfo fi, string thumbFileName, int thumbWidth, int thumbHeight)
    {
        try
        {


            if (new FileInfo(thumbFileName).Exists)
            {
                return;
            }

            var service = new BitmapService();
            service.LoadBitmap(fi.FullName);


            service.ResizeImage(thumbWidth, thumbHeight);

            switch (fi.Extension.ToLower())
            {
                case ".png":
                    service.SaveAsPng(thumbFileName);
                    break;
                default:
                    service.SaveAsJpeg(thumbFileName);
                    break;
            }

            service.Dispose();

        }
        catch (Exception ex)
        {
            throw new Exception($"{fi.FullName}", ex);
        }
    }

    /// <summary>
    /// Generate image for web with reduced resolution and size
    /// </summary>
    /// <param name="fi">File to resize</param>
    /// <param name="maxImageSize">Maximum file size allowed</param>
    /// <param name="targetWidth">Target width of the image</param>
    /// <param name="targetHeight">Target height of the image</param>
    /// <returns></returns>
    public static ImageSize GenerateWebImage(FileInfo fi, long maxImageSize, int targetWidth, int targetHeight)
    {
        var imageSize = new ImageSize { Width = -1, Height = -1 };

        try
        {

            var service = new BitmapService();
            service.LoadBitmap(fi.FullName);

            if (fi.Length <= maxImageSize)
            {
                imageSize.Width = service.CurrentBitmap.Width;
                imageSize.Height = service.CurrentBitmap.Height;
                return imageSize;
            }

            service.ResizeImage(targetWidth, targetHeight);


            switch (fi.Extension.ToLower())
            {
                case ".png":
                    service.SaveAsPng(fi.FullName);
                    break;
                default:
                    service.SaveAsJpeg(fi.FullName);
                    break;
            }

            imageSize.Width = service.CurrentBitmap.Width;
            imageSize.Height = service.CurrentBitmap.Height;
                
            service.Dispose();


            return imageSize;
        }
        catch (Exception ex)
        {
            throw new Exception($"{fi.FullName}", ex);
        }
    }

}
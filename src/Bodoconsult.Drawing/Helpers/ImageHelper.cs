// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace Bodoconsult.Drawing.Helpers;

/// <summary>
/// Helper class for image and bitmap handling
/// </summary>
[SupportedOSPlatform("windows")]
public static class ImageHelper
{

    /// <summary>
    /// Get encoder info for a MIME type
    /// </summary>
    /// <param name="mimeType">MIME type</param>
    /// <returns></returns>
    public static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        var encoders = ImageCodecInfo.GetImageEncoders();

        for (var j = 0; j < encoders.Length; ++j)
        {
            //if (encoders[j].MimeType.ToLower() == mimeType.ToLower())
            if (string.Equals(encoders[j].MimeType, mimeType, StringComparison.InvariantCultureIgnoreCase))
            {
                return encoders[j];
            }
        }

        return null;
    }


    /// <summary>
    /// Save the bitmap as JPEG file
    /// </summary>
    /// <param name="bitmap">Bitmap to save as JPEG file</param>
    /// <param name="fullPath">full file name of the target file</param>
    /// <param name="compressionLevel">Compression level from 1 to 100 with 1 lowest quality and 100 highest quality</param>
    public static void SaveAsJpeg(Bitmap bitmap, string fullPath, uint compressionLevel = 80)
    {

        //save the image to a memorystream to apply the compression level
        var jpgEncoder = GetEncoderInfo("image/jpeg");
        using var ms = new MemoryStream();

        //var encoderParameters = new EncoderParameters(1);
        //encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, (byte)compressionLevel);

        var encoderParameters = new EncoderParameters(1)
        {
            Param = { [0] = new EncoderParameter(Encoder.Quality, compressionLevel) }
        };

        bitmap.Save(ms, jpgEncoder, encoderParameters);

        //save the image as byte array here if you want the return type to be a Byte Array instead of Image
        //byte[] imageAsByteArray = ms.ToArray();

        //write to file
        using var file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        ms.WriteTo(file);
        file.Close();
        ms.Close();
    }


    /// <summary>
    /// Save the bitmap as PNG file
    /// </summary>
    /// <param name="bitmap">Bitmap to save as PNG file</param>
    /// <param name="fullPath">full file name of the target file</param>
    public static void SaveAsPng(Bitmap bitmap, string fullPath)
    {

        bitmap.Save(fullPath, ImageFormat.Png);

    }

}
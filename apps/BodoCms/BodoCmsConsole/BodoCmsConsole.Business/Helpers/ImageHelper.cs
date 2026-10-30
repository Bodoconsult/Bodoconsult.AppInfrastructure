// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoCmsConsole.Business.App;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Drawing;
using Bodoconsult.Drawing.Model;

namespace BodoCmsConsole.Business.Helpers;

/// <summary>
/// Helepr class for image related tasks
/// </summary>
public static class ImageHelper
{
    private static readonly IAppLoggerProxy? Logger = Globals.Instance.Logger;

    /// <summary>
    /// Maximum image size allowed in bytes
    /// </summary>
    public const int MaxImageSize = 3000000;

    /// <summary>
    /// Small picture width in pixels
    /// </summary>
    public const int SmallPicWidth = 110;

    /// <summary>
    /// Small picture height in pixels
    /// </summary>
    public const int SmallPicHeight = 110;

    /// <summary>
    /// Normal maximum picture width in pixels
    /// </summary>
    public const int NormalPicWidth = 1200;

    /// <summary>
    /// Normal maximum picture height in pixels
    /// </summary>
    public const int NormalPicHeight = 1200;

    /// <summary>
    /// Generate an image for a website from an existing image
    /// </summary>
    /// <param name="fi">Image file</param>
    /// <returns>New image size</returns>
    public static ImageSize GenerateWebImage(FileInfo fi)
    {
        try
        {
            return ImageTools.GenerateWebImage(fi, MaxImageSize, NormalPicWidth, NormalPicHeight);
        }
        catch (Exception ex)
        {
            Logger?.LogError($"GenerateWebImage:{fi.FullName}", ex);
            return new ImageSize { Width = -1, Height = -1 };
        }
    }

    /// <summary>
    /// Get the image size of an image file
    /// </summary>
    /// <param name="path">Full path to the image file</param>
    /// <returns>Image size</returns>
    public static ImageSize GetImageSize(string path)
    {
        return ImageTools.GetImageSize(path);
    }

    /// <summary>
    /// Generate a thumbnail for a file
    /// </summary>
    /// <param name="fi">File</param>
    /// <param name="relativePath">Relative path</param>
    /// <returns>HTML for the image with thumbnail</returns>
    public static string GenerateThumb(FileSystemInfo fi, string relativePath)
    {
        try
        {
            var url = relativePath + fi.Name.Replace(fi.Extension, $"_t{fi.Extension}");
            var thumb = fi.FullName.Replace(fi.Extension, $"_t{fi.Extension}");

            if (new FileInfo(thumb).Exists)
            {
                return url;
            }

            ImageTools.GenerateThumb(fi, thumb, SmallPicWidth, SmallPicHeight);

            return url;
        }
        catch (Exception ex)
        {
            Logger?.LogError($"GenerateThumb: {fi.FullName}", ex);
            return string.Empty;
        }
    }
}
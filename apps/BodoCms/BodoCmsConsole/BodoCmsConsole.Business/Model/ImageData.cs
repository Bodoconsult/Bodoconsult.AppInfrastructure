// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Represents an image file in the database
/// </summary>
public class ImageData
{
    /// <summary>
    /// Image ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Image path
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Clear text name of the image file
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Target path for image if has bo be copied
    /// </summary>
    public string TargetPath { get; set; } = string.Empty;

    /// <summary>
    /// Thumbnail creation required?
    /// </summary>
    public bool Thumbnail { get; set; }

    /// <summary>
    /// Image with in pixels
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Image height n pixels
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Use for slideshow
    /// </summary>
    public bool SlideShow { get; set; }

    /// <summary>
    /// Is the file changed?
    /// </summary>
    public bool IsChanged { get; set; }
}
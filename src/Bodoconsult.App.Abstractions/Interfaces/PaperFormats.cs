// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Predefined paper formats. Papersize is given always in portrait mode (meaning paper width smaller or equal to paper height). Set <see cref="TypoPaperFormat.Landscape"/> to true for getting a landscape paper
/// </summary>
public static class PaperFormats
{
    /// <summary>
    /// US letter paper format portrait
    /// </summary>
    public static TypoPaperFormat LetterFormat => new()
    {
        PaperFormatName = "Letter",
        Size = new TypoSize(21.6, 27.9),
        Landscape = false
    };

    /// <summary>
    /// US letter paper format
    /// </summary>
    public static TypoPaperFormat LetterLandscapeFormat => new()
    {
        PaperFormatName = "Letter",
        Size = new TypoSize(21.6, 27.9),
        Landscape = true
    };

    /// <summary>
    /// US legal paper format portrait
    /// </summary>
    public static TypoPaperFormat LegalFormat => new()
    {
        PaperFormatName = "Legal",
        Size = new TypoSize(21.6, 35.6),
        Landscape = false
    };

    /// <summary>
    /// US legal paper format landscape
    /// </summary>
    public static TypoPaperFormat LegalLandscapeFormat => new()
    {
        PaperFormatName = "Legal",
        Size = new TypoSize(21.6, 35.6),
        Landscape = true
    };

    /// <summary>
    /// DIN A5 paper format portrait
    /// </summary>
    public static TypoPaperFormat A5Format => new()
    {
        PaperFormatName = "A5",
        Size = new TypoSize(21, 148),
        Landscape = false
    };


    /// <summary>
    /// DIN A5 paper format landscape
    /// </summary>
    public static TypoPaperFormat A5LandscapeFormat => new()
    {
        PaperFormatName = "A5",
        Size = new TypoSize(21, 148),
        Landscape = true
    };

    /// <summary>
    /// DIN A4 paper format portrait
    /// </summary>
    public static TypoPaperFormat A4Format => new()
    {
        PaperFormatName = "A4",
        Size = new TypoSize(21, 29.7),
        Landscape = false
    };

    /// <summary>
    /// DIN A4 paper format landscape
    /// </summary>
    public static TypoPaperFormat A4LandscapeFormat => new()
    {
        PaperFormatName = "A4",
        Size = new TypoSize(21, 29.7),
        Landscape = true
    };

    /// <summary>
    /// DIN A3 paper format portrait
    /// </summary>
    public static TypoPaperFormat A3Format => new()
    {
        PaperFormatName = "A3",
        Size = new TypoSize(29.7, 42),
        Landscape = false
    };

    /// <summary>
    /// DIN A3 paper format landscape
    /// </summary>
    public static TypoPaperFormat A3LandscapeFormat => new()
    {
        PaperFormatName = "A3",
        Size = new TypoSize(29.7, 42),
        Landscape = true
    };

    /// <summary>
    /// DIN A2 paper format portrait
    /// </summary>
    public static TypoPaperFormat A2Format => new()
    {
        PaperFormatName = "A2",
        Size = new TypoSize(42, 59.4),
        Landscape = false
    };

    /// <summary>
    /// DIN A2 paper format landscape
    /// </summary>
    public static TypoPaperFormat A2LandscapeFormat => new()
    {
        PaperFormatName = "A2",
        Size = new TypoSize(42, 59.4),
        Landscape = true
    };

    /// <summary>
    /// DIN A1 paper format portrait
    /// </summary>
    public static TypoPaperFormat A1Format => new()
    {
        PaperFormatName = "A1",
        Size = new TypoSize(84.1, 59.4),
        Landscape = false
    };

    /// <summary>
    /// DIN A1 paper format landscape
    /// </summary>
    public static TypoPaperFormat A1LandscapeFormat => new()
    {
        PaperFormatName = "A1",
        Size = new TypoSize(84.1, 59.4),
        Landscape = true
    };

    /// <summary>
    /// DIN A3 paper format portrait
    /// </summary>
    public static TypoPaperFormat A0Format => new()
    {
        PaperFormatName = "A0",
        Size = new TypoSize(84.1, 118.9),
        Landscape = false
    };

    /// <summary>
    /// DIN A3 paper format landscape
    /// </summary>
    public static TypoPaperFormat A0LandscapeFormat => new()
    {
        PaperFormatName = "A0",
        Size = new TypoSize(84.1, 118.9),
        Landscape = false
    };
}
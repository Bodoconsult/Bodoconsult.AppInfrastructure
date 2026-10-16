// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Predefined paper formats
/// </summary>
public static class PaperFormats
{
    /// <summary>
    /// US letter paper format
    /// </summary>
    public static TypoPaperFormat LetterFormat { get; } = new()
    {
        PaperFormatName = "Letter",
        Size = new TypoSize(21.6, 27.9),
    };

    /// <summary>
    /// US legal paper format
    /// </summary>
    public static TypoPaperFormat LegalFormat { get; } = new()
    {
        PaperFormatName = "Legal",
        Size = new TypoSize(21.6, 35.6),
    };

    /// <summary>
    /// DIN A5 paper format
    /// </summary>
    public static TypoPaperFormat A5Format { get; } = new()
    {
        PaperFormatName = "A5",
        Size = new TypoSize(148, 21),
    };

    /// <summary>
    /// DIN A4 paper format
    /// </summary>
    public static TypoPaperFormat A4Format { get; } = new()
    {
        PaperFormatName = "A4",
        Size = new TypoSize(21, 29.7),
    };

    /// <summary>
    /// DIN A3 paper format
    /// </summary>
    public static TypoPaperFormat A3Format { get; } = new()
    {
        PaperFormatName = "A3",
        Size = new TypoSize(29.7, 42),
    };

    /// <summary>
    /// DIN A2 paper format
    /// </summary>
    public static TypoPaperFormat A2Format { get; } = new()
    {
        PaperFormatName = "A2",
        Size = new TypoSize(42, 59.4),
    };

    /// <summary>
    /// DIN A1 paper format
    /// </summary>
    public static TypoPaperFormat A1Format { get; } = new()
    {
        PaperFormatName = "A1",
        Size = new TypoSize(59.4, 84.1),
    };

    /// <summary>
    /// DIN A3 paper format
    /// </summary>
    public static TypoPaperFormat A0Format { get; } = new()
    {
        PaperFormatName = "A0",
        Size = new TypoSize(84.1, 118.9),
    };
}
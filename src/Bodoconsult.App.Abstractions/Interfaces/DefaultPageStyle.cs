// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Default page style
/// </summary>
public class DefaultPageStyle : ITypoPageStyle
{
    /// <summary>
    /// Current margins in cm
    /// </summary>
    public TypoThickness TypoMargins { get; } = new(2, 1.5, 1.3, 1.2);

    /// <summary>
    /// Paper format
    /// </summary>
    public TypoPaperFormat TypoPaperFormat { get; } = PaperFormats.A4Format;

    /// <summary>
    /// Number of text columns the type area is divided in. Default: 1
    /// </summary>
    public int NumberOfColumns { get; set; } = 1;

    /// <summary>
    /// The space between text columns in the type area in cm. Default: 0
    /// </summary>
    public double ColumnGap { get; set; }

    /// <summary>
    /// The resulting text column width in cm
    /// </summary>
    public double ColumnWidth =>
        (TypoPaperFormat.Size.Width - TypoMargins.Left - TypoMargins.Right - (NumberOfColumns * ColumnGap)) /
        NumberOfColumns;
}
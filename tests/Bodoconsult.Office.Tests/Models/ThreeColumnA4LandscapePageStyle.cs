// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Office.Tests.Models;

/// <summary>
/// Default page style A4 landscape with 3 text columns
/// </summary>
public class ThreeColumnA4LandscapePageStyle : ITypoPageStyle
{
    /// <summary>
    /// Current margins in cm
    /// </summary>
    public TypoThickness TypoMargins { get; } = new() { Left = 3, Top = 2, Right = 2, Bottom = 2 };

    /// <summary>
    /// Paper format
    /// </summary>
    public TypoPaperFormat TypoPaperFormat { get; } = new() { PaperFormatName = "A4", Size = new TypoSize(29.7, 21) };

    /// <summary>
    /// Number of text columns the type area is divided in
    /// </summary>
    public int NumberOfColumns { get; set; } = 3;

    /// <summary>
    /// The space between text columns in the type area in cm
    /// </summary>
    public double Space { get; set; } = 1;

    /// <summary>
    /// The resulting text column width in cm
    /// </summary>
    public double ColumnWidth => 0;
}
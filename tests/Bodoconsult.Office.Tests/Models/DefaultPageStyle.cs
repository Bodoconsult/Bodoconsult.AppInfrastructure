// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Office.Tests.Models;

/// <summary>
/// Default page style A4 portrait 1 text column
/// </summary>
public class DefaultPageStyle : ITypoPageStyle
{
    /// <summary>
    /// Current margins in cm
    /// </summary>
    public TypoThickness TypoMargins { get; } = new() { Left = 3, Top = 2, Right = 2, Bottom = 2 };

    /// <summary>
    /// Paper format
    /// </summary>
    public TypoPaperFormat TypoPaperFormat { get; } = new();

    /// <summary>
    /// Number of text columns the type area is divided in
    /// </summary>
    public int NumberOfColumns { get; set; } = 1;

    /// <summary>
    /// The space between text columns in the type area in cm
    /// </summary>
    public double ColumnGap { get; set; } = 0;

    /// <summary>
    /// The resulting text column width in cm
    /// </summary>
    public double ColumnWidth => 0;
}
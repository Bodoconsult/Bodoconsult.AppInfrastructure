// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Common page style settings
/// </summary>
public interface ITypoPageStyle 
{
    /// <summary>
    /// Current margins in cm
    /// </summary>
    TypoThickness TypoMargins { get; }

    /// <summary>
    /// Paper format
    /// </summary>
    TypoPaperFormat TypoPaperFormat { get; }

    /// <summary>
    /// Number of text columns the type area is divided in
    /// </summary>
    int NumberOfColumns { get; set; }

    /// <summary>
    /// The space between text columns in the type area in cm
    /// </summary>
    double ColumnGap { get; set; }

    /// <summary>
    /// The resulting text column width in cm
    /// </summary>
    double ColumnWidth { get; }
}
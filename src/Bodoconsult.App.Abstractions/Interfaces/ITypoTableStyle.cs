// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Drawing;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for table styles with basic table properties
/// </summary>
public interface ITypoTableStyle
{
    /// <summary>
    /// Border spacing in cm
    /// </summary>
    double BorderSpacing { get; }

    /// <summary>
    /// Margins in cm
    /// </summary>
    TypoThickness TypoMargins { get; }

    /// <summary>
    /// Border brush
    /// </summary>
    TypoBrush TypoBorderBrush { get; }

    /// <summary>
    /// Current borderline width setting
    /// </summary>
    TypoThickness TypoBorderThickness { get; }

    /// <summary>
    /// Inside the table horizontal border width in cm
    /// </summary>
    double InsideHorizontalBorderWidth { get; }

    /// <summary>
    /// Inside the table vertical border width in cm
    /// </summary>
    double InsideVerticalBorderWidth { get; }

    /// <summary>
    /// Alternating background color for tables
    /// </summary>
    TypoColor TypoTableAlternateBackColor { get; }

    /// <summary>
    /// Background color
    /// </summary>
    TypoColor TypoTableBackColor { get; }

    /// <summary>
    /// Table border color
    /// </summary>
    TypoColor TypoTableBorderColor { get; }

    /// <summary>
    /// Background color for the header row
    /// </summary>
    TypoColor TypoTableHeaderBackColor { get; }
}
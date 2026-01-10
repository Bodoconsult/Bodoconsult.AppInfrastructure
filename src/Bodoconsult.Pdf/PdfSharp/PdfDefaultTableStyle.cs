// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// Default PDF table style
/// </summary>
public class PdfDefaultTableStyle : ITypoTableStyle
{
    /// <summary>
    /// Border spacing in cm
    /// </summary>
    public double BorderSpacing { get; set; } = 2 * TypoThickness.LineWidth1Pt;

    /// <summary>
    /// Margins in cm
    /// </summary>
    public TypoThickness TypoMargins { get; set; } = new(0);

    /// <summary>
    /// Border brush
    /// </summary>
    public TypoBrush TypoBorderBrush { get; set; } = new TypoSolidColorBrush(TypoColors.Black);

    /// <summary>
    /// Current borderline width setting
    /// </summary>
    public TypoThickness TypoBorderThickness { get; set; } = new TypoThickness(0.5 * TypoThickness.LineWidth1Pt);

    /// <summary>
    /// Inside the table horizontal border width in cm
    /// </summary>
    public double InsideHorizontalBorderWidth { get; set; } = 0.5 * TypoThickness.LineWidth1Pt;

    /// <summary>
    /// Inside the table vertical border width in cm
    /// </summary>
    public double InsideVerticalBorderWidth { get; set; } = 0.5 * TypoThickness.LineWidth1Pt;

    /// <summary>
    /// Alternating background color for tables
    /// </summary>
    public TypoColor TypoTableAlternateBackColor { get; set; } = TypoColors.White;

    /// <summary>
    /// Background color
    /// </summary>
    public TypoColor TypoTableBackColor { get; set; } =  TypoColors.White;

    /// <summary>
    /// Table border color
    /// </summary>
    public TypoColor TypoTableBorderColor { get; set; } =  TypoColors.Black;

    /// <summary>
    /// Background color for the header row
    /// </summary>
    public TypoColor TypoTableHeaderBackColor { get; set; } = TypoColors.LightGray;
}
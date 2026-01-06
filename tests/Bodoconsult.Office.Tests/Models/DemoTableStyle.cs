// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Office.Tests.Models;

public class DemoTableStyle : ITypoTableStyle
{
    /// <summary>
    /// Border spacing in cm
    /// </summary>
    public double BorderSpacing { get; set; } = 0.1;

    /// <summary>
    /// Margins in cm
    /// </summary>
    public TypoThickness TypoMargins { get; set; } = new(0, MeasurementHelper.GetCmFromPt(11), 0, 0);

    /// <summary>
    /// Border brush
    /// </summary>
    public TypoBrush TypoBorderBrush { get; set; } = new TypoSolidColorBrush("#000000");

    /// <summary>
    /// Current borderline width setting
    /// </summary>
    public TypoThickness TypoBorderThickness { get; set; } = new(0.5 * TypoThickness.LineWidth1Pt, 0.5 * TypoThickness.LineWidth1Pt, 0.5 * TypoThickness.LineWidth1Pt, 0.5 * TypoThickness.LineWidth1Pt);

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
    public TypoColor TypoTableAlternateBackColor { get; set; } = TypoColors.LightBlue;

    /// <summary>
    /// Background color
    /// </summary>
    public TypoColor TypoTableBackColor { get; set; } = TypoColors.White;

    /// <summary>
    /// Table border color
    /// </summary>
    public TypoColor TypoTableBorderColor { get; set; } = TypoColors.Black;
}
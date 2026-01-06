// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Style for <see cref="Table"/> instances
/// </summary>
public class TableStyle : StyleBase, ITypoTableStyle
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public TableStyle()
    {
        TagToUse = "TableStyle";
        Name = TagToUse;
    }

    /// <summary>
    /// Margins. Margin left and right are ignored. Table is always centered
    /// </summary>
    public Thickness Margins { get; set; } = new(0, MeasurementHelper.GetCmFromPt(Styleset.DefaultFontSize), 0, 0);

    /// <summary>
    /// Margins
    /// </summary>
    [DoNotSerialize]
    public TypoThickness TypoMargins => Margins;

    /// <summary>
    /// Border spacing in cm
    /// </summary>
    public double BorderSpacing { get; set; } = Styleset.DefaultTablePaddingWidth;

    /// <summary>
    /// Border brush
    /// </summary>
    public Brush BorderBrush { get; set; } = new SolidColorBrush("#000000");

    /// <summary>
    /// Border brush
    /// </summary>
    [DoNotSerialize]
    public TypoBrush TypoBorderBrush => BorderBrush;

    /// <summary>
    /// Current borderline width setting
    /// </summary>
    public Thickness BorderThickness { get; set; } = new(0.5 * TypoThickness.LineWidth1Pt, 0.5 * TypoThickness.LineWidth1Pt, 0.5 * TypoThickness.LineWidth1Pt, 0.5 * TypoThickness.LineWidth1Pt);

    /// <summary>
    /// Current borderline width setting
    /// </summary>
    [DoNotSerialize]
    public TypoThickness TypoBorderThickness => BorderThickness;

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
    public Color TableAlternateBackColor { get; set; } = new(TypoColors.White);

    /// <summary>
    /// Alternating background color for tables
    /// </summary>
    [DoNotSerialize]
    public TypoColor TypoTableAlternateBackColor => TableAlternateBackColor;

    /// <summary>
    /// Background color
    /// </summary>
    public Color TableBackColor { get; set; } = new(TypoColors.White);

    /// <summary>
    /// Background color
    /// </summary>
    [DoNotSerialize]
    public TypoColor TypoTableBackColor => TableBackColor;

    /// <summary>
    /// Table border color
    /// </summary>
    public Color TableBorderColor { get; set; } = new(TypoColors.Black);

    /// <summary>
    /// Table border color
    /// </summary>
    [DoNotSerialize]
    public TypoColor TypoTableBorderColor => TableBorderColor;

    /// <summary>
    /// Table header background color
    /// </summary>
    public Color TableHeaderBackgroundColor { get; set; } = new(TypoColors.LightGray);

    /// <summary>
    /// Background color for the header row
    /// </summary>
    [DoNotSerialize]
    public TypoColor TypoTableHeaderBackColor => TableHeaderBackgroundColor;
}
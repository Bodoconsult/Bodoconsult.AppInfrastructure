// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// A style for a normal left aligned paragraph
/// </summary>
public class ParagraphStyleBase : StyleBase, ITypoParagraphStyle
{
    /// <summary>
    /// Font name
    /// </summary>
    public string FontName { get; set; } = Styleset.DefaultFontName;

    /// <summary>
    /// Font size in pt
    /// </summary>
    public int FontSize { get; set; } = Styleset.DefaultFontSize;

    /// <summary>
    /// Font color
    /// </summary>
    public Color FontColor { get; set; } = Styleset.DefaultColor;

    /// <summary>
    /// Font color
    /// </summary>
    [DoNotSerialize] 
    public TypoColor TypoFontColor => FontColor;

    /// <summary>
    /// Bold
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Italic
    /// </summary>
    public bool Italic { get; set; }

    /// <summary>
    /// Text alignment legt, center, justify or right. Default: left
    /// </summary>
    public TypoTextAlignment TextAlignment { get; set; } = TypoTextAlignment.Left;

    /// <summary>
    /// Margins
    /// </summary>
    public Thickness Margins { get; set; } = new(0, MeasurementHelper.GetCmFromPt(Styleset.DefaultFontSize * 0.5), 0, 0);

    /// <summary>
    /// Margins
    /// </summary>
    [DoNotSerialize] 
    public TypoThickness TypoMargins => Margins;

    /// <summary>
    /// Border brush
    /// </summary>
    public Brush BorderBrush { get; set; }

    /// <summary>
    /// Border brush
    /// </summary>
    [DoNotSerialize]
    public TypoBrush TypoBorderBrush => BorderBrush;

    /// <summary>
    /// Current borderline width setting
    /// </summary>
    public Thickness BorderThickness { get; set; } = new(0, 0, 0, 0);

    /// <summary>
    /// Current borderline width setting
    /// </summary>
    [DoNotSerialize]
    public TypoThickness TypoBorderThickness => BorderThickness;

    /// <summary>
    /// Paddings. Padding settings are applied only if a border is set
    /// </summary>
    public Thickness Paddings { get; set; } = new(0, 0, 0, 0);

    /// <summary>
    /// Paddings. Padding settings are applied only if a border is set
    /// </summary>
    [DoNotSerialize]
    public TypoThickness TypoPaddings => Paddings;

    /// <summary>
    /// Indent of the first line in pt. Negative number is indicating a hanging indent
    /// </summary>
    public double FirstLineIndent { get; set; }

    /// <summary>
    /// Add a page break before the heading. Default: false
    /// </summary>
    public bool PageBreakBefore { get; set; } = false;

    /// <summary>
    /// Add a page break before the heading. Default: false
    /// </summary>
    public bool KeepWithNextParagraph { get; set; } = false;

    /// <summary>
    /// Keep the paragraph together on one side. Default: false
    /// </summary>
    public bool KeepTogether { get; set; } = false;

    /// <summary>
    /// Line height in cm. Default: 116% of Styleset.DefaultFontSize
    /// </summary>
    public double LineHeight { get; set; } = MeasurementHelper.GetCmFromPt(Styleset.DefaultFontSize * 1.16);

    /// <summary>
    /// Current line spacing rule
    /// </summary>
    public LineSpacingRuleEnum LineSpacingRule { get; set; } = LineSpacingRuleEnum.Auto;
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Office.Tests.Models;

/// <summary>
/// A demo class for a <see cref="ITypoParagraphStyle"/> implementation
/// </summary>
public class DemoStyle : ITypoParagraphStyle
{
    /// <summary>
    /// Font name
    /// </summary>
    public string FontName { get; set; }

    /// <summary>
    /// Font size in pt
    /// </summary>
    public int FontSize { get; set; }

    /// <summary>
    /// Font color
    /// </summary>
    public TypoColor TypoFontColor { get; set; }

    /// <summary>
    /// Bold
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Italic
    /// </summary>
    public bool Italic { get; set; }

    /// <summary>
    /// Text alignment legt, center, justify or right
    /// </summary>
    public TypoTextAlignment TextAlignment { get; set; } = TypoTextAlignment.Left;

    /// <summary>
    /// Margins
    /// </summary>
    public TypoThickness TypoMargins { get; set; } = new(0, 0, 0, 0);

    /// <summary>
    /// Border brush
    /// </summary>
    public TypoBrush TypoBorderBrush { get; set; } = new TypoSolidColorBrush(TypoColors.Black);

    /// <summary>
    /// Current borderline width setting
    /// </summary>
    public TypoThickness TypoBorderThickness { get; set; } = new(0, 0, 0, 0);

    /// <summary>
    /// Paddings. Padding settings are applied only if a border is set
    /// </summary>
    public TypoThickness TypoPaddings { get; set; } = new(0, 0, 0, 0);

    /// <summary>
    /// Indent of the first line in pt. Negative number is indicating a hanging indent
    /// </summary>
    public double FirstLineIndent { get; set; }

    /// <summary>
    /// Add a page break before the heading. Default: false
    /// </summary>
    public bool PageBreakBefore { get; set; }

    /// <summary>
    /// Add a page break before the heading. Default: false
    /// </summary>
    public bool KeepWithNextParagraph { get; set; }

    /// <summary>
    /// Keep the paragraph together on one side. Default: false
    /// </summary>
    public bool KeepTogether { get; set; }

    /// <summary>
    /// Line height in cm
    /// </summary>
    public double LineHeight { get; set; } = MeasurementHelper.GetCmFromPt(11 * 1.18);

    /// <summary>
    /// Current line spacing rule
    /// </summary>
    public LineSpacingRuleEnum LineSpacingRule { get; set; } = LineSpacingRuleEnum.Auto;
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for paragraph styles
/// </summary>
public interface ITypoParagraphStyle
{
    /// <summary>
    /// Font name
    /// </summary>
    string FontName { get; }

    /// <summary>
    /// Font size in pt
    /// </summary>
    int FontSize { get; }

    /// <summary>
    /// Font color
    /// </summary>
    TypoColor TypoFontColor { get; }

    /// <summary>
    /// Bold
    /// </summary>
    bool Bold { get; set; }

    /// <summary>
    /// Italic
    /// </summary>
    bool Italic { get; set; }

    /// <summary>
    /// Text alignment legt, center, justify or right
    /// </summary>
    TypoTextAlignment TextAlignment { get; }

    /// <summary>
    /// Margins
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
    /// Paddings. Padding settings are applied only if a border is set
    /// </summary>
    TypoThickness TypoPaddings { get; }

    /// <summary>
    /// Indent of the first line in pt. Negative number is indicating a hanging indent
    /// </summary>
    double FirstLineIndent { get; }

    /// <summary>
    /// Add a page break before the heading. Default: false
    /// </summary>
    bool PageBreakBefore { get; }

    /// <summary>
    /// Add a page break before the heading. Default: false
    /// </summary>
    bool KeepWithNextParagraph { get; }

    /// <summary>
    /// Keep the paragraph together on one side. Default: false
    /// </summary>
    bool KeepTogether { get; }

    /// <summary>
    /// Line height in cm
    /// </summary>
    double LineHeight { get; set; }

    /// <summary>
    /// Current line spacing rule
    /// </summary>
    LineSpacingRuleEnum LineSpacingRule { get; set; }

    /// <summary>
    /// Activate widow control
    /// </summary>
    bool WidowControl { get; set; }
}
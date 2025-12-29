// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Base style for styles with page settings
/// </summary>
public abstract class PageStyleBase : StyleBase
{
    /// <summary>
    /// Paper format
    /// </summary>
    public PageFormat PageFormat { get; set; } = new();

    /// <summary>
    /// Page margins for type area in cm
    /// </summary>
    public Thickness Margins { get; set; } = new(3, 2, 2, 2);

    /// <summary>
    /// Type area width in cm
    /// </summary>
    [DoNotSerialize]
    public double TypeAreaWidth => PageFormat.Size.Width - Margins.Left - Margins.Right;

    /// <summary>
    /// Type area height in cm
    /// </summary>
    [DoNotSerialize]
    public double TypeAreaHeight => PageFormat.Size.Height - Margins.Top - Margins.Bottom;

    /// <summary>
    /// Max image width in cm
    /// </summary>
    [DoNotSerialize]
    public double MaxImageWidth => 0.95 * TypeAreaWidth;

    /// <summary>
    /// Max image height in cm
    /// </summary>
    [DoNotSerialize]
    public double MaxImageHeight => 0.33 * TypeAreaHeight;

    /// <summary>
    /// Space reserved for the header in cm
    /// </summary>
    public double HeaderHeight { get; set; } = 0.5;

    /// <summary>
    /// Bottom margin of the header in cm
    /// </summary>
    public double HeaderMarginBottom { get; set; } = 0.2;

    /// <summary>
    /// Space reserved for the footer in cm
    /// </summary>
    public double FooterHeight { get; set; } = 0.5;

    /// <summary>
    /// Margin in footer above the footer text and below the main text in cm
    /// </summary>
    public double FooterMarginTop { get; set; } = 0.2;
}
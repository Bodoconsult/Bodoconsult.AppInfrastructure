// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using System;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Base style for styles with page settings
/// </summary>
public abstract class PageStyleBase : StyleBase, ITypoPageStyle
{
    /// <summary>
    /// Paper format
    /// </summary>
    public PaperFormat PaperFormat { get; set; } = new();

    /// <summary>
    /// Paper format
    /// </summary>
    [DoNotSerialize]
    public TypoPaperFormat TypoPaperFormat => PaperFormat;

    /// <summary>
    /// Page margins for type area in cm
    /// </summary>
    public Thickness Margins { get; set; } = new(3, 2.5, 2, 2);

    /// <summary>
    /// Dots per inch (for converting to pixels). Default: 96dpi
    /// </summary>
    public double DotsPerInch { get; set; } = 96;

    /// <summary>
    /// Sets the factor for the calculation of the left margin. See <see cref="ITypoPageStyle.SetMargins"/> for details
    /// </summary>
    public double MarginLeftFactor { get; set; } = 2;

    /// <summary>
    /// Sets the factor for the calculation of the right margin. See <see cref="ITypoPageStyle.SetMargins"/> for details
    /// </summary>
    public double MarginRightFactor { get; set; } = 2;

    /// <summary>
    /// Sets the factor for the calculation of the top margin. See <see cref="ITypoPageStyle.SetMargins"/> for details
    /// </summary>
    public double MarginTopFactor { get; set; } = 1;

    /// <summary>
    /// Sets the factor for the calculation of the bottom margin. See <see cref="ITypoPageStyle.SetMargins"/> for details
    /// </summary>
    public double MarginBottomFactor { get; set; } = 2;

    /// <summary>
    /// Unit used for margin calculations in cm (Teil in cm)
    /// 
    /// <see cref="ITypoPageStyle.MarginUnit"/> = PaperSize.Size.Width - TypeAreaRect.Size.Width / (<see cref="ITypoPageStyle.SetMargins"/>.left + <see cref="ITypoPageStyle.SetMargins"/>.right)
    /// </summary>
    public double MarginUnit { get; private set; }

    /// <summary>
    /// Width of the grid column divider in the type area in cm (Spaltenabstand in cm)
    /// </summary>
    public double GridColumnDividerWidth { get; set; } = 0.5;

    /// <summary>
    /// Grid column width in cm
    /// </summary>
    public double GridColumnWidth { get; set; } = 2.5;

    /// <summary>
    /// Number of grid columns to use for the layout of a column in the type area
    /// </summary>
    public int GridColumnCount { get; set; } = 6;

    /// <summary>
    /// Type area rect dimensions in cm (Abmessungen des Satzspiegels in cm)
    /// </summary>
    public TypoRect TypeAreaRect { get; private set; }

    /// <summary>
    /// Header area rect dimensions in cm
    /// </summary>
    public TypoRect HeaderAreaRect { get; private set; }

    /// <summary>
    /// Footer area rect dimensions in cm
    /// </summary>
    public TypoRect FooterAreaRect { get; private set; }

    /// <summary>
    /// Current margins in cm
    /// </summary>
    [DoNotSerialize]
    public TypoThickness TypoMargins => Margins;

    /// <summary>
    /// Height of the page header in cm
    /// </summary>
    public double PageHeaderHeight { get; set; } = 1;

    /// <summary>
    /// Margin between page header and type area in cm
    /// </summary>
    public double PageHeaderMargin { get; set; } = 0.5;

    /// <summary>
    /// Height of the page header in cm
    /// </summary>
    public double PageFooterHeight { get; set; } = 1;

    /// <summary>
    /// Margin between page header and type area in cm
    /// </summary>
    public double PageFooterMargin { get; set; } = 0.5;

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
    [DoNotSerialize]
    public double ColumnWidth => (TypeAreaWidth - (NumberOfColumns - 1) * ColumnGap) / NumberOfColumns;

    /// <summary>
    /// Coordinates of the vertical lines of the typo grid
    /// </summary>
    public double[] VerticalLines { get; private set; }

    #region Methods

    /// <summary>
    /// Calculate margins. Margins are calculate as follows:
    ///
    /// TypeAreaWidth = NumberOfColumns * (GridColumnCount * GridColumnWidth + (GridColumnCount - 1) * GridColumnDividerWidth) + (NumberOfColumns - 1) * Space
    /// 
    /// <see cref="ITypoPageStyle.MarginUnit"/> = (PaperSize.Size.Width - TypeAreaWidth) / (<see cref="ITypoPageStyle.MarginLeftFactor"/> + <see cref="ITypoPageStyle.MarginRightFactor"/>)
    /// 
    /// Margins.Left  = <see cref="ITypoPageStyle.MarginLeftFactor"/> * <see cref="ITypoPageStyle.MarginUnit"/>
    /// Margins.Right  = <see cref="ITypoPageStyle.MarginRightFactor"/> * <see cref="ITypoPageStyle.MarginUnit"/>
    /// Margins.Top  = <see cref="ITypoPageStyle.MarginTopFactor"/> * <see cref="ITypoPageStyle.MarginUnit"/>
    /// Margins.Bottom  = <see cref="ITypoPageStyle.MarginBottomFactor"/>* <see cref="ITypoPageStyle.MarginUnit"/>
    /// 
    /// </summary>
    public void SetMargins()
    {
        var typeAreaWidth = NumberOfColumns * (GridColumnCount * GridColumnWidth + (GridColumnCount - 1) * GridColumnDividerWidth) + (NumberOfColumns - 1) * ColumnGap;

        double mu;

        if (TypoPaperFormat.Landscape)
        {
            mu = TypoPaperFormat.Size.Height - typeAreaWidth;
        }
        else
        {
            mu = TypoPaperFormat.Size.Width - typeAreaWidth;
        }

        MarginUnit = mu / (MarginLeftFactor + MarginRightFactor);

        if (MarginUnit < 0)
        {
            throw new ArgumentException("MarginUnit has to be positive. Ether the paper width is too small or the number of grid columns multiple by the requested column width is too big!");
        }

        TypoMargins.Left = Math.Round(MarginLeftFactor * MarginUnit, 2);
        TypoMargins.Right = Math.Round(MarginRightFactor * MarginUnit, 2);
        TypoMargins.Top = Math.Round(MarginTopFactor * MarginUnit, 2);
        TypoMargins.Bottom = Math.Round(MarginBottomFactor * MarginUnit, 2);

        // Type area
        var typeAreaHeight = TypoPaperFormat.Size.Height - TypoMargins.Top - TypoMargins.Bottom;

        var p1 = new TypoPoint(TypoMargins.Left, TypoMargins.Top);
        var p2 = new TypoPoint(TypoMargins.Left + typeAreaWidth, TypoMargins.Top + typeAreaHeight);
        TypeAreaRect = new TypoRect(p1, p2);

        // Header
        var hp1 = new TypoPoint(p1.X, p1.Y - PageHeaderMargin - PageHeaderHeight);
        var hp2 = new TypoPoint(p1.X + typeAreaWidth, p1.Y - PageHeaderMargin);
        HeaderAreaRect = new TypoRect(hp1, hp2);

        // Footer
        var fp1 = new TypoPoint(p1.X, p1.Y + PageFooterMargin);
        var fp2 = new TypoPoint(p1.X + typeAreaWidth, p1.Y + PageFooterMargin + PageFooterHeight);
        FooterAreaRect = new TypoRect(fp1, fp2);
    }

    /// <summary>
    /// Create all vertical lines for the typographic grid
    /// </summary>
    public void CalculateVerticalLines()
    {
        var numberOfLines = GridColumnCount * 2 * NumberOfColumns;

        VerticalLines = new double[numberOfLines];

        var left = TypoMargins.Left;
        for (var column = 0; column < NumberOfColumns; column++)
        {
            for (var i = 0; i < GridColumnCount; i++)
            {
                VerticalLines[i * 2] = left + i * (GridColumnWidth + GridColumnDividerWidth);
                VerticalLines[i * 2 + 1] = left + i * (GridColumnWidth + GridColumnDividerWidth) + GridColumnWidth;
            }

            left += ColumnWidth + ColumnGap;
        }
    }

    /// <summary>
    /// Get the width of a landscape element which should a certain number of layout columns in cm
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <returns>Width in cm</returns>
    public double GetWidth(int numberOfColumnsUsed)
    {
        return numberOfColumnsUsed * GridColumnWidth + (numberOfColumnsUsed - 1) * GridColumnDividerWidth;
    }

    /// <summary>
    /// Get the height of an element using the Golden Schnitt ratio in cm
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <returns>Height in cm</returns>
    public double GetHeight(int numberOfColumnsUsed)
    {
        return TypoPaperFormat.Landscape
            ? Math.Round((numberOfColumnsUsed * GridColumnWidth + (numberOfColumnsUsed - 1) * GridColumnDividerWidth) *
                         TypographicConstants.GoldenerSchnittRatio, 2)
            : Math.Round((numberOfColumnsUsed * GridColumnWidth + (numberOfColumnsUsed - 1) * GridColumnDividerWidth) /
                         TypographicConstants.GoldenerSchnittRatio, 2);
    }

    /// <summary>
    /// Get the width of a landscape element which should a certain number of layout columns in pixels
    /// </summary>
    /// <param name="numberOfColumnsUsed">Number of columns used</param>
    /// <returns>Width in pixels</returns>
    public int GetPixelWidth(int numberOfColumnsUsed)
    {
        return (int)(GetWidth(numberOfColumnsUsed) * TypographicConstants.InchPerCentimeter * DotsPerInch);
    }

    /// <summary>
    /// Get the height of an element using the Golden Schnitt ratio in pixels
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <returns>Height in pixels</returns>
    public int GetPixelHeight(int numberOfColumnsUsed)
    {
        var height = GetHeight(numberOfColumnsUsed) * TypographicConstants.InchPerCentimeter * DotsPerInch;
        return (int)height;
    }

    #endregion

    /// <summary>
    /// Type area width in cm
    /// </summary>
    [DoNotSerialize]
    public double TypeAreaWidth => PaperFormat.Size.Width - Margins.Left - Margins.Right;

    /// <summary>
    /// Type area height in cm
    /// </summary>
    [DoNotSerialize]
    public double TypeAreaHeight => PaperFormat.Size.Height - Margins.Top - Margins.Bottom;

    /// <summary>
    /// Max image width in cm
    /// </summary>
    [DoNotSerialize]
    public double MaxImageWidth => 0.95 * ColumnWidth;

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
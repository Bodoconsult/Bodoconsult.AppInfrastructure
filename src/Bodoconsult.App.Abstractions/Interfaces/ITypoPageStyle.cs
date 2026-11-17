// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Common page style settings
/// </summary>
public interface ITypoPageStyle 
{
    /// <summary>
    /// Dots per inch (for converting to pixels). Default: 96dpi
    /// </summary>
    double DotsPerInch { get; set; }

    /// <summary>
    /// Sets the factor for the calculation of the left margin. See <see cref="SetMargins"/> for details
    /// </summary>
    double MarginLeftFactor { get; set; }

    /// <summary>
    /// Sets the factor for the calculation of the right margin. See <see cref="SetMargins"/> for details
    /// </summary>
    double MarginRightFactor { get; set; }

    /// <summary>
    /// Sets the factor for the calculation of the top margin. See <see cref="SetMargins"/> for details
    /// </summary>
    double MarginTopFactor { get; set; }

    /// <summary>
    /// Sets the factor for the calculation of the bottom margin. See <see cref="SetMargins"/> for details
    /// </summary>
    double MarginBottomFactor { get; set; }

    /// <summary>
    /// Unit used for margin calculations in cm (Teil in cm)
    /// 
    /// <see cref="MarginUnit"/> = PaperSize.Size.Width - TypeAreaRect.Size.Width / (<see cref="SetMargins"/>.left + <see cref="SetMargins"/>.right)
    /// </summary>
    double MarginUnit { get; }

    /// <summary>
    /// Width of the grid column divider in the type area in cm (Spaltenabstand in cm)
    /// </summary>
    double GridColumnDividerWidth { get; set; }

    /// <summary>
    /// Grid column width in cm
    /// </summary>
    double GridColumnWidth { get; set; }

    /// <summary>
    /// Number of grid columns to use for the layout of a column in the type area
    /// </summary>
    int GridColumnCount { get; set; }

    /// <summary>
    /// Type area rect dimensions in cm (Abmessungen des Satzspiegels in cm)
    /// </summary>
    TypoRect TypeAreaRect { get; }

    /// <summary>
    /// Header area rect dimensions in cm
    /// </summary>
    TypoRect HeaderAreaRect { get; }

    /// <summary>
    /// Footer area rect dimensions in cm
    /// </summary>
    TypoRect FooterAreaRect { get; }

    /// <summary>
    /// Current margins in cm
    /// </summary>
    TypoThickness TypoMargins { get; }

    /// <summary>
    /// Height of the page header in cm
    /// </summary>
    double PageHeaderHeight { get; set; }

    /// <summary>
    /// Margin between page header and type area in cm
    /// </summary>
    double PageHeaderMargin { get; set; }

    /// <summary>
    /// Height of the page header in cm
    /// </summary>
    double PageFooterHeight { get; set; }

    /// <summary>
    /// Margin between page header and type area in cm
    /// </summary>
    double PageFooterMargin { get; set; }

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

    /// <summary>
    /// Coordinates of the vertical lines of the typo grid
    /// </summary>
    double[]? VerticalLines { get; }

    /// <summary>
    /// Calculate margins. Margins are calculate as follows:
    ///
    /// TypeAreaWidth = NumberOfColumns * (GridColumnCount * GridColumnWidth + (GridColumnCount - 1) * GridColumnDividerWidth) + (NumberOfColumns - 1) * Space
    /// 
    /// <see cref="MarginUnit"/> = (PaperSize.Size.Width - TypeAreaWidth) / (<see cref="MarginLeftFactor"/> + <see cref="MarginRightFactor"/>)
    /// 
    /// Margins.Left  = <see cref="MarginLeftFactor"/> * <see cref="MarginUnit"/>
    /// Margins.Right  = <see cref="MarginRightFactor"/> * <see cref="MarginUnit"/>
    /// Margins.Top  = <see cref="MarginTopFactor"/> * <see cref="MarginUnit"/>
    /// Margins.Bottom  = <see cref="MarginBottomFactor"/>* <see cref="MarginUnit"/>
    /// 
    /// </summary>
    void SetMargins();

    /// <summary>
    /// Create all vertical lines for the typographic grid
    /// </summary>
    public void CalculateVerticalLines();

    /// <summary>
    /// Get the width of a landscape element which should a certain number of layout columns in cm
    /// </summary>
    /// <param name="numberOfColumnsUsed">Number of columns used</param>
    /// <returns>Width in cm</returns>
    double GetWidth(int numberOfColumnsUsed);

    /// <summary>
    /// Get the height of an element using the Golden Schnitt ratio in cm
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <returns>Height in cm</returns>
    double GetHeight(int numberOfColumnsUsed);

    /// <summary>
    /// Get the width of a landscape element which should a certain number of layout columns in pixels
    /// </summary>
    /// <param name="numberOfColumnsUsed">Number of columns used</param>
    /// <returns>Width in pixels</returns>
    int GetPixelWidth(int numberOfColumnsUsed);

    /// <summary>
    /// Get the height of an element using the Golden Schnitt ratio in pixels
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <returns>Height in pixels</returns>
    int GetPixelHeight(int numberOfColumnsUsed);
}
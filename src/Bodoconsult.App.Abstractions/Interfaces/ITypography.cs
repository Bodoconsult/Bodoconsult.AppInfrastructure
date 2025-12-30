// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for typographic data. Coordinates system origin is in the left top corner. X-axis going to right. Y-axis going down.
/// </summary>
public interface ITypography: ITypoPageStyle
{
    /// <summary>
    /// Indicator where to place the page number (or the field for it) in a footer text or header text
    /// </summary>
    public const string PageFieldIndicator = "<<page>>";

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
    public TypoRect TypeAreaRect { get; }

    /// <summary>
    /// Header area rect dimensions in cm
    /// </summary>
    public TypoRect HeaderAreaRect { get; }

    /// <summary>
    /// Footer area rect dimensions in cm
    /// </summary>
    public TypoRect FooterAreaRect { get; }

    /// <summary>
    /// Default font name
    /// </summary>
    string FontName { get; set; }

    /// <summary>
    /// Default font size in pt
    /// </summary>
    double FontSize { get; set; }

    /// <summary>
    /// Small font size in pt
    /// </summary>
    double SmallFontSize { get; set; }

    /// <summary>
    /// Tiny font size in pt
    /// </summary>
    double ExtraSmallFontSize { get; set; }

    /// <summary>
    /// Title font name
    /// </summary>
    string TitleFontName { get; set; }

    /// <summary>
    /// Title font size in pt
    /// </summary>
    double TitleFontSize { get; set; }

    /// <summary>
    /// Subtitle font name
    /// </summary>
    string SubTitleFontName { get; set; }

    /// <summary>
    /// Subtitle font size in pt
    /// </summary>
    double SubTitleFontSize { get; set; }

    /// <summary>
    /// Font name used for headings
    /// </summary>
    string HeadingFontName { get; set; }

    /// <summary>
    /// Font size used for headings of level 1 in pt
    /// </summary>
    double HeadingFontSize1 { get; set; }

    /// <summary>
    /// Font size used for headings of level 2 in pt
    /// </summary>
    double HeadingFontSize2 { get; set; }

    /// <summary>
    /// Font size used for headings of level 3 in pt
    /// </summary>
    double HeadingFontSize3 { get; set; }

    /// <summary>
    /// Font size used for headings of level 4 in pt
    /// </summary>
    double HeadingFontSize4 { get; set; }

    /// <summary>
    /// Font size used for headings of level 5 in pt
    /// </summary>
    double HeadingFontSize5 { get; set; }

    /// <summary>
    /// Line height in cm (Zeilenabstand ZAB in cm)
    /// </summary>
    double LineHeight { get; set; }

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
    /// Dots per inch (for converting to pixels). Default: 96dpi
    /// </summary>
    double DotsPerInch { get; set; }

    /// <summary>
    /// Coordinates of the vertical lines of the typo grid
    /// </summary>
    double[] VerticalLines { get; }

    /// <summary>
    /// Path to logo to print in the page header
    /// </summary>
    string LogoPath { get; set; }

    /// <summary>
    /// Width of the logo in the page header
    /// </summary>
    double LogoWidth { get; set; }

    /// <summary>
    /// Styling for charts in the document
    /// </summary>
    ChartStyle ChartStyle { get; set; }

    /// <summary>
    /// Copyright to print in charts and other items
    /// </summary>
    string Copyright { get; set; }

    /// <summary>
    /// Color for table header background
    /// </summary>
    TypoColor TableHeaderBackground { get; set; }

    /// <summary>
    /// Color for table body background
    /// </summary>
    TypoColor TableBodyBackground { get; set; }

    /// <summary>
    /// Color for table header background
    /// </summary>
    TypoColor TableHeaderUnborderedBackground { get; set; }
    
    /// <summary>
    /// Color for table body background
    /// </summary>
    TypoColor TableBodyUnborderedBackground { get; set; }

    /// <summary>
    /// Corner radius of a table in cm
    /// </summary>
    double TableCornerRadius { get; set; }

    /// <summary>
    /// Border color of the table
    /// </summary>
    TypoColor TableBorderColor { get; set; }

    /// <summary>
    /// Border width of a table in cm
    /// </summary>
    double TableBorderWidth { get; set; }

    /// <summary>
    /// Create all vertical lines for the typographic grid
    /// </summary>
    void CalculateVerticalLines();

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
    /// Get the width of a landscape element which should a certain number of layout columns in cm
    /// </summary>
    /// <param name="numberOfColumnsUsed">Number of columns used</param>
    /// <returns>Width in cm</returns>
    double GetWidth(int numberOfColumnsUsed);

    /// <summary>
    /// Get the height of an element using the Golden Schnitt ratio in cm
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <param name="landscape"></param>
    /// <returns>Height in cm</returns>
    double GetHeight(int numberOfColumnsUsed, bool landscape = true);

    /// <summary>
    /// Get the width of a landscape element which should a certain number of layout columns in pixels
    /// </summary>
    /// <param name="numberOfColumnsUsed">Number of columns used</param>
    /// <returns>Width in cm</returns>
    int GetPixelWidth(int numberOfColumnsUsed);

    /// <summary>
    /// Get the height of an element using the Golden Schnitt ratio in pixels
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <param name="landscape"></param>
    /// <returns></returns>
    int GetPixelHeight(int numberOfColumnsUsed, bool landscape = true);
}
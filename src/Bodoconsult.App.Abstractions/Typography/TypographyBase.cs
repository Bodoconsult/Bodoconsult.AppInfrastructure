// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Abstractions.Typography;

/// <summary>
/// Base class for typography data
/// </summary>
public class TypographyBase : ITypography
{
    private double _columnWidth;

    /// <summary>
    /// Default ctor
    /// </summary>
    public TypographyBase()
    {
        FontName = "Calibri";
        FontSize = 11;
        SmallFontSize = FontSize - 2;
        ExtraSmallFontSize = SmallFontSize - 2;

        HeadingFontName = FontName;
        HeadingFontSize5 = FontSize;
        HeadingFontSize4 = HeadingFontSize5 + 2;
        HeadingFontSize3 = HeadingFontSize4 + 2;
        HeadingFontSize2 = HeadingFontSize3 + 2;
        HeadingFontSize1 = HeadingFontSize2 + 4;

        TitleFontName = FontName;
        SubTitleFontName = FontName;

        TitleFontSize = HeadingFontSize1 + 6;
        SubTitleFontSize = HeadingFontSize1 + 2;

        LineHeight = 0.5;
        GridColumnDividerWidth = 0.5;
        GridColumnWidth = 2.5;
        GridColumnCount = 6;
        DotsPerInch = 96;
        LogoWidth = 2 * GridColumnWidth + GridColumnDividerWidth;

        MarginLeftFactor = 2;
        MarginRightFactor = 2;
        MarginTopFactor = 1;
        MarginBottomFactor = 2;

        SetMargins();

        ChartStyle = new ChartStyle();
        Copyright = "Bodoconsult GmbH";

        TableBodyBackground = TypoColors.White;
        TableHeaderBackground = TypoColors.White;
        TableBodyUnborderedBackground = TypoColors.Transparent;
        TableHeaderUnborderedBackground = TypoColors.Transparent;
        TableCornerRadius = 0.3;
        TableBorderWidth = 0.05;
        TableBorderColor = TypoColor.FromArgb(178, 204, 255);
    }

    /// <summary>
    /// Paper format. Default: DIN A4
    /// </summary>
    public TypoPaperFormat TypoPaperFormat { get; set; } = new();

    /// <summary>
    /// Number of text columns the type area is divided in
    /// </summary>
    public int NumberOfColumns { get; set; } = 1;

    /// <summary>
    /// The space between text columns in the type area in cm
    /// </summary>
    public double Space { get; set; } = 0;


    /// <summary>
    /// The resulting text column width in cm
    /// </summary>
    public double ColumnWidth => _columnWidth;

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
    /// Border color of the table
    /// </summary>
    public TypoColor TableBorderColor { get; set; }

    /// <summary>
    /// Border width of a table in cm
    /// </summary>
    public double TableBorderWidth { get; set; }

    /// <summary>
    /// Copyright to print in charts and other items
    /// </summary>
    public string Copyright { get; set; }

    /// <summary>
    /// Default font name
    /// </summary>
    public string FontName { get; set; }

    /// <summary>
    /// Default font size in pt
    /// </summary>
    public double FontSize { get; set; }

    /// <summary>
    /// Small font size in pt
    /// </summary>
    public double SmallFontSize { get; set; }

    /// <summary>
    /// Tiny font size in pt
    /// </summary>
    public double ExtraSmallFontSize { get; set; }

    /// <summary>
    /// Title font name
    /// </summary>
    public string TitleFontName { get; set; }

    /// <summary>
    /// Title font size in pt
    /// </summary>
    public double TitleFontSize { get; set; }

    /// <summary>
    /// Subtitle font name
    /// </summary>
    public string SubTitleFontName { get; set; }

    /// <summary>
    /// Subtitle font size in pt
    /// </summary>
    public double SubTitleFontSize { get; set; }

    /// <summary>
    /// Font name used for headings
    /// </summary>
    public string HeadingFontName { get; set; }

    /// <summary>
    /// Font size used for headings of level 1 in pt
    /// </summary>
    public double HeadingFontSize1 { get; set; }

    /// <summary>
    /// Font size used for headings of level 2 in pt
    /// </summary>
    public double HeadingFontSize2 { get; set; }

    /// <summary>
    /// Font size used for headings of level 3 in pt
    /// </summary>
    public double HeadingFontSize3 { get; set; }

    /// <summary>
    /// Font size used for headings of level 4 in pt
    /// </summary>
    public double HeadingFontSize4 { get; set; }

    /// <summary>
    /// Font size used for headings of level 5 in pt
    /// </summary>
    public double HeadingFontSize5 { get; set; }

    /// <summary>
    /// Line height in cm (Zeilenabstand ZAB in cm)
    /// </summary>
    public double LineHeight { get; set; }

    /// <summary>
    /// Width of the column divider in cm (Spaltenabstand in cm)
    /// </summary>
    public double GridColumnDividerWidth { get; set; }

    /// <summary>
    /// Column width in cm
    /// </summary>
    public double GridColumnWidth { get; set; }

    /// <summary>
    /// Sets the factor for the calculation of the left margin. See <see cref="ITypography.SetMargins"/> for details
    /// </summary>
    public double MarginLeftFactor { get; set; }

    /// <summary>
    /// Sets the factor for the calculation of the right margin. See <see cref="ITypography.SetMargins"/> for details
    /// </summary>
    public double MarginRightFactor { get; set; }

    /// <summary>
    /// Sets the factor for the calculation of the top margin. See <see cref="ITypography.SetMargins"/> for details
    /// </summary>
    public double MarginTopFactor { get; set; }

    /// <summary>
    /// Sets the factor for the calculation of the bottom margin. See <see cref="ITypography.SetMargins"/> for details
    /// </summary>
    public double MarginBottomFactor { get; set; }

    /// <summary>
    /// Number of grid columns to use for the layout of a column in the type area
    /// </summary>
    public int GridColumnCount { get; set; }

    /// <summary>
    /// Unit used for margin calculations in cm (Teil in cm)
    /// 
    /// <see cref="MarginUnit"/> = PaperSize.Size.Width - TypeAreaRect.Size.Width / (<see cref="SetMargins"/>.left + <see cref="SetMargins"/>.right)
    /// </summary>
    public double MarginUnit { get; private set; }

    /// <summary>
    /// Current margins in cm
    /// </summary>
    public TypoThickness TypoMargins { get; set; }

    /// <summary>
    /// Height of the page header in cm
    /// </summary>
    public double PageHeaderHeight { get; set; }

    /// <summary>
    /// Margin between page header and type area in cm
    /// </summary>
    public double PageHeaderMargin { get; set; }

    /// <summary>
    /// Height of the page header in cm
    /// </summary>
    public double PageFooterHeight { get; set; }

    /// <summary>
    /// Margin between page header and type area in cm
    /// </summary>
    public double PageFooterMargin { get; set; }

    /// <summary>
    /// Dots per inch (for converting to pixels). Default: 96dpi
    /// </summary>
    public double DotsPerInch { get; set; }

    /// <summary>
    /// Coordinates of the vertical lines of the typo grid
    /// </summary>
    public double[] VerticalLines { get; private set; }

    /// <summary>
    /// Create all vertical lines for the typographic grid
    /// </summary>
    public void CalculateVerticalLines()
    {
        var numberOfLines = GridColumnCount * 2;

        VerticalLines = new double[numberOfLines];

        for (var i = 0; i < GridColumnCount; i++)
        {
            VerticalLines[i * 2] = TypoMargins.Left + i * (GridColumnWidth + GridColumnDividerWidth);
            VerticalLines[i * 2 + 1] = TypoMargins.Left + i * (GridColumnWidth + GridColumnDividerWidth) + GridColumnWidth;
        }
    }

    /// <summary>
    /// Path to logo to print in the page header
    /// </summary>
    public string LogoPath { get; set; }

    /// <summary>
    /// Width of the logo in the page header in cm
    /// </summary>
    public double LogoWidth { get; set; }

    /// <summary>
    /// Styling for charts in the document
    /// </summary>
    public ChartStyle ChartStyle { get; set; }

    /// <summary>
    /// Color for table header background
    /// </summary>
    public TypoColor TableHeaderBackground { get; set; }

    /// <summary>
    /// Color for table body background
    /// </summary>
    public TypoColor TableBodyBackground { get; set; }

    /// <summary>
    /// Color for table header background
    /// </summary>
    public TypoColor TableHeaderUnborderedBackground { get; set; }

    /// <summary>
    /// Color for table body background
    /// </summary>
    public TypoColor TableBodyUnborderedBackground { get; set; }


    /// <summary>
    /// Corner radius of a table in cm
    /// </summary>
    public double TableCornerRadius { get; set; }

    #region Methods

    /// <summary>
    /// Set ratios used to calculate margins. Margins are calculate as follows:
    /// 
    /// <see cref="MarginUnit"/> = PaperSize.Size.Width - TypeAreaRect.Size.Width / (<see cref="MarginLeftFactor"/> + <see cref="MarginRightFactor"/>)
    /// 
    /// Margins.Left  = <see cref="MarginLeftFactor"/> * <see cref="MarginUnit"/>
    /// Margins.Right  = <see cref="MarginRightFactor"/> * <see cref="MarginUnit"/>
    /// Margins.Top  = <see cref="MarginTopFactor"/> * <see cref="MarginUnit"/>
    /// Margins.Bottom  = <see cref="MarginBottomFactor"/>* <see cref="MarginUnit"/>
    /// 
    /// </summary>
    public void SetMargins()
    {
        var typeAreaWidth = GridColumnCount * GridColumnWidth + (GridColumnCount - 1) * GridColumnDividerWidth;

        var mu = TypoPaperFormat.Size.Width - typeAreaWidth;

        MarginUnit = mu / (MarginLeftFactor + MarginRightFactor);

        TypoMargins.Left = MarginLeftFactor * MarginUnit;
        TypoMargins.Right = MarginRightFactor * MarginUnit;
        TypoMargins.Top = MarginTopFactor * MarginUnit;
        TypoMargins.Bottom = MarginBottomFactor * MarginUnit;

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

        // Effective column width
        _columnWidth = (TypeAreaRect.Size.Width - (NumberOfColumns - 1) * Space) / NumberOfColumns;
    }

    /// <summary>
    /// Get the width of a landscape element which should a certain number of layout columns in cm
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <returns></returns>
    public double GetWidth(int numberOfColumnsUsed)
    {
        return numberOfColumnsUsed * GridColumnWidth + (numberOfColumnsUsed - 1) * GridColumnDividerWidth;
    }

    /// <summary>
    /// Get the height of an element using the Golden Schnitt ratio in cm
    /// </summary>
    /// <param name="numberOfColumnsUsed"></param>
    /// <param name="landscape"></param>
    /// <returns></returns>
    public double GetHeight(int numberOfColumnsUsed, bool landscape = true)
    {
        return landscape
            ? Math.Round((numberOfColumnsUsed * GridColumnWidth + (numberOfColumnsUsed - 1) * GridColumnDividerWidth) /
                         TypographicConstants.GoldenerSchnittRatio, 2)
            : Math.Round((numberOfColumnsUsed * GridColumnWidth + (numberOfColumnsUsed - 1) * GridColumnDividerWidth) *
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
    /// <param name="numberOfColumnsUsed">Number of columns used</param>
    /// <param name="landscape">Landscape?</param>
    /// <returns>Height in pixels</returns>
    public int GetPixelHeight(int numberOfColumnsUsed, bool landscape = true)
    {
        return (int)(GetHeight(numberOfColumnsUsed, landscape) * TypographicConstants.InchPerCentimeter * DotsPerInch);
    }

    #endregion

}
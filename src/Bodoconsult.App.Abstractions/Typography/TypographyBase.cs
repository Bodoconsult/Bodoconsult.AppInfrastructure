// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using System.Globalization;

namespace Bodoconsult.App.Abstractions.Typography;

/// <summary>
/// Base class for typography data
/// </summary>
public class TypographyBase : BaseTypoPageStyle, ITypography
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public TypographyBase()
    {
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
    /// Language code like en or de (only first 2 letters needed). Default: de
    /// </summary>
    public string CurrentLanguage
    {
        get;
        set
        {
            field = value;
            CultureInfo = new CultureInfo(value);
        }
    } = "en";

    /// <summary>
    /// Current culture info
    /// </summary>
    public CultureInfo CultureInfo { get; set; } = new("en");

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
    /// Name(s) of the author(s)
    /// </summary>
    public string Authors { get; set; } = "Authors";

    /// <summary>
    /// Company name
    /// </summary>
    public string Company { get; set; } = "Company";

    /// <summary>
    /// Company website
    /// </summary>
    public string? CompanyWebsite { get; set; }

    /// <summary>
    /// Table of content (TOC) heading
    /// </summary>
    public string TocHeading { get; set; } = "Table of content";

    /// <summary>
    /// Table of figures (TOF) heading
    /// </summary>
    public string TofHeading { get; set; } = "Table of figures";

    /// <summary>
    /// Table of equations (TOE) heading
    /// </summary>
    public string ToeHeading { get; set; } = "Table of equations";

    /// <summary>
    /// Table of tables (TOT) heading
    /// </summary>
    public string TotHeading { get; set; } = "Table of tables";

    /// <summary>
    /// Default font name
    /// </summary>
    public string FontName { get; set; } = "Arial";

    /// <summary>
    /// Default font size in pt
    /// </summary>
    public double FontSize { get; set; } = 11;

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
    /// Path to logo to print in the page header
    /// </summary>
    public string? LogoPath { get; set; }

    /// <summary>
    /// Width of the logo in the page header in cm
    /// </summary>
    public double LogoWidth { get; set; }

    /// <summary>
    /// Title of the document
    /// </summary>
    public string Title { get; set; } = "Your title";

    /// <summary>
    /// Document description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Keywords separated by comma
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    /// Footer text
    /// </summary>
    public string? FooterText { get; set; }

    /// <summary>
    /// Header text
    /// </summary>
    public string? HeaderText { get; set; }

    /// <summary>
    /// Defines a template for the header. Segments left, middle or right are separated by pipe.
    /// Use the following indicators to position these elements in the left, middle or right segment:
    /// - ITypography.PageFieldIndicator for a page number
    /// - ITypography.CompanyIndicator for the company name
    /// - ITypography.TextIndicator for header or footer text or if missing the title
    /// - ITypography.LogoIndicator for a logo
    /// - ITypography.DateIndicator for the current date
    /// - ITypography.DateTimeIndicator for the current date and time
    /// </summary>
    public string HeaderTemplate { get; set; } = "<<text>>||<<logo>>";

    /// <summary>
    /// Defines a template for the header. Segments left, middle or right are separated by pipe.
    /// Use the following indicators to position these elements in the left, middle or right segment:
    /// - ITypography.PageFieldIndicator for a page number
    /// - ITypography.CompanyIndicator for the company name
    /// - ITypography.TextIndicator for header or footer text or if missing the title
    /// - ITypography.LogoIndicator for a logo
    /// - ITypography.DateIndicator for the current date
    /// - ITypography.DateTimeIndicator for the current date and time
    /// </summary>
    public string FooterTemplate { get; set; } = "<<text>>|<<date>>|<<page>>";

    /// <summary>
    /// Text like page or Seite to write in front of the page number in the footer
    /// </summary>
    public string PageNumberPrefix { get; set; } = "Page";

    /// <summary>
    /// Path to the background image or null if no background image should be used. For a A4 portrait page the size of the image should be 1400 px x 1980 px
    /// </summary>
    public string? BackgroundImagePath { get; set; }

    /// <summary>
    /// Watermark text
    /// </summary>
    public string? WatermarkText { get; set; }

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
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for typographic data. Coordinates system origin is in the left top corner. X-axis going to right. Y-axis going down.
/// </summary>
public interface ITypography: ITypoPageStyle, ITypoMetaData
{
    /// <summary>
    /// Indicator where to place the page number (or the field for it) in a footer text or header text
    /// </summary>
    public const string PageFieldIndicator = "<<page>>";

    /// <summary>
    /// Indicator where to place the text of a header or footer
    /// </summary>
    public const string TextIndicator = "<<text>>";

    /// <summary>
    /// Indicator where to place the logo in a header or footer
    /// </summary>
    public const string LogoIndicator = "<<logo>>";

    /// <summary>
    /// Indicator where to place the logo in a header or footer
    /// </summary>
    public const string CompanyIndicator = "<<company>>";

    /// <summary>
    /// Indicator where to place the logo in a header or footer
    /// </summary>
    public const string DateIndicator = "<<date>>";

    /// <summary>
    /// Indicator where to place the logo in a header or footer
    /// </summary>
    public const string DateTimeIndicator = "<<datetime>>";

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
    /// Styling for charts in the document
    /// </summary>
    ChartStyle ChartStyle { get; set; }

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
}
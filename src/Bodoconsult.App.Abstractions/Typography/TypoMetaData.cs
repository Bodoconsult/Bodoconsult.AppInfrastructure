// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using System.Globalization;

namespace Bodoconsult.App.Abstractions.Typography;

/// <summary>
/// Document metadata
/// </summary>
public class TypoMetaData: ITypoMetaData
{
    private string _currentLanguage = "en";

    /// <summary>
    /// Language code like en or de (only first 2 letters needed). Default: en
    /// </summary>
    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            _currentLanguage = value;
            CultureInfo = new CultureInfo(value);
        }
    }

    /// <summary>
    /// Current culture info
    /// </summary>
    public CultureInfo CultureInfo { get; set; } = new("en");

    /// <summary>
    /// Copyright to print in charts and other items
    /// </summary>
    public string Copyright { get; set; }

    /// <summary>
    /// Name(s) of the author(s)
    /// </summary>
    public string Authors { get; set; }

    /// <summary>
    /// Company name
    /// </summary>
    public string Company { get; set; }

    /// <summary>
    /// Company website
    /// </summary>
    public string CompanyWebsite { get; set; }

    /// <summary>
    /// Path to logo to print in the page header
    /// </summary>
    public string LogoPath { get; set; }

    /// <summary>
    /// Width of the logo to print in the page header in cm. Default: 2cm
    /// </summary>
    public double LogoWidth { get; set; } = 2;

    /// <summary>
    /// Title of the document
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Document description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Keywords separated by comma
    /// </summary>
    public string Keywords { get; set; }

    /// <summary>
    /// Footer text
    /// </summary>
    public string FooterText { get; set; }

    /// <summary>
    /// Header text
    /// </summary>
    public string HeaderText { get; set; }


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
    public string FooterTemplate { get; set; } = "<<company>>|<<date>>|<<page>>";

    /// <summary>
    /// Text like page or Seite to write in front of the page number
    /// </summary>
    public string PageNumberPrefix { get; set; } = "Page";

    /// <summary>
    /// Path to the background image or null if no background image should be used. For a A4 portrait page the size of the image should be 1400 px x 1980 px
    /// </summary>
    public string BackgroundImagePath { get; set; }

    /// <summary>
    /// Watermark text
    /// </summary>
    public string WatermarkText { get; set; }
}
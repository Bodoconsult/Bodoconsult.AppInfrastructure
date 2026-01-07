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
    /// Defines a template for the header.
    /// Use ITypography.PageFieldIndicator, ITypography.CompanyIndicator, ITypography.TextIndicator and ITypography.LogoIndicator to position these elements in the left, middle or right segment.
    /// Segments separated by pipe.
    /// </summary>
    public string HeaderTemplate { get; set; } = "<<text>>||<<logo>>";

    /// <summary>
    /// Defines a template for the header.
    /// Use ITypography.PageFieldIndicator, ITypography.TextIndicator, ITypography.CompanyIndicator and ITypography.LogoIndicator to position these elements in the left, middle or right segment.
    /// Segments separated by pipe.
    /// </summary>
    public string FooterTemplate { get; set; } = "<<company>>||<<page>>";

    /// <summary>
    /// Text like page or Seite to write in front of the page number in the footer
    /// </summary>
    public string FooterPageText { get; set; } = "Page";
}
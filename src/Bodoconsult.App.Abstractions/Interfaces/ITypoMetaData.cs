// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for document metadata
/// </summary>
public interface ITypoMetaData
{
    /// <summary>
    /// Language code like en or de (only first 2 letters needed). Default: de
    /// </summary>
    public string CurrentLanguage { get; set; }

    /// <summary>
    /// Current culture info
    /// </summary>
    public CultureInfo CultureInfo { get; set; }

    /// <summary>
    /// Copyright to print in charts and other items
    /// </summary>
    string Copyright { get; set; }

    /// <summary>
    /// Name(s) of the author(s)
    /// </summary>
    string Authors { get; set; }

    /// <summary>
    /// Company name
    /// </summary>
    string Company { get; set; }

    /// <summary>
    /// Company website
    /// </summary>
    string CompanyWebsite { get; set; }

    /// <summary>
    /// Path to logo to print in the page header
    /// </summary>
    string LogoPath { get; set; }

    /// <summary>
    /// Width of the logo to print in the page header in cm
    /// </summary>
    double LogoWidth { get; set; }

    /// <summary>
    /// Title of the document
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Document description
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// Keywords separated by comma
    /// </summary>
    string Keywords { get; set; }

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
    string HeaderTemplate { get; set; }

    /// <summary>
    /// Defines a template for the header.
    /// Use ITypography.PageFieldIndicator, ITypography.TextIndicator, ITypography.CompanyIndicator and ITypography.LogoIndicator to position these elements in the left, middle or right segment.
    /// Segments separated by pipe.
    /// </summary>
    string FooterTemplate { get; set; }

    /// <summary>
    /// Text like page or Seite to write in front of the page number in the footer
    /// </summary>
    string FooterPageText { get; set; }
}
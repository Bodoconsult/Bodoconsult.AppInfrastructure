// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Abstractions.Typography;

/// <summary>
/// Document metadata
/// </summary>
public class TypoMetaData: ITypoMetaData
{
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
}
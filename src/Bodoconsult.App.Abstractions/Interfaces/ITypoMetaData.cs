// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for document metadata
/// </summary>
public interface ITypoMetaData
{
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

}
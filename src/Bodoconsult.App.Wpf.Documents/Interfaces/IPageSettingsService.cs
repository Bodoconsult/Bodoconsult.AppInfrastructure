// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Documents.Delegates;

namespace Bodoconsult.App.Wpf.Documents.Interfaces;

/// <summary>
/// Interface for page settings used for paginator
/// </summary>
public interface IPageSettingsService
{
    /// <summary>
    /// Page size in DIUs
    /// </summary>
    Size PageSize { get; set; }

    /// <summary>
    /// The origin of the content
    /// </summary>
    Point ContentOrigin { get; }

    /// <summary>
    /// Current content size
    /// </summary>
    Size ContentSize { get; }

    ///<summary>
    /// Repeat table headers? Default: false
    ///</summary>
    bool RepeatTableHeaders { get; set; }

    /// <summary>
    /// The defined header area
    /// </summary>
    Rect HeaderRect { get; }

    /// <summary>
    /// The defined footer area
    /// </summary>
    Rect FooterRect { get; }


    #region Metadata

    /// <summary>
    /// Document metadata 
    /// </summary>
    ITypoMetaData DocumentMetaData { get; set; }

    #endregion

    #region Delegates for drawing main page sections

    /// <summary>
    /// Delegate to print a header to the document page
    /// </summary>
    DrawSectionDelegate DrawHeaderDelegate { get; set; }

    /// <summary>
    /// Delegate to print a footer to the document page
    /// </summary>
    DrawSectionDelegate DrawFooterDelegate { get; set; }

    /// <summary>
    /// Font name to use for header
    /// </summary>
    string HeaderFontName { get; set; }

    /// <summary>
    /// Header font size
    /// </summary>
    double HeaderFontSize { get; set; }

    /// <summary>
    /// Font name to use for footer
    /// </summary>
    string FooterFontName { get; set; }

    /// <summary>
    /// Footer font size
    /// </summary>
    double FooterFontSize { get; set; }

    /// <summary>
    /// Page number format for TOC, TOE, TOF and TOT sections
    /// </summary>
    PageNumberFormatEnum TocPageNumberFormat { get; set; }

    /// <summary>
    /// Page number format for content sections
    /// </summary>
    PageNumberFormatEnum ContentPageNumberFormat { get; set; }

    #endregion
}
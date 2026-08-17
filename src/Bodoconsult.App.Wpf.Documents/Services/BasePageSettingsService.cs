// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Documents.Delegates;
using Bodoconsult.App.Wpf.Documents.Interfaces;
using PropertyChanged;
using Thickness = System.Windows.Thickness;

namespace Bodoconsult.App.Wpf.Documents.Services;

/// <summary>
/// Base class used for WPF page settings
/// </summary>
[AddINotifyPropertyChangedInterface]
public abstract class BasePageSettingsService : IPageSettingsService
{
    #region Page settings

    /// <summary>
    /// Page size in DIUs
    /// </summary>
    public Size PageSize { get; set; }

    /// <summary>
    /// Page margins in DIUs
    /// </summary>
    public Thickness Margins { get; set; }

    /// <summary>
    /// Space reserved for the header in DIUs
    /// </summary>
    public double HeaderHeight { get; set; }

    /// <summary>
    /// Bottom margin of the header in DIUs
    /// </summary>
    public double HeaderMarginBottom { get; set; }

    /// <summary>
    /// Space reserved for the footer in DIUs
    /// </summary>
    public double FooterHeight { get; set; } = 25;

    /// <summary>
    /// Margin in footer above the footer text and below the main text in DIUs
    /// </summary>
    public double FooterMarginTop { get; set; } = 14;

    #endregion

    #region Metadata

    /// <summary>
    /// Document metadata 
    /// </summary>
    public ITypoMetaData DocumentMetaData { get; set; }

    #endregion

    #region Delegates for drawing main page sections

    /// <summary>
    /// Delegate to print a header to the document page
    /// </summary>
    public DrawSectionDelegate DrawHeaderDelegate { get; set; }

    /// <summary>
    /// Delegate to print a footer to the document page
    /// </summary>
    public DrawSectionDelegate DrawFooterDelegate { get; set; }

    #endregion


    #region Important measures for page sections calculate from page settings

    /// <summary>
    /// Current content size
    /// </summary>
    public Size ContentSize
    {
        get
        {
            var size = new Size(PageSize.Width - Margins.Left - Margins.Right,
                PageSize.Height - (Margins.Top + Margins.Bottom + HeaderHeight + FooterHeight));

            return size;
        }
    }

    /// <summary>
    /// The origin of the content
    /// </summary>
    public Point ContentOrigin =>
        new(
            Margins.Left,
            Margins.Top + HeaderRect.Height
        );

    /// <summary>
    /// The defined header area
    /// </summary>
    public Rect HeaderRect =>
        new(
            Margins.Left, Margins.Top,
            ContentSize.Width, HeaderHeight
        );

    /// <summary>
    /// The defined footer area
    /// </summary>
    public Rect FooterRect =>
        new(
            Margins.Left, ContentOrigin.Y + ContentSize.Height,
            ContentSize.Width, FooterHeight
        );

    #endregion


    ///<summary>
    /// Repeat table headers? Default: false
    ///</summary>
    public bool RepeatTableHeaders { get; set; }

    /// <summary>
    /// Font name to use for header
    /// </summary>
    public string HeaderFontName { get; set; } = "Aptos";

    /// <summary>
    /// Header font size
    /// </summary>
    public double HeaderFontSize { get; set; } = 8;

    /// <summary>
    /// Font name to use for footer
    /// </summary>
    public string FooterFontName { get; set; } = "Aptos";

    /// <summary>
    /// Footer font size
    /// </summary>
    public double FooterFontSize { get; set; } = 8;

    /// <summary>
    /// Text like page or Seite to write in front of the page number in the footer
    /// </summary>
    public string FooterPageText { get; set; } = "Page";

    /// <summary>
    /// Page number format for TOC, TOE, TOF and TOT sections
    /// </summary>
    public PageNumberFormatEnum TocPageNumberFormat { get; set; } = PageNumberFormatEnum.UpperRoman;

    /// <summary>
    /// Page number format for content sections
    /// </summary>
    public PageNumberFormatEnum ContentPageNumberFormat { get; set; } = PageNumberFormatEnum.Decimal;

}
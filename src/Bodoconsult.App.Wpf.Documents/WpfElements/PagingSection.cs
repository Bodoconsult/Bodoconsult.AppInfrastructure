// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Section = System.Windows.Documents.Section;

namespace Bodoconsult.App.Wpf.Documents.WpfElements;

/// <summary>
/// A class derived from <see cref="System.Windows.Documents.Section"/> to allowed more complex paging features in a paginator
/// </summary>
public class PagingSection: Section
{
    /// <summary>
    /// Do not include the current section in the numbering for TOC, TOE and TOF
    /// </summary>
    public bool DoNotIncludeInNumbering { get; set; }

    /// <summary>
    /// Is a header required? Default: true
    /// </summary>
    public bool IsHeaderRequired { get; set; } = true;

    /// <summary>
    /// Is a footer required? Default: true
    /// </summary>
    public bool IsFooterRequired { get; set; } = true;

    /// <summary>
    /// Is a restart of the page numbering required? Default: false
    /// </summary>
    public bool IsRestartPageNumberingRequired { get; set; } = false;

    /// <summary>
    /// Page number format
    /// </summary>
    public PageNumberFormatEnum PageNumberFormat { get; set; } = PageNumberFormatEnum.Decimal;

}
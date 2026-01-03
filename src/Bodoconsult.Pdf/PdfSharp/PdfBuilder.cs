// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.Stylesets;
using PdfSharp.Fonts;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// Class representing a single-column textPDF document and basic functionality to add content to it. Adjust to use with LDML. Does not create TOC etc. automatically
/// </summary>
public class PdfBuilder : PdfBuilderBase
{
    #region Constructors

    /// <summary>
    /// Default ctor to load a complete styleset
    /// </summary>
    /// <param name="styleSet">Styleset to use</param>
    /// <param name="fontResolver">Font resolver to load</param>
    public PdfBuilder(IStyleSet styleSet, IFontResolver fontResolver)
    {
        LoadDefaults();

        GlobalFontSettings.FontResolver ??= fontResolver;

        LoadStyleset(styleSet);
    }

    #endregion

}
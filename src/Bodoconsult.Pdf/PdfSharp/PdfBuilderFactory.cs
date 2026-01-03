// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.Interfaces;
using Bodoconsult.Pdf.Stylesets;
using PdfSharp.Fonts;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// Current implementation of <see cref="IPdfBuilder"/> delivering an instance of <see cref="PdfBuilder"/> for a single-column text PDF file to create or an instance of
/// <see cref="MultiColumnPdfBuilder"/> for a multi-column text PDF file to create
/// </summary>
public class PdfBuilderFactory : IPdfBuilderFactory
{
    private readonly IFontResolver _fontResolver;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="fontResolver">Current font resolver instance</param>
    public PdfBuilderFactory(IFontResolver fontResolver)
    {
        _fontResolver = fontResolver;
    }

    /// <summary>
    /// Create an instance of <see cref="IPdfBuilder"/> for creating a PDF file
    /// </summary>
    /// <param name="styleSet">Current styleset to use for the new PDF file</param>
    /// <returns>Instance of <see cref="IPdfBuilder"/> for creating a PDF file</returns>
    public IPdfBuilder CreateInstance(IStyleSet styleSet)
    {
        if (styleSet.NumberOfColumns > 1)
        {
            return new MultiColumnPdfBuilder(styleSet, _fontResolver);
        }

        return new PdfBuilder(styleSet, _fontResolver);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using PdfSharp;

namespace Bodoconsult.Pdf.Extensions;

/// <summary>
/// Extensions for <see cref="TypoPaperFormat"/>
/// </summary>
public static class TypoPaperFormatExtensions
{
    /// <summary>
    ///  Convert a <see cref="TypoColor"/> instance to a PDF page size
    /// </summary>
    /// <param name="typoColor"><see cref="TypoPaperFormat"/> instance to convert</param>
    /// <returns>PDF page size</returns>
    public static PageSize ToPageSize(this TypoPaperFormat typoColor)
    {
        switch (typoColor.PaperFormatName.ToLowerInvariant())
        {
            case "a0":
                return PageSize.A0;
            case "a1":
                return PageSize.A1;
            case "a2":
                return PageSize.A2;
            case "a3":
                return PageSize.A3;
            case "a5":
                return PageSize.A5;
            case "letter":
                return PageSize.Letter;
            case "legal":
                return PageSize.Legal;
            case "a4":
            default:
                return PageSize.A4;
        }
    }

}
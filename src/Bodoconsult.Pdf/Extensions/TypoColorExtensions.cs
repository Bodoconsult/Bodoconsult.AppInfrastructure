// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using MigraDoc.DocumentObjectModel;

namespace Bodoconsult.Pdf.Extensions;

/// <summary>
/// Extensions for <see cref="TypoColor"/>
/// </summary>
public static class TypoColorExtensions
{
    /// <summary>
    ///  Convert a <see cref="TypoColor"/> instance to a PDF color
    /// </summary>
    /// <param name="typoColor"><see cref="TypoColor"/> instance to convert</param>
    /// <returns>PDF color</returns>
    public static Color ToPdfColor(this TypoColor typoColor)
    {
        return new Color(typoColor.A, typoColor.R, typoColor.G, typoColor.B);
    }

}
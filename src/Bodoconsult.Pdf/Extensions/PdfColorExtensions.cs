// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using MigraDoc.DocumentObjectModel;
using System;

namespace Bodoconsult.Pdf.Extensions;

/// <summary>
/// Extensions for getting <see cref="Color"/> as result
/// </summary>
public static class PdfColorExtensions
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

    /// <summary>
    /// Get a color from HTML color string with 7 or 9 chars length like #000000  or #00000000
    /// </summary>
    /// <param name="htmlColor">HTML color string with 7 or 9 chars length</param>
    /// <returns>Color or null</returns>
    public static Color FromHtml(this string htmlColor)
    {
        Color color;

        byte r;
        byte g;
        byte b;

        if (htmlColor.Length == 7)
        {
            r = Convert.ToByte(htmlColor.Substring(1, 2), 16);
            g = Convert.ToByte(htmlColor.Substring(3, 2), 16);
            b = Convert.ToByte(htmlColor.Substring(5, 2), 16);

            color = Color.FromRgb(r, g, b);
            return color;
        }

        var a = Convert.ToByte(htmlColor.Substring(1, 2), 16);
        r = Convert.ToByte(htmlColor.Substring(3, 2), 16);
        g = Convert.ToByte(htmlColor.Substring(5, 2), 16);
        b = Convert.ToByte(htmlColor.Substring(7, 2), 16);

        color = Color.FromArgb(a, r, g, b);
        return color;
    }
}

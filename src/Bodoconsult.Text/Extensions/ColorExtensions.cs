// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Extensions;

/// <summary>
/// Extenion methods for <see cref="Color"/>
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Get an HTML string like #000000 for a color
    /// </summary>
    /// <param name="color">Current color</param>
    /// <returns>HTML color string like #000000</returns>
    public static string ToHtml(this Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Get a LDML color like #000000 for a color
    /// </summary>
    /// <param name="color">Current color</param>
    /// <returns>HTML color string like #000000</returns>
    public static Color ToLdmlColor(this TypoColor color)
    {
        return new Color(color.A, color.R, color.G, color.B);
    }

    /// <summary>
    /// Get an HTML string like #000000 for a color
    /// </summary>
    /// <param name="color">Current color</param>
    /// <returns>HTML color string like #000000</returns>
    public static MigraDoc.DocumentObjectModel.Color ToPdfColor(this Color color)
    {
        return MigraDoc.DocumentObjectModel.Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
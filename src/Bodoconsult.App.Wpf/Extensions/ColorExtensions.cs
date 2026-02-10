// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Media;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Wpf.Extensions;

/// <summary>
/// Extension methods for <see cref="Color"/>
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
        return color.A == 255 ?
            $"#{color.R:X2}{color.G:X2}{color.B:X2}" :
            $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Get an HTML string like #000000 for a color
    /// </summary>
    /// <param name="color">Current color</param>
    /// <returns>HTML color string like #000000</returns>
    public static TypoColor ToTypoColor(this Color color)
    {
        return TypoColor.FromArgb(color.A, color.R, color.G, color.B);
    }

    /// <summary>
    /// Does this color
    /// </summary>
    /// <param name="color">Current WPF color</param>
    /// <param name="other"><see cref="TypoColor"/> to compare</param>
    /// <returns>True if both colors are same</returns>
    public static bool IsEqualTo(this Color color, TypoColor other)
    {
        return color.A == other.A && color.R == other.R && color.G == other.G && color.B == other.B;
    }
}
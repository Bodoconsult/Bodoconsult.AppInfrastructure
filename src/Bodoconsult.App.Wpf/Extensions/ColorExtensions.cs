// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Media;

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
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Abstractions.Extensions;

/// <summary>
/// Extenion methods for <see cref="TypoColor"/>
/// </summary>
public static class TypoColorExtensions
{
    /// <summary>
    /// Get an HTML string like #000000 for a color
    /// </summary>
    /// <param name="color">Current color</param>
    /// <returns>HTML color string like #000000</returns>
    public static string ToHtml(this TypoColor color)
    {
        return color.A == 255 ? 
            $"#{color.R:X2}{color.G:X2}{color.B:X2}" : 
            $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Get an HTML string like #000000 for a color without leading #
    /// </summary>
    /// <param name="color">Current color</param>
    /// <returns>HTML color string like #000000</returns>
    public static string ToHtml2(this TypoColor color)
    {
        return color.A == 255 ?
            $"{color.R:X2}{color.G:X2}{color.B:X2}" :
            $"{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Converts a <see cref="TypoColor"/> to a premultiplied Int32 - 4 byte ARGB structure.
    /// </summary>
    /// <param name="color">The color to convert</param>
    /// <returns>The ARGB int representation of the color</returns>
    public static int ToArgbInt(this TypoColor color)
    {
        var a = color.A + 1;
        var col = (color.A << 24) | ((byte)((color.R * a) >> 8) << 16) | ((byte)((color.G * a) >> 8) << 8) | (byte)((color.B * a) >> 8);
        return col;
    }

    /// <summary>
    /// Converts a <see cref="TypoColor"/> to an int value
    /// </summary>
    /// <param name="color">The color to convert</param>
    /// <returns>The int representation of the color</returns>
    public static int ToInt(this TypoColor color)
    {
        ToRgba(color, out var r, out var g, out var b, out var a);
        var argb = a << 24 | r << 16 | g << 8 | b;
        return argb;
    }

    /// <summary>
    /// Converts a <see cref="TypoColor"/> to an RGB int value
    /// </summary>
    /// <param name="color">The color to convert</param>
    /// <returns>The int representation of the color</returns>
    public static int ToRgbInt(this TypoColor color)
    {
        var rgb = (color.R << 16) | (color.G << 8) | color.B;
        return rgb;
    }

    private static void ToRgba(TypoColor color, out byte r, out byte g, out byte b, out byte a)
    {
        a = (byte)(color.A * 255f);
        r = (byte)(color.R * 255f);
        g = (byte)(color.G * 255f);
        b = (byte)(color.B * 255f);
    }
}
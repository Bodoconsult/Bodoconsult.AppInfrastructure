// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Media;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Wpf.Extensions;

/// <summary>
/// Extension methods for <see cref="TypoColor"/>
/// </summary>
public static class TypoColorExtensions
{
    /// <summary>
    /// Get an WPF color from a <see cref="TypoColor"/> instance
    /// </summary>
    /// <param name="color">Current <see cref="TypoColor"/> instance</param>
    /// <returns>WPF color</returns>
    public static Color ToColor(this TypoColor color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    /// <summary>
    /// Does this color
    /// </summary>
    /// <param name="color">Current WPF color</param>
    /// <param name="other"><see cref="TypoColor"/> to compare</param>
    /// <returns>True if both colors are same</returns>
    public static bool IsEqualTo(this TypoColor color, Color other)
    {
        return color.A == other.A && color.R == other.R && color.G == other.G && color.B == other.B;
    }
}
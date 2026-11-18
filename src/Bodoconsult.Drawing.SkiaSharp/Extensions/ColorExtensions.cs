// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using SkiaSharp;

namespace Bodoconsult.Drawing.SkiaSharp.Extensions;

/// <summary>
/// Color extension methods
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Convert a <see cref="TypoColor"/> instance to a <see cref="SKColor"/> instance
    /// </summary>
    /// <param name="color"><see cref="TypoColor"/> instance</param>
    /// <returns><see cref="SKColor"/> instance</returns>
    // ReSharper disable once InconsistentNaming
    public static SKColor ToSKColor(this TypoColor color)
    {
        return new SKColor(color.R, color.G, color.B, color.A);
    }
}
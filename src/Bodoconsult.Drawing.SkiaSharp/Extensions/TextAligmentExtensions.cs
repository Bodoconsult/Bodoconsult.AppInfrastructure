// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.App.Abstractions.Interfaces;
using SkiaSharp;

namespace Bodoconsult.Drawing.SkiaSharp.Extensions;

/// <summary>
/// Text alignment extension methods
/// </summary>
public static class TextAligmentExtensions
{
    /// <summary>
    /// Convert a <see cref="TypoColor"/> instance to a <see cref="SKTextAlign"/> instance
    /// </summary>
    /// <param name="alignment">Text alignment</param>
    /// <returns><see cref="SKTextAlign"/> instance</returns>
    // ReSharper disable once InconsistentNaming
    public static SKTextAlign ToSKTextAlign(this TypoTextAlignment alignment)
    {
        switch (alignment)
        {
            case TypoTextAlignment.Left:
                return SKTextAlign.Left;
            case TypoTextAlignment.Center:
                return SKTextAlign.Center;
            case TypoTextAlignment.Justify:
                return SKTextAlign.Left;
            case TypoTextAlignment.Right:
                return SKTextAlign.Right;
            default:
                throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
        }
    }
}
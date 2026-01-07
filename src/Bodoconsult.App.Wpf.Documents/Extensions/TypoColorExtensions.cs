// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Media;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Wpf.Documents.Extensions;

/// <summary>
/// WPF relevante extensions for <see cref="TypoColor"/>
/// </summary>
public static class TypoColorExtensions
{
    /// <summary>
    /// Convert <see cref="TypoColor"/> to <see cref="Color"/>
    /// </summary>
    /// <param name="color">Typo color</param>
    /// <returns>Returns WPF color</returns>
    public static Color ToWpfColor(this TypoColor color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// A brush using a solid color
/// </summary>
public class TypoSolidColorBrush : TypoBrush
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public TypoSolidColorBrush()
    { }

    /// <summary>
    /// Ctor to load a Color
    /// </summary>
    /// <param name="color"></param>
    public TypoSolidColorBrush(TypoColor color)
    {
        TypoColor = color;
    }

    /// <summary>
    /// Ctor with a HTML color index
    /// </summary>
    /// <param name="color"></param>
    public TypoSolidColorBrush(string color)
    {
        TypoColor = TypoColor.FromHtml(color) ?? TypoColors.Black;
    }
}
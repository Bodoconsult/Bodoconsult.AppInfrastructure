// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Text;
using Bodoconsult.Text.Extensions;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// A brush using a solid color
/// </summary>
public class SolidColorBrush : Brush
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public SolidColorBrush()
    { }

    /// <summary>
    /// Ctor to load a Color
    /// </summary>
    /// <param name="color"></param>
    public SolidColorBrush(Color color)
    {
        TypoColor = color;
    }

    /// <summary>
    /// Ctor with a HTML color index
    /// </summary>
    /// <param name="color"></param>
    public SolidColorBrush(string color)
    {
        TypoColor = Documents.Color.FromHtml(color);
    }

    /// <summary>
    /// Add the current element to a document defined in LDML (Logical document markup language)
    /// </summary>
    /// <param name="document">StringBuilder instance to create the LDML in</param>
    /// <param name="indent">Current indent</param>
    public override void ToLdmlString(StringBuilder document, string indent)
    {
        var color = (Color)TypoColor;
        document.Append($"{indent}<SolidColorBrush Color=\"{color.ToHtml()}\"/>{Environment.NewLine}");
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Extensions;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Style used for watermarks
/// </summary>
public class WatermarkStyle : ParagraphStyleBase
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public WatermarkStyle()
    {
        TagToUse = "WatermarkStyle";
        Name = TagToUse;
        FontSize = 150;
        FontColor = TypoColors.LightGray.ToLdmlColor();
    }
}
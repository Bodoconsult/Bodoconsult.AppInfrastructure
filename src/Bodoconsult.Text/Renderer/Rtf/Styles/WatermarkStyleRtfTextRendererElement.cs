// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Rtf.Styles;

/// <summary>
/// Rtf rendering element for <see cref="WatermarkStyle"/> instances
/// </summary>
public class WatermarkStyleRtfTextRendererElement : RtfParagraphStyleTextRendererElementBase
{
    private readonly ParagraphStyleBase _watermarkStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WatermarkStyleRtfTextRendererElement(WatermarkStyle watermarkStyle) : base(watermarkStyle)
    {
        _watermarkStyle = watermarkStyle;
        ClassName = "WatermarkStyle";
    }
}
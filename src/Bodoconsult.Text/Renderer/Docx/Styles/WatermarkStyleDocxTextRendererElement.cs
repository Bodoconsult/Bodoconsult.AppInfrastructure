// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Docx.Styles;

/// <summary>
/// Docx rendering element for <see cref="WatermarkStyle"/> instances
/// </summary>
public class WatermarkStyleDocxTextRendererElement : DocxParagraphStyleTextRendererElementBase
{
    private readonly WatermarkStyle _style;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WatermarkStyleDocxTextRendererElement(WatermarkStyle style) : base(style)
    {
        _style = style;
        ClassName = "WatermarkStyle";
    }
}
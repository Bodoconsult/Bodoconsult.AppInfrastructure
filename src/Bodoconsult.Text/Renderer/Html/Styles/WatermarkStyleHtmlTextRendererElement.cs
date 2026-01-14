// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Html.Styles;

/// <summary>
/// HTML rendering element for <see cref="WatermarkStyle"/> instances
/// </summary>
public class WatermarkStyleHtmlTextRendererElement : HtmlParagraphStyleTextRendererElementBase
{
    private readonly WatermarkStyle _watermarkStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WatermarkStyleHtmlTextRendererElement(WatermarkStyle watermarkStyle) : base(watermarkStyle)
    {
        _watermarkStyle = watermarkStyle;
        ClassName = "WatermarkStyle";
    }
}
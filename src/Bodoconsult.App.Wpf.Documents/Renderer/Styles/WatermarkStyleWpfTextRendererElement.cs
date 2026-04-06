// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.App.Wpf.Documents.Renderer.Styles;

/// <summary>
/// WPF rendering element for <see cref="WatermarkStyle"/> instances
/// </summary>
public class WatermarkStyleWpfTextRendererElement : WpfParagraphStyleTextRendererElementBase
{
    private readonly WatermarkStyle _style;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WatermarkStyleWpfTextRendererElement(WatermarkStyle style) : base(style)
    {
        _style = style;
        ClassName = "WatermarkStyle";
    }
}
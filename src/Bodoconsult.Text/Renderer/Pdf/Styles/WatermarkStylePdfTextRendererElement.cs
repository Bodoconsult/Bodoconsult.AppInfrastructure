// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Pdf.Styles;

/// <summary>
/// PDF rendering element for <see cref="WatermarkStyle"/> instances
/// </summary>
public class WatermarkStylePdfTextRendererElement : PdfParagraphStyleTextRendererElementBase
{
    private readonly WatermarkStyle _watermarkStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WatermarkStylePdfTextRendererElement(WatermarkStyle watermarkStyle) : base(watermarkStyle)
    {
        _watermarkStyle = watermarkStyle;
        ClassName = "WatermarkStyle";
    }
}
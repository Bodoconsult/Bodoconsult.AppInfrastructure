// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;
using System.Collections.Generic;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Figure"/> instances
/// </summary>
public class FigureDocxTextRendererElement : DocxTextRendererElementBase
{
    private readonly Figure _figure;

    /// <summary>
    /// Default ctor
    /// </summary>
    public FigureDocxTextRendererElement(Figure figure) : base(figure)
    {
        _figure = figure;
        ClassName = figure.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(DocxTextDocumentRenderer renderer)
    {
        // Get max height and with for images in cm
        StylesetHelper.GetMaxWidthAndHeightInCm(renderer.Styleset, out var maxWidth, out var maxHeight);

        StylesetHelper.GetWidthAndHeightInCm(MeasurementHelper.GetCmFromPx(_figure.OriginalWidth),
            MeasurementHelper.GetCmFromPx(_figure.OriginalHeight), maxWidth, maxHeight, out var width, out var height);

        renderer.DocxDocument.AddImage(_figure.Uri, "Image", MeasurementHelper.GetPxFromCm(width), MeasurementHelper.GetPxFromCm(height));

        var childs = new List<Inline>();

        if (!string.IsNullOrEmpty(_figure.CurrentPrefix))
        {
            childs.Add(new Span(_figure.CurrentPrefix));
        }
        childs.AddRange(_figure.ChildInlines);

        var sb = new List<OpenXmlElement>();
        DocxDocumentRendererHelper.RenderBlockInlinesToRunsForDocx(renderer, childs, sb);

        renderer.DocxDocument.AddParagraph(sb, "Figure", _figure.TagName);

    }
}
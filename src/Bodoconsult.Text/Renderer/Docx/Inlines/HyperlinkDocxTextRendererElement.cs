// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Office;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using Hyperlink = Bodoconsult.Text.Documents.Hyperlink;

namespace Bodoconsult.Text.Renderer.Docx.Inlines;

/// <summary>
/// Render a <see cref="Documents.Hyperlink"/> element
/// </summary>
public class HyperlinkDocxTextRendererElement : InlineDocxTextRendererElementBase
{
    private readonly Hyperlink _span;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="span">Hyperlink</param>
    public HyperlinkDocxTextRendererElement(Hyperlink span)
    {
        _span = span;
    }

    /// <summary>
    /// Render the inline to a run
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="runs">Current list of runs</param>
    /// <exception cref="NotSupportedException"></exception>
    public override void RenderToString(DocxTextDocumentRenderer renderer, List<OpenXmlElement> runs)
    {
        // No childs
        if (_span.ChildInlines.Count == 0)
        {
            var run = DocxBuilder.CreateHyperlink(_span.Uri, _span.Content, renderer.DocxDocument.MainDocumentPart);
            runs.Add(run);
        }
        else // Childs
        {
            List<OpenXmlElement> runs2 = [];

            foreach (var inline in _span.ChildInlines)
            {
                var item = (InlineDocxTextRendererElementBase)renderer.PdfTextRendererElementFactory.CreateInstanceDocx(inline);
                item.RenderToString(renderer, runs2);
            }

            var run = DocxBuilder.CreateHyperlink(_span.Uri, runs2, renderer.DocxDocument.MainDocumentPart); ;
            runs.Add(run);
        }
    }
}
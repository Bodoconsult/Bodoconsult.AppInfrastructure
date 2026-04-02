// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using Bodoconsult.Office;
using DocumentFormat.OpenXml;
using Bold = Bodoconsult.Text.Documents.Bold;

namespace Bodoconsult.Text.Renderer.Docx.Inlines;

/// <summary>
/// Render a <see cref="Documents.Bold"/> element
/// </summary>
public class BoldDocxTextRendererElement : InlineDocxTextRendererElementBase
{
    private readonly Bold _span;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="span">Paragraph</param>
    public BoldDocxTextRendererElement(Bold span)
    {
        _span = span;
    }


    ///// <summary>
    ///// Render the inline element to string
    ///// </summary>
    ///// <param name="renderer">Current renderer</param>
    ///// <param name="paragraph">Paragraph to render the inline into</param>
    //public override void RenderIt(DocxTextDocumentRenderer renderer, Paragraph paragraph)
    //{

    //}

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
            var run = DocxBuilder.CreateRunBold(_span.Content);
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

            var run = DocxBuilder.CreateRunBold(runs2);
            runs.Add(run);
        }
    }
}
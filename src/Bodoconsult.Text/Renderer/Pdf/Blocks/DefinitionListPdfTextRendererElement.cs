// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using System.Text;
using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;

namespace Bodoconsult.Text.Renderer.Pdf.Blocks;

/// <summary>
/// PDF rendering element for <see cref="DefinitionList"/> instances
/// </summary>
public class DefinitionListPdfTextRendererElement : PdfTextRendererElementBase
{
    private readonly DefinitionList _item;

    /// <summary>
    /// Default ctor
    /// </summary>
    public DefinitionListPdfTextRendererElement(DefinitionList item) : base(item)
    {
        _item = item;
        ClassName = item.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(PdfTextDocumentRenderer renderer)
    {

        var dt = new List<PdfDefinitionListTerm>();

        foreach (var childBlock in _item.ChildBlocks)
        {
            var term = (DefinitionListTerm)childBlock;

            var termItem = new PdfDefinitionListTerm();

            var sb = new StringBuilder();

            PdfDocumentRendererHelper.RenderBlockInlinesToStringForPdf(renderer, term.ChildInlines, sb);
            termItem.Term = sb.ToString();

            foreach (var listItems in term.ChildBlocks)
            {
                sb.Clear();
                PdfDocumentRendererHelper.RenderBlockInlinesToStringForPdf(renderer, listItems.ChildInlines, sb);
                termItem.Items.Add(sb.ToString());
            }

            dt.Add(termItem);
        }

        renderer.PdfDocument.AddDefinitionList(dt);
    }
}
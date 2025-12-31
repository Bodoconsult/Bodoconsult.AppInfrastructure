// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using Bodoconsult.Office;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="DefinitionList"/> instances
/// </summary>
public class DefinitionListDocxTextRendererElement : DocxTextRendererElementBase
{
    private readonly DefinitionList _item;

    /// <summary>
    /// Default ctor
    /// </summary>
    public DefinitionListDocxTextRendererElement(DefinitionList item) : base(item)
    {
        _item = item;
        ClassName = item.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(DocxTextDocumentRenderer renderer)
    {
        var ps = (PageStyleBase)renderer.Styleset.FindStyle("DocumentStyle");

        var rows = new List<DocxDefinitionListRow>();

        foreach (var childBlock in _item.ChildBlocks)
        {
            var term = (DefinitionListTerm)childBlock;

            var row = new DocxDefinitionListRow
            {
                TermStyleId = "DefinitionListTerm",
                ItemsStyleId = "DefinitionListItem"
            };

            // Term
            var sb = new List<OpenXmlElement>();
            DocxDocumentRendererHelper.RenderBlockInlinesToRunsForDocx(renderer, term.ChildInlines, sb);
            row.Term.AddRange(sb);


            foreach (var listItems in term.ChildBlocks)
            {
                sb = new List<OpenXmlElement>();
                DocxDocumentRendererHelper.RenderBlockInlinesToRunsForDocx(renderer, listItems.ChildInlines, sb);
                row.Items.Add(sb);
            }

            rows.Add(row);
        }

        renderer.DocxDocument.AddDefinitionList(rows, 0.25 * ps.ColumnWidth, 0.75 * ps.ColumnWidth);
    }
}
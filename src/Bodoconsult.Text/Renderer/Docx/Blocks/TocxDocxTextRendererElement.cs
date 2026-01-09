// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.Office;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Toc1"/> instances
/// </summary>
public class TocxDocxTextRendererElement : ParagraphDocxTextRendererElementBase
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public TocxDocxTextRendererElement(ParagraphBase tocx) : base(tocx)
    {
        ClassName = tocx.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(DocxTextDocumentRenderer renderer)
    {
        var styleName = ParagraphBase.StyleName.Replace("Style", string.Empty);

        if (styleName == "Paragraph")
        {
            styleName = "Normal";
        }

        //Debug.Print(styleName);

        var childs = new List<Inline>();

        if (string.IsNullOrEmpty(ParagraphBase.CurrentPrefix))
        {
            childs.Add(new Span(ParagraphBase.CurrentPrefix));
        }

        childs.AddRange(ParagraphBase.ChildInlines);

        var runs = new List<OpenXmlElement>();


        DocxDocumentRendererHelper.RenderBlockInlinesToRunsForDocx(renderer, childs, runs);
        runs.Add(new Run(new TabChar()));

        

        var para = renderer.DocxDocument.AddParagraph(runs, styleName);
        para.ParagraphProperties ??= new ParagraphProperties();
        para.ParagraphProperties.Tabs = new Tabs();
        var docStyle = (PageStyleBase)renderer.Styleset.FindStyle("DocumentStyle");


        var tabStop = new TabStop
        {
            Val = TabStopValues.Right,
            Position = MeasurementHelper.GetTwipsFromCm(docStyle.ColumnWidth)
        };
        para.ParagraphProperties.Tabs.Append(tabStop);


        // Add bookmark page ref
        DocxBuilder.AddBookmarkRef(para, ParagraphBase.TagName);
    }
}
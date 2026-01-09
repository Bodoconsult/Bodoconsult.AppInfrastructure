// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;
using System.Collections.Generic;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="HeadingBase"/> instances
/// </summary>
public abstract class HeadingBaseDocxTextRendererElement : ParagraphDocxTextRendererElementBase
{
    private readonly HeadingBase _headingBase;

    /// <summary>
    /// Default ctor
    /// </summary>
    protected HeadingBaseDocxTextRendererElement(HeadingBase headingBase) : base(headingBase)
    {
        _headingBase = headingBase;
        ClassName = headingBase.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(DocxTextDocumentRenderer renderer)
    {
        var styleName = ParagraphBase.StyleName.Replace("Style", string.Empty);

        //Debug.Print(styleName);

        var childs = new List<Inline>();

        if (string.IsNullOrEmpty(ParagraphBase.CurrentPrefix))
        {
            childs.Add(new Span(ParagraphBase.CurrentPrefix));
        }

        childs.AddRange(ParagraphBase.ChildInlines);

        var runs = new List<OpenXmlElement>();

        DocxDocumentRendererHelper.RenderBlockInlinesToRunsForDocx(renderer, childs, runs);
        renderer.DocxDocument.AddParagraph(runs, styleName, ParagraphBase.TagName);
    }
}
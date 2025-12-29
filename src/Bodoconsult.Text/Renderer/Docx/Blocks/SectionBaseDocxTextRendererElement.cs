// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using System.Linq;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="SectionBase"/> instances
/// </summary>
public abstract class SectionBaseDocxTextRendererElement : DocxTextRendererElementBase
{
    private readonly SectionBase _sectionBase;

    /// <summary>
    /// Default ctor
    /// </summary>
    protected SectionBaseDocxTextRendererElement(SectionBase sectionBase) : base(sectionBase)
    {
        _sectionBase = sectionBase;
        ClassName = sectionBase.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="section">Current section</param>
    /// <param name="styleName">Stylename to use for caption</param>
    /// <param name="caption">Caption</param>
    public static void RenderItInternal(DocxTextDocumentRenderer renderer, SectionBase section, string styleName, string caption)
    {
        if (section.ChildBlocks.Count == 0)
        {
            return;
        }

        var sections = renderer.Document.ChildBlocks.Where(x => x is SectionBase).ToList();

        var index = sections.IndexOf(section);

        var isLastSection = index == sections.Count - 1;


        renderer.DocxDocument.AddSection(isLastSection, section.IsRestartPageNumberingRequired);
        var style = (PageStyleBase )renderer.Document.Styleset.FindStyle("DocumentStyle");

        renderer.DocxDocument.SetBasicPageProperties(style.PageFormat.Size.Width, style.PageFormat.Size.Height, style.Margins.Left, style.Margins.Top, style.Margins.Right, style.Margins.Bottom);

        var tabPosition = style.TypeAreaWidth;

        if (!string.IsNullOrEmpty(renderer.Document.DocumentMetaData.HeaderText))
        {
            renderer.DocxDocument.AddHeaderToCurrentSection(renderer.Document.DocumentMetaData.HeaderText, tabPosition, section.PageNumberFormat);
        }
        if (!string.IsNullOrEmpty(renderer.Document.DocumentMetaData.FooterText))
        {
            renderer.DocxDocument.AddFooterToCurrentSection(renderer.Document.DocumentMetaData.FooterText, tabPosition, section.PageNumberFormat);
        }

        if (!string.IsNullOrEmpty(caption))
        {
            renderer.DocxDocument.AddParagraph(caption, styleName);
        }

        DocxDocumentRendererHelper.RenderBlockChildsToDocx(renderer, section.ChildBlocks);

    }
}
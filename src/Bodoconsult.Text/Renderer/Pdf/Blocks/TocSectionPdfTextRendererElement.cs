// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;

namespace Bodoconsult.Text.Renderer.Pdf.Blocks;

/// <summary>
/// PDF rendering element for <see cref="TocSection"/> instances
/// </summary>
public class TocSectionPdfTextRendererElement : PdfTextRendererElementBase
{
    private readonly TocSection _tocSection;

    /// <summary>
    /// Default ctor
    /// </summary>
    public TocSectionPdfTextRendererElement(TocSection tocSection) : base(tocSection)
    {
        _tocSection = tocSection;
        ClassName = tocSection.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(PdfTextDocumentRenderer renderer)
    {
        if (_tocSection.ChildBlocks.Count == 0)
        {
            return;
        }

        if (!string.IsNullOrEmpty(renderer.Document.DocumentMetaData.HeaderText))
        {
            renderer.PdfDocument.SetHeader(renderer.Document.DocumentMetaData.HeaderText);
        }
        if (!string.IsNullOrEmpty(renderer.Document.DocumentMetaData.FooterText))
        {
            renderer.PdfDocument.SetFooter(renderer.Document.DocumentMetaData.FooterText);
        }
        renderer.PdfDocument.CreateTocSection(_tocSection.IsRestartPageNumberingRequired, _tocSection.PageNumberFormat);

        PdfDocumentRendererHelper.RenderBlockChildsToPdf(renderer, Block.ChildBlocks);
    }
}
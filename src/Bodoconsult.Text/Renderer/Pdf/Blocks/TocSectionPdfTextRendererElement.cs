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

        var metaData = renderer.Document.DocumentMetaData;

        if (!string.IsNullOrEmpty(metaData.HeaderText))
        {
            renderer.PdfDocument.SetHeader(metaData.HeaderText, "Header", metaData.LogoPath);
        }
        if (!string.IsNullOrEmpty(renderer.Document.DocumentMetaData.FooterText))
        {
            renderer.PdfDocument.SetFooter(metaData.FooterText);
        }

        renderer.PdfDocument.CreateTocSection(_tocSection.IsRestartPageNumberingRequired, _tocSection.PageNumberFormat);

        PdfDocumentRendererHelper.RenderBlockChildsToPdf(renderer, Block.ChildBlocks);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;

namespace Bodoconsult.Text.Renderer.Pdf.Blocks;

/// <summary>
/// PDF rendering element for <see cref="TotSection"/> instances
/// </summary>
public class TotSectionPdfTextRendererElement : PdfTextRendererElementBase
{
    private readonly TotSection _totSection;

    /// <summary>
    /// Default ctor
    /// </summary>
    public TotSectionPdfTextRendererElement(TotSection totSection) : base(totSection)
    {
        _totSection = totSection;
        ClassName = totSection.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(PdfTextDocumentRenderer renderer)
    {
        if (_totSection.ChildBlocks.Count == 0)
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

        renderer.PdfDocument.CreateTotSection(_totSection.IsRestartPageNumberingRequired, _totSection.PageNumberFormat);

        PdfDocumentRendererHelper.RenderBlockChildsToPdf(renderer, Block.ChildBlocks);
    }
}
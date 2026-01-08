// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;

namespace Bodoconsult.Text.Renderer.Pdf.Blocks;

/// <summary>
/// PDF rendering element for <see cref="TofSection"/> instances
/// </summary>
public class TofSectionPdfTextRendererElement : PdfTextRendererElementBase
{
    private readonly TofSection _tofSection;

    /// <summary>
    /// Default ctor
    /// </summary>
    public TofSectionPdfTextRendererElement(TofSection tofSection) : base(tofSection)
    {
        _tofSection = tofSection;
        ClassName = tofSection.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(PdfTextDocumentRenderer renderer)
    {
        if (_tofSection.ChildBlocks.Count == 0)
        {
            return;
        }

        var metaData = renderer.Document.DocumentMetaData;

        if (!string.IsNullOrEmpty(metaData.HeaderText))
        {
            renderer.PdfDocument.SetHeader();
        }
        if (!string.IsNullOrEmpty(renderer.Document.DocumentMetaData.FooterText))
        {
            renderer.PdfDocument.SetFooter();
        }

        renderer.PdfDocument.CreateTofSection(_tofSection.IsRestartPageNumberingRequired, _tofSection.PageNumberFormat);

        PdfDocumentRendererHelper.RenderBlockChildsToPdf(renderer, Block.ChildBlocks);
    }
}
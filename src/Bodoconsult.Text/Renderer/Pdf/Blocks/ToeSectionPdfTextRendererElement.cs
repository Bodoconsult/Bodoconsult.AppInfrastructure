// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;

namespace Bodoconsult.Text.Renderer.Pdf.Blocks;

/// <summary>
/// PDF rendering element for <see cref="ToeSection"/> instances
/// </summary>
public class ToeSectionPdfTextRendererElement : PdfTextRendererElementBase
{
    private readonly ToeSection _toeSection;

    /// <summary>
    /// Default ctor
    /// </summary>
    public ToeSectionPdfTextRendererElement(ToeSection toeSection) : base(toeSection)
    {
        _toeSection = toeSection;
        ClassName = toeSection.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(PdfTextDocumentRenderer renderer)
    {
        if (_toeSection.ChildBlocks.Count == 0)
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

        renderer.PdfDocument.CreateToeSection(_toeSection.IsRestartPageNumberingRequired, _toeSection.PageNumberFormat);

        PdfDocumentRendererHelper.RenderBlockChildsToPdf(renderer, Block.ChildBlocks);
    }
}
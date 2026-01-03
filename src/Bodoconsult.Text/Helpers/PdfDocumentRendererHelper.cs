// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Renderer.Pdf;
using Bodoconsult.Text.Renderer.Pdf.Inlines;
using MigraDoc.DocumentObjectModel;
using Color = MigraDoc.DocumentObjectModel.Color;

namespace Bodoconsult.Text.Helpers;

/// <summary>
/// Helper class for document rendering
/// </summary>
public static class PdfDocumentRendererHelper
{
    /// <summary>
    /// Render the child blocks of a block to PDF
    /// </summary>
    /// <param name="renderer">Curent PDF renderer</param>
    /// <param name="childBlocks">Child blocks to render</param>
    public static void RenderBlockChildsToPdf(PdfTextDocumentRenderer renderer, ReadOnlyLdmlList<Block> childBlocks)
    {
        foreach (var block in childBlocks)
        {
            var rendererElement = renderer.PdfTextRendererElementFactory.CreateInstancePdf(block);
            rendererElement.RenderIt(renderer);
        }
    }

    /// <summary>
    /// Render block inline childs to string for PDF
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="childInlines">Child inlines</param>
    /// <param name="sb">String</param>
    public static void RenderBlockInlinesToStringForPdf(PdfTextDocumentRenderer renderer, List<Inline> childInlines,
        StringBuilder sb)
    {
        foreach (var inline in childInlines)
        {
            var rendererElement =
                (InlinePdfTextRendererElementBase)renderer.PdfTextRendererElementFactory.CreateInstancePdf(inline);
            rendererElement.RenderToString(renderer, sb);
        }
    }

    /// <summary>
    /// Map style to PDF style
    /// </summary>
    /// <param name="style">Style</param>
    /// <param name="pdfStyle">PDF style</param>
    public static void RenderParagraphStyle(ParagraphStyleBase style, Style pdfStyle)
    {
        if (pdfStyle == null)
        {
            return;
        }

        pdfStyle.Font.Bold = style.Bold;
        pdfStyle.Font.Italic = style.Italic;
        pdfStyle.Font.Name = style.FontName;
        pdfStyle.Font.Color = GetPdfColor(style.FontColor);
        pdfStyle.Font.Size = style.FontSize;

        pdfStyle.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(style.Margins.Top);
        pdfStyle.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(style.Margins.Bottom);
        pdfStyle.ParagraphFormat.LeftIndent = Unit.FromCentimeter(style.Margins.Left);
        pdfStyle.ParagraphFormat.RightIndent = Unit.FromCentimeter(style.Margins.Right);
        pdfStyle.ParagraphFormat.PageBreakBefore = style.PageBreakBefore;
        pdfStyle.ParagraphFormat.KeepTogether = style.KeepTogether;
        pdfStyle.ParagraphFormat.KeepWithNext = style.KeepWithNextParagraph;

        // ToDo: Border
        var black = TypoColors.Black.ToLdmlColor();

        var color = (Documents.Color)style.BorderBrush?.Color ?? black;

        var lb = pdfStyle.ParagraphFormat.Borders.Left;
        lb.Color = GetPdfColor(color);
        lb.Visible = style.BorderThickness.Left > 0;
        lb.Width = Unit.FromPoint(style.BorderThickness.Left);

        var rb = pdfStyle.ParagraphFormat.Borders.Right;
        rb.Color = GetPdfColor(color);
        rb.Visible = style.BorderThickness.Right > 0;
        rb.Width = Unit.FromPoint(style.BorderThickness.Right);

        var tb = pdfStyle.ParagraphFormat.Borders.Top;
        tb.Color = GetPdfColor(color);
        tb.Visible = style.BorderThickness.Top > 0;
        tb.Width = Unit.FromPoint(style.BorderThickness.Top);

        var bb = pdfStyle.ParagraphFormat.Borders.Bottom;
        bb.Color = GetPdfColor(color);
        bb.Visible = style.BorderThickness.Bottom > 0;
        bb.Width = Unit.FromPoint(style.BorderThickness.Bottom);
    }

    private static Color GetPdfColor(Documents.Color color)
    {
        var pdfColor = new Color(color.A, color.R, color.G, color.B);
        return pdfColor;
    }
}
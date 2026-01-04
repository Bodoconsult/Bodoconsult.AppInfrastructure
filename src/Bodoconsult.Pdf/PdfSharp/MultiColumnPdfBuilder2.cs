// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Stylesets;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// Class representing a multi-column text PDF document and basic functionality to add content to it. Adjust to use with LDML. Does not create TOC etc. automatically
/// </summary>
public class MultiColumnPdfBuilder2 : PdfBuilderBase
{
    #region Constructors

    /// <summary>
    /// Default ctor to load a complete styleset
    /// </summary>
    /// <param name="styleSet">Styleset to use</param>
    /// <param name="fontResolver">Font resolver to load</param>
    public MultiColumnPdfBuilder2(IStyleSet styleSet, IFontResolver fontResolver)
    {
        LoadDefaults();

        GlobalFontSettings.FontResolver ??= fontResolver;

        LoadStyleset(styleSet);
    }

    #endregion

    /// <summary>
    /// Add a heading 1 to the content section
    /// </summary>
    /// <param name="level">Heading level</param>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    protected override Paragraph AddHeadingInternal(int level, string text, string tag)
    {
        var paragraph = new Paragraph
        {
            Style = $"Heading{level}",
            Tag = tag
        };

        paragraph.AddText(text ?? string.Empty);

        Content.Add(paragraph);

        return paragraph;
    }

    /// <summary>
    /// Add a figure to the document
    /// </summary>
    /// <param name="imagePath">Full file path to the figure image</param>
    /// <param name="legend">Legend for the figure</param>
    /// <param name="tag">Link tag name</param>
    /// <param name="width">Width in cm</param>
    /// <param name="height">Height in cm</param>
    public override void AddFigure(string imagePath, string legend, string tag, double width, double height)
    {
        AddImage(imagePath, width, height);

        if (string.IsNullOrEmpty(legend))
        {
            return;
        }

        var p = Content.AddParagraph(legend, "FigureLegend");

        if (string.IsNullOrEmpty(tag))
        {
            return;
        }

        p.Tag = tag;
    }

    /// <summary>
    /// Add an entry to the TOE
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced equation</param>
    public override Paragraph AddToeEntry(string text, string tag)
    {
        var p = Toe.AddParagraph();
        p.Style = "TOE";
        p.AddText($"{text}\t{ITypography.PageFieldIndicator}");
        return p;
    }

    /// <summary>
    /// Add an entry to the TOF
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced figure</param>
    public override Paragraph AddTofEntry(string text, string tag)
    {
        var p = Tof.AddParagraph();
        p.Style = "TOF";
        p.AddText($"{text}\t{ITypography.PageFieldIndicator}");
        return p;
    }

    /// <summary>
    /// Add an entry to the TOT
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    public override Paragraph AddTotEntry(string text, string tag)
    {
        var p = Tot.AddParagraph();
        p.Style = "TOT";
        p.AddText($"{text}\t{ITypography.PageFieldIndicator}");
        return p;
    }

    /// <summary>
    /// Add an entry to the TOC
    /// </summary>
    /// <param name="level">Heading level</param>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    protected override Paragraph AddTocEntryInternal(int level, string text, string tag)
    {
        var p = Toc.AddParagraph();
        p.Style = $"TOC{level}";
        p.AddText($"{text}\t{ITypography.PageFieldIndicator}");
        p.Tag = tag;
        return p;
    }


    /// <summary>
    /// Save Pdf to a stream
    /// </summary>
    /// <param name="stream">Stream</param>
    public override void RenderToPdf(Stream stream)
    {
        var document = CreateMasterPdfDocument();
        document.Save(stream);
    }


    /// <summary>
    /// Save Pdf to a file
    /// </summary>
    /// <param name="fileName">Full path for pdf file's destination</param>
    /// <param name="showPdfFile">Show Pdf-File in a viewer</param>
    public override void RenderToPdf(string fileName, bool showPdfFile)
    {
        var document = CreateMasterPdfDocument();

        document.Save(fileName);

        if (!showPdfFile)
        {
            return;
        }

        // ...and start a viewer.
        OpenFile(fileName);
    }


    private static Section GetSection(DocumentRenderer docRenderer, int pageNumber)
    {
        return docRenderer.GetDocumentObjectsFromPage(pageNumber)[0].Section;
    }

    private XRect GetRect(int index)
    {
        var x = StyleSet.PageSetupOriginal.LeftMargin.Point + index * StyleSet.ColumnWidth.Point + index * StyleSet.Space.Point;

        var rect = new XRect(x, 0, StyleSet.PageSetup.PageWidth.Point, StyleSet.PageSetup.PageHeight.Point);
        return rect;
    }



    private PdfDocument CreateMasterPdfDocument()
    {
        var document = new PdfDocument();
        document.Info.Title = Document.Info.Title;
        document.Info.Subject = Document.Info.Subject;
        document.Info.Author = Document.Info.Author;


        // Create a renderer and prepare (=layout) the document
        var docRenderer = new DocumentRenderer(Document);
        docRenderer.PrepareDocument();

        // For clarity, we use point as unit of measure in this sample.
        // A4 is the standard letter size in Germany (21cm x 29.7cm).
        var a4Rect = new XRect(0, 0, StyleSet.PageSetup.PageWidth.Point, StyleSet.PageSetup.PageHeight.Point);

        var pageCount = docRenderer.FormattedDocument?.PageCount;

        var idx = 0;

        XGraphics gfx = null;

        var oldSection = GetSection(docRenderer, 1);

        var renderInfo = docRenderer.GetRenderInfoFromPage(1)[0];

        for (var pageNum = 0; pageNum < pageCount; pageNum++)
        {
            var section = GetSection(docRenderer, pageNum + 1);

            if (section != oldSection)
            {
                var page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
                page.Size = PageSize.A4;
                page.Orientation = StyleSet.PageSetupOriginal.Orientation == Orientation.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait;

                idx = 0;
                oldSection = section;
            }

            if (idx == 0)
            {
                var page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
                page.Size = PageSize.A4;
                page.Orientation = StyleSet.PageSetupOriginal.Orientation == Orientation.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait;
            }
            else
            {
                if (gfx == null)
                {
                    throw new ArgumentNullException(nameof(gfx));
                }
            }

            var rect = GetRect(idx);

            // Use BeginContainer / EndContainer for simplicity only. You can naturaly use you own transformations.
            var container = gfx.BeginContainer(rect, a4Rect, XGraphicsUnit.Point);

            // Render the page. Note that page numbers start with 1.
            docRenderer.RenderPage(gfx, pageNum + 1);

            //var docOs = docRenderer.GetRenderInfoFromPage(pageNum + 1);

            //foreach (var docO in docOs)
            //{
            //    var ca = docO.LayoutInfo.ContentArea;
            //    docRenderer.RenderObject(gfx, ca.X, ca.Y, ca.Width, docO.DocumentObject);
            //}


            // Note: The outline and the hyperlinks (table of content) does not work in the produced PDF document.

            // Pop the previous graphical state
            gfx.EndContainer(container);

            idx++;
            if (idx >= StyleSet.NumberOfColumns)
            {
                idx = 0;
            }
        }

        return document;
    }
}
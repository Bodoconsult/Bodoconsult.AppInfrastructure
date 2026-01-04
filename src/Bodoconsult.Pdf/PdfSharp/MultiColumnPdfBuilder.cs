// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Stylesets;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Quality;
using PdfSharp.UniversalAccessibility.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// Class representing a multi-column text PDF document and basic functionality to add content to it. Adjust to use with LDML. Does not create TOC etc. automatically
/// </summary>
public class MultiColumnPdfBuilder : PdfBuilderBase
{
    private readonly Dictionary<string, int> _pageInfo = new();

    #region Constructors

    /// <summary>
    /// Default ctor to load a complete styleset
    /// </summary>
    /// <param name="styleSet">Styleset to use</param>
    /// <param name="fontResolver">Font resolver to load</param>
    public MultiColumnPdfBuilder(IStyleSet styleSet, IFontResolver fontResolver)
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
        if (string.IsNullOrEmpty(tag))
        {
            return p;
        }

        p.Tag = tag;
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
        if (string.IsNullOrEmpty(tag))
        {
            return p;
        }

        p.Tag = tag;
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
        if (string.IsNullOrEmpty(tag))
        {
            return p;
        }

        p.Tag = tag;
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
    /// Add a header
    /// </summary>
    /// <param name="section">Section to add the header to</param>
    protected override void AddHeaderInternal(Section section)
    { }

    /// <summary>
    /// Add a footer
    /// </summary>
    /// <param name="section">Section to add the footer to</param>
    /// <param name="pageNumberFormat">Null or ROMAN, roman, ALPHABETIC, alphabetic</param>
    protected override void AddFooterInternal(Section section, string pageNumberFormat = null)
    { }

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

    /// <summary>
    /// Create a table legend
    /// </summary>
    /// <param name="legend">Legend text</param>
    /// <param name="tag">Bookmark tag</param>
    protected override void CreateTableLegend(string legend, string tag)
    {
        var legendP = Content.AddParagraph(legend, "TableLegend");
        if (string.IsNullOrEmpty(tag))
        {
            return;
        }
        legendP.Tag = tag;
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

        _pageInfo.Clear();

        var document = new PdfDocument();
        document.Info.Title = Document.Info.Title;
        document.Info.Subject = Document.Info.Subject;
        document.Info.Author = Document.Info.Author;


        // Create a renderer and prepare (=layout) the document
        var docRenderer = new DocumentRenderer(Document);
        docRenderer.PrepareDocument();

        var pageCount = docRenderer.FormattedDocument?.PageCount ?? 0;
        var oldSection = GetSection(docRenderer, 1);

        // Find the new page numbers
        FindPageNumbers(pageCount, docRenderer, oldSection);

        // Apply new pagenumbers
        CheckPageNumbersForSection(Toe);
        CheckPageNumbersForSection(Tof);
        CheckPageNumbersForSection(Tot);
        CheckPageNumbersForSection(Toc);

        // Now render the pages
        RenderPages(docRenderer, document);

        return document;
    }

    private XRect a4Rect;

    private PageSetup headerPageSetup;

    private PageSetup footerPageSetup;

    private void RenderPages(DocumentRenderer docRenderer, PdfDocument document)
    {
        // For clarity, we use point as unit of measure in this sample.
        // A4 is the standard letter size in Germany (21cm x 29.7cm).
        a4Rect = new XRect(0, 0, StyleSet.PageSetup.PageWidth.Point, StyleSet.PageSetup.PageHeight.Point);


        headerPageSetup = new PageSetup();
        headerPageSetup.PageWidth = StyleSet.PageSetupOriginal.PageWidth - StyleSet.PageSetupOriginal.LeftMargin -
                                    StyleSet.PageSetupOriginal.RightMargin;
        headerPageSetup.PageHeight = Unit.FromCentimeter(1.5);

        footerPageSetup = new PageSetup();
        footerPageSetup.PageWidth = headerPageSetup.PageWidth;
        footerPageSetup.PageHeight = headerPageSetup.PageHeight;


        // Use a fresh renderer now
        //docRenderer = new DocumentRenderer(Document);
        //docRenderer.PrepareDocument();
        var pageCount = docRenderer.FormattedDocument?.PageCount ?? 0;

        var idx = 0;

        XGraphics gfx = null;

        var oldSection = GetSection(docRenderer, 1);

        PdfPage page = null;

        //var renderInfo = docRenderer.GetRenderInfoFromPage(1)[0];
        for (var pageNum = 0; pageNum < pageCount; pageNum++)
        {
            var section = GetSection(docRenderer, pageNum + 1);

            if (idx != 0 && section != oldSection)
            {
                //var page = document.AddPage();
                //gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
                //page.Size = PageSize.A4;
                //page.Orientation = StyleSet.PageSetupOriginal.Orientation == Orientation.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait;

                idx = 0;
                oldSection = section;
            }

            if (idx == 0)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
                page.Size = PageSize.A4;
                page.Orientation = StyleSet.PageSetupOriginal.Orientation == Orientation.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait;

                // Draw background image if necessary
                if (!string.IsNullOrEmpty(BackgroundImagePath) && File.Exists(BackgroundImagePath))
                {
                    var image = XImage.FromFile(BackgroundImagePath);
                    gfx.DrawImage(image, 0, 0, page.Width.Point, page.Height.Point);

                }

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

            // Pop the previous graphical state
            gfx.EndContainer(container);

            PrintHeader(page, gfx, pageNum + 1);
            PrintFooter(page, gfx, pageNum + 1);

            idx++;
            if (idx >= StyleSet.NumberOfColumns)
            {
                idx = 0;
            }
        }
    }

    private void PrintHeader(PdfPage page, XGraphics gfx, int pageNum)
    {
        if (string.IsNullOrEmpty(HeaderText) && string.IsNullOrEmpty(HeaderLogoPath))
        {
            return;
        }

        var x = StyleSet.PageSetupOriginal.LeftMargin.Point;

        // Use BeginContainer / EndContainer for simplicity only. You can naturaly use you own transformations.

        if (!string.IsNullOrEmpty(HeaderLogoPath))
        {
            x = 150;
            var y = 15;

            var image = XImage.FromFile(HeaderLogoPath);
            gfx.DrawImage(image, 0, 0, x, y);
        }

        if (!string.IsNullOrEmpty(HeaderText))
        {
            XFont font = new XFont("Arial", 8);
            XBrush brush = new XSolidBrush(XColor.FromArgb(255,0,0,0 ));

            x = StyleSet.PageSetupOriginal.PageWidth.Point - StyleSet.PageSetupOriginal.RightMargin.Point - gfx.MeasureString(HeaderText, font).Width; 
            var y = 0.5 * StyleSet.PageSetupOriginal.TopMargin.Point;

            gfx.DrawString(HeaderText, font, brush, x, y);
        }
    }

    private void PrintFooter(PdfPage page, XGraphics gfx, int pageNum)
    {
        if (string.IsNullOrEmpty(FooterText))
        {
            return;
        }

        double x;

        XFont font = new XFont("Arial", 8);
        XBrush brush = new XSolidBrush(XColor.FromArgb(255, 0, 0, 0));

        var y = StyleSet.PageSetupOriginal.PageHeight.Point - 0.5 * StyleSet.PageSetupOriginal.BottomMargin.Point;

        if (!string.IsNullOrEmpty(FooterText))
        {
            x = StyleSet.PageSetupOriginal.LeftMargin.Point;
            gfx.DrawString(FooterText, font, brush, x, y);
        }

        var pageNumStr = pageNum.ToString();
        x = StyleSet.PageSetupOriginal.PageWidth.Point - StyleSet.PageSetupOriginal.RightMargin.Point - gfx.MeasureString(pageNumStr, font).Width;
        gfx.DrawString(pageNumStr, font, brush, x, y);
    }

    private void FindPageNumbers(int pageCount, DocumentRenderer docRenderer, Section oldSection)
    {
        var idx = 0;
        var newPageNum = -1;

        for (var pageNum = 0; pageNum < pageCount; pageNum++)
        {
            var section = GetSection(docRenderer, pageNum + 1);

            if (idx == 0 || section != oldSection)
            {
                if (section != Toc && section != Toe && section != Tof && section != Tot)
                {
                    newPageNum++;
                }
                idx = 0;
                oldSection = section;
            }

            if (section != Toc && section != Toe && section != Tof && section != Tot)
            {
                var docOs = docRenderer.GetDocumentObjectsFromPage(pageNum + 1);
                foreach (var docO in docOs)
                {
                    var tag = (string)docO.Tag ?? "";
                    if (!string.IsNullOrEmpty(tag))
                    {
                        _pageInfo.Add(tag, newPageNum + 1);
                    }
                }
            }

            idx++;
            if (idx >= StyleSet.NumberOfColumns)
            {
                idx = 0;
            }
        }
    }

    private void CheckPageNumbersForSection(Section section)
    {
        if (section == null)
        {
            return;
        }
        
        
        foreach (var docO in section.Elements)
        {
            var tag = (string)docO.Tag ?? "";

            if (string.IsNullOrEmpty(tag))
            {
                continue;
            }

            Debug.Print(tag);

            if (!_pageInfo.TryGetValue(tag, out var pageNumber))
            {
                continue;
            }

            var p = (Paragraph)docO;
            foreach (var e in p.Elements)
            {
                if (e is not Text text)
                {
                    continue;
                }

                if (text.Content.Contains(ITypography.PageFieldIndicator))
                {
                    text.Content = text.Content.Replace(ITypography.PageFieldIndicator, pageNumber.ToString(),
                        StringComparison.InvariantCultureIgnoreCase);
                }
            }
        }
    }
}
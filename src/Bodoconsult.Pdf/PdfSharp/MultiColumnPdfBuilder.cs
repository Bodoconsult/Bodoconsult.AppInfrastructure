// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Stylesets;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Bodoconsult.App.Abstractions.Extensions;

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

        var p = Content.AddParagraph(legend, "Figure");

        if (string.IsNullOrEmpty(tag))
        {
            return;
        }

        p.Tag = tag;
    }

    /// <summary>
    /// Add a figure to the document
    /// </summary>
    /// <param name="imagePath">Full file path to the equation image</param>
    /// <param name="legend">Legend for the equation</param>
    /// <param name="tag">Link tag name</param>
    /// <param name="width">Width in cm</param>
    /// <param name="height">Height in cm</param>
    public override void AddEquation(string imagePath, string legend, string tag, double width, double height)
    {
        AddImage(imagePath, width, height);

        if (string.IsNullOrEmpty(legend))
        {
            return;
        }

        var p = Content.AddParagraph(legend, "Equation");

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
    protected override void AddFooterInternal(Section section, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
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

    /// <summary>
    /// Render the pages
    /// </summary>
    /// <param name="docRenderer">Current renderer</param>
    /// <param name="document">Current document</param>
    /// <exception cref="ArgumentNullException">Thrown if there is no drawing surface creatable</exception>
    protected void RenderPages(DocumentRenderer docRenderer, PdfDocument document)
    {
        // For clarity, we use point as unit of measure in this sample.
        // A4 is the standard letter size in Germany (21cm x 29.7cm).
        var a4Rect = new XRect(0, 0, StyleSet.PageSetup.PageWidth.Point, StyleSet.PageSetup.PageHeight.Point);

        // Use a fresh renderer now
        //docRenderer = new DocumentRenderer(Document);
        //docRenderer.PrepareDocument();
        var pageCount = docRenderer.FormattedDocument?.PageCount ?? 0;

        var idx = 0;

        XGraphics gfx = null;

        var oldSection = GetSection(docRenderer, 1);

        var currentPageNum = 0;
        //var renderInfo = docRenderer.GetRenderInfoFromPage(1)[0];
        for (var pageNum = 0; pageNum < pageCount; pageNum++)
        {
            var section = GetSection(docRenderer, pageNum + 1);

            var si = SectionInfos.FirstOrDefault(x => x.Section == section);

            var pageNumberFormat = si?.PageNumberFormat ?? PageNumberFormatEnum.Decimal;

            if (idx != 0 && section != oldSection)
            {
                if (section.PageSetup.StartingNumber == 1)
                {
                    currentPageNum = 0;
                }

                idx = 0;

                oldSection = section;
            }

            if (idx == 0)
            {
                currentPageNum++;
                var page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
                page.Size = PageSize.A4;
                page.Orientation = StyleSet.PageSetupOriginal.Orientation == Orientation.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait;

                // Draw background image if necessary
                if (!string.IsNullOrEmpty(BackgroundImagePath) && File.Exists(BackgroundImagePath))
                {
                    var image = XImage.FromFile(BackgroundImagePath);
                    gfx.DrawImage(image, 0, 0, page.Width.Point, page.Height.Point);
                }

                PrintHeader(gfx);
                PrintFooter(gfx, currentPageNum, pageNumberFormat);
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

            idx++;
            if (idx >= StyleSet.NumberOfColumns)
            {
                idx = 0;
            }
        }
    }

    /// <summary>
    /// Print header. Override this method if you want to implement another header
    /// </summary>
    /// <param name="gfx">Graphics</param>
    protected virtual void PrintHeader(XGraphics gfx)
    {
        if (string.IsNullOrEmpty(HeaderText) && string.IsNullOrEmpty(HeaderLogoPath))
        {
            return;
        }

        var style = StyleSet.Footer;

        var font = new XFont(style.Font.Name, style.Font.Size.Point);
        var brush = new XSolidBrush(XColor.FromArgb((int)style.Font.Color.A, (int)style.Font.Color.R, (int)style.Font.Color.G, (int)style.Font.Color.B));

        var x = StyleSet.PageSetupOriginal.LeftMargin.Point;
        var y = 0.5 * StyleSet.PageSetupOriginal.TopMargin.Point;

        // Borderline
        if (style.ParagraphFormat.Borders.Width.Point > 0)
        {
            var borderColor = style.ParagraphFormat.Borders.Color;
            var pen = new XPen(XColor.FromArgb((int)borderColor.A, (int)borderColor.R, (int)borderColor.G, (int)borderColor.B))
            {
                Width = style.ParagraphFormat.Borders.Width.Point
            };
            gfx.DrawLine(pen, x, y + 3, StyleSet.PageSetupOriginal.PageWidth.Point - StyleSet.PageSetupOriginal.RightMargin.Point, y + 3);
        }

        // Logo
        if (!string.IsNullOrEmpty(HeaderLogoPath))
        {
            var height = Unit.FromCentimeter(HeaderLogoHeight);

            var image = XImage.FromFile(HeaderLogoPath);

            var rel = image.PointWidth / image.PixelHeight;

            var width = height.Point / rel;

            gfx.DrawImage(image, x, y - height.Point, width, height.Point);
        }

        // Header text
        if (!string.IsNullOrEmpty(HeaderText))
        {
            x = StyleSet.PageSetupOriginal.PageWidth.Point - StyleSet.PageSetupOriginal.RightMargin.Point - gfx.MeasureString(HeaderText, font).Width;
            gfx.DrawString(HeaderText, font, brush, x, y);
        }
    }

    /// <summary>
    /// Print footer. Override this method if you want to implement another header
    /// </summary>
    /// <param name="gfx">Graphics</param>
    /// <param name="pageNum">Current page number</param>
    /// <param name="pageNumberFormat">Page number format</param>
    protected virtual void PrintFooter(XGraphics gfx, int pageNum, PageNumberFormatEnum pageNumberFormat)
    {
        if (string.IsNullOrEmpty(FooterText))
        {
            return;
        }

        var style = StyleSet.Footer;

        var font = new XFont(style.Font.Name, style.Font.Size.Point);
        var brush = new XSolidBrush(XColor.FromArgb((int)style.Font.Color.A, (int)style.Font.Color.R, (int)style.Font.Color.G, (int)style.Font.Color.B));

        var x = StyleSet.PageSetupOriginal.LeftMargin.Point;
        var y = StyleSet.PageSetupOriginal.PageHeight.Point - 0.5 * StyleSet.PageSetupOriginal.BottomMargin.Point;

        var footerText = FooterText.Replace("\t", string.Empty, StringComparison.InvariantCultureIgnoreCase);
        var isPageNumber = false;

        if (footerText.Contains(ITypography.PageFieldIndicator, StringComparison.InvariantCultureIgnoreCase))
        {
            isPageNumber = true;
            footerText = footerText.Replace(ITypography.PageFieldIndicator, string.Empty,
                StringComparison.InvariantCultureIgnoreCase);
        }

        // Borderline
        if (style.ParagraphFormat.Borders.Width.Point > 0)
        {
            var borderColor = style.ParagraphFormat.Borders.Color;
            var pen = new XPen(XColor.FromArgb((int)borderColor.A, (int)borderColor.R, (int)borderColor.G, (int)borderColor.B))
            {
                Width = style.ParagraphFormat.Borders.Width.Point
            };
            gfx.DrawLine(pen, x, y - style.Font.Size.Point - 3, StyleSet.PageSetupOriginal.PageWidth.Point - StyleSet.PageSetupOriginal.RightMargin.Point, y - style.Font.Size.Point - 3);
        }

        // Footer text
        if (!string.IsNullOrEmpty(footerText))
        {
            x = StyleSet.PageSetupOriginal.LeftMargin.Point;
            gfx.DrawString(footerText, font, brush, x, y);
        }

        // Page number
        if (!isPageNumber)
        {
            return;
        }

        // ToDo: add upper and lower latin

        string pageNumStr;
        switch (pageNumberFormat)
        {
            case PageNumberFormatEnum.UpperRoman:
                pageNumStr = $"{PageNumberPrefix} {pageNum.ArabicToRoman().ToUpperInvariant()}";
                break;
            case PageNumberFormatEnum.LowerRoman:
                pageNumStr = $"{PageNumberPrefix} {pageNum.ArabicToRoman().ToLowerInvariant()}";
                break;
            case PageNumberFormatEnum.UpperLatin:
                pageNumStr = $"{PageNumberPrefix} {pageNum.ToUpperLatin()}";
                break;
            case PageNumberFormatEnum.LowerLatin:
                pageNumStr = $"{PageNumberPrefix} {pageNum.ToLowerLatin()}";
                break;
            case PageNumberFormatEnum.Decimal:
            default:
                pageNumStr = $"{PageNumberPrefix} {pageNum}";
                break;
        }

        Debug.Print(pageNumStr);

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

            //Debug.Print(tag);

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
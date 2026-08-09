// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Extensions;
using Bodoconsult.Pdf.Helpers;
using Bodoconsult.Pdf.Interfaces;
using Bodoconsult.Pdf.Stylesets;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DataTableExtensions = Bodoconsult.Pdf.Extensions.DataTableExtensions;

// https://www.pdfsharp.net/wiki-1.5/Print.aspx?Page=Watermark-sample

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// Base class for PDF builder classes
/// </summary>
public abstract class PdfBuilderBase : IPdfBuilder
{
    #region Protected properties

    /// <summary>
    /// Current PDF document
    /// </summary>
    protected Document Document = new();

    /// <summary>
    /// Content section
    /// </summary>
    protected Section Content;

    /// <summary>
    /// TOC section
    /// </summary>
    protected Section Toc;
    /// <summary>
    /// TOF section
    /// </summary>
    protected Section Tof;

    /// <summary>
    /// TOE section
    /// </summary>
    protected Section Toe;

    /// <summary>
    /// TOT section
    /// </summary>
    protected Section Tot;

    /// <summary>
    /// Current section infos
    /// </summary>
    protected List<SectionInfo> SectionInfos = [];

    /// <summary>
    /// Width of a <see cref="DateTime"/> value
    /// </summary>
    protected const double WidthDateTime = 8;

    /// <summary>
    /// Width of a <see cref="Double"/> value
    /// </summary>
    protected const double WidthDouble = 10;

    /// <summary>
    /// Width of an integer value
    /// </summary>
    protected const double WidthInteger = 8;

    /// <summary>
    /// Current header style name
    /// </summary>
    protected string HeaderStyleName;

    /// <summary>
    /// Current footer style name
    /// </summary>
    protected string FooterStyleName;

    #endregion

    /// <summary>
    /// Current styleset to use
    /// </summary>
    public IStyleSet StyleSet { get; private set; }

    /// <summary>
    /// Current started table
    /// </summary>
    public Table Table { get; protected set; }

    /// <summary>
    /// The title for the table of content (TOC)
    /// </summary>
    public string TitleTableOfContent { get; set; } = "Table of content";

    /// <summary>
    /// The title for the table of figures (TOF)
    /// </summary>
    public string TitleTableOfFigures { get; set; } = "Table of figures";

    /// <summary>
    /// The title for the table of equations (TOE)
    /// </summary>
    public string TitleTableOfEquations { get; set; } = "Table of equations";

    /// <summary>
    /// The title for the table of tables (TOT)
    /// </summary>
    public string TitleTableOfTables { get; set; } = "Table of tables";

    /// <summary>
    /// The word written before the page number in a page footer
    /// </summary>
    public string PageNumberPrefix { get; set; } = "Page";

    /// <summary>
    /// Increment
    /// </summary>
    public int Increment { get; set; }

    /// <summary>
    /// Add a page break if necessary
    /// </summary>
    public bool AddPageBreakIfNecessary { get; set; }

    /// <summary>
    /// Get the current width of the page.
    /// </summary>
    public double Width
    {
        get
        {
            double w;

            var ps = StyleSet.PageSetup;

            if (ps.Orientation == Orientation.Landscape)
            {
                w = ps.PageHeight.Centimeter - ps.RightMargin.Centimeter -
                    ps.LeftMargin.Centimeter;
                return w;
            }

            w = ps.PageWidth.Centimeter - ps.RightMargin.Centimeter -
                ps.LeftMargin.Centimeter;

            return w;
        }
    }

    /// <summary>
    /// Save Pdf to a file
    /// </summary>
    /// <param name="fileName">Full path for pdf file's destination</param>
    /// <param name="showPdfFile">Show Pdf-File in a viewer</param>
    public virtual void RenderToPdf(string fileName, bool showPdfFile)
    {

        var renderer = new PdfDocumentRenderer { Document = Document };
        renderer.RenderDocument();

        if (!string.IsNullOrEmpty(StyleSet.DocumentMetaData.WatermarkText))
        {
            CreateWatermark(renderer.PdfDocument);
        }

        // Save the document...
        renderer.PdfDocument.Save(fileName);

        if (!showPdfFile)
        {
            return;
        }

        OpenFile(fileName);
    }

    /// <summary>
    /// Create a watermark
    /// </summary>
    /// <param name="pdf">Current PDF document</param>
    protected void CreateWatermark(PdfDocument pdf)
    {
        var watermark = StyleSet.DocumentMetaData.WatermarkText;

        var style = StyleSet.Watermark;

        var emSize = MeasurementHelper.GetEmFromPt(style.Font.Size.Point);

        var font = new XFont(style.Font.Name, emSize, XFontStyleEx.BoldItalic);

        for (var idx = 0; idx < pdf.Pages.Count; idx++)
        {
            var page = pdf.Pages[idx];
            // Get an XGraphics object for drawing above the existing content
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);

            // Get the size (in point) of the text
            var size = gfx.MeasureString(watermark, font);

            // Define a rotation transformation at the center of the page
            gfx.TranslateTransform(page.Width.Point / 2, page.Height.Point / 2);
            gfx.RotateTransform(-Math.Atan(page.Height.Point / page.Width.Point) * 180 / Math.PI);
            gfx.TranslateTransform(-page.Width.Point / 2, -page.Height.Point / 2);

            // Create a dimmed brush
            XBrush brush = new XSolidBrush(XColor.FromArgb(128, (int)style.Font.Color.R, (int)style.Font.Color.G, (int)style.Font.Color.B));

            // Create a string format
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Near
            };

            // Draw the string
            gfx.DrawString(watermark, font, brush,
                new XPoint((page.Width.Point - size.Width) / 2, (page.Height.Point - size.Height) / 2),
                format);
        }
    }

    /// <summary>
    /// Open the newly created file
    /// </summary>
    /// <param name="fileName">Full file patj</param>
    protected static void OpenFile(string fileName)
    {
        // ...and start a viewer.
        var p = new Process
        {
            StartInfo = new ProcessStartInfo(fileName)
            {
                UseShellExecute = true
            }
        };
        p.Start();
    }

    /// <summary>
    /// Save Pdf to a stream
    /// </summary>
    /// <param name="stream">Stream</param>
    public virtual void RenderToPdf(Stream stream)
    {
        var renderer = new PdfDocumentRenderer { Document = Document };
        renderer.RenderDocument();

        if (!string.IsNullOrEmpty(StyleSet.DocumentMetaData.WatermarkText))
        {
            CreateWatermark(renderer.PdfDocument);
        }

        renderer.PdfDocument.Save(stream);
    }

    /// <summary>
    /// Set general document information
    /// </summary>
    /// <param name="title">Title of the file</param>
    /// <param name="subject">Subject of the file</param>
    /// <param name="author">Author of the file</param>
    public void SetDocInfo(string title, string subject, string author)
    {
        Document.Info.Title = title;
        Document.Info.Subject = subject;
        Document.Info.Author = author;
    }

    /// <summary>
    /// Set the page settings for content section
    /// </summary>
    /// <param name="style">Current page style</param>
    /// <param name="styleSet">Current page setup</param>
    /// <returns>Page setup for individual settings</returns>
    public static void SetPage(ITypoPageStyle style, IStyleSet styleSet)
    {
        var format = style.TypoPaperFormat.PaperFormatName.ToLowerInvariant() switch
        {
            "a0" => PageFormat.A0,
            "a1" => PageFormat.A1,
            "a2" => PageFormat.A2,
            "a3" => PageFormat.A3,
            "a4" => PageFormat.A4,
            "a5" => PageFormat.A5,
            "b5" => PageFormat.B5,
            "ledger" => PageFormat.Ledger,
            "letter" => PageFormat.Letter,
            "legal" => PageFormat.Legal,
            "p11x17" => PageFormat.P11x17,
            _ => PageFormat.A4
        };

        // More than one column
        if (style.NumberOfColumns > 1)
        {
            // Set up the page setup for the overall page
            styleSet.PageSetupOriginal = new PageSetup
            {
                PageHeight = Unit.FromCentimeter(style.TypoPaperFormat.Size.Height),
                PageWidth = Unit.FromCentimeter(style.TypoPaperFormat.Size.Width)
            };
            styleSet.PageSetupOriginal.Orientation = styleSet.PageSetupOriginal.PageWidth > styleSet.PageSetupOriginal.PageHeight
                ? Orientation.Landscape
                : Orientation.Portrait;
            styleSet.PageSetupOriginal.PageFormat = format;
            styleSet.PageSetupOriginal.LeftMargin = Unit.FromCentimeter(style.TypoMargins.Left);
            styleSet.PageSetupOriginal.TopMargin = Unit.FromCentimeter(style.TypoMargins.Top);
            styleSet.PageSetupOriginal.RightMargin = Unit.FromCentimeter(style.TypoMargins.Right);
            styleSet.PageSetupOriginal.BottomMargin = Unit.FromCentimeter(style.TypoMargins.Bottom);

            // Set up the page setup for the page parts
            styleSet.PageSetup.PageHeight = Unit.FromCentimeter(style.TypoPaperFormat.Size.Height);
            styleSet.PageSetup.PageWidth = Unit.FromCentimeter(style.ColumnWidth);
            styleSet.PageSetup.Orientation = Orientation.Portrait;
            styleSet.PageSetup.PageFormat = format;
            styleSet.PageSetup.LeftMargin = 0;
            styleSet.PageSetup.TopMargin = Unit.FromCentimeter(style.TypoMargins.Top);
            styleSet.PageSetup.RightMargin = 0;
            styleSet.PageSetup.BottomMargin = Unit.FromCentimeter(style.TypoMargins.Bottom);
            return;
        }

        // One column
        styleSet.PageSetup.PageHeight = Unit.FromCentimeter(style.TypoPaperFormat.Size.Height);
        styleSet.PageSetup.PageWidth = Unit.FromCentimeter(style.TypoPaperFormat.Size.Width);
        styleSet.PageSetup.Orientation = styleSet.PageSetup.PageWidth > styleSet.PageSetup.PageHeight
            ? Orientation.Landscape
            : Orientation.Portrait;
        styleSet.PageSetup.PageFormat = format;
        styleSet.PageSetup.LeftMargin = Unit.FromCentimeter(style.TypoMargins.Left);
        styleSet.PageSetup.TopMargin = Unit.FromCentimeter(style.TypoMargins.Top);
        styleSet.PageSetup.RightMargin = Unit.FromCentimeter(style.TypoMargins.Right);
        styleSet.PageSetup.BottomMargin = Unit.FromCentimeter(style.TypoMargins.Bottom);


    }

    /// <summary>
    /// Add a ney style based on style "Normal"
    /// </summary>
    /// <param name="styleName">Name of the new style</param>
    /// <returns>New style object</returns>
    public Style AddStyle(string styleName)
    {
        // Check, if style already exists
        var i = Document.Styles.GetIndex(styleName);
        return i >= 0 ? Document.Styles[styleName] : Document.Styles.AddStyle(styleName, "Normal");

        // Create new style
    }

    /// <summary>
    /// Add a style to document
    /// </summary>
    /// <param name="style">Style</param>
    /// <returns>Added style</returns>
    public Style AddStyle(Style style)
    {
        if (style is null)
        {
            return null;
        }

        Document.Styles.Add(style);

        return style;
    }

    /// <summary>
    /// Add a ney style based on another style
    /// </summary>
    /// <param name="styleName">Style name</param>
    /// <param name="baseStyleName">name of the style, the new one is based on</param>
    /// <returns>Added style</returns>
    public Style AddStyle(string styleName, string baseStyleName)
    {
        // Check, if style already exists
        var i = Document.Styles.GetIndex(styleName);
        return i >= 0 ? Document.Styles[styleName] : Document.Styles.AddStyle(styleName, baseStyleName);
    }

    /// <summary>
    /// Add a content section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void CreateContentSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        //if (_content is null)
        //{
        Content = Document.AddSection();
        Content.PageSetup = StyleSet.PageSetup.Clone();

        if (isRestartPageNumberingRequired)
        {
            Content.PageSetup.StartingNumber = 1;
        }

        var si = new SectionInfo
        {
            Section = Content,
            IsRestartPageNumberingRequired = isRestartPageNumberingRequired,
            PageNumberFormat = pageNumberFormat
        };
        SectionInfos.Add(si);

        var par = Content.AddParagraph(string.Empty);
        par.Format.SpaceBefore = 0;
        par.Format.Font.Size = 2;

        AddHeaderInternal(Content, pageNumberFormat);
        AddFooterInternal(Content, pageNumberFormat);

        //}
    }


    /// <summary>
    /// Add a TOC section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void CreateTocSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        Toc = Document.AddSection();
        Toc.PageSetup = StyleSet.PageSetup.Clone();
        Content = Toc;

        if (isRestartPageNumberingRequired)
        {
            Content.PageSetup.StartingNumber = 1;
        }

        var si = new SectionInfo
        {
            Section = Toc,
            IsRestartPageNumberingRequired = isRestartPageNumberingRequired,
            PageNumberFormat = pageNumberFormat
        };
        SectionInfos.Add(si);

        var par = Content.AddParagraph(string.Empty);
        par.Format.SpaceBefore = 0;
        par.Format.Font.Size = 2;

        AddHeaderInternal(Toc, pageNumberFormat);
        AddFooterInternal(Toc, pageNumberFormat);

        var p = Toc.AddParagraph(TitleTableOfContent, "TocHeading");
        p.AddBookmark("Content");
    }

    /// <summary>
    /// Add a TOF section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void CreateTofSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        Tof = Document.AddSection();
        Tof.PageSetup = StyleSet.PageSetup.Clone();
        Content = Tof;

        if (isRestartPageNumberingRequired)
        {
            Content.PageSetup.StartingNumber = 1;
        }

        var si = new SectionInfo
        {
            Section = Tof,
            IsRestartPageNumberingRequired = isRestartPageNumberingRequired,
            PageNumberFormat = pageNumberFormat
        };
        SectionInfos.Add(si);

        var par = Content.AddParagraph(string.Empty);
        par.Format.SpaceBefore = 0;
        par.Format.Font.Size = 2;

        AddHeaderInternal(Tof, pageNumberFormat);
        AddFooterInternal(Tof, pageNumberFormat);

        var p = Tof.AddParagraph(TitleTableOfFigures, "TofHeading");
        p.AddBookmark("Figures");
    }

    /// <summary>
    /// Add a TOF section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void CreateToeSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        Toe = Document.AddSection();
        Toe.PageSetup = StyleSet.PageSetup.Clone();
        Content = Toe;

        if (isRestartPageNumberingRequired)
        {
            Content.PageSetup.StartingNumber = 1;
        }

        var si = new SectionInfo
        {
            Section = Toe,
            IsRestartPageNumberingRequired = isRestartPageNumberingRequired,
            PageNumberFormat = pageNumberFormat
        };
        SectionInfos.Add(si);

        var par = Content.AddParagraph(string.Empty);
        par.Format.SpaceBefore = 0;
        par.Format.Font.Size = 2;

        AddHeaderInternal(Toe, pageNumberFormat);
        AddFooterInternal(Toe, pageNumberFormat);

        var p = Toe.AddParagraph(TitleTableOfEquations, "ToeHeading");
        p.AddBookmark("Equations");
    }

    /// <summary>
    /// Add a TOT section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void CreateTotSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        Tot = Document.AddSection();
        Content = Tot;
        Tot.PageSetup = StyleSet.PageSetup.Clone();

        if (isRestartPageNumberingRequired)
        {
            Content.PageSetup.StartingNumber = 1;
        }

        var si = new SectionInfo
        {
            Section = Tot,
            IsRestartPageNumberingRequired = isRestartPageNumberingRequired,
            PageNumberFormat = pageNumberFormat
        };
        SectionInfos.Add(si);

        var par = Content.AddParagraph(string.Empty);
        par.Format.SpaceBefore = 0;
        par.Format.Font.Size = 2;

        AddHeaderInternal(Tot, pageNumberFormat);
        AddFooterInternal(Tot, pageNumberFormat);

        var p = Tot.AddParagraph(TitleTableOfTables, "TotHeading");
        p.AddBookmark("Tables");

    }

    /// <summary>
    /// Add an TOC entry level 1 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    public Paragraph AddToc1Entry(string text, string tag)
    {
        return AddTocEntryInternal(1, text, tag);
    }

    /// <summary>
    /// Add an TOC entry level 2 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    public Paragraph AddToc2Entry(string text, string tag)
    {
        return AddTocEntryInternal(2, text, tag);
    }

    /// <summary>
    /// Add an TOC entry level 3 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    public Paragraph AddToc3Entry(string text, string tag)
    {
        return AddTocEntryInternal(3, text, tag);
    }

    /// <summary>
    /// Add an TOC entry level 4 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    public Paragraph AddToc4Entry(string text, string tag)
    {
        return AddTocEntryInternal(4, text, tag);
    }

    /// <summary>
    /// Add an TOC entry level 5 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    public Paragraph AddToc5Entry(string text, string tag)
    {
        return AddTocEntryInternal(5, text, tag);
    }

    /// <summary>
    /// Add an entry to the TOC
    /// </summary>
    /// <param name="level">Heading level</param>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    protected virtual Paragraph AddTocEntryInternal(int level, string text, string tag)
    {
        var p = Toc.AddParagraph();
        p.Style = $"TOC{level}";

        var hyperlink = p.AddHyperlink(tag);
        hyperlink.AddText($"{text}\t");
        hyperlink.AddPageRefField(tag);

        return p;
    }

    /// <summary>
    /// Add an entry to the TOE
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced equation</param>
    public virtual Paragraph AddToeEntry(string text, string tag)
    {
        var p = Toe.AddParagraph();
        p.Style = "TOE";

        var hyperlink = p.AddHyperlink(tag);
        hyperlink.AddText($"{text}\t");
        hyperlink.AddPageRefField(tag);

        return p;
    }

    /// <summary>
    /// Add an entry to the TOF
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced figure</param>
    public virtual Paragraph AddTofEntry(string text, string tag)
    {
        var p = Tof.AddParagraph();
        p.Style = "TOF";

        var hyperlink = p.AddHyperlink(tag);
        hyperlink.AddText($"{text}\t");
        hyperlink.AddPageRefField(tag);

        return p;
    }

    /// <summary>
    /// Add an entry to the TOT
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    public virtual Paragraph AddTotEntry(string text, string tag)
    {
        var p = Tot.AddParagraph();
        p.Style = "TOT";

        var hyperlink = p.AddHyperlink(tag);
        hyperlink.AddText($"{text}\t");
        hyperlink.AddPageRefField(tag);

        return p;
    }

    /// <summary>
    /// Add a heading 1 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    public Paragraph AddHeading1(string text, string tag)
    {
        return AddHeadingInternal(1, text, tag);
    }
    /// <summary>
    /// Add a heading 2 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    public Paragraph AddHeading2(string text, string tag)
    {
        return AddHeadingInternal(2, text, tag);
    }

    /// <summary>
    /// Add a heading 3 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    public Paragraph AddHeading3(string text, string tag)
    {
        return AddHeadingInternal(3, text, tag);
    }

    /// <summary>
    /// Add a heading 4 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    public Paragraph AddHeading4(string text, string tag)
    {
        return AddHeadingInternal(4, text, tag);
    }

    /// <summary>
    /// Add a heading 5 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    public Paragraph AddHeading5(string text, string tag)
    {
        return AddHeadingInternal(5, text, tag);
    }

    /// <summary>
    /// Add a heading 1 to the content section
    /// </summary>
    /// <param name="level">Heading level</param>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    protected virtual Paragraph AddHeadingInternal(int level, string text, string tag)
    {
        var paragraph = new Paragraph
        {
            Style = $"Heading{level}"
        };

        paragraph.AddBookmark(tag);
        paragraph.AddText(text ?? string.Empty);

        Content.Add(paragraph);

        return paragraph;
    }


    /// <summary>
    /// Add a paragraph to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    public Paragraph AddParagraph(string text)
    {
        var paragraph = new Paragraph();
        paragraph.AddText(text ?? string.Empty);
        Content.Add(paragraph);
        return paragraph;
    }

    /// <summary>
    /// Add a paragraph to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="styleName">Name of the style to use</param>
    public virtual Paragraph AddParagraph(string text, string styleName)
    {
        if (string.IsNullOrEmpty(styleName))
        {
            styleName = "Normal";
        }
        if (string.IsNullOrEmpty(text))
        {
            text = string.Empty;
        }

        var paragraph = new Paragraph();

        paragraph.AddText(text);

        var i = Document.Styles.GetIndex(styleName);
        if (i < 0)
        {
            throw new ArgumentException($"Stylename {styleName} not found!");
        }

        paragraph.Style = styleName;
        Content.Add(paragraph);
        return paragraph;
    }

    /// <summary>
    /// Add a WARNING paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddWarning(string text)
    {
        return AddParagraph(text, "Warning");
    }

    /// <summary>
    /// Add an INFO paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddInfo(string text)
    {
        return AddParagraph(text, "Info");
    }

    /// <summary>
    /// Add an ERROR paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddError(string text)
    {
        return AddParagraph(text, "Error");
    }

    /// <summary>
    /// Add a CODE paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddCode(string text)
    {
        return AddParagraph(text, "Code");
    }

    /// <summary>
    /// Add a CITATION paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <param name="source">Source for the citation</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddCitation(string text, string source)
    {
        var p = AddParagraph(text, "Citation");

        AddParagraph(source, "CitationSource");

        return p;
    }

    /// <summary>
    /// Add a left-aligned paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddParagraphLeft(string text)
    {
        var p = AddParagraph(text, "Normal");
        return p;
    }

    /// <summary>
    /// Add a title
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddTitle(string text)
    {
        var p = AddParagraph(text, "Title");
        return p;
    }

    /// <summary>
    /// Add a subtitle
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddSubtitle(string text)
    {
        var p = AddParagraph(text, "Subtitle");
        return p;
    }

    /// <summary>
    /// Add a section title
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddSectionTitle(string text)
    {
        var p = AddParagraph(text, "SectionTitle");
        return p;
    }

    /// <summary>
    /// Add a section subtitle
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddSectionSubtitle(string text)
    {
        var p = AddParagraph(text, "SectionSubtitle");
        return p;
    }

    /// <summary>
    /// Add a right-aligned paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddParagraphRight(string text)
    {
        var p = AddParagraph(text, "ParagraphRight");
        return p;
    }

    /// <summary>
    /// Add a centered paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddParagraphCenter(string text)
    {
        var p = AddParagraph(text, "ParagraphCenter");
        return p;
    }

    /// <summary>
    /// Add a justified paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    public Paragraph AddParagraphJustify(string text)
    {
        var p = AddParagraph(text, "ParagraphJustify");
        return p;
    }

    /// <summary>
    /// Add a paragraph object to the content
    /// </summary>
    /// <param name="paragraph">Paragraph to add</param>
    public void AddParagraph(Paragraph paragraph)
    {
        var i = Document.Styles.GetIndex(paragraph.Style);
        if (i < 0)
        {
            throw new ArgumentException($"Stylename {paragraph.Style} not found!");
        }

        Content.Add(paragraph);
    }

    /// <summary>
    /// Add an empty paragraph to the content
    /// </summary>
    /// <param name="addPageBreak">Add a page break before the empty paragraph</param>
    public void AddEmpty(bool addPageBreak = false)
    {
        if (addPageBreak)
        {
            Content.AddPageBreak();
        }
        Content.AddParagraph("", "Empty");
    }

    /// <summary>
    /// Add a figure to the document
    /// </summary>
    /// <param name="imagePath">Full file path to the figure image</param>
    /// <param name="legend">Legend for the figure</param>
    /// <param name="tag">Link tag name</param>
    /// <param name="width">Width in cm</param>
    /// <param name="height">Height in cm</param>
    public virtual void AddFigure(string imagePath, string legend, string tag, double width, double height)
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

        p.AddBookmark(tag);
    }

    /// <summary>
    /// Add a figure to the document
    /// </summary>
    /// <param name="imagePath">Full file path to the equation image</param>
    /// <param name="legend">Legend for the equation</param>
    /// <param name="tag">Link tag name</param>
    /// <param name="width">Width in cm</param>
    /// <param name="height">Height in cm</param>
    public virtual void AddEquation(string imagePath, string legend, string tag, double width, double height)
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

        p.AddBookmark(tag);
    }

    /// <summary>
    /// Set a footer text for content and toc
    /// </summary>
    public void SetFooter()
    {
        SetFooter("Footer");
    }

    /// <summary>
    /// Set a footer text for content and toc
    /// </summary>

    public void SetFooter(string styleName)
    {
        FooterStyleName = styleName;
    }

    /// <summary>
    /// Add a footer. Override this method if you want to implement another footer
    /// </summary>
    /// <param name="section">Section to add the footer to</param>
    /// <param name="pageNumberFormat">Null or ROMAN, roman, ALPHABETIC, alphabetic</param>
    protected virtual void AddFooterInternal(Section section, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        var md = StyleSet.DocumentMetaData;

        if (section is null || string.IsNullOrEmpty(md.FooterTemplate))
        {
            return;
        }

        var para = new Paragraph
        {
            Style = FooterStyleName
        };

        var sections = md.FooterTemplate.ToLowerInvariant().Split('|');

        // Draw left element
        CreateHeaderFooterElement(md, sections[0], para, 0, false, pageNumberFormat);

        // Draw middle element
        CreateHeaderFooterElement(md, sections[1], para, 1, false, pageNumberFormat);

        // Draw right element
        CreateHeaderFooterElement(md, sections[2], para, 2, false, pageNumberFormat);

        section.Footers.Primary.Add(para);
    }

    private void CreateHeaderFooterElement(ITypoMetaData documentMetaData, string section, Paragraph para, int position, bool isHeader, PageNumberFormatEnum pageNumberFormat)
    {
        ArgumentNullException.ThrowIfNull(documentMetaData);

        var width = StyleSet.PageSetup.PageWidth.Point -
                                        StyleSet.PageSetup.LeftMargin.Point -
                                        StyleSet.PageSetup.RightMargin.Point;

        if (position == 1)
        {
            para.Format.TabStops.ClearAll();
            para.Format.AddTabStop(width / 2, TabAlignment.Center);
            para.Format.AddTabStop(width, TabAlignment.Right);
            para.AddText("\t");
        }

        // Logo
        if (section == ITypography.LogoIndicator && !string.IsNullOrEmpty(documentMetaData.LogoPath))
        {
            var image = para.AddImage(documentMetaData.LogoPath);
            image.Width = Unit.FromCentimeter(documentMetaData.LogoWidth);
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Left = ShapePosition.Left;
            image.Top = ShapePosition.Center;
            image.LockAspectRatio = true;
            image.WrapFormat.Style = WrapStyle.Through;
        }

        // Footer / header text
        if (section == ITypography.TextIndicator)
        {
            var text = isHeader ? documentMetaData.HeaderText : documentMetaData.FooterText;

            if (string.IsNullOrEmpty(text))
            {
                text = documentMetaData.Title;
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }
            }

            para.AddText(text);
        }

        // Footer / header text
        if (section == ITypography.CompanyIndicator)
        {
            var text = documentMetaData.Company;

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            para.AddText(text);
        }

        // Page number
        if (section == ITypography.PageFieldIndicator)
        {
            para.AddText($"{PageNumberPrefix} ");

            var p = para.AddPageField();

            switch (pageNumberFormat)
            {
                case PageNumberFormatEnum.UpperRoman:
                    p.Format = "ROMAN";
                    break;
                case PageNumberFormatEnum.LowerRoman:
                    p.Format = "roman";
                    break;
                case PageNumberFormatEnum.UpperLatin:
                    p.Format = "ALPHABETIC";
                    break;
                case PageNumberFormatEnum.LowerLatin:
                    p.Format = "alphabetic";
                    break;
                case PageNumberFormatEnum.Decimal:
                default:
                    break;
            }

            //para.AddNumPagesField();
        }

        // Date
        if (section == ITypography.DateIndicator)
        {
            var text = DateTime.Now.ToString("d", documentMetaData.CultureInfo);
            para.AddText(text);
        }

        // DateTime
        if (section == ITypography.DateTimeIndicator)
        {
            var text = DateTime.Now.ToString("g", documentMetaData.CultureInfo);
            para.AddText(text);
        }

        if (position == 1)
        {
            para.AddText("\t");
        }
    }

    /// <summary>
    /// Set a header for the document
    /// </summary>
    public void SetHeader()
    {
        SetHeader("Header");
    }

    /// <summary>
    /// Set a header for the document
    /// </summary>

    /// <param name="styleName">Name of the style to use for the header</param>
    public void SetHeader(string styleName)
    {
        HeaderStyleName = styleName;
    }

    /// <summary>
    /// Add a header. Override this method if you want to implement another header
    /// </summary>
    /// <param name="section">Section to add the header to</param>
    /// <param name="pageNumberFormat">Null or ROMAN, roman, ALPHABETIC, alphabetic</param>
    protected virtual void AddHeaderInternal(Section section, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        var md = StyleSet.DocumentMetaData;

        if (section is null || string.IsNullOrEmpty(md.HeaderTemplate))
        {
            return;
        }

        // Background image
        if (!string.IsNullOrEmpty(md.BackgroundImagePath) && File.Exists(md.BackgroundImagePath))
        {
            var image = section.Headers.Primary.AddImage(md.BackgroundImagePath);
            image.Height = section.PageSetup.PageHeight;
            image.Width = section.PageSetup.PageWidth;
            image.RelativeVertical = RelativeVertical.Page;
            image.RelativeHorizontal = RelativeHorizontal.Page;
            image.WrapFormat.Style = WrapStyle.Through;
        }

        // Header
        var para = new Paragraph
        {
            Style = HeaderStyleName
        };

        var sections = md.HeaderTemplate.ToLowerInvariant().Split('|');

        // Draw left element
        CreateHeaderFooterElement(md, sections[0], para, 0, true, pageNumberFormat);

        // Draw middle element
        CreateHeaderFooterElement(md, sections[1], para, 1, true, pageNumberFormat);

        // Draw right element
        CreateHeaderFooterElement(md, sections[2], para, 2, true, pageNumberFormat);

        section.Headers.Primary.Add(para);
    }

    /// <summary>
    /// Add a table to the document
    /// </summary>
    public void AddTable(PdfTable dt)
    {
        AddTableInternal(Content, dt);
    }

    private Table AddTableInternal(DocumentObject documentObject, PdfTable dt)
    {
        const int columnMaxLength = 25;

        // Load the table style
        var style = Document.Styles[dt.TableStyleName];

        ArgumentNullException.ThrowIfNull(style);

        // Add a heading
        if (!string.IsNullOrEmpty(dt.Heading))
        {
            AddParagraph(dt.Heading, dt.HeadingStyleName);
        }

        // Add aditional infos

        if (!string.IsNullOrEmpty(dt.AdditionalInfos))
        {
            AddParagraph(dt.AdditionalInfos, dt.AdditionalInfosStyleName);
        }

        // Create table and set basic table settings
        var borderColor = dt.TableStyle.TypoBorderBrush.TypoColor.ToPdfColor();


        Table table;

        if (documentObject is Section s)
        {
            table = s.AddTable();
        }
        else if (documentObject is TextFrame f)
        {
            table = f.AddTable();
        }
        else
        {
            throw new ArgumentException("Wrong DocumentObject type. Must be Section or TextFrame");
        }

        table.LeftPadding = 2;
        table.RightPadding = 2;
        table.TopPadding = 2;
        table.BottomPadding = 2;
        table.Borders.Left.Width = Unit.FromCentimeter(dt.TableStyle.TypoBorderThickness.Left);
        table.Borders.Top.Width = Unit.FromCentimeter(dt.TableStyle.TypoBorderThickness.Top);
        table.Borders.Right.Width = Unit.FromCentimeter(dt.TableStyle.TypoBorderThickness.Right);
        table.Borders.Bottom.Width = Unit.FromCentimeter(dt.TableStyle.TypoBorderThickness.Bottom);
        table.Borders.Color = borderColor;
        table.Rows.Alignment = RowAlignment.Center;
        table.Style = dt.TableStyleName;

        // Add an empty paragraph to keep distance
        var par = Content.AddParagraph(string.Empty);
        par.Format.SpaceBefore = StyleSet.Table.ParagraphFormat.SpaceBefore;
        par.Format.Font.Size = 2;

        // Calculate maxlength for columns
        for (var i = 0; i < dt.Columns.Count; i++)
        {
            var col = dt.Columns[i];
            if (col.MaxLength < col.ColumnName.Length)
            {
                col.MaxLength = col.ColumnName.Length;
            }
        }

        foreach (var r in dt.Rows)
        {
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var cell = r.Cells[i];

                var col = dt.Columns[i];
                if (col.MaxLength < cell.Content.Length)
                {
                    col.MaxLength = cell.Content.Length;
                }
            }
        }

        foreach (var column in dt.Columns)
        {
            if (column.MaxLength > columnMaxLength)
            {
                //column.
            }
        }




        var colCount = dt.Columns.Count;

        for (var i = 1; i <= colCount; i++)
        {
            var col = dt.Columns[i - 1];

            if (col.MaxLength > 25)
            {
                col.MaxLength = 25;
            }

            var column = table.AddColumn();
            column.Borders.Color = borderColor;
            column.Width = Unit.FromCentimeter(col.MaxLength * 0.16);
            column.Format.Alignment = col.TextAlignment switch
            {
                PdfTextAlignment.Left => ParagraphAlignment.Left,
                PdfTextAlignment.Center => ParagraphAlignment.Center,
                PdfTextAlignment.Right => ParagraphAlignment.Right,
                _ => ParagraphAlignment.Left
            };
        }

        // Kopfzeile schreiben
        var header = table.AddRow();
        header.Shading.Color = dt.TableStyle.TypoTableHeaderBackColor.ToPdfColor();
        header.Format.Font.Color = Colors.Black;
        header.Format.Font.Size = style.Font.Size;
        header.Format.Font.Name = style.Font.Name;

        for (var i = 0; i < table.Columns.Count; i++)
        {
            var cell = header.Cells[i];
            var p = cell.AddParagraph(dt.Columns[i].ColumnName);
            p.Format.Font.Size = style.Font.Size;
            p.Format.Font.Name = style.Font.Name;
            p.Format.Font.Bold = true;
        }

        // Inhaltszeilen schreiben
        var shadow = false;

        foreach (var r in dt.Rows)
        {
            var row = table.AddRow();

            if (r.ShadingColor.HasValue)
            {
                row.Shading.Color = r.ShadingColor.Value;
            }
            else
            {
                row.Shading.Color = shadow ? dt.TableStyle.TypoTableBackColor.ToPdfColor() : dt.TableStyle.TypoTableAlternateBackColor.ToPdfColor();
            }

            for (var i = 0; i < table.Columns.Count; i++)
            {
                var cell = row.Cells[i];

                var dataCell = r.Cells[i];

                var p = cell.AddParagraph(dataCell.Content);
                p.Format.Font.Size = style.Font.Size;
                p.Format.Font.Name = style.Font.Name;
            }

            shadow = !shadow;
        }

        if (string.IsNullOrEmpty(dt.Legend))
        {
            return table;
        }

        CreateTableLegend(dt.Legend, dt.Tag);
        return table;
    }

    /// <summary>
    /// Create a table legend
    /// </summary>
    /// <param name="legend">Legend text</param>
    /// <param name="tag">Bookmark tag</param>
    protected virtual void CreateTableLegend(string legend, string tag)
    {
        Paragraph legendP;

        if (string.IsNullOrEmpty(legend))
        {
            legendP = Content.AddParagraph(" ", "TableLegend");
            legendP.Format.Font.Size = 2;
            legendP.Format.SpaceBefore = 0;
            legendP.Format.SpaceAfter = 0;
        }
        else
        {
            legendP = Content.AddParagraph(legend, "TableLegend");
        }
        if (string.IsNullOrEmpty(tag))
        {
            return;
        }
        legendP.AddBookmark(tag);
    }

    /// <summary>
    /// Add a table to the content. Prefer using AddTable method with <see cref="PdfTable"/> parameter instead
    /// </summary>
    /// <param name="dt">Data to show in the table</param>
    /// <param name="heading">Heading for the table to be presented before the table</param>
    /// <param name="headingStyleName">Style name for the heading</param>
    /// <param name="additionalInfos">Additional info for the table to be presented before the table</param>
    /// <param name="additionalInfosStyleName">Style name for the additonal info</param>
    /// <param name="width"></param>
    /// <param name="tableStyle">Name of the style to use for table formatting (not all properties supported)</param>
    [Obsolete("Do not use it. Prefer using AddTable method with <see cref=\"PdfTable\"/> parameter instead")]
    public void AddTable(DataTable dt, string heading, string headingStyleName, string additionalInfos, string additionalInfosStyleName, double width = 0, string tableStyle = "NormalTable")
    {
        var pdfTable = string.Equals(dt.Columns[0].ColumnName, "cssstyle", StringComparison.InvariantCultureIgnoreCase) ?
            dt.ToPdfTableWithCssInfo(DataTableExtensions.BodoconsultCssColors) :
            dt.ToPdfTable();

        pdfTable.Heading = heading;
        pdfTable.HeadingStyleName = headingStyleName;
        pdfTable.AdditionalInfos = additionalInfos;
        pdfTable.AdditionalInfosStyleName = additionalInfosStyleName;
        pdfTable.TableStyleName = tableStyle;

        AddTable(pdfTable);

        ////if (Math.Abs(width) < 0.000001)
        ////{
        ////    width = Width;
        ////}

        //if (!string.IsNullOrEmpty(heading))
        //{
        //    AddParagraph(heading, headingStyleName);
        //}

        //if (!string.IsNullOrEmpty(additionalInfos))
        //{
        //    AddParagraph(additionalInfos, additionalInfosStyleName);
        //}

        //var style = Document.Styles[tableStyle];
        //if (style is null)
        //{
        //    throw new ArgumentNullException(nameof(style));
        //}


        ////frame.FillFormat.Color = Colors.White;
        //var table = Content.AddTable();
        //table.LeftPadding = 2;
        //table.Borders.Width = 0.5;
        //table.Borders.Color = TableBorderColor;

        //var colCount = dt.Columns.Count;


        //var startCol = 1;
        //var format = new string[dt.Columns.Count];

        //var usedWidth = 0D;
        //var colCountNotUsed = 0;

        //var fontSize = Unit.FromCentimeter(style.Font.Size.Centimeter / 40.0).Point;

        //// Ermittle Breite der Nicht-Text-Spalten und Anzahl der Text-Spalten
        //for (var i = 1; i <= colCount; i++)
        //{
        //    var col = dt.Columns[i - 1];

        //    if (col.ColumnName.ToLower() == "cssstyle") continue;

        //    var t = col.DataType.ToString().Replace("System.", string.Empty).ToLower();

        //    switch (t)
        //    {
        //        case "datetime":
        //            usedWidth += WidthDateTime * fontSize;
        //            break;
        //        case "decimal":
        //        case "double":
        //        case "single":
        //        case "float":
        //            usedWidth += WidthDouble * fontSize;
        //            break;
        //        case "int":
        //        case "int16":
        //        case "int32":
        //        case "int64":
        //            usedWidth += WidthInteger * fontSize;
        //            break;
        //        default:
        //            colCountNotUsed++;
        //            break;
        //    }
        //}

        //// Errechne dann die zur Verfügung stehende maximale Breite der Text-Spalten
        //var widthText = colCountNotUsed > 0 ? Math.Round((Width - usedWidth) / colCountNotUsed, 1) - 0.1 : 2.0;

        //if (widthText > 7.0) widthText = 7.0;

        //for (var i = 1; i <= colCount; i++)
        //{
        //    var col = dt.Columns[i - 1];

        //    if (col.ColumnName.ToLower() == "cssstyle")
        //    {
        //        startCol = 2;
        //        continue;
        //    }

        //    double colWidth;

        //    var column = table.AddColumn();
        //    column.Borders.Color = TableBorderColor;

        //    var t = col.DataType.ToString().Replace("System.", string.Empty).ToLower();
        //    switch (t)
        //    {
        //        case "datetime":
        //            colWidth = WidthDateTime * fontSize;
        //            format[i - 1] = "dd.MM.yyyy";
        //            break;
        //        case "decimal":
        //        case "double":
        //        case "single":
        //            colWidth = WidthDouble * fontSize;
        //            column.Format.Alignment = ParagraphAlignment.Right;
        //            format[i - 1] = "#,##0.00";
        //            break;
        //        case "int":
        //        case "int16":
        //        case "int32":
        //        case "int64":
        //            colWidth = WidthInteger * fontSize;
        //            column.Format.Alignment = ParagraphAlignment.Right;
        //            format[i - 1] = "#,##0";
        //            break;
        //        default:
        //            colWidth = widthText;
        //            column.Format.Alignment = ParagraphAlignment.Left;
        //            break;
        //    }

        //    column.Width = Unit.FromCentimeter(colWidth);
        //}



        //var korr = startCol == 2 ? 1 : 0;

        //// Kopfzeile schreiben
        //var header = table.AddRow();
        //header.Shading.Color = TableHeaderBackgroundColor;
        //header.Format.Font.Color = Colors.Black;
        //header.Format.Font.Size = style.Font.Size;
        //header.Format.Font.Name = style.Font.Name;

        //for (var i = 1; i <= table.Columns.Count; i++)
        //{
        //    var cell = header.Cells[i - 1];
        //    var p = cell.AddParagraph(dt.Columns[i - 1 + korr].ColumnName);
        //    p.Format.Font.Size = style.Font.Size;
        //    p.Format.Font.Name = style.Font.Name;
        //    p.Format.Font.Bold = true;
        //}

        //// Inhaltszeilen schreiben
        //var shadow = false;

        //foreach (DataRow r in dt.Rows)

        ////for (var zeile = schleife * Increment; zeile < (schleife + 1) * Increment; zeile++)
        //{
        //    var row = table.AddRow();
        //    //row.KeepWith = 2;
        //    var css = string.Empty;
        //    if (startCol == 2) css = r[0].ToString();

        //    Color shadingColor;

        //    if (string.IsNullOrEmpty(css))
        //    {
        //        shadingColor = shadow ? TableBackColor : TableAlternateBackColor;
        //    }
        //    else
        //    {
        //        switch (css.ToLower())
        //        {
        //            case "wr_cell_h1":
        //                shadingColor = ShadingH1Color;
        //                break;
        //            case "wr_cell_h2":
        //                shadingColor = ShadingH2Color;
        //                break;
        //            case "wr_cell_h3":
        //                shadingColor = ShadingH3Color;
        //                break;
        //            case "wr_cell_risk1":
        //                shadingColor = ShadingRisk1Color;
        //                break;
        //            case "wr_cell_risk2":
        //                shadingColor = ShadingRisk2Color;
        //                break;
        //            default:
        //                shadingColor = shadow ? TableBackColor : TableAlternateBackColor;
        //                break;
        //        }
        //    }

        //    row.Shading.Color = shadingColor;

        //    for (var i = 0; i < table.Columns.Count; i++)
        //    {
        //        var cell = row.Cells[i];

        //        if (string.IsNullOrEmpty(format[i + korr]))
        //        {
        //            var p = cell.AddParagraph(r[i + korr].ToString() ?? string.Empty);
        //            p.Format.Font.Size = style.Font.Size;
        //            p.Format.Font.Name = style.Font.Name;
        //            //p.Format.Shading.Color = shadingColor;
        //        }
        //        else
        //        {
        //            if (format[i + korr].ToLowerInvariant().Contains("yy"))
        //            {
        //                var z = r[i + korr].ToString();
        //                if (!string.IsNullOrEmpty(z))
        //                {
        //                    var p = cell.AddParagraph(Convert.ToDateTime(z).ToString(format[i + korr]));
        //                    p.Format.Font.Size = style.Font.Size;
        //                    p.Format.Font.Name = style.Font.Name;
        //                    //p.Format.Shading.Color = shadingColor;
        //                }
        //            }
        //            else
        //            {
        //                var z = r[i + korr].ToString();
        //                if (string.IsNullOrEmpty(z))
        //                {
        //                    continue;
        //                }

        //                var p = cell.AddParagraph(Convert.ToDouble(z).ToString(format[i + korr]));
        //                p.Format.Font.Size = style.Font.Size;
        //                p.Format.Font.Name = style.Font.Name;
        //                // p.Format.Shading.Color = shadingColor;
        //            }
        //        }
        //    }

        //    shadow = !shadow;
        //}

        ////var widthTable = table.Columns.Cast<Column>().Aggregate(0D, (current, t) => current + t.Width.Centimeter);
        ////frame.Width = Unit.FromCentimeter(widthTable);

    }


    /// <summary>
    /// Add a table in a separate frame
    /// </summary>
    /// <param name="dt">Data to show in the table</param>
    /// <param name="heading">Heading for the table</param>
    /// <param name="headingStyleName">Style name for the heading</param>
    /// <param name="additionalInfos"></param>
    /// <param name="additionalInfosStyleName"></param>
    /// <param name="width"></param>

    public void AddTableFrame(DataTable dt, string heading, string headingStyleName, string additionalInfos = null, string additionalInfosStyleName = null, double width = 0)
    {

        if (width < 0.000001)
        {
            width = Width;
        }

        if (Math.Abs(width) < 0.000001)
        {
            width = Width;
        }

        var anzahlSchleifen = dt.Rows.Count / Increment + 1;

        if (dt.Rows.Count > Increment / 2 && AddPageBreakIfNecessary) Content.AddPageBreak();

        if (!string.IsNullOrEmpty(heading))
        {
            AddParagraph(heading, headingStyleName);
        }

        if (!string.IsNullOrEmpty(additionalInfos))
        {
            AddParagraph(additionalInfos, additionalInfosStyleName);
        }

        for (var schleife = 0; schleife < anzahlSchleifen; schleife++)
        {
            if (schleife > 0)
            {
                Content.AddPageBreak();
                if (!string.IsNullOrEmpty(heading))
                {
                    var p = Content.AddParagraph($"{heading} (Forts.)");
                    p.Style = headingStyleName;
                }
            }

            var frame = Content.AddTextFrame();
            frame.Height = Unit.FromCentimeter(6F);
            frame.Width = Unit.FromCentimeter(width);
            frame.Left = ShapePosition.Center;

            CreateTable(dt, schleife, frame);
        }
    }

    /// <summary>
    /// Add a definition list with left and right column
    /// </summary>
    /// <param name="dt">List with <see cref="PdfDefinitionListTerm"/> items</param>
    /// <param name="style1">Name of the style to use for left column</param>
    /// <param name="style2">Name of the style to use for right column</param>
    /// <param name="columnWidth1">Column width column 1 in percent</param>
    public void AddDefinitionList(List<PdfDefinitionListTerm> dt, string style1 = "DefinitionListTerm", string style2 = "DefinitionListItem", double columnWidth1 = 0.2)
    {
        const double borderWidth = 0;

        var table = Content.AddTable();
        table.TopPadding = 4;
        table.Borders.Width = borderWidth;
        table.BottomPadding = 4;

        var column1 = table.AddColumn(Unit.FromCentimeter(columnWidth1 * Width));
        column1.Format.Alignment = ParagraphAlignment.Left;
        column1.RightPadding = 2;

        var column2 = table.AddColumn(Unit.FromCentimeter((1 - columnWidth1) * Width));
        column2.Format.Alignment = ParagraphAlignment.Left;
        //column2.LeftPadding = 2;

        foreach (var r in dt)
        {
            var row = table.AddRow();

            row.Borders.Width = borderWidth;
            row.BottomPadding = 2;

            var cell1 = row.Cells[0];
            cell1.Borders.Width = borderWidth;
            var p1 = cell1.AddParagraph(r.Term);

            if (!string.IsNullOrEmpty(style1))
            {
                p1.Style = style1;
            }

            var cell2 = row.Cells[1];
            cell2.Borders.Width = borderWidth;
            foreach (var dl in r.Items)
            {
                var p2 = cell2.AddParagraph(dl);
                if (!string.IsNullOrEmpty(style2))
                {
                    p2.Style = style2;
                }
            }
        }
    }

    /// <summary>
    /// Add a definition list with left and right column
    /// </summary>
    /// <param name="dt">DataTable with two columns</param>
    /// <param name="style1">Name of the style to use for left column</param>
    /// <param name="style2">Name of the style to use for right column</param>
    /// <param name="columnWidth1">Column width column 1 in percent</param>
    public void AddDefinitionList(DataTable dt, string style1 = "DefinitionListTerm", string style2 = "DefinitionListItem", double columnWidth1 = 0.2)
    {
        const double borderWidth = 0;

        var table = Content.AddTable();
        table.TopPadding = 4;
        table.Borders.Width = borderWidth;
        table.BottomPadding = 4;

        var column1 = table.AddColumn(Unit.FromCentimeter(columnWidth1 * Width));
        column1.Format.Alignment = ParagraphAlignment.Left;
        column1.RightPadding = 2;

        var column2 = table.AddColumn(Unit.FromCentimeter((1 - columnWidth1) * Width));
        column2.Format.Alignment = ParagraphAlignment.Left;
        //column2.LeftPadding = 2;

        foreach (DataRow r in dt.Rows)
        {
            var row = table.AddRow();

            row.Borders.Width = borderWidth;
            row.BottomPadding = 2;

            var cell1 = row.Cells[0];
            cell1.Borders.Width = borderWidth;
            var p1 = cell1.AddParagraph(r[0].ToString() ?? string.Empty);

            if (!string.IsNullOrEmpty(style1))
            {
                p1.Style = style1;
            }

            var cell2 = row.Cells[1];
            cell2.Borders.Width = borderWidth;

            var p2 = cell2.AddParagraph(r[1].ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(style2))
            {
                p2.Style = style2;
            }
        }
    }

    /// <summary>
    /// Create a table
    /// </summary>
    /// <param name="dt">Data table</param>
    /// <param name="schleife"></param>
    /// <param name="frame">Current frame</param>
    /// <param name="borderWidth">Border width in pt</param>
    /// <param name="tableStyle">Style to use for the table</param>
    protected void CreateTable(DataTable dt, int schleife, TextFrame frame, double borderWidth = 0.5, string tableStyle = "NormalTable")
    {
        var pdfTable = string.Equals(dt.Columns[0].ColumnName, "cssstyle", StringComparison.InvariantCultureIgnoreCase) ?
            dt.ToPdfTableWithCssInfo(DataTableExtensions.BodoconsultCssColors) :
            dt.ToPdfTable();

        var table = AddTableInternal(frame, pdfTable);

        var widthTable = table.Columns.Cast<Column>().Aggregate(0D, (current, t) => current + t.Width.Centimeter);
        frame.Width = Unit.FromCentimeter(widthTable);

        //var style = Document.Styles[tableStyle];
        //if (style is null)
        //{
        //    throw new ArgumentNullException(nameof(style));
        //}

        ////frame.FillFormat.Color = Colors.White;
        //var table = frame.AddTable();

        //table.Borders.Width = borderWidth;
        //table.BottomPadding = 0;
        //table.TopPadding = 0;

        //if (borderWidth > 0)
        //{
        //    table.Borders.Color = TableBorderColor;
        //}

        //var colCount = dt.Columns.Count;


        //var startCol = 1;
        //var format = new string[dt.Columns.Count];

        //var usedWidth = 0D;
        //var colCountNotUsed = 0;

        //var fontSize = Unit.FromCentimeter(style.Font.Size.Centimeter / 40.0);

        //// Ermittle Breite der Nicht-Text-Spalten und Anzahl der Text-Spalten
        //for (var i = 1; i <= colCount; i++)
        //{
        //    var col = dt.Columns[i - 1];

        //    if (col.ColumnName.ToLower() == "cssstyle")
        //    {
        //        continue;
        //    }

        //    var t = col.DataType.ToString().Replace("System.", string.Empty).ToLower();

        //    switch (t)
        //    {
        //        case "datetime":
        //            usedWidth += WidthDateTime * fontSize.Point;
        //            break;
        //        case "decimal":
        //        case "double":
        //        case "single":
        //        case "float":
        //            usedWidth += WidthDouble * fontSize.Point;
        //            break;
        //        case "int":
        //        case "int16":
        //        case "int32":
        //        case "int64":
        //            usedWidth += WidthInteger * fontSize.Point;
        //            break;
        //        default:
        //            colCountNotUsed++;
        //            break;
        //    }
        //}

        //// Errechne dann die zur Verfügung stehende maximale Breite der Text-Spalten
        //var widthText = colCountNotUsed > 0 ? Math.Round((frame.Width.Centimeter - usedWidth) / colCountNotUsed, 1) - 0.1 : 2.0;

        //if (widthText > 7.0)
        //{
        //    widthText = 7.0;
        //}

        //for (var i = 1; i <= colCount; i++)
        //{
        //    var col = dt.Columns[i - 1];

        //    if (col.ColumnName.ToLower() == "cssstyle")
        //    {
        //        startCol = 2;
        //        continue;
        //    }

        //    double width;

        //    var column = table.AddColumn();
        //    column.Borders.Color = TableBorderColor;

        //    var t = col.DataType.ToString().Replace("System.", string.Empty).ToLower();
        //    switch (t)
        //    {
        //        case "datetime":
        //            width = WidthDateTime * fontSize.Point;
        //            format[i - 1] = "dd.MM.yyyy";
        //            break;
        //        case "decimal":
        //        case "double":
        //        case "single":
        //            width = WidthDouble * fontSize.Point;
        //            column.Format.Alignment = ParagraphAlignment.Right;
        //            format[i - 1] = "#,##0.00";
        //            break;
        //        case "int":
        //        case "int16":
        //        case "int32":
        //        case "int64":
        //            width = WidthInteger * fontSize.Point;
        //            column.Format.Alignment = ParagraphAlignment.Right;
        //            format[i - 1] = "#,##0";
        //            break;
        //        default:
        //            width = widthText;
        //            column.Format.Alignment = ParagraphAlignment.Left;
        //            break;
        //    }

        //    column.Width = Unit.FromCentimeter(width);
        //}



        //var korr = startCol == 2 ? 1 : 0;

        //// Kopfzeile schreiben
        //var header = table.AddRow();
        //header.Shading.Color = TableBackColor;
        //header.Format.Font.Color = Colors.Black;
        //header.Format.Font.Size = style.Font.Size - 0.5;
        //header.Format.Font.Name = style.Font.Name;
        //header.Format.Font.Bold = true;
        //header.Format.SpaceAfter = style.ParagraphFormat.SpaceAfter;
        //header.Format.SpaceBefore = style.ParagraphFormat.SpaceBefore;

        //for (var i = 1; i <= table.Columns.Count; i++)
        //{
        //    var cell = header.Cells[i - 1];
        //    cell.AddParagraph(dt.Columns[i - 1 + korr].ColumnName);

        //}

        //// Inhaltszeilen schreiben
        //var shadow = false;
        //for (var zeile = schleife * Increment; zeile < (schleife + 1) * Increment; zeile++)
        //{
        //    if (zeile >= dt.Rows.Count)
        //    {
        //        break;
        //    }

        //    var r = dt.Rows[zeile];
        //    var row = table.AddRow();

        //    row.BottomPadding = 0;
        //    row.TopPadding = 0;

        //    var css = string.Empty;
        //    if (startCol == 2)
        //    {
        //        css = r[0].ToString();
        //    }

        //    Color shadingColor;

        //    if (string.IsNullOrEmpty(css))
        //    {
        //        shadingColor = shadow ? TableBackColor : TableAlternateBackColor;
        //    }
        //    else
        //    {
        //        switch (css.ToLower())
        //        {
        //            case "wr_cell_h1":
        //                shadingColor = ShadingH1Color;
        //                break;
        //            case "wr_cell_h2":
        //                shadingColor = ShadingH2Color;
        //                break;
        //            case "wr_cell_h3":
        //                shadingColor = ShadingH3Color;
        //                break;
        //            case "wr_cell_risk1":
        //                shadingColor = ShadingRisk1Color;
        //                break;
        //            case "wr_cell_risk2":
        //                shadingColor = ShadingRisk2Color;
        //                break;
        //            default:
        //                shadingColor = shadow ? TableBackColor : TableAlternateBackColor;
        //                break;
        //        }
        //    }

        //    row.Shading.Color = shadingColor;

        //    for (var i = 0; i < table.Columns.Count; i++)
        //    {
        //        var cell = row.Cells[i];
        //        cell.Format.SpaceAfter = 0;
        //        cell.Format.SpaceBefore = 0;

        //        var s = format[i + korr];

        //        if (string.IsNullOrEmpty(s))
        //        {
        //            var p = cell.AddParagraph((r[i + korr].ToString() ?? string.Empty).Trim());
        //            p.Format.Font.Size = style.Font.Size;
        //            p.Format.Font.Name = style.Font.Name;
        //            p.Format.SpaceAfter = style.ParagraphFormat.SpaceAfter;
        //            p.Format.SpaceBefore = style.ParagraphFormat.SpaceBefore;
        //            p.Format.Shading.Color = shadingColor;
        //        }
        //        else
        //        {
        //            if (s.ToLower().Contains("yy"))
        //            {
        //                var z = (r[i + korr].ToString() ?? string.Empty).Trim();
        //                if (string.IsNullOrEmpty(z))
        //                {
        //                    continue;
        //                }
        //                var p = cell.AddParagraph(Convert.ToDateTime(z).ToString(format[i + korr]));
        //                p.Format.Font.Size = style.Font.Size;
        //                p.Format.Font.Name = style.Font.Name;
        //                p.Format.SpaceAfter = style.ParagraphFormat.SpaceAfter;
        //                p.Format.SpaceBefore = style.ParagraphFormat.SpaceBefore;
        //                p.Format.Shading.Color = shadingColor;
        //            }
        //            else
        //            {
        //                var z = r[i + korr].ToString();
        //                if (string.IsNullOrEmpty(z))
        //                {
        //                    continue;
        //                }
        //                var p = cell.AddParagraph(Convert.ToDouble(z).ToString(format[i + korr]));
        //                p.Format.Font.Size = style.Font.Size;
        //                p.Format.Font.Name = style.Font.Name;
        //                p.Format.SpaceAfter = style.ParagraphFormat.SpaceAfter;
        //                p.Format.SpaceBefore = style.ParagraphFormat.SpaceBefore;
        //                p.Format.Shading.Color = shadingColor;
        //            }
        //        }
        //    }

        //    shadow = !shadow;
        //}

        //var widthTable = table.Columns.Cast<Column>().Aggregate(0D, (current, t) => current + t.Width.Centimeter);
        //frame.Width = Unit.FromCentimeter(widthTable);
    }

    /// <summary>
    /// Seitenumbruch in Text einfügen
    /// </summary>
    public void NewPage()
    {
        Content.AddPageBreak();
    }


    /// <summary>
    /// Add a chart
    /// </summary>
    /// <param name="chart"></param>
    public void AddChart(Chart chart)
    {
        Content.Add(chart);
    }

    /// <summary>
    /// Start a table with style NormalTable
    /// </summary>
    public void TableStart()
    {
        TableStart("NormalTable");
    }

    /// <summary>
    /// Start a table with a certain style
    /// </summary>
    /// <param name="style">Style to apply to new table</param>
    public void TableStart(string style)
    {
        var p = Content.AddParagraph();
        p.Style = style;
        p.AddText(" \t\t\t");
        Table = Content.AddTable();
        Table.Borders.Visible = false;
        Table.BottomPadding = 0.3F;
        //_table.Style = "ChartTable";
        Table.TopPadding = 9;

        Content.AddParagraph();
    }

    /// <summary>
    /// Add a column to the currently started table
    /// </summary>
    /// <param name="alignment"></param>
    /// <param name="width"></param>
    public void TableAddColumn(ParagraphAlignment alignment, double width)
    {
        var column = Table.AddColumn();
        column.Format.Alignment = alignment;
        column.Width = Unit.FromCentimeter(width);
    }

    /// <summary>
    /// End the currently started table
    /// </summary>
    public void TableEnd()
    {
        Table = null;
    }

    /// <summary>
    /// Add a row to the currently started table
    /// </summary>
    /// <returns></returns>
    public Row TableAddRow()
    {
        return Table.AddRow();
    }

    /// <summary>
    /// Fill content in a certain table cell defined by row and column number
    /// </summary>
    /// <param name="column">Column number of the cell starting with 0</param>
    /// <param name="row">Row number of the cell starting with 0</param>
    /// <param name="content">Content to fill in the cell</param>
    public void TableSetContent(int column, int row, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            content = string.Empty;
        }

        var cell = Table.Rows[row].Cells[column];
        cell.AddParagraph(content);
    }

    /// <summary>
    /// Fill content in a certain table cell defined by row and column number
    /// </summary>
    /// <param name="column">Column number of the cell starting with 0</param>
    /// <param name="row">Row number of the cell starting with 0</param>
    /// <param name="chart">Chart to fill in the cell</param>
    public void TableSetContent(int column, int row, Chart chart)
    {
        Table.Rows[row][column].Add(chart);

    }

    /// <summary>
    /// Fill image in a certain table cell defined by row and column number
    /// </summary>
    /// <param name="column">Column number of the cell starting with 0</param>
    /// <param name="row">Row number of the cell starting with 0</param>
    /// <param name="imagePath">Image to fill in the cell</param>
    /// <param name="width">Width of the image in cm</param>
    /// <param name="height">Height of the image in cm</param>
    public void TableSetContent(int column, int row, string imagePath, double width, double height)
    {
        var t = Table.Rows[row][column];

        var image = t.AddImage(imagePath);

        image.Width = Unit.FromCentimeter(width);
        image.Height = Unit.FromCentimeter(height);
        //image.Left = 0;
    }

    /// <summary>
    /// Add an image
    /// </summary>
    /// <param name="fileName">Path to the image to add</param>
    /// <param name="width">Width of the image in cm</param>
    /// <param name="height">Height of the image in cm</param>
    public void AddImage(string fileName, double width, double height)
    {
        // Add an empty paragraph to keep distance
        var frame = Content.AddTextFrame();
        frame.Height = Unit.FromCentimeter(height);
        frame.Width = Unit.FromCentimeter(width);
        frame.Left = ShapePosition.Center;

        var p = frame.AddParagraph();
        p.Style = "Image";

        var image = p.AddImage(fileName);
        image.Width = frame.Width;
        image.Height = frame.Height;
        image = null;
    }

    /// <summary>
    /// Fill image in a certain table cell of a small table defined by row and column number
    /// </summary>
    /// <param name="column">Column number of the cell starting with 0</param>
    /// <param name="row">Row number of the cell starting with 0</param>
    /// <param name="data">Data to fill in the cells</param>
    /// <param name="heading">Heading for the table</param>
    /// <param name="width">Width of the image in cm</param>
    /// <param name="height">Height of the image in cm</param>
    public void TableSetContentSmallTable(int column, int row, DataTable data, string heading, double width, double height = 6F)
    {
        var t = Table.Rows[row][column];

        if (!string.IsNullOrEmpty(heading))
        {
            var p = t.AddParagraph(heading);
            p.Style = "NoHeading1";
        }

        var frame = t.AddTextFrame();
        frame.Height = Unit.FromCentimeter(height);
        frame.Left = ShapePosition.Center;
        frame.Width = Unit.FromCentimeter(width);

        CreateTable(data, 0, frame);
    }

    /// <summary>
    /// Create a table from a list of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">Data items to show</param>
    /// <param name="listFields">List of fields to show</param>
    /// <param name="showHeader">Show a header. Default false</param>
    public void AddTable<T>(IList<T> items, IList<PdfTableField> listFields, bool showHeader)
    {

        var myObjectType = typeof(T);

        var fieldInfo = myObjectType.GetProperties().ToList();

        // Header

        var p = Content.AddParagraph();
        p.Style = "NormalTable";
        Table = Content.AddTable();
        Table.Borders.Visible = false;
        //_table.Style = "ChartTable";
        Table.TopPadding = 9;
        Table.Rows.LeftIndent = Unit.FromCentimeter(1);

        foreach (var field in listFields)
        {
            var columnDef = Table.AddColumn(Unit.FromCentimeter(field.Width));
            columnDef.Format.Alignment = field.TextAlign switch
            {
                PdfTextAlignment.Center => ParagraphAlignment.Center,
                PdfTextAlignment.Right => ParagraphAlignment.Right,
                _ => ParagraphAlignment.Left
            };
        }


        // header anzeigen
        var column = 0;
        if (showHeader)
        {
            var header = Table.AddRow();

            foreach (var field in listFields)
            {
                var cell = header.Cells[column];
                cell.AddParagraph(field.Header);
                column++;
            }
        }

        // Show data
        foreach (var item in items)
        {
            var row = Table.AddRow();

            column = 0;
            foreach (var field in listFields)
            {
                var f = field;
                var info = fieldInfo.FirstOrDefault(x => x.Name == f.Name);

                if (info is null) continue;

                string value;

                if (string.IsNullOrEmpty(field.Format))
                {
                    value = info.GetValue(item, null)?.ToString() ?? string.Empty;
                }
                else
                {
                    var x = $"{{0:{field.Format}}}";
                    value = string.Format(x, info.GetValue(item, null));

                }

                var cell = row.Cells[column];
                cell.AddParagraph(value);
                column++;
            }
        }

    }

    /// <summary>
    /// Add an HTML code
    /// </summary>
    /// <param name="html">HTML code to add</param>
    public void AddHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            html = string.Empty;
        }

        if (!html.Contains('<'))
        {
            AddParagraph(html);
            return;
        }

        html = html.Replace("&nbsp;", " ").Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("\r\n\r\n", "\r\n");

        var startTag = html.IndexOf("<", StringComparison.InvariantCultureIgnoreCase);


        while (startTag > -1 && startTag < html.Length - 1)
        {
            var nextLetter = html.Substring(startTag + 1, 2);

            switch (nextLetter.ToLower())
            {
                case "p ":
                case "p>":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":

                    var endTag = html.IndexOf("<", startTag + 2, StringComparison.InvariantCultureIgnoreCase);
                    var endTagStartTag = html.IndexOf(">", startTag + 1, StringComparison.InvariantCultureIgnoreCase);

                    var content = html.Substring(endTagStartTag + 1, endTag - endTagStartTag - 1);

                    switch (nextLetter.ToLower())
                    {
                        case "p ":
                        case "p>":
                            AddParagraph(content);
                            break;
                        case "h1":
                            AddParagraph(content, "Heading1");
                            break;
                        case "h2":
                            AddParagraph(content, "Heading2");
                            break;
                        case "h3":
                            AddParagraph(content, "Heading3");
                            break;
                        case "h4":
                            AddParagraph(content, "Heading4");
                            break;
                        case "h5":
                            AddParagraph(content, "Heading5");
                            break;
                    }

                    startTag = endTag + 1;
                    break;
                    //default:
                    //    break;
            }

            startTag = html.IndexOf("<", startTag + 1, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    /// <summary>
    /// Create a footer with three section left, middle and right
    /// </summary>
    /// <param name="footerLeft">Content of left footer section</param>
    /// <param name="footerMiddle">Content of left middle section</param>
    /// <param name="footerRight">Content of left right section</param>
    /// <param name="styleName">Style to use for the footer</param>
    public void CreateFooter3(string footerLeft, string footerMiddle, string footerRight, string styleName)
    {
        var table = Content.Footers.Primary.AddTable();
        table.Borders.Visible = false;
        table.TopPadding = 9;
        //table.Rows.LeftIndent = Unit.FromCentimeter(1);

        var w = Unit.FromCentimeter(Width / 3);

        var col = table.AddColumn();
        col.Width = w;

        col = table.AddColumn();
        col.Format.Alignment = ParagraphAlignment.Center;
        col.Width = w;

        col = table.AddColumn();
        col.Format.Alignment = ParagraphAlignment.Right;
        col.Width = w;

        table.AddRow();

        var cell = table.Rows[0][0];
        var p = cell.AddParagraph(footerLeft);
        p.Style = styleName;

        cell = table.Rows[0][1];
        p = cell.AddParagraph(footerMiddle);
        p.Style = styleName;


        cell = table.Rows[0][2];
        p = cell.AddParagraph(footerRight);
        p.Style = styleName;


    }

    /// <summary>
    /// Add a pagebreak to the content section
    /// </summary>
    public void AddPageBreak()
    {
        Content.AddPageBreak();
    }


    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Content = null;
        Toc = null;
        Document = null;
    }

    /// <summary>
    /// Load default values
    /// </summary>
    protected void LoadDefaults()
    {
        AddPageBreakIfNecessary = false;
        Increment = 21;
    }

    /// <summary>
    /// Load the styleset
    /// </summary>
    /// <param name="styleSet">Styleset to load</param>
    /// <exception cref="ArgumentException">Style Normal is not existing</exception>
    protected void LoadStyleset(IStyleSet styleSet)
    {
        StyleSet = styleSet;

        var md = StyleSet.DocumentMetaData;

        SetDocInfo(md.Title, md.Description, md.Authors);

        //ObjectHelper.MapProperties(_ps, _document.DefaultPageSetup);

        var style = Document.Styles["Normal"];

        ArgumentNullException.ThrowIfNull(style);

        // ToDo complete clone of NORMAL style
        style.Font.Name = styleSet.Normal.Font.Name;
        style.Font.Size = styleSet.Normal.Font.Size;
        style.ParagraphFormat.SpaceBefore = styleSet.Normal.ParagraphFormat.SpaceBefore;
        style.ParagraphFormat.SpaceAfter = styleSet.Normal.ParagraphFormat.SpaceAfter;
        style.ParagraphFormat.PageBreakBefore = styleSet.Normal.ParagraphFormat.PageBreakBefore;
        style.ParagraphFormat.Alignment = styleSet.Normal.ParagraphFormat.Alignment;

        ObjectHelper.MapProperties(styleSet.Normal, style);
        ObjectHelper.MapProperties(styleSet.Normal.Font, style.Font);
        ObjectHelper.MapProperties(styleSet.Normal.ParagraphFormat, style.ParagraphFormat);
        ObjectHelper.MapProperties(styleSet.Normal.ParagraphFormat.Borders.Left, style.ParagraphFormat.Borders.Left);
        ObjectHelper.MapProperties(styleSet.Normal.ParagraphFormat.Borders.Right, style.ParagraphFormat.Borders.Right);
        ObjectHelper.MapProperties(styleSet.Normal.ParagraphFormat.Borders.Top, style.ParagraphFormat.Borders.Top);
        ObjectHelper.MapProperties(styleSet.Normal.ParagraphFormat.Borders.Bottom, style.ParagraphFormat.Borders.Bottom);

        //AddStyle(styleSet.Normal);
        AddStyle(styleSet.ParagraphCenter);
        AddStyle(styleSet.ParagraphRight);
        AddStyle(styleSet.ParagraphJustify);
        AddStyle(styleSet.Footer);

        AddStyle(styleSet.NormalTable);
        AddStyle(styleSet.Bullet1);
        AddStyle(styleSet.ChartCell);
        AddStyle(styleSet.ChartTitle);
        AddStyle(styleSet.ChartYLabel);
        AddStyle(styleSet.Code);
        AddStyle(styleSet.Details);
        AddStyle(styleSet.DefinitionListItem);
        AddStyle(styleSet.DefinitionListTerm);

        AddStyle(styleSet.Header);
        AddStyle(styleSet.Heading1);
        AddStyle(styleSet.Heading2);
        AddStyle(styleSet.Heading3);
        AddStyle(styleSet.Heading4);
        AddStyle(styleSet.Heading5);
        AddStyle(styleSet.NoHeading1);
        AddStyle(styleSet.Table);
        AddStyle(styleSet.TableLegend);
        AddStyle(styleSet.Image);
        AddStyle(styleSet.Figure);
        AddStyle(styleSet.Equation);
        AddStyle(styleSet.Title);
        AddStyle(styleSet.Subtitle);
        AddStyle(styleSet.SectionTitle);
        AddStyle(styleSet.SectionSubtitle);
        AddStyle(styleSet.TocHeading);
        AddStyle(styleSet.Toc1);
        AddStyle(styleSet.Toc2);
        AddStyle(styleSet.Toc3);
        AddStyle(styleSet.Toc4);
        AddStyle(styleSet.Toc5);
        AddStyle(styleSet.ToeHeading);
        AddStyle(styleSet.Toe);
        AddStyle(styleSet.TofHeading);
        AddStyle(styleSet.Tof);
        AddStyle(styleSet.TotHeading);
        AddStyle(styleSet.Tot);
        AddStyle(styleSet.Citation);
        AddStyle(styleSet.CitationSource);
        AddStyle(styleSet.Info);
        AddStyle(styleSet.Warning);
        AddStyle(styleSet.Error);
    }

    /// <summary>
    /// Get a style form the document
    /// </summary>
    /// <param name="styleName">Name of the style</param>
    /// <returns>Style</returns>
    public Style GetStyle(string styleName)
    {
        return Document.Styles[styleName];
    }

    /// <summary>
    /// Section information
    /// </summary>
    protected class SectionInfo
    {
        /// <summary>
        /// Current section
        /// </summary>
        public Section Section { get; set; }

        /// <summary>
        /// Is a restart of the page numbering required? Default: false
        /// </summary>
        public bool IsRestartPageNumberingRequired { get; set; }

        /// <summary>
        /// Page number format
        /// </summary>
        public PageNumberFormatEnum PageNumberFormat { get; set; } = PageNumberFormatEnum.Decimal;
    }
}
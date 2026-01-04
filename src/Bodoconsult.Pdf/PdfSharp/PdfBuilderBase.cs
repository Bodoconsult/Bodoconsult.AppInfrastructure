// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Helpers;
using Bodoconsult.Pdf.Interfaces;
using Bodoconsult.Pdf.Stylesets;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
    protected List<SectionInfo> SectionInfos = new();

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
    /// Current header text
    /// </summary>
    protected string HeaderText;

    /// <summary>
    /// Current header style name
    /// </summary>
    protected string HeaderStyleName;

    /// <summary>
    /// Current header logo path
    /// </summary>
    protected string HeaderLogoPath;

    /// <summary>
    /// Current footer text
    /// </summary>
    protected string FooterText;

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
    /// Currently started table
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
    /// Alternating background color for tables
    /// </summary>
    public Color AlternateBackColor { get; set; }

    /// <summary>
    /// Background color
    /// </summary>
    public Color BackColor { get; set; }

    /// <summary>
    /// Table border color
    /// </summary>
    public Color TableBorderColor { get; set; }

    // Farben für Stylesheets wie "wr_cell_h1"

    /// <summary>
    /// Color for shading of risk class 1
    /// </summary>
    public Color ShadingRisk2Color { get; set; }

    /// <summary>
    /// Color for shading of risk class 2
    /// </summary>
    public Color ShadingRisk1Color { get; set; }

    /// <summary>
    /// Color for shading of headline 3
    /// </summary>
    public Color ShadingH3Color { get; set; }

    /// <summary>
    /// Color for shading of headline 2
    /// </summary>
    public Color ShadingH2Color { get; set; }

    /// <summary>
    /// Color for shading of headline 1
    /// </summary>
    public Color ShadingH1Color { get; set; }

    /// <summary>
    /// Add a page break if necessary
    /// </summary>
    public bool AddPageBreakIfNecessary { get; set; }

    /// <summary>
    /// Path to the background image or null if no background image should be used
    /// </summary>
    public string BackgroundImagePath { get; set; }

    /// <summary>
    /// Get the current width of the page.
    /// </summary>
    public double Width
    {
        get
        {
            double w;

            var ps = StyleSet.PageSetup;

            //if (Content != null)
            //{
            //    ps = Content.PageSetup;
            //}

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

        // Save the document...
        renderer.PdfDocument.Save(fileName);

        if (!showPdfFile)
        {
            return;
        }

        OpenFile(fileName);
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
        var format = PageFormat.A4;

        switch (style.TypoPaperFormat.PaperFormatName.ToLowerInvariant())
        {
            case "a0":
                format = PageFormat.A0;
                break;
            case "a1":
                format = PageFormat.A1;
                break;
            case "a2":
                format = PageFormat.A2;
                break;
            case "a3":
                format = PageFormat.A3;
                break;
            case "a4":
                format = PageFormat.A4;
                break;
            case "a5":
                format = PageFormat.A5;
                break;
            case "b5":
                format = PageFormat.B5;
                break;
            case "ledger":
                format = PageFormat.Ledger;
                break;
            case "letter":
                format = PageFormat.Letter;
                break;
            case "legal":
                format = PageFormat.Legal;
                break;
            case "p11x17":
                format = PageFormat.P11x17;
                break;
                //case "a0":
                //    format = PageFormat.A0;
                //    break;
                //case "a0":
                //    format = PageFormat.A0;
                //    break;
                //default:
                //break;
        }

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
        if (style == null)
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
        //if (_content == null)
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

        AddHeaderInternal(Content);
        AddFooterInternal(Content);

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

        AddHeaderInternal(Toc);
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

        AddHeaderInternal(Tof);
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

        AddHeaderInternal(Toe);
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

        AddHeaderInternal(Tot);
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

        paragraph.AddText(text ?? string.Empty);

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
    /// Use &#60;&#60;page&#62;&#62; and &#60;&#60;pages&#62;&#62; fur current page number and number of pages in document
    /// </summary>
    /// <param name="text"></param>
    /// <param name="styleName"></param>
    public void SetFooter(string text, string styleName = "Footer")
    {
        FooterText = text;
        FooterStyleName = styleName;
    }

    /// <summary>
    /// Add a footer
    /// </summary>
    /// <param name="section">Section to add the footer to</param>
    /// <param name="pageNumberFormat">Null or ROMAN, roman, ALPHABETIC, alphabetic</param>
    protected virtual void AddFooterInternal(Section section, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        if (section == null || string.IsNullOrEmpty(FooterText))
        {
            return;
        }

        var paragraph = new Paragraph
        {
            Style = "Footer"
        };

        var text = FooterText;

        if (text.Contains(ITypography.PageFieldIndicator))
        {
            paragraph.Format.AddTabStop(Unit.FromCentimeter(Width), TabAlignment.Right);

            var vorher = text[..text.IndexOf(ITypography.PageFieldIndicator, StringComparison.Ordinal)];
            var nachher = text.Substring(text.IndexOf(ITypography.PageFieldIndicator, StringComparison.Ordinal) + 8,
                text.Length - text.IndexOf(ITypography.PageFieldIndicator, StringComparison.Ordinal) - 8);
            paragraph.AddText(vorher);

            var p = paragraph.AddPageField();

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

            if (nachher.Contains(ITypography.PageFieldIndicator))
            {
                vorher = nachher[..nachher.IndexOf(ITypography.PageFieldIndicator, StringComparison.Ordinal)];
                nachher = nachher.Substring(nachher.IndexOf(ITypography.PageFieldIndicator, StringComparison.Ordinal) + 9,
                    nachher.Length - nachher.IndexOf(ITypography.PageFieldIndicator, StringComparison.Ordinal) - 9);

                paragraph.AddText(vorher);
                paragraph.AddText($" {PageNumberPrefix} ");
                paragraph.AddNumPagesField();
            }
            paragraph.AddText(nachher);
        }
        else
        {
            paragraph.AddText(FooterText);
        }

        paragraph.Style = FooterStyleName;
        section.Footers.Primary.Add(paragraph);
    }

    /// <summary>
    /// Set a header for the document
    /// </summary>
    /// <param name="text">Header text</param>
    /// <param name="styleName">Name of the style to use for the header</param>
    public void SetHeader(string text, string styleName = "Header")
    {
        SetHeader(text, styleName, null);
    }

    /// <summary>
    /// Set a header for the document
    /// </summary>
    /// <param name="text">Header text</param>
    /// <param name="styleName">Name of the style to use for the header</param>
    /// <param name="logoPath">Path to a logo image or null</param>
    public void SetHeader(string text, string styleName, string logoPath)
    {
        HeaderText = text;
        HeaderStyleName = styleName;
        HeaderLogoPath = logoPath;
    }

    /// <summary>
    /// Add a header
    /// </summary>
    /// <param name="section">Section to add the header to</param>
    protected virtual void AddHeaderInternal(Section section)
    {
        if (section == null || (string.IsNullOrEmpty(HeaderText) && string.IsNullOrEmpty(BackgroundImagePath)))
        {
            return;
        }

        if (!string.IsNullOrEmpty(BackgroundImagePath) && File.Exists(BackgroundImagePath))
        {
            var image = section.Headers.Primary.AddImage(BackgroundImagePath);
            image.Height = StyleSet.PageSetup.PageHeight;
            image.Width = StyleSet.PageSetup.PageWidth;
            image.RelativeVertical = RelativeVertical.Page;
            image.RelativeHorizontal = RelativeHorizontal.Page;
            image.WrapFormat.Style = WrapStyle.Through;
        }

        var paragraph = new Paragraph
        {
            Format =
            {
                Alignment = ParagraphAlignment.Left
            }
        };

        paragraph.Format.TabStops.ClearAll();
        paragraph.Style = "Header";

        var width = StyleSet.PageSetup.Orientation == Orientation.Landscape ? Unit.FromCentimeter(StyleSet.PageSetup.PageHeight.Centimeter -
                StyleSet.PageSetup.LeftMargin.Centimeter -
                StyleSet.PageSetup.RightMargin.Centimeter) :
            Unit.FromCentimeter(StyleSet.PageSetup.PageWidth.Centimeter -
                                StyleSet.PageSetup.LeftMargin.Centimeter -
                                StyleSet.PageSetup.RightMargin.Centimeter);

        paragraph.Format.AddTabStop(width, TabAlignment.Right);


        if (!string.IsNullOrEmpty(HeaderLogoPath))
        {
            var image = paragraph.AddImage(HeaderLogoPath);
            image.Height = Unit.FromCentimeter(0.5);
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Left = ShapePosition.Left;
            image.Top = ShapePosition.Center;
            image.LockAspectRatio = true;
            image.WrapFormat.Style = WrapStyle.Through;
        }

        paragraph.AddText($"\t{HeaderText}");

        paragraph.Style = HeaderStyleName;
        section.Headers.Primary.Add(paragraph);
    }

    /// <summary>
    /// Add a table to the document
    /// </summary>
    /// <param name="dt">Current table data</param>
    /// <param name="legend">Legend for the table</param>
    /// <param name="tag">Link tag name</param>
    /// <param name="width">Width in cm to use or 0 for textarea width</param>
    /// <param name="tableStyle">Name of the style to use for table. Default: NormalTable</param>
    public void AddTable(PdfTable dt, string legend, string tag, double width = 0, string tableStyle = "NormalTable")
    {
        if (Math.Abs(width) < 0.000001)
        {
            width = Width;
        }

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

        var style = Document.Styles[tableStyle];

        // Create table now
        var table = Content.AddTable();
        table.LeftPadding = 2;
        table.Borders.Width = 0.5;
        table.Borders.Color = TableBorderColor;
        table.Rows.Alignment = RowAlignment.Center;
        table.Style = tableStyle;

        var colCount = dt.Columns.Count;

        for (var i = 1; i <= colCount; i++)
        {
            var col = dt.Columns[i - 1];

            if (col.MaxLength > 25)
            {
                col.MaxLength = 25;
            }

            var column = table.AddColumn();
            column.Borders.Color = TableBorderColor;
            column.Width = Unit.FromCentimeter(col.MaxLength * 0.16);
            switch (col.TextAlignment)
            {
                case PdfTextAlignment.Left:
                    column.Format.Alignment = ParagraphAlignment.Left;
                    break;
                case PdfTextAlignment.Center:
                    column.Format.Alignment = ParagraphAlignment.Center;
                    break;
                case PdfTextAlignment.Right:
                    column.Format.Alignment = ParagraphAlignment.Right;
                    break;
                default:
                    column.Format.Alignment = ParagraphAlignment.Left;
                    break;
            }
        }

        // Kopfzeile schreiben
        var header = table.AddRow();
        header.Shading.Color = BackColor;
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
            row.Shading.Color = shadow ? BackColor : AlternateBackColor;

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

        if (string.IsNullOrEmpty(legend))
        {
            return;
        }

        CreateTableLegend(legend, tag);
    }

    /// <summary>
    /// Create a table legend
    /// </summary>
    /// <param name="legend">Legend text</param>
    /// <param name="tag">Bookmark tag</param>
    protected virtual void CreateTableLegend(string legend, string tag)
    {
        var legendP = Content.AddParagraph(legend, "TableLegend");
        if (string.IsNullOrEmpty(tag))
        {
            return;
        }
        legendP.AddBookmark(tag);
    }

    /// <summary>
    /// Add a table to the content
    /// </summary>
    /// <param name="dt">Data to show in the table</param>
    /// <param name="heading">Heading for the table</param>
    /// <param name="headingStyleName">Style name for the heading</param>
    /// <param name="additionalInfos"></param>
    /// <param name="additionalInfosStyleName"></param>
    /// <param name="width"></param>
    /// <param name="tableStyle">Name of the style to use for table formatting (not all properties supported)</param>
    public void AddTable(DataTable dt, string heading, string headingStyleName, string additionalInfos, string additionalInfosStyleName, double width = 0, string tableStyle = "NormalTable")
    {

        if (Math.Abs(width) < 0.000001)
        {
            width = Width;
        }

        if (!string.IsNullOrEmpty(heading))
        {
            AddParagraph(heading, headingStyleName);
        }

        if (!string.IsNullOrEmpty(additionalInfos))
        {
            AddParagraph(additionalInfos, additionalInfosStyleName);
        }

        var style = Document.Styles[tableStyle];

        //frame.FillFormat.Color = Colors.White;
        var table = Content.AddTable();
        table.LeftPadding = 2;
        table.Borders.Width = 0.5;
        table.Borders.Color = TableBorderColor;

        var colCount = dt.Columns.Count;


        var startCol = 1;
        var format = new string[dt.Columns.Count];

        var usedWidth = 0D;
        var colCountNotUsed = 0;

        var fontSize = Unit.FromCentimeter(style.Font.Size.Centimeter / 40.0);

        // Ermittle Breite der Nicht-Text-Spalten und Anzahl der Text-Spalten
        for (var i = 1; i <= colCount; i++)
        {
            var col = dt.Columns[i - 1];

            if (col.ColumnName.ToLower() == "cssstyle") continue;

            var t = col.DataType.ToString().Replace("System.", string.Empty).ToLower();

            switch (t)
            {
                case "datetime":
                    usedWidth += WidthDateTime * fontSize.Point;
                    break;
                case "decimal":
                case "double":
                case "single":
                case "float":
                    usedWidth += WidthDouble * fontSize.Point;
                    break;
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    usedWidth += WidthInteger * fontSize.Point;
                    break;
                default:
                    colCountNotUsed++;
                    break;
            }
        }

        // Errechne dann die zur Verfügung stehende maximale Breite der Text-Spalten
        var widthText = colCountNotUsed > 0 ? Math.Round((Width - usedWidth) / colCountNotUsed, 1) - 0.1 : 2.0;

        if (widthText > 7.0) widthText = 7.0;

        for (var i = 1; i <= colCount; i++)
        {
            var col = dt.Columns[i - 1];

            if (col.ColumnName.ToLower() == "cssstyle")
            {
                startCol = 2;
                continue;
            }

            double colWidth;

            var column = table.AddColumn();
            column.Borders.Color = TableBorderColor;

            var t = col.DataType.ToString().Replace("System.", string.Empty).ToLower();
            switch (t)
            {
                case "datetime":
                    colWidth = WidthDateTime * fontSize.Point;
                    format[i - 1] = @"dd.MM.yyyy";
                    break;
                case "decimal":
                case "double":
                case "single":
                    colWidth = WidthDouble * fontSize.Point;
                    column.Format.Alignment = ParagraphAlignment.Right;
                    format[i - 1] = @"#,##0.00";
                    break;
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    colWidth = WidthInteger * fontSize.Point;
                    column.Format.Alignment = ParagraphAlignment.Right;
                    format[i - 1] = @"#,##0";
                    break;
                default:
                    colWidth = widthText;
                    column.Format.Alignment = ParagraphAlignment.Left;
                    break;
            }

            column.Width = Unit.FromCentimeter(colWidth);
        }



        var korr = startCol == 2 ? 1 : 0;

        // Kopfzeile schreiben
        var header = table.AddRow();
        header.Shading.Color = BackColor;
        header.Format.Font.Color = Colors.Black;
        header.Format.Font.Size = style.Font.Size;
        header.Format.Font.Name = style.Font.Name;

        for (var i = 1; i <= table.Columns.Count; i++)
        {
            var cell = header.Cells[i - 1];
            var p = cell.AddParagraph(dt.Columns[i - 1 + korr].ColumnName);
            p.Format.Font.Size = style.Font.Size;
            p.Format.Font.Name = style.Font.Name;
            p.Format.Font.Bold = true;
        }

        // Inhaltszeilen schreiben
        var shadow = false;

        foreach (DataRow r in dt.Rows)

        //for (var zeile = schleife * Increment; zeile < (schleife + 1) * Increment; zeile++)
        {
            var row = table.AddRow();
            //row.KeepWith = 2;
            var css = string.Empty;
            if (startCol == 2) css = r[0].ToString();

            Color shadingColor;

            if (string.IsNullOrEmpty(css))
            {
                shadingColor = shadow ? BackColor : AlternateBackColor;
            }
            else
            {
                switch (css.ToLower())
                {
                    case "wr_cell_h1":
                        shadingColor = ShadingH1Color;
                        break;
                    case "wr_cell_h2":
                        shadingColor = ShadingH2Color;
                        break;
                    case "wr_cell_h3":
                        shadingColor = ShadingH3Color;
                        break;
                    case "wr_cell_risk1":
                        shadingColor = ShadingRisk1Color;
                        break;
                    case "wr_cell_risk2":
                        shadingColor = ShadingRisk2Color;
                        break;
                    default:
                        shadingColor = shadow ? BackColor : AlternateBackColor;
                        break;
                }
            }

            row.Shading.Color = shadingColor;

            for (var i = 0; i < table.Columns.Count; i++)
            {
                var cell = row.Cells[i];

                if (string.IsNullOrEmpty(format[i + korr]))
                {
                    var p = cell.AddParagraph(r[i + korr].ToString());
                    p.Format.Font.Size = style.Font.Size;
                    p.Format.Font.Name = style.Font.Name;
                    //p.Format.Shading.Color = shadingColor;
                }
                else
                {
                    if (format[i + korr].ToLower().Contains("yy"))
                    {
                        var z = r[i + korr].ToString();
                        if (!string.IsNullOrEmpty(z))
                        {
                            var p = cell.AddParagraph(Convert.ToDateTime(z).ToString(format[i + korr]));
                            p.Format.Font.Size = style.Font.Size;
                            p.Format.Font.Name = style.Font.Name;
                            //p.Format.Shading.Color = shadingColor;
                        }
                    }
                    else
                    {
                        var z = r[i + korr].ToString();
                        if (!string.IsNullOrEmpty(z))
                        {
                            var p = cell.AddParagraph(Convert.ToDouble(z).ToString(format[i + korr]));
                            p.Format.Font.Size = style.Font.Size;
                            p.Format.Font.Name = style.Font.Name;
                            // p.Format.Shading.Color = shadingColor;

                        }
                    }
                }
            }

            shadow = !shadow;
        }

        //var widthTable = table.Columns.Cast<Column>().Aggregate(0D, (current, t) => current + t.Width.Centimeter);
        //frame.Width = Unit.FromCentimeter(widthTable);

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
                ;
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
            var p1 = cell1.AddParagraph(r[0].ToString());

            if (!string.IsNullOrEmpty(style1))
            {
                p1.Style = style1;
            }

            var cell2 = row.Cells[1];
            cell2.Borders.Width = borderWidth;

            var p2 = cell2.AddParagraph(r[1].ToString());
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

        var style = Document.Styles[tableStyle];



        //frame.FillFormat.Color = Colors.White;
        var table = frame.AddTable();

        table.Borders.Width = borderWidth;
        table.BottomPadding = 0;
        table.TopPadding = 0;

        if (borderWidth > 0)
        {
            table.Borders.Color = TableBorderColor;
        }

        var colCount = dt.Columns.Count;


        var startCol = 1;
        var format = new string[dt.Columns.Count];

        var usedWidth = 0D;
        var colCountNotUsed = 0;

        var fontSize = Unit.FromCentimeter(style.Font.Size.Centimeter / 40.0);

        // Ermittle Breite der Nicht-Text-Spalten und Anzahl der Text-Spalten
        for (var i = 1; i <= colCount; i++)
        {
            var col = dt.Columns[i - 1];

            if (col.ColumnName.ToLower() == "cssstyle")
            {
                continue;
            }

            var t = col.DataType.ToString().Replace("System.", string.Empty).ToLower();

            switch (t)
            {
                case "datetime":
                    usedWidth += WidthDateTime * fontSize.Point;
                    break;
                case "decimal":
                case "double":
                case "single":
                case "float":
                    usedWidth += WidthDouble * fontSize.Point;
                    break;
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    usedWidth += WidthInteger * fontSize.Point;
                    break;
                default:
                    colCountNotUsed++;
                    break;
            }
        }

        // Errechne dann die zur Verfügung stehende maximale Breite der Text-Spalten
        var widthText = colCountNotUsed > 0 ? Math.Round((frame.Width.Centimeter - usedWidth) / colCountNotUsed, 1) - 0.1 : 2.0;

        if (widthText > 7.0)
        {
            widthText = 7.0;
        }

        for (var i = 1; i <= colCount; i++)
        {
            var col = dt.Columns[i - 1];

            if (col.ColumnName.ToLower() == "cssstyle")
            {
                startCol = 2;
                continue;
            }

            double width;

            var column = table.AddColumn();
            column.Borders.Color = TableBorderColor;

            var t = col.DataType.ToString().Replace("System.", string.Empty).ToLower();
            switch (t)
            {
                case "datetime":
                    width = WidthDateTime * fontSize.Point;
                    format[i - 1] = @"dd.MM.yyyy";
                    break;
                case "decimal":
                case "double":
                case "single":
                    width = WidthDouble * fontSize.Point;
                    column.Format.Alignment = ParagraphAlignment.Right;
                    format[i - 1] = @"#,##0.00";
                    break;
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    width = WidthInteger * fontSize.Point;
                    column.Format.Alignment = ParagraphAlignment.Right;
                    format[i - 1] = @"#,##0";
                    break;
                default:
                    width = widthText;
                    column.Format.Alignment = ParagraphAlignment.Left;
                    break;
            }

            column.Width = Unit.FromCentimeter(width);
        }



        var korr = startCol == 2 ? 1 : 0;

        // Kopfzeile schreiben
        var header = table.AddRow();
        header.Shading.Color = BackColor;
        header.Format.Font.Color = Colors.Black;
        header.Format.Font.Size = style.Font.Size - 0.5;
        header.Format.Font.Name = style.Font.Name;
        header.Format.Font.Bold = true;
        header.Format.SpaceAfter = style.ParagraphFormat.SpaceAfter;
        header.Format.SpaceBefore = style.ParagraphFormat.SpaceBefore;

        for (var i = 1; i <= table.Columns.Count; i++)
        {
            var cell = header.Cells[i - 1];
            cell.AddParagraph(dt.Columns[i - 1 + korr].ColumnName);

        }

        // Inhaltszeilen schreiben
        var shadow = false;
        for (var zeile = schleife * Increment; zeile < (schleife + 1) * Increment; zeile++)
        {
            if (zeile >= dt.Rows.Count)
            {
                break;
            }

            var r = dt.Rows[zeile];
            var row = table.AddRow();

            row.BottomPadding = 0;
            row.TopPadding = 0;

            var css = string.Empty;
            if (startCol == 2)
            {
                css = r[0].ToString();
                css = r[0].ToString();
            }

            Color shadingColor;

            if (string.IsNullOrEmpty(css))
            {
                shadingColor = shadow ? BackColor : AlternateBackColor;
            }
            else
            {
                switch (css.ToLower())
                {
                    case "wr_cell_h1":
                        shadingColor = ShadingH1Color;
                        break;
                    case "wr_cell_h2":
                        shadingColor = ShadingH2Color;
                        break;
                    case "wr_cell_h3":
                        shadingColor = ShadingH3Color;
                        break;
                    case "wr_cell_risk1":
                        shadingColor = ShadingRisk1Color;
                        break;
                    case "wr_cell_risk2":
                        shadingColor = ShadingRisk2Color;
                        break;
                    default:
                        shadingColor = shadow ? BackColor : AlternateBackColor;
                        break;
                }
            }

            row.Shading.Color = shadingColor;

            for (var i = 0; i < table.Columns.Count; i++)
            {
                var cell = row.Cells[i];
                cell.Format.SpaceAfter = 0;
                cell.Format.SpaceBefore = 0;

                var s = format[i + korr];

                if (string.IsNullOrEmpty(s))
                {
                    var p = cell.AddParagraph((r[i + korr].ToString() ?? string.Empty).Trim());
                    p.Format.Font.Size = style.Font.Size;
                    p.Format.Font.Name = style.Font.Name;
                    p.Format.SpaceAfter = style.ParagraphFormat.SpaceAfter;
                    p.Format.SpaceBefore = style.ParagraphFormat.SpaceBefore;
                    p.Format.Shading.Color = shadingColor;
                }
                else
                {
                    if (s.ToLower().Contains("yy"))
                    {
                        var z = (r[i + korr].ToString() ?? string.Empty).Trim();
                        if (string.IsNullOrEmpty(z))
                        {
                            continue;
                        }
                        var p = cell.AddParagraph(Convert.ToDateTime(z).ToString(format[i + korr]));
                        p.Format.Font.Size = style.Font.Size;
                        p.Format.Font.Name = style.Font.Name;
                        p.Format.SpaceAfter = style.ParagraphFormat.SpaceAfter;
                        p.Format.SpaceBefore = style.ParagraphFormat.SpaceBefore;
                        p.Format.Shading.Color = shadingColor;
                    }
                    else
                    {
                        var z = r[i + korr].ToString();
                        if (string.IsNullOrEmpty(z))
                        {
                            continue;
                        }
                        var p = cell.AddParagraph(Convert.ToDouble(z).ToString(format[i + korr]));
                        p.Format.Font.Size = style.Font.Size;
                        p.Format.Font.Name = style.Font.Name;
                        p.Format.SpaceAfter = style.ParagraphFormat.SpaceAfter;
                        p.Format.SpaceBefore = style.ParagraphFormat.SpaceBefore;
                        p.Format.Shading.Color = shadingColor;
                    }
                }
            }

            shadow = !shadow;
        }

        var widthTable = table.Columns.Cast<Column>().Aggregate(0D, (current, t) => current + t.Width.Centimeter);
        frame.Width = Unit.FromCentimeter(widthTable);
    }





    /// <summary>
    /// Seitenumbruch in Text einfügen
    /// </summary>
    public void NewPage()
    {
        Content.AddPageBreak();
    }


    /// <summary>
    /// Defines the styles used in the document.
    /// </summary>
    public void DefineStyles()
    {
        //// Get the predefined style Normal.
        //Style style = document.Styles["Normal"];
        //// Because all styles are derived from Normal, the next line changes the 
        //// font of the whole document. Or, more exactly, it changes the font of
        //// all styles and paragraphs that do not redefine the font.
        //style.Font.Name = "Times New Roman";

        //// Heading1 to Heading9 are predefined styles with an outline level. An outline level
        //// other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
        //// in PDF.

        //style = document.Styles["Heading1"];
        //style.Font.Name = "Tahoma";
        //style.Font.Size = 14;
        //style.Font.Bold = true;
        //style.Font.Color = Colors.DarkBlue;
        //style.ParagraphFormat.PageBreakBefore = true;
        //style.ParagraphFormat.SpaceAfter = 6;

        //style = document.Styles["Heading2"];
        //style.Font.Size = 12;
        //style.Font.Bold = true;
        //style.ParagraphFormat.PageBreakBefore = false;
        //style.ParagraphFormat.SpaceBefore = 6;
        //style.ParagraphFormat.SpaceAfter = 6;

        //style = document.Styles["Heading3"];
        //style.Font.Size = 10;
        //style.Font.Bold = true;
        //style.Font.Italic = true;
        //style.ParagraphFormat.SpaceBefore = 6;
        //style.ParagraphFormat.SpaceAfter = 3;

        //style = document.Styles[StyleNames.Header];
        //style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

        //style = document.Styles[StyleNames.Footer];
        //style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

        //// Create a new style called TextBox based on style Normal
        //style = document.Styles.AddStyle("TextBox", "Normal");
        //style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
        //style.ParagraphFormat.Borders.Width = 2.5;
        //style.ParagraphFormat.Borders.Distance = "3pt";
        //style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

        //// Create a new style called TOC based on style Normal
        //style = document.Styles.AddStyle("TOC", "Normal");
        //style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
        //style.ParagraphFormat.Font.Color = Colors.Blue;
    }





    ///// <summary>
    ///// Defines the cover page.
    ///// </summary>
    //public void DefineCover()
    //{
    //    //Section section = document.AddSection();

    //    //Paragraph paragraph = section.AddParagraph();
    //    //paragraph.Format.SpaceAfter = "3cm";

    //    _content.AddImage()

    //    Image image = _content.AddImage("../../images/Logo landscape.png");
    //    image.Width = "10cm";

    //    //paragraph = section.AddParagraph("A sample document that demonstrates the\ncapabilities of MigraDoc");
    //    //paragraph.Format.Font.Size = 16;
    //    //paragraph.Format.Font.Color = Colors.DarkRed;
    //    //paragraph.Format.SpaceBefore = "8cm";
    //    //paragraph.Format.SpaceAfter = "3cm";

    //    //paragraph = section.AddParagraph("Rendering date: ");
    //    //paragraph.AddDateField();
    //}

    /// <summary>
    /// Defines page setup, headers, and footers.
    /// </summary>
    public void DefineContentSection()
    {
        var section = Document.AddSection();
        section.PageSetup = StyleSet.PageSetup.Clone();
        section.PageSetup.OddAndEvenPagesHeaderFooter = false;
        section.PageSetup.StartingNumber = 1;

        var paragraph = new Paragraph();
        paragraph.AddText("Hallo Welt");

        section.Add(paragraph);

        //var header = section.Headers.Primary;
        //header.AddParagraph("\tOdd Page Header");

        //header = section.Headers.EvenPage;
        //header.AddParagraph("Even Page Header");

        //// Create a paragraph with centered page number. See definition of style "Footer".
        //var paragraph = new Paragraph();
        //paragraph.AddTab();
        //paragraph.AddPageField();

        //// Add paragraph to footer for odd pages.
        //section.Footers.Primary.Add(paragraph);
        //// Add clone of paragraph to footer for odd pages. Cloning is necessary because an object must
        //// not belong to more than one other object. If you forget cloning an exception is thrown.
        //section.Footers.EvenPage.Add(paragraph.Clone());
    }

    ///// <summary>
    ///// Defines the cover page.
    ///// </summary>
    //public static void DefineTableOfContents(Document document)
    //{
    //    var section = document.LastSection;

    //    section.AddPageBreak();
    //    var paragraph = section.AddParagraph(TitleTableOfContent);
    //    paragraph.Format.Font.Size = 14;
    //    paragraph.Format.Font.Bold = true;
    //    paragraph.Format.SpaceAfter = 24;
    //    paragraph.Format.OutlineLevel = OutlineLevel.Level1;

    //    paragraph = section.AddParagraph();
    //    paragraph.Style = "TOC";
    //    var hyperlink = paragraph.AddHyperlink("Paragraphs");
    //    hyperlink.AddText("Paragraphs\t");
    //    hyperlink.AddPageRefField("Paragraphs");

    //    paragraph = section.AddParagraph();
    //    paragraph.Style = "TOC";
    //    hyperlink = paragraph.AddHyperlink("Tables");
    //    hyperlink.AddText("Tables\t");
    //    hyperlink.AddPageRefField("Tables");

    //    paragraph = section.AddParagraph();
    //    paragraph.Style = "TOC";
    //    hyperlink = paragraph.AddHyperlink("Charts");
    //    hyperlink.AddText("Charts\t");
    //    hyperlink.AddPageRefField("Charts");

    //}


    //public void DefineParagraphs()
    //{
    ////{
    ////    Paragraph paragraph = document.LastSection.AddParagraph("Paragraph Layout Overview", "Heading1");
    ////    paragraph.AddBookmark("Paragraphs");

    ////    DemonstrateAlignment(document);
    ////    DemonstrateIndent(document);
    ////    DemonstrateFormattedText(document);
    ////    DemonstrateBordersAndShading(document);
    //}

    //static void DemonstrateAlignment(Document document)
    //{
    //    //document.LastSection.AddParagraph("Alignment", "Heading2");

    //    //document.LastSection.AddParagraph("Left Aligned", "Heading3");

    //    //Paragraph paragraph = document.LastSection.AddParagraph();
    //    //paragraph.Format.Alignment = ParagraphAlignment.Left;
    //    //paragraph.AddText(FillerText.Text);

    //    //document.LastSection.AddParagraph("Right Aligned", "Heading3");

    //    //paragraph = document.LastSection.AddParagraph();
    //    //paragraph.Format.Alignment = ParagraphAlignment.Right;
    //    //paragraph.AddText(FillerText.Text);

    //    //document.LastSection.AddParagraph("Centered", "Heading3");

    //    //paragraph = document.LastSection.AddParagraph();
    //    //paragraph.Format.Alignment = ParagraphAlignment.Center;
    //    //paragraph.AddText(FillerText.Text);

    //    //document.LastSection.AddParagraph("Justified", "Heading3");

    //    //paragraph = document.LastSection.AddParagraph();
    //    //paragraph.Format.Alignment = ParagraphAlignment.Justify;
    //    //paragraph.AddText(FillerText.MediumText);
    //}

    //static void DemonstrateIndent(Document document)
    //{
    //    //document.LastSection.AddParagraph("Indent", "Heading2");

    //    //document.LastSection.AddParagraph("Left Indent", "Heading3");

    //    //Paragraph paragraph = document.LastSection.AddParagraph();
    //    //paragraph.Format.LeftIndent = "2cm";
    //    //paragraph.AddText(FillerText.Text);

    //    //document.LastSection.AddParagraph("Right Indent", "Heading3");

    //    //paragraph = document.LastSection.AddParagraph();
    //    //paragraph.Format.RightIndent = "1in";
    //    //paragraph.AddText(FillerText.Text);

    //    //document.LastSection.AddParagraph("First Line Indent", "Heading3");

    //    //paragraph = document.LastSection.AddParagraph();
    //    //paragraph.Format.FirstLineIndent = "12mm";
    //    //paragraph.AddText(FillerText.Text);

    //    //document.LastSection.AddParagraph("First Line Negative Indent", "Heading3");

    //    //paragraph = document.LastSection.AddParagraph();
    //    //paragraph.Format.LeftIndent = "1.5cm";
    //    //paragraph.Format.FirstLineIndent = "-1.5cm";
    //    //paragraph.AddText(FillerText.Text);
    //}

    //static void DemonstrateFormattedText(Document document)
    //{
    //    //document.LastSection.AddParagraph("Formatted Text", "Heading2");

    //    ////document.LastSection.AddParagraph("Left Aligned", "Heading3");

    //    //Paragraph paragraph = document.LastSection.AddParagraph();
    //    //paragraph.AddText("Text can be formatted ");
    //    //paragraph.AddFormattedText("bold", TextFormat.Bold);
    //    //paragraph.AddText(", ");
    //    //paragraph.AddFormattedText("italic", TextFormat.Italic);
    //    //paragraph.AddText(", or ");
    //    //paragraph.AddFormattedText("bold & italic", TextFormat.Bold | TextFormat.Italic);
    //    //paragraph.AddText(".");
    //    //paragraph.AddLineBreak();
    //    //paragraph.AddText("You can set the ");
    //    //FormattedText formattedText = paragraph.AddFormattedText("size ");
    //    //formattedText.Size = 15;
    //    //paragraph.AddText("the ");
    //    //formattedText = paragraph.AddFormattedText("color ");
    //    //formattedText.Color = Colors.Firebrick;
    //    //paragraph.AddText("the ");
    //    //formattedText = paragraph.AddFormattedText("font", new Font("Verdana"));
    //    //paragraph.AddText(".");
    //    //paragraph.AddLineBreak();
    //    //paragraph.AddText("You can set the ");
    //    //formattedText = paragraph.AddFormattedText("subscript");
    //    //formattedText.Subscript = true;
    //    //paragraph.AddText(" or ");
    //    //formattedText = paragraph.AddFormattedText("superscript");
    //    //formattedText.Superscript = true;
    //    //paragraph.AddText(".");
    //}

    //static void DemonstrateBordersAndShading(Document document)
    //{
    //    document.LastSection.AddPageBreak();
    //    document.LastSection.AddParagraph("Borders and Shading", "Heading2");

    //    document.LastSection.AddParagraph("Border around Paragraph", "Heading3");

    //    Paragraph paragraph = document.LastSection.AddParagraph();
    //    paragraph.Format.Borders.Width = 2.5;
    //    paragraph.Format.Borders.Color = Colors.Navy;
    //    paragraph.Format.Borders.Distance = 3;
    //    paragraph.AddText(FillerText.MediumText);

    //    document.LastSection.AddParagraph("Shading", "Heading3");

    //    paragraph = document.LastSection.AddParagraph();
    //    paragraph.Format.Shading.Color = Colors.LightCoral;
    //    paragraph.AddText(FillerText.Text);

    //    document.LastSection.AddParagraph("Borders & Shading", "Heading3");

    //    paragraph = document.LastSection.AddParagraph();
    //    paragraph.Style = "TextBox";
    //    paragraph.AddText(FillerText.MediumText);
    //}


    //public static void DefineTables(Document document)
    //{
    //    Paragraph paragraph = document.LastSection.AddParagraph("Table Overview", "Heading1");
    //    paragraph.AddBookmark("Tables");

    //    DemonstrateSimpleTable(document);
    //    DemonstrateAlignment(document);
    //    DemonstrateCellMerge(document);
    //}

    //public static void DemonstrateSimpleTable(Document document)
    //{
    //    document.LastSection.AddParagraph("Simple Tables", "Heading2");

    //    Table table = new Table();
    //    table.Borders.Width = 0.75;

    //    Column column = table.AddColumn(Unit.FromCentimeter(2));
    //    column.Format.Alignment = ParagraphAlignment.Center;

    //    table.AddColumn(Unit.FromCentimeter(5));

    //    Row row = table.AddRow();
    //    row.Shading.Color = Colors.PaleGoldenrod;
    //    Cell cell = row.Cells[0];
    //    cell.AddParagraph("Itemus");
    //    cell = row.Cells[1];
    //    cell.AddParagraph("Descriptum");

    //    row = table.AddRow();
    //    cell = row.Cells[0];
    //    cell.AddParagraph("1");
    //    cell = row.Cells[1];
    //    cell.AddParagraph(FillerText.ShortText);

    //    row = table.AddRow();
    //    cell = row.Cells[0];
    //    cell.AddParagraph("2");
    //    cell = row.Cells[1];
    //    cell.AddParagraph(FillerText.Text);

    //    table.SetEdge(0, 0, 2, 3, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

    //    document.LastSection.Add(table);
    //}

    //public static void DemonstrateAlignment(Document document)
    //{
    //    document.LastSection.AddParagraph("Cell Alignment", "Heading2");

    //    Table table = document.LastSection.AddTable();
    //    table.Borders.Visible = true;
    //    table.Format.Shading.Color = Colors.LavenderBlush;
    //    table.Shading.Color = Colors.Salmon;
    //    table.TopPadding = 5;
    //    table.BottomPadding = 5;

    //    Column column = table.AddColumn();
    //    column.Format.Alignment = ParagraphAlignment.Left;

    //    column = table.AddColumn();
    //    column.Format.Alignment = ParagraphAlignment.Center;

    //    column = table.AddColumn();
    //    column.Format.Alignment = ParagraphAlignment.Right;

    //    table.Rows.Height = 35;

    //    Row row = table.AddRow();
    //    row.VerticalAlignment = VerticalAlignment.Top;
    //    row.Cells[0].AddParagraph("Text");
    //    row.Cells[1].AddParagraph("Text");
    //    row.Cells[2].AddParagraph("Text");

    //    row = table.AddRow();
    //    row.VerticalAlignment = VerticalAlignment.Center;
    //    row.Cells[0].AddParagraph("Text");
    //    row.Cells[1].AddParagraph("Text");
    //    row.Cells[2].AddParagraph("Text");

    //    row = table.AddRow();
    //    row.VerticalAlignment = VerticalAlignment.Bottom;
    //    row.Cells[0].AddParagraph("Text");
    //    row.Cells[1].AddParagraph("Text");
    //    row.Cells[2].AddParagraph("Text");
    //}

    //public static void DemonstrateCellMerge(Document document)
    //{
    //    document.LastSection.AddParagraph("Cell Merge", "Heading2");

    //    Table table = document.LastSection.AddTable();
    //    table.Borders.Visible = true;
    //    table.TopPadding = 5;
    //    table.BottomPadding = 5;

    //    Column column = table.AddColumn();
    //    column.Format.Alignment = ParagraphAlignment.Left;

    //    column = table.AddColumn();
    //    column.Format.Alignment = ParagraphAlignment.Center;

    //    column = table.AddColumn();
    //    column.Format.Alignment = ParagraphAlignment.Right;

    //    table.Rows.Height = 35;

    //    Row row = table.AddRow();
    //    row.Cells[0].AddParagraph("Merge Right");
    //    row.Cells[0].MergeRight = 1;

    //    row = table.AddRow();
    //    row.VerticalAlignment = VerticalAlignment.Bottom;
    //    row.Cells[0].MergeDown = 1;
    //    row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
    //    row.Cells[0].AddParagraph("Merge Down");

    //    table.AddRow();
    //}

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
        var frame = Content.AddTextFrame();
        frame.Height = Unit.FromCentimeter(height);
        frame.Width = Unit.FromCentimeter(width);
        frame.Left = ShapePosition.Center;
        var image = frame.AddImage(fileName);
        image.Width = frame.Width;
        image.Height = frame.Height;
        // image = null;
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
            switch (field.TextAlign)
            {
                case PdfTextAlignment.Center:
                    columnDef.Format.Alignment = ParagraphAlignment.Center;
                    break;
                case PdfTextAlignment.Right:
                    columnDef.Format.Alignment = ParagraphAlignment.Right;
                    break;
                default:
                    columnDef.Format.Alignment = ParagraphAlignment.Left;
                    break;
            }
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

                if (info == null) continue;

                string value;

                if (string.IsNullOrEmpty(field.Format))
                {
                    value = info.GetValue(item, null).ToString();
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

        if (!html.Contains("<"))
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

        BackColor = Colors.LightSteelBlue;
        TableBorderColor = Colors.DarkGray;
        AlternateBackColor = Colors.White;
        ShadingH1Color = Colors.GreenYellow;
        ShadingH2Color = Colors.YellowGreen;
        ShadingH3Color = Colors.Gold;
        ShadingRisk1Color = Colors.Red;
        ShadingRisk2Color = Colors.Orange;
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

        //ObjectHelper.MapProperties(_ps, _document.DefaultPageSetup);

        var style = Document.Styles["Normal"];

        if (style == null)
        {
            throw new ArgumentException("No style Normal found in styleset");
        }

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
        public bool IsRestartPageNumberingRequired { get; set; } = false;

        /// <summary>
        /// Page number format
        /// </summary>
        public PageNumberFormatEnum PageNumberFormat { get; set; } = PageNumberFormatEnum.Decimal;
    }
}
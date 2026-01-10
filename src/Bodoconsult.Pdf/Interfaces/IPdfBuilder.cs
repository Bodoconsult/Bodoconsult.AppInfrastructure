// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Pdf.Stylesets;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Bodoconsult.Pdf.Interfaces;

/// <summary>
/// Interface for PDF file builder instances
/// </summary>
public interface IPdfBuilder : IDisposable
{
    /// <summary>
    /// Current styleset to use
    /// </summary>
    IStyleSet StyleSet { get; }

    /// <summary>
    /// Current started table
    /// </summary>
    Table Table { get; }

    /// <summary>
    /// The title for the table of content (TOC)
    /// </summary>
    string TitleTableOfContent { get; set; }

    /// <summary>
    /// The title for the table of figures (TOF)
    /// </summary>
    string TitleTableOfFigures { get; set; }

    /// <summary>
    /// The title for the table of equations (TOE)
    /// </summary>
    string TitleTableOfEquations { get; set; }

    /// <summary>
    /// The title for the table of tables (TOT)
    /// </summary>
    string TitleTableOfTables { get; set; }

    /// <summary>
    /// The word written before the page number in a page footer
    /// </summary>
    string PageNumberPrefix { get; set; }

    /// <summary>
    /// Increment
    /// </summary>
    int Increment { get; set; }

    /// <summary>
    /// Add a page break if necessary
    /// </summary>
    bool AddPageBreakIfNecessary { get; set; }

    /// <summary>
    /// Path to the background image or null if no background image should be used
    /// </summary>
    string BackgroundImagePath { get; set; }

    /// <summary>
    /// Get the current width of the page.
    /// </summary>
    double Width { get; }

    /// <summary>
    /// Save Pdf to a file
    /// </summary>
    /// <param name="fileName">Full path for pdf file's destination</param>
    /// <param name="showPdfFile">Show Pdf-File in a viewer</param>
    void RenderToPdf(string fileName, bool showPdfFile);

    /// <summary>
    /// Save Pdf to a stream
    /// </summary>
    /// <param name="stream">Stream</param>
    void RenderToPdf(Stream stream);

    /// <summary>
    /// Set general document information
    /// </summary>
    /// <param name="title">Title of the file</param>
    /// <param name="subject">Subject of the file</param>
    /// <param name="author">Author of the file</param>
    void SetDocInfo(string title, string subject, string author);

    /// <summary>
    /// Add a ney style based on style "Normal"
    /// </summary>
    /// <param name="styleName">Name of the new style</param>
    /// <returns>New style object</returns>
    Style AddStyle(string styleName);

    /// <summary>
    /// Add a style to document
    /// </summary>
    /// <param name="style">Style</param>
    /// <returns>Added style</returns>
    Style AddStyle(Style style);

    /// <summary>
    /// Add a ney style based on another style
    /// </summary>
    /// <param name="styleName">Style name</param>
    /// <param name="baseStyleName">name of the style, the new one is based on</param>
    /// <returns>Added style</returns>
    Style AddStyle(string styleName, string baseStyleName);

    /// <summary>
    /// Add a content section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    void CreateContentSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal);

    /// <summary>
    /// Add a TOC section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    void CreateTocSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal);

    /// <summary>
    /// Add a TOF section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    void CreateTofSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal);

    /// <summary>
    /// Add a TOF section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    void CreateToeSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal);

    /// <summary>
    /// Add a TOT section to the document
    /// </summary>
    /// <param name="isRestartPageNumberingRequired">Is a restart of the page numbering required for this section?</param>
    /// <param name="pageNumberFormat">Page number format</param>
    void CreateTotSection(bool isRestartPageNumberingRequired = false, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal);

    /// <summary>
    /// Add an TOC entry level 1 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    Paragraph AddToc1Entry(string text, string tag);

    /// <summary>
    /// Add an TOC entry level 2 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    Paragraph AddToc2Entry(string text, string tag);

    /// <summary>
    /// Add an TOC entry level 3 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    Paragraph AddToc3Entry(string text, string tag);

    /// <summary>
    /// Add an TOC entry level 4 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    Paragraph AddToc4Entry(string text, string tag);

    /// <summary>
    /// Add an TOC entry level 5 to the TOC
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced heading</param>
    Paragraph AddToc5Entry(string text, string tag);

    /// <summary>
    /// Add an entry to the TOE
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced equation</param>
    Paragraph AddToeEntry(string text, string tag);

    /// <summary>
    /// Add an entry to the TOF
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced figure</param>
    Paragraph AddTofEntry(string text, string tag);

    /// <summary>
    /// Add an entry to the TOT
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    Paragraph AddTotEntry(string text, string tag);

    /// <summary>
    /// Add a heading 1 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    Paragraph AddHeading1(string text, string tag);

    /// <summary>
    /// Add a heading 2 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    Paragraph AddHeading2(string text, string tag);

    /// <summary>
    /// Add a heading 3 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    Paragraph AddHeading3(string text, string tag);

    /// <summary>
    /// Add a heading 4 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    Paragraph AddHeading4(string text, string tag);

    /// <summary>
    /// Add a heading 5 to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="tag">Name of the tag of the referenced table</param>
    Paragraph AddHeading5(string text, string tag);

    /// <summary>
    /// Add a paragraph to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    Paragraph AddParagraph(string text);

    /// <summary>
    /// Add a paragraph to the content section
    /// </summary>
    /// <param name="text">Content to add</param>
    /// <param name="styleName">Name of the style to use</param>
    Paragraph AddParagraph(string text, string styleName);

    /// <summary>
    /// Add a paragraph object to the content
    /// </summary>
    /// <param name="paragraph">Paragraph to add</param>
    void AddParagraph(Paragraph paragraph);

    /// <summary>
    /// Add a WARNING paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddWarning(string text);

    /// <summary>
    /// Add an INFO paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddInfo(string text);

    /// <summary>
    /// Add an ERROR paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddError(string text);

    /// <summary>
    /// Add a CODE paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddCode(string text);

    /// <summary>
    /// Add a CITATION paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <param name="source">Source for the citation</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddCitation(string text, string source);

    /// <summary>
    /// Add a left-aligned paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddParagraphLeft(string text);

    /// <summary>
    /// Add a title
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddTitle(string text);

    /// <summary>
    /// Add a subtitle
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddSubtitle(string text);

    /// <summary>
    /// Add a section title
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddSectionTitle(string text);

    /// <summary>
    /// Add a section subtitle
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddSectionSubtitle(string text);

    /// <summary>
    /// Add a right-aligned paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddParagraphRight(string text);

    /// <summary>
    /// Add a centered paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddParagraphCenter(string text);

    /// <summary>
    /// Add a justified paragraph
    /// </summary>
    /// <param name="text">Initial text to add to the paragraph</param>
    /// <returns>The new paragraph instance</returns>
    Paragraph AddParagraphJustify(string text);

    /// <summary>
    /// Add an empty paragraph to the content
    /// </summary>
    /// <param name="addPageBreak">Add a page break before the empty paragraph</param>
    void AddEmpty(bool addPageBreak = false);

    /// <summary>
    /// Add a figure to the document
    /// </summary>
    /// <param name="imagePath">Full file path to the figure image</param>
    /// <param name="legend">Legend for the figure</param>
    /// <param name="tag">Link tag name</param>
    /// <param name="width">Width in cm</param>
    /// <param name="height">Height in cm</param>
    void AddFigure(string imagePath, string legend, string tag, double width, double height);

    /// <summary>
    /// Add a figure to the document
    /// </summary>
    /// <param name="imagePath">Full file path to the equation image</param>
    /// <param name="legend">Legend for the equation</param>
    /// <param name="tag">Link tag name</param>
    /// <param name="width">Width in cm</param>
    /// <param name="height">Height in cm</param>
    void AddEquation(string imagePath, string legend, string tag, double width, double height);

    /// <summary>
    /// Set a footer text for content and toc. Use style Footer
    /// </summary>

    void SetFooter();

    /// <summary>
    /// Set a header for the document. Use style Header
    /// </summary>
    void SetHeader();

    /// <summary>
    /// Set a header for the document
    /// </summary>
    /// <param name="styleName">Name of the style to use for the header</param>
    void SetHeader(string styleName);

    /// <summary>
    /// Add a table to the document
    /// </summary>
    /// <param name="dt">Current table data</param>
    void AddTable(PdfTable dt);

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
    [Obsolete]
    void AddTable(DataTable dt, string heading, string headingStyleName, string additionalInfos, string additionalInfosStyleName, double width = 0, string tableStyle = "NormalTable");

    /// <summary>
    /// Create a table from a list of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">Data items to show</param>
    /// <param name="listFields">List of fields to show</param>
    /// <param name="showHeader">Show a header. Default false</param>
    void AddTable<T>(IList<T> items, IList<PdfTableField> listFields, bool showHeader);

    /// <summary>
    /// Add a table in a separate frame
    /// </summary>
    /// <param name="dt">Data to show in the table</param>
    /// <param name="heading">Heading for the table</param>
    /// <param name="headingStyleName">Style name for the heading</param>
    /// <param name="additionalInfos"></param>
    /// <param name="additionalInfosStyleName"></param>
    /// <param name="width"></param>
    void AddTableFrame(DataTable dt, string heading, string headingStyleName, string additionalInfos = null, string additionalInfosStyleName = null, double width = 0);

    /// <summary>
    /// Add a definition list with left and right column
    /// </summary>
    /// <param name="dt">List with <see cref="PdfDefinitionListTerm"/> items</param>
    /// <param name="style1">Name of the style to use for left column</param>
    /// <param name="style2">Name of the style to use for right column</param>
    /// <param name="columnWidth1">Column width column 1 in percent</param>
    void AddDefinitionList(List<PdfDefinitionListTerm> dt, string style1 = "DefinitionListTerm", string style2 = "DefinitionListItem", double columnWidth1 = 0.2);

    /// <summary>
    /// Add a definition list with left and right column
    /// </summary>
    /// <param name="dt">DataTable with two columns</param>
    /// <param name="style1">Name of the style to use for left column</param>
    /// <param name="style2">Name of the style to use for right column</param>
    /// <param name="columnWidth1">Column width column 1 in percent</param>
    void AddDefinitionList(DataTable dt, string style1 = "DefinitionListTerm", string style2 = "DefinitionListItem", double columnWidth1 = 0.2);

    /// <summary>
    /// Seitenumbruch in Text einfügen
    /// </summary>
    void NewPage();

    ///// <summary>
    ///// Defines the styles used in the document.
    ///// </summary>
    //void DefineStyles();

    ///// <summary>
    ///// Defines page setup, headers, and footers.
    ///// </summary>
    //void DefineContentSection();

    /// <summary>
    /// Add a chart
    /// </summary>
    /// <param name="chart"></param>
    void AddChart(Chart chart);

    /// <summary>
    /// Start a table with style NormalTable
    /// </summary>
    void TableStart();

    /// <summary>
    /// Start a table with a certain style
    /// </summary>
    /// <param name="style">Style to apply to new table</param>
    void TableStart(string style);

    /// <summary>
    /// Add a column to the currently started table
    /// </summary>
    /// <param name="alignment"></param>
    /// <param name="width"></param>
    void TableAddColumn(ParagraphAlignment alignment, double width);

    /// <summary>
    /// End the currently started table
    /// </summary>
    void TableEnd();

    /// <summary>
    /// Add a row to the currently started table
    /// </summary>
    /// <returns></returns>
    Row TableAddRow();

    /// <summary>
    /// Fill content in a certain table cell defined by row and column number
    /// </summary>
    /// <param name="column">Column number of the cell starting with 0</param>
    /// <param name="row">Row number of the cell starting with 0</param>
    /// <param name="content">Content to fill in the cell</param>
    void TableSetContent(int column, int row, string content);

    /// <summary>
    /// Fill content in a certain table cell defined by row and column number
    /// </summary>
    /// <param name="column">Column number of the cell starting with 0</param>
    /// <param name="row">Row number of the cell starting with 0</param>
    /// <param name="chart">Chart to fill in the cell</param>
    void TableSetContent(int column, int row, Chart chart);

    /// <summary>
    /// Fill image in a certain table cell defined by row and column number
    /// </summary>
    /// <param name="column">Column number of the cell starting with 0</param>
    /// <param name="row">Row number of the cell starting with 0</param>
    /// <param name="imagePath">Image to fill in the cell</param>
    /// <param name="width">Width of the image in cm</param>
    /// <param name="height">Height of the image in cm</param>
    void TableSetContent(int column, int row, string imagePath, double width, double height);

    /// <summary>
    /// Add an image
    /// </summary>
    /// <param name="fileName">Path to the image to add</param>
    /// <param name="width">Width of the image in cm</param>
    /// <param name="height">Height of the image in cm</param>
    void AddImage(string fileName, double width, double height);

    /// <summary>
    /// Fill image in a certain table cell of a small table defined by row and column number
    /// </summary>
    /// <param name="column">Column number of the cell starting with 0</param>
    /// <param name="row">Row number of the cell starting with 0</param>
    /// <param name="data">Data to fill in the cells</param>
    /// <param name="heading">Heading for the table</param>
    /// <param name="width">Width of the image in cm</param>
    /// <param name="height">Height of the image in cm</param>
    void TableSetContentSmallTable(int column, int row, DataTable data, string heading, double width, double height = 6F);

    /// <summary>
    /// Add an HTML code
    /// </summary>
    /// <param name="html">HTML code to add</param>
    void AddHtml(string html);

    ///// <summary>
    ///// Create a footer with three section left, middle and right
    ///// </summary>
    ///// <param name="footerLeft">Content of left footer section</param>
    ///// <param name="footerMiddle">Content of left middle section</param>
    ///// <param name="footerRight">Content of left right section</param>
    ///// <param name="styleName">Style to use for the footer</param>
    //void CreateFooter3(string footerLeft, string footerMiddle, string footerRight, string styleName);

    /// <summary>
    /// Add a pagebreak to the content section
    /// </summary>
    void AddPageBreak();

    /// <summary>
    /// Get a style form the document
    /// </summary>
    /// <param name="styleName">Name of the style</param>
    /// <returns>Style</returns>
    Style GetStyle(string styleName);
}
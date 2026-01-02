// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using MigraDoc.DocumentObjectModel;

namespace Bodoconsult.Pdf.Stylesets;

/// <summary>
/// Interface for stylesets used to format PDF files created by PdfSharp and MigraDoc
/// </summary>
public interface IStyleSet
{
    /// <summary>
    /// Create the page setup. This method must set <see cref="PageSetup"/>!
    /// </summary>
    /// <returns>Page setup</returns>
    PageSetup CreatePageSetup();

    /// <summary>
    /// Calculate measures <see cref="TypeAreaWidth"/> and ColumnWidth!
    /// </summary>
    void CalculateMeasures();

    /// <summary>
    /// Initialize all styles
    /// </summary>
    void InitializeStyles();

    /// <summary>
    /// Number of text columns the type area is divided in
    /// </summary>
    int NumberOfColumns { get; set; } 

    /// <summary>
    /// The space between text columns in the type area in cm
    /// </summary>
    Unit Space { get; set; }

    /// <summary>
    /// Type area width
    /// </summary>
    Unit TypeAreaWidth { get; }

    /// <summary>
    /// The resulting text column width
    /// </summary>
    Unit ColumnWidth { get; set; }

    /// <summary>
    /// Normal paragraphs (default style)
    /// </summary>
    Style Normal { get; }

    /// <summary>
    /// Style used a table base. Do NOT change this style
    /// </summary>
    Style NormalTable { get; }

    /// <summary>
    /// Title
    /// </summary>
    Style Title { get; }

    /// <summary>
    /// Subtitle
    /// </summary>
    Style Subtitle { get; }

    /// <summary>
    /// Section title
    /// </summary>
    Style SectionTitle { get; }

    /// <summary>
    /// Section subtitle
    /// </summary>
    Style SectionSubtitle { get; }

    /// <summary>
    /// Heading level 1
    /// </summary>
    Style Heading1 { get; }

    /// <summary>
    /// Heading level 2
    /// </summary>
    Style Heading2 { get; }

    /// <summary>
    /// Heading level 3
    /// </summary>
    Style Heading3 { get; }

    /// <summary>
    /// Heading level 4
    /// </summary>
    Style Heading4 { get; }

    /// <summary>
    /// Heading level 5
    /// </summary>
    Style Heading5 { get; }

    /// <summary>
    /// No heading 1 for small tables
    /// </summary>
    Style NoHeading1 { get; }

    /// <summary>
    /// Chart title style
    /// </summary>
    Style ChartTitle { get; }

    /// <summary>
    /// Chart y-axis label style
    /// </summary>
    Style ChartYLabel { get; }

    /// <summary>
    /// Style used for tables
    /// </summary>
    Style Table { get; }

    /// <summary>
    /// Style used for table legends
    /// </summary>
    Style TableLegend { get; }

    /// <summary>
    /// Style used for figure legends
    /// </summary>
    Style FigureLegend { get; }

    /// <summary>
    /// Style used for equation legends
    /// </summary>
    Style EquationLegend { get; }

    /// <summary>
    /// Style for TOC section heading
    /// </summary>
    Style TocHeading { get; }

    /// <summary>
    /// Style for TOC heading 1
    /// </summary>
    Style Toc1 { get; }

    /// <summary>
    /// Style for TOC heading 2
    /// </summary>
    Style Toc2 { get; }

    /// <summary>
    /// Style for TOC heading 3
    /// </summary>
    Style Toc3 { get; }

    /// <summary>
    /// Style for TOC heading 4
    /// </summary>
    Style Toc4 { get; }

    /// <summary>
    /// Style for TOC heading 5
    /// </summary>
    Style Toc5 { get; }

    /// <summary>
    /// Style for TOE section heading
    /// </summary>
    Style ToeHeading { get; }

    /// <summary>
    /// Style for TOE entry
    /// </summary>
    Style Toe { get; }

    /// <summary>
    /// Style for TOF section heading
    /// </summary>
    Style TofHeading { get; }

    /// <summary>
    /// Style for TOE entry
    /// </summary>
    Style Tof { get; }

    /// <summary>
    /// Style for TOT section heading
    /// </summary>
    Style TotHeading { get; }

    /// <summary>
    /// Style for TOE entry
    /// </summary>
    Style Tot { get; }

    /// <summary>
    /// Page header style
    /// </summary>
    Style Header { get; }

    /// <summary>
    /// Details style
    /// </summary>
    Style Details { get; }

    /// <summary>
    /// Chart cell style
    /// </summary>
    Style ChartCell { get; }

    /// <summary>
    /// Page footer style
    /// </summary>
    Style Footer { get; }

    /// <summary>
    /// Style for code segment paragraphs
    /// </summary>
    Style Code { get; }

    /// <summary>
    /// Style for info segment paragraphs
    /// </summary>
    Style Info { get; }

    /// <summary>
    /// Style for warning segment paragraphs
    /// </summary>
    Style Warning { get; }

    /// <summary>
    /// Style for error segment paragraphs
    /// </summary>
    Style Error { get; }

    /// <summary>
    /// Style for citation segment paragraphs
    /// </summary>
    Style Citation { get; }

    /// <summary>
    /// Style for citation source segment paragraphs
    /// </summary>
    Style CitationSource { get; }

    /// <summary>
    /// A style for bulleted lists
    /// </summary>
    Style Bullet1 { get; }

    /// <summary>
    /// A style to add empty space of make margins top on pages possible
    /// </summary>
    Style Empty { get; }

    /// <summary>
    /// Style for definition list term
    /// </summary>
    Style DefinitionListTerm { get; }

    /// <summary>
    /// Style for definition list item
    /// </summary>
    Style DefinitionListItem { get; }

    /// <summary>
    /// Current page setup
    /// </summary>
    PageSetup PageSetup { get; set; }

    /// <summary>
    /// The original page setup. Used only for multi-column text PDF documents
    /// </summary>
    PageSetup PageSetupOriginal { get; set; }

    /// <summary>
    /// Centered paragraph style
    /// </summary>
    Style ParagraphCenter { get; set; }

    /// <summary>
    /// Right-aligned paragraph style
    /// </summary>
    Style ParagraphRight { get; set; }

    /// <summary>
    /// Justified paragraph style
    /// </summary>
    Style ParagraphJustify { get; set; }
}
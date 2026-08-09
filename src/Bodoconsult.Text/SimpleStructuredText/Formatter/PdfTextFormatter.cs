// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Data;
using System.IO;
using System.Runtime.Versioning;
using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Pdf.Stylesets;
using Bodoconsult.Text.Enums;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;
using Bodoconsult.Pdf.Extensions;
using MigraDoc.DocumentObjectModel;

namespace Bodoconsult.Text.Formatter;

/// <summary>
/// An <see cref="ITextFormatter"/> implementation creating a PDF file
/// </summary>
[SupportedOSPlatform("windows")]
public class PdfTextFormatter : ITextFormatter, IDisposable
{
    /// <summary>
    /// The increment to use
    /// </summary>
    public int Increment { get; set; }

    /// <summary>
    /// Current style set
    /// </summary>
    public IStyleSet StyleSet { get; set; }

    /// <summary>
    /// Background color
    /// </summary>
    public Color BackColor { get; set; }

    /// <summary>
    /// Border color for table borders
    /// </summary>
    public Color TableBorderColor { get; set; }

    /// <summary>
    /// Color to use for headline
    /// </summary>
    public Color HeadLineColor { get; set; }

    /// <summary>
    /// Color for details
    /// </summary>
    public Color DetailsColor { get; set; }

    /// <summary>
    /// Path to the logo image file
    /// </summary>
    public string LogoImagePath { get; set; }

    /// <summary>
    /// Subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Get the formatted text
    /// </summary>
    /// <returns>formatted text</returns>
    public string GetFormattedText()
    {
        var dl = new DataTable();
        dl.Columns.Add("Left");
        dl.Columns.Add("Right");


        Pdf = new PdfCreator(StyleSet)
        {
            Increment = Increment,
        };

        //Pdf.SetDocInfo(Title, Subject, Author);
        Pdf.SetHeader();
        Pdf.SetFooter();
        // Pdf.BackgroundImagePath = BackgroundImagePath;
        Pdf.CreateTocSection("Inhaltsverzeichnis");
        Pdf.CreateContentSection();

        for (var i = 0; i < StructuredText.TextItems.Count; i++)
        {

            var ti = StructuredText.TextItems[i];
            var content = ti.Content.Replace("??br??", "\r\n");

            switch (ti.LogicalType)
            {
                case TextItemType.H1:
                    Pdf.AddParagraph(content, "Heading1");
                    break;
                case TextItemType.H2:
                    Pdf.AddParagraph(content, "Heading2");
                    break;
                case TextItemType.H3:
                    Pdf.AddParagraph(content, "Heading3");
                    break;
                case TextItemType.H4:
                    Pdf.AddParagraph(content, "Heading4");
                    break;
                case TextItemType.P:
                    Pdf.AddParagraph(content, "Normal");
                    break;
                case TextItemType.ListItem:
                    Pdf.AddParagraph(content, "Bullet1");
                    break;
                case TextItemType.DefinitionListLine:

                    var isfirst = false;
                    var islast = false;

                    if (i == 0)
                    {
                        isfirst = true;
                    }
                    else if (i == StructuredText.TextItems.Count - 1)
                    {
                        islast = true;
                    }
                    else
                    {
                        var itemBefore = StructuredText.TextItems[i - 1];
                        if (itemBefore.LogicalType != TextItemType.DefinitionListLine) isfirst = true;

                        var itemAfter = StructuredText.TextItems[i + 1];
                        if (itemAfter.LogicalType != TextItemType.DefinitionListLine) islast = true;
                    }

                    if (isfirst)
                    {
                        dl.Clear();

                    }

                    var dlitem = (DefinitionListTextItem)ti;

                    var row = dl.Rows.Add();
                    row["Left"] = dlitem.Content;
                    row["Right"] = dlitem.Content2;

                    if (islast)
                    {
                        Pdf.AddDefinitionList(dl, "Normal", "Normal");
                    }
                    break;
                case TextItemType.Xml:
                case TextItemType.Code:
                    Pdf.AddParagraph(content, "Code");
                    break;
                case TextItemType.Table:
                    GetTable((TableTextItem)ti);
                    break;
            }


        }

        return null;
    }

    private void GetTable(TableTextItem ti)
    {
        var dt = new DataTable();

        using (var stream = new MemoryStream())
        {
            var writer = new StreamWriter(stream);
            writer.Write(ti.DataTableXml);
            writer.Flush();
            stream.Position = 0;
            dt.ReadXml(stream);
        }

        var pdfTable = string.Equals(dt.Columns[0].ColumnName, "cssstyle", StringComparison.InvariantCultureIgnoreCase) ?
            dt.ToPdfTableWithCssInfo(Bodoconsult.Pdf.Extensions.DataTableExtensions.BodoconsultCssColors) :
            dt.ToPdfTable();

        pdfTable.Heading = ti.Content;
        pdfTable.HeadingStyleName = "NoHeading1";

        Pdf.AddTable(pdfTable);

    }

    /// <summary>Structured text to format</summary>
    public IStructuredText StructuredText { get; set; }

    /// <summary>
    /// Template to place the structered text. Must contain placeholder {0} for the structured text
    /// </summary>
    public string Template { get; set; }

    /// <summary>Title for documentation</summary>
    public string Title { get; set; }

    /// <summary>
    /// Show the date the text was created. I.e.: "date created: 7.7.2018"
    /// </summary>
    public string DateString { get; set; }

    /// <summary>
    /// Save as file
    /// </summary>
    /// <param name="fileName">Full path for the PDF file</param>
    public void SaveAsFile(string fileName)
    {
        Pdf.RenderToPdf(fileName, false);
    }



    /// <summary>
    /// PdfCreator instance: do not use directly
    /// </summary>
    public PdfCreator Pdf;

    /// <summary>
    /// Default ctor
    /// </summary>
    public PdfTextFormatter()
    {
        BackColor = new Color(211, 223, 240);
        TableBorderColor = new Color(217, 230, 245);
        HeadLineColor = Colors.Black;
        DetailsColor = Colors.Black;

        StyleSet = new DefaultStyleSet();
        StyleSet.CreatePageSetup();
        StyleSet.CalculateMeasures();
        StyleSet.InitializeStyles();

    }

    /// <summary>
    /// ctor with a stylesheet for customized formatting
    /// </summary>
    /// <param name="styleSet"></param>
    public PdfTextFormatter(IStyleSet styleSet)
    {
        BackColor = new Color(211, 223, 240);
        TableBorderColor = new Color(217, 230, 245);
        HeadLineColor = Colors.Black;
        DetailsColor = Colors.Black;

        StyleSet = styleSet;
        StyleSet.CreatePageSetup();
        StyleSet.CalculateMeasures();
        StyleSet.InitializeStyles();
    }


    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        try
        {
            Pdf.Dispose();
        }
        catch
        {
            // ignored
        }
    }
}

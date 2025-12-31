// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Pdf.Stylesets;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;
using Color = MigraDoc.DocumentObjectModel.Color;
using Document = Bodoconsult.Text.Documents.Document;

namespace Bodoconsult.Text.Renderer.Pdf;

/// <summary>
/// Render a <see cref="Documents.Document"/> to a PDF file
/// </summary>
public class PdfTextDocumentRenderer : BaseDocumentRenderer
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="document">Document to render</param>
    /// <param name="textRendererElementFactory">Current factory for text renderer elements</param>
    /// <param name="fontResolver">Current font resolver</param>
    public PdfTextDocumentRenderer(Document document, ITextRendererElementFactory textRendererElementFactory, IFontResolver fontResolver) : base(document)
    {
        var metaData = document.DocumentMetaData;

        PdfTextRendererElementFactory = (IPdfTextRendererElementFactory)textRendererElementFactory;
        IStyleSet styleSet = new DefaultStyleSet();
        styleSet.CreatePageSetup();

        var style = (DocumentStyle)Styleset.FindStyle("DocumentStyle");
        if (style != null)
        {
            PdfBuilderBase.SetPage(style, styleSet.PageSetup);

            styleSet.NumberOfColumns = style.NumberOfColumns;
            styleSet.Space = Unit.FromCentimeter(style.Space);

            if (style.NumberOfColumns > 1)
            {
                styleSet.ColumnWidth = Unit.FromCentimeter(style.ColumnWidth);
            }
            else
            {
                styleSet.ColumnWidth = Unit.FromCentimeter(style.TypeAreaWidth);
            }

        }


        styleSet.CalculateMeasures();
        styleSet.InitializeStyles();


        PdfDocument = new PdfBuilder(styleSet, fontResolver);

        PdfDocument.TitleTableOfFigures = metaData.TofHeading;
        PdfDocument.TitleTableOfEquations = metaData.ToeHeading;
        PdfDocument.TitleTableOfTables = metaData.TotHeading;
        PdfDocument.TitleTableOfContent = metaData.TocHeading;
        PdfDocument.BackColor = new Color(metaData.BackColor.A, metaData.BackColor.R, metaData.BackColor.G, metaData.BackColor.B);
        PdfDocument.AlternateBackColor = new Color(metaData.AlternateBackColor.A, metaData.AlternateBackColor.R, metaData.AlternateBackColor.G, metaData.AlternateBackColor.B);
        PdfDocument.TableBorderColor = new Color(metaData.TableBorderColor.A, metaData.TableBorderColor.R, metaData.TableBorderColor.G, metaData.TableBorderColor.B);
    }

    /// <summary>
    /// The current PDF document
    /// </summary>
    public PdfBuilder PdfDocument { get; }

    /// <summary>
    /// Current styleset
    /// </summary>
    public IStyleSet StyleSet { get; set; }

    /// <summary>
    /// Current text renderer element factory
    /// </summary>
    public IPdfTextRendererElementFactory PdfTextRendererElementFactory { get; protected set; }

    /// <summary>
    /// Render the document
    /// </summary>
    public override void RenderIt()
    {


        var rendererElement = PdfTextRendererElementFactory.CreateInstancePdf(Document);
        rendererElement.RenderIt(this);
    }

    /// <summary>
    /// Save the rendered document as file
    /// </summary>
    /// <param name="fileName">Full file path. Existing file will be overwritten</param>
    public override void SaveAsFile(string fileName)
    {
        PdfDocument.RenderToPdf(fileName, false);
    }
}
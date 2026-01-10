// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.Factories;
using Bodoconsult.Pdf.Interfaces;
using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Pdf.Stylesets;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Interfaces;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;
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
        styleSet.DocumentMetaData = metaData;

        styleSet.CreatePageSetup();

        var style = (DocumentStyle)Styleset.FindStyle("DocumentStyle");
        if (style != null)
        {
            PdfBuilderBase.SetPage(style, styleSet);

            styleSet.NumberOfColumns = style.NumberOfColumns;
            styleSet.Space = Unit.FromCentimeter(style.ColumnGap);

            styleSet.ColumnWidth = Unit.FromCentimeter(style.NumberOfColumns > 1 ? style.ColumnWidth : style.TypeAreaWidth);
        }


        styleSet.CalculateMeasures();
        styleSet.InitializeStyles();

        var factory = new PdfBuilderFactory(fontResolver);

        PdfDocument = factory.CreateInstance(styleSet);
        PdfDocument.TitleTableOfFigures = metaData.TofHeading;
        PdfDocument.TitleTableOfEquations = metaData.ToeHeading;
        PdfDocument.TitleTableOfTables = metaData.TotHeading;
        PdfDocument.TitleTableOfContent = metaData.TocHeading;
    }

    /// <summary>
    /// The current PDF document
    /// </summary>
    public IPdfBuilder PdfDocument { get; }

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
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Office;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Docx;

/// <summary>
/// Render a <see cref="Document"/> to a DOCX file
/// </summary>
public class DocxTextDocumentRenderer : BaseDocumentRenderer
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="document">Document to render</param>
    /// <param name="textRendererElementFactory">Current factory for text renderer elements</param>
    public DocxTextDocumentRenderer(Document document, ITextRendererElementFactory textRendererElementFactory) : base(document)
    {
        DocumentMetaData = document.DocumentMetaData;

        PdfTextRendererElementFactory = (IDocxTextRendererElementFactory)textRendererElementFactory;
        DocxDocument = new DocxBuilder(DocumentMetaData);
        DocxDocument.CreateDocument();

        //DocxDocument.TitleTableOfFigures = metaData.TofHeading;
        //DocxDocument.TitleTableOfEquations = metaData.ToeHeading;
        //DocxDocument.TitleTableOfTables = metaData.TotHeading;
        //DocxDocument.TitleTableOfContent = metaData.TocHeading;
        //DocxDocument.BackColor = new Color(metaData.BackColor.A, metaData.BackColor.R, metaData.BackColor.G, metaData.BackColor.B);
        //DocxDocument.AlternateBackColor = new Color(metaData.AlternateBackColor.A, metaData.AlternateBackColor.R, metaData.AlternateBackColor.G, metaData.AlternateBackColor.B);
        //DocxDocument.TableBorderColor = new Color(metaData.TableBorderColor.A, metaData.TableBorderColor.R, metaData.TableBorderColor.G, metaData.TableBorderColor.B);
    }

    /// <summary>
    /// Current metadata
    /// </summary>
    DocumentMetaData DocumentMetaData { get; }

    /// <summary>
    /// The current PDF document
    /// </summary>
    public DocxBuilder DocxDocument { get; }

    /// <summary>
    /// User interface priority
    /// </summary>
    public int UiPriority { get; set; }

    /// <summary>
    /// Current text renderer element factory
    /// </summary>
    public IDocxTextRendererElementFactory PdfTextRendererElementFactory { get; protected set; }

    /// <summary>
    /// Render the document
    /// </summary>
    public override void RenderIt()
    {
        var rendererElement = PdfTextRendererElementFactory.CreateInstanceDocx(Document);
        rendererElement.RenderIt(this);
    }

    /// <summary>
    /// Save the rendered document as file
    /// </summary>
    /// <param name="fileName">Full file path. Existing file will be overwritten</param>
    public override void SaveAsFile(string fileName)
    {
        DocxDocument.SaveDocument(fileName);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Pdf.Styles;

/// <summary>
/// Base class for <see cref="PageStyleBase"/> based styles
/// </summary>
public abstract class PdfPageStyleTextRendererElementBase : IPdfTextRendererElement
{

    /// <summary>
    /// Current block to renderer
    /// </summary>
    public PageStyleBase Style { get; private set; }

    /// <summary>
    /// CSS class name
    /// </summary>
    public string ClassName { get; protected set; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="style">Current page style</param>
    protected PdfPageStyleTextRendererElementBase(PageStyleBase style)
    {
        Style = style;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public void RenderIt(PdfTextDocumentRenderer renderer)
    {
        //var pdfStyle = renderer.PdfDocument.PageSetup;

        //pdfStyle.Orientation = Style.TypeAreaHeight < Style.TypeAreaWidth ? Orientation.Landscape : Orientation.Portrait;
        //pdfStyle.PageWidth = Unit.FromCentimeter(Style.PaperFormat.Size.Width);
        //pdfStyle.PageHeight = Unit.FromCentimeter(Style.PaperFormat.Size.Height);
        //pdfStyle.LeftMargin = Unit.FromCentimeter(Style.Margins.Left);
        //pdfStyle.RightMargin = Unit.FromCentimeter(Style.Margins.Right);
        //pdfStyle.TopMargin = Unit.FromCentimeter(Style.Margins.Top);
        //pdfStyle.BottomMargin = Unit.FromCentimeter(Style.Margins.Bottom);

        //// ToDo: other formats
        //pdfStyle.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;


        // https://stackoverflow.com/questions/44578660/adding-multi-page-migradoc-document-to-a-pdfsharp-document
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public void RenderIt(ITextDocumentRenderer renderer)
    {
        throw new NotSupportedException();
    }
}
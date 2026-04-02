// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Pdf.Styles;

/// <summary>
/// Base class for <see cref="ParagraphStyleBase"/> based styles
/// </summary>
public class PdfParagraphStyleTextRendererElementBase : IPdfTextRendererElement
{

    /// <summary>
    /// Current block to renderer
    /// </summary>
    public ParagraphStyleBase Style { get; private set; }

    /// <summary>
    /// CSS class name
    /// </summary>
    public string ClassName { get; protected set; }

    /// <summary>
    /// Additonal CSS styling tags
    /// </summary>
    public List<string> AdditionalCss { get; } = [];

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="style">Current paragraph style</param>
    public PdfParagraphStyleTextRendererElementBase(ParagraphStyleBase style)
    {
        Style = style;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public virtual void RenderIt(ITextDocumentRenderer renderer)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public void RenderIt(PdfTextDocumentRenderer renderer)
    {

        var styleName = Style.Name.Replace("Style", string.Empty);

        if (styleName == "Paragraph")
        {
            styleName = "Normal";
        }

        var pdfStyle = renderer.PdfDocument.GetStyle(styleName);

        Debug.Print($"{Style.Name} => {pdfStyle?.Name}");

        PdfDocumentRendererHelper.RenderParagraphStyle(Style, pdfStyle);
    }
}
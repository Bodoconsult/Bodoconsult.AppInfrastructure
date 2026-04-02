// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Docx.Styles;

/// <summary>
/// Base class for <see cref="ParagraphStyleBase"/> based styles
/// </summary>
public class DocxParagraphStyleTextRendererElementBase : IDocxTextRendererElement
{

    /// <summary>
    /// Current block to renderer
    /// </summary>
    public ParagraphStyleBase Style { get; }

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
    public DocxParagraphStyleTextRendererElementBase(ParagraphStyleBase style)
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
    public void RenderIt(DocxTextDocumentRenderer renderer)
    {
        Debug.Print(Style.Name);

        var styleName = Style.Name.Replace("Style", string.Empty);

        if (styleName == "Paragraph")
        {
            styleName = "Normal";
        }

        renderer.DocxDocument.AddNewStyle(styleName, styleName, Style, renderer.UiPriority);
        renderer.UiPriority++;
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Text;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Rtf.Styles;

/// <summary>
/// Base class for <see cref="ParagraphStyleBase"/> based styles
/// </summary>
public class RtfParagraphStyleTextRendererElementBase : ITextRendererElement
{
    /// <summary>
    /// Rtf tag to use for rendering
    /// </summary>
    protected string TagToUse = "p";

    /// <summary>
    /// Current block to renderer
    /// </summary>
    public ParagraphStyleBase Style { get; private set; }
    /// <summary>
    /// CSS class name
    /// </summary>
    public string ClassName { get; protected set; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="style">Current paragraph style</param>
    public RtfParagraphStyleTextRendererElementBase(ParagraphStyleBase style)
    {
        Style = style;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public virtual void RenderIt(ITextDocumentRenderer renderer)
    {
        var name = Style.GetType().Name;

        var sb = new StringBuilder();

        sb.Append($"{{p{renderer.Styleset.GetIndexOfStyle(name)} {RtfHelper.GetFormatSettings(Style, renderer.Styleset)} {name};");

        sb.Append($"}}{Environment.NewLine}");
        renderer.Content.Append(sb);
    }
}
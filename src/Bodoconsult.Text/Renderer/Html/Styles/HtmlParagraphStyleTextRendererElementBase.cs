// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.Text;
using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Extensions;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Html.Styles;

/// <summary>
/// Base class for <see cref="ParagraphStyleBase"/> based styles
/// </summary>
public class HtmlParagraphStyleTextRendererElementBase: ITextRendererElement
{
    /// <summary>
    /// HTML tag to use for rendering
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
    /// Additonal CSS styling tags
    /// </summary>
    public List<string> AdditionalCss { get;  } = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="style">Current paragraph style</param>
    public HtmlParagraphStyleTextRendererElementBase(ParagraphStyleBase style)
    {
        Style = style;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public virtual void RenderIt(ITextDocumentRenderer renderer)
    {
        var sb = new StringBuilder();

        sb.AppendLine($".{Style.GetType().Name}");
        sb.AppendLine("{");
        sb.AppendLine($"     font-family: \"{Style.FontName}\";");
        sb.AppendLine($"     font-size: {Style.FontSize.ToString("0")}pt;");

        if (Style.Bold)
        {
            sb.AppendLine("     font-weight: bold;");
        }

        if (Style.Italic)
        {
            sb.AppendLine("     font-style: italic;");
        }
        
        sb.AppendLine($"     margin: {Style.Margins.Top.ToString("0.00")}cm {Style.Margins.Right.ToString("0.00")}cm {Style.Margins.Bottom.ToString("0.00")}cm {Style.Margins.Left.ToString("0.00")}cm;");
        sb.AppendLine($"     padding: {Style.Paddings.Top.ToString("0")}pt {Style.Paddings.Right.ToString("0")}pt {Style.Paddings.Bottom.ToString("0")}pt {Style.Paddings.Left.ToString("0")}pt;");
        sb.AppendLine($"     border-width: {Style.BorderThickness.Top.FromCmToPoint()}pt {Style.BorderThickness.Right.FromCmToPoint()}pt {Style.BorderThickness.Bottom.FromCmToPoint()}pt {Style.BorderThickness.Left.FromCmToPoint()}pt;");
        
        var color = (Color)Style.BorderBrush?.TypoColor;
        sb.AppendLine($"     border-color: {color?.ToHtml() ?? "#000000"};");
        sb.AppendLine("     border-style: solid;");
        sb.AppendLine($"     text-align: {Style.TextAlignment.ToString().ToLowerInvariant()};");

        // Shading
        if (Style.TypoShading != null)
        {
            sb.AppendLine($"     background-color: {Style.TypoShading.ToHtml()};");
        }

        foreach (var css in AdditionalCss)
        {
            sb.AppendLine($"     {css}");
        }

        
        sb.AppendLine("}");
        renderer.Content.Append(sb);
    }
}
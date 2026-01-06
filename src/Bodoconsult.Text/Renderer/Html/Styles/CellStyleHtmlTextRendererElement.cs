// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Extensions;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Html.Styles;

/// <summary>
/// HTML rendering element for table cells instances
/// </summary>
public abstract class CellStyleHtmlTextRendererElement : HtmlParagraphStyleTextRendererElementBase
{
    /// <summary>
    /// Default ctor
    /// </summary>
    protected CellStyleHtmlTextRendererElement(ParagraphStyleBase style) : base(style)
    {
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {
        var tableStyle = (TableStyle)renderer.Styleset.FindStyle("TableStyle");

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

        var color = tableStyle.BorderBrush.TypoColor.ToHtml();
        sb.AppendLine($"     border-left: {tableStyle.BorderThickness.Left.FromCmToPoint()}pt solid {color};");
        sb.AppendLine($"     border-top: {tableStyle.BorderThickness.Top.FromCmToPoint()}pt solid  {color};");
        sb.AppendLine($"     border-right: {tableStyle.BorderThickness.Right.FromCmToPoint()}pt solid  {color};");
        sb.AppendLine($"     border-bottom: {tableStyle.BorderThickness.Bottom.FromCmToPoint()}pt solid # {color};");
        sb.AppendLine($"     text-align: {Style.TextAlignment.ToString().ToLowerInvariant()};");

        foreach (var css in AdditionalCss)
        {
            sb.AppendLine($"     {css}");
        }

        sb.AppendLine("}");
        renderer.Content.Append(sb);
    }
}
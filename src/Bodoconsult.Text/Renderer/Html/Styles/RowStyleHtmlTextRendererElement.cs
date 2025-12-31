// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Html.Styles;

/// <summary>
/// HTML rendering element for <see cref="RowStyle"/> instances
/// </summary>
public class RowStyleHtmlTextRendererElement : HtmlStyleTextRendererElementBase
{
    private readonly RowStyle _rowStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public RowStyleHtmlTextRendererElement(RowStyle rowStyle)
    {
        _rowStyle = rowStyle;
        ClassName = "RowStyle";
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {
        //var style = (TableStyle)renderer.Styleset.FindStyle("TableStyle");

        var sb = new StringBuilder();

        sb.AppendLine($".{_rowStyle.GetType().Name}");
        sb.AppendLine("{");

        //var color = style.BorderBrush.Color.ToHtml();
        //sb.AppendLine($"\tborder-left: {style.BorderThickness.Left.FromCmToPoint()}pt solid {color};");
        //sb.AppendLine($"\tborder-top: {style.BorderThickness.Top.FromCmToPoint()}pt solid  {color};");
        //sb.AppendLine($"\tborder-right: {style.BorderThickness.Right.FromCmToPoint()}pt solid  {color};");
        //sb.AppendLine($"\tborder-bottom: {style.BorderThickness.Bottom.FromCmToPoint()}pt solid # {color};");
        sb.AppendLine("}");
        renderer.Content.Append(sb);
    }
}
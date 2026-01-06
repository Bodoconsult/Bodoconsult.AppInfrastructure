// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Docx.Styles;

/// <summary>
/// Docx rendering element for <see cref="ListStyle"/> instances
/// </summary>
public class ListStyleDocxTextRendererElement : DocxParagraphStyleTextRendererElementBase
{
    private readonly ListStyle _listStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public ListStyleDocxTextRendererElement(ListStyle listStyle) : base(listStyle)
    {
        _listStyle = listStyle;
        ClassName = "ListStyle";
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {
        // Get the content of all inlines as string
        var sb = new StringBuilder();

        sb.AppendLine($".{Style.GetType().Name}");
        sb.AppendLine("{");
        sb.AppendLine($"     font-family: \"{Style.FontName}\";");
        sb.AppendLine($"     font-size: {Style.FontSize}pt;");
        sb.AppendLine($"     margin: {Style.Margins.Top}pt {Style.Margins.Right}pt {Style.Margins.Bottom}pt {Style.Margins.Left}pt;");
        sb.AppendLine($"     border-width: {Style.BorderThickness.Top}pt {Style.BorderThickness.Right}pt {Style.BorderThickness.Bottom}pt {Style.BorderThickness.Left}pt;");

        var color = (Color)Style.BorderBrush?.TypoColor;
        sb.AppendLine($"     border-color: {color?.ToHtml() ?? "#000000"};");
        sb.AppendLine("}");
        renderer.Content.Append(sb);
    }
}
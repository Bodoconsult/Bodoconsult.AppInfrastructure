using System.Text;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Html.Styles;

/// <summary>
/// HTML rendering element for <see cref="ListItemStyle"/> instances
/// </summary>
public class ListItemStyleHtmlTextRendererElement : HtmlParagraphStyleTextRendererElementBase
{
    private readonly ListItemStyle _listItemStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public ListItemStyleHtmlTextRendererElement(ListItemStyle listItemStyle) : base(listItemStyle)
    {
        _listItemStyle = listItemStyle;
        ClassName = "ListItemStyle";
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
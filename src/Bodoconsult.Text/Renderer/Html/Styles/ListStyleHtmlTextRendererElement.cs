using System.Text;
using Bodoconsult.App.Extensions;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Html.Styles;

/// <summary>
/// HTML rendering element for <see cref="ListStyle"/> instances
/// </summary>
public class ListStyleHtmlTextRendererElement : HtmlParagraphStyleTextRendererElementBase
{
    private readonly ListStyle _listStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public ListStyleHtmlTextRendererElement(ListStyle listStyle) : base(listStyle)
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
        sb.AppendLine($"     margin: {Style.Margins.Top}cm {Style.Margins.Right}cm {Style.Margins.Bottom}cm {Style.Margins.Left}cm;");
        sb.AppendLine($"     border-width: {Style.BorderThickness.Top.FromCmToPoint()}pt {Style.BorderThickness.Right.FromCmToPoint()}pt {Style.BorderThickness.Bottom.FromCmToPoint()}pt {Style.BorderThickness.Left.FromCmToPoint()}pt;");

        var color = (Color)Style.BorderBrush?.TypoColor;
        sb.AppendLine($"     border-color: {color?.ToHtml() ?? "#000000"};");
        sb.AppendLine("}");
        renderer.Content.Append(sb);
    }
}
using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Html.Styles;

/// <summary>
/// HTML rendering element for <see cref="WarningStyle"/> instances
/// </summary>
public class WarningStyleHtmlTextRendererElement : HtmlParagraphStyleTextRendererElementBase
{
    private readonly WarningStyle _warningStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WarningStyleHtmlTextRendererElement(WarningStyle warningStyle) : base(warningStyle)
    {
        _warningStyle = warningStyle;
        ClassName = "WarningStyle";
    }
}
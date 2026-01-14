// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Html;

/// <summary>
/// HTML rendering element for <see cref="Document"/> instances
/// </summary>
public class DocumentHtmlTextRendererElement : HtmlTextRendererElementBase
{
    private readonly Document _document;

    /// <summary>
    /// Default ctor
    /// </summary>
    public DocumentHtmlTextRendererElement(Document document) : base(document)
    {
        _document = document;
        ClassName = document.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {
        renderer.Content.AppendLine("<!DOCTYPE html>");
        renderer.Content.AppendLine("<html>");
        renderer.Content.AppendLine("<head>");
        renderer.Content.AppendLine("<meta charset=\"utf-8\">");

        DocumentRendererHelper.RenderBlockChildsToHtml(renderer, _document.ChildBlocks);

        //renderer.Content.AppendLine("</div>");
        renderer.Content.AppendLine("</body>");
        renderer.Content.AppendLine("</html>");

    }
}
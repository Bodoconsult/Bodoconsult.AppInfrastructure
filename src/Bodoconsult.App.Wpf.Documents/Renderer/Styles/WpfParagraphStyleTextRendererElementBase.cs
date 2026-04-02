// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using Bodoconsult.App.Wpf.Documents.Helpers;
using Bodoconsult.App.Wpf.Documents.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;
using Paragraph = System.Windows.Documents.Paragraph;

namespace Bodoconsult.App.Wpf.Documents.Renderer.Styles;

/// <summary>
/// Base class for <see cref="ParagraphStyleBase"/> based styles
/// </summary>
public class WpfParagraphStyleTextRendererElementBase : IWpfTextRendererElement
{

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
    public List<string> AdditionalCss { get; } = [];

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="style">Current paragraph style</param>
    public WpfParagraphStyleTextRendererElementBase(ParagraphStyleBase style)
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
    public virtual void RenderIt(WpfTextDocumentRenderer renderer)
    {

        var styleName = Style.Name;

        renderer.Dispatcher.Invoke(() =>
        {
            var style = new Style(typeof(Paragraph));

            WpfDocumentRendererHelper.RenderParagraphStyle(Style, style);
            renderer.StyleSet.Add(styleName, style);
        });
    }
}
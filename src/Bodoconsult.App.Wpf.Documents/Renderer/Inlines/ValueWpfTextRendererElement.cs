// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Documents;
using Bodoconsult.Text.Documents;
using Inline = Bodoconsult.Text.Documents.Inline;
using Paragraph = System.Windows.Documents.Paragraph;
using Span = System.Windows.Documents.Span;
using TextElement = System.Windows.Documents.TextElement;

namespace Bodoconsult.App.Wpf.Documents.Renderer.Inlines;

/// <summary>
/// Render a <see cref="Bodoconsult.Text.Documents.Value"/> element
/// </summary>
public class ValueWpfTextRendererElement : InlineWpfTextRendererElementBase
{
    private readonly Value _value;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="value">Paragraph</param>
    public ValueWpfTextRendererElement(Value value)
    {
        _value = value;
    }

    /// <summary>
    /// Render the inline to a string
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="element">Base text element</param>
    /// <param name="childInlines">Child inlines of an inline</param>
    /// <exception cref="NotSupportedException"></exception>
    public override void RenderToElement(WpfTextDocumentRenderer renderer, TextElement element,
        List<Inline> childInlines)
    {

        if (element is Paragraph paragraph)
        {
            var span = new Span(new Run(_value.Content));
            paragraph.Inlines.Add(span);

            return;
        }

        if (element is System.Windows.Documents.Inline inline)
        {



        }
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.Text.Documents;

namespace Bodoconsult.App.Wpf.Documents.Renderer.Blocks;

/// <summary>
/// WPF rendering element for <see cref="Document"/> instances
/// </summary>
public class DocumentWpfTextRendererElement : WpfTextRendererElementBase
{
    private readonly Document _document;

    /// <summary>
    /// Default ctor
    /// </summary>
    public DocumentWpfTextRendererElement(Document document) : base(document)
    {
        _document = document;
        ClassName = document.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(WpfTextDocumentRenderer renderer)
    {

        var style = (PageStyleBase)renderer.Styleset.FindStyle("DocumentStyle");


        // Render TOC etc
        renderer.WpfDocument = renderer.WpfDocumentToc;

        var ps = renderer.PageSettings;

        renderer.Dispatcher.Invoke(() =>
        {
            renderer.WpfDocument.PageWidth = ps.PageSize.Width;
            renderer.WpfDocument.PageHeight = ps.PageSize.Height;
            renderer.WpfDocument.PagePadding = ps.Margins;

            // Multi-column text layout
            if (style.NumberOfColumns > 1)
            {
                renderer.WpfDocument.ColumnWidth = MeasurementHelper.GetPxFromCm(style.ColumnWidth);
                renderer.WpfDocument.ColumnGap = MeasurementHelper.GetPxFromCm(style.ColumnGap);
            }
            else // Singe-column text layout
            {
                renderer.WpfDocument.ColumnWidth = double.NaN;
                renderer.WpfDocument.ColumnGap = double.NaN;
            }

        });

        foreach (var child in _document.ChildBlocks)
        {

            if (child is Section)
            {
                break;
            }

            if (child is SectionBase sb)
            {
                ps.TocPageNumberFormat = sb.PageNumberFormat;
            }

            var element = renderer.WpfTextRendererElementFactory.CreateInstanceWpf(child);
            element.RenderIt(renderer);
        }

        // Render content
        renderer.WpfDocument = renderer.WpfDocumentContent;

        renderer.Dispatcher.Invoke(() =>
        {
            renderer.WpfDocument.PageWidth = ps.PageSize.Width;
            renderer.WpfDocument.PageHeight = ps.PageSize.Height;
            renderer.WpfDocument.PagePadding = ps.Margins;
            renderer.WpfDocument.ColumnWidth = double.NaN;
        });

        foreach (var child in _document.ChildBlocks)
        {

            if (child is not Section)
            {
                continue;
            }

            if (child is SectionBase sb)
            {
                ps.ContentPageNumberFormat = sb.PageNumberFormat;
            }

            var element = renderer.WpfTextRendererElementFactory.CreateInstanceWpf(child);
            element.RenderIt(renderer);
        }


    }
}
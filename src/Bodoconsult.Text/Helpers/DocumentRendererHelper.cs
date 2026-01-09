// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Renderer.Html;
using Bodoconsult.Text.Renderer.PlainText;
using Bodoconsult.Text.Renderer.Rtf.Blocks;
using Bodoconsult.Text.Renderer.Rtf.Inlines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bodoconsult.Text.Helpers;

/// <summary>
/// Helper class for document rendering
/// </summary>
public static class DocumentRendererHelper
{
    /// <summary>
    /// Get a page number in as formatted string
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageNumberFormat">Page number format</param>
    /// <returns>Formatted number string</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string GetFormattedNumber(int page, PageNumberFormatEnum pageNumberFormat)
    {
        switch (pageNumberFormat)
        {
            case PageNumberFormatEnum.Decimal:
                return page.ToString("0");
            case PageNumberFormatEnum.UpperRoman:
                return page.ArabicToRoman().ToUpperInvariant();
            case PageNumberFormatEnum.LowerRoman:
                return page.ArabicToRoman().ToLowerInvariant();
            case PageNumberFormatEnum.UpperLatin:
                return page.ToUpperLatin();
            case PageNumberFormatEnum.LowerLatin:
                return page.ToLowerLatin();
            default:
                throw new ArgumentOutOfRangeException(nameof(pageNumberFormat), pageNumberFormat, null);
        }
    }

    /// <summary>
    /// Render the child inlines
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="sb">String to render the nested inlines in</param>
    /// <param name="childInlines">Current child inlines</param>
    /// <param name="tag">Tag to add before and after the inlines</param>
    /// <param name="isBlock">Parent element is a block</param>
    public static void RenderInlineChildsToPlainText(ITextDocumentRenderer renderer, StringBuilder sb,
        List<Inline> childInlines,
        string tag = null, bool isBlock = false)
    {

        if (!string.IsNullOrEmpty(tag))
        {
            sb.Append(tag);
        }

        foreach (var inline in childInlines)
        {
            var rendererElement =
                (InlinePlainTextRendererElementBase)renderer.TextRendererElementFactory.CreateInstance(inline);
            rendererElement.RenderToString(renderer, sb);
        }

        if (!string.IsNullOrEmpty(tag))
        {
            sb.Append(tag);
        }
    }

    /// <summary>
    /// Render the child inlines
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="sb">String to render the nested inlines in</param>
    /// <param name="childInlines">Current child inlines</param>
    /// <param name="tag">Tag to add before and after the inlines</param>
    /// <param name="isBlock">Parent element is a block</param>
    public static void RenderInlineChildsToHtml(ITextDocumentRenderer renderer, StringBuilder sb,
        List<Inline> childInlines,
        string tag = null, bool isBlock = false)
    {

        if (!string.IsNullOrEmpty(tag))
        {
            sb.Append(tag);
        }

        foreach (var inline in childInlines)
        {
            var rendererElement =
                (InlineHtmlTextRendererElementBase)renderer.TextRendererElementFactory.CreateInstance(inline);
            rendererElement.RenderToString(renderer, sb);
        }

        if (!string.IsNullOrEmpty(tag))
        {
            sb.Append(tag);
        }
    }

    /// <summary>
    /// Render child blocks
    /// </summary>
    /// <param name="renderer">Current renderer instance</param>
    /// <param name="childBlocks">Child blocks</param>
    public static void RenderBlockChildsToHtml(ITextDocumentRenderer renderer, List<Block> childBlocks)
    {
        foreach (var block in childBlocks)
        {
            var rendererElement = renderer.TextRendererElementFactory.CreateInstance(block);
            rendererElement.RenderIt(renderer);
        }
    }

    /// <summary>
    /// Render table rows to HTML
    /// </summary>
    /// <param name="renderer">Current renderer instance</param>
    /// <param name="rows">Child blocks</param>
    public static void RenderRowsToHtml(ITextDocumentRenderer renderer, List<Row> rows)
    {
        foreach (var row in rows)
        {
            var rendererElement = renderer.TextRendererElementFactory.CreateInstance(row);
            rendererElement.RenderIt(renderer);
        }
    }

    /// <summary>
    /// Render items to HTML
    /// </summary>
    /// <param name="renderer">Current renderer instance</param>
    /// <param name="items">Items to render</param>
    public static void RenderItemsToHtml<T>(ITextDocumentRenderer renderer, List<T> items) where T : DocumentElement
    {
        foreach (var item in items)
        {
            var rendererElement = renderer.TextRendererElementFactory.CreateInstance(item);
            rendererElement.RenderIt(renderer);
        }
    }

    /// <summary>
    /// Render table rows to RTF
    /// </summary>
    /// <param name="renderer">Current renderer instance</param>
    /// <param name="rows">Child blocks</param>
    /// <param name="tableStyle"></param>
    public static void RenderRowsToRtf(ITextDocumentRenderer renderer, List<Row> rows, TableStyle tableStyle)
    {
        
        var backColor = tableStyle.TableBackColor;
        var alternateBackColor = tableStyle.TableAlternateBackColor;

        for (var index = 0; index < rows.Count; index++)
        {
            var row = rows[index];

            var cellColor = index % 2.0 < 0.01 ? alternateBackColor : backColor;
            
            var rendererElement = (RowRtfTextRendererElement)renderer.TextRendererElementFactory.CreateInstance(row);
            rendererElement.Color = cellColor;
            rendererElement.RenderIt(renderer);
        }
    }

    /// <summary>
    /// Render child blocks
    /// </summary>
    /// <param name="renderer">Current renderer instance</param>
    /// <param name="childBlocks">Child blocks</param>
    public static void RenderBlockChildsToPlain(ITextDocumentRenderer renderer, List<Block> childBlocks)
    {
        foreach (var block in childBlocks)
        {
            var rendererElement = renderer.TextRendererElementFactory.CreateInstance(block);
            rendererElement.RenderIt(renderer);
        }
    }

    /// <summary>
    /// Render child blocks
    /// </summary>
    /// <param name="renderer">Current renderer instance</param>
    /// <param name="childBlocks">Child blocks</param>
    public static void RenderBlockChildsToRtf(ITextDocumentRenderer renderer, List<Block> childBlocks)
    {
        foreach (var block in childBlocks)
        {
            var rendererElement = renderer.TextRendererElementFactory.CreateInstance(block);
            rendererElement.RenderIt(renderer);
        }
    }

    /// <summary>
    /// Render child inlines
    /// </summary>
    /// <param name="renderer">Current renderer instance</param>
    /// <param name="sb">Current string</param>
    /// <param name="childInlines">Child inlines</param>
    public static void RenderInlineChildsToRtf(ITextDocumentRenderer renderer, StringBuilder sb,
        List<Inline> childInlines)
    {
        foreach (var inline in childInlines)
        {
            var rendererElement =
                (InlineRtfTextRendererElementBase)renderer.TextRendererElementFactory.CreateInstance(inline);
            rendererElement.RenderToString(renderer, sb);
        }
    }

    /// <summary>
    /// Render tables cells  to HTML
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="cells">List of all cellss to render</param>
    public static void RenderCellsToHtml(ITextDocumentRenderer renderer, List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            var rendererElement = (CellHtmlTextRendererElement)renderer.TextRendererElementFactory.CreateInstance(cell);
            rendererElement.RenderIt(renderer);
        }
    }

    /// <summary>
    /// Render table rows to plain text
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="tableRows">List of all rows to render</param>
    /// <param name="rowData"></param>
    public static void RenderRowsToPlain(ITextDocumentRenderer renderer, List<Row> tableRows,
        List<List<string>> rowData)
    {
        foreach (var row in tableRows)
        {
            var rendererElement = (RowPlainTextRendererElement)renderer.TextRendererElementFactory.CreateInstance(row);
            rendererElement.RenderToString(renderer, rowData);
        }
    }

    /// <summary>
    /// Render table cells to plain text
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="cells">List of all cells to render</param>
    /// <param name="row">List with all rows of the table to print</param>
    public static void RenderCellsToPlain(ITextDocumentRenderer renderer, List<Cell> cells, List<string> row)
    {
        foreach (var cell in cells)
        {
            var rendererElement =
                (CellPlainTextRendererElement)renderer.TextRendererElementFactory.CreateInstance(cell);
            rendererElement.RenderToString(renderer, row);
        }
    }

    /// <summary>
    /// Render table cells widths to RTF
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="cells">List of all cells to render</param>
    /// <param name="color"></param>
    public static void RenderCellsToRtf(ITextDocumentRenderer renderer, List<Cell> cells, Color color)
    {
        //foreach (var cell in cells)
        //{
        //    var rendererElement = (CellRtfTextRendererElement)renderer.TextRendererElementFactory.CreateInstance(cell);
        //    rendererElement.RenderToString(renderer);
        //}

        var colorIndex = renderer.Styleset.GetIndexOfColor(color) + 1;

        var sb = new StringBuilder();

        for (var index = 0; index < cells.Count; index++)
        {
            var cell = cells[index];
            //var twips = cell.Column.MaxLength * 200;
            renderer.Content.Append($@"\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs\clcbpat{colorIndex}\cellx{index + 1}000 ");

            var rendererElement = (CellRtfTextRendererElement)renderer.TextRendererElementFactory.CreateInstance(cell);
            rendererElement.RenderToString(renderer, sb);
        }

        renderer.Content.Append(sb);
    }

    /// <summary>
    /// Render table cells to RTF
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="cells">List of all cells to render</param>
    public static void RenderCellWidthsToRtf(ITextDocumentRenderer renderer, List<Cell> cells)
    {
        for (var index = 0; index < cells.Count; index++)
        {
            //var cell = cells[index];
            //var twips = cell.Column.MaxLength * 200;
            renderer.Content.Append($@"\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs \cellx{index + 1}000 ");
        }
    }

    /// <summary>
    /// Get the alignment for a datatype
    /// </summary>
    /// <param name="type">Datatype</param>
    /// <returns></returns>
    public static string GetAlignment(Type type)
    {
        // Right aligned
        if (type == typeof(double) || type == typeof(float) ||
            type == typeof(short) || type == typeof(int) ||
            type == typeof(long) || type == typeof(Int128) ||
            type == typeof(byte))
        {
            return "Right";
        }

        // Centered aligned
        if (type == typeof(bool) || type == typeof(DateTime))
        {
            return "Center";
        }

        // Default: left aligned
        return "Left";
    }

    /// <summary>
    /// Get the alignment for a datatype (Left returns string.Empty)
    /// </summary>
    /// <param name="type">Datatype</param>
    /// <returns></returns>
    public static string GetAlignment1(Type type)
    {
        // Right aligned
        if (type == typeof(double) || type == typeof(float) ||
            type == typeof(short) || type == typeof(int) ||
            type == typeof(long) || type == typeof(Int128) ||
            type == typeof(byte))
        {
            return "Right";
        }

        // Centered aligned
        if (type == typeof(bool) || type == typeof(DateTime))
        {
            return "Center";
        }

        // Default: left aligned
        return string.Empty;
    }

    /// <summary>
    /// Create an image in an RTF document
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="image">Curent image</param>
    /// <exception cref="NotSupportedException"></exception>
    public static void CreateImageRtf(ITextDocumentRenderer renderer, ImageBase image)
    {
        // Get the content of all inlines as string
        var sb = new StringBuilder();

        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle(image.StyleName);
        renderer.Content.Append(
            $@"\pard\plain\q{renderer.Styleset.GetIndexOfStyle(image.StyleName)}{RtfHelper.GetFormatSettings(style, renderer.Styleset)}");

        // Get max height and with for images in twips
        StylesetHelper.GetMaxWidthAndHeightInTwips(renderer.Styleset, out var maxWidth, out var maxHeight);

        StylesetHelper.GetWidthAndHeightInTwips(MeasurementHelper.GetTwipsFromPx(image.OriginalWidth),
            MeasurementHelper.GetTwipsFromPx(image.OriginalHeight), maxWidth, maxHeight, out var width, out var height);

        // Add the image
        var bytes = ImageHelper.GetBytes(image.Uri);

        var path = image.Uri.ToLowerInvariant();

        sb.Append(@"{{\*\shppict\pict");

        if (path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
        {
            sb.Append("\\jpegblip");
        }
        else if (path.EndsWith(".png"))
        {
            sb.Append("\\pngblip");
        }
        else
        {
            throw new NotSupportedException("Unsupported image format. Use JPEG or PNG images!");
        }

        sb.Append($@"\picw{width}\pich{height}\picwgoal{width}\pichgoal{height}\bin{{");

        var str = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
        sb.Append(str);
        sb.Append("}}{\\line{");

        // Debug.Print(sb.ToString());

        var childs = new List<Inline>();

        if (!string.IsNullOrEmpty(image.CurrentPrefix))
        {
            childs.Add(new Span(image.CurrentPrefix));
        }

        childs.AddRange(image.ChildInlines);

        RenderInlineChildsToRtf(renderer, sb, childs);

        renderer.Content.Append($@"{{\*\bkmkstart {image.TagName}}}{{{sb}}}{{\*\bkmkend {image.TagName}}}}}");

        renderer.Content.Append($"}}\\par}}{Environment.NewLine}");
    }

    /// <summary>
    /// Create an image in an HTML document
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="image">Current image</param>
    /// <param name="tagToUse">Tag to use</param>
    /// <param name="localCss">Local CSS class name</param>
    /// <param name="className">Class name</param>
    public static void CreateImageHtml(ITextDocumentRenderer renderer, ImageBase image, string tagToUse,
        string localCss, string className)
    {
        // Get the content of all inlines as string
        var sb = new StringBuilder();

        if (string.IsNullOrEmpty(localCss))
        {
            renderer.Content.Append($"<{tagToUse} class=\"{className}\">");
        }
        else
        {
            renderer.Content.Append($"<{tagToUse} class=\"{className}\" style=\"{localCss}\">");
        }

        var childs = new List<Inline>();

        if (!string.IsNullOrEmpty(image.CurrentPrefix))
        {
            renderer.Content.Append($"<a name=\"{image.TagName}\" />");
            childs.Add(new Span(image.CurrentPrefix));
        }

        childs.AddRange(image.ChildInlines);

        RenderInlineChildsToHtml(renderer, sb, childs);

        // Get max height and with for images in twips
        StylesetHelper.GetMaxWidthAndHeightInTwips(renderer.Styleset, out var maxWidth, out var maxHeight);

        StylesetHelper.GetWidthAndHeightInTwips(MeasurementHelper.GetTwipsFromPx(image.OriginalWidth),
            MeasurementHelper.GetTwipsFromPx(image.OriginalHeight), maxWidth, maxHeight, out var width, out var height);

        renderer.Content.Append(
            $"<img src=\"{image.Uri}\" alt=\"{sb}\" width=\"{MeasurementHelper.GetPxFromTwips(width)}px\" height=\"{MeasurementHelper.GetPxFromTwips(height)}px\"/><br/>");
        renderer.Content.Append(sb);
        renderer.Content.Append($"</{tagToUse}>{Environment.NewLine}");
    }

    /// <summary>
    /// Create an image in plain text
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="image">Current image</param>
    public static void CreateImagePlainText(ITextDocumentRenderer renderer, ImageBase image)
    {
        var sb = new StringBuilder();
        sb.Append(image.CurrentPrefix);
        var childs = new List<Inline>();

        if (!string.IsNullOrEmpty(image.CurrentPrefix))
        {
            childs.Add(new Span(image.CurrentPrefix));
        }

        childs.AddRange(image.ChildInlines);

        RenderInlineChildsToPlainText(renderer, sb, childs, tag: string.Empty, isBlock: true);
        renderer.Content.Append($"![{sb}]({image.Uri} \"{sb}\"){Environment.NewLine}{Environment.NewLine}");
    }
}
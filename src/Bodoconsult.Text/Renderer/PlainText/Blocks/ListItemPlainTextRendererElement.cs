// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Text;
using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.PlainText;

/// <summary>
/// Text rendering element for <see cref="ListItem"/> instances
/// </summary>
public class ListItemPlainTextRendererElement : ParagraphBasePlainTextRendererElement
{

    private readonly ListItem _listItem;

    /// <summary>
    /// Default ctor
    /// </summary>
    public ListItemPlainTextRendererElement(ListItem listItem)
    {
        Paragraph = listItem;
        _listItem = listItem;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {
        // Get the content of all inlines as string
        string listChars = " ";

        var list = (List)_listItem.Parent;

        switch (list.ListStyleType)
        {
            case ListStyleTypeEnum.Disc:
                listChars = "*";
                break;
            case ListStyleTypeEnum.Circle:
                listChars = "o";
                break;
            case ListStyleTypeEnum.Square:
                listChars = "-";
                break;
            case ListStyleTypeEnum.Decimal:
                list.Counter++;
                listChars = $"{list.Counter}";
                break;
            case ListStyleTypeEnum.DecimalLeadingZero:
                list.Counter++;
                listChars = $"{list.Counter:00}";
                break;
            case ListStyleTypeEnum.UpperRoman:
                list.Counter++;
                listChars = $"{list.Counter.ArabicToRoman().ToUpperInvariant()}";
                break;
            case ListStyleTypeEnum.LowerRoman:
                list.Counter++;
                listChars = $"{list.Counter.ArabicToRoman().ToLowerInvariant()}";
                break;
            case ListStyleTypeEnum.UpperLatin:
                // ToDo: Get latin letter
                list.Counter++;
                listChars = $"{list.Counter}";
                break;
            case ListStyleTypeEnum.LowerLatin:
                // ToDo: Get latin letter
                list.Counter++;
                listChars = $"{list.Counter}";
                break;
            case ListStyleTypeEnum.Customized:
                listChars = list.ListStyleTypeChar.ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var sb = new StringBuilder();

        DocumentRendererHelper.RenderInlineChildsToPlainText(renderer, sb, Paragraph.ChildInlines, string.Empty);

        // Now let the formatter work
        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle($"{Paragraph.GetType().Name}Style");
        var formatter = new PlainTextParagraphFormatter(sb.ToString(), style, renderer.PageStyleBase)
        {
            LeftRightBorderChar = LeftRightBorderChar,
            TopBottomBorderChar = TopBottomBorderChar,
            ListChars = listChars,
        };
        formatter.FormatText();

        // Now add the formatted text to the rendered content
        renderer.Content.Append($"{formatter.GetFormattedText()}");
    }
}
//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using Bodoconsult.Text.Extensions;

//namespace Bodoconsult.Text.Documents;

///// <summary>
///// Parse a given string to list of inline elements
///// </summary>
//public class ContentParser
//{
//    private string _content;
//    private Block _block;

//    private string[] StartTags = new[] { "Bold", "Italic", "Span" };


//    /// <summary>
//    /// Default ctor
//    /// </summary>
//    /// <param name="content">Content to parse</param>
//    /// <param name="block"></param>
//    public ContentParser(string content, Block block)
//    {
//        _content = ClearContent(content);
//        _block = block;

//    }

//    private string ClearContent(string content)
//    {
//        return content.Replace("<b>", "<Bold>", StringComparison.CurrentCultureIgnoreCase)
//            .Replace("<s>", "<Span>", StringComparison.CurrentCultureIgnoreCase)
//            .Replace("<i>", "<Italic>", StringComparison.CurrentCultureIgnoreCase)
//            .Replace("</b>", "</Bold>", StringComparison.CurrentCultureIgnoreCase)
//            .Replace("</s>", "</Span>", StringComparison.CurrentCultureIgnoreCase)
//            .Replace("</i>", "</Italic>", StringComparison.CurrentCultureIgnoreCase)
//            .Replace("<br>", "<LineBreak/>", StringComparison.CurrentCultureIgnoreCase)
//            .Trim();
//    }

//    /// <summary>
//    /// Parse the content
//    /// </summary>
//    /// <exception cref="System.NotImplementedException"></exception>
//    public void Parse()
//    {
//        if (string.IsNullOrEmpty(_content))
//        {
//            return;
//        }

//        var i = _content.IndexOf("<", StringComparison.OrdinalIgnoreCase);
//        var j = _content.IndexOf(">", StringComparison.OrdinalIgnoreCase);

//        if (i < 0 || j < 0)
//        {
//            _block.AddInline(new Span(_content));
//            return;
//        }

//        // Check start
//        var start = 0;
//        var tag = _content.Substring(i + 1, j - i - 1).FirstCharToUpperCase();

//        if (StartTags.Contains(tag))
//        {
//            if (i > 0)
//            {
//                _content = $"<Span>{_content.Substring(start, i)}</Span>{_content[i..]}";
//            }
//        }

//        // Check end
//        var a = _content.LastIndexOf("<", StringComparison.OrdinalIgnoreCase);
//        var e = _content.LastIndexOf(">", StringComparison.OrdinalIgnoreCase);

//        if (i == a || j == e)
//        {
//            _content += $"</{tag}>";
//        }
//        else
//        {
//            tag = _content.Substring(a + 1, e - a - 1).FirstCharToUpperCase();

//            if (StartTags.Contains(tag))
//            {
//                if (a > 0)
//                {
//                    _content += $"</{tag}>";
//                }
//            }
//            // End tag
//            else if (StartTags.Contains(tag.Replace("/", string.Empty).FirstCharToUpperCase()))
//            {
//                if (e < _content.Length - 1)
//                {
//                    _content = $"{_content.Substring(0, e+1)}<Span>{_content.Substring(e+1, _content.Length - e - 1)}</Span>";
//                }
//            }
//            {

//            }
//        }

//        var reader = new LdmlReader($"<Paragraph>{_content}</Paragraph>");

//        reader.ParseLdml();

//        var p = (Paragraph)reader.TextElement;

//        foreach (var inline in p.ChildInlines)
//        {
//            _block.AddInline(inline);
//        }
//    }

//    private void ChooseInline(List<Inline> inlines, string tag, string content)
//    {
//        Debug.Print($"{tag}:{content}");

//        switch (tag)
//        {
//            case "s":
//                inlines.Add(new Span(content));
//                break;
//            case "b":
//                inlines.Add(new Bold(content));
//                break;
//            case "i":
//                inlines.Add(new Italic(content));
//                break;
//        }
//    }
//}
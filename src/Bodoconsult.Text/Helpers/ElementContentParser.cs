// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Linq;
using Bodoconsult.App.Extensions;
using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Helpers;

/// <summary>
/// Parse a given string to list of inline elements
/// </summary>
public static class ElementContentParser
{
    private static readonly string[] StartTags = ["Bold", "Italic", "Span"];

    /// <summary>
    /// Clear a string from certain tags
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string ClearContent(string content)
    {
        return content
            // Replace tags first
            .Replace("<b>", "??ab??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<s>", "??as??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<i>", "??ai??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<h>", "??ah??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("</b>", "??eb??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("</s>", "??es??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("</i>", "??ei??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<br>", "??lb??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<br/>", "??lb", StringComparison.CurrentCultureIgnoreCase)
            .Replace("</h>", "??eh??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<Bold>", "??ab??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<Span>", "??as??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<Italic>", "??ai??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("<Hyperlink>", "??ah??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("</Bold>", "??eb??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("</Span>", "??es??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("</Italic>", "??ei??", StringComparison.CurrentCultureIgnoreCase)
            .Replace("</Hyperlink>", "??eh", StringComparison.CurrentCultureIgnoreCase)
            
            // Escape the string now
            .Replace("<", "&lt;", StringComparison.OrdinalIgnoreCase)
            .Replace(">", "&gt;", StringComparison.OrdinalIgnoreCase)
            .Replace("\"", "&quot;", StringComparison.OrdinalIgnoreCase)
            .Replace("'", "&apos;", StringComparison.OrdinalIgnoreCase)
            //.Replace("&", "&amp;", StringComparison.OrdinalIgnoreCase)

            // Now restore tags
            .Replace("??eb??", "</Bold>", StringComparison.CurrentCultureIgnoreCase)
            .Replace("??es??", "</Span>", StringComparison.CurrentCultureIgnoreCase)
            .Replace("??ei??", "</Italic>", StringComparison.CurrentCultureIgnoreCase)
            .Replace("??eh", "</Hyperlink>", StringComparison.CurrentCultureIgnoreCase)
            .Replace("??ab??", "<Bold>", StringComparison.CurrentCultureIgnoreCase)
            .Replace("??as??", "<Span>", StringComparison.CurrentCultureIgnoreCase)
            .Replace("??ai??", "<Italic>", StringComparison.CurrentCultureIgnoreCase)
            .Replace("??ah", "<Hyperlink>", StringComparison.CurrentCultureIgnoreCase)
            .Replace("??lb??", "<LineBreak/>", StringComparison.CurrentCultureIgnoreCase)
            .Trim();
    }

    /// <summary>
    /// Parse the content
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public static void Parse(string content, Block block)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        content = ClearContent(content);

        var i = content.IndexOf("<", StringComparison.OrdinalIgnoreCase);
        var j = content.IndexOf(">", StringComparison.OrdinalIgnoreCase);

        if (i < 0 || j < 0)
        {
            block.AddInline(new Span(content));
            return;
        }

        ////"   &quot;
        ////'   &apos;
        ////    < &lt;
        ////    > &gt;
        ////    &&amp;

        //if (i > j)
        //{
        //    content = $"{content.Substring(0, i)}&lt;{content.Substring(i+1)}";
        //}

        // Check start
        var start = 0;
        var tag = content.Substring(i + 1, j - i - 1).FirstCharToUpperCase();

        if (StartTags.Contains(tag))
        {
            if (i > 0)
            {
                content = $"<Span>{content.Substring(start, i)}</Span>{content[i..]}";
            }
        }

        // Check end
        var a = content.LastIndexOf("<", StringComparison.OrdinalIgnoreCase);
        var e = content.LastIndexOf(">", StringComparison.OrdinalIgnoreCase);

        if (i == a || j == e)
        {
            content += $"</{tag}>";
        }
        else
        {
            tag = content.Substring(a + 1, e - a - 1).FirstCharToUpperCase();

            if (StartTags.Contains(tag))
            {
                if (a > 0)
                {
                    content += $"</{tag}>";
                }
            }
            // End tag
            else if (StartTags.Contains(tag.Replace("/", string.Empty).FirstCharToUpperCase()))
            {
                if (e < content.Length - 1)
                {
                    content = $"{content[..(e + 1)]}<Span>{content.Substring(e + 1, content.Length - e - 1)}</Span>";
                }
            }
            {

            }
        }

        var reader = new LdmlReader($"<Paragraph>{content}</Paragraph>");

        reader.ParseLdml();

        var p = (Paragraph)reader.TextElement;

        foreach (var inline in p.ChildInlines)
        {
            block.AddInline(inline);
        }
    }
}
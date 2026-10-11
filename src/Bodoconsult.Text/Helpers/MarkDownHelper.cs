// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.Net;

namespace Bodoconsult.Text.Helpers;

/// <summary>
/// Helping functionality for Markdown
/// </summary>
public class MarkdownHelper
{
    ///// <summary>
    ///// Parse Markdown and replace links 
    ///// </summary>
    ///// <param name="html"></param>
    ///// <returns></returns>
    //public static string ParseMarkdown(string html)
    //{
    //    ////return html;

    //    //if (!html.Contains("[", StringComparison.OrdinalIgnoreCase) || !html.Contains("](", StringComparison.OrdinalIgnoreCase)) return html;

    //    //var i = html.IndexOf('[', StringComparison.Ordinal);
    //    //var j = html.IndexOf("](", i + 1, StringComparison.Ordinal);
    //    //var k = html.IndexOf("]", i + 1, StringComparison.Ordinal);
    //    //var e = html.IndexOf(")", j + 1, StringComparison.Ordinal);

    //    //while (i >= 0 && j >= 0)
    //    //{

    //    //    if (k == j)
    //    //    {
    //    //        var pos1 = i;
    //    //        var name = html.Substring(i + 1, j - i - 1);

    //    //        var url = html.Substring(j + 2, e - j - 2);

    //    //        Debug.Print(url);
    //    //        Debug.Print(name);

    //    //        var link = $"<a href=\"{url}\" alt=\"{name}\">{name}</a>";

    //    //        html = html[..pos1] + link + html.Substring(e + 1, html.Length - e - 1);
    //    //    }
    //    //    else
    //    //    {
    //    //        i = i + 1;
    //    //    }



    //    //    i = html.IndexOf("[", i + 1, StringComparison.Ordinal);
    //    //    j = html.IndexOf("](", i + 1, StringComparison.Ordinal);
    //    //    k = html.IndexOf("]", i + 1, StringComparison.Ordinal);
    //    //    e = html.IndexOf(")", i + 1, StringComparison.Ordinal);
    //    //}

    //    return html;
    //}

    /// <summary>
    /// Get the content prepared for Markdown 
    /// </summary>
    /// <param name="content">raw content</param>
    /// <returns>encoded and resolved content (links, ...)</returns>

    public static string GetContentAsMarkdown(string content)
    {
        return string.IsNullOrEmpty(content)
            ? "&nbsp;" : WebUtility.HtmlEncode(content.Replace("<para>", "??pa??")
                    .Replace("</para>", "??pe??")).Replace("\r\n", "<br />")
                .Replace("\t", "&nbsp;&nbsp;&nbsp;").Replace("??br??", "<br />");
    }

    /// <summary>
    /// Create content as paragraph with one or more Markdown-p-tags
    /// </summary>
    /// <param name="content">raw content</param>
    /// <param name="className">CSS class name</param>
    /// <returns>encoded and resolved content (links, ...)</returns>
    public static string GetContentAsMarkdownParagraph(string content, string className = null)
    {
        content = GetContentAsMarkdown(content).Replace("??empty??", "&nbsp;");

        var startP = string.Empty;

        if (content.Contains("??pa??"))
        {
            content = content.Replace("??pa??", startP, StringComparison.OrdinalIgnoreCase)
                .Replace("??pe??", "</p>", StringComparison.OrdinalIgnoreCase)
                .Replace("</p>\r\n", "</p>", StringComparison.OrdinalIgnoreCase)
                .Replace("</p>", "\r\n\r\n", StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            content = $"{startP}{content}\r\n\r\n";
        }

        return content;
    }

    /// <summary>
    /// Create content as paragraph with one or more Markdown-p-tags for a defintion list
    /// </summary>
    /// <param name="content">raw content</param>
    /// <param name="className">CSS class name</param>
    /// <returns>encoded and resolved content (links, ...)</returns>
    public static string GetContentAsMarkdownParagraphForDefinitionList(string content, string className = null)
    {
        content = GetContentAsMarkdown(content).Replace("??empty??", "&nbsp;");

        const string startP = ": ";

        if (content.Contains("??pa??"))
        {
            content = content.Replace("??pa??", startP, StringComparison.OrdinalIgnoreCase)
                .Replace("??pe??", "</p>", StringComparison.OrdinalIgnoreCase)
                .Replace("</p>\r\n", "</p>", StringComparison.OrdinalIgnoreCase)
                .Replace("</p>", "\r\n", StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            content = $"{startP}{content}\r\n";
        }

        return content;
    }
}
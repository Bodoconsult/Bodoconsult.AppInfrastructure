// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Diagnostics;
using System.Net;

namespace Bodoconsult.Text.Helpers;

/// <summary>
/// Helping functionality for HTML
/// </summary>
public class HtmlHelper
{

    /// <summary>
    /// Parse HTML and replace links 
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string ParseHtml(string html)
    {
        //return html;

        if (!html.Contains('[') || !html.Contains("](")) return html;

        var i = html.IndexOf('[');
        var j = html.IndexOf("](", i + 1, StringComparison.Ordinal);
        var k = html.IndexOf(']', i + 1);
        var e = html.IndexOf(')', j + 1);

        while (i >= 0 && j >= 0)
        {

            if (k == j)
            {
                var pos1 = i;
                var name = html.Substring(i + 1, j - i - 1);

                var url = html.Substring(j + 2, e - j - 2);

                Debug.Print(url);
                Debug.Print(name);

                var link = $"<a href=\"{url}\" alt=\"{name}\">{name}</a>";

                html = html[..pos1] + link + html[(e + 1)..];
            }
            else
            {
                i = i + 1;
            }



            i = html.IndexOf("[", i + 1, StringComparison.Ordinal);
            j = html.IndexOf("](", i + 1, StringComparison.Ordinal);
            k = html.IndexOf("]", i + 1, StringComparison.Ordinal);
            e = html.IndexOf(")", i + 1, StringComparison.Ordinal);
        }

        return html;
    }


    /// <summary>
    /// Get the content prepared for HTML 
    /// </summary>
    /// <param name="content">raw content</param>
    /// <returns>encoded and resolved content (links, ...)</returns>

    public static string GetContentAsHtml(string content)
    {
        return string.IsNullOrEmpty(content)
            ? "&nbsp;": ParseHtml(WebUtility.HtmlEncode(content.Replace("<para>", "??pa??")
                    .Replace("</para>", "??pe??")).Replace("\r\n", "<br />")
                .Replace("\t", "&nbsp;&nbsp;&nbsp;").Replace("??br??", "<br />"));
    }


    /// <summary>
    /// Create content as paragraph with one or more HTML-p-tags
    /// </summary>
    /// <param name="content">raw content</param>
    /// <param name="className">CSS class name</param>
    /// <returns>encoded and resolved content (links, ...)</returns>
    public static string GetContentAsHtmlParagraph(string content, string className = null)
    {
        content = GetContentAsHtml(content).Replace("??empty??", "&nbsp;");

        var startP = string.IsNullOrEmpty(className) ? "<p>" : $"<p{className}>";

        if (content.Contains("??pa??"))
        {
            content = content.Replace("??pa??", startP)
                .Replace("??pe??", "</p>")
                .Replace("</p>\r\n", "</p>");
        }
        else
        {
            content = $"{startP}{content.Replace("\r\n", "</p><p>")}</p>";
        }

        return content;
    }
}
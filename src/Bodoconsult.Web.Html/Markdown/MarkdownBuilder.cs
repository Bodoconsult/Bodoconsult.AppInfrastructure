// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using System.Text;
using Bodoconsult.Web.Html.Extensions;

namespace Bodoconsult.Web.Html.Markdown;

/// <summary>
/// Build a markdown file
/// </summary>
public class MarkdownBuilder
{
    /// <summary>
    /// Internal field for the Markdown content. Should not be accessible from outside directly
    /// </summary>
    private readonly StringBuilder _builder = new();

    /// <summary>
    /// 
    /// </summary>
    private readonly string _tagEnd = $"{Environment.NewLine}{Environment.NewLine}";

    /// <summary>
    ///  Get the current Markdown as string
    /// </summary>
    /// <returns>Markdown as string</returns>
    public string GetContent()
    {
        return _builder.ToString();
    }

    /// <summary>
    /// Save the amrkdown as UTF-8 file
    /// </summary>
    /// <param name="filePath">Path to save the file in</param>
    /// <exception cref="ArgumentException">Thrown if filePath is null or empty</exception>
    public void Save(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("filePath is null or empty");
        }

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        File.WriteAllText(filePath, _builder.ToString(), Encoding.UTF8);
    }

    /// <summary>
    /// Clear the markdown content
    /// </summary>
    public void Clear()
    {
        _builder.Clear();
    }

    /// <summary>
    /// Add a headline level 1
    /// </summary>
    /// <param name="content">Headline content</param>
    public void AddHeadline1(string content)
    {
        _builder.Append($"# {content.EscapeChars()}{_tagEnd}");
    }

    /// <summary>
    /// Add a headline level 2
    /// </summary>
    /// <param name="content">Headline content</param>
    public void AddHeadline2(string content)
    {
        _builder.Append($"## {content.EscapeChars()}{_tagEnd}");
    }

    /// <summary>
    /// Add a headline level 3
    /// </summary>
    /// <param name="content">Headline content</param>
    public void AddHeadline3(string content)
    {
        _builder.Append($"### {content.EscapeChars()}{_tagEnd}");
    }

    /// <summary>
    /// Add a headline level 4
    /// </summary>
    /// <param name="content">Headline content</param>
    public void AddHeadline4(string content)
    {
        _builder.Append($"#### {content.EscapeChars()}{_tagEnd}");
    }

    /// <summary>
    /// Add a headline level 5
    /// </summary>
    /// <param name="content">Headline content</param>
    public void AddHeadline5(string content)
    {
        _builder.Append($"##### {content.EscapeChars()}{_tagEnd}");
    }

    /// <summary>
    ///Add a paragraph
    /// </summary>
    /// <param name="content">Paragraph content</param>

    public void AddParagraph(string content)
    {
        _builder.Append($"{content.EscapeChars()}{_tagEnd}");
    }


    /// <summary>
    ///Add a code paragraph
    /// </summary>
    /// <param name="content">Code paragraph content</param>

    public void AddCode(string content)
    {
        _builder.Append($"```{_tagEnd}{content.EscapeChars()}{_tagEnd}```{_tagEnd}");
    }

    ///  <summary>
    /// Add a code paragraph for a certain development language
    ///  </summary>
    ///  <param name="content">Code paragraph content</param>
    ///  <param name="type">Code type: csharp, xml, sql, json, ...</param>
    public void AddCode(string content, string type)
    {
        _builder.Append($"``` {type}{_tagEnd}{content.EscapeChars()}{_tagEnd}```{_tagEnd}");
    }

    /// <summary>
    /// Add List
    /// </summary>
    /// <param name="content">List content</param>

    public void AddList(string content)
    {
        _builder.Append($"-{content.EscapeChars()}{_tagEnd}");
    }

    /// <summary>
    /// Add raw markdown formatted content
    /// </summary>
    /// <param name="markdown">Markdown formatted content</param>
    public void AddRawMarkdown(string markdown)
    {
        _builder.Append(markdown);
    }

    /// <summary>
    /// Add Bold
    /// </summary>
    /// <param name="content">List content</param>
    public static string AddBold(string content)
    {
        return $"**{content.EscapeChars()}**";
    }

    /// <summary>
    /// Add Italic
    /// </summary>
    /// <param name="content">List content</param>
    public static string AddItalic(string content)
    {
        return $"*{content.EscapeChars()}*";
    }

    /// <summary>
    /// Add Italic Bold
    /// </summary>
    /// <param name="content">List content</param>
    public static string AddItalicBold(string content)
    {
        return $"***{content.EscapeChars()}***";
    }

    /// <summary>
    /// Add Url
    /// </summary>
    /// <param name="url">Hyperlink</param>

    public static string AddUrl(string url)
    {
        return $"[{url}]({url})";
    }

    /// <summary>
    /// Add Url
    /// </summary>
    /// <param name="url">Hyperlink</param>
    /// <param name="description"></param>
    public static string AddUrl(string url, string description)
    {
        return $"[{description.EscapeChars()}]({url})";
    }

    /// <summary>
    /// Add UrlImage
    /// </summary>
    /// <param name="url">Hyperlink</param>

    public static string AddUrlImage(string url)
    {
        return $"![{url}]({url})";
    }

    /// <summary>
    /// Add UrlImageDescription
    /// </summary>
    /// <param name="url">Hyperlink</param>
    /// <param name="description"></param>
    public static string AddUrlImageDescription(string url, string description)
    {
        return $"![{description.EscapeChars()}]({url})";
    }
}
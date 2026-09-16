// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.Web.Html.Extensions;

/// <summary>
/// Helper class for markdown extensions
/// </summary>
public static class MarkdownExtensions
{
    /// <summary>
    ///  Escape a string for markdown
    /// </summary>
    /// <param name="value">String to escape</param>
    /// <returns>Escaped string</returns>
    public static string EscapeChars(this string value)
    {
        return value.Replace("*", "\\*").Replace("\\", "\\\\").Replace("`", "\\`").Replace("_", "\\_")
            .Replace("{", "\\{").Replace("}", "\\}")
            .Replace("[", "\\[").Replace("]", "\\]").Replace("<", "\\<").Replace(">", "\\>").Replace("(", "\\(")
            .Replace(")", "\\)")
            .Replace(".", "\\.").Replace("+", "\\+").Replace("-", "\\-").Replace("#", "\\#").Replace("!", "\\!")
            .Replace("|", "\\|");
    }
}
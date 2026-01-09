// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Bodoconsult.App.Extensions;

/// <summary>
/// Extension methods for string class
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Replace [0xXXX] tags in a string with a hex char. [0x1\ will be replaced with \u
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ReplaceHexTag(this string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        if (!data.Contains("[0x"))
        {
            return data;
        }


        var i = data.IndexOf("[0x", StringComparison.OrdinalIgnoreCase);

        var j = data.IndexOf("]", i + 1, StringComparison.OrdinalIgnoreCase);

        while (i > -1)
        {
            var hex = $"0x{data.Substring(i + 3, j - i - 3).ToUpperInvariant()}";

            var hexInt = Convert.ToInt32(hex, 16);

            data = data[..i]
                   + (char)hexInt + data[(j + 1)..];


            i = data.IndexOf("[0x", i, StringComparison.OrdinalIgnoreCase);
            if (i < 0)
            {
                break;
            }

            j = data.IndexOf("]", i + 1, StringComparison.OrdinalIgnoreCase);

        }

        return data;
    }

    /// <summary>
    /// String value as SQL string
    /// </summary>
    /// <param name="value">String value to format as SQL</param>
    /// <returns>Value as SQL string formatted</returns>
    public static string AsSqlString(this string value)
    {
        return string.IsNullOrEmpty(value) ? "null" : $"'{value}'";
    }

    /// <summary>
    /// Make first char of a string a lowercase char
    /// </summary>
    /// <param name="value">String</param>
    /// <returns>String with first char being lowercase</returns>
    public static string FirstCharToLowerCase(this string value)
    {
        if (!string.IsNullOrEmpty(value) && char.IsUpper(value[0]))
        {
            return value.Length == 1 ?
                char.ToLower(value[0]).ToString() :
                char.ToLower(value[0]) + value[1..];
        }

        return value;
    }

    /// <summary>
    /// Make first char of a string an uppercase char. The rest is lowercase
    /// </summary>
    /// <param name="value">String</param>
    /// <returns>String with first char being uppercase. The rest is lowercase</returns>
    public static string FirstCharToUpperCase(this string value)
    {
        return value.Length == 1 ?
            char.ToUpper(value[0]).ToString() :
            char.ToUpper(value[0]) + value[1..].ToLowerInvariant();
    }

    /// <summary>
    ///  Limit the length of a string to a certain value
    /// </summary>
    /// <param name="value">String</param>
    /// <param name="length">Allowed maximum length</param>
    /// <returns>Truncated string</returns>
    public static string LimitToLength(this string value, int length)
    {
        if (value == null)
        {
            return null;
        }

        value = value.TrimEnd();

        if (string.IsNullOrEmpty(value) || value.Length <= length)
        {
            return value;
        }

        return value[..length];
    }

    /// <summary>
    /// Get a string froma secure string
    /// </summary>
    /// <param name="value">Secure string</param>
    /// <returns>Clear text string</returns>
    public static string SecureStringToString(this SecureString value)
    {
        var ptr = Marshal.SecureStringToGlobalAllocUnicode(value);
        try
        {
            return Marshal.PtrToStringUni(ptr);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(ptr);
        }
    }

    /// <summary>
    /// Repeat a string
    /// </summary>
    /// <param name="text">String to repeat</param>
    /// <param name="n">Number of repetitions</param>
    /// <returns></returns>
    public static string Repeat(this string text, uint n)
    {
        try
        {
            var textAsSpan = text.AsSpan();
            var span = new Span<char>(new char[textAsSpan.Length * (int)n]);
            for (var i = 0; i < n; i++)
            {
                textAsSpan.CopyTo(span.Slice(i * textAsSpan.Length, textAsSpan.Length));
            }

            return span.ToString();
        }
        catch (Exception e)
        {
            Debug.Print($"{n} {e.Message}");
            return text;
        }
    }

    /// <summary>
    /// Repeat a string
    /// </summary>
    /// <param name="text">String to repeat</param>
    /// <param name="n">Number of repetitions</param>
    /// <returns></returns>
    public static string Repeat(this string text, int n)
    {
        try
        {
            var textAsSpan = text.AsSpan();
            var span = new Span<char>(new char[textAsSpan.Length * n]);
            for (var i = 0; i < n; i++)
            {
                textAsSpan.CopyTo(span.Slice(i * textAsSpan.Length, textAsSpan.Length));
            }

            return span.ToString();
        }
        catch (Exception e)
        {
            Debug.Print($"{n} {e.Message}");
            return text;
        }
    }

    /// <summary>
    /// Count the number of spaces in a string
    /// </summary>
    /// <param name="value">String</param>
    /// <returns>Number of spaces in a string</returns>
    public static int SpaceCount(this string value)
    {
        var count = 0;

        if (string.IsNullOrEmpty(value))
        {
            return count;
        }

        var mem = value.AsMemory();
        for (var i = 0; i < mem.Length; i++)
        {
            if (mem.Slice(i, 1).Span[0].Equals(' '))
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Convert a string to a Guid. Invalid input strings end up in a Guid.Empty
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Guid or Guid.Empty</returns>
    public static Guid ToGuid(this string input)
    {
        return Guid.TryParse(input, out var guid) ? guid : Guid.Empty;
    }

    /// <summary>
    /// Escape a string for using it as an XML value
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Escaped string</returns>
    public static string EscapeForXml(this string input)
    {
        return SecurityElement.Escape(input);
    }
}
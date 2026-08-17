// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Extensions;
using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.PlainText;

/// <summary>
/// Paragraph formatter for plain text rendering
/// </summary>
public class PlainTextParagraphFormatter
{

    private string _leftMargin;
    private readonly string _content;
    private readonly ParagraphStyleBase _paragraphStyle;
    private readonly PageStyleBase _pageStyle;
    private readonly ReadOnlyMemory<char> _bytes;
    private int _maxLength;

    private string _leftBorderChar;
    private string _rightBorderChar;

    private string _leftPadding;
    private string _rightPadding;
    private string _topBottomLine;

    /// <summary>
    /// Char used for left and right borders
    /// </summary>
    public string LeftRightBorderChar { get; set; } = "|";

    /// <summary>
    /// Char used for top and bottom borders
    /// </summary>
    public string TopBottomBorderChar { get; set; } = "-";

    /// <summary>
    /// Lines produced from content
    /// </summary>
    public List<string> Lines { get; } = [];

    /// <summary>
    /// Page width in number of chars. Public only for tests
    /// </summary>
    public int WidthInChars { get; set; }

    /// <summary>
    /// List chars
    /// </summary>
    public string ListChars { get; set; }

    /// <summary>
    /// Char width in cm
    /// </summary>
    private const double CharWidth = 0.20;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="content">Content to format</param>
    /// <param name="paragraphStyle">Paragraph style</param>
    /// <param name="pageStyle">Document or section page style</param>
    public PlainTextParagraphFormatter(string content, ParagraphStyleBase paragraphStyle, PageStyleBase pageStyle)
    {
        _content = content;
        _bytes = _content.AsMemory();

        _paragraphStyle = paragraphStyle;
        _pageStyle = pageStyle;
    }

    /// <summary>
    /// Format the text
    /// </summary>
    public void FormatText()
    {
        CalculateValues();

        GetLines();

        FormatLines();
    }

    /// <summary>
    /// Calculate all values required for formatting
    /// </summary>
    public void CalculateValues()
    {
        WidthInChars = (int)((_pageStyle.TypeAreaWidth - MeasurementHelper.GetCmFromPt(_paragraphStyle.Margins.Left) - MeasurementHelper.GetCmFromPt(_paragraphStyle.Margins.Right)) / CharWidth);


        var marginLeft = (uint)((MeasurementHelper.GetCmFromPt(_paragraphStyle.Margins.Left)) / CharWidth);

        _leftMargin = marginLeft == 0 ? string.Empty : " ".Repeat(marginLeft);


        // Check if left border is needed
        if (_paragraphStyle.BorderThickness.Left > 0)
        {
            _leftBorderChar = LeftRightBorderChar;
            WidthInChars -= 1;
        }
        else
        {
            _leftBorderChar = string.Empty;
        }

        // Check if right border is needed
        if (_paragraphStyle.BorderThickness.Left > 0)
        {
            _rightBorderChar = LeftRightBorderChar;
            WidthInChars -= 1;
        }
        else
        {
            _rightBorderChar = string.Empty;
        }

        // Check if left padding is needed
        if (_paragraphStyle.Paddings.Left > 0)
        {
            _leftPadding = " ";
            WidthInChars -= 1;
        }
        else
        {
            _leftPadding = string.Empty;
        }

        // Check if right padding is needed
        if (_paragraphStyle.Paddings.Right > 0)
        {
            _rightPadding = " ";
            WidthInChars -= 1;
        }
        else
        {
            _rightPadding = string.Empty;
        }

        // Check if top border and/or bottom border is needed
        if (_paragraphStyle.BorderThickness.Top > 0 || _paragraphStyle.BorderThickness.Bottom > 0)
        {
            _topBottomLine = TopBottomBorderChar.Repeat(WidthInChars +
                                                                    2 * LeftRightBorderChar.Length +
                                                                    _leftPadding.Length +
                                                                    _rightPadding.Length);
        }
        else
        {
            _topBottomLine = string.Empty;
        }
    }

    private void FormatLines()
    {
        var result = new List<string>();

        AddMargin(result, _paragraphStyle.Margins.Top);

        if (_paragraphStyle.BorderThickness.Top > 0)
        {
            result.Add(_leftMargin + _topBottomLine);
        }

        var firstIndex = 0;
        var isFirstLine = false;

        foreach (var line in Lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                if (firstIndex == 0)
                {
                    isFirstLine = true;
                    firstIndex = 99;
                }
                else
                {
                    isFirstLine = false;
                }
            }

            FormatLine(result, line, isFirstLine);
        }

        if (_paragraphStyle.BorderThickness.Bottom > 0)
        {
            result.Add(_leftMargin + _topBottomLine);
        }

        AddMargin(result, _paragraphStyle.Margins.Bottom);

        Lines.Clear();
        Lines.AddRange(result);
    }

    private void AddMargin(List<string> result, double thickness)
    {
        if (thickness <= 0)
        {
            return;
        }

        var lineNumber = (int)Math.Ceiling(thickness / _paragraphStyle.FontSize);

        for (var i = 0; i < lineNumber; i++)
        {
            result.Add("");
        }
    }

    private void FormatLine(List<string> result, string line, bool isFirstLine)
    {

        var missinglength = WidthInChars - line.Length;

        if (string.IsNullOrEmpty(line))
        {
            result.Add(line);
            return;
        }

        string fLine;

        switch (_paragraphStyle.TextAlignment)
        {
            case TypoTextAlignment.Right:
                fLine = $"{_leftMargin}{_leftBorderChar}{_leftPadding}{line.PadLeft(WidthInChars)}{_rightPadding}{_rightBorderChar}";
                break;
            case TypoTextAlignment.Center:
            {
                var length = (uint)((WidthInChars - line.Length) / 2.0);
                fLine = (length > 0
                    ? $"{_leftMargin}{_leftBorderChar}{_leftPadding}{" ".Repeat(length)}{line}{" ".Repeat(length)}{_rightPadding}{_rightBorderChar}"
                    : $"{_leftMargin}{_leftBorderChar}{_leftPadding}{line}{_rightPadding}{_rightBorderChar}");

                break;
            }
            // ToDo: justify
            case TypoTextAlignment.Justify:
                line = FillLine(line, missinglength, WidthInChars);
                fLine = $"{_leftMargin}{_leftBorderChar}{_leftPadding}{line}{_rightPadding}{_rightBorderChar}";
                break;
            default:
                fLine =
                    $"{_leftMargin}{_leftBorderChar}{_leftPadding}{line}{" ".Repeat(missinglength)}{_rightPadding}{_rightBorderChar}";
                break;
        }

        // Add list char if necessary
        if (isFirstLine && !string.IsNullOrEmpty(ListChars))
        {
            result.Add(ListChars + fLine[(ListChars.Length)..]);
            return;
        }

        result.Add(fLine);
    }

    /// <summary>
    /// Fill a line to justify it
    /// </summary>
    /// <param name="line">Line to fill</param>
    /// <param name="missinglength">Missing length</param>
    /// <param name="widthInChars"></param>
    /// <returns>Justified line</returns>
    public static string FillLine(string line, int missinglength, int widthInChars)
    {
        try
        {
            if (line.Length < 0.8 * widthInChars)
            {
                return line;
            }

            var length = line.Length + missinglength;

            var bytes = Encoding.UTF8.GetBytes(line);

            var array = new Memory<byte>(new byte[length]);

            for (var i = 0; i < array.Length; i++)
            {
                array.Slice(i, 1).Span[0] = 0x20;
            }

            var numberOfBlanks = line.SpaceCount();

            var blanksPerUnit = (int)Math.Ceiling(missinglength / (double)numberOfBlanks);

            var pos = length - 1;
            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                var value = bytes[i];

                //Debug.Print($"{i}  {pos} {value}  {missinglength}");

                array.Slice(pos, 1).Span[0] = value;

                // Blank
                if (value == 0x20)
                {
                    if (missinglength > 0 && missinglength < blanksPerUnit)
                    {
                        missinglength = 0;
                    }
                    else if (missinglength >= blanksPerUnit)
                    {
                        missinglength -= blanksPerUnit;
                        pos -= blanksPerUnit + 1;
                    }
                    else
                    {
                        pos--;
                    }
                }
                else // Other char
                {
                    pos--;
                }

                if (pos < 0)
                {
                    break;
                }
            }

            return Encoding.UTF8.GetString(array.ToArray());
        }
        catch //(Exception e)
        {
            return string.Empty;
        }
    }

    private void GetLines()
    {
        // Only one line
        if (_content.Length < WidthInChars)
        {
            Lines.Add(_content);
            return;
        }

        // More than one line
        var pos = WidthInChars - 1;
        var altPos = 0;
        var length = _bytes.Length - 1;


        while (true)
        {

            // Check if current char is a blank (0x20). If not go back until a blank is found
            while (pos < length && pos >= 0 && _bytes.Span[pos] != 0x20)
            {
                pos--;
            }

            if (pos < 0)
            {
                if (Lines.Count == 0)
                {
                    Lines.Add(_content);
                }

                return;
            }

            if (pos >= length)
            {
                Lines.Add(_content.Substring(altPos, _content.Length - altPos));
                return;
            }

            // Take the part of the content from altPos up to pos as new line
            var value = _bytes.Slice(altPos, pos - altPos).Span.ToString();

            if (_maxLength < value.Length)
            {
                _maxLength = value.Length;
            }

            //Debug.Print($"{altPos} {pos}: {value.Length}: {value}");

            Lines.Add(value);

            altPos = pos + 1;
            pos += WidthInChars - 1;
        }
    }


    /// <summary>
    /// Get the formatted text as result
    /// </summary>
    /// <returns><see cref="StringBuilder"/> instance containing the formatted text</returns>
    public StringBuilder GetFormattedText()
    {
        var sb = new StringBuilder();

        foreach (var line in Lines)
        {
            sb.AppendLine(line);
        }

        return sb;
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Generic;

namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Basic implementation for <see cref="IStylesheet"/>
/// </summary>
public class Stylesheet : IStylesheet
{
    private readonly Dictionary<string, string> _attributes = new();

    /// <summary>
    /// Get a style by style name
    /// </summary>
    /// <param name="styleName">Style name</param>
    /// <returns>Style</returns>
    public string GetStyle(string styleName)
    {
        return _attributes.GetValueOrDefault(styleName);
    }

    /// <summary>
    /// Set CSS settings for "Table" element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassTable(string value)
    {
        _attributes.Add("cssClassTable", value);
    }

    /// <summary>
    /// Set CSS settings for "TR" header element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassTrHeader(string value)
    {
        _attributes.Add("cssClassTrHeader", value);
    }

    /// <summary>
    /// Set CSS settings for left bound "TH" element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassTh(string value)
    {
        _attributes.Add("cssClassTh", value);
    }

    /// <summary>
    /// Set CSS settings for centered "TH" element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassThCenter(string value)
    {
        _attributes.Add("cssClassThCenter", value);
    }

    /// <summary>
    /// Set CSS settings for right bound "TH" element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassThRight(string value)
    {
        _attributes.Add("cssClassThRight", value);
    }

    /// <summary>
    /// Set CSS settings for "TR" body element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassTrBody(string value)
    {
        _attributes.Add("cssClassTrBody", value);
    }

    /// <summary>
    /// Set CSS settings for left bound "TD" element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassTd(string value)
    {
        _attributes.Add("cssClassTd", value);
    }

    /// <summary>
    /// Set CSS settings for centered "TD" element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassTdCenter(string value)
    {
        _attributes.Add("cssClassTdCenter", value);
    }

    /// <summary>
    /// Set CSS settings for right bound "TD" element
    /// </summary>
    /// <param name="value">Style settings</param>
    public void CssClassTdRight(string value)
    {
        _attributes.Add("cssClassTdRight", value);
    }
}
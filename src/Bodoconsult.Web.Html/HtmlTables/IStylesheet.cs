// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using System.Collections.Generic;

namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Interface for simple HTML stylesheets
/// </summary>
public interface IStylesheet
{
    //IDictionary<string, string> Attributes { get; set; }

    /// <summary>
    /// Get a style by style name
    /// </summary>
    /// <param name="styleName">Style name</param>
    /// <returns>Style</returns>
    string GetStyle(string styleName);

    /// <summary>
    /// Set CSS settings for "Table" element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassTable(string value);

    /// <summary>
    /// Set CSS settings for left bound "TH" element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassTh(string value);

    /// <summary>
    /// Set CSS settings for centered "TH" element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassThCenter(string value);

    /// <summary>
    /// Set CSS settings for right bound "TH" element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassThRight(string value);

    /// <summary>
    /// Set CSS settings for left bound "TD" element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassTd(string value);

    /// <summary>
    /// Set CSS settings for centered "TD" element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassTdCenter(string value);

    /// <summary>
    /// Set CSS settings for right bound "TD" element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassTdRight(string value);

    /// <summary>
    /// Set CSS settings for "TR" header element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassTrHeader(string value);

    /// <summary>
    /// Set CSS settings for "TR" body element
    /// </summary>
    /// <param name="value">Style settings</param>
    void CssClassTrBody(string value);
}
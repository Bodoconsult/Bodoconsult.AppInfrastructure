// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;

namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Interface for HTML element implementations
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IElement<out T>
{
    /// <summary>
    /// Parent element
    /// </summary>
    T Parent { get; }

    /// <summary>
    /// Html attributes for elements. Use it to add class, id, style or other 
    /// attributes to an HTML tag
    /// </summary>
    IDictionary<string, string> Attributes { get; set; }

    /// <summary>
    /// Render element into an html string
    /// </summary>
    /// <returns>html string</returns>
    string RenderIt();
}
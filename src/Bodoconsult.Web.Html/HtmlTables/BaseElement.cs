// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using System.Text;

namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Base class for HTML elements
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseElement<T> : IElement<T>
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="parent">Parent element or null</param>
    protected BaseElement(T parent)
    {
        Attributes = new Dictionary<string, string>();
        Parent = parent;
    }

    /// <summary>
    /// Parent element
    /// </summary>
    public T Parent { get; private set; }

    /// <summary>
    /// Attributes
    /// </summary>
    public IDictionary<string, string> Attributes { get; set; }

    /// <summary>
    /// Render element into an html string
    /// </summary>
    /// <returns>html string</returns>
    public virtual string RenderIt()
    {
        return string.Empty;
    }


    internal string RenderAttributes(Table parent, string tag)
    {
        var erg = new StringBuilder();

        erg.Append(string.Empty);
        var style = parent.Stylesheet;
        if (style == null)
        {
            foreach (var ra in Attributes)
            {
                erg.Append($" {ra.Key}=\"{ra.Value}\"");
            }

            return erg.ToString();
        }

        switch (tag)
        {
            case "table":
                return $" class=\"{style.GetStyle("cssClassTable")}\"";
            case "tr-header":
                return $" class=\"{style.GetStyle("cssClassTrHeader")}\"";
            case "th":
                return $" class=\"{style.GetStyle("cssClassTh")}\"";
            case "th-center":
                return $" class=\"{style.GetStyle("cssClassThCenter")}\"";
            case "th-right":
                return $" class=\"{style.GetStyle("cssClassThRight")}\"";
            case "tr-body":
                return $" class=\"{style.GetStyle("cssClassTrBody")}\"";
            case "td":
                return $" class=\"{style.GetStyle("cssClassTd")}\"";
            case "td-center":
                return $" class=\"{style.GetStyle("cssClassTdCenter")}\"";
            case "td-right":
                return $" class=\"{style.GetStyle("cssClassTdRight")}\"";
        }

        return string.Empty;
    }
}

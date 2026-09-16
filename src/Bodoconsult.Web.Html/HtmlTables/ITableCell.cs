// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;

namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Interface for HTML table cells
/// </summary>
public interface ITableCell
{
    /// <summary>
    /// Attributes
    /// </summary>
    IDictionary<string, string> Attributes { get; set; }

    /// <summary>
    /// Text in the cell
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <returns>Rendered HTML string</returns>
    string RenderIt();
}
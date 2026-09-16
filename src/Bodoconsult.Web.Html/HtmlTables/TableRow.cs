// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using System.Linq;

namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Table row HTML element
/// </summary>
public class TableRow : BaseElement<Table>
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="parent">Row parent</param>
    public TableRow(Table parent): base(parent)
    {
        parent.Rows.Add(this);
    }

    /// <summary>
    /// Cells in the row
    /// </summary>
    public List<ITableCell> Cells { get; set; } = [];

    /// <summary>
    /// Render element into an html string
    /// </summary>
    /// <returns>html string</returns>
    public override string RenderIt()
    {
        var what = Cells.Any(x => x.GetType() == typeof(TableHeaderCell)) ? "tr-header" : "tr-body";
        var content = Cells.Aggregate("", (current, cell) => current + cell.RenderIt());
        var erg = $"\t\t<tr{RenderAttributes(Parent, what)}>\r\n{content}\t\t</tr>\r\n";
        return erg;
    }
}
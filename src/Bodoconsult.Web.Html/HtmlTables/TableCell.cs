// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Table cell HTML element
/// </summary>
public class TableCell : BaseElement<TableRow>, ITableCell
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="parent">Cell parent</param>
    public TableCell(TableRow parent): base(parent)
    {
        parent.Cells.Add(this);
    }

    /// <summary>
    /// Cell text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Render element into an html string
    /// </summary>
    /// <returns>html string</returns>
    public override string RenderIt()
    {
        var content = Text;
        var erg = $"\t\t\t<td{RenderAttributes(Parent.Parent, "td")}>\r\n\t\t\t\t{content}\r\n\t\t\t</td>\r\n";
        return erg;
    }
}
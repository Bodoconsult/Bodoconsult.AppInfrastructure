// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Table cell HTML element
/// </summary>
public class TableHeaderCell : BaseElement<TableRow>, ITableCell
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="parent">Cell parent</param>
    public TableHeaderCell(TableRow parent): base(parent)
    {
        parent.Cells.Add(this);
    }

    /// <summary>
    /// Text in the cell
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Render element into an html string
    /// </summary>
    /// <returns>html string</returns>
    public override string RenderIt()
    {
        var content = Text;
        var erg = $"\t\t\t<th{RenderAttributes(Parent.Parent, "th")}>\r\n\t\t\t\t{content}\r\n\t\t\t</th>\r\n";
        return erg;
    }
}
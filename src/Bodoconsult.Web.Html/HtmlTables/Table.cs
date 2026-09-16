// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Generic;
using System.Linq;

namespace Bodoconsult.Web.Html.HtmlTables;

/// <summary>
/// Table HTMl element
/// </summary>
public class Table : BaseElement<RootElement>
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public Table(): base(null)
    { }

    /// <summary>
    /// Rows in the table
    /// </summary>
    public List<TableRow> Rows { get; set; } = [];

    /// <summary>
    /// Stylesheet for formatting 
    /// </summary>
    public IStylesheet Stylesheet { get; set; }

    /// <summary>
    /// Render element into an html string
    /// </summary>
    /// <returns>html string</returns>
    public override string RenderIt()
    {
        var type = typeof(TableHeaderCell);

        var content = string.Empty;
        var content1 = Rows.Where(x => x.Cells.Any(y => y.GetType() == type)).Aggregate(string.Empty, (current, cell) => current + cell.RenderIt());

        if (!string.IsNullOrEmpty(content1))
        {
            content += $"\t<thead>\r\n{content1}\t</thead>\r\n";
        }

        content1 = Rows.Where(x => x.Cells.All(y => y.GetType() != type)).Aggregate(string.Empty, (current, cell) => current + cell.RenderIt());

        if (!string.IsNullOrEmpty(content1))
        {
            content += $"\t<tbody>\r\n{content1}\t</tbody>\r\n";
        }


        var erg = $"\r\n<table{RenderAttributes(this, "table")}>\r\n{content}</table>\r\n";
        return erg;
    }

    ///// <summary>
    ///// Render element into an MvcHtmlString for RAZOR view
    ///// </summary>
    ///// <returns>html string as MvcHtmlString</returns>
    //public MvcHtmlString RenderItMvc()
    //{
    //    return new MvcHtmlString(RenderIt());
    //}
}
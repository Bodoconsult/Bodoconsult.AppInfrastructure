// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Web.Html.HtmlTables;
using NUnit.Framework;

namespace Bodoconsult.Web.Html.Test.Html;

[TestFixture]
internal class TableTests
{
    [Test]
    public void TestSimpleTable()
    {
        // Arrange
        var table = new Table();

        // Header
        var headerrow = new TableRow(table);
        ITableCell cell = new TableHeaderCell(headerrow) { Text = "Header1" };
        cell = new TableHeaderCell(headerrow) { Text = "Header2" };


        // Content
        var contentrow = new TableRow(table);
        cell = new TableCell(contentrow) { Text = "Content1" };
        cell = new TableCell(contentrow) { Text = "Content2" };

        // Act
        var s = table.RenderIt();

        // Assert
        Debug.Print(s);

        Assert.That(!string.IsNullOrEmpty(s));


        if (string.IsNullOrEmpty(s))
        {
            return;
        }

        Assert.That(s.Contains("<table" ));
        Assert.That(s.Contains("</table>"));
        Assert.That(s.Contains("<tr"));
        Assert.That(s.Contains("</tr>"));
        Assert.That(s.Contains("<thead>"));
        Assert.That(s.Contains("</thead>"));
        Assert.That(s.Contains("<tbody>"));
        Assert.That(s.Contains("</tbody>"));
        Assert.That(s.Contains("<th"));
        Assert.That(s.Contains("</th>"));
        Assert.That(s.Contains("<td"));
        Assert.That(s.Contains("</td>"));
    }

    [Test]
    public void TestAdvancedTable()
    {
        // Arrange
        var table = new Table();
        table.Attributes.Add("class","cssTableClass");

        // Header
        var headerrow = new TableRow(table);
        headerrow.Attributes.Add("class","cssHeaderRowClass" );
        ITableCell cell = new TableHeaderCell(headerrow) { Text = "Header1" };
        cell.Attributes.Add("class", "cssHeaderCellClass" );
        cell = new TableHeaderCell(headerrow) { Text = "Header2" };

        // Content
        var contentrow = new TableRow(table);
        cell = new TableCell(contentrow) { Text = "Content1" };
        cell.Attributes.Add("class", "cssContentCellClass" );
        cell = new TableCell(contentrow) { Text = "Content2" };


        // Act
        var s = table.RenderIt();

        // Assert
        Debug.Print(s);

        Assert.That(!string.IsNullOrEmpty(s));


        if (string.IsNullOrEmpty(s)) return;

        Assert.That(s.Contains("<table"));
        Assert.That(s.Contains("</table>"));
        Assert.That(s.Contains("<tr"));
        Assert.That(s.Contains("</tr>"));
        Assert.That(s.Contains("<thead>" ));
        Assert.That(s.Contains("</thead>" ));
        Assert.That(s.Contains("<tbody>" ));
        Assert.That(s.Contains("</tbody>" ));
        Assert.That(s.Contains("<th" ));
        Assert.That(s.Contains("</th>" ));
        Assert.That(s.Contains("<td" ));
        Assert.That(s.Contains("</td>" ));
        Assert.That(s.Contains("class=\"cssTableClass\"" ));
        Assert.That(s.Contains("class=\"cssHeaderRowClass\"" ));
        Assert.That(s.Contains("class=\"cssHeaderCellClass\"" ));
        Assert.That(s.Contains("class=\"cssContentCellClass\"" ));
    }


    [Test]
    public void TestAdvancedTableWithStylesheet()
    {
        var styleSheet = new Stylesheet();
        styleSheet.CssClassTable("cssTableClass");
        styleSheet.CssClassTrHeader("cssHeaderRowClass");
        styleSheet.CssClassTh("cssHeaderCellClass");
        styleSheet.CssClassTrBody("cssBodyRowClass");
        styleSheet.CssClassTd("cssContentCellClass");


        // Arrange
        var table = new Table {Stylesheet = styleSheet};

        // Header
        var headerrow = new TableRow(table);
        ITableCell cell = new TableHeaderCell(headerrow) { Text = "Header1" };
        cell = new TableHeaderCell(headerrow) { Text = "Header2" };

        // Content
        var contentrow = new TableRow(table);
        cell = new TableCell(contentrow) { Text = "Content1" };
        cell = new TableCell(contentrow) { Text = "Content2" };


        // Act
        var s = table.RenderIt();

        // Assert
        Debug.Print(s);

        Assert.That(!string.IsNullOrEmpty(s));


        if (string.IsNullOrEmpty(s)) return;

        Assert.That(s.Contains("<table" ));
        Assert.That(s.Contains("</table>" ));
        Assert.That(s.Contains("<tr" ));
        Assert.That(s.Contains("</tr>" ));
        Assert.That(s.Contains("<thead>" ));
        Assert.That(s.Contains("</thead>" ));
        Assert.That(s.Contains("<tbody>" ));
        Assert.That(s.Contains("</tbody>" ));
        Assert.That(s.Contains("<th" ));
        Assert.That(s.Contains("</th>" ));
        Assert.That(s.Contains("<td" ));
        Assert.That(s.Contains("</td>" ));
        Assert.That(s.Contains("class=\"cssTableClass\"" ));
        Assert.That(s.Contains("class=\"cssHeaderRowClass\"" ));
        Assert.That(s.Contains("class=\"cssHeaderCellClass\"" ));
        Assert.That(s.Contains("class=\"cssContentCellClass\"" ));
    }


    //[Test]
    //public void TestSimpleTableWithHtmlAttributes()
    //{
    //    // Arrange
    //    var table = new Table();
    //    table.Attributes.Add("class", "test-table-class");
    //    table.Attributes.Add("class", "test-table-id");

    //    // Header
    //    var headerrow = new TableRow();
    //    ITableCell cell = new TableHeaderCell { Text = "Header1" };
    //    headerrow.Cells.Add(cell);
    //    cell = new TableHeaderCell { Text = "Header2" };
    //    headerrow.Cells.Add(cell);
    //    table.Rows.Add(headerrow);

    //    // Content
    //    var contentrow = new TableRow();
    //    cell = new TableCell { Text = "Content1" };
    //    contentrow.Cells.Add(cell);
    //    cell = new TableCell { Text = "Content2" };
    //    contentrow.Cells.Add(cell);
    //    table.Rows.Add(contentrow);

    //    // Act
    //    var s = table.RenderIt();

    //    // Assert
    //    Debug.Print(s);

    //    Assert.That(!string.IsNullOrEmpty(s));


    //    if (string.IsNullOrEmpty(s)) return;

    //    Assert.That(s.Contains(s, "<table");
    //    Assert.That(s.Contains(s, "</table>");
    //    Assert.That(s.Contains(s, "<tr");
    //    Assert.That(s.Contains(s, "</tr>");
    //    Assert.That(s.Contains(s, "<thead>");
    //    Assert.That(s.Contains(s, "</thead>");
    //    Assert.That(s.Contains(s, "<tbody>");
    //    Assert.That(s.Contains(s, "</tbody>");
    //    Assert.That(s.Contains(s, "<th");
    //    Assert.That(s.Contains(s, "</th>");
    //    Assert.That(s.Contains(s, "<td");
    //    Assert.That(s.Contains(s, "</td>");
    //}
}
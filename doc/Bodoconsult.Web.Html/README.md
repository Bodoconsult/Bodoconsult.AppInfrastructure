Bodoconsult.Web.Html
===================================

# What does the library 

The Bodoconsult.Web.Html library containes tools

-   [to create simple HTML table (Table class)](#create-a-simple-html-table)

-   [to create an advanced HTML table with more formatting (Table class width Sytlesheet class)](#create-an-advanced-html-table)

-   [to create simple markdown documents from code (MarkdownBuilder class)](#markdownbuilder-create-simple-markdown-files-from-code) 

-   []()

# How to use the library

The source code contain NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

# Create a simple HTML table

``` csharp
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
```

# Create an advanced HTML table

``` csharp
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

## Create a HTML table from a System.Data.DataTable

var dt = TestHelper.GetDataTable("LineChart.xml");
var erg = TableFormatter.FormatAsHtml(dt, true);
```

# MarkdownBuilder: create simple markdown files from code

``` csharp
[Test]
public void Save_ValidContent_FileSaved()
{
    var path = Path.Combine(Path.GetTempPath(), "test.md");

    if (File.Exists(path))
    {
        File.Delete(path);
    }

    Assert.That(File.Exists(path), Is.False);

    // Arrange 
    var builder = new MarkdownBuilder();
    const string content = "Test blubb blubb";

    // Act  
    builder.AddHeadline1("Headline 1");
    builder.AddParagraph(content);
    builder.AddRawMarkdown("## SubHeadline\r\n\r\nBlubb blabb blubb\r\n\r\n");
    builder.AddParagraph(content);
    builder.AddParagraph(content);

    builder.AddHeadline1("Headline 2");

    builder.AddParagraph(content);
    builder.AddParagraph(content);
    builder.AddParagraph(content);

    builder.Save(path);

    // Assert
    Assert.That(File.Exists(path));

    if (File.Exists(path))
    {
        File.Delete(path);
    }
}
```

# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.


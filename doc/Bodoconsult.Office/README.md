Bodoconsult.Office
===================

# Overview

>   [Use XlsxBuilder class to create OpenXML XSLX files](#use-xlsxbuilder-class-to-create-openxml-xslx-files)

>   [Use XlsxLateBindingBuilder class to create XLSX via COM late binding](#use-xlsxlatebindingbuilder-class-to-create-xlsx-via-com-late-binding)

>   [Use CsvBuilder class to create CSV files](#use-csvbuilder-class-to-create-csv-files)

>   [Use DocxBuilder class to create OpenXML DOCX files](#use-docxbuilder-class-to-create-openxml-docx-files)

## What does the library

Bodoconsult.Office library simplifies creating OpenXML spredsheets (XLSX) for database data in form of System.Data.DataTable objects.

It was developed with the intention to easily export database data to OpenXML XLSX spreadsheet files for use in Microsoft Excel (R).

For users of older versions of this library: the central classes were renamed starting with package version 1.0.8:

-   Csv => CsvBuilder

-   XslxOpenXml => XslxBuilder

-   ExcelLateBinding => XslxLateBindingBuilder

## How to use the library

The source code contain NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

# Use XlsxBuilder class to create OpenXML XSLX files

The XlsxBuilder class writes the content of a DataTable (in the sample code the variable dt) directly to an OpenXML spreadsheet file.

``` csharp
var path = Path.Combine(FileHelper.TempPath, "openxml.xlsx");

if (File.Exists(path))
{
    File.Delete(path);
}

var dt = TestHelper.GetDataTable("LineChart.xml");

var oe = new XlsxBuilder();
oe.Status += ShowStatus;
oe.Error += ShowError;
oe.NumberFormatDouble = "#,##0.000000";
oe.NewWorkbook(path);

oe.NewSheet("Daten");

oe.SelectRange(1, 1);
oe.Style = XlsxStyles.Header;
oe.SetValue("Hallo Welt1");
			
oe.FillDataTable(dt, 4, 1);

oe.Quit();
```

# Use XlsxLateBindingBuilder class to create XLSX via COM late binding

The XlsxLateBindingBuilder class uses COM late binding to export a DataTable (in the sample code the variable dt) to an Excel spreadsheet.

This requires Microsoft Excel (TM) installed on the PC running the code.

``` csharp
var dt = TestHelper.GetDataTable("LineChart.xml");

var excel = new XlsxLateBindingBuilder();
excel.Status += ShowStatus;
excel.NewWorkbook();
//if (e.ErrorCode != 0) return;

//excel.NewSheet("Daten");
excel.SelectSheetFirst("TransactionData");
excel.Header("Test");
excel.NumberFormat = "#,##0.000000";
excel.FillDataTable(dt, 4, 1);

excel.NewSheet("Daten2");
excel.Header("Test2");
excel.NumberFormat = "#,##0.00";
excel.FillDataTable(dt, 4, 1);

excel.Dispose();
```

# Use CsvBuilder class to create CSV files

Use the CsvBuilder class to create CSV formatted data files.

``` csharp
[Test]
public void Export_ValidDataTable_FileCreated()
{
    // Assert
    var path = Path.Combine(FileHelper.TempPath, "test.csv");

    if (File.Exists(path))
    {
        File.Delete(path);
    }

    var dt = TestHelper.GetDataTable("LineChart.xml");

    var excel = new CsvBuilder(dt)
    {
        FileName = path
    };

    // Act
    excel.Export();

    // Assert
    Assert.That(File.Exists(path));
}
```

# Use DocxBuilder class to create OpenXML DOCX files

Use the DocxBuilder class to create DOCX word processing files. 

Here a sample showing the most important features of DocxBuilder class:

``` csharp
[Test]
public void RealWorld_MultipleSectionsWithPageNumbering_DocxCreated()
{
    // Arrange 
    var path = Path.Combine(FileHelper.TempPath, "test.docx");

    if (File.Exists(path))
    {
        File.Delete(path);
    }

    var imagePath = Path.Combine(TestHelper.TestDataPath, "image.png");

    List<OpenXmlElement> runs;

    // Heading1 style
    var style = new DemoStyle
    {
        TypoFontColor = TypoColors.Cyan,
        FontName = "Aptos Black",
        FontSize = 20,
        Bold = true,
        Italic = true,
        TypoMargins =
        {
            Bottom = 2.5,
            Left = 1.5
        },
        TextAlignment = TypoTextAlignment.Center,
        TypoBorderThickness =
        {
            Bottom = 0.07,
            Left = 0.07,
            Right = 0.07,
            Top = 0.07,
        },
        TypoPaddings =
        {
            Bottom = 0.1,
            Left = 0.1,
            Right = 0.1,
            Top = 0.1,
        }
    };

    // Basics
    var dmd = CreateTypoMetaData();

    var docx = new DocxBuilder(dmd);
    docx.CreateDocument(path);

    // Create styles
    docx.AddNewStyle("heading1", "heading 1", style, 2);

    // First section
    docx.AddSection(false);
    docx.SetBasicPageProperties(21, 29.4, 5, 2, 2, 2);
    docx.AddHeaderToCurrentSection("Header section 1", 10);
    docx.AddFooterToCurrentSection($"Footer section 1\t{ITypography.PageFieldIndicator}", 10);

    docx.AddParagraph("Heading section 1", "heading1");
    docx.AddParagraph(TestHelper.MassText, "Normal");

    // Add an image
    docx.AddImage(imagePath, "Normal", 600, 400);

    // Add a definition list
    var dlRows = new List<DocxDefinitionListRow>();

    for (var j = 0; j < 5; j++)
    {
        var dlRow = new DocxDefinitionListRow
        {
            TermStyleId = "Normal",
            ItemsStyleId = "Normal"
        };

        // Term
        dlRow.Term.Add(DocxBuilder.CreateRun($"Test term {j}"));

        // Items
        for (var i = 0; i < 5; i++)
        {
            runs = [DocxBuilder.CreateRun($"Term item {j}-{i}")];

            dlRow.Items.Add(runs);
        }

        dlRows.Add(dlRow);
    }

    docx.AddDefinitionList(dlRows, 3, 9);

    docx.AddParagraph(TestHelper.MassText, "Normal");

    // New section
    docx.AddSection(true, true);
    docx.SetBasicPageProperties(21, 29.4, 8, 2, 2, 2);
    docx.AddHeaderToCurrentSection("Header section 2", 10);
    docx.AddFooterToCurrentSection($"Footer section 2\t{ITypography.PageFieldIndicator}", 10);

    // Heading and text
    docx.AddParagraph("Heading section 1", "heading1");
    docx.AddParagraph(TestHelper.MassText, "Normal");

    // Add multiple runs to a paragraph
    runs =
    [
        DocxBuilder.CreateRun("Das ist "),
        DocxBuilder.CreateRunBold("ein "),
        DocxBuilder.CreateRunItalic("Test für einen Hyperlink "),
        DocxBuilder.CreateHyperlink("http://www.bodoconsult.de", "Bodoconsult", docx.MainDocumentPart),
        DocxBuilder.CreateRun(" im Text!"),
        DocxBuilder.CreateLineBreak(),
        DocxBuilder.CreateRun("Das ist 1 ..."),
        DocxBuilder.CreatePageBreak(),
        DocxBuilder.CreateRun("Das ist 2 ...")
    ];

    docx.AddParagraph(runs, "Normal");

    docx.AddParagraph(TestHelper.MassText, "Normal");

    // Add a list
    var listItems = new List<List<OpenXmlElement>>();

    for (var i = 0; i < 10; i++)
    {
        runs = [DocxBuilder.CreateRun($"Test item {i}")];
        listItems.Add(runs);
    }

    docx.AddList(listItems, "Normal", ListStyleTypeEnum.Circle);

    docx.AddParagraph(TestHelper.MassText, "Normal");


    // Add a table
    var rows = new List<DocxTableRow>();

    var row = new DocxTableRow();

    var cell = new DocxTableCell();
    cell.Items.Add([DocxBuilder.CreateRun("A text")]);
    cell.StyleId = "Normal";
    row.Cells.Add(cell);

    cell = new DocxTableCell();
    cell.Items.Add([DocxBuilder.CreateRun("B text")]);
    cell.StyleId = "Normal";
    row.Cells.Add(cell);

    rows.Add(row);

    row = new DocxTableRow();

    cell = new DocxTableCell();
    cell.Items.Add([DocxBuilder.CreateRun("C text")]);
    cell.StyleId = "Normal";
    row.Cells.Add(cell);

    cell = new DocxTableCell();
    cell.Items.Add([DocxBuilder.CreateRun("D text")]);
    cell.StyleId = "Normal";
    row.Cells.Add(cell);

    rows.Add(row);

    ITypoTableStyle tableStyle = new DemoTableStyle();
    docx.AddTable(rows, tableStyle);

    // Assert
    Assert.That(File.Exists(path));

    docx.Dispose();

    FileSystemHelper.RunInDebugMode(path);
}

private TypoMetaData CreateTypoMetaData()
{
    return new TypoMetaData()
    {
        Authors = "Robert Leisner",
        Company = "Bodoconsult",
        FooterText = "FooterText",
        HeaderText = "HeaderText"
    };
}
```

# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.


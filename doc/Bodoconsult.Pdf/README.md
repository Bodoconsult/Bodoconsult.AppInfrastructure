Bodoconsult.Pdf
=============

# Overview

>   [PdfCreator class for simple reporting to PDF](#pdfcreator-class-for-simple-reporting-to-pdf)

>   [PdfBuilder class for more complex PDF files](#pdfbuilder-class-for-more-complex-pdf-files)

> [Adjusting layout for PdfCreator and PdfBuilder](#adjusting-layout)

>   >   [Adjusting layout by altering the DefaultStyleSet](#adjusting-layout-by-altering-the-defaultstyleset)

>   >   [Adjusting layout by creating an own StyleSet](#adjusting-layout-by-creating-an-own-styleset)

## What does the library

Bodoconsult.Pdf library simplifies the usage of the cool PDfSharp and Migradoc libraries <https://http://pdfsharp.net/> to create PDF files. 

The more simple PdfCreator class provides easy to use methods for common tasks for PDF creation.

The more complex PdfBuilder class is the more raw access to PDF creation with more features.

Bodoconsult.Pdf library was developed originally to create reports from databases with tables and charts from databases.

## How to use the library

The source code contain NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

# PdfCreator class for simple reporting to PDF

PdfCreator class is intended to be used for simple reports saved as PDF files. It provides a reduced set of features compared to PDFBuilder class. The following code shows most of the features of PdfCreator class:

``` csharp
[Test]
public void TestPdfCreatorWithDefaultStyleSheet()
{
    var fileName = Path.Combine(TestHelper.TempPath, "pdf1.pdf");
    CreateFile(fileName);
}

private void CreateFile(string fileName, bool landscape = false)
{
    var code = ResourceHelper.GetTextResource("code1.txt").Replace("\\t", "\t");


    if (File.Exists(fileName))
    {
        File.Delete(fileName);
    }

    var styleset = new DefaultStyleSet();
    styleset.CreatePageSetup();

    if (landscape)
    {
        LoadLandScapeSettings(styleset);
    }
    else
    {
        styleset.CalculateMeasures();
        styleset.InitializeStyles();
    }

    var pdf = new PdfCreator(styleset);

    pdf.SetDocInfo("Test", "Subject", "Author");

    pdf.SetHeader();
    pdf.SetFooter();

    pdf.CreateTocSection("Document title");
    pdf.CreateContentSection();

     
    pdf.AddParagraph("Heading 1", "Heading1");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(code, "Code");
    pdf.AddParagraph(TestDataHelper.MassText, "Normal");


    pdf.AddDefinitionList(DataHelper.GetDefinitionListAsDataTable());

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");
    pdf.AddTable(DataHelper.GetSmallDataTable(), "Tabellenüberschrift", "NoHeading1", "some additional info",
        "Details", pdf.Width);


    pdf.AddTableFrame(DataHelper.GetSmallDataTable(), "Tabellenüberschrift Frame", "NoHeading1", "some additional info",
        "Details", pdf.Width / 2);

    pdf.AddTableFrame(DataHelper.GetSmallDataTable(), "Tabellenüberschrift Frame", "NoHeading1", null,
        "Details", pdf.Width / 2);

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph("Heading 2", "Heading1");
    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph("Heading 2-1", "Heading2");
    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddImage(TestHelper.TestChartImage, 10, 6);

    pdf.AddParagraph("Heading 2-2", "Heading2");
    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph("Aufzählung 1", "Bullet1");
    pdf.AddParagraph("Aufzählung 2", "Bullet1");
    pdf.AddParagraph("Aufzählung 3", "Bullet1");
    pdf.AddParagraph("Aufzählung 4", "Bullet1");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(code, "Code");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(TestDataHelper.MassText, "Info");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(TestDataHelper.MassText, "Warning");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(TestDataHelper.MassText, "Error");

    pdf.AddParagraph("Blubb blabb blubb", "Citation");

    pdf.AddParagraph("Robert Leisner", "CitationSource");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.RenderToPdf(fileName, false);

    Assert.That(File.Exists(fileName));

    FileSystemHelper.RunInDebugMode(fileName);
}
```

# PdfBuilder class for more complex PDF files

PdfBuilder class is intended to be used for simple reports saved as PDF files. The following code shows most of the features of PdfBuilder class:

``` csharp
[Test]
public void TestPdfCreatorWithDefaultStyleSheet()
{
    var fileName = Path.Combine(TestHelper.TempPath, "pdf1.pdf");
    CreateFile(fileName);
}

private void CreateFile(string fileName, bool landscape = false)
{
    var code = ResourceHelper.GetTextResource("code1.txt").Replace("\\t", "\t");

    var pdfTable = DataHelper.GetSmallDataTable().ToPdfTable();

    if (File.Exists(fileName))
    {
        File.Delete(fileName);
    }

    var styleset = new DefaultStyleSet();
    styleset.CreatePageSetup();

    if (landscape)
    {
        LoadLandScapeSettings(styleset);
    }
    else
    {
        styleset.CalculateMeasures();
        styleset.InitializeStyles();
    }

    var pdf = new PdfBuilder(styleset, new WindowsFontResolver());

    pdf.SetHeader();
    pdf.SetFooter();

    pdf.CreateTocSection();
    pdf.AddToc1Entry("Überschrift 1", "b1");
    pdf.AddToc1Entry("Überschrift 2", "b2");
    pdf.AddToc2Entry("Überschrift 2", "b3");
    pdf.AddToc2Entry("Überschrift 2", "b4");

    pdf.CreateTofSection();
    pdf.AddTofEntry("Figure 1", "f1");

    pdf.CreateToeSection();
    pdf.AddToeEntry("Equation 1", "e1");

    pdf.CreateTotSection();
    pdf.AddTotEntry("Table 1", "t1");

    pdf.CreateContentSection();

    pdf.AddParagraph("Document title", "Title");

    pdf.AddHeading1("Überschrift 1", "b1");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddDefinitionList(GetDefinitionList());

    pdf.AddParagraph(code, "Code");
    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddFigure(TestHelper.TestChartImage, "Distribution", "f1", 15, 10);

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddEquation(TestHelper.TestEquationImage, "Equation", "e1", 7, 3);


    pdf.AddDefinitionList(DataHelper.GetDefinitionListAsDataTable());

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddTable(pdfTable, "Tabellenunterschrift", "t1", pdf.Width);

    pdf.AddTable(DataHelper.GetSmallDataTable(), "Tabellenüberschrift", "NoHeading1", "some additional info",
        "Details", pdf.Width);

    pdf.AddTableFrame(DataHelper.GetSmallDataTable(), "Tabellenüberschrift Frame", "NoHeading1", "some additional info",
        "Details", pdf.Width / 2);

    pdf.AddTableFrame(DataHelper.GetSmallDataTable(), "Tabellenüberschrift Frame", "NoHeading1", null,
        "Details", pdf.Width / 2);

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddHeading1("Überschrift 2", "b2");
    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddHeading2("Überschrift 2-1", "b3");
    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddImage(TestHelper.TestChartImage, 10, 6);

    pdf.AddHeading2("Überschrift 2-2", "b4");
    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph("Aufzählung 1", "Bullet1");
    pdf.AddParagraph("Aufzählung 2", "Bullet1");
    pdf.AddParagraph("Aufzählung 3", "Bullet1");
    pdf.AddParagraph("Aufzählung 4", "Bullet1");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(code, "Code");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(TestDataHelper.MassText, "Info");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(TestDataHelper.MassText, "Warning");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.AddParagraph(TestDataHelper.MassText, "Error");

    pdf.AddParagraph("Blubb blabb blubb", "Citation");

    pdf.AddParagraph("Robert Leisner", "CitationSource");

    pdf.AddParagraph(TestDataHelper.MassText, "Normal");

    pdf.RenderToPdf(fileName, false);

    Assert.That(File.Exists(fileName));

    FileSystemHelper.RunInDebugMode(fileName);
}

private static List<PdfDefinitionListTerm> GetDefinitionList()
{
    var result = new List<PdfDefinitionListTerm>();

    var item = new PdfDefinitionListTerm
    {
        Term = "Blubb",
        Items = [TestDataHelper.MassText, TestDataHelper.MassText]
    };
    result.Add(item);

    item = new PdfDefinitionListTerm
    {
        Term = "Blobb",
        Items = [TestDataHelper.MassText]
    };
    result.Add(item);

    item = new PdfDefinitionListTerm
    {
        Term = "Blabb",
        Items = [TestDataHelper.MassText, TestDataHelper.MassText, TestDataHelper.MassText]
    };
    result.Add(item);

    return result;
}
```

# Adjusting layout

If you need to adjust the layout of the PDF file, start with a DefaultStyleSet and change the properties of the stylesheet as required or create an own StyleSet

## Adjusting layout by altering the DefaultStyleSet

In the above samples this is done in a separate method in the following code line for paper orientation landscape:

``` csharp
if (landscape)
{
    LoadLandScapeSettings(styleset);
}
```

Here the code of the method to set paper orientation to landscape:

``` csharp
private void LoadLandScapeSettings(IStyleSet styleSet)
{
    var ps = styleSet.PageSetup;
    ps.Orientation = Orientation.Landscape;
    var w = ps.PageWidth;
    var h = ps.PageHeight;
    ps.PageHeight = w;
    ps.PageWidth = h;
    ps.LeftMargin = Unit.FromCentimeter(1.5);
    ps.RightMargin = Unit.FromCentimeter(1.5);
    ps.TopMargin = Unit.FromCentimeter(1.5);
    ps.BottomMargin = Unit.FromCentimeter(1.5);

    var width = Unit.FromCentimeter( ps.PageHeight.Centimeter - ps.TopMargin.Centimeter - ps.BottomMargin.Centimeter);

    var style = styleSet.Normal;
    style.Font.Name = "Arial Narrow";
    style.Font.Size = 9;

    // Spezielles Format für Tabellenbasis (nicht ändern!)
    style = styleSet.NormalTable;
    style.Font.Name = "Arial Narrow";
    style.Font.Size = 8;
    style.ParagraphFormat.SpaceBefore = 0;
    style.ParagraphFormat.SpaceAfter = 0;
    style.ParagraphFormat.PageBreakBefore = false;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

    style = styleSet.Heading1;
    style.Font.Name = "Arial Black";
    style.Font.Size = 12;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.PageBreakBefore = true;
    style.ParagraphFormat.KeepTogether = true;
    style.ParagraphFormat.KeepWithNext = true;
    style.ParagraphFormat.SpaceAfter = 2;
    style.ParagraphFormat.SpaceBefore = 4;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

    style = styleSet.Title;
    style.Font.Name = "Arial Black";
    style.Font.Size = 16;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.SpaceAfter = 9;
    style.ParagraphFormat.SpaceBefore = 12;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

    style = styleSet.NoHeading1;
    style.Font.Name = "Arial Black";
    style.Font.Size = 10;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.SpaceAfter = 2;
    style.ParagraphFormat.SpaceBefore = 4;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

    style = styleSet.ChartTitle;
    style.Font.Name = "Arial Narrow";
    style.Font.Size = 10;
    style.Font.Bold = true;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.SpaceBefore = 3;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
    style.ParagraphFormat.SpaceAfter = 3;

    style = styleSet.ChartYLabel;
    style.Font.Name = "Arial Narrow";
    style.Font.Size = 8;
    style.Font.Color = Colors.Black;

    style = styleSet.Toc1;
    style.Font.Name = "Arial";
    style.Font.Size = 10;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.SpaceBefore = 3;
    style.ParagraphFormat.TabStops.ClearAll();
    style.ParagraphFormat.AddTabStop(width, TabAlignment.Right);
    style.ParagraphFormat.SpaceAfter = 0;
    style.ParagraphFormat.Borders.Bottom.Width = 0;
    style.ParagraphFormat.Borders.Top.Width = 0;
    style.ParagraphFormat.Borders.Right.Width = 0;
    style.ParagraphFormat.Borders.Left.Width = 0;


    style = styleSet.TocHeading;
    style.Font.Name = "Arial Black";
    style.Font.Size = 12;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.SpaceAfter = 3;
    style.ParagraphFormat.SpaceAfter = 6;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

    // Kopfzeile
    style = styleSet.Header;
    style.Font.Name = "Arial Narrow";
    style.Font.Size = 8;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Right;
    style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.2);
    style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.5);
    style.ParagraphFormat.Borders.Bottom.Width = 1;
    style.ParagraphFormat.Borders.Bottom.Color = Colors.Black;

    style = styleSet.Details;
    style.Font.Name = "Arial Narrow";
    style.Font.Size = 10;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.SpaceAfter = 6;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

    style = styleSet.ChartCell;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

    style = styleSet.Footer;
    style.Font.Name = "Arial Narrow";
    style.Font.Size = 8;
    style.Font.Color = Colors.Black;
    style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
    //style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.5);
    style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.5);
    style.ParagraphFormat.TabStops.ClearAll();
    style.ParagraphFormat.AddTabStop(width, TabAlignment.Right);
    style.ParagraphFormat.Borders.Top.Width = 1;
    style.ParagraphFormat.Borders.Top.Color = Colors.Black;
}
```

## Adjusting layout by creating an own StyleSet

Below you can see the current implementation of the DefaultStyleSet. Feel free to use this implementation to build a new StyleSet for your own purposes.

``` csharp
/// <summary>
/// Defines a default style set.Use subclassing for creating advanced stylesets
/// </summary>
public class DefaultStyleSet : IStyleSet
{

    public DefaultStyleSet()
    {

        double verticalMargin = 3;

        PageSetup = new PageSetup
        {
            Orientation = Orientation.Portrait,
            PageFormat = PageFormat.A4,
            PageWidth = Unit.FromCentimeter(21.0),
            PageHeight = Unit.FromCentimeter(29.7),
            LeftMargin = Unit.FromCentimeter(2.5),
            TopMargin = Unit.FromCentimeter(1.9),
            RightMargin = Unit.FromCentimeter(2.5),
            BottomMargin = Unit.FromCentimeter(1.9)
        };


        var width = PageSetup.PageWidth - PageSetup.LeftMargin - PageSetup.RightMargin;

        Normal = new Style("Normal", null)
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = verticalMargin,
                SpaceAfter = verticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Left,
                
            }
        };

        //Normal.ParagraphFormat.Borders.Left.Width = 1;
        //Normal.ParagraphFormat.Borders.Left.Color = Colors.Black;

        NormalTable = new Style("NormalTable", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 8
            },
            ParagraphFormat =
            {
                SpaceBefore = 0,
                SpaceAfter = 0,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Center
            }
        };

        Heading1 = new Style("Heading1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 16,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                PageBreakBefore = true,
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = verticalMargin,
                SpaceBefore = 3 * verticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };


        Heading2 = new Style("Heading2", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 14,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = verticalMargin,
                SpaceBefore = 2 * verticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };

        Heading3 = new Style("Heading3", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 12,
                Color = Colors.Black,
                Bold = true
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = verticalMargin,
                SpaceBefore = 2 * verticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };


        Heading4 = new Style("Heading4", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 12,
                Color = Colors.Black,
                Bold = true
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = verticalMargin,
                SpaceBefore = verticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };


        Title = new Style("Title", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 20,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 4 * verticalMargin,
                SpaceBefore = 5 * verticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };


        NoHeading1 = new Style("NoHeading1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = verticalMargin,
                SpaceBefore = 2 * verticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };

        //style.ParagraphFormat.
        //style.ParagraphFormat.Shading.Color = Colors.Orange;


        ChartTitle = new Style("ChartTitle", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 10,
                Bold = true,
                Color = Colors.Red
            },
            ParagraphFormat =
            {
                SpaceBefore = 1.5 * verticalMargin,
                Alignment = ParagraphAlignment.Center,
                SpaceAfter = 1.5 * verticalMargin
            }
        };

        ChartYLabel = new Style("ChartYLabel", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 8,
                Color = Colors.Black
            }
        };

        Toc1 = new Style("TOC1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * verticalMargin,
                SpaceAfter = verticalMargin
            }
        };

        Toc1.ParagraphFormat.Borders.Bottom.Width = 0;
        Toc1.ParagraphFormat.Borders.Top.Width = 0;
        Toc1.ParagraphFormat.Borders.Right.Width = 0;
        Toc1.ParagraphFormat.Borders.Left.Width = 0;
        Toc1.ParagraphFormat.TabStops.AddTabStop(width, TabAlignment.Right);

        Toc2 = new Style("TOC2", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * verticalMargin,
                SpaceAfter = verticalMargin
            }
        };

        Toc2.ParagraphFormat.Borders.Bottom.Width = 0;
        Toc2.ParagraphFormat.Borders.Top.Width = 0;
        Toc2.ParagraphFormat.Borders.Right.Width = 0;
        Toc2.ParagraphFormat.Borders.Left.Width = 0;
        Toc2.ParagraphFormat.LeftIndent = Unit.FromCentimeter(1);
        Toc2.ParagraphFormat.TabStops.AddTabStop(width, TabAlignment.Right);

        Toc3 = new Style("TOC3", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * verticalMargin,
                SpaceAfter = verticalMargin
            }
        };

        Toc3.ParagraphFormat.Borders.Bottom.Width = 0;
        Toc3.ParagraphFormat.Borders.Top.Width = 0;
        Toc3.ParagraphFormat.Borders.Right.Width = 0;
        Toc3.ParagraphFormat.Borders.Left.Width = 0;
        Toc3.ParagraphFormat.LeftIndent = Unit.FromCentimeter(2);
        Toc3.ParagraphFormat.TabStops.AddTabStop(width, TabAlignment.Right);

        Toc4 = new Style("TOC4", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * verticalMargin,
                SpaceAfter = verticalMargin
            }
        };

        Toc4.ParagraphFormat.Borders.Bottom.Width = 0;
        Toc4.ParagraphFormat.Borders.Top.Width = 0;
        Toc4.ParagraphFormat.Borders.Right.Width = 0;
        Toc4.ParagraphFormat.Borders.Left.Width = 0;
        Toc4.ParagraphFormat.LeftIndent = Unit.FromCentimeter(3);
        Toc4.ParagraphFormat.TabStops.AddTabStop(width, TabAlignment.Right);


        TocHeading = new Style("TocHeading", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 12,
                Color = Colors.Black
            },
            ParagraphFormat = { SpaceAfter = verticalMargin }
        };
        TocHeading.ParagraphFormat.SpaceAfter = verticalMargin;
        TocHeading.ParagraphFormat.Alignment = ParagraphAlignment.Center;

        // Kopfzeile
        Header = new Style("Header", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 8,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Right,
                SpaceAfter = 4 * verticalMargin,
                SpaceBefore = 0
            }
        };
        Header.ParagraphFormat.Borders.Bottom.Width = 1;
        Header.ParagraphFormat.Borders.Bottom.Color = Colors.Black;


        Details = new Style("Details", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 3 * verticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };



        Table = new Style("Table", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 9,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 3 * verticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };



        ChartCell = new Style("ChartCell", "Normal")
        {
            ParagraphFormat = { Alignment = ParagraphAlignment.Center }
        };

        Footer = new Style("Footer", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 8,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Left,
                SpaceAfter = 0
            }
        };

        Footer.ParagraphFormat.Borders.Top.Visible = true;
        Footer.ParagraphFormat.Borders.Top.Width = 0.5;
        Footer.ParagraphFormat.Borders.Top.Color = Colors.Black;
        Footer.ParagraphFormat.AddTabStop(width, TabAlignment.Right);

        Code = new Style("Code", "Normal")
        {
            Font =
            {
                Name = "Courier New",
                Size = 10,
                Color = Colors.Black,
                Italic = true
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Left,
                SpaceBefore = 2 * verticalMargin,
                SpaceAfter = 2 * verticalMargin
            }
        };
        Code.ParagraphFormat.TabStops.ClearAll();
        Code.ParagraphFormat.TabStops.AddTabStop("1cm");
        Code.ParagraphFormat.TabStops.AddTabStop("2cm");
        Code.ParagraphFormat.TabStops.AddTabStop("3cm");
        Code.ParagraphFormat.TabStops.AddTabStop("4cm");
        Code.ParagraphFormat.TabStops.AddTabStop("5cm");
        Code.ParagraphFormat.TabStops.AddTabStop("6cm");
        Code.ParagraphFormat.TabStops.AddTabStop("7cm");
        Code.ParagraphFormat.TabStops.AddTabStop("8cm");
        Code.ParagraphFormat.TabStops.AddTabStop("9cm");
        Code.ParagraphFormat.TabStops.AddTabStop("10cm");
        Code.ParagraphFormat.TabStops.AddTabStop("11cm");
        Code.ParagraphFormat.TabStops.AddTabStop("12cm");
        Code.ParagraphFormat.TabStops.AddTabStop("13cm");
        Code.ParagraphFormat.TabStops.AddTabStop("14cm");
        Code.ParagraphFormat.TabStops.AddTabStop("26.7cm", TabAlignment.Right);


        Bullet1 = new Style("Bullet1", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Left,
                SpaceBefore = verticalMargin,
                SpaceAfter = verticalMargin
            }
        };
        Bullet1.ParagraphFormat.AddTabStop("26.7cm", TabAlignment.Right);
        Bullet1.ParagraphFormat.LeftIndent = Unit.FromCentimeter(1);
        var li = new ListInfo { ListType = ListType.BulletList1 };
        Bullet1.ParagraphFormat.ListInfo = li;

    }

    /// <summary>
    /// Normal paragraphs (default style)
    /// </summary>
    public PageSetup PageSetup { get; set; }


    /// <summary>
    /// Normal paragraphs (default style)
    /// </summary>
    public Style Normal { get; }

    /// <summary>
    /// Spezielles Format für Tabellenbasis (nicht ändern!)
    /// </summary>
    public Style NormalTable { get; }


    /// <summary>
    /// Table format
    /// </summary>
    public Style Table { get; }


    /// <summary>
    /// Title
    /// </summary>
    public Style Title { get; }

    /// <summary>
    /// Heading level 1
    /// </summary>
    public Style Heading1 { get; }

    /// <summary>
    /// Heading level 2
    /// </summary>
    public Style Heading2 { get; }

    /// <summary>
    /// Heading level 3
    /// </summary>
    public Style Heading3 { get; }


    /// <summary>
    /// Heading level 4
    /// </summary>
    public Style Heading4 { get; }


    public Style NoHeading1 { get; }


    public Style ChartTitle { get; }


    public Style ChartYLabel { get; }


    public Style Toc1 { get; }


    public Style Toc2 { get; }

    public Style Toc3 { get; }


    public Style Toc4 { get; }



    public Style TocHeading1 { get; }


    public Style Header { get; }



    public Style Details { get; }



    public Style ChartCell { get; }



    public Style Footer { get; }



    public Style Code { get; }



    public Style Bullet1 { get; }


}
```

## Remarks

PdfSharp and Migradoc do not support stylesets by default- Same for PdfSharp.MigraDoc.netstandard 1.3.1. MigraDoc supports styles for styling PDF files.

Bodoconsult.Pdf added styleset functionality. A styleset is defined as a list of MigraDoc styles. Therefore you can use all style settings supported by MigraDoc styles. 

With exception of the Normal style all styles defined in a styleset are injected as new styles to the MigraDoc PDF document. 

The Normal style exists in a MigraDoc PDF document by default. The Normal style from the styleset therefore has to cloned to the MigraDoc PDF document. 
It may happen that not all properties are cloned correctly.

# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.


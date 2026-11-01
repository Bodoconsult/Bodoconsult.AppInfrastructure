# What does the library

Bodoconsult.Core.Pdf library simplifies the usage of the cool PDfSharp and Migradoc libraries (<https://http://pdfsharp.net/>) to create PDF files. 
It uses the Nuget package PdfSharp.MigraDoc.netstandard 1.3.1. 
For more information on this package see <https://www.nuget.org/packages/PdfSharp.MigraDoc.netstandard/1.3.1>.



The central PdfCreator class provides easy to use methods for common tasks for PDF creation.


Bodoconsult.Core.Pdf library was developed originally to create reports from databases with tables and charts from databases.


# How to use the library

The source code contain NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

## Basic usage

            var styleset = new DefaultStyleSet();
            if (landscape) LoadLandScapeSettings(styleset);

            var pdf = new PdfCreator(styleset);

            pdf.SetDocInfo("Test", "Susbject", "Author");

            pdf.SetHeader("Kopfzeile", "Header1", logoFileName);
            pdf.SetFooter("Footer \t<<page>> / <<pages>>");

            pdf.CreateTocSection("Inhaltsverzeichnis");
            pdf.CreateContentSection();

         
            pdf.AddParagraph("Überschrift 1", "Heading1");

            pdf.AddParagraph(TestHelper.Masstext1, "Normal");

            pdf.AddParagraph(code, "Code");
            pdf.AddParagraph(TestHelper.Masstext1, "Normal");


            pdf.AddDefinitionList(TestHelper.GetDefinitionList(), "Normal", "Normal");

            pdf.AddParagraph(TestHelper.Masstext1, "Normal");
            pdf.AddTable(TestHelper.GetDataTable(), "Tabellenüberschrift", "NoHeading1", "some additional info",
                "Details", pdf.Width);


            pdf.AddTableFrame(TestHelper.GetDataTable(), "Tabellenüberschrift Frame", "NoHeading1", "some additional info",
                "Details", pdf.Width / 2);

            pdf.AddTableFrame(TestHelper.GetDataTable(), "Tabellenüberschrift Frame", "NoHeading1", null,
                "Details", pdf.Width / 2);


            pdf.AddParagraph("Überschrift 2", "Heading1");
            pdf.AddParagraph(TestHelper.Masstext1, "Normal");

            pdf.AddParagraph("Überschrift 2-1", "Heading2");
            pdf.AddParagraph(TestHelper.Masstext1, "Normal");
			
			pdf.AddImage(imageFileName, 150, 50);

            pdf.AddParagraph("Überschrift 2-2", "Heading2");
            pdf.AddParagraph(TestHelper.Masstext1, "Normal");

            pdf.AddParagraph("Aufzählung 1", "Bullet1");
            pdf.AddParagraph("Aufzählung 2", "Bullet1");
            pdf.AddParagraph("Aufzählung 3", "Bullet1");
            pdf.AddParagraph("Aufzählung 4", "Bullet1");

            pdf.AddParagraph(TestHelper.Masstext1, "Normal");

            pdf.AddParagraph(code, "Code");

            pdf.AddParagraph(TestHelper.Masstext1, "Normal");

            pdf.RenderToPdf(fileName, false);

## Adjusting layout

If you need to adjust the layout of the PDF file, start with a DefaultStyleSet and change the properties of the stylesheet as required. 

In the above sample this is done in a separate method in the following code line for paper orientation landscape:


            if (landscape) LoadLandScapeSettings(styleset);


Here the code of the method to set paper orientation to landscape:


        private void LoadLandScapeSettings(IStyleSet styleSet)
        {
            var ps = styleSet.PageSetup;
            ps.Orientation = Orientation.Landscape;
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


            style = styleSet.TocHeading1;
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

## Remarks

PdfSharp and Migradoc do not support stylesets by default- Same for PdfSharp.MigraDoc.netstandard 1.3.1. MigraDoc supports styles for styling PDF files.

Bodoconsult.Core.Pdf added styleset functionality. A styleset is defined as a list of MigraDoc styles. Therefore you can use all style settings supported by MigraDoc styles. 

With exception of the Normal style all styles defined in a styleset are injected as new styles to the MigraDoc PDF document. 

The Normal style exists in a MigraDoc PDF document by default. The Normal style from the styleset therefore has to cloned to the MigraDoc PDF document. 
It may happen that not all properties are cloned correctly.

# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software company.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.


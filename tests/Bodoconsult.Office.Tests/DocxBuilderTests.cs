// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// Info: https://ludovicperrichon.com/create-a-word-document-with-openxml-and-c/

// https://pvs-studio.com/en/blog/posts/csharp/0856/

// https://stackoverflow.com/questions/14144599/open-xml-word-c-sharp-split-into-two-columns

// https://github.com/devel0/netcore-docx/blob/master/src/docx/Styles.cs

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using Bodoconsult.App.Helpers;
using Bodoconsult.Office.Helpers;
using Bodoconsult.Office.Tests.Helpers;
using Bodoconsult.Office.Tests.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Bodoconsult.Office.Tests;

// https://woodsworkblog.wordpress.com/2012/08/06/add-header-and-footer-to-an-existing-word-document-with-openxml-sdk-2-0/

[TestFixture]
internal class DocxBuilderTests
{

    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        // Act  
        var docx = new DocxBuilder();

        // Assert
        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Null);
        Assert.That(docx.MainDocumentPart, Is.Null);
        Assert.That(docx.Body, Is.Null);

        docx.Dispose();
    }

    [Test]
    public void Create_ValidSetupFilePath_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var docx = new DocxBuilder();

        // Act  
        docx.CreateDocument(path);

        // Assert
        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();
    }

    [Test]
    public void Create_ValidSetupMemoryStream_DocxCreated()
    {
        // Arrange 
        var docx = new DocxBuilder();

        // Act  
        docx.CreateDocument();

        // Assert

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();
    }

    [Test]
    public void SaveDocument_ValidSetupMemoryStream_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var docx = new DocxBuilder();
        docx.CreateDocument();
        docx.AddParagraph("Blubb", "Normal");

        // Act  
        docx.SaveDocument(path);

        // Assert
        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddSection_ValidSetupMemoryStream_DocxCreated()
    {
        // Arrange 
        var pageStyle1 = new DefaultPageStyle();

        var docx = new DocxBuilder();
        docx.CreateDocument();

        // Act  
        docx.AddSection(pageStyle1);

        // Assert
        Assert.That(docx.CurrentSection, Is.Not.Null);

        docx.Dispose();
    }

    [Test]
    public void AddParagraph_SimpleTextNormal_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);

        // Act  
        docx.AddParagraph("Blubb", "Normal");

        // Assert
        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddParagraph_MultipleRunsNormal_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);

        // Act  
        var runs = new List<OpenXmlElement>
        {
            DocxBuilder.CreateRun("Das ist "),
            DocxBuilder.CreateRunBold("ein "),
            DocxBuilder.CreateRunItalic("Test für einen Hyperlink "),
            DocxBuilder.CreateHyperlink("http://www.bodoconsult.de", "Bodoconsult", docx.MainDocumentPart),
            DocxBuilder.CreateRun(" im Text!"),
            DocxBuilder.CreateLineBreak(),
            DocxBuilder.CreateRun("Das ist 1 ..."),
            DocxBuilder.CreatePageBreak(),
            DocxBuilder.CreateRun("Das ist 2 ..."),
        };

        docx.AddParagraph(runs, "Normal");

        // Assert
        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddParagraph_MultipleSections_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1, false);
        docx.AddParagraph("Section1", "Normal");

        // Act  
        docx.AddSection(pageStyle1);

        var runs = new List<OpenXmlElement>
        {
            DocxBuilder.CreateRun("Das ist "),
            DocxBuilder.CreateRunBold("ein "),
            DocxBuilder.CreateRunItalic("Test für einen Hyperlink "),
            DocxBuilder.CreateHyperlink("http://www.bodoconsult.de", "Bodoconsult", docx.MainDocumentPart),
            DocxBuilder.CreateRun(" im Text!"),
            DocxBuilder.CreateLineBreak(),
            DocxBuilder.CreateRun("Das ist 1 ..."),
            DocxBuilder.CreatePageBreak(),
            DocxBuilder.CreateRun("Das ist 2 ..."),
        };

        docx.AddParagraph(runs, "Normal");

        // Assert
        Assert.That(File.Exists(path));

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }


    [Test]
    public void AddFooterToCurrentSection_MultipleSectionsWithPageNumbering_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        var pageStyle1 = new DefaultPageStyle();

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1, false);
        docx.AddHeaderToCurrentSection( 10);
        docx.AddFooterToCurrentSection(10);

        docx.AddParagraph("Section1", "Normal");

        // Act  
        docx.AddSection(pageStyle1, true, true);
        docx.AddHeaderToCurrentSection(10);
        docx.AddFooterToCurrentSection(10);

        var runs = new List<OpenXmlElement>
        {
            DocxBuilder.CreateRun("Das ist "),
            DocxBuilder.CreateRunBold("ein "),
            DocxBuilder.CreateRunItalic("Test für einen Hyperlink "),
            DocxBuilder.CreateHyperlink("http://www.bodoconsult.de", "Bodoconsult", docx.MainDocumentPart),
            DocxBuilder.CreateRun(" im Text!"),
            DocxBuilder.CreateLineBreak(),
            DocxBuilder.CreateRun("Das ist 1 ..."),
            DocxBuilder.CreatePageBreak(),
            DocxBuilder.CreateRun("Das ist 2 ..."),
        };

        docx.AddParagraph(runs, "Normal");

        // Assert
        Assert.That(File.Exists(path));

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddHeaderToCurrentSection_MultipleSections_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        var pageStyle1 = new DefaultPageStyle();

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1, false);
        docx.AddHeaderToCurrentSection(10);
        docx.AddParagraph("Section1", "Normal");

        // Act  
        docx.AddSection(pageStyle1);
        docx.AddHeaderToCurrentSection(10);

        var runs = new List<OpenXmlElement>
        {
            DocxBuilder.CreateRun("Das ist "),
            DocxBuilder.CreateRunBold("ein "),
            DocxBuilder.CreateRunItalic("Test für einen Hyperlink "),
            DocxBuilder.CreateHyperlink("http://www.bodoconsult.de", "Bodoconsult", docx.MainDocumentPart),
            DocxBuilder.CreateRun(" im Text!"),
            DocxBuilder.CreateLineBreak(),
            DocxBuilder.CreateRun("Das ist 1 ..."),
            DocxBuilder.CreatePageBreak(),
            DocxBuilder.CreateRun("Das ist 2 ..."),
        };

        docx.AddParagraph(runs, "Normal");

        // Assert
        Assert.That(File.Exists(path));

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddFooterToCurrentSection_MultipleSections_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1, false);
        docx.AddFooterToCurrentSection(10);
        docx.AddParagraph("Section1", "Normal");

        // Act  
        docx.AddSection(pageStyle1);
        docx.AddFooterToCurrentSection(10);

        var runs = new List<OpenXmlElement>
        {
            DocxBuilder.CreateRun("Das ist "),
            DocxBuilder.CreateRunBold("ein "),
            DocxBuilder.CreateRunItalic("Test für einen Hyperlink "),
            DocxBuilder.CreateHyperlink("http://www.bodoconsult.de", "Bodoconsult", docx.MainDocumentPart),
            DocxBuilder.CreateRun(" im Text!"),
            DocxBuilder.CreateLineBreak(),
            DocxBuilder.CreateRun("Das ist 1 ..."),
            DocxBuilder.CreatePageBreak(),
            DocxBuilder.CreateRun("Das ist 2 ..."),
        };

        docx.AddParagraph(runs, "Normal");

        // Assert
        Assert.That(File.Exists(path));

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddParagraph_MultipleSections2Columns_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle2 = new ThreeColumnA4LandscapePageStyle();
        var pageStyle1 = new DefaultPageStyle();

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1, false);
        docx.AddParagraph("Section1", "Normal");

        // Act  
        docx.AddSection(pageStyle2);
        //docx.SetBasicPageProperties(21, 29.4, 8, 2, 2, 2);

        var runs = new List<OpenXmlElement>
        {
            DocxBuilder.CreateRun("Das ist "),
            DocxBuilder.CreateRunBold("ein "),
            DocxBuilder.CreateRunItalic("Test für einen Hyperlink "),
            DocxBuilder.CreateHyperlink("http://www.bodoconsult.de", "Bodoconsult", docx.MainDocumentPart),
            DocxBuilder.CreateRun(" im Text!"),
            DocxBuilder.CreateLineBreak(),
            DocxBuilder.CreateRun("Das ist 1 ..."),
            DocxBuilder.CreateColumnBreak(),
            DocxBuilder.CreateRun("Das ist 2 ..."),
            DocxBuilder.CreatePageBreak(),
            DocxBuilder.CreateRun("Das ist 3 ..."),

        };

        docx.AddParagraph(runs, "Normal");

        // Assert
        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddParagraph_SimpleTextHeading1_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        // Heading 1
        var styleRunPropertiesH1 = new StyleRunProperties();
        var color1 = new Color { Val = "2F5496" };
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue("32")
        };
        styleRunPropertiesH1.Append(color1);
        styleRunPropertiesH1.Append(fontSize1);
        // Check above at the begining of the word creation to check where mainPart come from
        

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);

        // Act  
        docx.AddNewStyle( "heading1", "heading 1", styleRunPropertiesH1, 2);
        docx.AddParagraph("Heading 1", "heading1");

        // Assert
        docx.AddParagraph("Blubb", "Normal");

        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }


    [Test]
    public void AddParagraph_SimpleTextDemoStyleHeading1_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        // Heading1 style
        var style = new DemoStyle
        {
            TypoFontColor = TypoColors.Cyan,
            FontName = "Arial Black",
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
                Bottom = 0.1,
                Left = 0.1,
                Right = 0.1,
                Top = 0.1,
            },
            TypoPaddings =
            {
                Bottom = 0.1,
                Left = 0.1,
                Right = 0.1,
                Top = 0.1,
            }
        };

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);

        // Act  
        docx.AddNewStyle("heading1", "heading 1", style, 2);
        docx.AddParagraph("Heading 1", "heading1");

        // Assert
        docx.AddParagraph("Blubb", "Normal");

        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddParagraph_Image_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        var imagePath = Path.Combine(TestHelper.TestDataPath, "image.png");

        // Heading 1
        var styleRunPropertiesH1 = new StyleRunProperties();
        var color1 = new Color { Val = "2F5496" };
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue("32")
        };
        styleRunPropertiesH1.Append(color1);
        styleRunPropertiesH1.Append(fontSize1);
        // Check above at the begining of the word creation to check where mainPart come from


        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);

        // Act  
        docx.AddNewStyle("heading1", "heading 1", styleRunPropertiesH1, 2);
        docx.AddParagraph("Heading 1", "heading1");
        docx.AddParagraph("Blubb", "Normal");

        // Assert
        docx.AddImage(imagePath, "Normal", 600, 400);

        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void SetBasicPageProperties_SimpleTextHeading1_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        // Heading 1
        var styleRunPropertiesH1 = new StyleRunProperties();
        var color1 = new Color { Val = "2F5496" };
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue("32")
        };
        styleRunPropertiesH1.Append(color1);
        styleRunPropertiesH1.Append(fontSize1);
        // Check above at the begining of the word creation to check where mainPart come from


        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);
        
        // Act 
        docx.AddNewStyle("heading1", "heading 1", styleRunPropertiesH1, 2);
        docx.AddParagraph("Heading 1", "heading1");

        // Assert
        docx.AddParagraph("Blubb", "Normal");

        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddMetadata_ValidSetupFilePath_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        // Heading 1
        var styleRunPropertiesH1 = new StyleRunProperties();
        var color1 = new Color { Val = "2F5496" };
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue("32")
        };
        styleRunPropertiesH1.Append(color1);
        styleRunPropertiesH1.Append(fontSize1);

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);
        docx.AddNewStyle("heading1", "heading 1", styleRunPropertiesH1, 2);
        docx.AddParagraph("Heading 1", "heading1");

        var md = new TypoMetaData
        {
            Authors = "RL",
            Company = "BCG",
            Title = "Title"
        };

        // Act  
        docx.AddMetadata(md);

        // Assert
        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddList_ValidSetupFilePath_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }


        var pageStyle1 = new DefaultPageStyle();
        // Heading 1
        var styleRunPropertiesH1 = new StyleRunProperties();
        var color1 = new Color { Val = "2F5496" };
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue("32")
        };
        styleRunPropertiesH1.Append(color1);
        styleRunPropertiesH1.Append(fontSize1);

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);
        docx.AddNewStyle("heading1", "heading 1", styleRunPropertiesH1, 2);
        docx.AddParagraph("Heading 1", "heading1");

        // Act  
        var listItems = new List<List<OpenXmlElement>>();

        for (var i = 0; i < 10; i++)
        {
            var runs = new List<OpenXmlElement> { DocxBuilder.CreateRun($"Test item {i}") };

            listItems.Add(runs);
        }

        docx.AddList(listItems, "Normal", ListStyleTypeEnum.Circle);

        // Assert
        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddTable_ValidSetupFilePath_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        // Heading 1
        var styleRunPropertiesH1 = new StyleRunProperties();
        var color1 = new Color { Val = "2F5496" };
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue("32")
        };
        styleRunPropertiesH1.Append(color1);
        styleRunPropertiesH1.Append(fontSize1);

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);
        docx.AddNewStyle("heading1", "heading 1", styleRunPropertiesH1, 2);
        docx.AddParagraph("Heading 1", "heading1");

        // Act  
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
        cell.Items.Add([ DocxBuilder.CreateRun("C text")]);
        cell.StyleId = "Normal";
        row.Cells.Add(cell);

        cell = new DocxTableCell();
        cell.Items.Add([DocxBuilder.CreateRun("D text")]);
        cell.StyleId = "Normal";
        row.Cells.Add(cell);

        rows.Add(row);

        ITypoTableStyle style = new DemoTableStyle();
        docx.AddTable(rows, style);

        // Assert
        Assert.That(File.Exists(path));

        Assert.That(docx, Is.Not.Null);
        Assert.That(docx.Docx, Is.Not.Null);
        Assert.That(docx.MainDocumentPart, Is.Not.Null);
        Assert.That(docx.Body, Is.Not.Null);

        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }

    [Test]
    public void AddDefinitionList_ValidSetupFilePath_DocxCreated()
    {
        // Arrange 
        var path = Path.Combine(FileHelper.TempPath, "test.docx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var pageStyle1 = new DefaultPageStyle();

        // Heading 1
        var styleRunPropertiesH1 = new StyleRunProperties();
        var color1 = new Color { Val = "2F5496" };
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue("32")
        };
        styleRunPropertiesH1.Append(color1);
        styleRunPropertiesH1.Append(fontSize1);

        var docx = new DocxBuilder();
        docx.CreateDocument(path);
        docx.AddSection(pageStyle1);
        docx.AddNewStyle("heading1", "heading 1", styleRunPropertiesH1, 2);
        docx.AddParagraph("Heading 1", "heading1");

        // Act  
        var rows = new List<DocxDefinitionListRow>();

        for (var j = 0; j < 5; j++)
        {
            var row = new DocxDefinitionListRow
            {
                TermStyleId = "Normal",
                ItemsStyleId = "Normal"
            };

            // Term
            row.Term.Add(DocxBuilder.CreateRun($"Test term {j}"));

            // Items
            for (var i = 0; i < 5; i++)
            {
                var runs = new List<OpenXmlElement> { DocxBuilder.CreateRun($"Term item {j}-{i}") };

                row.Items.Add(runs);
            }

            rows.Add(row);
        }

        docx.AddDefinitionList(rows, 3, 9);

        // Assert
        Assert.That(File.Exists(path));
        docx.Dispose();

        FileSystemHelper.RunInDebugMode(path);
    }


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

        var pageStyle1 = new DefaultPageStyle();

        // Heading1 style
        var style = new DemoStyle
        {
            TypoFontColor = TypoColors.Cyan,
            FontName = "Arial Black",
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
                Bottom = 0.1,
                Left = 0.1,
                Right = 0.1,
                Top = 0.1,
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
        var docx = new DocxBuilder();
        docx.CreateDocument(path);

        // Create styles
        docx.AddNewStyle("heading1", "heading 1", style, 2);

        // First section
        docx.AddSection(pageStyle1, false);
        docx.AddHeaderToCurrentSection(10);
        docx.AddFooterToCurrentSection(10);

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
        docx.AddSection(pageStyle1, true, true);
        docx.AddHeaderToCurrentSection(10);
        docx.AddFooterToCurrentSection(10);

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

        OpenXmlHelper.ValidateWordDocument(path);

        FileSystemHelper.RunInDebugMode(path);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Helpers;

namespace Bodoconsult.Text.Test.Helpers;

public static class TestDataHelper
{
    public const string MassText =
        "Lorem ipsum dolor ≥ = &gt; &#61; sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

    public const string CitationText = "Geld ist geprägte Freiheit";

    public static Document CreateDocument()
    {
        // Styleset (add after metadata)
        var styleset = StylesetHelper.CreateTestStyleset();
        var doc = CreateDocInternal(styleset);
        return doc;
    }

    public static Document CreateDocumentLandscape3Columns()
    {
        // Styleset (add after metadata)
        var styleset = StylesetHelper.CreateTestStylesetLandscape3Columns();
        var doc = CreateDocInternal(styleset);
        return doc;
    }

    private static Document CreateDocInternal(Styleset styleset)
    {
        var doc = new Document
        {
            Name = "MyReport",
        };
        
        // Metadata (add before style set)
        var meta = new DocumentMetaData
        {
            Title = "Title of the document",
            Description = "Blubb blabb bleeb",
            Keywords = "Blubb, blabb, bleeb",
            Company = "Bodoconsult GmbH",
            CompanyWebsite = "http://www.bodoconsult.de",
            Authors = "Robert Leisner",
            IsTocRequired = true,
            IsFiguresTableRequired = true,
            IsEquationsTableRequired = true,
            IsTablesTableRequired = true,
            FooterText = "Bodoconsult GmbH",
            HeaderText = "HeaderText",
            LogoPath = TestHelper.TestLogoImage,
            LogoWidth = 2,
            //BackgroundImagePath = TestHelper.TestBackgroundImage, 
            WatermarkText = "DraftX"
        };

        doc.AddBlock(meta);

        // Add styleset now
        doc.AddBlock(styleset);

        // Add TOC, TOF and TOE
        doc.AddBaseSections();

        // New section
        var section = new Section
        {
            Name = "Body",
            IsRestartPageNumberingRequired = true
        };

        doc.AddBlock(section);

        // Add a Title
        var title = new Title("Title");
        section.AddBlock(title);

        // Add a Subtitle
        var subtitle = new Subtitle("Subtitle");
        section.AddBlock(subtitle);

        // Add a SectionTitle
        var sectionTitle = new SectionTitle("SectionTitle");
        section.AddBlock(sectionTitle);

        // Add a SectionTitle
        var sectionSubtitle = new SectionSubtitle("SectionSubtitle");
        section.AddBlock(sectionSubtitle);

        // Add a heading level 1
        var heading1 = new Heading1("1. Heading level 1");
        section.AddBlock(heading1);

        // Add a heading level 2
        var heading2 = new Heading2("1.1. Heading level 2");
        section.AddBlock(heading2);

        // Add a heading level 3
        var heading3 = new Heading3("1.1.1. Heading level 3");
        section.AddBlock(heading3);

        // Add a heading level 4
        var heading4 = new Heading4("1.1.1.1. Heading level 4");
        section.AddBlock(heading4);

        // Add a heading level 4
        var heading5 = new Heading5("1.1.1.1.1. Heading level 5");
        section.AddBlock(heading5);

        // Add a simple paragraph
        var paragraph = new Paragraph($"Paragraph: {MassText}");
        section.AddBlock(paragraph);

        // Add a simple Central paragraph
        var paragraphCentral = new ParagraphCenter($"ParagraphCentral: {MassText}");
        section.AddBlock(paragraphCentral);

        // Add a simple Right paragraph
        var paragraphRight = new ParagraphRight($"ParagraphRight: {MassText}");
        section.AddBlock(paragraphRight);

        // Add a simple Justify paragraph
        var paragraphJustify = new ParagraphJustify($"ParagraphJustify: {MassText}");
        section.AddBlock(paragraphJustify);

        // Add a heading level 1
        heading1 = new Heading1("2. Heading level 1");
        section.AddBlock(heading1);

        // Add a heading level 2
        heading2 = new Heading2("2.1. Heading level 2");
        section.AddBlock(heading2);

        // Add a heading level 3
        heading3 = new Heading3("2.1.1. Heading level 3");
        section.AddBlock(heading3);

        // Add a heading level 4
        heading4 = new Heading4("2.1.1.1. Heading level 4");
        section.AddBlock(heading4);

        // Add a heading level 5
        heading5 = new Heading5("2.1.1.1.1. Heading level 5");
        section.AddBlock(heading5);


        // Add a paragraph with multiple inlines
        paragraph = new Paragraph("Paragraph with LineBreak and Bold and BoldItalic: ");

        //   Add a simple span
        var span = new Span(MassText);
        paragraph.AddInline(span);

        //   Add a line break
        paragraph.AddInline(new LineBreak());

        //   Add bold text
        var bold = new Bold(MassText);
        paragraph.AddInline(bold);

        section.AddBlock(paragraph);

        // Add a bold and italic text paragraph
        var bold2 = new Bold(MassText);

        var italic = new Italic();
        italic.AddInline(bold2);

        paragraph = new Paragraph();
        paragraph.AddInline(italic);
        section.AddBlock(paragraph);

        // Add a Citation
        var citation = new Citation($"Citation: {CitationText}")
        {
            Source = "Fjodor Dostojewski"
        };
        section.AddBlock(citation);

        // Add a code
        var code = new Code($"Code: {MassText}");
        section.AddBlock(code);

        // Add an info
        var info = new Info($"Info: {MassText}");
        section.AddBlock(info);

        // Keep distance between info and warning
        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        // Add a Warning
        var warning = new Warning($"Warning: {MassText}");
        section.AddBlock(warning);

        // Keep distance between warning and error
        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        // Add an error
        var error = new Error($"Error: {MassText}");
        section.AddBlock(error);

        // Add a hyperlink
        paragraph = new Paragraph("ParagraphWithHyperlink: this is a link: ");
        var link = new Hyperlink("Link to blubb")
        {
            Uri = "http://www.bodoconsult.de"
        };
        paragraph.AddInline(link);
        section.AddBlock(paragraph);

        // Add an image
        var image = new Image("Test image", TestHelper.TestChartImage)
        {
            OriginalHeight = 1075,
            OriginalWidth = 1725
        };
        section.AddBlock(image);

        // Add a figure 1
        var figure1 = new Figure("Figure 1", TestHelper.TestChartImage)
        {
            OriginalHeight = 1075,
            OriginalWidth = 1725
        }; ;
        section.AddBlock(figure1);

        // Add a figure 2
        var figure2 = new Figure("Figure 2", TestHelper.TestDistributionImage)
        {
            OriginalHeight = 1075,
            OriginalWidth = 1725
        }; ;
        section.AddBlock(figure2);

        // Add an equation 1
        var equiation1 = new Equation("Equation 1", TestHelper.TestEquationImage)
        {
            OriginalHeight = 100,
            OriginalWidth = 300
        }; ;
        section.AddBlock(equiation1);


        // Add an equation 2
        var equiation2 = new Equation("Equation 2", TestHelper.TestEquationImage)
        {
            OriginalHeight = 100,
            OriginalWidth = 300
        }; ;
        section.AddBlock(equiation2);


        // Add a list
        var list = new List();

        var listItem1 = new ListItem($"First list item: {MassText}");
        list.AddBlock(listItem1);

        var listItem2 = new ListItem($"Second list item: {MassText}");
        list.AddBlock(listItem2);

        section.AddBlock(list);

        // Keep distance
        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        // Add a table
        var dt = DataHelper.GetDataTable();

        var dtp = new DataTableParser(dt, "Portfolio");
        dtp.ParseColumns();
        dtp.ParseRows();

        // Assert
        section.AddBlock(dtp.Table);

        // Keep distance
        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        // Add definition list
        var dl = CreateDefinitionList();
        section.AddBlock(dl);

        // Keep distance
        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);
        return doc;
    }

    public static DefinitionList CreateDefinitionList()
    {
        var dl = new DefinitionList();

        var dlt = new DefinitionListTerm("Share");

        var dli = new DefinitionListItem("Blubb");
        // var dli = new DefinitionListItem(MassText);
        dlt.AddBlock(dli);

        dli = new DefinitionListItem("Blabb");
        dlt.AddBlock(dli);

        dl.AddBlock(dlt);

        dlt = new DefinitionListTerm("Fixed income");

        dli = new DefinitionListItem(MassText);
        dlt.AddBlock(dli);

        dl.AddBlock(dlt);

        dlt = new DefinitionListTerm("Real estate");

        dli = new DefinitionListItem(MassText);
        dlt.AddBlock(dli);

        dli = new DefinitionListItem(MassText);
        dlt.AddBlock(dli);

        dl.AddBlock(dlt);
        return dl;
    }

    public static Document CreateDocumentRealWorld()
    {
        var doc = new Document
        {
            Name = "MyReport",
        };

        CreateRealWorldReportContent(doc);

        return doc;
    }

    public static void CreateRealWorldReportContent(Document doc)
    {
        // Styleset (add after metadata)
        var styleset = StylesetHelper.CreateTestStyleset();
        doc.AddBlock(styleset);

        // Metadata (add before style set)
        var meta = new DocumentMetaData
        {
            Title = "Securities portfolio management",
            Description = "Basics of sercurity portfolio management",
            Keywords = "Securities, portfolio, management, risk, asset, allocation",
            Company = "Bodoconsult GmbH",
            CompanyWebsite = "http://www.bodoconsult.de",
            Authors = "Robert Leisner",
            IsTocRequired = true,
            IsFiguresTableRequired = true,
            IsEquationsTableRequired = true
        };

        doc.AddBlock(meta);

        // Add TOC, TOF and TOE
        doc.AddBaseSections();

        // New section
        var section = new Section
        {
            Name = "Body"
        };

        doc.AddBlock(section);

        // Add a Title
        var title = new Title("Securities portfolio management");
        section.AddBlock(title);

        // Add a Subtitle
        var subtitle = new Subtitle("The art of managing risk for securities portfolios");
        section.AddBlock(subtitle);

        // Add a heading level 1
        var heading1 = new Heading1("Introduction");
        section.AddBlock(heading1);

        // Add a simple paragraph
        var paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        heading1 = new Heading1("Securities portfolios");
        section.AddBlock(heading1);

        // Add a heading level 2
        var heading2 = new Heading2("Asset Allocation");
        section.AddBlock(heading2);

        section.AddBlock(paragraph);

        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        var citation = new Citation(CitationText)
        {
            Source = "Fjodor Dostojewski"
        };
        section.AddBlock(citation);

        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        // Add a heading level 3
        var heading3 = new Heading3("1.1.1. Heading level 3");
        section.AddBlock(heading3);

        // Add a heading level 4
        var heading4 = new Heading4("1.1.1.1. Heading level 4");
        section.AddBlock(heading4);

        // Add a heading level 4
        var heading5 = new Heading5("1.1.1.1.1. Heading level 5");
        section.AddBlock(heading5);

        // Add a simple paragraph
        paragraph = new Paragraph($"Paragraph: {MassText}");
        section.AddBlock(paragraph);

        // Add a simple Central paragraph
        var paragraphCentral = new ParagraphCenter($"ParagraphCentral: {MassText}");
        section.AddBlock(paragraphCentral);

        // Add a simple Right paragraph
        var paragraphRight = new ParagraphRight($"ParagraphRight: {MassText}");
        section.AddBlock(paragraphRight);

        // Add a simple Justify paragraph
        var paragraphJustify = new ParagraphJustify($"ParagraphJustify: {MassText}");
        section.AddBlock(paragraphJustify);

        // Add a heading level 1
        heading1 = new Heading1("2. Heading level 1");
        section.AddBlock(heading1);

        // Add a heading level 2
        heading2 = new Heading2("2.1. Heading level 2");
        section.AddBlock(heading2);

        // Add a heading level 3
        heading3 = new Heading3("2.1.1. Heading level 3");
        section.AddBlock(heading3);

        // Add a heading level 4
        heading4 = new Heading4("2.1.1.1. Heading level 4");
        section.AddBlock(heading4);

        // Add a heading level 4
        heading5 = new Heading5("2.1.1.1.1. Heading level 5");
        section.AddBlock(heading5);

        // Add a paragraph with multiple inlines
        paragraph = new Paragraph("Paragraph with LineBreak and Bold and BoldItalic: ");

        //   Add a simple span
        var span = new Span(MassText);
        paragraph.AddInline(span);

        //   Add a line break
        paragraph.AddInline(new LineBreak());

        //   Add bold text
        var bold = new Bold(MassText);
        paragraph.AddInline(bold);

        section.AddBlock(paragraph);

        // Add a bold and italic text paragraph
        var bold2 = new Bold(MassText);

        var italic = new Italic();
        italic.AddInline(bold2);

        paragraph = new Paragraph();
        paragraph.AddInline(italic);
        section.AddBlock(paragraph);

        // Add a Citation


        // Add a code
        var code = new Code($"Code: {MassText}");
        section.AddBlock(code);

        // Add an info
        var info = new Info($"Info: {MassText}");
        section.AddBlock(info);

        // Keep distance between info and warning
        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        // Add a Warning
        var warning = new Warning($"Warning: {MassText}");
        section.AddBlock(warning);

        // Keep distance between warning and error
        paragraph = new Paragraph(MassText);
        section.AddBlock(paragraph);

        // Add an error
        var error = new Error($"Error: {MassText}");
        section.AddBlock(error);

        // Add a hyperlink
        paragraph = new Paragraph("ParagraphWithHyperlink: this is a link: ");
        var link = new Hyperlink("Link to blubb")
        {
            Uri = "http://www.bodoconsult.de"
        };
        paragraph.AddInline(link);
        section.AddBlock(paragraph);

        // Add an image
        var image = new Image("Test image", TestHelper.TestChartImage)
        {
            OriginalHeight = 1075,
            OriginalWidth = 1725
        };
        section.AddBlock(image);

        // Add a figure 1
        var figure1 = new Figure("Figure 1", TestHelper.TestChartImage)
        {
            OriginalHeight = 1075,
            OriginalWidth = 1725
        }; ;
        section.AddBlock(figure1);

        // Add a figure 2
        var figure2 = new Figure("Figure 2", TestHelper.TestDistributionImage)
        {
            OriginalHeight = 1075,
            OriginalWidth = 1725
        }; ;
        section.AddBlock(figure2);

        // Add an equation 1
        var equiation1 = new Equation("Equation 1", TestHelper.TestEquationImage)
        {
            OriginalHeight = 159,
            OriginalWidth = 588
        }; ;
        section.AddBlock(equiation1);

        // Add an equation 2
        var equiation2 = new Equation("Equation 2", TestHelper.TestEquationImage)
        {
            OriginalHeight = 159,
            OriginalWidth = 588
        }; ;
        section.AddBlock(equiation2);

        // Add a list
        var list = new List();

        var listItem1 = new ListItem("First list item");
        list.AddBlock(listItem1);

        var listItem2 = new ListItem("Second list item");
        list.AddBlock(listItem2);

        section.AddBlock(list);
    }
}

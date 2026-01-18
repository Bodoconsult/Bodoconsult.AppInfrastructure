// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Wpf.Documents.General;
using Bodoconsult.App.Wpf.Documents.Helpers;
using Bodoconsult.App.Wpf.Documents.Reports;
using Bodoconsult.App.Wpf.Documents.Services;
using Bodoconsult.App.Wpf.Documents.Test.Helpers;
using Bodoconsult.App.Wpf.Helpers;
using Bodoconsult.App.Wpf.I18N;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.Wpf.Documents.Test;

[TestFixture]
public class ReportBaseTests
{

    private readonly string _tempPath = Path.GetTempPath();

    private readonly II18N _i18N= Bodoconsult.I18N.I18N.Current;

    /// <summary>
    /// Constructor to initialize class
    /// </summary>
    public ReportBaseTests()
    {
        _i18N.Reset();

        var provider = new WpfFileLocalesProvider(TestHelper.Assembly, "Locales");
        _i18N.AddProvider(provider);


        _i18N.Init();

        _i18N.Locale = "de";
    }


    ///// <summary>
    ///// Runs in front of each test method
    ///// </summary>
    //[TestInitialize]
    //public void Setup()
    //{


    //}

    ///// <summary>
    ///// Cleanup aft test methods
    ///// </summary>
    //[TestCleanup]
    //public void Cleanup()
    //{


    //}


    [Test]
    public void TestReportBase_AddElementsDirectly()
    {
        //Arrange

        var typoService = new TypographySettingsService
        {
            MaxImageHeight = 300,
            DocumentMetaData = new TypoMetaData()
            {
                LogoPath = TestHelper.TestLogoImage,
                FooterText = "Bodoconsult GmbH",
                CurrentLanguage = "de"
            },
            FigureCounterPrefix = "Abb.",
            ShowFigureCounter = true,
        };


        var r = new ReportBase(typoService);
        var fileName = Path.Combine(_tempPath, "TestReportBase_AddElementsDirectly.xps");

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }


        // Act

        r.Components.Add(new ReportTitleElement { Content = "ReportTitle" });

        r.Components.Add(new ReportHeaderElement { Content = "Header Level 1", Level = 1 });
        r.Components.Add(new ReportParagraphElement { Content = FlowDocHelper.MassText });

        r.Components.Add(new ReportImageElement { ImagePath = @"Resources\testimage.png" });

        r.Components.Add(new ReportTableElement { Data = FlowDocHelper.GetTableData(5, 4), TableType = TableTypes.NormalNoHeaderUnbordered });


        r.Components.Add(new ReportHeaderElement { Content = "Image from stream", Level = 2 });

        var ms = new MemoryStream();

        var stream = File.OpenRead(TestHelper.TestChartImage);

        stream.CopyTo(ms);

        r.Components.Add(new ReportImageElement { ImageData = ms });
        stream.Close();



        r.Components.Add(new ReportParagraphElement { Content = FlowDocHelper.MassText });

        r.Components.Add(new ReportFigureElement { ImagePath = @"Resources\testimage.png", Title = "Image title1212" });

        r.Components.Add(new ReportHeaderElement { Content = "Header Level 2", Level = 2 });


        r.Components.Add(new ReportParagraphElement { Content = FlowDocHelper.MassTextWithImages });


        r.Components.Add(new ReportHeaderElement { Content = "Header Level 2", Level = 2 });
        r.Components.Add(new ReportParagraphElement { Content = FlowDocHelper.MassText });

        var textblock =
            $"<P>{FlowDocHelper.MassText}</P>{FlowDocHelper.ImageTag}<P>{FlowDocHelper.MassTextTags}</P>{FlowDocHelper.ImageTagWithTitle}<P>{FlowDocHelper.MassText}</P>";

        r.Components.Add(new ReportTextBlockElement { Content = textblock });


        r.Components.Add(new ReportPageBreakElement());
        r.Components.Add(new ReportHeaderElement { Content = "Header Level 1 with page break before", Level = 1 });

        r.Components.Add(new ReportParagraphElement { Content = FlowDocHelper.MassText });

        r.Components.Add(new ReportTableElement { Data = FlowDocHelper.GetTableData(4, 2) });


        r.Components.Add(new ReportHeaderElement { Content = "Header Level 1 with page break before", Level = 1 });

        r.Components.Add(new ReportParagraphElement { Content = FlowDocHelper.MassText });

        r.Components.Add(new ReportTableElement { Data = FlowDocHelper.GetTableData(4, 2), TableType = TableTypes.ExtraSmall });

        r.BuildReport();
        r.SaveAsXps(fileName);
        r.Dispose();



        //Assert
        Assert.That(File.Exists(fileName));

        FileSystemHelper.RunInDebugMode(fileName);


    }

    [Test]
    public void TestReportBase_DefaultPageSettings()
    {
        //Arrange
        var typoService = new TypographySettingsService
        {
            DocumentMetaData = new TypoMetaData()
            {
                LogoPath = TestHelper.TestLogoImage,
                FooterText = "Bodoconsult GmbH",
                CurrentLanguage = "de"
            },
            FigureCounterPrefix = "Abb.",
        };

        var fileName = Path.Combine(_tempPath,"TestReportBase_DefaultPageSettings.xps");

        CreateReport(typoService, fileName);

        //Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }


    [Test]
    public void TestReportBase_ElegantPageSettings()
    {
        //Arrange

        var typoService = new TypographySettingsService();
        DocumentHelper.A4ElegantPrintDefintion(typoService);
        typoService.DocumentMetaData = new TypoMetaData()
        {
            LogoPath = TestHelper.TestLogoImage,
            FooterText = "Bodoconsult GmbH",
            CurrentLanguage = "de"
        };
        typoService.FigureCounterPrefix = "Abb.";

        var fileName = Path.Combine(_tempPath, "TestReportBase_ElegantPageSettings.xps");

        CreateReport(typoService, fileName);


        //Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }



    [Test]
    public void TestReportBase_Typography()
    {
        //Arrange

        var typo = new ElegantTypographyPageHeader("Times New Roman", "Times New Roman", "Arial Black");

        var typoService = new TypographySettingsService(typo)
        {
            MaxImageHeight = 300,
            DocumentMetaData = new TypoMetaData()
            {
                LogoPath = TestHelper.TestLogoImage,
                FooterText = "Bodoconsult GmbH",
                CurrentLanguage = "de"
            },
            FigureCounterPrefix = "Abb.",
            ShowFigureCounter = true
        };

        var fileName = Path.Combine(_tempPath, "TestReportBase_ElegantPageSettings.xps");

        CreateReport(typoService, fileName);


        //Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }


    [Test]
    public void TestReportBase_IndividualPageSettings()
    {
        //Arrange
        var typoService = new TypographySettingsService();
        DocumentHelper.A4ElegantPrintDefintion(typoService);
        typoService.DocumentMetaData = new TypoMetaData()
        {
            LogoPath = TestHelper.TestLogoImage,
            FooterText = "Bodoconsult GmbH",
            CurrentLanguage = "de"
        };
        typoService.FigureCounterPrefix = "Abb.";
        typoService.UserDefinedTypographyFile =
            Path.Combine(TestHelper.TestDataPath, "TypographyCustomer.xaml");

        var fileName = Path.Combine(_tempPath, "TestReportBase_IndividualPageSettings.xps");

        CreateReport(typoService, fileName);


        //Assert
        Assert.That(File.Exists(fileName));

        FileSystemHelper.RunInDebugMode(fileName);
    }

    [Test]
    public void TestReportBase_Demo_FileCreated()
    {
        //Arrange
        var fileName = Path.Combine(_tempPath, "TestReportBase_Demo.pdf");

        // Define a typography
        var typo = new ElegantTypographyPageHeader("Times New Roman", "Times New Roman", "Arial Black");

        // Get the typo service needed
        var typoService = new TypographySettingsService(typo)
        {
            MaxImageHeight = 300,
            DocumentMetaData = new TypoMetaData()
            {
                LogoPath = TestHelper.TestLogoImage,
                FooterText = "Bodoconsult GmbH",
                CurrentLanguage = "de"
            },
            FigureCounterPrefix = "Abb.",
            ShowFigureCounter = true,
        };
            
        // Act

        // Create the base report
        var r = new ReportBase(typoService, _i18N);

        // Add a translated title and subtitle
        r.AddTitle("Resx:Simulation.Wpf.ReportTitle");
        r.AddTitle2("I18N:Simulation.Wpf.ReportTitle");

        // Add paragraphs
        r.AddHeader("Add a paragraph", 1);
        r.AddHeader("", 2);
        r.AddParagraph(FlowDocHelper.MassText);
        r.AddHeader("Add a paragraph", 2);
        r.AddParagraph(FlowDocHelper.MassTextTags);

        // Add a numbered List
        r.AddHeader("Add a numbered list", 1);
        r.AddNumberedList(FlowDocHelper.GetListData());

        // Add a  table
        r.AddHeader("Add a table", 1);
        r.AddTable(FlowDocHelper.GetTableDataNumeric(5, 5));

        // Add a text block
        r.AddHeader("Add XAML textblock", 1);
        r.AddHeader("From paragraph", 2);
        r.AddXamlTextblock($"<Paragraph>{FlowDocHelper.MassTextTags}</Paragraph><Paragraph>Test test test</Paragraph>", "StandardWithIndent");

        r.AddHeader("From FlowDocument", 2);
        r.AddXamlTextblock(string.Format("<FlowDocument><Paragraph>{0}</Paragraph><Paragraph>{0}</Paragraph></FlowDocument>", FlowDocHelper.MassTextTags), "StandardWithIndent");

        // Add an image
        r.AddHeader("Add an image", 2);
        r.AddParagraph(FlowDocHelper.MassText);
        r.AddImage(@"Resources\testimage.png");
        r.AddParagraph(FlowDocHelper.MassText);

        // Add a pagebreak
        r.AddPageBreak();

        // Add a figure
        r.AddHeader("Add a figure", 2);
        r.AddParagraph(FlowDocHelper.MassText);
        r.AddFigure(@"Resources\testimage.png", "Image title1212");
        r.AddParagraph(FlowDocHelper.MassText);

        // Now build the report
        r.BuildReport();

        // Now save the report as PDF
        r.SaveAsPdf(fileName);

        //Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }



    private void CreateReport(TypographySettingsService typoService, string fileName)
    {

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        typoService.DocumentMetaData.CurrentLanguage = "de";

        var r = new ReportBase(typoService, _i18N);

        // Act
        r.AddTitle("Resx:Simulation.Wpf.ReportTitle");

        r.AddTitle("{Resx:Simulation.Wpf.ReportTitle}<LineBreak />Test");

        r.AddTitle2("ReportTitle2");


        const string contentFile = @"pack://siteOfOrigin:,,,/Resources/Content/SimulationMethodDescription.txt";

        r.AddHeader("", 1);
        r.AddParagraph(FlowDocHelper.MassText);
        r.AddNumberedList(FlowDocHelper.GetListData());


        r.AddHeader("Alignment table cells", 1);
        r.AddTable(FlowDocHelper.GetTableDataNumeric(5, 5));

        r.AddHeader("Add XAML textblock", 1);
        r.AddHeader("From paragraph", 2);

        r.AddXamlTextblock(
            $"<Paragraph>{FlowDocHelper.MassTextTags}</Paragraph><Paragraph>Test test test</Paragraph>", "StandardWithIndent");

        r.AddHeader("From FlowDocument", 2);

        r.AddXamlTextblock(string.Format("<FlowDocument><Paragraph>{0}</Paragraph><Paragraph>{0}</Paragraph></FlowDocument>", FlowDocHelper.MassTextTags), "StandardWithIndent");


        r.AddHeader("Resx:Simulation.Wpf.ReportTitle", 1);

        r.AddTable(FlowDocHelper.GetTableData(5, 4), TableTypes.NormalNoHeaderUnbordered);

        r.AddParagraph(FlowDocHelper.MassTextShort);

        r.AddTable(FlowDocHelper.GetTableData(5, 4), TableTypes.Normal);

        r.AddTextblock(WpfHelper.LoadPlainTextFile(contentFile), "StandardWithIndent");


        r.AddHeader("Header Level 1", 1);
        r.AddHeader("Kumulative kumulative test Verteilungsfunktion der Simulationsergebnisse", 2);


        r.AddTextblock(FlowDocHelper.MassTextWithImages2);

        //r.AddParagraph(FlowDocHelper.MassTextWithImages1);

        r.AddParagraph(FlowDocHelper.MassText);

        r.AddImage(@"Resources\testimage.png");
        r.AddHeader("Header Level 2", 2);
        r.AddParagraph(FlowDocHelper.MassText);

        r.AddFigure(@"Resources\testimage.png", "Image title1212");


        r.AddHeader("Header Level 2", 2);


        r.AddParagraph(FlowDocHelper.MassTextWithImages);

        r.AddHeader("Header Level 1", 1);

        r.AddHeader("Header Level 2", 2);

        r.AddParagraph(FlowDocHelper.MassText);

        var textblock =
            $"<P>{FlowDocHelper.MassText}</P>{FlowDocHelper.ImageTag}<P>{FlowDocHelper.MassTextTags}</P>{FlowDocHelper.ImageTagWithTitle}<P>{FlowDocHelper.MassText}</P>";

        r.AddTextblock(textblock);


        r.AddHeader("Header Level 2", 2);

        r.AddHeader("Header Level 3", 3);
        r.AddParagraph(FlowDocHelper.MassText);

        r.AddHeader("Header Level 3", 3);
        r.AddParagraph(FlowDocHelper.MassText);

        r.AddPageBreak();
        r.AddHeader("Header Level 1 with page break before", 1);

        r.AddParagraph(FlowDocHelper.MassText);

        r.AddSubHeader("Table XY");
        r.AddTable(FlowDocHelper.GetTableData(4, 2));

        r.AddSubHeader("Table YZ");
        r.AddTable(FlowDocHelper.GetTableData(6, 3));

        r.AddHeader("Header Level 1 with page break before", 1);

        r.AddParagraph(FlowDocHelper.MassText);

        r.AddTable(FlowDocHelper.GetTableData(4, 2), TableTypes.ExtraSmall);

        r.AddHeader("Disclaimer", 1);
        r.AddTextblock(FlowDocHelper.Disclaimer);

        r.BuildReport();
        r.SaveAsXps(fileName);
        r.Dispose();
    }


    [Test]
    public void TestReportBase_ShortCuts()
    {
        //Arrange

        var typoService = new TypographySettingsService
        {
            MaxImageHeight = 300,
            DocumentMetaData = new TypoMetaData()
            {
                LogoPath = TestHelper.TestLogoImage,
                FooterText = "Bodoconsult GmbH",
                CurrentLanguage = "de"
            },
            FigureCounterPrefix = "Abb.",
            ShowFigureCounter = true,
        };

        var r = new ReportBase(typoService);
        var fileName = Path.Combine(_tempPath, "TestReportBase_ShortCuts.xps");

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }


        // Act
        r.AddTitle("ReportTitle");

        r.AddHeader("Header Level 1", 1);
        r.AddHeader("Header Level 2", 2);

        r.AddParagraph(FlowDocHelper.MassText);

        r.AddImage(@"Resources\testimage.png");
        r.AddHeader("Header Level 2", 2);
        r.AddParagraph(FlowDocHelper.MassText);

        r.AddFigure(@"Resources\testimage.png", "Image title1212");


        r.AddHeader("Header Level 2", 2);


        r.AddParagraph(FlowDocHelper.MassTextWithImages);

        r.AddHeader("Header Level 1", 1);

        r.AddHeader("Header Level 2", 2);

        r.AddParagraph(FlowDocHelper.MassText);

        var textblock =
            $"<P>{FlowDocHelper.MassText}</P>{FlowDocHelper.ImageTag}<P>{FlowDocHelper.MassTextTags}</P>{FlowDocHelper.ImageTagWithTitle}<P>{FlowDocHelper.MassText}</P>";

        r.AddTextblock(textblock);


        r.AddHeader("Header Level 2", 2);

        r.AddHeader("Header Level 3", 3);
        r.AddParagraph(FlowDocHelper.MassText);

        r.AddHeader("Header Level 3", 3);
        r.AddParagraph(FlowDocHelper.MassText);

        r.AddPageBreak();
        r.AddHeader("Header Level 1 with page break before", 1);

        r.AddParagraph(FlowDocHelper.MassText);

        r.AddTable(FlowDocHelper.GetTableData(4, 2));


        r.AddHeader("Header Level 1 with page break before", 1);

        r.AddParagraph(FlowDocHelper.MassText);

        r.AddTable(FlowDocHelper.GetTableData(4, 2), TableTypes.ExtraSmall);

        r.AddHeader("Disclaimer", 1);
        r.AddTextblock(FlowDocHelper.Disclaimer);

        r.BuildReport();
        r.SaveAsXps(fileName);
        r.Dispose();



        //Assert
        Assert.That(File.Exists(fileName));

        FileSystemHelper.RunInDebugMode(fileName);
    }

}
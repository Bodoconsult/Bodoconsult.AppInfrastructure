// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Bodoconsult.App.Abstractions.Typography;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Wpf.Documents.General;
using Bodoconsult.App.Wpf.Documents.Services;
using Bodoconsult.App.Wpf.Documents.Test.Helpers;
using Bodoconsult.App.Wpf.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Documents.Test
{
    [TestFixture]
    [RequiresThread(ApartmentState.STA)]
    public class FlowDocumentServiceTests
    {
        private readonly string _tempPath = Path.GetTempPath();

        private readonly string _testDataPath = TestHelper.TestDataPath;

        private static string _chartXamlPath;

        public FlowDocumentServiceTests()
        {
            var fi = new FileInfo(Assembly.GetExecutingAssembly().Location);


            _chartXamlPath = Path.Combine(_testDataPath, "3DChart.xaml");

        }

        [Test]
        public void SaveAsXps_NormalDocument_FileIsCreated()
        {

            // Arrange
            var fileName = Path.Combine(_tempPath, "FlowDocService1.xps");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Act
            var fds = GetFlowDocumentService();

            //var stream = File.OpenRead(@"c:\temp\charts3d.jpg");
            //fds.AddImage(stream);
            //stream.Close();

            fds.SaveAsXps(fileName);

            // Assert
            Assert.That(File.Exists(fileName));

            FileSystemHelper.RunInDebugMode(fileName);
        }


        [Test]
        public void SaveAsXps_DocumentFromTypography_FileIsCreated()
        {

            // Arrange
            var fileName = Path.Combine(_tempPath, "FlowDocService2.xps");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }


            // Act
            var fds = GetFlowDocumentServiceWithTypography();

            //var stream = File.OpenRead(@"c:\temp\charts3d.jpg");
            //fds.AddImage(stream);
            //stream.Close();

            fds.SaveAsXps(fileName);

            // Assert
            Assert.That(File.Exists(fileName));

            FileSystemHelper.RunInDebugMode(fileName);
        }


        [Test]
        public void SaveAsXps_DocumentFromTypographyCompact_FileIsCreated()
        {

            // Arrange
            var fileName = Path.Combine(_tempPath, "FlowDocService2.xps");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var typoService = new TypographySettingsService();
            typoService.LoadCompactDefaults();
            typoService.Typography.FontName = "Times New Roman";
            typoService.Typography.HeadingFontName = "Times New Roman";
            typoService.Typography.TitleFontName = "Arial Black";

            // Act
            var fds = GetFlowDocumentServiceWithCompactTypography(typoService);

            //var stream = File.OpenRead(@"c:\temp\charts3d.jpg");
            //fds.AddImage(stream);
            //stream.Close();

            fds.SaveAsXps(fileName);



            // Assert
            Assert.That(File.Exists(fileName));

            FileSystemHelper.RunInDebugMode(fileName);
        }


        private FlowDocumentService GetFlowDocumentService()
        {

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


            var fds = new FlowDocumentService(typoService);
            GetReport(fds);

            return fds;
        }

        private static void GetReport(FlowDocumentService fds)
        {
            fds.AddSection();
            fds.AddTitle("Title for this test document");
            fds.AddTitle2("Subtitle for this test document");

            fds.AddHeader1("Add XAML Textblock");
            fds.AddHeader2("From paragraph");
            fds.AddXamlTextblock(
                $"<Paragraph>{FlowDocHelper.MassTextTags}</Paragraph><Paragraph>Test test test</Paragraph>");
            fds.AddHeader1("Add XAML Textblock");
            fds.AddHeader2("From paragraph");
            fds.AddXamlTextblock(
                $"<Paragraph>{FlowDocHelper.MassTextTags}</Paragraph><Paragraph>Test test test</Paragraph>");

            fds.AddTextBlock(
                $"<H2>Überschrift 2 A</H2><P>{FlowDocHelper.MassTextTags}</P><H2>Überschrift 2 B</H2><P>Test test test</P>");


            fds.AddHeader2("From flowdocument");
            fds.AddXamlTextblock(
                string.Format("<FlowDocument><Paragraph>{0}</Paragraph><Paragraph>{0}</Paragraph></FlowDocument>",
                    FlowDocHelper.MassTextTags));

            fds.AddTable(FlowDocHelper.GetTableData(24, 3));

            fds.AddHeader1("Canvas blubb Co; gjgj");
            //fds.AddRuler();

            fds.AddHeader2("Canvas from live object as figure");

            var canvas = (Canvas)WpfHelper.LoadElementFromXamlFile(_chartXamlPath);
            fds.AddFigure(canvas, 200, 200, "Canvas as figure from live object");

            fds.AddHeader2("Canvas from file");

            fds.AddCanvas(_chartXamlPath, 300,
                200);

            fds.AddHeader2("Canvas from live object");

            fds.Dispatcher.Invoke(() =>
            {
                canvas = (Canvas)WpfHelper.LoadElementFromXamlFile(_chartXamlPath);
            });



            fds.AddCanvas(canvas, 200, 200);

            fds.AddHeader2("Canvas from file as figure");

            fds.AddFigure(_chartXamlPath, 300,
                200, "Canvas as figure from file");


            //fds.AddHeader2("Image from stream");
            //var stream = File.OpenRead(@"c:\temp\charts3d.jpg");
            //fds.AddImage(stream);
            //stream.Close();

            fds.AddHeader2(FlowDocHelper.Header2);
            fds.AddParagraph(FlowDocHelper.MassText);

            fds.AddParagraph(FlowDocHelper.MassTextTags);

            // paragraph with images
            fds.AddParagraph(FlowDocHelper.MassTextWithImages);

            // Add a longer text block with paragraphs

            var textblock =
                $"<P>{FlowDocHelper.MassText}</P>{FlowDocHelper.ImageTag}<P>{FlowDocHelper.MassTextTags}</P>{FlowDocHelper.ImageTagWithTitle}<P>{FlowDocHelper.MassText}</P>";

            fds.AddParagraph("Test Textblock Start");
            fds.AddTextBlock(textblock);
            fds.AddParagraph("Test Textblock Ende");

            fds.AddNumberedList(FlowDocHelper.GetListData(), TextMarkerStyle.Disc);
            fds.AddParagraph(FlowDocHelper.MassTextTags);
            fds.AddTable(FlowDocHelper.GetTableData(4, 2));

            fds.AddPageBreak();
            fds.AddParagraph(FlowDocHelper.MassText);

            fds.AddImage(@"Resources\testimage.png");
            fds.AddHeader2(FlowDocHelper.Header2);
            fds.AddParagraph(FlowDocHelper.MassText);
            fds.AddTable(FlowDocHelper.GetTableData(10, 6), TableTypes.Small);
            fds.AddParagraph(FlowDocHelper.MassText);

            fds.AddPageBreak();
            fds.AddParagraph(FlowDocHelper.GetInlineSamples());
            fds.AddParagraph(FlowDocHelper.MassText);

            fds.AddHeader2(FlowDocHelper.Header2);
            fds.AddParagraph(FlowDocHelper.MassText);
            fds.AddTable(FlowDocHelper.GetTableData(10, 6), TableTypes.SmallUnbordered);
            fds.AddParagraph(FlowDocHelper.MassText);

            fds.AddHeader1(FlowDocHelper.Header1);
            //fds.AddRuler();
            fds.AddParagraph(FlowDocHelper.MassText);
            fds.AddFigure(@"Resources\testimage.png", "Quantum ipsum lorem delete");
            fds.AddParagraph(FlowDocHelper.MassText);
            fds.AddParagraph(FlowDocHelper.MassText);

            fds.AddParagraph("FigureCount: ??FigureCount??");
            fds.AddParagraph("FigureCountP1 (following figure): ??FigureCountP1??");
            fds.AddParagraph("FigureCountM1 (previous figure): ??FigureCountM1??");


            fds.AddTextBlock("FigureCount: ??FigureCount??");

            fds.AddDefaultFooterAndHeader();
        }


        private FlowDocumentService GetFlowDocumentServiceWithTypography()
        {

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

            var fds = new FlowDocumentService(typoService);
            GetReport(fds);
            return fds;
        }


        private FlowDocumentService GetFlowDocumentServiceWithCompactTypography(
            TypographySettingsService typoService)
        {

            var typo = new CompactTypographyPageHeader("Times New Roman", "Times New Roman", "Arial Black");

            typoService.LoadTypography(typo);
            typoService.MaxImageHeight = 300;
            typoService.DocumentMetaData = new TypoMetaData()
            {
                LogoPath = TestHelper.TestLogoImage,
                FooterText = "Bodoconsult GmbH",
                CurrentLanguage = "de"
            };
            typoService.FigureCounterPrefix = "Abb.";
            typoService.ShowFigureCounter = true;

            var fds = new FlowDocumentService(typoService);
            GetReport(fds);
            return fds;
        }


        [CancelAfter(5000000)]
        [Test]
        public void SaveAsPdf_DocumentCompact_FileIsCreated()
        {

            // Arrange
            var fileName = Path.Combine(_tempPath, "FlowDocService2.pdf");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Act
            var fds = GetFlowDocumentService();

            fds.SaveAsPdf(fileName);


            // Assert
            Assert.That(File.Exists(fileName));

            FileSystemHelper.RunInDebugMode(fileName);
        }



        [Test]
        public void SaveAsPdfDocumentCompact_FileIsCreated()
        {

            // Arrange
            var fileName = Path.Combine(_tempPath, "FlowDocService2.pdf");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Act
            var fds = GetFlowDocumentService();
            fds.SaveAsPdf(fileName);

            // Assert
            Assert.That(File.Exists(fileName));

            FileSystemHelper.RunInDebugMode(fileName);
        }

        [CancelAfter(5000000)]
        [Test]
        public void SaveAsPdf_Demo_FileIsCreated()
        {

            // Arrange
            var fileName = Path.Combine(_tempPath, "Demo.pdf");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Act

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
                ShowFigureCounter = true
            };

            // Now get the flow document service with the document contained
            var fds = new FlowDocumentService(typoService);

            // Fill the document now
            fds.AddSection();
            fds.AddTitle("Title for this test document");
            fds.AddTitle2("Subtitle for this test document");

            // Simple paragraphs
            fds.AddParagraph(FlowDocHelper.MassText);
            fds.AddParagraphCentered(FlowDocHelper.MassText);
            fds.AddParagraphRight(FlowDocHelper.MassText);

            // Paragraphs with tags in the content
            fds.AddParagraph(FlowDocHelper.MassTextTags);

            // Add XAML markup
            fds.AddHeader1("Add Textblock");
            fds.AddHeader2("From XAML markup");
            fds.AddXamlTextblock($"<Paragraph>{FlowDocHelper.MassTextTags}</Paragraph><Paragraph>Test test test</Paragraph>");

            // Add HTML markup
            fds.AddHeader2("From HTML markup");
            fds.AddTextBlock($"<H2>Heading 2 A</H2><P>{FlowDocHelper.MassTextTags}</P><H2>Heading 2 B</H2><P>Test test test</P>");

            // Add a table
            fds.AddHeader1("Add table");
            fds.AddTable(FlowDocHelper.GetTableData(24, 3));

            // Add a figure
            fds.AddHeader1("Add a figure");

            fds.AddHeader2("Add a figure from file");
            fds.AddFigure(TestHelper.TestChartImage, "Image title", 300, 200);

            fds.AddHeader2("Add a figure from XAML file");
            fds.AddFigure(_chartXamlPath, 300, 200, "Canvas as figure from file");

            // Add an image
            fds.AddHeader1("Add an image");
            fds.AddHeader2("Add an image from a file");
            fds.AddImage(TestHelper.TestChartImage);

            fds.AddHeader2("Add an image from resources");
            fds.AddImage(@"Resources\testimage.png");

            // Add a default header and footer to the document
            fds.AddDefaultFooterAndHeader();

            // Save as PDF
            fds.SaveAsPdf(fileName);

            // Assert
            Assert.That(File.Exists(fileName));

            FileSystemHelper.RunInDebugMode(fileName);
        }
    }
}

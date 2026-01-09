// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Wpf.Documents.Helpers;
using Bodoconsult.App.Wpf.Documents.Interfaces;
using Bodoconsult.App.Wpf.Documents.Paginators;
using Bodoconsult.App.Wpf.Documents.Services;
using Bodoconsult.Pdf.Helpers;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Renderer;
using PdfSharp.Xps;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using Section = System.Windows.Documents.Section;
using Size = System.Windows.Size;
using Thickness = System.Windows.Thickness;

namespace Bodoconsult.App.Wpf.Documents.Renderer;

/// <summary>
/// Render a <see cref="Document"/> to a PDF file
/// </summary>
public class WpfTextDocumentRenderer : BaseDocumentRenderer
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="document">Document to render</param>
    /// <param name="textRendererElementFactory">Current factory for text renderer elements</param>
    public WpfTextDocumentRenderer(Document document, ITextRendererElementFactory textRendererElementFactory) : base(document)
    {
        //var metaData = document.DocumentMetaData;

        LoadPageSettings();

        WpfTextRendererElementFactory = (IWpfTextRendererElementFactory)textRendererElementFactory;

        Dispatcher = Application.Current.Dispatcher;
        Dispatcher.Invoke(() =>
        {
            WpfDocumentToc = new FlowDocument();
            WpfDocumentContent = new FlowDocument();
            WpfDocument = WpfDocumentToc;
        });
    }

    private void LoadPageSettings()
    {
        // Load page settings
        var style = (PageStyleBase)Styleset.FindStyle("DocumentStyle");
        PageSettings.PageSize = new Size(MeasurementHelper.GetDiuFromCm(style.PaperFormat.Size.Width), MeasurementHelper.GetDiuFromCm(style.PaperFormat.Size.Height));
        PageSettings.Margins = new Thickness(
            MeasurementHelper.GetDiuFromCm(style.Margins.Left),
            MeasurementHelper.GetDiuFromCm(style.Margins.Left),
            MeasurementHelper.GetDiuFromCm(style.Margins.Left),
            MeasurementHelper.GetDiuFromCm(style.Margins.Left));
        PageSettings.FooterHeight = MeasurementHelper.GetDiuFromCm(style.FooterHeight);
        PageSettings.FooterMarginTop = MeasurementHelper.GetDiuFromCm(style.FooterMarginTop);
        PageSettings.HeaderHeight = MeasurementHelper.GetDiuFromCm(style.HeaderHeight);
        PageSettings.HeaderMarginBottom = MeasurementHelper.GetDiuFromCm(style.HeaderMarginBottom);
        PageSettings.DrawFooterDelegate = DefaultFooter;

        if (!string.IsNullOrEmpty(Document.DocumentMetaData.LogoPath))
        {
            PageSettings.DrawHeaderDelegate = DefaultHeader;
        }

        var footerStyle = (ParagraphStyleBase)Styleset.FindStyle("FooterStyle");
        PageSettings.FooterHeight = MeasurementHelper.GetDiuFromCm(style.FooterHeight);
        PageSettings.FooterFontName = footerStyle.FontName;
        PageSettings.FooterFontSize = MeasurementHelper.GetDiuFromPoint(footerStyle.FontSize);
        PageSettings.FooterPageText = Document.DocumentMetaData.FooterText;

        var headerStyle = (ParagraphStyleBase)Styleset.FindStyle("HeaderStyle");
        PageSettings.HeaderHeight = MeasurementHelper.GetDiuFromCm(style.HeaderHeight);
        PageSettings.HeaderFontName = headerStyle.FontName;
        PageSettings.HeaderFontSize = MeasurementHelper.GetDiuFromPoint(headerStyle.FontSize);

        PageSettings.DocumentMetaData = Document.DocumentMetaData;
        PageSettings.FooterPageText = $"{Document.DocumentMetaData.Company}\t{Document.DocumentMetaData.PageNumberPrefix}";
    }

    /// <summary>
    /// Current dispatcher
    /// </summary>
    public Dispatcher Dispatcher { get; private set; }

    /// <summary>
    /// Current page settings
    /// </summary>
    public WpfDocumentPageSettingsService PageSettings { get; } = new();

    /// <summary>
    /// The current PDF document part without TOC, TOE, TOF and TOT
    /// </summary>
    public FlowDocument WpfDocument { get; set; }

    /// <summary>
    /// The current PDF document part containing TOC, TOE, TOF and TOT
    /// </summary>
    public FlowDocument WpfDocumentToc { get; private set; }

    /// <summary>
    /// The current PDF document part without TOC, TOE, TOF and TOT
    /// </summary>
    public FlowDocument WpfDocumentContent { get; private set; }

    /// <summary>
    /// Current document section for adding content
    /// </summary>
    public Section CurrentSection { get; set; }

    /// <summary>
    /// Current styleset
    /// </summary>
    public ResourceDictionary StyleSet { get; } = new();

    /// <summary>
    /// Current text renderer element factory
    /// </summary>
    public IWpfTextRendererElementFactory WpfTextRendererElementFactory { get; protected set; }

    /// <summary>
    /// Render the document
    /// </summary>
    public override void RenderIt()
    {
        var rendererElement = WpfTextRendererElementFactory.CreateInstanceWpf(Document);
        rendererElement.RenderIt(this);
    }

    /// <summary>
    /// Save the rendered document as file
    /// </summary>
    /// <param name="fileName">Full file path. Existing file will be overwritten</param>
    public override void SaveAsFile(string fileName)
    {

        var fi = new FileInfo(fileName);
        var pureName = fi.Name.Replace(fi.Extension, string.Empty);

        var path1 = Path.Combine(fi.DirectoryName, $"{pureName}_TOC.pdf");
        var path2 = Path.Combine(fi.DirectoryName, $"{pureName}_CON.pdf");

        SaveAsPdf(WpfDocumentToc, path1, PageSettings.TocPageNumberFormat);
        SaveAsPdf(WpfDocumentContent, path2, PageSettings.ContentPageNumberFormat);

        //FileSystemHelper.RunInDebugMode(path1);
        //FileSystemHelper.RunInDebugMode(path2);

        PdfHelper.MergePdfs(fileName, [path1, path2]);

        FileSystemHelper.RunInDebugMode(fileName);
    }

    private void SaveAsPdf(FlowDocument wpfDocument, string path, PageNumberFormatEnum pageNumberFormat)
    {
        Dispatcher.Invoke(() =>
        {
            var lMemoryStream = new MemoryStream();
            using (var container = Package.Open(lMemoryStream, FileMode.Create))
            {
                using (var xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
                {
                    var rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
                    rsm.SaveAsXaml(new HeaderFooterPaginator(wpfDocument, PageSettings, Dispatcher, pageNumberFormat));

                    //rsm.SaveAsXaml(((IDocumentPaginatorSource)wpfDocument).DocumentPaginator);
                    rsm.Commit();
                }
            }

            var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
            XpsConverter.Convert(pdfXpsDoc, path, 0);
        });
    }

    /// <summary>
    /// Add a footer with page numbering
    /// </summary>
    /// <param name="context">Current drawing context</param>
    /// <param name="area">The available area for the section to draw</param>
    /// <param name="page">The page number (starting with 0) to print in</param>
    /// <param name="dpi">The dpi number to use</param>
    /// <param name="pageNumberFormat">Page number format</param>
    private void DefaultFooter(DrawingContext context, Rect area, int page, double dpi, PageNumberFormatEnum pageNumberFormat)
    {
        if (string.IsNullOrEmpty(Document.DocumentMetaData.FooterTemplate))
        {
            return;
        }

        var style = (FooterStyle)Styleset.FindStyle("FooterStyle");
        var sections = Document.DocumentMetaData.FooterTemplate.ToLowerInvariant().Split('|');

        var margin = 25;

        Dispatcher.Invoke(() =>
        {
            // Draw left element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, Document.DocumentMetaData, sections[0], 0, dpi, false, page, PageNumberFormatEnum.Decimal, style.FontName, style.FontSize, margin);

            // Draw middle element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, Document.DocumentMetaData, sections[1], 1, dpi, false, page, PageNumberFormatEnum.Decimal, style.FontName, style.FontSize, margin);

            // Draw right element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, Document.DocumentMetaData, sections[2], 2, dpi, false, page, PageNumberFormatEnum.Decimal, style.FontName, style.FontSize, margin);
        });
    }




    /// <summary>
    /// Add a header with a logo on the rightend side
    /// </summary>
    /// <param name="context">Current drawing context</param>
    /// <param name="area">The available area for the section to draw</param>
    /// <param name="page">The page number (starting with 0) to print in</param>
    /// <param name="dpi">The dpi number to use</param>
    /// <param name="pageNumberFormat">Page number format</param>
    private void DefaultHeader(DrawingContext context, Rect area, int page, double dpi, PageNumberFormatEnum pageNumberFormat)
    {
        if (string.IsNullOrEmpty(Document.DocumentMetaData.HeaderTemplate))
        {
            return;
        }

        var style = (HeaderStyle)Styleset.FindStyle("HeaderStyle");

        var sections = Document.DocumentMetaData.HeaderTemplate.ToLowerInvariant().Split('|');

        var margin = 25;

        Dispatcher.Invoke(() =>
        {
            // Draw left element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, Document.DocumentMetaData, sections[0], 0, dpi, true, page, pageNumberFormat, style.FontName, style.FontSize, margin);

            // Draw middle element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, Document.DocumentMetaData, sections[1], 1, dpi, true, page, pageNumberFormat, style.FontName, style.FontSize, margin);

            // Draw right element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, Document.DocumentMetaData, sections[2], 2, dpi, true, page, pageNumberFormat, style.FontName, style.FontSize, margin);
        });
    }
}
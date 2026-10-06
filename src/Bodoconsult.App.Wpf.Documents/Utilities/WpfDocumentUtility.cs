// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Documents.Paginators;
using Bodoconsult.App.Wpf.Documents.Services;
using Thickness = System.Windows.Thickness;

namespace Bodoconsult.App.Wpf.Documents.Utilities;

/// <summary>
/// Utilities for flow documents or fixed documents in WPF
/// </summary>
public static class WpfDocumentUtility
{
    /// <summary>
    /// File formats the document can be exported into
    /// </summary>
    public enum ExportFormat
    {
        /// <summary>
        /// Export file as XAML
        /// </summary>
        Xaml,

        /// <summary>
        /// Export file as RTF
        /// </summary>
        Rtf,

        /// <summary>
        /// Export file as XAML package (may include binary Data of images ie.)
        /// </summary>
        XamlPackage,

        /// <summary>
        /// Export as plain text file
        /// </summary>
        Text
    }


    /// <summary>
    /// Print a flow document
    /// </summary>
    /// <param name="document">flow document to print</param>
    public static void PrintFlowDocument(FlowDocument document)
    {
        var dialog = new PrintDialog();

        if (dialog.ShowDialog() == false)
        {
            return;
        }

        var oldPageWidth = document.PageWidth;
        var oldPageHeight = document.PageHeight;
        var thickness = document.PagePadding;
        var colWidth = document.ColumnWidth;
            
        document.PageWidth = dialog.PrintableAreaWidth;
        document.PageHeight = dialog.PrintableAreaHeight;
        document.PagePadding = new Thickness(0.3 * document.PageWidth,
            0.15 * document.PageHeight,
            0.2 * document.PageWidth,
            0.15 * document.PageHeight);
        document.ColumnWidth = dialog.PrintableAreaWidth;

        var definition = new TypographySettingsService
        {
            FooterHeight = 25,
            DrawFooterDelegate = Footer
        };
        dialog.PrintDocument(new HeaderFooterPaginator(document, definition, document.Dispatcher, PageNumberFormatEnum.Decimal), "Print document");

        //var paginator = document as IDocumentPaginatorSource;
        //dialog.PrintDocument(paginator.DocumentPaginator, "Print document");

        document.PageWidth = oldPageWidth;
        document.PageHeight = oldPageHeight;
        document.PagePadding = thickness;
        document.ColumnWidth = colWidth;
    }

    private static void Footer(DrawingContext context, Rect bounds, int pageNr, double dpi, PageNumberFormatEnum pageNumberFormat)
    {
        // Create the initial formatted text string.
        var formattedText = new FormattedText(
            $"Seite {pageNr + 1}",
            CultureInfo.GetCultureInfo("en-us"),
            FlowDirection.LeftToRight,
            new Typeface("Verdana"),
            10,
            Brushes.Black, dpi);

        //// Set a maximum width and height. If the text overflows these values, an ellipsis "..." appears.
        //formattedText.MaxTextWidth = 300;
        //formattedText.MaxTextHeight = 240;

        //// Use a larger font size beginning at the first (zero-based) character and continuing for 5 characters.
        //// The font size is calculated in terms of points -- not as device-independent pixels.
        //formattedText.SetFontSize(36 * (96.0 / 72.0), 0, 5);

        //// Use a Bold font weight beginning at the 6th character and continuing for 11 characters.
        //formattedText.SetFontWeight(FontWeights.Bold, 6, 11);

        //// Use a linear gradient brush beginning at the 6th character and continuing for 11 characters.
        //formattedText.SetForegroundBrush(
        //                        new LinearGradientBrush(
        //                        Colors.Orange,
        //                        Colors.Teal,
        //                        90.0),
        //                        6, 11);

        //// Use an Italic font style beginning at the 28th character and continuing for 28 characters.
        //formattedText.SetFontStyle(FontStyles.Italic, 28, 28);

        // Draw the formatted text string to the DrawingContext of the control.
        context.DrawText(formattedText, new Point(bounds.X + bounds.Width - formattedText.Width,
            bounds.Y + bounds.Height - formattedText.Height));

    }

    ///// <summary>
    ///// Save a document as XPS file
    ///// </summary>
    ///// <param name="document">flow document to save</param>
    ///// <param name="path">path to save the document</param>
    //public void SaveAsXps(FlowDocument document, string path)
    //{

    //    if (File.Exists(path)) File.Delete(path);

    //    XpsDocument xpsDocument = new XpsDocument();

    // siehe Seite 475 .NET WPF-Buch

    //}


    /// <summary>
    /// Save a document as XAML file
    /// </summary>
    /// <param name="document">flow document to save</param>
    /// <param name="path">path to save the document</param>
    public static void SaveDocumentAsXaml(FlowDocument document, string path)
    {
        var xamlFile = File.OpenWrite(path);
        XamlWriter.Save(document, xamlFile);
        xamlFile.Close();
    }

    /// <summary>
    /// Save a document's Content as file in certain format. Valid formats are Rtf, Xaml, XamlPackage
    /// </summary>
    /// <param name="document">flow document to save</param>
    /// <param name="path">path to save the document</param>
    /// <param name="format">format of the saved file</param>
    public static void SaveDocumentContentAsFile(FlowDocument document, string path, DataFormat format)
    {
        var content = new TextRange(document.ContentStart, document.ContentEnd);

        if (!content.CanSave(format.Name))
        {
            return;
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var stream = new MemoryStream();
        content.Save(stream, DataFormats.Rtf);
        using var fstream = File.OpenWrite(path);
        stream.WriteTo(fstream);
        fstream.Flush();
        fstream.Close();
    }

    /// <summary>
    /// Load a document's Content from a file in certain format. Valid formats are Rtf, Xaml, XamlPackage
    /// </summary>
    /// <param name="document">flow document to save</param>
    /// <param name="path">path to save the document</param>
    /// <param name="format">format of the saved file</param>
    public static void LoadDocumentFromFile(FlowDocument document, string path, DataFormat format)
    {
        var content = new TextRange(document.ContentStart, document.ContentEnd);

        if (!content.CanLoad(format.Name))
        {
            return;
        }

        using var stream = File.OpenRead(path);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        content.Load(ms, DataFormats.Rtf);
    }


    /// <summary>
    /// Save a <see cref="FlowDocument"/> as XPS file
    /// </summary>
    /// <param name="document">FlowDocument</param>
    /// <param name="path">Path to save the XPS file</param>
    public static void SaveDocumentAsXps(FlowDocument document, string path)
    {
        //ForceRenderFlowDocument(document);

        using var container = Package.Open(path, FileMode.Create);
        using var xpsDoc = new XpsDocument(container, CompressionOption.Maximum);
        var rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);

        var definition = new TypographySettingsService
        {
            FooterHeight = 25,
            DrawFooterDelegate = Footer
        };

        rsm.SaveAsXaml(new HeaderFooterPaginator(document, definition, document.Dispatcher, PageNumberFormatEnum.Decimal));
        rsm.Commit();
    }


//        private const string ForceRenderFlowDocumentXaml = @"<Window xmlns=""http://schemas.microsoft.com/netfx/2007/xaml/presentation""
//                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
//                 <FlowDocumentScrollViewer Name=""viewer""/>
//                 </Window>";

    //public static void ForceRenderFlowDocument(FlowDocument document)
    //{

    //    using (var reader = new XmlTextReader(new StringReader(ForceRenderFlowDocumentXaml)))
    //    {
    //        var window = XamlReader.Load(reader) as Window;
    //        if (window != null)
    //        {
    //            var viewer = LogicalTreeHelper.FindLogicalNode(window, "viewer") as FlowDocumentScrollViewer;
    //            viewer.Document = document;
    //            // Show the window way off-screen
    //            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //            window.Top = int.MaxValue;
    //            window.Left = int.MaxValue;
    //            window.ShowInTaskbar = false;
    //            window.Show();
    //            // Ensure that dispatcher has done the layout and render passes
    //            window.UpdateLayout(); 
    //            window.Dispatcher.Invoke(DispatcherPriority.Loaded, new Action(() => { }));

    //            window.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => { }));

    //            viewer.Document = null;
    //        }
    //        window.Close();
    //    }
    //}
}
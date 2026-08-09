// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Delegates;
using Bodoconsult.App.Wpf.Documents.General;
using Bodoconsult.App.Wpf.Documents.Services;
using PropertyChanged;

namespace Bodoconsult.App.Wpf.Documents.Reports;

/// <summary>
/// Base class for WPF reports based on FlowDocuments
/// </summary>
/// <example>
/// 
///             var pageDefinition = new PrintDefinition
///            {
///                    MaxImageHeight = 350,
///                LogoPath = @"C:\bodoconsult\Logos\logo.jpg",
///                FooterText = "Bodoconsult GmbH",
///                FigureCounterPrefix = "Abb.",
///                ShowFigureCounter = true,
///            };
///
///            var r = new ReportBase(pageDefinition);
///            const string fileName = @"C:\temp\TestReportBase.xps";
///
///            if (File.Exists(fileName)) File.Delete(fileName);
///
///             //Generate content
///            r.AddTitle("ReportTitle");
///
///            r.AddHeader("Header Level 1", 1);
///
///            r.AddParagraph(FlowDocHelper.MassText);
///            r.AddImage(ImagePath = @"Resources\testimage.png");
///
///             // Generate report and save it as XPS file
///            r.BuildReport();
///           r.SaveAsXps(fileName);
///            r.Dispose();
/// 
/// </example>
[AddINotifyPropertyChangedInterface]
public class ReportBase : IDisposable
{

    #region ctor

    /// <summary>
    /// Default constructor for class
    /// </summary>
    public ReportBase()
    {
        BaseConstructor(new FlowDocumentService());
    }

    /// <summary>
    /// Constructor to provide customized page settings
    /// </summary>
    /// <param name="typographySettingsService">Current typo settings service to use</param>
    public ReportBase(TypographySettingsService typographySettingsService)
    {
        BaseConstructor(new FlowDocumentService(typographySettingsService));
    }

    /// <summary>
    /// Constructor to provide customized page settings and I18N
    /// </summary>
    /// <param name="typographySettingsService">Current typo settings service to use</param>
    /// <param name="i18N">Internationalisation</param>
    public ReportBase(TypographySettingsService typographySettingsService, II18N i18N)
    {
        BaseConstructor(new FlowDocumentService(typographySettingsService, i18N));
    }



    private void BaseConstructor(FlowDocumentService flowDocumentService)
    {
        Components = new List<IReportElement>();

        Labels = new Dictionary<string, string>();

        FlowDocumentService = flowDocumentService;

        FlowDocumentService.AddSection();

        if (FlowDocumentService.TypographySettingsService.DrawHeaderDelegate is null &&
            FlowDocumentService.TypographySettingsService.DrawFooterDelegate is null)
        {
            FlowDocumentService.AddDefaultFooterAndHeader();
        }
    }


    ///// <summary>
    ///// Load typographic Data for report printing. Page definition is not loaded here. Use <see cref="DocumentHelper.LoadFromTypography"/> to before instanciating <see cref="ReportBase"/> object.
    ///// </summary>
    ///// <param name="typography"></param>
    ///// <example>
    ///// var typo = new ElegantTypographyPageHeader();
    /////
    ///// var pageDefinition = DocumentHelper.LoadFromTypography(typo);
    ///// 
    ///// //Other print settings as needed
    ///// pageDefinition.MaxImageHeight = 300;
    ///// pageDefinition.LogoPath = @"C:\bodoconsult\Logos\logo.jpg";
    ///// ...
    ///// 
    ///// var r = new ReportBase(pageDefinition);
    ///// r.LoadTypographicData(typo);
    ///// 
    ///// // Define report
    ///// ...
    ///// 
    ///// // Export report as XPS
    ///// r.BuildReport();
    ///// r.SaveAsXps(fileName);            
    ///// r.Dispose();
    ///// 
    ///// </example>
    //public void LoadTypographicData(ITypography typography)
    //{
    //    new TypographySettingsService().LoadTypography(typography);
    //}



    #endregion

    #region Status message

    /// <summary>
    /// Sends a status message to the UI
    /// </summary>
    public event SendStatusMessageDelegate StatusMessage;

    /// <summary>
    /// Fires the StatusMessage event to send a status message to the UI
    /// </summary>
    /// <param name="message">message to send</param>
    protected void OnSendStatus(string message)
    {
        var x = StatusMessage;
        if (x is null) return;
        x(message);
    }

    #endregion



    #region Report components and their processing

    /// <summary>
    /// All components of a WPF report
    /// </summary>
    public IList<IReportElement> Components { get; set; }

    /// <summary>
    /// Render all components of a report
    /// </summary>
    public void BuildReport()
    {
        OnSendStatus("Build report...");
        foreach (var component in Components)
        {
            Debug.Print(component.GetType().FullName);
            component.RenderIt(FlowDocumentService);
        }

    }


    #endregion



    /// <summary>
    /// Counts the images added to the report
    /// </summary>
    public int ImageCounter { get; internal set; }

    /// <summary>
    /// Contains strings for language dependent labeling
    /// </summary>
    public Dictionary<string, string> Labels { get; set; }


    /// <summary>
    /// Current <see cref="FlowDocumentService"/> with the <see cref="FlowDocument"/> contained
    /// </summary>
    public FlowDocumentService FlowDocumentService { get; set; }


    /// <summary>
    /// Dispose the ReportBase object cleanly
    /// </summary>
    public void Dispose()
    {
        FlowDocumentService = null;
    }


    /// <summary>
    /// Get the <see cref="FlowDocument"/> containing the report
    /// </summary>
    public FlowDocument Document => FlowDocumentService.Document;


    /// <summary>
    /// Save document as XPS file
    /// </summary>
    /// <param name="xpsFileName"></param>
    public void SaveAsXps(string xpsFileName)
    {
        OnSendStatus("Save report as XPS...");
        FlowDocumentService.SaveAsXps(xpsFileName);
    }
    /// <summary>
    /// Save document as PDF file
    /// </summary>
    /// <param name="pdfFileName"></param>
    public void SaveAsPdf(string pdfFileName)
    {
        OnSendStatus("Save report as PDF...");
        FlowDocumentService.SaveAsPdf(pdfFileName);
    }

    /// <summary>
    /// Save document as PDF file
    /// </summary>
    public MemoryStream SaveAsStream()
    {
        return FlowDocumentService.SaveAsStream();
    }

    /// <summary>
    /// Add a title to the report
    /// </summary>
    /// <param name="title">Title to print to report</param>
    public void AddTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return;
        }
        Components.Add(new ReportTitleElement { Content = title });
    }


    /// <summary>
    /// Add a title to the report
    /// </summary>
    /// <param name="title">Title to print to report</param>
    /// <param name="array">Replaces one or more format items in a specified string with the string representation of a specified object.</param>
    public void AddTitle(string title, params object[] array)
    {
        if (string.IsNullOrEmpty(title))
        {
            return;
        }
        Components.Add(new ReportTitleElement { Content = string.Format(title, array) });
    }


    /// <summary>
    /// Add a title level 2 to the report
    /// </summary>
    /// <param name="title">Title to print to report</param>
    public void AddTitle2(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return;
        }
        Components.Add(new ReportTitle2Element { Content = title });
    }

    /// <summary>
    /// Add a title level 2 to the report
    /// </summary>
    /// <param name="title">Title to print to report</param>
    /// <param name="array">Replaces one or more format items in a specified string with the string representation of a specified object.</param>
    public void AddTitle2(string title, params object[] array)
    {
        if (string.IsNullOrEmpty(title)) return;
        Components.Add(new ReportTitle2Element { Content = string.Format(title, array) });
    }


    /// <summary>
    /// Add a header to the report
    /// </summary>
    /// <param name="content"></param>
    /// <param name="level">1=top to 5=lowest, default 1</param>
    /// <param name="noCount">If noCount is true, the header will not be included in the header counting. Only header levels 1 to 3.</param>
    public void AddHeader(string content, int level, bool noCount = false)
    {
        if (string.IsNullOrEmpty(content)) return;
        Components.Add(new ReportHeaderElement
        {
            Content = content,
            Level = level,
            NoCount = noCount
        });
    }

    /// <summary>
    /// Add a header to the report
    /// </summary>
    /// <param name="content"></param>
    /// <param name="level">1=top to 5=lowest, default 1</param>
    /// <param name="noCount">If noCount is true, the header will not be included in the header counting. Only header levels 1 to 3.</param>
    /// <param name="array">Replaces one or more format items in a specified string with the string representation of a specified object.</param>
    public void AddHeader(string content, int level, bool noCount = false, params object[] array)
    {
        if (string.IsNullOrEmpty(content)) return;
        Components.Add(new ReportHeaderElement
        {
            Content = string.Format(content, array),
            Level = level,
            NoCount = noCount
        });
    }

    /// <summary>
    /// Add a standard paragraph
    /// </summary>
    /// <param name="content"></param>
    public void AddParagraph(string content)
    {
        if (string.IsNullOrEmpty(content)) return;
        Components.Add(new ReportParagraphElement
        {
            Content = content
        });
    }

    /// <summary>
    /// Add a centered paragraph
    /// </summary>
    /// <param name="content"></param>
    public void AddParagraphCentered(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        Components.Add(new ReportParagraphElement
        {
            Content = content,
            TextAlignment = TextAlignment.Center
        });
    }

    /// <summary>
    /// Add a standard paragraph
    /// </summary>
    /// <param name="content"></param>
    public void AddParagraphRight(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        Components.Add(new ReportParagraphElement
        {
            Content = content,
            TextAlignment = TextAlignment.Right
        });
    }


    /// <summary>
    /// Add a standard paragraph
    /// </summary>
    /// <param name="content"></param>
    /// <param name="fontSize">Logical font size of the paragraph. Default is Regular. See <see cref="FontSize"/></param>
    public void AddParagraph(string content, FontSize fontSize)
    {
        if (string.IsNullOrEmpty(content)) return;
        Components.Add(new ReportParagraphElement
        {
            Content = content,
            FontSize = fontSize
        });
    }

    /// <summary>
    /// Add a standard paragraph
    /// </summary>
    /// <param name="content"></param>
    /// <param name="fontSize">Logical font size of the paragraph. Default is Regular. See <see cref="FontSize"/></param>
    public void AddParagraphCentered(string content, FontSize fontSize)
    {
        if (string.IsNullOrEmpty(content)) return;
        Components.Add(new ReportParagraphElement
        {
            Content = content,
            FontSize = fontSize,
            TextAlignment = TextAlignment.Center
        });
    }


    /// <summary>
    /// Add a standard paragraph with right text alignment
    /// </summary>
    /// <param name="content"></param>
    /// <param name="fontSize">Logical font size of the paragraph. Default is Regular. See <see cref="FontSize"/></param>
    public void AddParagraphRight(string content, FontSize fontSize)
    {
        if (string.IsNullOrEmpty(content)) return;
        Components.Add(new ReportParagraphElement
        {
            Content = content,
            FontSize = fontSize,
            TextAlignment = TextAlignment.Right
        });
    }


    /// <summary>
    /// Add a table to the report
    /// </summary>
    /// <param name="data">2-dimensional array to print in a table</param>
    /// <param name="keepTogether">Keep the table together ron one page (use false if table gets longer then one page). Default: true</param>
    public void AddTable(string[,] data, bool keepTogether = true)
    {
        Components.Add(new ReportTableElement { Data = data, KeepTogether = keepTogether });
    }

    /// <summary>
    /// Add a table to the report
    /// </summary>
    /// <param name="data">2-dimensional array to print in a table</param>
    /// <param name="tableType">type of table see <see cref="TableTypes"/></param>
    /// <param name="keepTogether">Keep the table together ron one page (use false if table gets longer then one page). Default: true</param>
    public void AddTable(string[,] data, TableTypes tableType, bool keepTogether = true)
    {
        Components.Add(new ReportTableElement { Data = data, TableType = tableType, KeepTogether = keepTogether });
    }
    /// <summary>
    /// Add a page break to the report
    /// </summary>
    public void AddPageBreak()
    {
        Components.Add(new ReportPageBreakElement());
    }


    /// <summary>
    /// Add a text block containing tags like p and image
    /// </summary>
    /// <param name="textblock">text block to insert in the report</param>
    /// <example>
    /// &gt;P&lt;Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut 
    /// labore et dolore magna aliquyam erat, sed diam voluptua.&gt;/P&lt;&gt;Image src=\"c:\\temp\\charts3d.jpg\" /&lt;&gt;P&lt;At vero 
    /// eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren.&gt;/P&lt;
    /// </example>
    public void AddTextblock(string textblock)
    {
        if (string.IsNullOrEmpty(textblock)) return;
        Components.Add(new ReportTextBlockElement { Content = textblock });
    }

    /// <summary>
    /// Add a text block containing tags like p and image
    /// </summary>
    /// <param name="textblock">Text block to insert in the report</param>
    /// <param name="styleName">Name of the style to use for the text paragraphs</param>
    /// <example>
    /// &gt;P&lt;Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut 
    /// labore et dolore magna aliquyam erat, sed diam voluptua.&gt;/P&lt;&gt;Image src=\"c:\\temp\\charts3d.jpg\" /&lt;&gt;P&lt;At vero 
    /// eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren.&gt;/P&lt;
    /// </example>
    public void AddTextblock(string textblock, string styleName)
    {
        if (string.IsNullOrEmpty(textblock))
        {
            return;
        }
        Components.Add(new ReportTextBlockElement { Content = textblock, StyleName = styleName });
    }




    /// <summary>
    /// Add a XAML textblock to the report. Maybe a full <see cref="FlowDocument"/> or the valid blocks of such a document
    /// </summary>
    /// <param name="xaml">XAML to add to the report</param>
    public void AddXamlTextblock(string xaml)
    {
        if (string.IsNullOrEmpty(xaml)) return;
        Components.Add(new ReportXamlTextBlockElement { Content = xaml });
    }


    /// <summary>
    /// Add a XAML textblock to the report. Maybe a full <see cref="FlowDocument"/> or the valid blocks of such a document
    /// </summary>
    /// <param name="xaml">XAML to add to the report</param>
    /// <param name="styleName">Name of the style to use for the text paragraphs</param>
    public void AddXamlTextblock(string xaml, string styleName)
    {
        if (string.IsNullOrEmpty(xaml)) return;
        Components.Add(new ReportXamlTextBlockElement { Content = xaml, StyleName = styleName });
    }


    /// <summary>
    /// Add a figure (image plus legend below) to the report
    /// </summary>
    /// <param name="imagePath">Absolute or relative path to the image</param>
    /// <param name="title">legend text to be printed below the image</param>
    public ReportFigureElement AddFigure(string imagePath, string title)
    {
        var element = new ReportFigureElement { ImagePath = imagePath, Title = title };
        Components.Add(element);
        ImageCounter++;
        return element;
    }

    /// <summary>
    /// Add a figure (image plus legend below) to the report
    /// </summary>
    /// <param name="stream">Image as stream</param>
    /// <param name="title">legend text to be printed below the image</param>
    public ReportFigureElement AddFigure(Stream stream, string title)
    {
        stream.Position = 0;

        var ms = new MemoryStream();
        stream.CopyTo(ms);

        var element = new ReportFigureElement { ImageData = ms, Title = title };
        Components.Add(element);
        ImageCounter++;
        return element;
    }

    /// <summary>
    /// Add a image to the report
    /// </summary>
    /// <param name="imagePath">Absolute or relative path to the image</param>
    public ReportImageElement AddImage(string imagePath)
    {

        var element = new ReportImageElement { ImagePath = imagePath };
        Components.Add(element);
        ImageCounter++;
        return element;
    }


    /// <summary>
    /// Add a image to the report
    /// </summary>
    /// <param name="stream">Image as stream</param>
    public ReportImageElement AddImage(Stream stream)
    {
        stream.Position = 0;

        var ms = new MemoryStream();
        stream.CopyTo(ms);

        var element = new ReportImageElement { ImageData = ms };

        Components.Add(element);
        ImageCounter++;

        return element;
    }


    /// <summary>
    /// Get the full name of the next image file for the report
    /// </summary>
    /// <returns></returns>
    public string GetNextImageFileName()
    {
        return Path.Combine(Path.GetTempPath(), $"image{ImageCounter}.png");
    }

    /// <summary>
    /// Add a sub-headline paragraph
    /// </summary>
    /// <param name="content"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void AddSubHeader(string content)
    {
        Components.Add(new ReportSubheaderElement { Content = content });
    }


    /// <summary>
    /// Find a resource string a external assembly. Used for resolution of Resx: tags in <see cref="FlowDocumentService"/> method CheckContent()
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public string FindLanguageResource(string resourceName)
    {
        return FlowDocumentService.FindLanguageResource(resourceName);
    }


    /// <summary>
    /// Add a numbered list to the report
    /// </summary>
    /// <param name="list"></param>
    /// <param name="textMarkerStyle"></param>
    public void AddNumberedList(IList<string> list, TextMarkerStyle textMarkerStyle = TextMarkerStyle.Disc)
    {
        Components.Add(new ReportNumberedListElement { Data = list, ListStyle = textMarkerStyle });
    }
}
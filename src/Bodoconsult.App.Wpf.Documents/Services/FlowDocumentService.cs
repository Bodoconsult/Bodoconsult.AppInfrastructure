// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Documents.General;
using Bodoconsult.App.Wpf.Documents.Paginators;
using Bodoconsult.App.Wpf.Documents.WpfElements;
using Bodoconsult.App.Wpf.Helpers;
using PdfSharp.Xps;
using PropertyChanged;
using System.IO;
using System.IO.Packaging;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using System.Xaml;
using Bodoconsult.App.Wpf.Documents.Helpers;
using Figure = System.Windows.Documents.Figure;
using Image = System.Windows.Controls.Image;
using List = System.Windows.Documents.List;
using ListItem = System.Windows.Documents.ListItem;
using Paragraph = System.Windows.Documents.Paragraph;
using Thickness = System.Windows.Thickness;
using XamlReader = System.Windows.Markup.XamlReader;
using XamlWriter = System.Windows.Markup.XamlWriter;

namespace Bodoconsult.App.Wpf.Documents.Services;

/// <summary>
/// Service for easier creating flow documents
/// </summary>
[AddINotifyPropertyChangedInterface]
[SupportedOSPlatform("windows")]
public class FlowDocumentService
{

    private string _currentStyleSheetPath;

    private ResourceDictionary _styleRd;

    private readonly II18N _i18N;

    private string _currentDir;

    #region Ctor

    /// <summary>
    /// Default constructur for the class. Loads defaults for page settings.
    /// </summary>
    public FlowDocumentService()
    {
        TypographySettingsService = new TypographySettingsService();
        BaseConstructor();
    }


    /// <summary>
    /// Constructor to provide customized page settings
    /// </summary>
    /// <param name="typographySettingsService">Current typo settings service instance</param>
    public FlowDocumentService(TypographySettingsService typographySettingsService)
    {
        TypographySettingsService = typographySettingsService;
        BaseConstructor();
    }

    /// <summary>
    /// Constructor to provide customized page settings
    /// </summary>
    /// <param name="typographySettingsService">Current typo settings service instance</param>
    /// <param name="i18N">Internationaliation</param>
    public FlowDocumentService(TypographySettingsService typographySettingsService, II18N i18N)
    {
        TypographySettingsService = typographySettingsService;
        BaseConstructor();
        _i18N = i18N;
    }

    /// <summary>
    /// Base constructor routine
    /// </summary>
    private void BaseConstructor()
    {
        var fileName = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(fileName);

        try
        {
            var fi = new FileInfo(fileName);

            _currentDir = fi.DirectoryName;
        }
        catch
        {
            // Do nothing
        }

        Dispatcher = Application.Current.Dispatcher;
        Dispatcher.Invoke(() =>
        {
            Document = new FlowDocument
            {
                DataContext = TypographySettingsService
            };
        });

        TypographySettingsService.SetDefaultMargins();

        if (TypographySettingsService.MaxImageWidth < 0.0000000001)
        {
            TypographySettingsService.MaxImageWidth = TypographySettingsService.PageSize.Width - TypographySettingsService.Margins.Left - TypographySettingsService.Margins.Right;
        }

        LoadStyleSheet();
    }

    /// <summary>
    /// Current dispatcher
    /// </summary>
    public Dispatcher Dispatcher { get; private set; }

    #endregion


    #region Styling



    /// <summary>
    /// Current typo settings service
    /// </summary>
    public TypographySettingsService TypographySettingsService { get; }

    /// <summary>
    /// Path to the compact typography
    /// </summary>
    public const string StyleNormal = "/Bodoconsult.App.Wpf.Documents;component/Resources/Typography.xaml";

    ///// <summary>
    ///// Path to the compact typography
    ///// </summary>
    //public const string StyleCompact = "/Bodoconsult.App.Wpf.Documents;component/Resources/TypographyCompact.xaml";



    /// <summary>
    /// Load the style sheet defined in <see cref="TypographySettingsService"/>
    /// </summary>
    public void LoadStyleSheet()
    {
        Dispatcher.Invoke(() =>
        {
            _currentStyleSheetPath = string.IsNullOrEmpty(TypographySettingsService.UserDefinedTypographyFile)
                ? _currentStyleSheetPath = StyleNormal
                : TypographySettingsService.UserDefinedTypographyFile;

            _styleRd = new ResourceDictionary
            {
                Source = new Uri(_currentStyleSheetPath, UriKind.RelativeOrAbsolute)
            };
        });
    }

    #endregion

    #region

    private const string SpanHeader1 = "{0}. ";

    private const string SpanHeader2 = "{0}.{1}. ";

    private const string SpanHeader3 = "{0}.{1}.{2}. ";

    #endregion



    /// <summary>
    /// Tag prefix used to declare content as resource key
    /// </summary>
    public const string ResxTag = "Resx:";


    ///// <summary>
    ///// Currently used resource dictionary for language issues
    ///// </summary>
    //public ResourceDictionary LanguageRd { get; private set; }





    //private static Semaphore _pool;

    /// <summary>
    /// Document
    /// </summary>
    public FlowDocument Document { get; set; }

    /// <summary>
    /// Current section in the document
    /// </summary>
    public PagingSection CurrentSection { get; set; }

    /// <summary>
    /// Counter for tables
    /// </summary>
    public int TableCounter { get; set; }


    ///// <summary>
    ///// Indent of the first line in a paragraph
    ///// </summary>
    //public double TextIndent { get; set; }





    #region internally used counters and variables

    private bool _isPageBreak;
    private int _figureCounter;
    private int _imageCounter;


    private readonly int[] _headlines = new int[5];

    private int _oldLevel;

    #endregion









    /// <summary>
    /// Add a new section to the document
    /// </summary>
    public void AddSection()
    {
        Dispatcher.Invoke(() =>
        {
            CurrentSection = new PagingSection
            {
                BreakPageBefore = _isPageBreak,
                IsRestartPageNumberingRequired = false
            };

            // Add Section to FlowDocument 
            Document.Blocks.Add(CurrentSection);
        });
    }

    /// <summary>
    /// Add a paragraph to the current section
    /// </summary>
    /// <param name="content"></param>
    public void AddParagraph(string content)
    {

        //    _dispatcher.Invoke(() =>
        //    {
        //        var p = new Paragraph();

        //        var image = new Image
        //        {
        //            Source = new BitmapImage(new Uri(@"c:\bodoconsult\logos\logo.jpg")),
        //            Height= 14

        //        };

        //        var i = new InlineUIContainer(image);

        //        p.Inlines.Add(i);

        //        p.Inlines.Add(new Run(content));

        //        CurrentSection.Blocks.Add(p);
        //    });

        AddContent(content, "Standard");
    }

    /// <summary>
    /// Add a paragraph to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    public void AddParagraph(string content, params object[] array)
    {
        content = string.Format(content, array);
        AddContent(content, "Standard");
    }


    /// <summary>
    /// Add a centered paragraph to the current section
    /// </summary>
    /// <param name="content"></param>
    public void AddParagraphCentered(string content)
    {
        AddContent(content, "StandardCenter");
    }

    /// <summary>
    /// Add a centered paragraph to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    public void AddParagraphCentered(string content, params object[] array)
    {
        content = string.Format(content, array);
        AddContent(content, "StandardCenter");
    }


    /// <summary>
    /// Add a centered paragraph to the current section
    /// </summary>
    /// <param name="content"></param>
    public void AddParagraphRight(string content)
    {
        AddContent(content, "StandardRight");
    }

    /// <summary>
    /// Add a centered paragraph to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    public void AddParagraphRight(string content, params object[] array)
    {
        content = string.Format(content, array);
        AddContent(content, "StandardRight");
    }

    /// <summary>
    /// Add a smaller paragraph to the current section (smaller font-size and line-height)
    /// </summary>
    /// <param name="content"></param>
    public void AddSmallParagraph(string content)
    {
        AddContent(content, "SmallText");
    }


    /// <summary>
    /// Add a centered smaller paragraph to the current section (smaller font-size and line-height)
    /// </summary>
    /// <param name="content"></param>
    public void AddSmallParagraphCentered(string content)
    {
        AddContent(content, "SmallTextCentered");
    }


    /// <summary>
    /// Add a smaller paragraph with right text alignment to the current section (smaller font-size and line-height)
    /// </summary>
    /// <param name="content"></param>
    public void AddSmallParagraphRight(string content)
    {
        AddContent(content, "SmallTextRight");
    }


    /// <summary>
    /// Add a smaller paragraph to the current section (smaller font-size and line-height)
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    public void AddSmallParagraph(string content, params object[] array)
    {
        content = string.Format(content, array);
        AddContent(content, "SmallText");
    }

    /// <summary>
    /// Add a smaller paragraph to the current section (smaller font-size and line-height)
    /// </summary>
    /// <param name="content"></param>
    public void AddExtraSmallParagraph(string content)
    {
        AddContent(content, "ExtraSmallText");
    }


    /// <summary>
    /// Add a centered extra small paragraph to the current section (smaller font-size and line-height)
    /// </summary>
    /// <param name="content"></param>
    public void AddExtraSmallParagraphCentered(string content)
    {
        AddContent(content, "ExtraSmallTextCentered");
    }


    /// <summary>
    /// Add an extra small paragraph with right text alignment to the current section (smaller font-size and line-height)
    /// </summary>
    /// <param name="content"></param>
    public void AddExtraSmallParagraphRight(string content)
    {
        AddContent(content, "ExtraSmallTextRight");
    }

    /// <summary>
    /// Add a smaller paragraph to the current section (smaller font-size and line-height)
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    public void AddSmallExtraParagraph(string content, params object[] array)
    {
        content = string.Format(content, array);
        AddContent(content, "ExtraSmallText");
    }


    /// <summary>
    /// Add a text block containing tags like p and image
    /// </summary>
    /// <param name="content">text block to insert in the report</param>
    /// <example>
    /// &gt;P&lt;Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut 
    /// labore et dolore magna aliquyam erat, sed diam voluptua.&gt;/P&lt;&gt;Image src=\"c:\\temp\\charts3d.jpg\" /&lt;&gt;P&lt;At vero 
    /// eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren.&gt;/P&lt;
    /// </example>
    public void AddTextBlock(string content)
    {
        AddTextBlock(content, "Standard");
    }

    /// <summary>
    /// Add a text block containing tags like p and image
    /// </summary>
    /// <param name="content">text block to insert in the report</param>
    /// <param name="styleName">style name to use for paragraphs in the text block</param>
    /// <example>
    /// &gt;P&lt;Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut 
    /// labore et dolore magna aliquyam erat, sed diam voluptua.&gt;/P&lt;&gt;Image src=\"c:\\temp\\charts3d.jpg\" /&lt;&gt;P&lt;At vero 
    /// eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren.&gt;/P&lt;
    /// </example>
    public void AddTextBlock(string content, string styleName)
    {

        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        if (content.Contains("<Paragraph"))
        {
            AddXamlTextblock(content);
            return;
        }


        content = content.Replace("<p>", "<P>").Replace("<h1>", "<H1>").Replace("<h2>", "<H2>")
            .Replace("</h1>", "</H1>").Replace("</h2>", "</H2>")
            .Replace("</p>", "</P>").Replace("<image", "<Image")
            .Replace("</H2>", "??Block??").Replace("</H1>", "??Block??").Replace("</P>", "??Block??");


        var imageIndex = content.IndexOf("<Image", StringComparison.OrdinalIgnoreCase);

        while (imageIndex > 0)
        {

            var endIndex = content.IndexOf("/>", imageIndex, StringComparison.OrdinalIgnoreCase);

            var leftPart = content[..endIndex];
            var rightPart = content.Substring(endIndex + 2, content.Length - endIndex - 2);

            content = $"{leftPart}??Block??{rightPart}";


            imageIndex = content.IndexOf("<Image", imageIndex + 10, StringComparison.OrdinalIgnoreCase);
        }



        var blocks = content.Split(["??Block??"],
            StringSplitOptions.RemoveEmptyEntries);

        foreach (var block in blocks)
        {
            var block1 = block.Trim().Replace("\r", string.Empty).Replace("\n", string.Empty);

            if (string.IsNullOrEmpty(block1)) continue;

            if (block1.StartsWith("<P>"))
            {
                AddContent(block1.Replace("<P>", string.Empty).Trim(), styleName);
                continue;
            }


            if (block1.StartsWith("<H1>"))
            {
                AddHeader1(block1.Replace("<H1>", string.Empty).Trim());
                continue;
            }

            if (block1.StartsWith("<H2>"))
            {
                AddHeader2(block1.Replace("<H2>", string.Empty).Trim());
                continue;
            }

            if (!block1.StartsWith("<Image"))
            {
                AddContent(block1, styleName);
                continue;
            }



            var source = GetTag(block1, "src=");

            var title = GetTag(block1, "title=");

            var widthTag = GetTag(block1, "width=");

            var heightTag = GetTag(block1, "height=");

            var width = 0;

            if (!string.IsNullOrEmpty(widthTag))
            {
                width = Convert.ToInt32(widthTag);
            }

            var height = 0;

            if (!string.IsNullOrEmpty(heightTag))
            {
                height = Convert.ToInt32(heightTag);
            }

            if (!string.IsNullOrEmpty(title))
            {
                if (width > 0 || height > 0)
                {
                    AddFigure(source, title, width, height);
                }
                else
                {
                    AddFigure(source, title);
                }
            }
            else
            {
                if (width > 0 || height > 0)
                {
                    AddImage(source, width, height);
                }
                else
                {
                    AddImage(source);
                }
            }
        }
    }


    /// <summary>
    /// Add a XAML textblock. May contain only XAML for <see cref="System.Windows.Documents.Paragraph"/> objects and their valid content
    /// </summary>
    /// <param name="xaml">XAML string for <see cref="System.Windows.Documents.Paragraph"/> objects</param>
    /// <param name="styleName">Name of the style to use for paragraphs</param>
    public void AddXamlTextblock(string xaml, string styleName = "Standard")
    {

        if (string.IsNullOrEmpty(xaml))
        {
            return;
        }

        Dispatcher.Invoke(() =>
        {
            var style = FindStyleResource(styleName);

            var newcontent = xaml.StartsWith("<FlowDocument")
                ? xaml
                : $"<FlowDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{xaml}</FlowDocument>";

            if (!newcontent.Contains("xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'"))
            {
                newcontent = newcontent.Replace("<FlowDocument",
                    "<FlowDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
            }

            var newDoc = XamlReader.Parse(newcontent) as FlowDocument;

            if (newDoc is null)
            {
                return;
            }

            foreach (var block in newDoc.Blocks.ToArray())
            {
                if (block is Paragraph paragraph)
                {
                    paragraph.Style = style;
                }

                CurrentSection.Blocks.Add(block);
            }
        });
    }


    private static string GetTag(string block1, string tag)
    {
        var tagLength = tag.Length + 1;

        var sourceBeginIndex = block1.IndexOf(tag, StringComparison.OrdinalIgnoreCase);
        if (sourceBeginIndex == -1)
        {
            return null;
        }

        var sourceEndIndex = block1.IndexOf("\"", sourceBeginIndex + tagLength, StringComparison.OrdinalIgnoreCase);
        var source = block1.Substring(sourceBeginIndex + tagLength, sourceEndIndex - sourceBeginIndex - tagLength);

        return source;
    }


    private void AddContent(string content, string styleName, string prefix = "")
    {
        if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(styleName))
        {
            return;
        }

        Dispatcher.Invoke(() =>
        {
            var style = FindStyleResource(styleName);

            var paragraph = CheckContent(content, prefix);
            paragraph.BreakPageBefore = _isPageBreak;
            //paragraph.TextIndent = TextIndent;
            paragraph.Style = style;
            CurrentSection.Blocks.Add(paragraph);
            _isPageBreak = false;
        });
    }

    /// <summary>
    /// Check content and return a paragraph made of this content
    /// </summary>
    /// <param name="prefix">Prefixes like numbers of headlines</param>
    /// <param name="content">content to check</param>
    /// <returns></returns>
    public Paragraph CheckContent(string content, string prefix)
    {
        Paragraph p = null;

        Dispatcher.Invoke(() =>
        {
            if (content.StartsWith("Resx:", StringComparison.OrdinalIgnoreCase))
            {
                content = FindLanguageResource(content.Replace(ResxTag, string.Empty, StringComparison.OrdinalIgnoreCase));
            }
            if (content.StartsWith("I18n:", StringComparison.OrdinalIgnoreCase))
            {
                content = FindLanguageResource(content.Replace(II18N.I18NTag, string.Empty, StringComparison.OrdinalIgnoreCase));
            }
            else
            {

                //content = content.Replace("<=", "&lt;&#61;").Replace(">=", "&gt;&#61;");

                content = content.Replace("<i>", "<Italic>")
                    .Replace("</i>", "</Italic>")
                    .Replace("<b>", "<Bold>")
                    .Replace("</b>", "</Bold>")
                    .Replace("<u>", "<Underline>")
                    .Replace("</u>", "</Underline>");

                // Check, if Resx-tag exists in the middle of the content
                var pos = content.IndexOf(ResxTag, StringComparison.CurrentCultureIgnoreCase);
                while (pos > -1)
                {
                    var endPos = content.IndexOf("}", pos + ResxTag.Length, StringComparison.CurrentCultureIgnoreCase);


                    var tag = content.Substring(pos + ResxTag.Length, endPos - pos - ResxTag.Length);

                    var tagValue = FindLanguageResource(tag);

                    content = content[..(pos - 1)] + tagValue + content[(endPos + 1)..];

                    pos = content.IndexOf(ResxTag, pos + tagValue.Length, StringComparison.CurrentCultureIgnoreCase);
                }

                content = content.Replace("&", "&amp;").Replace("&amp;gt;", "&gt;").Replace("&amp;lt;", "&lt;").Replace("&amp;#", "&#");

                if (content.Contains("FigureCount"))
                {
                    content = content.Replace("??FigureCount??", $"{_figureCounter:#,##0}").
                        Replace("??FigureCountM1??", $"{_figureCounter - 1:#,##0}").
                        Replace("??FigureCountP1??", $"{_figureCounter + 1:#,##0}");
                }


                var containsImages = content.Contains("<Image");

                // Content conatins Image tag: replace with image block
                if (containsImages)
                {

                    var i = content.IndexOf("<image", StringComparison.OrdinalIgnoreCase);

                    while (i > 0)
                    {
                        var leftPart = content[..i];

                        var endIndex = content.IndexOf("/>", i + 1, StringComparison.OrdinalIgnoreCase);

                        var rightPart = content.Substring(endIndex + 2, content.Length - endIndex - 2);


                        var sourceBeginIndex = content.IndexOf("src=", i, StringComparison.OrdinalIgnoreCase);
                        var sourceEndIndex = content.IndexOf("\"", sourceBeginIndex + 5, StringComparison.OrdinalIgnoreCase);

                        var source = content.Substring(sourceBeginIndex + 5, sourceEndIndex - sourceBeginIndex - 5);

                        source = CheckPath(source);

                        if (!File.Exists(source))
                        {
                            i = content.IndexOf("<image", i + 5, StringComparison.OrdinalIgnoreCase);
                            continue;
                        }
                        var image = string.Format(TypographySettingsService.ImageTemplate, source, null, TypographySettingsService.MaxImageHeight, TypographySettingsService.MaxImageWidth);

                        content = leftPart
                                  + image
                                  + rightPart;

                        i = content.IndexOf("<image", i + image.Length, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }


            //// var newcontent = string.Format("<Paragraph xml:space='preserve' xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:mc='http://schemas.openxmlformats.org/markup-compatibility/2006'>{0}</Paragraph>", prefix + content);
            var newcontent =
                $"<Paragraph xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{prefix + content}</Paragraph>";
            p = XamlReader.Parse(newcontent) as Paragraph;


            //p = XamlReader.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(newcontent))) as Paragraph;

        });

        return p;
    }


    /// <summary>
    /// Add a numbered list to the document with bullets, numbers and others ahead of the paragraph
    /// </summary>
    /// <param name="data"></param>
    /// <param name="listStyle"></param>
    public void AddNumberedList(IEnumerable<string> data, TextMarkerStyle listStyle)
    {

        Dispatcher.Invoke(() =>
        {
            var style = FindStyleResource("Standard");
            var list = new List
            {
                MarkerStyle = listStyle,
                MarkerOffset = 15,
                Margin = new Thickness(0, 0, 0, 0)
            };

            foreach (var item in data)
            {

                var paragraph = CheckContent(item, string.Empty);
                paragraph.Style = style;
                CurrentSection.Blocks.Add(paragraph);

                var listItem = new ListItem(paragraph);
                list.ListItems.Add(listItem);
            }

            CurrentSection.Blocks.Add(list);
        });


    }




    /// <summary>
    /// Add a manual page break to the document
    /// </summary>
    public void AddPageBreak()
    {
        _isPageBreak = true;
    }
    /// <summary>
    /// Add a title to the current section
    /// </summary>
    /// <param name="content"></param>
    public void AddTitle(string content)
    {
        AddContent(" ", "BeforeTitle");
        AddContent(content, "Title");
    }
    /// <summary>
    /// Add a title level 2 to the current section
    /// </summary>
    /// <param name="content"></param>
    public void AddTitle2(string content)
    {
        AddContent(content, "Title2");
    }


    /// <summary>
    /// Add a title to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    public void AddTitle(string content, params object[] array)
    {
        content = string.Format(content, array);

        AddContent(" ", "BeforeTitle");
        AddContent(content, "Title");
    }

    /// <summary>
    /// Add a headline level 1 to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="noCount">If noCount is true, the header will not be included in the header counting</param>
    public void AddHeader1(string content, bool noCount = false)
    {
        var number = string.Empty;

        if (!noCount)
        {
            _oldLevel = 1;
            _headlines[0]++;
            number = TypographySettingsService.AutoNumbering ? string.Format(SpanHeader1, _headlines[0]) : string.Empty;
        }

        AddContent(content, "Headline1", number);
    }

    /// <summary>
    /// Add a headline level 1 to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="noCount">If noCount is true, the header will not be included in the header counting</param>
    /// <param name="array"></param>
    public void AddHeader1(string content, bool noCount = false, params object[] array)
    {
        var number = string.Empty;

        if (!noCount)
        {
            _oldLevel = 1;
            _headlines[0]++;
            number = TypographySettingsService.AutoNumbering ? string.Format(SpanHeader1, _headlines[0]) : string.Empty;
        }

        content = string.Format(content, array);
        AddContent(content, "Headline1", number);



    }

    /// <summary>
    /// Add a headline level 2 to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="noCount">If noCount is true, the header will not be included in the header counting</param>
    public void AddHeader2(string content, bool noCount = false)
    {

        var number = string.Empty;

        if (!noCount)
        {

            if (_oldLevel != 2)
            {
                _headlines[1] = 0;
                _oldLevel = 2;
            }
            _headlines[1]++;
            number = TypographySettingsService.AutoNumbering
                ? string.Format(SpanHeader2, _headlines[0], _headlines[1])
                : string.Empty;
        }
        AddContent(content, "Headline2", number);
    }

    /// <summary>
    /// Add a headline level 2 to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="noCount">If noCount is true, the header will not be included in the header counting</param>
    /// <param name="array"></param>
    public void AddHeader2(string content, bool noCount = false, params object[] array)
    {
        var number = string.Empty;

        if (!noCount)
        {

            if (_oldLevel != 2)
            {
                _headlines[1] = 0;
                _oldLevel = 2;
            }
            _headlines[1]++;
            number = TypographySettingsService.AutoNumbering
                ? string.Format(SpanHeader2, _headlines[0], _headlines[1])
                : string.Empty;
        }
        content = string.Format(content, array);
        AddContent(content, "Headline2", number);
    }


    /// <summary>
    /// Add a headline level 3 to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="noCount">If noCount is true, the header will not be included in the header counting</param>
    public void AddHeader3(string content, bool noCount = false)
    {
        var number = string.Empty;

        if (!noCount)
        {

            if (_oldLevel != 3)
            {
                _headlines[2] = 0;
                _oldLevel = 3;
            }
            _headlines[2]++;
            number = TypographySettingsService.AutoNumbering
                ? string.Format(SpanHeader3, _headlines[0], _headlines[1], _headlines[2])
                : string.Empty;
        }

        AddContent(content, "Headline3", number);
    }

    /// <summary>
    /// Add a headline level 3 to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    /// <param name="noCount">If noCount is true, the header will not be included in the header counting</param>
    public void AddHeader3(string content, bool noCount = false, params object[] array)
    {
        var number = string.Empty;

        if (!noCount)
        {

            if (_oldLevel != 3)
            {
                _headlines[2] = 0;
                _oldLevel = 3;
            }
            _headlines[2]++;
            number = TypographySettingsService.AutoNumbering
                ? string.Format(SpanHeader3, _headlines[0], _headlines[1], _headlines[2])
                : string.Empty;
        }

        content = string.Format(content, array);
        AddContent(content, "Headline3", number);
    }

    /// <summary>
    /// Add a headline level 4 to the current section
    /// </summary>
    /// <param name="content"></param>
    public void AddHeader4(string content)
    {
        AddContent(content, "Headline4");
    }

    /// <summary>
    /// Add a headline level 4 to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    public void AddHeader4(string content, params object[] array)
    {
        content = string.Format(content, array);
        AddContent(content, "Headline2");
    }


    /// <summary>
    /// Add a headline level 5 to the current section
    /// </summary>
    /// <param name="content"></param>
    public void AddHeader5(string content)
    {
        AddContent(content, "Headline5");
    }

    /// <summary>
    /// Add a headline level 5 to the current section
    /// </summary>
    /// <param name="content"></param>
    /// <param name="array"></param>
    public void AddHeader5(string content, params object[] array)
    {
        content = string.Format(content, array);
        AddContent(content, "Headline5");
    }


    /// <summary>
    /// Add an image from a local or web path
    /// </summary>
    /// <param name="path"></param>
    public void AddImage(string path)
    {
        path = CheckPath(path);

        if (!File.Exists(path))
        {
            return;
        }

        Dispatcher.Invoke(() =>
        {

            // ToDo: check why images are not loaded from resources

            var image = new Image { Name = $"image{_imageCounter}" };
            _imageCounter++;

            var bimg = new BitmapImage();
            bimg.BeginInit();

            var uri = new Uri(path, UriKind.RelativeOrAbsolute);

            bimg.UriSource = uri;
            //bimg.CacheOption = BitmapCacheOption.OnLoad;
            bimg.EndInit();
            image.Source = bimg;

            AddFigureBase(image, null, 0, 0);
        });
    }

    private string CheckPath(string path)
    {
        if (!path.Contains("@Path"))
        {
            return path;
        }

        var s = path.Replace("@Path", _currentDir, StringComparison.OrdinalIgnoreCase);
        return s;
    }


    /// <summary>
    /// Add an image from a local or web path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="width">width of the image in pixels</param>
    /// <param name="height">height of the image in pixels</param>
    public void AddImage(string path, int width, int height)
    {
        path = CheckPath(path);

        if (!File.Exists(path))
        {
            return;
        }

        Dispatcher.Invoke(() =>
        {

            var image = new Image { Name = $"image{_imageCounter}" };
            _imageCounter++;

            var bimg = new BitmapImage();
            bimg.BeginInit();

            var uri = new Uri(path, UriKind.RelativeOrAbsolute);

            bimg.UriSource = uri;
            //bimg.CacheOption = BitmapCacheOption.OnLoad;
            bimg.EndInit();
            image.Source = bimg;

            //image.Width = width;
            //image.Height = height;

            AddFigureBase(image, null, width, height);
        });
    }

    /// <summary>
    /// Add a canvas from a XAML file as image to the document
    /// </summary>
    /// <param name="path">path the canvas is save to</param>
    /// <param name="width">width of the image in pixels</param>
    /// <param name="height">height of the image in pixels</param>
    public void AddCanvas(string path, int width, int height)
    {

        Dispatcher.Invoke(() =>
        {

            var canvas = (Canvas)WpfHelper.LoadElementFromXamlFile(path);

            var stream = WpfHelper.RenderCanvasToImageStream(canvas, width, height, WpfHelper.ImageFormat.Png);

            AddImage(stream);

        });
    }


    /// <summary>
    /// Add a canvas from a XAML file as image to the document
    /// </summary>
    /// <param name="canvas">Canvas to add to the document</param>
    /// <param name="width">width of the image in pixels</param>
    /// <param name="height">height of the image in pixels</param>
    public void AddCanvas(Canvas canvas, int width, int height)
    {
        Dispatcher.Invoke(() =>
        {
            var xaml = XamlWriter.Save(canvas);
            canvas = (Canvas)XamlServices.Parse(xaml);
            var stream = WpfHelper.RenderCanvasToImageStream(canvas, width, height, WpfHelper.ImageFormat.Png);

            AddImage(stream);

        });
    }


    /// <summary>
    /// Add an image from a local or web path
    /// </summary>
    /// <param name="stream"></param>
    public void AddImage(Stream stream)
    {
        Dispatcher.Invoke(() =>
        {

            stream.Position = 0;

            var image = new Image
            {

                Name = $"image{_imageCounter}",
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.Both,
            };
            _imageCounter++;

            var bimg = new BitmapImage();
            bimg.BeginInit();
            bimg.StreamSource = stream;
            bimg.CacheOption = BitmapCacheOption.OnLoad;
            bimg.EndInit();

            image.Source = bimg;
            //image.Width = bimg.Width;
            //image.Height = bimg.Height;


            AddFigureBase(image, null, 0, 0);
        });
    }


    /// <summary>
    /// Add an image from a local or web path
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void AddImage(Stream stream, int width, int height)
    {
        Dispatcher.Invoke(() =>
        {
            stream.Position = 0;

            var image = new Image
            {

                Name = $"image{_imageCounter}",
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.Both,
            };
            _imageCounter++;

            var bimg = new BitmapImage();
            bimg.BeginInit();
            bimg.StreamSource = stream;
            bimg.CacheOption = BitmapCacheOption.OnLoad;
            bimg.EndInit();

            image.Source = bimg;
            //image.Width = width;
            //image.Height = height;


            AddFigureBase(image, null, width, height);
        });
    }

    /// <summary>
    /// Add a ruler to the current section
    /// </summary>
    public void AddRuler()
    {
        // ToDo:
        //var ruler = new Line
        //{
        //    X1 = 0,
        //    X2 = 999999,
        //    Y1 = 0,
        //    Y2 = 0,
        //    Stroke = new SolidColorBrush(Colors.Black),
        //    StrokeThickness = 2,
        //    Stretch = Stretch.Fill
        //};
        //var container = new InlineUIContainer(ruler);
        //CurrentSection.Blocks.Add(new Paragraph(container));
    }


    /// <summary>
    /// Add an image from a local or web path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="title"></param>
    public void AddFigure(string path, string title)
    {
        path = CheckPath(path);

        if (!File.Exists(path))
        {
            return;
        }

        _figureCounter++;

        Dispatcher.Invoke(() =>
        {
            var image = new Image();
            var bimg = new BitmapImage();
            bimg.BeginInit();
            bimg.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bimg.EndInit();
            image.Source = bimg;

            AddFigureBase(image, title, 0, 0);
        });
    }


    /// <summary>
    /// Add an image from a local or web path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="title"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void AddFigure(string path, string title, int width, int height)
    {
        path = CheckPath(path);

        if (!File.Exists(path))
        {
            return;
        }

        _figureCounter++;

        Dispatcher.Invoke(() =>
        {
            var image = new Image();
            var bimg = new BitmapImage();
            bimg.BeginInit();
            bimg.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bimg.EndInit();
            image.Source = bimg;

            AddFigureBase(image, title, width, height);
        });
    }

    private void AddFigureBase(FrameworkElement image, string title, double maxWidth, double maxHeight)
    {
        Dispatcher.Invoke(() =>
        {
            image.MaxHeight = maxHeight > 0 ? maxHeight : TypographySettingsService.MaxImageHeight;
            image.MaxWidth = maxWidth > 0 ? maxWidth : TypographySettingsService.MaxImageWidth;

            var figure = new Figure();

            var style = FindStyleResource("FigureBlock");

            var block = new BlockUIContainer(image)
            {
                Name =
                    string.IsNullOrEmpty(title)
                        ? $"imageContainer{_imageCounter}"
                        : $"figureContainer{_figureCounter}",
                BreakPageBefore = _isPageBreak,
                Style = style
            };

            style = FindStyleResource("FigureImage");

            figure.Style = style;
            figure.Blocks.Add(block);
            figure.HorizontalAnchor = FigureHorizontalAnchor.ColumnCenter;
            figure.CanDelayPlacement = false;

            Paragraph paragraphContainer;

            if (string.IsNullOrEmpty(title))
            {
                style = FindStyleResource("FigureOnlyImage");
                paragraphContainer = new Paragraph { Style = style };
            }
            else
            {
                style = FindStyleResource("FigureText");

                paragraphContainer =
                    CheckContent(
                        (TypographySettingsService.ShowFigureCounter
                            ? $"{TypographySettingsService.FigureCounterPrefix} {_figureCounter:#,##0}: "
                            : string.Empty) + title, string.Empty);
                paragraphContainer.Style = style;

            }

            paragraphContainer.Inlines.Add(figure);

            CurrentSection.Blocks.Add(paragraphContainer);
            _isPageBreak = false;
        });
    }


    /// <summary>
    /// Add a image from a local or web path
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="title"></param>
    public void AddFigure(Stream stream, string title)
    {

        _figureCounter++;

        Dispatcher.Invoke(() =>
        {
            stream.Position = 0;

            var image = new Image
            {

                Name = $"image{_imageCounter}",
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.Both,
            };
            _imageCounter++;

            var bimg = new BitmapImage();
            bimg.BeginInit();
            bimg.StreamSource = stream;
            bimg.CacheOption = BitmapCacheOption.OnLoad;
            bimg.EndInit();

            image.Source = bimg;

            AddFigureBase(image, title, 0, 0);
        });
    }

    /// <summary>
    /// Add a image from a local or web path
    /// </summary>
    /// <param name="stream">Image as <see cref="Stream"/></param>
    /// <param name="width">Width of the image in pixels</param>
    /// <param name="height">Height of the image in pixels</param>
    /// <param name="title">Title for the figure</param>
    public void AddFigure(Stream stream, int width, int height, string title)
    {

        _figureCounter++;

        Dispatcher.Invoke(() =>
        {
            stream.Position = 0;

            var image = new Image
            {

                Name = $"image{_imageCounter}",
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.Both,
            };
            _imageCounter++;

            var bimg = new BitmapImage();
            bimg.BeginInit();
            bimg.StreamSource = stream;
            bimg.CacheOption = BitmapCacheOption.OnLoad;
            bimg.EndInit();

            image.Source = bimg;
            //image.Width = width;
            //image.Height = height;


            AddFigureBase(image, title, 0, 0);
        });
    }

    /// <summary>
    /// Add a canvas from a file as figure to the document
    /// </summary>
    /// <param name="canvasPath">path the canvas is saved as file</param>
    /// <param name="width">Width of the image in pixels</param>
    /// <param name="height">Height of the image in pixels</param>
    /// <param name="title">Title for the figure</param>
    public void AddFigure(string canvasPath, int width, int height, string title)
    {
        Dispatcher.Invoke(() =>
        {
            var canvas = (Canvas)WpfHelper.LoadElementFromXamlFile(canvasPath);

            var stream = WpfHelper.RenderCanvasToImageStream(canvas, width, height, WpfHelper.ImageFormat.Png);

            AddFigure(stream, title);
        });
    }
    /// <summary>
    /// Add a canvas object as figure to the document
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="width">Width of the image in pixels</param>
    /// <param name="height">Height of the image in pixels</param>
    /// <param name="title">Title for the figure</param>
    public void AddFigure(Canvas canvas, int width, int height, string title)
    {
        var xaml = XamlWriter.Save(canvas);

        Dispatcher.Invoke(() =>
        {
            canvas = (Canvas)XamlServices.Parse(xaml);
            var stream = WpfHelper.RenderCanvasToImageStream(canvas, width, height, WpfHelper.ImageFormat.Png);

            AddFigure(stream, title);
        });
    }


    /// <summary>
    /// Add the default footer and header for the current paginator to the page
    /// </summary>
    public void AddDefaultFooterAndHeader()
    {
        if (!string.IsNullOrEmpty(TypographySettingsService.DocumentMetaData.LogoPath))
        {
            TypographySettingsService.DrawHeaderDelegate = DefaultHeader;
        }

        TypographySettingsService.DrawFooterDelegate = DefaultFooter;
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
        if (string.IsNullOrEmpty(TypographySettingsService.DocumentMetaData.FooterTemplate))
        {
            return;
        }
        var sections = TypographySettingsService.DocumentMetaData.FooterTemplate.ToLowerInvariant().Split('|');

        Dispatcher.Invoke(() =>
        {
            // Draw left element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, TypographySettingsService.DocumentMetaData, sections[0], 0, dpi, false, page, pageNumberFormat, TypographySettingsService.FooterFontName, TypographySettingsService.FooterFontSize, TypographySettingsService.FooterMarginTop);

            // Draw middle element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, TypographySettingsService.DocumentMetaData, sections[1], 1, dpi, false, page, pageNumberFormat, TypographySettingsService.FooterFontName, TypographySettingsService.FooterFontSize, TypographySettingsService.FooterMarginTop);

            // Draw right element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, TypographySettingsService.DocumentMetaData, sections[2], 2, dpi, false, page, pageNumberFormat, TypographySettingsService.FooterFontName, TypographySettingsService.FooterFontSize, TypographySettingsService.FooterMarginTop);
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
        if (string.IsNullOrEmpty(TypographySettingsService.DocumentMetaData.HeaderTemplate))
        {
            return;
        }
        var sections = TypographySettingsService.DocumentMetaData.HeaderTemplate.ToLowerInvariant().Split('|');

        Dispatcher.Invoke(() =>
        {
            // Draw left element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, TypographySettingsService.DocumentMetaData, sections[0], 0, dpi, true, page, PageNumberFormatEnum.Decimal, TypographySettingsService.HeaderFontName, TypographySettingsService.HeaderFontSize, TypographySettingsService.HeaderMarginBottom);

            // Draw middle element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, TypographySettingsService.DocumentMetaData, sections[1], 1, dpi, true, page, PageNumberFormatEnum.Decimal, TypographySettingsService.HeaderFontName, TypographySettingsService.HeaderFontSize, TypographySettingsService.HeaderMarginBottom);

            // Draw right element
            WpfDocumentRendererHelper.CreateHeaderFooterElement(context, area, TypographySettingsService.DocumentMetaData, sections[2], 2, dpi, true, page, PageNumberFormatEnum.Decimal, TypographySettingsService.HeaderFontName, TypographySettingsService.HeaderFontSize, TypographySettingsService.HeaderMarginBottom);
        });
    }


    /// <summary>
    /// Add a normal table with border
    /// </summary>
    /// <param name="data"></param>
    /// <param name="keepTogether">Keep the table together on one page</param>
    public void AddTable(string[,] data, bool keepTogether = true)
    {
        AddTable(data, TableTypes.Normal);
    }

    /// <summary>
    /// Add a normal table with no border
    /// </summary>
    /// <param name="data"></param>
    /// <param name="keepTogether">Keep the table together on one page</param>
    public void AddTableUnbordered(string[,] data, bool keepTogether = true)
    {
        AddTable(data, TableTypes.NormalUnbordered);
    }

    /// <summary>
    /// Add a small table with border
    /// </summary>
    /// <param name="data"></param>
    /// <param name="keepTogether">Keep the table together on one page</param>
    public void AddSmallTable(string[,] data, bool keepTogether = true)
    {
        AddTable(data, TableTypes.Small);
    }

    /// <summary>
    /// Add a small table with no border
    /// </summary>
    /// <param name="data"></param>
    /// <param name="keepTogether">Keep the table together on one page</param>
    public void AddSmallTableUnbordered(string[,] data, bool keepTogether = true)
    {
        AddTable(data, TableTypes.SmallUnbordered);
    }

    /// <summary>
    /// Add an extrasmall table with border
    /// </summary>
    /// <param name="data"></param>
    /// <param name="keepTogether">Keep the table together on one page</param>
    public void AddExtraSmallTable(string[,] data, bool keepTogether = true)
    {
        AddTable(data, TableTypes.ExtraSmall);
    }

    /// <summary>
    /// Add an extrasmall table with no border
    /// </summary>
    /// <param name="keepTogether">Keep the table together on one page</param>
    /// <param name="data"></param>
    public void AddExtraSmallTableUnbordered(string[,] data, bool keepTogether = true)
    {
        AddTable(data, TableTypes.ExtraSmallUnbordered);
    }


    /// <summary>
    /// Add a table to the flow document
    /// </summary>
    /// <param name="data">Data to how in the table</param>
    /// <param name="typeOfTable">Type of table</param>
    /// <param name="keepTogether">Keep the table together on one page</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void AddTable(string[,] data, TableTypes typeOfTable, bool keepTogether = true)
    {

        Dispatcher.Invoke(() =>
        {

            var table = new TableService(data, _isPageBreak, typeOfTable, this, keepTogether);
            table.LoadStyles();
            table.AddHeader();
            table.AddFooter();
            table.AddColumns();
            table.AddHeaderRow();
            table.AddDataRows();

            _isPageBreak = false;
        });
    }

    /// <summary>
    /// Find a resource depending on the DocumentStyle selected
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public Style FindStyleResource(string resourceName)
    {
        Style style = null;
        Dispatcher.Invoke(() =>
        {
            style = (Style)_styleRd[resourceName];
        });
        return style;
    }


    /// <summary>
    /// Update a resource key in the typography dictionary
    /// </summary>
    /// <param name="resourceKey"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public void SetResourceValue<T>(string resourceKey, T value)
    {
        _styleRd[resourceKey] = value;
    }



    /// <summary>
    /// Find a resource string an external assembly. Used for resolution of Resx: tags in <see cref="CheckContent"/>.
    /// Makes use of <see cref="II18N"/> which requires resources to registered before using via <see cref="II18NBase.AddProvider"/>.
    /// Language is injected via <see cref="TypographySettingsService"/>.
    /// </summary>
    /// <param name="resourceKey"></param>
    /// <returns></returns>
    public string FindLanguageResource(string resourceKey)
    {

        var erg = _i18N.Translate(resourceKey);


        if (string.IsNullOrEmpty(erg))
        {
            erg = resourceKey;
        }

        return erg;
    }



    #region Export document

    /// <summary>
    /// Save document as XPS file
    /// </summary>
    /// <param name="path"></param>
    public void SaveAsPdf(string path)
    {
        Dispatcher.Invoke(() =>
        {
            var lMemoryStream = new MemoryStream();
            using (var container = Package.Open(lMemoryStream, FileMode.Create))
            {
                using (var xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
                {
                    var rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);

                    //var definition = new PrintDefinition();

                    //if (PageFooter != null)
                    //{
                    //    definition.FooterHeight = FooterHeight;
                    //    definition.Footer += PageFooter;
                    //}
                    //if (PageHeader != null)
                    //{
                    //    definition.HeaderHeight = HeaderHeight;
                    //    definition.Header += PageHeader;
                    //}

                    //if (PageFooter != null) definition.Header += PageHeader;

                    var pn = new HeaderFooterPaginator(Document, TypographySettingsService, Dispatcher,
                        PageNumberFormatEnum.Decimal);

                    rsm.SaveAsXaml(pn);

                    //rsm.SaveAsXaml(((IDocumentPaginatorSource) Document).DocumentPaginator);
                    rsm.Commit();
                }
            }

            var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
            XpsConverter.Convert(pdfXpsDoc, path, 0);
        });
    }

    /// <summary>
    /// Save document as XPS file
    /// </summary>
    /// <returns></returns>
    public MemoryStream SaveAsStream()
    {
        //Trace.WriteLine("Xps export...");

        var lMemoryStream = new MemoryStream();

        Dispatcher.Invoke(() =>
        {
            using (var container = Package.Open(lMemoryStream, FileMode.Create))
            {
                using (var xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
                {
                    var rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);

                    //var definition = new PrintDefinition();

                    //if (PageFooter != null)
                    //{
                    //    definition.FooterHeight = FooterHeight;
                    //    definition.Footer += PageFooter;
                    //}
                    //if (PageHeader != null)
                    //{
                    //    definition.HeaderHeight = HeaderHeight;
                    //    definition.Header += PageHeader;
                    //}

                    rsm.SaveAsXaml(new HeaderFooterPaginator(Document, TypographySettingsService, Dispatcher, PageNumberFormatEnum.Decimal));

                    //rsm.SaveAsXaml(((IDocumentPaginatorSource) Document).DocumentPaginator);
                    rsm.Commit();
                }
            }

            lMemoryStream.Position = 0;
        }, DispatcherPriority.Normal);

        return lMemoryStream;
    }

    /// <summary>
    /// Save document as XPS file
    /// </summary>
    /// <param name="path"></param>
    public void SaveAsXps(string path)
    {
        //   Document._dispatcher.BeginInvoke(DispatcherPriority.Send,
        //(ThreadStart)delegate
        //{

        //Trace.WriteLine("Xps export...");

        Dispatcher.Invoke(() =>
        {
            using (var container = Package.Open(path, FileMode.Create))
            {
                using (var xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
                {
                    //Trace.WriteLine("Xps export 1...");

                    var rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);

                    //var definition = new PrintDefinition();

                    //if (PageFooter != null)
                    //{
                    //    definition.FooterHeight = FooterHeight;
                    //    definition.Footer += PageFooter;
                    //}
                    //if (PageHeader != null)
                    //{
                    //    definition.HeaderHeight = HeaderHeight;
                    //    definition.Header += PageHeader;
                    //}

                    //var paginator = ((IDocumentPaginatorSource)Document).DocumentPaginator;

                    var paginator = new HeaderFooterPaginator(Document, TypographySettingsService, Dispatcher, PageNumberFormatEnum.Decimal);
                    rsm.SaveAsXaml(paginator);

                    //Trace.WriteLine("Xps export 2...");
                    //rsm.SaveAsXaml(((IDocumentPaginatorSource)Document).DocumentPaginator);
                    rsm.Commit();
                }
            }



            //        _pool.Release();

            //});

            //       _pool.WaitOne();
        }, DispatcherPriority.Normal);
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void AddSubheader(string content)
    {
        AddContent(content, "Subheader");
    }


}
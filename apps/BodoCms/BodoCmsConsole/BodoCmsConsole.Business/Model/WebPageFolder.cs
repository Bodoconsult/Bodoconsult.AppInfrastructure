// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using BodoCmsConsole.Business.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Windows.FileSystem;
using Bodoconsult.Drawing;
using Bodoconsult.Drawing.Model;

namespace BodoCmsConsole.Business.Model;

[SupportedOSPlatform("windows")]
public class WebPageFolder
{
    private readonly bool _removeNumbers;
    private string _includeText = string.Empty;
    private readonly IAppLoggerProxy? _logger;
    private string _keywords = string.Empty;
    private string _description = string.Empty;
    private readonly string _defaultDescription;
    private readonly string _defaultKeywords;
    private readonly WebPageSettings _webPageSettings;
    private readonly string _relative;
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="webPageSettings">Current webpage settings</param>
    /// <param name="dir">Current directory info</param>
    /// <param name="folderData">Current folder data</param>
    /// <param name="removeNumbers">Remove heading numbers</param>
    /// <param name="relative">Relative path</param>
    /// <param name="folderCounter">Current folder counter</param>
    public WebPageFolder(IAppGlobals appGlobals, WebPageSettings webPageSettings,
        DirectoryInfo dir,
        FolderData? folderData,
        bool removeNumbers,
        string relative,
        int folderCounter)
    {

        //#if DEBUG
        //            if (dir.FullName.Contains("Bodoconsult.Core.Database"))
        //            {
        //                Debug.Print(dir.FullName);
        //            }
        //#endif

        //_folderData = folderData;

        _appGlobals = appGlobals;
        _logger = _appGlobals.Logger;
        _webPageSettings = webPageSettings;
        _relative = relative;

        BaseUrl = relative;

        Directory = dir;

        _removeNumbers = removeNumbers;
        _defaultDescription = _webPageSettings.DefaultDescription;
        _defaultKeywords = _webPageSettings.Keywords;

        ImagesPdf = new StringBuilder();
        OtherFilesPdf = new StringBuilder();
        UrlPdf = new StringBuilder();

        if (folderData != null)
        {
            Title = folderData.Name;
            TitleReadable = Title.Replace("_", " ");
            IsSummaryRequired = folderData.Summary;
            Keywords = folderData.Keywords;
            Description = folderData.Description;
            AdSetId = folderData.AdSetId;

            // Header-Image verlinken
            HeaderImage = folderData.HeaderImageLink;

            if (!string.IsNullOrEmpty(folderData.DisplayName))
            {
                TitleReadable = HtmlHelper.RemoveNumbers(folderData.DisplayName, removeNumbers);
            }
        }


        // Name ohne führende Nummern ermitteln


        //// Kein Summary, wenn unterster Ordner
        //if (dir.GetDirectories().Length==0 ) Summary = false;

        var summary = Path.Combine(dir.FullName, "Zusammenfassung.htm");

        if (!IsSummaryRequired && File.Exists(summary))
        {
            File.Delete(summary);
        }

        Summary = new Summary
        {
            Path = dir.FullName,
            Counter = folderCounter,
            IncludeSummariesForSubfolders = IsSummaryRequired,
            FileName = summary,
            Title = TitleReadable,
            Relative = relative,
        };

        _webPageSettings.Summaries.Add(Summary);

        GlobalSummary = new Summary
        {
            Path = dir.FullName,
            Counter = folderCounter
        };

        //Debug.Print("C: {1:000} P: {0}", dir.FullName, _folderCounter);

        if (_webPageSettings.IsSummaryRequired && !_webPageSettings.IsPrintVersionRequired) _webPageSettings.GlobalSummaries.Add(GlobalSummary);

        if (string.IsNullOrEmpty(Title)) Title = Directory.Name;

        Title = HtmlHelper.RemoveNumbers(Title[..1].ToUpper() + Title[1..], _removeNumbers);
    }

    /// <summary>
    /// Current folder's directory info
    /// </summary>
    public DirectoryInfo Directory { get; }

    /// <summary>
    /// Is a summary required
    /// </summary>
    public bool IsSummaryRequired { get; set; }

    /// <summary>
    /// The name of the first image to set for the slideshow
    /// </summary>
    public string FirstImage { get; set; } = string.Empty;

    /// <summary>
    /// List with all subdirectories
    /// </summary>
    public List<SubfolderData> SubDirectories { get; } = new();

    /// <summary>
    /// List with all images in the folder
    /// </summary>
    public List<ImageData> Images { get; } = new();

    /// <summary>
    /// List with other files than images in the folder
    /// </summary>
    public List<FileData> OtherFiles { get; } = new();

    /// <summary>
    /// Menu item URLs
    /// </summary>
    public List<MenuUrl> MenuUrls { get; set; } = new();

    /// <summary>
    /// Base URL
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Name of the folder's starting page
    /// </summary>
    public string TargetFileName => Path.Combine(Directory.FullName, "index.htm");

    /// <summary>
    /// Name of the folder's print version page
    /// </summary>
    public string PrintTargetFileName => Path.Combine(Directory.FullName, $"{"printer"}.htm");

    //public string SummaryPageFileName => Path.Combine(_dir.FullName, "Zusammenfassung.htm");

    /// <summary>
    /// Get the Open Graph Data for a folder
    /// </summary>
    public string OpenGraphData => HtmlHelper.CreateOpenGraphData(_webPageSettings.OpenGraph, TitleReadable, Description, OpenGraphImageLink,
        BaseUrl + HtmlHelper.StartpageName);

    /// <summary>
    /// Advertising set ID for the current folder
    /// </summary>
    public int AdSetId { get; set; }

    /// <summary>
    /// Content for META-Tag Keywords
    /// </summary>
    public string Keywords
    {
        get => (string.IsNullOrEmpty(_keywords) ? $"{_defaultKeywords},{Title.Replace("_", " ")}" : _keywords).Replace(";", ",");
        set => _keywords = value;
    }

    /// <summary>
    /// Content for META-Tag Keywords
    /// </summary>
    public string Description
    {
        get => string.IsNullOrEmpty(_description) ? $"{Title.Replace("_", " ")} {_defaultDescription}" : _description;
        set => _description = value;
    }

    /// <summary>
    /// Images PDF
    /// </summary>
    public StringBuilder ImagesPdf { get; set; }

    /// <summary>
    /// Other files PDF
    /// </summary>

    public StringBuilder OtherFilesPdf { get; set; }

    /// <summary>
    /// Urls for PDF
    /// </summary>
    public StringBuilder UrlPdf { get; set; }

    /// <summary>
    /// Content of the current summary
    /// </summary>
    public Summary Summary { get; }

    /// <summary>
    /// Content of the global summary
    /// </summary>
    public Summary GlobalSummary { get; }

    /// <summary>
    /// Check the filenames in the folder if they are valid for the web
    /// </summary>
    public void CheckFileNames()
    {
        var fiArr = Directory.GetFiles();
        foreach (var fri in fiArr)
        {
            FileHelper.CheckFileName(fri);
        }
    }

    /// <summary>
    /// Get the filename for Open Graph Opimization image files
    /// </summary>
    public string OpenGraphImageFileName => Path.Combine(Directory.FullName, OpenGraphHelper.OpenGraphFileName);

    /// <summary>
    /// Get the link to the Open Graph Opimization image files
    /// </summary>
    public string OpenGraphImageLink => BaseUrl + OpenGraphHelper.OpenGraphFileName;

    /// <summary>
    /// Image for the header
    /// </summary>
    public string HeaderImage { get; set; } = string.Empty;

    /// <summary>
    /// Folder title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Readable folder title
    /// </summary>
    public string TitleReadable { get; set; } = string.Empty;

    /// <summary>
    /// Create include text if an include file (toc.inc or index.inc) is available for the folder webpage
    /// </summary>
    /// <returns>Text to add</returns>
    public string AddIndexIncludeText
    {
        get
        {
            if (!string.IsNullOrEmpty(_includeText))
            {
                return _includeText;
            }

            var toc = Path.Combine(Directory.FullName, "toc.inc");

            var inc = Path.Combine(Directory.FullName, "index.inc");

            if (File.Exists(toc))
            {
                File.Move(toc, inc);
            }

            var s = HtmlHelper.ReadFile(inc);

            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            s = HtmlHelper.CheckIncludes(Directory.FullName, s);

            s = HtmlHelper.CheckIncludeImages(s, Directory.FullName, Images);


            _includeText = HtmlHelper.Parser(s).Replace("??Disclaimer??", _webPageSettings.Disclaimer)
                .Replace("??Company??", _webPageSettings.Company).Replace("??Author??", _webPageSettings.Author);

            return _includeText;
        }
    }

    /// <summary>
    /// Create the text with the slideshow images for the folder webpage
    /// </summary>
    /// <returns>Text to add</returns>

    public void AddSlideShowImages(StringBuilder html)
    {
        try
        {
            if (Images.Count == 0)
            {
                return;
            }

            foreach (var image in Images.Where(x => x.SlideShow))
            {
                var file = image.TargetPath;

                var size = new ImageSize
                {
                    Width = image.Width,
                    Height = image.Height
                };

                var fri = new FileInfo(file);
                var thumbFile = fri.FullName.Replace(fri.Extension, $"_t{fri.Extension}");

                if (image.Thumbnail && !_webPageSettings.ForceThumbnailCreation)
                {
                    size = File.Exists(thumbFile) ?
                        ImageTools.GetImageSize(thumbFile) :
                        ImageHelper.GenerateWebImage(fri);
                }
                else
                {
                    FileSystemHelper.DeleteFile(thumbFile);
                }

                IconHelper.AddExtension(fri);

                var img = HtmlHelper.HtmlImgBildarchiv(fri, string.Empty);

                if (string.IsNullOrEmpty(img))
                {
                    continue;
                }

                html.AppendFormat("{0}\r\n", img);

                //erg.AppendFormat("<li>{0}</li>",
                //    size.Width >= size.Height
                //        ? HtmlHelper.HtmlImgBildarchiv(fri, null, "quer")
                //        : HtmlHelper.HtmlImgBildarchiv(fri, null, "hoch"));

                ImagesPdf.AppendFormat("<p>{0}</p>",
                    size.Width >= size.Height
                        ? HtmlHelper.HtmlImgPlain($"??sub??{fri.Name}", fri.Name, "quer")
                        : HtmlHelper.HtmlImgPlain($"??sub??{fri.Name}", fri.Name, "hoch"));

                FirstImage ??= fri.Name;
            }

            if (html.Length < 1)
            {
                return;
            }

            html.Insert(0, "<div class=\"bildarchiv\">\r\n<div id=\"links\">\r\n");
            html.Append("\r\n</div>\r\n</div>\r\n");
        }
        catch //(Exception ex)
        {
            //var s = ex.Message;
        }
    }

    /// <summary>
    ///  Create the text with other files for the folder webpage
    /// </summary>
    /// <param name="erg">Website text</param>
    public void AddOtherFiles(StringBuilder erg)
    {
        foreach (var fri in OtherFiles.OrderBy(x => x.Name))
        {
            try
            {
                var file = fri.Name.ToLower();
                var inc = HtmlHelper.ReadFile(fri.FullName.Replace(fri.Extension, ".inc"));
                var icon = $"{fri.Extension.Replace(".", "")}.gif";

                if (string.IsNullOrEmpty(inc))
                {
                    var s =
                        $@"<p class=""Files""><img src=""??rel??images/{icon}"" alt=""Icon"" class=""icon""/>{HtmlHelper.HtmlLink1($"??sub??{fri.Name}",
                            HtmlHelper.RemoveNumbers(fri.DisplayName, _removeNumbers), null,
                            "_blank", null)}</p>";
                    erg.Append(s);
                    if (file != "zusammenfassung.htm")
                    {
                        OtherFilesPdf.Append(s);
                    }
                }
                else
                {
                    var s =
                        $@"<p class=""Files""><img src=""??rel??images/{icon}"" alt=""Icon"" class=""icon""/>{HtmlHelper.HtmlLink1($"??sub??{fri.Name}",
                            HtmlHelper.RemoveNumbers(fri.DisplayName, _removeNumbers), null,
                            "_blank", null)}</p><p class=""IncludeText"">{inc}</p>";
                    erg.Append(s);
                    if (file != "zusammenfassung.htm")
                    {
                        OtherFilesPdf.Append(s);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"AddOtherFiles: {fri.FullName}", ex);
            }
        }
    }

    /// <summary>
    /// Add .url files with URLs to the webpage
    /// </summary>
    public StringBuilder AddUrlFiles
    {
        get
        {
            var erg = new StringBuilder();

            var files = Directory.EnumerateFiles().Where(x => x.Extension.ToLower() == ".url").OrderBy(x => x.Name);
            foreach (var fri in files)
            {
                var inc = HtmlHelper.ReadFile(fri.FullName.Replace(fri.Extension, ".inc"));

                var u = new FileSystemUrl(fri);
                u.Read();
                erg.AppendFormat(
                    "<p class=\"Files\"><img src=\"??rel??images/htm.gif\" alt=\"Icon\" class=\"icon\"/>{0}</p>",
                    HtmlHelper.HtmlLink1(u.Url ?? string.Empty, u.Caption.Replace("_", " "), null, "_self", null));

                UrlPdf.AppendFormat(
                    "<p class=\"Files\"><img src=\"??rel??images/htm.gif\" alt=\"Icon\" class=\"icon\"/>{1}:&nbsp;{0}</p>",
                    HtmlHelper.HtmlLink1(u.Url ?? string.Empty, u.Url, null, "_self", null), u.Caption.Replace("_", " "));

                if (string.IsNullOrEmpty(inc))
                {
                    continue;
                }

                erg.AppendFormat("<p class=\"IncludeText\">{0}</p>", inc);
                UrlPdf.AppendFormat("<p class=\"IncludeText\">{0}</p>", inc);
            }

            UrlPdf.Replace("\r", "").Replace("\n", "");

            return erg;
        }
    }


    /// <summary>
    /// Add the folder to the sitemap
    /// </summary>
    /// <param name="isSitemapVisible">Is folder visible for the sitemap?</param>
    public void AddDirToSitemap(bool isSitemapVisible)
    {
        if (!isSitemapVisible)
        {
            return;
        }

        var helper = Directory.FullName[_webPageSettings.PathHomepage.Length..];

        var name = Regex.Replace(Directory.Name.Replace("_", " ").Replace(".", ""), @"^\s*\d+[\t ]+", "", RegexOptions.Multiline);

        var anzeige = name + (Directory.Name == helper ? "" : $" ({helper[..(helper.Length - Directory.Name.Length - 1)].Replace("\\", " / ")})");
        //Sitemap ergänzen

        var si = new SiteMapElement
        {
            Freq = "m",
            Id = 999,
            Name = anzeige,
            Prio = 0.1,
            TitleUrl = anzeige,
            Url = $"/{helper.Replace("\\", "/")}/index.htm"
        };
        _webPageSettings.SitemapItems.Add(si);
    }

    /// <summary>
    /// Create the sublinks for the folder webpage
    /// </summary>
    /// <returns>Text to add</returns>
    public string GetSubLinks()
    {
        var link = HtmlHelper.FormatLinks(MenuUrls, _webPageSettings);

        return _webPageSettings.LinksTop.Replace("??LinksTopRow??", link.ToString());
    }

    /// <summary>
    /// Add the summary conetnt
    /// </summary>
    public void AddSummaryContent()
    {
        var summaryContent = Summary.Content;
        summaryContent.Append(Header);
        summaryContent.Append(AddIndexIncludeText);

        if (UrlPdf.Length > 0)
        {
            summaryContent.Append($"<div class=\"FilesArea\">");
            summaryContent.Append(UrlPdf);
            summaryContent.Append("</div>");
        }

        summaryContent.Append(ImagesPdf);
    }

    /// <summary>
    /// Get file area
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public void GetFileArea(StringBuilder html)
    {
        var url = AddUrlFiles;
        var filesArea = new StringBuilder("\r\n??Link1??");
        AddOtherFiles(filesArea);

        if (url.Length > 0)
        {
            filesArea.Append(url);
        }

        var link1 = new StringBuilder();

        foreach (var folder in SubDirectories)
        {
            //var anzeigeName = _basis.DatabaseHandler.GetFolderLinkName(folder.FullName);

            var anzeigeName = HtmlHelper.RemoveNumbers(folder.DisplayName, _removeNumbers);

            link1.Append("<p class=\"Files\">");
            link1.AppendHtmlLink($"{folder.Name}/index.htm", anzeigeName, "text-decoration:none;", _webPageSettings.OrdnerZu, anzeigeName);
            link1.Append("</p>");
        }

        filesArea.Replace("??Link1??", link1.ToString()).Replace("\r", "").Replace("\n", "");

        if (filesArea.Length < 1)
        {
            html.Append(filesArea);
            return;
        }

        filesArea.Insert(0, "<div class=\"FilesArea\">");
        filesArea.Append("</div>");

        html.Append(filesArea);
    }

    /// <summary>
    /// HTML code for the Zusammenfassung
    /// </summary>
    public string UrlZusammenfassung =>
        IsSummaryRequired
            ? "<a href=\"Zusammenfassung.htm\" title=\"Druckbare Zusammenfassung\"><img src=\"??rel??images/link_intern.png\" alt=\"Druckbare Zusammenfassung\"> druckbare Zusammenfassung</a>"
            : "";

    public string Header => $@"<h1><a name=""{Directory.Name}"">{Title}</a></h1><p class=""smallBlack marginbottom"">Autor: ??Company??, ??Author??</p>";


    public void Export(bool sitemapVisible)
    {
        var html = new StringBuilder("??Inc??");

        // Include text
        html.Replace("??Inc??", AddIndexIncludeText);

        // Get the file area
        GetFileArea(html);

        // Images for slideshow
        AddSlideShowImages(html);

        AddSummaryContent();

        var p = new HtmlPage(_appGlobals)
        {
            Master = _webPageSettings.HtmlMaster,
            Author = _webPageSettings.Author,
            Publisher = _webPageSettings.Company,
            Company = _webPageSettings.Company,
            CompanyShort = _webPageSettings.CompanyShort,
            Description = Description,
            FeedbackLink = _webPageSettings.FeedbackLink,
            Keywords = _webPageSettings.Keywords,
            LinksMainNavi = _webPageSettings.MainLinks,
            LinksBottomNavi = _webPageSettings.BottomLinks,
            FirstImage = FirstImage,
            Content = html.ToString(),
            IsAvailableForRobots = sitemapVisible,
            DateLastChange = DateTime.Now,
            PageTitle = TitleReadable,
            TargetFile = TargetFileName,
            SlideShowJavascript = _webPageSettings.SlideShowMaster,
            BodyJavascript = _webPageSettings.BodyJsSlideShow,
            Relative = _relative,
            Url = _webPageSettings.HttpUrl,
            VCard = _webPageSettings.VCard,
            OpenGraphData = OpenGraphData,
            Disclaimer = _webPageSettings.Disclaimer,
            AnalyticsJavascript = _webPageSettings.AnalyticsJavascript,
            SummaryLink = UrlZusammenfassung,
            HeaderImage = HeaderImage,
            WebsiteUrl = _webPageSettings.HttpUrl,
        };

        // Create Open Graph Image for this folder
        OpenGraphHelper.Save(TitleReadable, Description, OpenGraphImageFileName);

        var link = GetSubLinks();
        if (link.Length > 0)
        {
            p.LinksSubNavi = link;
        }

        HtmlHelper.AddAdvertising(_webPageSettings, p, AdSetId);

        if (_webPageSettings.IsPrintVersionRequired) p.PrintVersion = "printer.htm";
        p.Save();

        // Get all parser errors
        foreach (var err in p.ParserErrors)
        {
            _webPageSettings.ParserErrors.Add(err);
        }

        if (!_webPageSettings.IsPrintVersionRequired)
        {
            return;
        }

        var pdf = new HtmlPagePrint(_webPageSettings.HtmlPdf)
        {
            Company = _webPageSettings.Company,
            CompanyShort = _webPageSettings.CompanyShort,
            Author = _webPageSettings.Author,
            Title = TitleReadable,
            FileName = PrintTargetFileName,
            Relative = _relative,
            Subfolder = string.Empty
        };

        pdf.Add(Summary.Content);

        pdf.Save();
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using BodoCmsConsole.Business.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Windows.FileSystem;
using Bodoconsult.Drawing;
using Bodoconsult.Drawing.Model;

namespace BodoCmsConsole.Business.Model;

[SupportedOSPlatform("windows")]
public class WebPageFolder
{
    public DirectoryInfo Directory { get; }

    private readonly bool _removeNumbers;

    public bool IsSummary { get; set; }

    private string _includeText = string.Empty;


    public string FirstImage { get; set; } = string.Empty;


    public List<SubfolderData> SubDirectories { get; } = new();


    public List<ImageData> Images { get;  } = new();


    public List<FileData> OtherFiles { get;  } = new();

    public List<MenuUrl> MenuUrls { get; set; } = new();

    public string BaseUrl { get; set; } 

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

    private readonly IAppLoggerProxy? _logger;

    public StringBuilder ImagesPdf { get; set; }

    public StringBuilder OtherFilesPdf { get; set; }

    public StringBuilder UrlPdf { get; set; }

    private string _keywords = string.Empty;
    private string _description = string.Empty;
    private readonly string _defaultDescription;
    private readonly string _defaultKeywords;

    private readonly WebPageSettings _webPageSettings;

    public Summary Summary { get; }

    public Summary GlobalSummary { get; }


    private readonly string _relative;
    private readonly IAppGlobals _appGlobals;

    //private readonly FolderData _folderData;


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
            IsSummary = folderData.Summary;
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

        if (!IsSummary && File.Exists(summary))
        {
            File.Delete(summary);
        }

        Summary = new Summary
        {
            Path = dir.FullName,
            Counter = folderCounter,
            IncludeSummariesForSubfolders = IsSummary,
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

    public string AddIndexIncludeText
    {
        get
        {
            if (!string.IsNullOrEmpty(_includeText)) return _includeText;

            var toc = Path.Combine(Directory.FullName, "toc.inc");

            var inc = Path.Combine(Directory.FullName, "index.inc");

            if (File.Exists(toc)) File.Move(toc, inc);

            var s = HtmlHelper.ReadFile(inc);

            if (string.IsNullOrEmpty(s)) return string.Empty;

            s = HtmlHelper.CheckIncludes(Directory.FullName, s);

            s = HtmlHelper.CheckIncludeImages(s, Directory.FullName, Images);


            _includeText = HtmlHelper.Parser(s).Replace("??Disclaimer??", _webPageSettings.Disclaimer)
                .Replace("??Company??", _webPageSettings.Company).Replace("??Author??", _webPageSettings.Author);

            return _includeText;
        }

    }




    public StringBuilder AddSlideShowImages()
    {
        var erg = new StringBuilder();

        try
        {

            if (!Images.Any())
            {
                return erg;
            }

            foreach (var image in Images.Where(x=> x.SlideShow))
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
                    if (File.Exists(thumbFile))
                    {
                        File.Delete(thumbFile);
                    }
                }

                IconHelper.AddExtension(fri);

                erg.AppendFormat("{0}\r\n", HtmlHelper.HtmlImgBildarchiv(fri, string.Empty));

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


            if (erg.Length < 1) return erg;
            erg.Insert(0, "<div class=\"bildarchiv\">\r\n<div id=\"links\">\r\n");
            erg.Append("\r\n</div>\r\n</div>\r\n");

            return erg;
        }
        catch //(Exception ex)
        {

            //var s = ex.Message;

            return erg;
        }
    }


    public StringBuilder AddOtherFiles()
    {
        var erg = new StringBuilder();

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
                    if (file != "zusammenfassung.htm") OtherFilesPdf.Append(s);
                }
                else
                {
                    var s =
                        $@"<p class=""Files""><img src=""??rel??images/{icon}"" alt=""Icon"" class=""icon""/>{HtmlHelper.HtmlLink1($"??sub??{fri.Name}",
                            HtmlHelper.RemoveNumbers(fri.DisplayName, _removeNumbers), null,
                            "_blank", null)}</p><p class=""IncludeText"">{inc}</p>";
                    erg.Append(s);
                    if (file != "zusammenfassung.htm") OtherFilesPdf.Append(s);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"AddOtherFiles: {fri.FullName}", ex);
            }
        }

        return erg;
    }

    /// <summary>
    /// 
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

                if (string.IsNullOrEmpty( inc))
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
    /// 
    /// </summary>
    /// <param name="sitemapVisible"></param>
    public void AddDirToSitemap(bool sitemapVisible)
    {
        if (!sitemapVisible)
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
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetSubLinks()
    {
        var link = HtmlHelper.FormatLinks(MenuUrls, _webPageSettings);

        return _webPageSettings.LinksTop.Replace("??LinksTopRow??", link.ToString());
    }

    /// <summary>
    /// Name of the folder's starting page
    /// </summary>
    public string TargetFileName => Path.Combine(Directory.FullName, "index.htm");

    /// <summary>
    /// Name of the folder's print version page
    /// </summary>
    public string PrintTargetFileName => Path.Combine(Directory.FullName, $@"{"printer"}.htm");

    //public string SummaryPageFileName => Path.Combine(_dir.FullName, "Zusammenfassung.htm");

    /// <summary>
    /// Get the Open Graph Data for a folder
    /// </summary>
    public string OpenGraphData => HtmlHelper.CreateOpenGraphData(_webPageSettings.OpenGraph, TitleReadable, Description, OpenGraphImageLink,
        BaseUrl + HtmlHelper.StartpageName);

    /// <summary>
    /// Id des Advertising sets für aktuellen ordner
    /// </summary>
    public int AdSetId { get; set; }


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
    /// <returns></returns>
    public StringBuilder GetFileArea()
    {

        var url = AddUrlFiles;

        var filesArea = new StringBuilder("\r\n??Link1??");
        filesArea.Append(AddOtherFiles());

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
            link1.AppendHtmlLink($@"{folder.Name}/index.htm", anzeigeName, "text-decoration:none;", _webPageSettings.OrdnerZu, anzeigeName);
            link1.Append("</p>");
        }

        filesArea.Replace("??Link1??", link1.ToString()).Replace("\r", "").Replace("\n", "");


        if (filesArea.Length < 1)
        {
            return filesArea;
        }

        filesArea.Insert(0, "<div class=\"FilesArea\">");
        filesArea.Append("</div>");

        return filesArea;
    }

    /// <summary>
    /// 
    /// </summary>
    public string UrlZusammenfassung
    {
        get => IsSummary
            ? "<a href=\"Zusammenfassung.htm\" title=\"Druckbare Zusammenfassung\"><img src=\"??rel??images/link_intern.png\" alt=\"Druckbare Zusammenfassung\"> druckbare Zusammenfassung</a>"
            : "";

    }

    public string Header => $@"<h1><a name=""{Directory.Name}"">{Title}</a></h1><p class=""smallBlack marginbottom"">Autor: ??Company??, ??Author??</p>";


    public void Export(bool sitemapVisible)
    {
        var html = new StringBuilder("??Inc??");

        // Include text
        html.Replace("??Inc??", AddIndexIncludeText);

        // Get the file area
        html.Append(GetFileArea());

        // Images for slideshow
        html.Append(AddSlideShowImages());

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


        // Open Graph Image erzeugen für diesen Ordner
        OpenGraphHelper.Save(TitleReadable, Description, OpenGraphImageFileName);

        var link = GetSubLinks();
        if (link.Length > 0) p.LinksSubNavi = link;

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
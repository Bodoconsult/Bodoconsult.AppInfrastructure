// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using BodoCmsConsole.Business.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Drawing;

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Represents a web page
/// </summary>
public class WebPage
{
    //private readonly WebBasis _basis;

    private readonly PageData _page;

    private string _printerName = string.Empty;

    private readonly string _pageAuthor;

    private readonly WebPageSettings _webPageSettings;
    private string _fileName =   string.Empty;

    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Images for this page
    /// </summary>
    public List<ImageData> Images { get; set; } = new();

    public WebPage(WebPageSettings webPageSettings, PageData page, IAppGlobals appGlobals)
    {
        _page = page;
        _webPageSettings = webPageSettings;
        _pageAuthor = string.IsNullOrEmpty(page.Author) ? _webPageSettings.Author : page.Author;
        _appGlobals = appGlobals;
    }


    public string FileName
    {
        get => _fileName;
        set
        {
            _fileName = value;
            _printerName = _fileName.Replace(".htm", "_printer.htm");
        }
    }


    public string HeaderFileLink { get; set; } = string.Empty;


    public string LinksSubNavi { get; set; } = string.Empty;


    public void Export()
    {
        var p = new HtmlPage(_appGlobals)
        {
            Master = _page.Id == 1 ? _webPageSettings.HtmlMasterH : _webPageSettings.HtmlMaster,
            WebsiteUrl = _webPageSettings.HttpUrl,
            HeaderImage = HeaderFileLink    // Header-Image verlinken
        };

        var title = _page.Title;
        var description = $"{_page.Description},{_webPageSettings.DefaultDescription}";

        var openGraphImageLink = FileName.Replace(".htm", "_og.png");

        var content =
            HtmlHelper.CheckIncludeImages(HtmlHelper.CheckIncludes(_webPageSettings.PathHomepage, _page.Content),
                $"{_webPageSettings.PathHomepage}images", Images);

        //var content = _page.Content;

        p.Url = _webPageSettings.HttpUrl;
        p.Relative = string.Empty;
        p.Keywords = $"{_page.Keywords},{_webPageSettings.Keywords}";
        p.Author = _pageAuthor;
        p.Publisher = _webPageSettings.Company;
        p.Company = _webPageSettings.Company;
        p.CompanyShort = _webPageSettings.CompanyShort;
        p.FeedbackLink = _webPageSettings.FeedbackLink;
        p.Description = description;
        p.LinksMainNavi = _webPageSettings.MainLinks;
        p.LinksBottomNavi = _webPageSettings.BottomLinks;
        p.LinksSubNavi = LinksSubNavi;
        p.Content = content;
        p.PageTitle = title;
        p.TargetFile = Path.Combine(_webPageSettings.PathHomepage, FileName);
        p.DateLastChange = _page.DateLastChange;
        p.BodyJavascript = _webPageSettings.BodyJs;
        p.VCard = _webPageSettings.VCard;
        p.AnalyticsJavascript = _webPageSettings.AnalyticsJavascript;

        var s = HtmlHelper.CreateOpenGraphData(_webPageSettings.OpenGraph, title, description, _webPageSettings.HttpUrl + openGraphImageLink, _webPageSettings.HttpUrl + FileName);

        p.OpenGraphData = s;

        if (_webPageSettings.IsPrintVersionRequired)
        {
            p.PrintVersion = _printerName;
        }

        //if (!string.IsNullOrEmpty(dt["AS_W2_Url"].ToString())) p.BannerRight(GetBannerRight(dt["AS_W2_Url"].ToString(), dt["AS_W2_Image"].ToString()));
        //if (!string.IsNullOrEmpty(dt["AS_W1_Url"].ToString())) p.BannerTop(GetBannerTop(dt["AS_W1_Url"].ToString(), dt["AS_W1_Image"].ToString()));

        HtmlHelper.AddAdvertising(_webPageSettings, p, _page.AdSetId);

        p.Save();

        // Get all parser errors
        foreach (var err in p.ParserErrors)
        {
            _webPageSettings.ParserErrors.Add(err);
        }


        // Open Grapg Image erzeugen für diese Seite
        OpenGraphHelper.Save(_page.Title, $"{_page.Description},{_webPageSettings.DefaultDescription}",
            Path.Combine(_webPageSettings.PathHomepage, openGraphImageLink));

        if (!_webPageSettings.IsPrintVersionRequired) return;

        var pdf = new HtmlPagePrint(_webPageSettings.HtmlPdf)
        {
            Company = _webPageSettings.Company,
            CompanyShort = _webPageSettings.CompanyShort,
            Author = _pageAuthor,
            Title = _page.Title,
            FileName = Path.Combine(_webPageSettings.PathHomepage, _printerName),
            Relative = string.Empty
        };

        pdf.Add("<h1>??PageTitle??</h1>\r\n<p class=\"smallBlack marginbottom\" > Autor: ??Company??, ??Author??</p>");
        pdf.Add(content);
        pdf.Save();

        if (!_webPageSettings.IsSummaryRequired || _page.Id != 1)
        {
            return;
        }

        pdf = new HtmlPagePrint(_webPageSettings.HtmlPdf)
        {
            Company = _webPageSettings.Company,
            CompanyShort = _webPageSettings.CompanyShort,
            Author = _pageAuthor,
            Title = _page.Title,
            FileName = Path.Combine(_webPageSettings.PathHomepage, "Zusammenfassung.htm"),
            Relative = string.Empty,
            Subfolder = string.Empty
        };
        pdf.Add("<h1>??PageTitle??</h1>\r\n<p>Autor: ??Company??, ??Author??</p>");
        pdf.Add(content);

        foreach (var summary in _webPageSettings.GlobalSummaries)
        {
            pdf.Add(summary.Content);
        }

        pdf.Save();
    }

}
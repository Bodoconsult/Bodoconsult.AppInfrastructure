// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using BodoCmsConsole.Business.Helpers;

namespace BodoCmsConsole.Business.Model;

public class WebPageSettings
{
    private string _httpUrl = string.Empty;
    private string _pathHomepage = string.Empty;
    private string _pathMaster = string.Empty;
    private readonly Random _rand = new Random();

    /// <summary>
    /// Path for saving the website in
    /// </summary>
    public string PathHomepage
    {
        get => _pathHomepage;
        set
        {
            if (value[^2..] != @"\")
            {
                value += @"\";
            }
            _pathHomepage = value;

            IncludePath = Path.Combine(_pathHomepage, "includes");
        }
    }

    /// <summary>
    /// Path to the master template
    /// </summary>
    public string PathMaster
    {
        get => _pathMaster;
        set
        {
            if (!value.EndsWith(@"\"))
            {
                value += @"\";
            }
            _pathMaster = value;

            SitemapGz = $"{_pathMaster}sitemap.gz";
            SitemapGzElement = $"{_pathMaster}sitemap_element.gz";

            HtmlMaster = HtmlHelper.ReadFile($"{_pathMaster}basis.htm");
            HtmlMasterH = HtmlHelper.ReadFile($"{_pathMaster}basish.htm");

            if (File.Exists($"{_pathMaster}pdf.htm"))
            {
                HtmlPdf = HtmlHelper.ReadFile($"{_pathMaster}pdf.htm");
            }

            IsPrintVersionRequired = !string.IsNullOrEmpty(HtmlPdf);

            SlideShowMaster = HtmlHelper.ReadFile($@"{PathMaster}prototypes\SlideShow.prot");

            LinksLeft = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksLeft.prot");
            LinksLeftElement = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksLeftElement.prot");

            LinksBottom = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksBottom.prot");
            LinksBottomElement = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksBottomElement.prot");

            LinksTop = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksTop.prot");
            LinksTopRow = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksTopRow.prot");
            LinksTopRowElement = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksTopRowElement.prot");
            LinksTopRow2 = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksTopRow2.prot");
            LinksTopRowElement2 = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\LinksTopRowElement2.prot");

            BodyJs = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\BodyJS.prot");
            BodyJsSlideShow = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\BodyJSSlideShow.prot");

            BannerTop = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\BannerTop.prot");
            BannerRight = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\BannerRight.prot");

            VCard = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\VCard.prot");
            OpenGraph = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\OpenGraph.prot");
            Disclaimer = HtmlHelper.ReadFile($@"{_pathMaster}prototypes\Disclaimer.prot");
        }
    }

    /// <summary>
    /// Company
    /// </summary>
    public string Company { get; set; } = string.Empty;

    /// <summary>
    /// Company name short version
    /// </summary>
    public string CompanyShort { get; set; } = string.Empty;

    /// <summary>
    /// Default description to be added for all pages
    /// </summary>
    public string DefaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// List of all errors registered during website creation
    /// </summary>
    public List<string> ParserErrors { get;  } = new();

    /// <summary>
    /// Author
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Include path
    /// </summary>
    public string IncludePath { get; set; } = string.Empty;

    /// <summary>
    /// VCard
    /// </summary>
    public string VCard { get; set; } = string.Empty;

    /// <summary>
    /// Links on the left side
    /// </summary>
    public string LinksLeft { get; set; } = string.Empty;

    /// <summary>
    /// Links on the left side elements
    /// </summary>
    public string LinksLeftElement { get; set; } = string.Empty;

    /// <summary>
    /// Links on the top
    /// </summary>
    public string LinksTop { get; set; } = string.Empty;

    /// <summary>
    /// Links on the top row
    /// </summary>
    public string LinksTopRow { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string LinksTopRowElement { get; set; } = string.Empty;


    /// <summary>
    /// 
    /// </summary>
    public string LinksTopRow2 { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string LinksTopRowElement2 { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string LinksBottom { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string LinksBottomElement { get; set; } = string.Empty;
    
    /// <summary>
    /// OpenGraph content
    /// </summary>
    public string OpenGraph { get; set; } = string.Empty;

    /// <summary>
    /// Disclaimer
    /// </summary>
    public string Disclaimer { get; set; } = string.Empty;

    /// <summary>
    /// Prefic for relative URLs
    /// </summary>
    public string UrlRelZusatz { get; set; } = string.Empty;
    
    /// <summary>
    /// Summaries
    /// </summary>
    public List<Summary> Summaries { get;  } = new();

    /// <summary>
    /// All summaries of the global summary
    /// </summary>
    public List<Summary> GlobalSummaries { get;  } = new();

    /// <summary>
    /// Should a print version be added
    /// </summary>
    public bool IsPrintVersionRequired { get; set; }

    /// <summary>
    /// HTML master template
    /// </summary>
    public string HtmlMaster { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string HtmlMasterH { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string HtmlPdf { get; set; } = string.Empty;

    /// <summary>
    /// Template for slideshows
    /// </summary>
    public string SlideShowMaster = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string MainLinks { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string BottomLinks { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string OrdnerZu { get; set; } = @"images\ordner_zu.gif";

    /// <summary>
    /// Keyword
    /// </summary>
    public string Keywords { get; set; } = string.Empty;

    /// <summary>
    /// Javascript related to Analytics 
    /// </summary>
    public string AnalyticsJavascript { get; set; } = string.Empty;
    
    /// <summary>
    /// Is a summary required?
    /// </summary>
    public bool IsSummaryRequired { get; set; }

    /// <summary>
    /// JS for the body
    /// </summary>
    public string BodyJs { get; set; } = string.Empty;

    /// <summary>
    /// JS for the slideshow
    /// </summary>
    public string BodyJsSlideShow { get; set; } = string.Empty;

    /// <summary>
    /// Feedback link
    /// </summary>

    public string FeedbackLink { get; set; } = string.Empty;

    /// <summary>
    /// HTTP url
    /// </summary>
    public string HttpUrl
    {
        get => _httpUrl;
        set
        {
            if (!value.EndsWith("/"))
            {
                value += "/";
            }
            _httpUrl = value;
        }
    }

    /// <summary>
    /// Sitemap
    /// </summary>
    public string Sitemap { get; set; } = string.Empty;
        
    /// <summary>
    /// 
    /// </summary>
    public string SitemapGz { get; set; } = string.Empty;
        
    /// <summary>
    /// 
    /// </summary>
    public string SitemapGzElement { get; set; } = string.Empty;

    /// <summary>
    /// List of advertising set data 
    /// </summary>
    public List<AdSetData> Adsets { get; } = new();

    /// <summary>
    /// Current running counter for advertising set data 
    /// </summary>
    public int CounterAdsets { get; set; }

    /// <summary>
    /// Top banner
    /// </summary>
    public string BannerTop { get; set; } = string.Empty;

    /// <summary>
    /// Right banner
    /// </summary>
    public string BannerRight { get; set; } = string.Empty;
    
    /// <summary>
    /// Sitemap items
    /// </summary>
    public List<SiteMapElement> SitemapItems { get;  } = new();

    /// <summary>
    /// List with all adset IDs
    /// </summary>
    public List<int> AdSetsIds { get; set; } = new();

    /// <summary>
    /// Sitempa filename without path
    /// </summary>
    public string SitemapFileName { get; set; } = string.Empty;

    /// <summary>
    /// Start page filename without path
    /// </summary>
    public string HomepageFileName { get; set; } = string.Empty;

    /// <summary>
    /// File extensions for web pages, i.e. htm, html 
    /// </summary>
    public string WebpageFileExtension { get; set; } = string.Empty;

    /// <summary>
    /// If true, all thumbnails will be created new. Otherwise the existing thumbs will be used, the missinh thumbs are created new.
    /// </summary>
    public bool ForceThumbnailCreation { get; set; }

    /// <summary>
    /// Get the next random advertising
    /// </summary>
    /// <returns>Random adset ID</returns>
    public int GetNextRandomAdsetId()
    {
        return _rand.Next(CounterAdsets - 1);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Web;
using BodoCmsConsole.Business.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Represents an HTML page
/// </summary>
public class HtmlPage
{
    private readonly HtmlStringBuilder _master;
    private string _author = string.Empty;
    private string _company = string.Empty;
    private string _companyShort = string.Empty;
    private string _disclaimer = string.Empty;
    private string _pageTitle = string.Empty;
    private string _relative = string.Empty;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">App globals</param>
    public HtmlPage(IAppGlobals appGlobals)
    {
        _master = new HtmlStringBuilder(100000)
        {
            StatusMessageDelegate = appGlobals.StatusMessageDelegate
        };
    }

    /// <summary>
    /// Url of the main website
    /// </summary>
    public string WebsiteUrl { get; set; } = string.Empty;

    /// <summary>
    /// List of all parser errors registered during saving the page (after Save())
    /// </summary>
    public List<string> ParserErrors => _master.ParserErrors;

    /// <summary>
    /// Is page available for robots
    /// </summary>
    public bool IsAvailableForRobots
    {
        set => _master.Replace("??Robots??", value ? "index,follow" : "noindex,nofollow");
    }

    /// <summary>
    /// Dateiname der HTML-Seite setzen
    /// </summary>
    public string TargetFile
    {
        set => _master.TargetFile = value;
    }

    /// <summary>
    /// Relative path
    /// </summary>
    public string Relative
    {
        set => _relative = value;
    }

    /// <summary>
    /// Comapany
    /// </summary>
    public string Company
    {
        set => _company = HttpUtility.HtmlEncode(value);
    }

    /// <summary>
    /// Shortname of the company
    /// </summary>
    public string CompanyShort
    {
        set { _companyShort = HttpUtility.HtmlEncode(value); _master.Replace("??CompanyShort??", _companyShort); }
    }

    /// <summary>
    /// Load a disclaimer placed in the html body with ??Disclaimer??
    /// </summary>
    public string Disclaimer
    {
        set => _disclaimer = HttpUtility.HtmlEncode(value);
    }

    /// <summary>
    /// HeaderImage festlegen
    /// </summary>
    public string HeaderImage
    {
        set => _master.Replace("??HeaderImage??", value);
    }

    /// <summary>
    /// Alternative name of the file
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Page title
    /// </summary>
    public string PageTitle
    {
        set => _pageTitle = value;
    }

    /// <summary>
    /// Author
    /// </summary>
    public string Author
    {
        set
        {
            _author = HttpUtility.HtmlEncode(value);
            _master.Replace("??Author??", _author);
        }
    }


    /// <summary>
    /// Publisher
    /// </summary>
    public string Publisher
    {
        set => _master.Replace("??Publisher??", HttpUtility.HtmlEncode(value));
    }

    /// <summary>
    /// Link to the print version of the page
    /// </summary>
    public string PrintVersion
    {
        set => _master.Replace("??Print??", value);
    }

    /// <summary>
    /// Link to summary or null
    /// </summary>
    public string SummaryLink
    {
        set => _master.Replace("??Summary??", value);
    }

    /// <summary>
    /// Keywords
    /// </summary>
    public string Keywords
    {
        set => _master.Replace("??Keywords??", HttpUtility.HtmlEncode(value.Replace(";", ",")));
    }

    /// <summary>
    /// Page content
    /// </summary>
    public string Content
    {
        set
        {
            _master.Replace("??PageContent??", value);
            _master.Replace("??PageContentJson??", HttpUtility.HtmlEncode(value));
        }
    }

    /// <summary>
    /// Set first image for slideshow
    /// </summary>
    public string FirstImage
    {
        set => _master.Replace("??Pic1??", value);
    }

    /// <summary>
    /// Images for the slideshow
    /// </summary>
    public string Slides
    {
        set => _master.Replace("??Slides??", value);
    }

    /// <summary>
    /// Insert a vCard
    /// </summary>
    public string VCard
    {
        set => _master.Replace("??vCard??", value);
    }

    /// <summary>
    /// Page description
    /// </summary>
    public string Description
    {
        set => _master.Replace("??Description??", HttpUtility.HtmlEncode(value));
    }

    /// <summary>
    /// Links for main navigation bar
    /// </summary>
    public string LinksMainNavi
    {
        set => _master.Replace("??LinksLeft??", value);
    }

    /// <summary>
    /// Links for bottom navigation bar
    /// </summary>
    public string LinksBottomNavi
    {
        set => _master.Replace("??LinksDown??", value);
    }

    /// <summary>
    /// Links for subnavigation bar at the tops
    /// </summary>
    public string LinksSubNavi
    {
        set => _master.Replace("??LinksTop??", value);
    }

    /// <summary>
    /// Javascript for Slideshow
    /// </summary>
    public string SlideShowJavascript
    {
        set => _master.Replace("??SlideShowJS??", value);
    }

    /// <summary>
    /// Javascript for common HTML body
    /// </summary>
    public string BodyJavascript
    {
        set => _master.Replace("??BodyJS??", value);
    }

    /// <summary>
    /// Feedback link
    /// </summary>
    public string FeedbackLink { get; set; } = string.Empty;

    /// <summary>
    /// HTML master
    /// </summary>
    public string Master
    {
        set => _master.Add(value);
    }

    /// <summary>
    /// Date of the last change of the page
    /// </summary>
    public DateTime DateLastChange
    {
        set
        {
            var date = $"{value:yyyy-MM-dd}";

            _master.Replace("??Date??", date);

            date = $"{value:dd.MM.yyyy HH:mm}";

            _master.Replace("??LastChanged??", date);
        }
    }
    
    /// <summary>
    /// HTML code for the top banner
    /// </summary>
    /// <param name="html">HTML code</param>
    public void BannerTop(string html)
    {
        _master.Replace("??BannerTop??", html);
    }

    /// <summary>
    /// HTML code for the right banner
    /// </summary>
    /// <param name="html">HTML code</param>
    public void BannerRight(string html)
    {
        _master.Replace("??BannerRight??", html);
    }

    /// <summary>
    /// OpenGraph data
    /// </summary>
    public string OpenGraphData
    {
        set => _master.Replace("??og_data??", value);
    }

    /// <summary>
    /// Save the page as HTML file
    /// </summary>
    public void Save()
    {
        SaveInternal();
    }

    private void SaveInternal()
    {
        _master.Replace("??PageTitle??", HttpUtility.HtmlEncode(_pageTitle));
        if (_pageTitle.Contains("<") && _pageTitle.Contains(">"))
        {
            //_master.Replace("??PageTitle??", _pageTitle);
            _master.Replace("??PageTitleHeader??", HttpUtility.HtmlEncode(HtmlHelper.StripHtml(_pageTitle.Replace("<br>", " ").Replace("<br/>", " ").Replace("<br />", " "))));
        }
        else
        {
            // _master.Replace("??PageTitle??", HttpUtility.HtmlEncode(_pageTitle));
            _master.Replace("??PageTitleHeader??", HttpUtility.HtmlEncode(_pageTitle));
        }

        _master.Replace("??analytics??", AnalyticsJavascript);
        _master.Replace("??Disclaimer??", _disclaimer);

        // Umlaute ersetzen
        _master.Replace("Ä", "&Auml;");
        _master.Replace("ä", "&auml;");
        _master.Replace("ü", "&uuml;");
        _master.Replace("Ü", "&Uuml;");
        _master.Replace("ö", "&ouml;");
        _master.Replace("Ö", "&Ouml;");
        _master.Replace("ß", "&szlig;");
        _master.Replace("€", "&euro;");
        _master.Replace("©", "&copy;");
        _master.Replace("®", "&reg;");
        _master.Replace("£", "&pound;");
        _master.Replace("$", "&#36;");
        _master.Replace("“", "&bdquo;");
        _master.Replace("”", "&rdquo;");
        _master.Replace("™", "&#153;");

        _master.Replace("”", "&rdquo;");
        _master.Replace("„", "&bdquo;");
        _master.Replace("„", "&#34;");
        _master.Replace("§", "&#167;");



        //
        //_master.Replace("", "");
        //_master.Replace("", "");



        // Übriggebliebene Tags entfernen (nur im Release wegen Übersichtlichkeit HTML)
        //#if (DEBUG)

        //#else

        //            //_master.Replace("\r", " \n");
        //            //_master.Replace("\n", " \n");
        //            _master.Replace("\t", "??a??");
        //            _master.Replace("\r", "??a??");
        //            _master.Replace("\n", "??a??");
        //            _master.Replace("       ", "");
        //            _master.Replace("      ", "");
        //            _master.Replace("     ", "");
        //            _master.Replace("    ", "");
        //            _master.Replace("   ", "");
        //            _master.Replace("  ", " ");
        //            _master.Replace("  ", "");
        //            //_master.Replace("??a????a????a??", "??a??");
        //            //_master.Replace("??a????a??", "??a??");
        //            //_master.Replace("??a????a??", "??a??");
        //            //_master.Replace("??a????a??", "??a??");
        //            //_master.Replace("??a????a????a??", "??a??");
        //            //_master.Replace("??a????a??", "??a??");
        //          _master.Replace("??a??", "");
        //#endif

        _master.Replace("??WebsiteUrl??", WebsiteUrl);

        _master.Replace("??Url??", Url);
        _master.Replace("??Publisher??", "");
        _master.Replace("??Description??", "");
        _master.Replace("??PageTitle??", "");
        _master.Replace("??PageTitleHeader??", "");
        _master.Replace("??Author??", _author);
        _master.Replace("??Keywords??", "");
        _master.Replace("??Company??", _company);
        _master.Replace("??CompanyShort??", _companyShort);
        _master.Replace("??Firma??", _company);
        _master.Replace("??FBLINK??", FeedbackLink);
        _master.Replace("??FeedbackLink??", FeedbackLink);
        _master.Replace("??PageContent??", "");
        _master.Replace("??LinksLeft??", "");
        _master.Replace("??LinksTop??", "");
        _master.Replace("??LinksDown??", "");
        _master.Replace("??Pic1??", "");
        _master.Replace("??Slides??", "");
        _master.Replace("??HeaderImage??", "");
        _master.Replace("??SlideShowJS??", "");
        _master.Replace("??Summary??", "&nbsp;");
        _master.Replace("??BodyJS??", "");
        _master.Replace("??BannerTop??", "");
        _master.Replace("??BannerRight??", "");
        _master.Replace("??og_data??", "");
        _master.Replace("<P>", "<p>");
        _master.Replace("</P>", "</p>");
        _master.Replace("<p><p>", "<p>");
        _master.Replace("<P><P>", "<p>");
        _master.Replace("<br />", "<br>");
        _master.Replace("<br/>", "<br>");
        _master.Replace("<BR>", "<br>");
        _master.Replace("??Robots??", "index,follow");
        _master.Replace("??rel??", _relative);
        _master.Replace("??Print??", "");
        _master.Replace("??sub??", "");
        _master.Replace("??CompanyShort??", _companyShort);
        _master.Replace("??MicrodataJson??", "");
        _master.Replace("??VCard??", "");
        _master.Replace("??WebsiteUrl??", "");
        _master.SaveHtml();
    }

    /// <summary>
    /// Template for Analytics related Javascript script
    /// </summary>
    public string AnalyticsJavascript { get; set; } = string.Empty;
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text;
using System.Web;
using BodoCmsConsole.Business.Enums;
using BodoCmsConsole.Business.Helpers;
using BodoCmsConsole.Business.Interfaces;
using BodoCmsConsole.Business.Model;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.Drawing;

namespace BodoCmsConsole.Business.Builders;

/// <summary>
/// Current implementation of website generation
/// </summary>
[SupportedOSPlatform("windows")]
public class WebsiteBuilder : IWebsiteBuilder
{
    private const string ScriptPath = @"src\";
    private readonly IAppLoggerProxy _logger;
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Counter for file system folders, used to sort folder summaries correctly
    /// </summary>
    private int _folderCounter;
    private readonly string? _errorMailReceiver;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="databaseHandler">Current database handler</param>
    /// <param name="appGlobals">Current app globals</param>
    public WebsiteBuilder(IDatabaseHandler databaseHandler, IAppGlobals appGlobals)
    {
        ArgumentNullException.ThrowIfNull(appGlobals.Logger);
        _logger = appGlobals.Logger;
        _appGlobals = appGlobals;
        _errorMailReceiver = appGlobals.AppStartParameter.ErrorMailAddress;
        DatabaseHandler = databaseHandler;

        //try
        //{
        //    // ReSharper disable AssignNullToNotNullAttribute
        //    //FileLog.CreateInstance(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "BodoCMS.log"), false);
        //    // ReSharper restore AssignNullToNotNullAttribute
        //    //Logger.InfoActive = false;
        //}
        //catch (Exception ex)
        //{
        //    Logger.LogError("Ctor", ex);

        //}
    }

    /// <summary>
    /// Current database handler
    /// </summary>
    public IDatabaseHandler DatabaseHandler { get; }

    /// <summary>
    /// Current state message handler
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; set; } = StatusMessageDelegateInternal;

    private static void StatusMessageDelegateInternal(string message)
    {
        Console.WriteLine(message);
    }

    /// <summary>
    /// Current web page settings
    /// </summary>
    public WebPageSettings WebPageSettings { get; set; } = new();

    /// <summary>
    /// Init the creation process
    /// </summary>
    public void Init()
    {
        DatabaseHandler.DoInit();

        SetMessage("Load website settings...");

        WebPageSettings.PathHomepage = DatabaseHandler.GetSetting("Web_TargetPath"); //+ "\\Build_" + DateTime.Now.ToString("yyyyMMdd_HHmm");
        WebPageSettings.HttpUrl = DatabaseHandler.GetSetting("WEB_URL");


        WebPageSettings.Company = DatabaseHandler.GetSetting("GEN_Company");
        WebPageSettings.CompanyShort = DatabaseHandler.GetSetting("GEN_CompanyShort");
        if (string.IsNullOrEmpty(WebPageSettings.CompanyShort))
        {
            WebPageSettings.CompanyShort = WebPageSettings.Company;
        }

        OpenGraphHelper.SourceFile = DatabaseHandler.GetSetting("WEB_OpenGraphImage");
        //OpenGraphHelper.OpenGraphImageWriter.SourceFile = OpenGraphHelper.OpenGraphImage;

        WebPageSettings.Author = DatabaseHandler.GetSetting("WEB_Author");
        WebPageSettings.Keywords = DatabaseHandler.GetSetting("WEB_Keywords");
        WebPageSettings.Sitemap = DatabaseHandler.GetSetting("WEB_SitemapName");


        WebPageSettings.UrlRelZusatz = DatabaseHandler.GetSetting("WEB_UrlRelZusatz");

        WebPageSettings.IsSummaryRequired = DatabaseHandler.GetSetting("WEB_Zusammenfassung") == "true";

        WebPageSettings.Adsets.AddRange(DatabaseHandler.GetAdsets());
        WebPageSettings.AdSetsIds.AddRange(DatabaseHandler.GetAdsetIds());
        WebPageSettings.CounterAdsets = DatabaseHandler.GetNumberOfAdsets();

        //DatabaseHandler.PrepareContent();

        WebPageSettings.FeedbackLink = DatabaseHandler.GetSetting("WEB_FeedbackLink");
        WebPageSettings.DefaultDescription = DatabaseHandler.GetSetting("WEB_DefaultDescription");

        WebPageSettings.SitemapFileName = DatabaseHandler.GetSetting("Web_Sitemap");

        WebPageSettings.HomepageFileName = DatabaseHandler.GetSetting("Web_StartPage");
        WebPageSettings.WebpageFileExtension = DatabaseHandler.GetSetting("WEB_Extension");

        IconHelper.SetPath($@"{WebPageSettings.PathHomepage}images\");
    }

    /// <summary>
    /// Load theme from theme folder
    /// </summary>
    public void LoadTheme()
    {
        //try
        //{
        var themeDirectory = DatabaseHandler.GetSetting("Web_MasterPath");
        SetMessage("Load theme...");

        WebPageSettings.PathMaster = themeDirectory;

        // Include path
        FileHelper.CheckIfDirExists(WebPageSettings.IncludePath);

        // Images-, CSS- und JS-Ordner kopieren in Webseite
        FileHelper.CopyDir($@"{WebPageSettings.PathMaster}images\", $@"{WebPageSettings.PathHomepage}images\");
        FileHelper.CopyDir(WebPageSettings.PathMaster + ScriptPath, WebPageSettings.PathHomepage + ScriptPath);
        FileHelper.CopyDir($@"{WebPageSettings.PathMaster}css\", $@"{WebPageSettings.PathHomepage}css\");

        var d = new FileInfo($"{WebPageSettings.PathMaster}basis.htm").LastWriteTime;
        DatabaseHandler.UpdateLastChanges(d);

        d = new FileInfo($"{WebPageSettings.PathMaster}basish.htm").LastWriteTime;
        DatabaseHandler.UpdateLastChanges(d);


        // Vorlagenmaster laden


        // Analytics
        var id = DatabaseHandler.GetSetting("GoogleAnalyticsId");

        if (!string.IsNullOrEmpty(id))
        {
            WebPageSettings.AnalyticsJavascript = HtmlHelper.ReadFile($@"{WebPageSettings.PathMaster}prototypes\Analytics.prot")
                .Replace("??analytics??", id);
        }

        // Unveränderliche Links ermitteln
        WebPageSettings.MainLinks = GetMainLinksForPage();
        WebPageSettings.BottomLinks = GetBottomLinksForPage();

        //}
        //catch (Exception ex)
        //{

        //    Logger.Error(ex);
        //}

    }


    /// <summary>
    /// Create the basic required directories for the homepage
    /// </summary>
    public void PrepareDirectories()
    {
        SetMessage("Create basic directories...");

        // Zielpfad für Website
        FileHelper.CheckIfDirExists(WebPageSettings.PathHomepage);
        FileHelper.CheckIfDirExists($@"{WebPageSettings.PathHomepage}images\");
        FileHelper.CheckIfDirExists($@"{WebPageSettings.PathHomepage}css\");
        FileHelper.CheckIfDirExists(WebPageSettings.PathHomepage + ScriptPath);
    }

    //private void SetSetting(string key, string value)
    //{
    //    const string query = "if exists (select * from settings where skey='{0}') update settings set [value]='{1}'"
    //                         + "where skey='{0}' else insert into settings(skey, [Value]) select '{0}', '{1}'";
    //    _db.ExecWithResult(string.Format(query, key, value));
    //}


    public void CopyBaseFiles()
    {
        try
        {
            SetMessage("Copy base files...");

            // Copy CSS
            File.Copy(WebPageSettings.PathMaster + WebPageSettings.OrdnerZu,
                $@"{WebPageSettings.PathHomepage}images\ordner_zu.gif", true);

            WebPageSettings.OrdnerZu = "??rel??images/ordner_zu.gif";
        }
        catch (Exception ex)
        {
            _logger.LogError("CopyBaseFiles", ex);
        }
    }


    public void ClearThumbs()
    {
        //try
        //{

        var images = DatabaseHandler.ImagesWithNoThumbs();
        foreach (var image in images)
        {
            var fri = new FileInfo(image.Path);

            var thumbFile = fri.FullName.Replace(fri.Extension, $"_t{fri.Extension}");

            FileSystemHelper.DeleteFile(thumbFile);
        }


        //}
        //catch
        //{
        //    // ignored
        //}
    }

    public int FindImages()
    {
        SetMessage("Find images in subdirectories...");

        try
        {
            var result = FindImagesDirectory(new DirectoryInfo(WebPageSettings.PathHomepage));

            var folders = DatabaseHandler.GetFoldersWithApproval();

            foreach (var folder in folders)
            {
                result += FindImagesDirectory(new DirectoryInfo(Path.Combine(WebPageSettings.PathHomepage, folder)));
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("FindImages", ex);
            return 0;
        }
    }

    /// <summary>
    /// Register images in a file system folder
    /// </summary>
    /// <param name="dir">Dirctory info</param>
    private int FindImagesDirectory(DirectoryInfo dir)
    {
        var result = 0;

        var allFiles = dir.EnumerateFiles();

        var files =
            allFiles
                .Where(
                    x => !x.Name.EndsWith($"_t{x.Extension}") &&
                         (x.Extension.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                          x.Extension.Equals(".png", StringComparison.CurrentCultureIgnoreCase) ||
                          x.Extension.Equals(".gif", StringComparison.CurrentCultureIgnoreCase))).ToList();

        SetMessage($"{dir.FullName}: {files.Count}");

        //SetMessage(files.Count().ToString(CultureInfo.InvariantCulture));
        if (files.Any())
        {
            foreach (var f in files.AsParallel())
            {
                var name = f.Name.ToUpperInvariant();

                if (name == "OPENGRAPH.PNG")
                {
                    continue;
                }

                if (name.Contains("_OG.PNG"))
                {
                    continue;
                }

                result++;
                Debug.Print($"   {f.FullName}");

                if (!DatabaseHandler.CheckImageExists(f.FullName))
                {
                    DatabaseHandler.CreateImage(f, f.FullName, ImageHelper.GetImageSize(f.FullName), f.Length > ImageHelper.MaxImageSize);
                }
            }
        }

        foreach (var d in dir.GetDirectories())
        {
            result += FindImagesDirectory(d);
        }

        return result;
    }

    /// <summary>
    /// Prüfe, ob alle Bilder auch wirklich noch existieren
    /// </summary>
    public void ClearImages()
    {
        SetMessage("Clear images...");
        _logger.LogInformation("Bilder");

        var images = DatabaseHandler.GetImagesToCheck();

        DatabaseHandler.StartCommandCollecting();

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount - 1,
        };

        Parallel.ForEach(images, options, image =>
        {
            //var targetPath = ;

            var f = new FileInfo(image.Path);

            if (f.Exists)
            {

                if (f.Name.Contains("_og.png")) return;

                //SetMessage("Bilder-Links einsetzen " + f.Name);

                var text = !image.Thumbnail
                    ? $"<img src=\"images/pic{image.Id}{f.Extension}\" width={image.Width} height={image.Height} alt=\"{image.Name}\"/>"
                    : HtmlHelper.HtmlImg(new FileInfo($@"{WebPageSettings.PathHomepage}images\pic{image.Id}{f.Extension}"), @"??rel??images/").Replace("'", "''");

                DatabaseHandler.PagesReplaceContentAll($"??PIC{image.Id}??", text);
            }
            else
            {
                DatabaseHandler.DeleteImage(image.Id);
                if (File.Exists(image.TargetPath)) File.Delete(image.TargetPath);
            }
        });

        DatabaseHandler.RunCommandsFromCollection();
    }

    /// <summary>
    /// Prepare the images
    /// </summary>
    public void PrepareImages()
    {
        DatabaseHandler.PrepareImages();

    }

    /// <summary>
    /// Copy images to target directory if needed
    /// </summary>
    public void CopyImages()
    {

        //try
        //{
        SetMessage("Copy images...");

        _logger.LogInformation("Copy images");

        var images = DatabaseHandler.GetImagesToCopy();


        DatabaseHandler.StartCommandCollecting();

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount - 1,
        };

        Parallel.ForEach(images, options, image =>
        {

            //    foreach (var image in images)
            //{
            var f = new FileInfo(image.Path);
            try
            {
                var s = $@"{WebPageSettings.PathHomepage}images\pic{image.Id + f.Extension}";
                f.CopyTo(s, true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CopyImages: {image.Path}", ex);
            }
        });

        //}
        //catch (Exception ex)
        //{

        //    Logger.Error(ex);
        //}
    }


    /// <summary>
    /// Seiten aus Datenbank erstellen
    /// </summary>
    public void BuildPages()
    {
        //try
        //{
        // Seiten aufbauen


        // Find images for includes
        var images = DatabaseHandler.GetImagesForPath($"{WebPageSettings.PathHomepage}images");
        var pages = DatabaseHandler.GetPagesToBuild();

        foreach (var page in pages)
        {
            var webPage = new WebPage(WebPageSettings, page, _appGlobals)
            {
                FileName = string.IsNullOrEmpty(page.Url) ? GetFileName(page.Id) : page.Url,
                LinksSubNavi = GetSubLinksForPage(page.Id)
            };

            webPage.Images.AddRange(images);

            // Header-Image verlinken
            if (!string.IsNullOrEmpty(page.HeaderImagePath))
            {
                webPage.HeaderFileLink = GetHeaderFileLink(page.HeaderImagePath);
            }

            webPage.Export();
        }

        //}
        //catch (Exception ex)
        //{
        //    SetMessage(ex.Message);
        //    Logger.Error(ex);
        //}

    }


    /// <summary>
    /// Get the link for a header image file
    /// </summary>
    /// <param name="headerFileName"></param>
    /// <param name="relative">relative address path</param>
    /// <returns>HTML link for header image</returns>
    public string GetHeaderFileLink(string headerFileName, string relative = "")
    {
        var imgData = DatabaseHandler.GetImageDataBySourcePath(headerFileName);

        if (imgData == null)
        {
            return string.Empty;
        }
        var i = $@"{WebPageSettings.PathHomepage}".Length;
        var path = relative + imgData.Path[i..].Replace("\\", "/");

        var headerFile = $@"<img src=""{path}"" alt=""{imgData.Name}"" >";
        return headerFile;
    }

    /// <summary>
    /// Get a filename for a page by its ID
    /// </summary>
    /// <param name="pageId">Requested page ID</param>
    /// <returns>Filename of the page</returns>
    public string GetFileName(int pageId)
    {
        var fileName = string.Empty;

        switch (pageId)
        {
            case -2:
                break;
            case -1:
                fileName = WebPageSettings.SitemapFileName;
                break;
            case 1:
                fileName = WebPageSettings.HomepageFileName; //GetSetting("Web_StartPage");
                break;
            default:
                fileName = $"p{pageId}.{WebPageSettings.WebpageFileExtension}";
                break;
        }

        return fileName;
    }


    private string GetMainLinksForPage()
    {
        try
        {
            //var link = GetLink(1, "Homepage", null, null, "Homepage");
            var link = string.Empty;

            var pages = DatabaseHandler.GetMainLinksForPage();
            foreach (var page in pages)
            {
                switch (page.PageType)
                {
                    case "F":
                        var url = $"/{page.Url.Replace(WebPageSettings.PathHomepage, string.Empty).Replace("\\", "/")}/index.htm";
                        link += GetLink(-2, page.Name, null,
                            url,
                            page.TitleUrl);
                        break;

                    case "D":
                        link += GetLink(-2, page.Name, null,
                            $"??rel??{page.Name}/index.htm",
                            page.TitleUrl);
                        break;
                    default:
                        link += GetLink(page.Id, page.Name,
                            (string.IsNullOrEmpty(page.Url) ? string.Empty : "??rel??") + page.Url, null,
                            page.TitleUrl);
                        break;
                }
            }

            link = WebPageSettings.LinksLeft.Replace("??LinksLeftElement??", link);

            return link;
        }
        catch (Exception ex)
        {
            _logger.LogError("GetMainLinksForPage", ex);
            return string.Empty;
        }
    }
    /// <summary>
    /// Links am Seitenende ermitteln
    /// </summary>
    /// <returns>Link zur Seite entsprechend dem Theme</returns>
    private string GetBottomLinksForPage()
    {
        try
        {
            var link = new StringBuilder();

            link.AppendLine("<li>Erstellt: ??LastChanged??</li>");

            var pages = DatabaseHandler.GetDownLinksForPages();

            foreach (var page in pages)
            {
                link.Append(GetBottomFooterLinks(page.Id, page.Name, page.Url, null, page.TitleUrl));
            }

            link.Append(GetBottomFooterLinks(-1, WebPageSettings.Sitemap, null, null, WebPageSettings.Sitemap));

            return WebPageSettings.LinksBottom.Replace("??LinksBottomElement??", link.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError("GetBottomLinksForPage", ex);
            return string.Empty;
        }
    }


    /// <summary>
    /// Get the sublinks for a page by its ID
    /// </summary>
    /// <param name="pageId">Requested page ID</param>
    /// <returns>Sublinks of the page</returns>
    public string GetSubLinksForPage(int pageId)
    {

        var menuUrls = new List<MenuUrl>();

        var pages = DatabaseHandler.GetSubLinksForPage(pageId);

        foreach (var page in pages)
        {

            var menuUrl = new MenuUrl
            {
                Name = page.Name,
                Title = string.IsNullOrEmpty(page.TitleUrl) ? page.Name : page.TitleUrl,
                Url = GetPageLink(page.Id, page.Url)
            };


            switch (page.Css)
            {
                case "Up":
                    menuUrl.Level = MenuLevel.Up;
                    break;
                case "Same":
                    menuUrl.Level = MenuLevel.Same;
                    break;
                default:
                    menuUrl.Level = MenuLevel.Down;
                    break;
            }

            menuUrls.Add(menuUrl);

        }

        var link = HtmlHelper.FormatLinks(menuUrls, WebPageSettings);

        return WebPageSettings.LinksTop.Replace("??LinksTopRow??", link.ToString());
    }


    private string GetBottomFooterLinks(int pageId, string name, string? url, string? fileName, string? title)
    {
        fileName ??= GetFileName(pageId);

        title ??= name;

        if (!string.IsNullOrEmpty(url))
        {
            fileName = url;
        }

        name = HttpUtility.HtmlEncode(HtmlHelper.StripHtml(name.Trim()));
        title = HttpUtility.HtmlEncode(HtmlHelper.StripHtml(title.Trim()));

        return WebPageSettings.LinksBottomElement
            .Replace("??name??", name)
            .Replace("??title??", title).Replace("??url??", $"??rel??{fileName.Replace(@"\", "/")}");

        //return @"&nbsp;&nbsp;&nbsp;" + HtmlLink(HttpUrl + fileName.Replace(@"\", "/"), name.Trim(), null, null, title, "bottom");
    }



    /// <summary>
    /// Links für linke Seite erstellen
    /// </summary>
    /// <param name="pageId"></param>
    /// <param name="name"></param>
    /// <param name="url"></param>
    /// <param name="fileName"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    private string GetLink(int pageId, string name, string? url, string? fileName, string title)
    {
        fileName ??= $"??rel??{GetFileName(pageId)}";


        if (string.IsNullOrEmpty(title))
        {
            title = name;
        }
        else
        {
            name = title;
        }


        if (!string.IsNullOrEmpty(url))
        {
            fileName = url;
        }

        name = HttpUtility.HtmlEncode(HtmlHelper.StripHtml(name.Trim()));
        title = HttpUtility.HtmlEncode(HtmlHelper.StripHtml(title.Trim()));
        return WebPageSettings.LinksLeftElement
            .Replace("??name??", name).Replace("??title??", title)
            .Replace("??url??", fileName.Replace(@"\", "/"));
    }

    private void GetOtherFiles(List<FileData> files, DirectoryInfo dir)
    {

        var allFiles = dir.EnumerateFiles();
        foreach (var fri in allFiles.OrderBy(x => x.Name))
        {
            switch (fri.Extension.ToLower())
            {
                case ".db":
                case ".jpg":
                case ".png":
                case ".gif":
                case ".url":
                case ".inc":
                    continue;
                default:

                    var file = fri.Name.ToLower();

                    if (file == $"{dir.Name}.htm") break;

                    switch (file)
                    {
                        case "printer.htm":
                        case "index.htm":
                            break;
                        case "slideshow.txt":
                        case "toc1.htm":
                        case "toc.htm":
                        case "slideshow.htm":
                            File.Delete(fri.FullName);
                            break;
                        default:
                            try
                            {
                                //DatabaseHandler.CheckFile(fri);

                                var x = new FileData
                                {
                                    Name = fri.Name,
                                    FullName = fri.FullName,
                                    DisplayName = DatabaseHandler.CheckFile(fri), // DatabaseHandler.GetDisplayName(fri.FullName)
                                    Extension = fri.Extension
                                };

                                files.Add(x);

                                IconHelper.AddExtension(fri);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"GetOtherFiles: {fri.FullName}", ex);
                            }

                            break;
                    }

                    break;
            }

        }
    }


    /// <summary>
    /// LinksTop erzeugen
    /// </summary>
    /// <param name="pageId">Seiten-ID</param>
    /// <param name="url">Link</param>
    /// <returns>Link zur Seite entsprechend dem Theme</returns>
    private string GetPageLink(int pageId, string url)
    {
        var fileName = GetFileName(pageId);

        if (!string.IsNullOrEmpty(url))
        {
            fileName = url;
        }

        return fileName.Replace(@"\", "/");
    }

    /// <summary>
    /// Create sitemap
    /// </summary>
    public void Sitemap()
    {
        try
        {
            //SitemapGZ
            var sitemapGz = HtmlHelper.ReadFile(WebPageSettings.SitemapGz);
            var sitemapGzElementStandard = HtmlHelper.ReadFile(WebPageSettings.SitemapGzElement);
            var sitemapGzElements = string.Empty;
            var anfang = string.Empty;

            SetMessage("Sitemap aufbauen");

            var html = new StringBuilder();
            var overview = new StringBuilder();

            DatabaseHandler.GetSitemapPages(WebPageSettings.SitemapItems);

            foreach (var s in from x in WebPageSettings.SitemapItems
                              orderby x.Name
                              select x)
            {
                var url = (string.IsNullOrEmpty(s.Url)) ? GetFileName(s.Id) : s.Url;

                //sitemapGZElement
                if (s.Id != 0)
                {
                    sitemapGzElements += sitemapGzElementStandard.Replace("??LINK??", url)
                        .Replace("??PRIO??", s.Prio.ToString())
                        .Replace("??DATE??", DateTime.Now.ToString("yyyy-MM-dd")).Replace("??FREQ??", s.Freq);
                }

                //Standard Sitemap
                var a = s.Name?[..1];
                if (anfang != a)
                {
                    anfang = a;

                    overview.Append($@"<a href=""#{anfang}"">{anfang}</a> ");
                    html.Append($"</p><h2>{anfang}<a name=\"{anfang}\"></h2><p>");
                }

                //if (s.Id != 0)
                //{
                //}

                html.AppendHtmlLink(url, s.Name, null, null, s.TitleUrl);

                html.Append("<br/>");
            }
            sitemapGz = sitemapGz.Replace("??ELEMENTS??", sitemapGzElements).Replace("p1.", "index.").Replace("p2.", "impressum.").Replace("p3.", "wie.");
            using (var sw = new StreamWriter($"{WebPageSettings.PathHomepage}sitemap.gz", false))
            {
                sw.WriteLine(sitemapGz);
                sw.Close();
            }

            using (var sw = new StreamWriter($"{WebPageSettings.PathHomepage}sitemap.xml", false))
            {
                sw.WriteLine(sitemapGz);
                sw.Close();
            }


            var fileName = GetFileName(-1);

            var p = new HtmlPage(_appGlobals)
            {
                Master = WebPageSettings.HtmlMaster,
                Author = WebPageSettings.Author,
                Publisher = WebPageSettings.Company,
                Company = WebPageSettings.Company,
                CompanyShort = WebPageSettings.CompanyShort,
                Description = $"Sitemap: Seitenübersicht der Webseite der {WebPageSettings.Company}, {WebPageSettings.DefaultDescription}",
                FeedbackLink = WebPageSettings.FeedbackLink,
                LinksMainNavi = WebPageSettings.MainLinks,
                LinksSubNavi = string.Empty,
                LinksBottomNavi = WebPageSettings.BottomLinks,
                Content = $"<p>{overview}</p><p>{html}</p>",
                VCard = WebPageSettings.VCard,
                AnalyticsJavascript = WebPageSettings.AnalyticsJavascript,
                PageTitle = WebPageSettings.Sitemap,
                TargetFile = WebPageSettings.PathHomepage + fileName,
                DateLastChange = DateTime.Now,
                BodyJavascript = WebPageSettings.BodyJs,
                WebsiteUrl = WebPageSettings.HttpUrl
            };

            HtmlHelper.AddAdvertising(WebPageSettings, p, 0);
            p.Save();

            var robots = HtmlHelper.ReadFile($@"{WebPageSettings.PathMaster}prototypes\Robots.prot");
            robots = robots.Replace("??url??", WebPageSettings.HttpUrl);

            var path = Path.Combine(WebPageSettings.PathHomepage, "robots.txt");

            var swr = new StreamWriter(path, false, Encoding.GetEncoding("ASCII"));
            swr.Write(robots);
            swr.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError("Sitemap", ex);
        }
    }

    /// <summary>
    /// Build pages for subdirectories
    /// </summary>
    public int OtherDirectories()
    {
        //try
        //{

        DatabaseHandler.UpdateRootFolderNames(WebPageSettings.PathHomepage);

        var result = 0;

        var folders = DatabaseHandler.GetOtherDirectories();

        foreach (var folder in folders)
        {
            FileHelper.CheckIfDirExists(WebPageSettings.PathHomepage + folder.Name);

            var dir = new DirectoryInfo(WebPageSettings.PathHomepage + folder.Name);

            DatabaseHandler.StartCommandCollecting();
            CheckSubFolders(dir);
            DatabaseHandler.RunCommandsFromCollection();

            result += CreateWebPageFolder(dir, folder.Subdomain ? WebPageSettings.HttpUrl : $"/{WebPageSettings.UrlRelZusatz}",
                folder.HeaderRemoveNumbers, folder.Sitemap, WebPageSettings.HttpUrl, 0);
        }

        CreateSummaries();

        IconHelper.SaveIcons();

        return result;
        //}
        //catch (Exception ex)
        //{
        //    Logger.Error(ex);
        //}

    }

    private void CheckSubFolders(DirectoryInfo dir)
    {

        var newDir = FileHelper.CheckDirName(ref dir);

        if (newDir == null)
        {
            return;
        }

        dir = newDir;

        DatabaseHandler.CheckFolder(dir);

        var dirs = dir.GetDirectories();

        FileHelper.RemoveDirectory("IMAGES", dirs);
        FileHelper.RemoveDirectory("CSS", dirs);
        FileHelper.RemoveDirectory("SRC", dirs);

        foreach (var subDir in dirs)
        {
            CheckSubFolders(subDir);
        }
    }

    /// <summary>
    /// Create summaries for signed folders
    /// </summary>
    private void CreateSummaries()
    {
        foreach (var sum in WebPageSettings.Summaries.Where(x => x.IncludeSummariesForSubfolders))
        {
            var sum1 = sum;

            SetMessage($"Zusammenfassung für Verzeichnis {sum1.Path} bearbeiten");

            // Main page
            var pdf = new HtmlPagePrint(WebPageSettings.HtmlPdf)
            {
                Company = WebPageSettings.Company,
                Title = sum1.Title,
                FileName = sum1.FileName,
                Relative = sum1.Relative,
                Subfolder = null,
            };
            pdf.Add(sum1.Content);

            // Subpages
            var len = sum1.Path.Length;

            foreach (var detail in WebPageSettings.Summaries.Where(x => x.Path != sum1.Path && x.Path.Contains(sum1.Path)).OrderBy(x => x.Counter))
            {
                var sub = detail.Path[len..].Replace(@"\", "/");

                if (!sub.EndsWith("/"))
                {
                    sub += "/";
                }

                if (sub.StartsWith("/"))
                {
                    sub = sub[1..];
                }

                pdf.Add(detail.Content.ToString().Replace("??sub??", sub));

            }

            pdf.Save();
        }
    }


    private int CreateWebPageFolder(DirectoryInfo dir, string relative, bool removeNumbers, bool sitemapVisible, string fullUrl, int level)
    {

        //    try
        //    {

        var result = 1;

        level++;

        //_counterDirectories++;

        // Prüfe, ob verzeichnisname gültig ist
        _folderCounter++;

        var fn = dir.FullName;

        // Get the folder data from database
        var folderData = DatabaseHandler.GetFolder(fn);

        if (folderData is null)
        {
            return result;
        }

        SetMessage($"Verzeichnis {fn} bearbeiten");

        if (!string.IsNullOrEmpty(folderData.HeaderImage))
        {
            folderData.HeaderImageLink = GetHeaderFileLink(folderData.HeaderImage, relative);
        }


        var x = new WebPageFolder(_appGlobals, WebPageSettings, dir, folderData, removeNumbers, relative, _folderCounter);
        x.Images.AddRange(DatabaseHandler.GetImagesForPath(fn));

        AddSubFolders(x.SubDirectories, dir, removeNumbers);

        GetSubLinkUrls(x, removeNumbers, level);

        GetOtherFiles(x.OtherFiles, dir);

        x.CheckFileNames();

        // Create pages for subdirectories
        foreach (var folder in x.SubDirectories)
        {
            if (string.IsNullOrEmpty( folder.FullName))
            {
                continue;
            }
            result += CreateWebPageFolder(new DirectoryInfo(folder.FullName), relative, removeNumbers, sitemapVisible,
                $"{fullUrl}{dir.Name}/", level);
        }

        x.AddDirToSitemap(sitemapVisible);

        x.Export(sitemapVisible);


        DatabaseHandler.UpdateChangedImages(x.Images);

        return result;
    }

    /// <summary>
    /// Add subfolders to the homepage
    /// </summary>
    /// <param name="subDirectories">List of subfolders</param>
    /// <param name="dir">Current directory</param>
    /// <param name="removeNumbers">Remove leading numbers</param>
    public void AddSubFolders(IList<SubfolderData> subDirectories, DirectoryInfo dir, bool removeNumbers)
    {
        var dirs = dir.GetDirectories();
        FileHelper.RemoveDirectory("IMAGES", dirs);
        FileHelper.RemoveDirectory("CSS", dirs);
        FileHelper.RemoveDirectory("SRC", dirs);

        foreach (var subfolder in dirs)
        {
            var item = new SubfolderData { FullName = subfolder.FullName, Name = subfolder.Name };

            var anzeigeName = DatabaseHandler.GetFolderLinkName(subfolder.FullName);
            anzeigeName = HtmlHelper.RemoveNumbers(anzeigeName, removeNumbers);

            item.DisplayName = anzeigeName;

            subDirectories.Add(item);
        }
    }


    private void SetMessage(string msg)
    {
        _logger.LogInformation(msg);
        StatusMessageDelegate(msg);
    }

    /// <summary>
    /// Show website after creation
    /// </summary>
    public void Show()
    {
        FileHelper.Shell(WebPageSettings.PathHomepage + GetFileName(1), string.Empty);
    }

    /// <summary>
    /// Remove old pages from homepage directory
    /// </summary>
    public void ClearOldPages()
    {
        var nId = DatabaseHandler.GetPagesMaxId();

        for (var i = 3; i <= nId; i++)
        {
            if (DatabaseHandler.IsPage(i))
            {
                continue;
            }

            var sFile = $"{WebPageSettings.PathHomepage}p{i}.htm";
            try
            {
                if (File.Exists(sFile))
                {
                    SetMessage($"Bereinige Seite P{i}.htm");
                    File.Delete(sFile);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ClearOldPages", ex);
            }
        }
    }

    ///// <summary>
    ///// Bilder bereinigen
    ///// </summary>
    //public void CheckImagesAtTargetPath()
    //{
    //    CheckImagesAtTargetPath(new DirectoryInfo(WebPageSettings.PathHomepage));

    //}

    /// <summary>
    /// Remove images not required anymore
    /// </summary>
    /// <param name="dir">Directory to check</param>
    public void CheckImagesAtTargetPath(DirectoryInfo dir)
    {
        if (DatabaseHandler.IsCheckDirectoryNotRequired(dir.FullName))
        {
            return;
        }

        //if (DatabaseHandler.ExecWithResult("exec dbo.spDirectories_CheckDirectories '" + dir.FullName + "'") == "1") return;

        foreach (var f in dir.GetFiles())
        {
            switch (f.Extension.ToLowerInvariant())
            {
                case ".gif":
                case ".jpg":
                    if (WebPageSettings.HtmlMaster.Contains(f.Name)) break;
                    if (WebPageSettings.HtmlMasterH.Contains(f.Name)) break;
                    if (WebPageSettings.LinksLeft.Contains(f.Name)) break;
                    if (WebPageSettings.LinksLeftElement.Contains(f.Name)) break;
                    if (WebPageSettings.LinksTop.Contains(f.Name)) break;
                    if (WebPageSettings.LinksTopRow.Contains(f.Name)) break;
                    if (WebPageSettings.LinksTopRowElement.Contains(f.Name)) break;

                    if (DatabaseHandler.IsImageCheckNotRequired(f.Name)) break;

                    //if (DatabaseHandler.ExecWithResult("exec dbo.spImages_CheckNeeded '" + f.Name + "'") == "1") break;
                    if (f.Name.Contains("_og.png")) break;
                    try
                    {
                        f.Delete();
                    }
                    catch (Exception ex)
                    {

                        _logger.LogError($"CheckImagesAtTargetPath: {f.FullName}", ex);
                    }
                    break;
                    //default:
                    //    break;
            }
        }

        foreach (var d in dir.GetDirectories())
        {
            CheckImagesAtTargetPath(d);
        }
    }

    /// <summary>
    ///  Check the header files and add it to the images
    /// </summary>
    public void CheckNewHeaderFiles()
    {
        var files = DatabaseHandler.GetNewHeaderFiles();
        foreach (var fileItem in files)
        {
            var file = fileItem.Path;

            if (!File.Exists(file))
            {
                continue;
            }

            if (DatabaseHandler.CheckImageExists(file))
            {
                continue;
            }

            var fi = new FileInfo(file);
            var target = Path.Combine($@"{WebPageSettings.PathHomepage}images", fi.Name);

            if (!File.Exists(target))
            {
                File.Copy(fi.FullName, target);
            }

            Debug.Print(fi.FullName);
            Debug.Print(target);

            DatabaseHandler.CreateImage(new FileInfo(file), target, ImageHelper.GetImageSize(file), false, false);
        }
    }

    #region Prepare pages

    /// <summary>
    /// Prepare the pages for usage
    /// </summary>
    public void PreparePages()
    {
        SetMessage("Prepare the links...");
        ReplacePageLinks();
        PrepareIncludes();
    }

    private void ReplacePageLinks()
    {
        try
        {
            var pages = DatabaseHandler.GetPagesList();
            foreach (var page in pages)
            {
                SetMessage($"Page {page.Id}");

                var link = string.IsNullOrEmpty(page.Url) ?
                    HtmlHelper.HtmlLink1($"p{page.Id}.htm", page.Link, null, null, null).Replace("'", "''") :
                    HtmlHelper.HtmlLink1(page.Url, page.Link, null, null, null).Replace("'", "''");

                DatabaseHandler.PagesReplaceContentAll($"??PAGE{page.Id}??", link);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("ReplacePageLinks", ex);
        }
    }

    private void PrepareIncludes()
    {
        var files = new DirectoryInfo(WebPageSettings.IncludePath).GetFiles("*.inc");

        foreach (var file in files)
        {
            var tag = $"??include:{file.Name}??";

            var content = File.ReadAllText(file.FullName);

            DatabaseHandler.PagesReplaceContentAll(tag, content);
        }
    }

    #endregion



    public void GetSubLinkUrls(WebPageFolder dir, bool removeNumbers, int level)
    {
        var home = dir.Directory.FullName[WebPageSettings.PathHomepage.Length..];
        if (home.Substring(home.Length - 1, 1) == "\\")
        {
            home = home[0..^1];
            //home = home.Substring(0, home.Length - 1);
        }

        var path = home.Split('\\');

        // Top-Links erzeugen
        var i = 1;
        var p = WebPageSettings.PathHomepage;

        foreach (var s in path)
        {
            var helper = string.Empty;

            if (string.IsNullOrEmpty(s))
            {
                continue;
            }

            if (s == path[^1])
            {
                continue;
            }

            helper = helper.PadRight(path.Length - i).Replace(" ", "../");

            p = Path.Combine(p, s);

            var anzeigeName = DatabaseHandler.GetFolderLinkName(p);

            var url = new MenuUrl
            {
                Name = HtmlHelper.RemoveNumbers(anzeigeName[..1].ToUpper() + anzeigeName[1..],
                    removeNumbers),
                Url = $"{helper}index.htm",
                Level = MenuLevel.Up
            };

            dir.MenuUrls.Add(url);
        }

        // Links für selbes Level erzeugen
        if (level > 1)
        {
            var parent = dir.Directory.Parent;
            var data = new List<SubfolderData>();

            if (parent is not null)
            {
                AddSubFolders(data, parent, removeNumbers);
            }

            HtmlHelper.GetLinks(MenuLevel.Same, dir.MenuUrls, data, "../");
        }

        // Down links erzeugen
        HtmlHelper.GetLinks(MenuLevel.Down, dir.MenuUrls, dir.SubDirectories);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        DatabaseHandler.Exec("EXEC [dbo].[spFilesUpdateEnd]");
        _logger.LogInformation("Done!");
    }


    /// <summary>
    /// Send an error mail
    /// </summary>
    public void SendErrorMail()
    {
        if (string.IsNullOrEmpty(_errorMailReceiver))
        {
            return;
        }

        if (WebPageSettings.ParserErrors.Count == 0)
        {
            return;
        }

        var msg = new StringBuilder();

        foreach (var err in WebPageSettings.ParserErrors)
        {
            msg.AppendLine(err);
        }

        var final = msg.ToString();
        _logger.LogError("ErrorMail", final);
        SetMessage(final);

        DatabaseHandler.SendMail(_errorMailReceiver, "BodoCms errors", final);
    }
}
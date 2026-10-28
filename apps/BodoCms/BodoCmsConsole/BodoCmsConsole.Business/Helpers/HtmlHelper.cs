// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BodoCmsConsole.Business.App;
using BodoCmsConsole.Business.Enums;
using BodoCmsConsole.Business.Model;
using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoCmsConsole.Business.Helpers;

/// <summary>
/// Helepr methods for HTML creation
/// </summary>
public static class HtmlHelper
{
    private static readonly IAppLoggerProxy? Logger = Globals.Instance.Logger;

    private static readonly Encoding DefaulEncoding = Encoding.GetEncoding("windows-1252");


    public const string StartpageName = @"index.htm";


    /// <summary>
    /// Create an HTML link
    /// </summary>
    /// <param name="s">Current string builder</param>
    /// <param name="url">Url for the link</param>
    /// <param name="name">Name to show for the link</param>
    /// <param name="cssStyle">style name of the link</param>
    /// <param name="imageUrl">Link to the image for the link</param>
    /// <param name="title">Title for the link</param>
    public static void AppendHtmlLink(this StringBuilder s, string url, string? name, string? cssStyle, string? imageUrl, string? title)
    {
        name ??= url;

        name = name.Replace("_", " ").Trim();

        title ??= name;

        cssStyle = cssStyle == null ? "" : $@" style=""{cssStyle}""";

        if (imageUrl != null)
        {
            imageUrl = $@"<img src=""{imageUrl}"" class=""icon"" >";
        }

        s.Append(
            $@"<a href=""{url}"" {cssStyle} onMouseOver=""window.status='{name}';return true"" onMouseOut=""window.status=''; return true"" title=""{title.Trim()}"">{imageUrl}{name}</a>");
    }


    /// <summary>
    /// Create a HTML link
    /// </summary>
    /// <param name="url">Url for the link</param>
    /// <param name="name">Name to show for the link</param>
    /// <param name="cssStyle">style name of the link</param>
    /// <param name="imageUrl">Link to the image for the link</param>
    /// <param name="title">Title for the link</param>
    /// <returns></returns>
    public static string HtmlLink(string url, string? name, string? cssStyle, string? imageUrl, string? title)
    {
        name ??= url;
        name = name.Replace("_", " ").Trim();

        title ??= name;

        cssStyle = cssStyle == null ? "" : $@" style=""{cssStyle}""";

        if (imageUrl != null)
        {
            imageUrl = $@"<img src=""{imageUrl}"" class=""icon"" >";
        }

        return
            $@"<a href=""{url}"" {cssStyle} onMouseOver=""window.status='{name}';return true"" onMouseOut=""window.status=''; return true"" title=""{title.Trim()}"">{imageUrl}{name}</a>";
    }


    public static string HtmlLink1(string url, string? name, string? cssStyle, string? window, string? cssClass)
    {
        name ??= url;
        name = name.Replace("_", " ").Trim();

        window ??= "_self";

        cssStyle = cssStyle == null ? "" : $@" style=""{cssStyle}"" ";

        cssClass = cssClass == null ? "" : $@" class=""{cssClass}"" ";

        return
            $@"<a href=""{url}"" {cssStyle}{cssClass} target=""{window}"" onMouseOver=""window.status='{name}';return true"" onMouseOut=""window.status=''; return true"">{name}</a>";
    }

    /// <summary>
    /// Parse a string for certain tags: @Year and @LYear
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string Parser(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var output = input.Replace("@Year", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture))
            .Replace("@LYear", (DateTime.Now.Year - 1).ToString(CultureInfo.InvariantCulture));
        return output;
    }



    /// <summary>
    /// Removes the HTML Code.
    /// </summary>
    /// <param name="inputString">The text.</param>
    /// <returns>The string without HTML Code</returns>
    public static string StripHtml(string inputString)
    {

        var s = Regex.Replace(inputString, "<.*?>", string.Empty);

        if (!s.Contains('<'))
        {
            return s;
        }

        var i = s.IndexOf("<", StringComparison.InvariantCultureIgnoreCase);
        s = s[..(i - 1)];
        return s;
    }


    public static string CheckIncludes(string dir, string s)
    {
        try
        {
            // Includes einsetzen
            const string search = "??include:";
            var len = search.Length;
            if (!s.Contains(search)) return s;

            var i = s.IndexOf(search, 0, StringComparison.InvariantCultureIgnoreCase);

            while (i > -1)
            {
                var j = s.IndexOf("??", i + 3, StringComparison.InvariantCultureIgnoreCase);
                var include = s.Substring(i + len, j - i - len);

                if (!include.EndsWith(".inc")) include += ".inc";

                include = Path.Combine(dir, include);

                var incContent = ReadFile(include);

                s = s[..i] + incContent + s.Substring(j + 2, s.Length - j - 2);

                if (i + 3 > s.Length) break;

                i = s.IndexOf(search, i + 3, StringComparison.InvariantCultureIgnoreCase);
            }
        }
        catch //(Exception e)
        {
            // ignored
        }


        return s;
    }


    /// <summary>
    /// ASCII-Datei komplett einlesen
    /// </summary>
    /// <param name="fileName"></param>
    public static string ReadFile(string fileName)
    {

        try
        {
            if (!new FileInfo(fileName).Exists)
            {
                return string.Empty;
            }

            //var fsIn = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);



            //var encoding = GetEncoding(fsIn);

            //fsIn.Position = 0;

            //var sr = new StreamReader(fsIn, encoding);

            //var s = sr.ReadToEnd();

            //sr.Close();
            //sr.Dispose();
            //fsIn.Close();
            //fsIn.Dispose();

            using var reader = new StreamReader(fileName, DefaulEncoding, true);

            //reader.Peek(); // you need this!
            //var encoding = reader.CurrentEncoding;
            var s = reader.ReadToEnd();


            return s;
        }
        catch (Exception ex)
        {
            Logger?.LogError($"HtmlHelper.ReadFile: {fileName}",ex);
            return string.Empty;
        }

    }


    ///// <summary>
    ///// Determines a text file's encoding by analyzing its byte order mark (BOM).
    ///// Defaults to ASCII when detection of the text file's endianness fails.
    ///// </summary>
    ///// <param name="filename">The text file to analyze.</param>
    ///// <returns>The detected encoding.</returns>
    //public static Encoding GetEncoding(FileStream fileStream)
    //{
    //    // Read the BOM
    //    var bom = new byte[4];

    //        fileStream.Read(bom, 0, 4);


    //    // Analyze the BOM
    //    if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
    //    if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
    //    if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
    //    if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
    //    if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
    //    if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

    //    // We actually have no idea what the encoding is if we reach this point, so
    //    // you may wish to return null instead of defaulting to ASCII
    //    return Encoding.GetEncoding("windows-1252");
    //}


    public static string? HtmlImgPlain(string path, string description, string cssClass)
    {
        try
        {
            if (!string.IsNullOrEmpty(cssClass))
            {
                cssClass = $@"class=""{cssClass}""";
            }

            return $@"<img src=""{path}"" alt=""{description}"" {cssClass} >";
        }
        catch (Exception ex)
        {
            Logger?.LogError($"HtmlImgPlain: {path}", ex);
            return null;
        }
    }

    public static string RemoveNumbers(string was, bool removeNumbers)
    {
        was = was.Replace("_", " ");

        if (!removeNumbers) return was;

        was = Regex.Replace(was, @"^\s*\d+[\t ]+", "");

        return was;
    }


    /// <summary>
    /// Links im Bildarchiv erzeugen
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="name">Anzeigename der URL</param>
    /// <param name="direction">Link nach Up bzw Down</param>
    /// <param name="template"></param>
    /// <param name="title">Link title</param>
    /// <returns>Link zur Seite entsprechend dem Theme</returns>
    internal static string GetLinkBildarchiv(string url, string name, string direction, string template, string? title = null)
    {
        title = HttpUtility.HtmlEncode(HtmlHelper.StripHtml(string.IsNullOrEmpty(title) ? name : title));
        name = HttpUtility.HtmlEncode(HtmlHelper.StripHtml(name.Trim().Replace("_", " ")));
        return template.Replace("??direction??", direction).Replace("??name??", name).Replace("??title??", title).Replace("??url??", url);
    }



    public static string CheckIncludeImages(string s, string dir, IList<ImageData> images)
    {
        // Bilder einsetzen
        s = s.Replace("??image2:", "??image:").Replace("??image3:", "??image:").Replace("??image4:", "??image:");

        CheckIncludeImagesIntern(ref s, dir, "??image:", "", images);
        //CheckIncludeImagesIntern(ref s, dir, "??image2:", "");
        //CheckIncludeImagesIntern(ref s, dir, "??image3:", "");
        //CheckIncludeImagesIntern(ref s, dir, "??image4:", "");
        return s;
    }

    private static void CheckIncludeImagesIntern(ref string s, string dir, string search, string cssClass, IList<ImageData> images)
    {
        var len = search.Length;

        if (!s.Contains(search))
        {
            return;
        }

        var i = s.IndexOf(search, 0, StringComparison.InvariantCultureIgnoreCase);

        while (i > -1)
        {
            var j = s.IndexOf("??", i + 3, StringComparison.InvariantCultureIgnoreCase);

            var bild = s.Substring(i + len, j - i - len);

            //var image = DatabaseHandler.GetImageData();
            var image = images.FirstOrDefault(x => x.TargetPath == Path.Combine(dir, bild));
                


            if (image != null)
            {
                if (image.SlideShow)
                {
                    image.SlideShow = false;
                    image.IsChanged = true;
                }
                if (image.Thumbnail)
                {
                    image.Thumbnail = false;
                    image.IsChanged = true;
                }
                    

                var imgLink = HtmlImgPlain($"??sub??{bild}", Parser(image.Name), cssClass);

                s = $"{s[..i]}{imgLink}{s[(j + 2)..]}";

                i += imgLink?.Length ?? 0;
            }

            i = s.IndexOf(search, i + 3, StringComparison.InvariantCultureIgnoreCase);
        }
    }


    public static string HtmlImgBildarchiv(FileSystemInfo f, string relativePath)
    {
        //try
        //{

        //  width=""75"" height=""75""

        var url =
            $@"<div class=""bildarchivlink""><a href=""{relativePath + f.Name.Replace(@"\", "/")}""><img src=""{ImageHelper.GenerateThumb(f, relativePath)}"" alt=""{f.Name.Replace(@"\", "/")}""></a></div>";

        return url;
        //}
        //catch (Exception ex)
        //{
        //    Logger.Error(ex);
        //    return null;
        //}
    }


    public static StringBuilder FormatLinks(IList<MenuUrl> menuUrls, WebPageSettings settings)
    {
        var link = new StringBuilder();

        if (menuUrls.Count > 5)
        {
            link.Append(GetLinks(menuUrls, MenuLevel.Up, 1, "Übergeordnete Seiten", settings));
            link.Append(GetLinks(menuUrls, MenuLevel.Same, 2, "Weitere Seiten", settings));
            link.Append(GetLinks(menuUrls, MenuLevel.Down, 3, "Untergeordnete Seiten", settings));
        }
        else
        {
            foreach (var url in menuUrls)
            {

                string image;

                switch (url.Level)
                {
                    case MenuLevel.Up:
                        image = "Up";
                        break;
                    case MenuLevel.Same:
                        image = "A";
                        break;
                    case MenuLevel.Down:
                        image = "Down";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                var link1 = HtmlHelper.GetLinkBildarchiv(url.Url,
                    url.Name,
                    image, settings.LinksTopRowElement);

                link.AppendLine(settings.LinksTopRow.Replace("??LinksTopRowElement??", link1));
            }
        }

        return link;
    }

    public static void GetLinks(MenuLevel level, IList<MenuUrl> menuUrls, IList<SubfolderData> subDirectories, string relative = "", bool removeNumbers = true)
    {
        foreach (var folder in subDirectories)
        {
            var dn = folder.DisplayName ?? folder.Name ?? string.Empty;

            var url = new MenuUrl
            {
                Name = RemoveNumbers(dn[..1].ToUpper() +dn[1..],
                    removeNumbers),
                Url = $"{relative}{folder.Name}/index.htm",
                Level = level
            };

            menuUrls.Add(url);
        }
    }


    public static StringBuilder GetLinks(IEnumerable<MenuUrl> menuUrls, MenuLevel menuLevel, int count, string title, WebPageSettings settings)
    {

        string image;

        switch (menuLevel)
        {
            case MenuLevel.Up:
                image = "Up";
                break;
            case MenuLevel.Same:
                image = "A";
                break;
            case MenuLevel.Down:
                image = "Down";
                break;
            default:
                throw new ArgumentOutOfRangeException($"MenuLevel not found: {menuLevel}");
        }

        var link = new StringBuilder();

        var data = menuUrls.Where(x => x.Level == menuLevel).ToList();

        if (data.Count == 0)
        {
            return link;
        }

        // Maximum two links

        if (data.Count < 3)
        {
            string link1 = "";
            foreach (var url in data)
            {
                link1 += HtmlHelper.GetLinkBildarchiv(url.Url,
                    url.Name,
                    image, settings.LinksTopRowElement);


            }
            link.AppendLine(settings.LinksTopRow.Replace("??LinksTopRowElement??", link1));

            return link;
        }

        var link2 = string.Empty;
        foreach (var url in data)
        {
            link2 += HtmlHelper.GetLinkBildarchiv(url.Url,
                url.Name,
                image, settings.LinksTopRowElement2);
        }

        link.AppendLine(settings.LinksTopRow2
                .Replace("??LinksTopRowElement??", link2)
                .Replace("??Count??", count.ToString()))
            .Replace("??Title??", title);

        return link;
    }


    /// <summary>
    /// Add a random advertising to the page
    /// </summary>
    /// <param name="webPageSettings">Current web page settings</param>
    /// <param name="p"></param>
    /// <param name="adSetId">ID of the pages advertising set</param>
    public static void AddAdvertising(WebPageSettings webPageSettings, HtmlPage p, int adSetId)
    {
        if (webPageSettings.CounterAdsets <= 0)
        {
            return;
        }

        if (adSetId == 0)
        {
            // Zufällige Zahl erzeugen und für diese dann die zugehörige Id des AdSets holen
            adSetId = webPageSettings.GetNextRandomAdsetId();
            adSetId = webPageSettings.AdSetsIds[adSetId];
        }

        var adSet = webPageSettings.Adsets.FirstOrDefault(x => x.Id == adSetId);

        if (adSet == null)
        {
            return;
        }


        var source = $"??rel??{adSet.W2Image}";
        p.BannerRight(webPageSettings.BannerRight.Replace("??url??", adSet.W2Url).Replace("??image??", source));

        source = $"??rel??{adSet.W1Image}";

        p.BannerTop(webPageSettings.BannerTop.Replace("??url??", adSet.W1Url).Replace("??image??", source));
    }


    public static string HtmlImg(FileSystemInfo f, string relativePath)
    {
        try
        {
            var url =
                $@"<a href=""{relativePath + f.Name.Replace(@"\", "/")}"" data-lightbox=""roadtrip""><img src=""{ImageHelper.GenerateThumb(f, relativePath)}"" width=""{ImageHelper.SmallPicWidth.ToString(CultureInfo.InvariantCulture)}"" height=""{ImageHelper.SmallPicHeight.ToString(CultureInfo.InvariantCulture)}"" alt=""{f.Name.Replace(@"\", "/")}"" border=""0"" ></a>";
            return url;
        }
        catch (Exception ex)
        {
            Logger?.LogError($"HtmlImg: {f.FullName}", ex);
            return string.Empty;
        }
    }


    /// <summary>
    /// Fill an HTML template with an OpenGraph image
    /// </summary>
    /// <param name="template"></param>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="openGraphImageUrl"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string CreateOpenGraphData(string template, string title, string description, string openGraphImageUrl, string url)
    {
        // Open Graph data
        var s = template;
        var i = title.Length;
        if (i > 95) i = 95;

        s = s.Replace("??Title??", title[..i]);

        i = description.Length;
        if (i > 297)
        {
            i = 297;
        }

        s = s.Replace("??Description??", description[..i])
            .Replace("??Image??", openGraphImageUrl)
            .Replace("??Url??", url);
        return s;
    }
}
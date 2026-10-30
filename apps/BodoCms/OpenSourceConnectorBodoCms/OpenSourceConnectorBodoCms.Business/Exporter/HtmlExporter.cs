// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Text;
using Bodoconsult.App.Helpers;
using Bodoconsult.Web.Html.Helpers;
using Markdig;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Business.Exporter;

/// <summary>
/// Create an HTML page from the packet data
/// </summary>
public class HtmlExporter
{
    private readonly FolderData _folder;
    private readonly string _target;
    private readonly StringBuilder _html = new();
    private readonly string _packetName;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="packetName">Packet name</param>
    /// <param name="folder">Current folder</param>
    /// <param name="target"></param>
    public HtmlExporter(string packetName, FolderData folder, string target)
    {
        _folder = folder;
        _target = target;
        _packetName = packetName;
    }

    /// <summary>
    /// Add the link to the GitHub repo
    /// </summary>
    public void AddGitHubLink()
    {
        _html.AppendLine(HtmlHelper.H2("Github repository"));

        var link = HtmlHelper.A(_folder.GitUrl, _folder.GitUrl);

        _html.AppendLine(HtmlHelper.Paragraph(link));
        _html.AppendLine(string.Empty);
    }

    /// <summary>
    /// Add the link to the Nuget package
    /// </summary>
    public void AddNugetLink()
    {
        var nugetUrl = $"https://www.nuget.org/packages/{_packetName}";

        _html.AppendLine(HtmlHelper.H2("Nuget package"));

        var link = HtmlHelper.A(nugetUrl, nugetUrl);

        _html.AppendLine(HtmlHelper.Paragraph(link));
        _html.AppendLine(string.Empty);
    }

    /// <summary>
    /// Add the repo links for README.html and LICENSE.html
    /// </summary>
    public void AddContentLinks()
    {
        _html.AppendLine(HtmlHelper.H2("Documentation"));

        var url = $"{_folder.GitUrl}/tree/main/doc/{_packetName}/README.md";

        var link = HtmlHelper.A(url, url);

        _html.AppendLine(HtmlHelper.Paragraph(link));
        _html.AppendLine(string.Empty);

        _html.AppendLine(HtmlHelper.H2("License"));

        url = $"{_folder.GitUrl}/tree/main/doc/{_packetName}/LICENSE.md";

        link = HtmlHelper.A(url, url);

        _html.AppendLine(HtmlHelper.Paragraph(link));
        _html.AppendLine(string.Empty);
    }

    private static string GetHtmlFromMarkdown(string markDown)
    {
        var result = Markdown.ToHtml(markDown);
        return result.Replace("<pre>","<div class=\"code\"><pre>").Replace("</pre>","</pre></div>");
    }

    /// <summary>
    /// Save the HTML as file
    /// </summary>
    public void Save()
    {
        var t = _html.ToString();

        FileSystemHelper.DeleteFile(_target);
        File.WriteAllText(_target, t, Encoding.UTF8);
    }

}
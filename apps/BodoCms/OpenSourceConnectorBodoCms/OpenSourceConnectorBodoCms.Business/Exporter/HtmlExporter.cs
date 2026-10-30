// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Text;
using Bodoconsult.Web.Html.Helpers;
using Markdig;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Business.Exporter;

public class HtmlExporter
{

    private readonly FolderData _folder;

    private readonly string _target;


    private readonly StringBuilder s = new StringBuilder();

    public HtmlExporter(FolderData folder, string target)
    {
        _folder = folder;
        _target = target;
    }


    public void AddGitHubLink()
    {

        this.s.AppendLine(HtmlHelper.H2("Github repository"));

        var link = HtmlHelper.A(_folder.GitUrl, _folder.GitUrl);

        this.s.AppendLine(HtmlHelper.Paragraph(link));
        s.AppendLine("");


    }

    public void AddNugetLink()
    {

        //if (string.IsNullOrEmpty(_folder.NugetUrl)) return;

        //this.s.AppendLine(HtmlHelper.H2("Nuget package"));

        //var link = HtmlHelper.A(_folder.NugetUrl, _folder.NugetUrl);

        //this.s.AppendLine(HtmlHelper.Paragraph(link));
        //s.AppendLine("");
    }


    public void AddContent()
    {

        if (!string.IsNullOrEmpty(_folder.ReadMe))
        {
            s.AppendLine(HtmlHelper.H2("About the library "+_folder.Name));
            s.Append(GetHtmlFromMarkdown(_folder.ReadMe.Replace("# ", "### ")));
            s.AppendLine("");
        }


        if (!string.IsNullOrEmpty(_folder.Licence))
        {
            s.AppendLine(HtmlHelper.H2("Licence for the library " + _folder.Name));
            s.Append(GetHtmlFromMarkdown(_folder.Licence.Replace("# ", "### ")));
            s.AppendLine("");
        }

    }



    private static string GetHtmlFromMarkdown(string markDown)
    {

        //var s = markDown.Split('\r', '\n');

        //var erg = new StringBuilder();

        //foreach (var t in s)
        //{
        //    var line = t;
        //    if (line.StartsWith("    ") || line.StartsWith('\t'))
        //    {
        //        if (!line.EndsWith("  "))
        //        {
        //            line+=  "  ";
        //        }
        //    }

        //    erg.AppendLine(line);
        //}



        //var result = Markdown.ToHtml(erg.ToString());

        var result = Markdown.ToHtml(markDown);



        return result.Replace("<pre>","<div class=\"code\"><pre>").Replace("</pre>","</pre></div>");
    }



    public void Save()
    {
        var t = s.ToString();

        if (File.Exists(_target)) File.Delete(_target);
        File.WriteAllText(_target, t, Encoding.UTF8);
    }

}
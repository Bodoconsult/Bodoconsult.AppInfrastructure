// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Text;

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Print version of a page
/// </summary>
internal class HtmlPagePrint
{
    private readonly StringBuilder _content = new();
    private readonly StringBuilder _master;
    private string? _title;

    public HtmlPagePrint(string master)
    {
        _master = new StringBuilder(master);
        Author = string.Empty;
        Company = string.Empty;
        CompanyShort = string.Empty;
    }

    /// <summary>
    /// Relative path
    /// </summary>
    public string? Relative { get; set; }

    /// <summary>
    /// Subfolder
    /// </summary>
    public string? Subfolder { get; set; }

    /// <summary>
    /// The company's name
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// The author of the page
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Short name of the Company
    /// </summary>
    public string CompanyShort { get; set; }

    /// <summary>
    /// Page title
    /// </summary>
    public string Title
    {
        set => _title = value.Replace("_", " ");
    }

    /// <summary>
    ///  Add content
    /// </summary>
    /// <param name="content">Content</param>
    public void Add(string content)
    {
        _content.Append(content);
    }

    /// <summary>
    ///  Add content
    /// </summary>
    /// <param name="content">Content</param>
    public void Add(StringBuilder content)
    {
        _content.Append(content);
    }

    /// <summary>
    /// Get the HTML string for the page
    /// </summary>
    public string Html
    {
        get {

            // First place content
            _master.Replace("??Content??", _content.ToString());

            // Then check all other tags
            _master.Replace("??PageTitle??", _title);
            _master.Replace("??Author??", Author);
            _master.Replace("??Company??", Company);
            _master.Replace("??CompanyShort??", CompanyShort);
            _master.Replace("??rel??", Relative);
            _master.Replace("??sub??", Subfolder);

            return _master.ToString();
        }
    }

    /// <summary>
    /// File name to save the HTML in
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Save the HTML as file with name <see cref="FileName"/>
    /// </summary>
    public void Save()
    {
        ArgumentNullException.ThrowIfNull(FileName);
        using var sw = new StreamWriter(FileName, false, Encoding.GetEncoding("utf-8"));
        sw.Write(Html);
        sw.Close();
    }
}
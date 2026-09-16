// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Text;
using Bodoconsult.Web.Mail.Models;
using BodoWebMailer.Business.Interfaces;

namespace BodoWebMailer.Business.Services;

/// <summary>
/// Fake implementation for <see cref="IMailStorageService"/>
/// </summary>
public sealed class FakeMailStorageService : IMailStorageService
{
    private readonly List<MailItem> _mailItems  = new();

    /// <summary>
    /// Get the mail account data from database
    /// </summary>
    /// <returns>JSON string with mail account data</returns>
    public string GetMailAccontData()
    {
        // ToDo: make it fakeable
        return string.Empty;
    }

    /// <summary>
    /// Get all mails to send
    /// </summary>
    /// <returns></returns>
    public IList<MailItem> GetMails()
    {
           
        return _mailItems;
    }

    /// <summary>
    /// Archive a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    public void ArchiveMail(MailItem mailItem)
    {
            // Do nothing
    }

    /// <summary>
    /// Set an error for a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    public void SetError(MailItem mailItem)
    {
            // Do nothing
    }

    /// <summary>
    /// Add a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    public void AddMailItem(MailItem mailItem)
    {
        _mailItems.Add(mailItem);
    }

    /// <summary>
    /// Run a SQL query to an HTML string
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="sql">SQL statement to export</param>
    /// <returns>Resulting HTML string</returns>
    public string RunSqlQueryAsHtml(string title, string sql)
    {
        var erg = new StringBuilder();

        erg.Append(  $"<h1>{title}<h1>");
        erg.Append("<table class=\"bordered\"><tr><th>Column1</th><th>Column2</th></tr><tr><td>Value1</td><td>Value2</td></tr></table>");

        return erg.ToString();
    }

    /// <summary>
    /// Run SQL query 
    /// </summary>
    /// <param name="fileName">Filename</param>
    /// <param name="sql">SQL statement to export</param>
    /// <returns>Resulting CSV string</returns>
    public string RunSqlQueryToCsvFile(string fileName, string sql)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"{fileName}.csv");

        if (File.Exists(tempPath))
        {
            File.Delete(tempPath);
        }

        const string content = "\"Test1\";\"Test2\";\r\n1;2;\r\n3;4;\r\n";

        // us-ascii
        var sw = new StreamWriter(tempPath, false, Encoding.GetEncoding("utf-8"));
        sw.Write(content);
        sw.Close();

        return tempPath;
    }

    /// <summary>
    /// Clear all mail items
    /// </summary>
    public void ClearMailItems()
    {
        _mailItems.Clear();
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using Bodoconsult.Web.Html.Html;
using Bodoconsult.Web.Html.HtmlTables;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Models;
using BodoWebMailer.Business.Interfaces;

namespace BodoWebMailer.Business.Services;

/// <summary>
/// Service for mail handling in a SqlServer database
/// </summary>
public sealed class SqlServerMailStorageService : IMailStorageService
{
    private readonly IConnManager _db;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="globals">Current app globals</param>
    public SqlServerMailStorageService(IAppGlobals globals)
    {
        _db = SqlClientConnManager.GetConnManager(globals.AppStartParameter.DefaultConnectionString);

#if DEBUG
        _db.Exec("INSERT INTO [dbo].[tMail] ([M_From],[M_To],[M_Subject],[M_Body], [M_SignatureTemplate], [M_LogoPath]) VALUES ('noreply@bodoconsult.de','test@bodoconsult.de','Test','Testbody', 'Bodoconsult', 'C:\\Bodoconsult\\Logos\\BodoConsult.gif')");
#endif
    }

    /// <summary>
    /// Get the mail account data from database
    /// </summary>
    /// <returns>JSON string with mail account data</returns>
    public string GetMailAccontData()
    {
        return _db.ExecWithResult("SELECT [Value] from dbo.Settings where [skey]='MailAccount';");
    }

    /// <summary>
    /// Get all mails to send
    /// </summary>
    /// <returns></returns>
    public IList<MailItem> GetMails()
    {
        var erg = new List<MailItem>();

        // Mails einlesen
        var dt = _db.GetDataTable("EXEC dbo.spMail_FetchAll");
        foreach(DataRow row in dt.Rows)
        {
            var mi = MapToMailItem(row);

            erg.Add(mi);
        }
        dt.Dispose();

        return erg;
    }

    /// <summary>
    /// Map a <see cref="DataRow"/> to a <see cref="MailItem"/>
    /// </summary>
    /// <param name="row">Data row conatining mail info</param>
    /// <returns>Mail item</returns>
    public static MailItem MapToMailItem(DataRow row)
    {
        var mi = new MailItem
        {
            MailGuid = row["M_ID"].ToString(),
            From = row["M_From"].ToString(),
            To = row["M_To"].ToString(),
            Subject = row["M_Subject"].ToString(),
            Body = row["M_Body"].ToString(),
            Logo = row["M_LogoPath"].ToString(),
            Attachments = row["M_Attachments"].ToString(),
            SignatureTemplate = row["M_SignatureTemplate"].ToString(),
            Archive = Convert.ToBoolean(row["M_Archive"]),
            Queries = row["M_Queries"].ToString(),
            Zip= Convert.ToBoolean(row["M_Zip"].ToString()),
            ZipPassword = PasswordHandler.Decrypt(row["M_ZipPassword"].ToString()),
        };
        return mi;
    }

    /// <summary>
    /// Archive a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    public void ArchiveMail(MailItem mailItem)
    {
        var sql = mailItem.Archive
            ? $"EXEC dbo.spMail_MoveToArchive '{mailItem.MailGuid}'"
            : $"EXEC dbo.spMail_Delete '{mailItem.MailGuid}'";
        _db.Exec(sql);
    }

    /// <summary>
    /// Set an error for a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    public void SetError(MailItem mailItem)
    {
        var sql = $"EXEC dbo.spMail_SetError '{mailItem.MailGuid}'";
        _db.Exec(sql);
    }

    /// <summary>
    /// Add a mail item
    /// </summary>
    /// <param name="mailItem">Current mail item</param>
    public void AddMailItem(MailItem mailItem)
    {
        var sql = "INSERT INTO [dbo].[tMail] " +
                  "([M_ID] " +
                  ",[M_From] " +
                  ",[M_To] " +
                  ",[M_Subject] " +
                  ",[M_Body] " +
                  ",[M_Error] " +
                  ",[M_LogoPath] " +
                  ",[M_SignatureTemplate] " +
                  ",[M_Attachments] " +
                  ",[M_Archive] " +
                  ",[M_Queries]) " +
                  "VALUES " +
                  "({0} " +
                  ",{1} " +
                  ",{2} " +
                  ",{3} " +
                  ",{4} " +
                  ", 0 " +
                  ",{5} " +
                  ",{6} " +
                  ",{7} " +
                  ", 0 " +
                  ",{8}) ";


        sql = string.Format(sql,
            GetValue(mailItem.MailGuid),
            GetValue(mailItem.From),
            GetValue(mailItem.To),
            GetValue(mailItem.Subject),
            GetValue(mailItem.Body),
            GetValue(mailItem.Logo),
            GetValue(mailItem.SignatureTemplate),
            GetValue(mailItem.Attachments),
            GetValue(mailItem.Queries));

        _db.Exec(sql);
    }

    /// <summary>
    /// Run a SQL query to an HTML string
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="sql">SQL statement to export</param>
    /// <returns>Resulting HTML string</returns>
    public string RunSqlQueryAsHtml(string title, string sql)
    {
        var erg = $"<h1>{title}<h1>";

        var style = new Stylesheet();
        style.CssClassTable("bordered");

        erg += TableFormatter.FormatAsHtml(_db.GetDataTable(sql), true, style);

        return erg;
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

        var content = TableFormatter.FormatAsCsv(_db.GetDataTable(sql), true);

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
#if DEBUG
        _db.Exec("delete from dbo.tMail");
#endif
    }

    private static string GetValue(string value)
    {
        return string.IsNullOrEmpty(value) ? "null" : $"'{value}'";
    }
}
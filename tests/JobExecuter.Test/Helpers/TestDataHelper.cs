// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Bodoconsult.Database.SqlClient;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Model;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.JobMailer;

namespace JobExecuter.Test.Helpers;

internal static class TestDataHelper
{
    static TestDataHelper()
    {
        AppPath = FileHelper.GetAppPath();
        TestDataPath = Path.Combine(AppPath, "TestData");

        var dirInfo = new DirectoryInfo(AppPath)?.Parent?.Parent?.Parent?.Parent?.Parent;

        ArgumentNullException.ThrowIfNull(dirInfo);

        SolutionPath = dirInfo.FullName;

        LocalPath = Path.Combine(Path.GetTempPath(), "JobExecuter");

        if (!Directory.Exists(LocalPath))
        {
            Directory.CreateDirectory(LocalPath);
        }
    }

    /// <summary>
    /// App path
    /// </summary>
    public static string AppPath { get; }

    /// <summary>
    /// Path for test data
    /// </summary>
    public static string TestDataPath { get; }

    /// <summary>
    /// Path of the solution
    /// </summary>
    public static string SolutionPath { get; }

    /// <summary>
    /// Path to store local data
    /// </summary>
    public static string LocalPath { get; }

    /// <summary>
    /// Path to BodoCMS include folder
    /// </summary>
    public const string IncludePath = @"S:\Websites\Bodoconsult_Master\includes\JobExecuter1.inc";


    private const string Conn = "Data Source=BCGS03DS;Initial Catalog=BodoCMS_BCG;Integrated Security=SSPI;TrustServerCertificate=True";

    private const int PageId = 153;
    public static string TempPath { get; set; } = @"C:\ProgramData\JobExecuter";

    /// <summary>
    /// Get an SMTPMailer account
    /// </summary>
    /// <returns></returns>
    public static SmtpJobMailer GetSmtpMailer()
    {
        throw new NotImplementedException();
        //const string fileName = @"C:\Daten\Projekte\_work\Data\JobExecuter.json";

        //var account = JsonHelper.LoadJsonFile<SmtpMailAccount>(fileName);

        //var s = new SmtpJobMailer(account);
        //s.LoadDefaultHtmlTemplate();
        //return s;
    }

    /// <summary>
    /// Get an O365Mailer account
    /// </summary>
    /// <returns></returns>
    public static O365JobMailer GetO365Mailer()
    {
        throw new NotImplementedException();
        //const string fileName = @"C:\Daten\Projekte\_work\Data\JobExecuter.json";

        //var account = JsonHelper.LoadJsonFile<SmtpMailAccount>(fileName);

        //var s = new SmtpJobMailer(account);
        //s.LoadDefaultHtmlTemplate();
        //return s;
    }

    public static void SaveAsHtmlPage(StructuredText sr)
    {

        var f = new HtmlTextFormatter
        {
            Title = "JobExecuter technical documentation",
            StructuredText = sr,
            Template = "{0}",
            DateString = $"Date created: {DateTime.Now:d}",
            MoveHeaderLevel = 1
        };

        var result = f.GetFormattedText();



        var db = SqlClientConnManager.GetConnManager(Conn);
        db.SetCommandTimeout(60);

        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "dbo.spPages_UpdateFromExternalSource"
        };
        lo.Parameters.Add("@PageId", SqlDbType.Int);
        lo.Parameters[0].Value = PageId;
        lo.Parameters.Add("@Content", SqlDbType.NVarChar);
        lo.Parameters[1].Value = result;

        Debug.Print(result.Length.ToString());

        db.Exec(lo);
    }

    /// <summary>
    /// Create a directory if it exists
    /// </summary>
    /// <param name="targetPath">Path to the directory to create</param>
    public static void CreateDirectory(string targetPath)
    {
        if (string.IsNullOrEmpty(targetPath))
        {
            return;
        }

        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Data;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using BodoWebReporting.Dal.Helpers;
using BodoWebReporting.Dal.Model;
using Microsoft.Data.SqlClient;

namespace BodoWebReporting.Dal.Handler;

/// <summary>
/// Data layer
/// </summary>
public class DataHandler
{

    public StatusHandler StatusChange;

    private readonly IConnManager _db;

    private readonly string _errorMailer;

    public string ConnectionString { get; }

    public DataHandler(string connectionString, string errorMailer)
    {
        ConnectionString = connectionString;
        _errorMailer = errorMailer;

        SetStatus("Opening database connection...");
        _db = SqlClientConnManager.GetConnManager(ConnectionString);
    }

    /// <summary>
    /// Get a data reader
    /// </summary>
    /// <param name="sql">SQL to run</param>
    /// <returns>IDataReader as result from SQL</returns>
    public IDataReader GetDataReader(string sql)
    {
        return _db.GetDataReader(sql);
    }


    /// <summary>
    /// Statusanzeige
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    private void SetStatus(string msg)
    {
        var x = StatusChange;
        x?.Invoke(msg);
    }

    /// <summary>
    /// Get a data table
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public DataTable GetDataTable(string sql)
    {
        return _db.GetDataTable(sql);
    }

    public void Exec(string sql)
    {
        _db.Exec(sql, false);
    }


    public IList<WebTable> GetWebTables(string targetPath)
    {

        var result = new List<WebTable>();


        var df = _db.GetDataTable("EXEC dbo.spWeb_Tables_FetchAll");
        foreach (DataRow row in df.Rows)
        {

            var wt = new WebTable
            {
                Name = row["WT_Name"].ToString(),
                Sql = TextHelper.Parser(row["WT_Sql"].ToString()),
                Id = Convert.ToInt32(row["WT_ID"].ToString()),
                TableType = row["WT_Type"].ToString(),
            };

            wt.FileName = Path.Combine(targetPath, $"web_tables_{wt.Id}.{wt.TableType}");

            result.Add(wt);
        }

        return result;
    }

    internal IList<CopyTask> GetWebTableTargets(string fileName, int id)
    {
        var result = new List<CopyTask>();

        var df = _db.GetDataTable($"EXEC dbo.spWeb_Tables_Targets_Get {id}");
        foreach (DataRow row in df.Rows)
        {
            var ct = new CopyTask
            {
                SourceFileName = fileName,
                TargetFileName = TextHelper.Parser(row["WTT_TargetPath"].ToString())
            };
            result.Add(ct);
        }

        return result;
    }

    public IList<ReportingFile> GetReportingFiles(string targetPath)
    {
        var result = new List<ReportingFile>();

        var df = _db.GetDataTable("EXEC dbo.spWEB_Reporting_Files");
        foreach (DataRow row in df.Rows)
        {

            var fileName = row["WR_FileName"].ToString();

            var rf = new ReportingFile
            {
                FileName = Path.Combine(targetPath, fileName),
                Author = row["WR_Author"].ToString(),
                Shortname = row["WR_ShortName"].ToString(),
                LogoPath = row["WR_LogoPath"].ToString()
            };


            var dt = _db.GetDataTable($"EXEC dbo.spWEB_Reporting '{fileName}'");
            for (var index = 0; index < dt.Rows.Count; index++)
            {
                var r = dt.Rows[index];
                var rfp = new ReportingFileParts
                {
                    Landscape = Convert.ToBoolean(r["WR_Landscape"].ToString()),
                    BackgroundImagePath = r["WR_BackgroundImagePath"].ToString(),
                    LogoPath = r["WR_LogoPath"].ToString(),
                    Title = r["WR_Title"].ToString(),
                    Title1 = r["WR_Title1"].ToString(),
                    Subject = r["WR_Subject"].ToString(),
                    Sql = r["WR_SQL"].ToString(),
                    AddInfoSql = r["WR_AddInfos"].ToString(),
                    ReportType = r["WR_Type"].ToString().ToLower().Trim(),
                    Name = r["WR_Name"].ToString(),
                    ShowInToc = Convert.ToBoolean(r["WR_ShowInToc"].ToString()),
                    ChartColumns = r["WR_C_Columns"].ToString(),
                    ChartColumnsDataTable = r["WR_C_ColumnsDataTable"].ToString(),
                    ChartYUnit = r["WR_C_Y_Unit"].ToString(),
                    ChartType = string.IsNullOrEmpty(r["WR_C_ChartType"].ToString())
                        ? ChartType.BarChart
                        : (ChartType)Enum.Parse(typeof(ChartType),
                            r["WR_C_ChartType"].ToString().Trim().ToLower() + "chart", true),
                    ChartLegendLength = string.IsNullOrEmpty(r["WR_C_LegendLength"].ToString())
                        ? (short)0
                        : Convert.ToInt16(r["WR_C_LegendLength"].ToString())
                };


                rf.ReportingFileParts.Add(rfp);
            }

            result.Add(rf);
        }

        return result;
    }


    /// <summary>
    /// Send an error mail
    /// </summary>
    /// <param name="subject">subject for the mail</param>
    /// <param name="message">Mail message</param>
    public void ErrorHandling(string subject, string message)
    {

        try
        {

            SetStatus(subject + ":" + message);

            var lo = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.[spMAIL_New]" };
            lo.Parameters.Add("@To", SqlDbType.VarChar);
            lo.Parameters[0].Value = _errorMailer;
            lo.Parameters.Add("@Subject", SqlDbType.VarChar);
            lo.Parameters[1].Value = subject;
            lo.Parameters.Add("@Msg", SqlDbType.VarChar);
            lo.Parameters[2].Value = $"{message}";
            _db.Exec(lo);

        }
        catch (Exception ex)
        {
            SetStatus($"WebChartHandler:ErrorHandling:{ex.Message}:{subject}:{message}");
        }
    }

    public IList<WebChart> GetWebCharts(string targetPath)
    {
        var result = new List<WebChart>();

        var df = _db.GetDataTable("EXEC dbo.spWeb_Charts_FetchAll");
        foreach (DataRow row in df.Rows)
        {
            var wc = new WebChart
            {
                FileName = Path.Combine(targetPath, $"Chart{row["WC_ID"]}.png"),
                Title = row["WC_Title"].ToString(),
                Sql = row["WC_Sql"].ToString(),
                YUnit = row["WC_YUnit"].ToString(),
                Columns = row["WC_Columns"].ToString(),
                ColumnsDataTable = row["WC_ColumnsDataTable"].ToString(),
                ChartType = (ChartType)Enum.Parse(typeof(ChartType), row["WC_ChartType"] + "chart", true),
                Copyright = row["WC_Copyright"].ToString(),
                Id = Convert.ToInt32(row["WC_Id"].ToString()),
                Width = Convert.ToInt32(row["WC_Width"].ToString()),
                Height = Convert.ToInt32(row["WC_Height"].ToString()),
                ExportDataTable = Convert.ToBoolean(row["WC_ExportDataTable"].ToString())
            };

            result.Add(wc);
        }

        return result;
    }

    public IList<CopyTask> GetReportingTargets(string targetPath)
    {
        var result = new List<CopyTask>();

        var df = _db.GetDataTable("EXEC dbo.spWEB_ReportingTarget");
        foreach (DataRow row in df.Rows)
        {
            var ct = new CopyTask
            {
                SourceFileName = Path.Combine(targetPath, row["RT_FileName"].ToString()),
                TargetFileName = row["RT_Target"].ToString()
            };

            result.Add(ct);
        }

        return result;
    }

    public IList<CopyTask> GetWebChartTargets(string sourcePath, string includePath, int webChartId)
    {
        var result = new List<CopyTask>();

        var df = _db.GetDataTable($"EXEC dbo.spWeb_Charts_Targets_Get {webChartId}");
        foreach (DataRow row in df.Rows)
        {
            var ct = new CopyTask
            {
                SourceFileName = sourcePath,
                TargetFileName = row["WCT_TargetPath"].ToString()
            };

            if (!string.IsNullOrEmpty(includePath))
            {
                ct.IncludeSourceFileName = includePath;
                ct.IncludeTargetFileName = Path.Combine(Path.GetDirectoryName(ct.TargetFileName),
                    Path.GetFileNameWithoutExtension(ct.TargetFileName) + ".inc");
            }

            result.Add(ct);
        }

        return result;
    }
}
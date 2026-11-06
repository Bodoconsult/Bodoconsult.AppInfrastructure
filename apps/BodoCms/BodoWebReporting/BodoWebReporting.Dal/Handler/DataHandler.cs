// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Data;
using System.Diagnostics.Eventing.Reader;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using BodoWebReporting.Dal.Helpers;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;
using Microsoft.Data.SqlClient;

namespace BodoWebReporting.Dal.Handler;

/// <summary>
/// Data layer
/// </summary>
public class DataHandler : IDataHandler
{
    private readonly IConnManager _db;
    private readonly string _errorMailer;


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="db">Current database connection</param>
    public DataHandler(IAppGlobals appGlobals, IConnManager db)
    {
        _db = db;
        _errorMailer = appGlobals.AppStartParameter.ErrorMailAddress ?? string.Empty;

        if (appGlobals is not IBodoWebReportingAppGlobals globals)
        {
            throw new ArgumentException("appGlobals is not IBodoWebReportingAppGlobals");
        }

        AppGlobals = globals;

        SetStatus("Opening database connection...");
    }

    public IBodoWebReportingAppGlobals AppGlobals { get; }

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
    /// Send a state message to the UI
    /// </summary>
    /// <param name="msg">Message</param>
    private void SetStatus(string msg)
    {
        AppGlobals.StatusMessageDelegate?.Invoke(msg);
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
        _db.Exec(sql);
    }

    /// <summary>
    /// Get <see cref="WebTable"/> instances for a certain target path
    /// </summary>
    /// <param name="targetPath">Target path</param>
    /// <returns>List with <see cref="WebTable"/> instances</returns>
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

    public IList<CopyTask> GetWebTableTargets(string fileName, int id)
    {
        var result = new List<CopyTask>();

        var df = _db.GetDataTable($"EXEC dbo.spWeb_Tables_Targets_Get {id}");
        foreach (DataRow row in df.Rows)
        {
            var target = TextHelper.Parser(row["WTT_TargetPath"].ToString());

            if (string.IsNullOrEmpty(target))
            {
                AppGlobals.Logger?.LogWarning($"Copying {fileName}: target is null");
                continue;
            }

            var ct = new CopyTask
            {
                SourceFileName = fileName,
                TargetFileName = target
            };
            result.Add(ct);
        }

        return result;
    }

    /// <summary>
    /// Get reporting files
    /// </summary>
    /// <param name="targetPath">Target path</param>
    /// <returns>List with <see cref="ReportingFile"/> instances</returns>
    public IList<ReportingFile> GetReportingFiles(string targetPath)
    {
        var result = new List<ReportingFile>();

        var df = _db.GetDataTable("EXEC dbo.spWEB_Reporting_Files");
        foreach (DataRow row in df.Rows)
        {

            var fileName = row["WR_FileName"].ToString();

            if (string.IsNullOrEmpty(fileName))
            {
                continue;
            }

            var rf = new ReportingFile
            {
                FileName = Path.Combine(targetPath, fileName),
                Author = row["WR_Author"].ToString() ?? "Author",
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
                    ReportType = r["WR_Type"].ToString()?.ToLowerInvariant().Trim(),
                    Name = r["WR_Name"].ToString(),
                    ShowInToc = Convert.ToBoolean(r["WR_ShowInToc"].ToString()),
                    ChartColumns = r["WR_C_Columns"].ToString(),
                    ChartColumnsDataTable = r["WR_C_ColumnsDataTable"].ToString(),
                    ChartYUnit = r["WR_C_Y_Unit"].ToString(),
                    ChartType = string.IsNullOrEmpty(r["WR_C_ChartType"].ToString())
                        ? ChartType.BarChart
                        : (ChartType)Enum.Parse(typeof(ChartType),
                            $"{r["WR_C_ChartType"].ToString()?.Trim().ToLowerInvariant()}chart", true),
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
            SetStatus($"{subject}: {message}");

#if DEBUG

#else
            var lo = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "dbo.[spMAIL_New]" };
            lo.Parameters.Add("@To", SqlDbType.VarChar);
            lo.Parameters[0].Value = _errorMailer;
            lo.Parameters.Add("@Subject", SqlDbType.VarChar);
            lo.Parameters[1].Value = subject;
            lo.Parameters.Add("@Msg", SqlDbType.VarChar);
            lo.Parameters[2].Value = $"{message}";
            _db.Exec(lo);
#endif
        }
        catch (Exception ex)
        {
            SetStatus($"WebChartHandler:ErrorHandling:{ex.Message}:{subject}:{message}");
        }
    }

    /// <summary>
    /// Get webcharts for a target path
    /// </summary>
    /// <param name="targetPath">Target path</param>
    /// <returns>List with  <see cref="WebChart"/> instances</returns>
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
                ChartType = (ChartType)Enum.Parse(typeof(ChartType), $"{row["WC_ChartType"]}chart", true),
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
            var fileName = row["RT_FileName"].ToString();

            if (string.IsNullOrEmpty(fileName))
            {
                AppGlobals.Logger?.LogWarning("FileName is null or empty");
                continue;
            }

            var target = row["RT_Target"].ToString();
            if (string.IsNullOrEmpty(target))
            {
                AppGlobals.Logger?.LogWarning("Target is null or empty");
                continue;
            }
            
            var ct = new CopyTask
            {
                SourceFileName = Path.Combine(targetPath, fileName),
                TargetFileName = target
            };

            result.Add(ct);
        }

        return result;
    }

    /// <summary>
    /// Get the copy targets for webcharts
    /// </summary>
    /// <param name="sourcePath">Source path</param>
    /// <param name="includePath">Include path</param>
    /// <param name="webChartId">WebChart database ID</param>
    /// <returns>List with copy tasks</returns>
    public IList<CopyTask> GetWebChartTargets(string sourcePath, string includePath, int webChartId)
    {
        var result = new List<CopyTask>();

        var df = _db.GetDataTable($"EXEC dbo.spWeb_Charts_Targets_Get {webChartId}");
        foreach (DataRow row in df.Rows)
        {
            var target = row["WCT_TargetPath"].ToString();
            if (string.IsNullOrEmpty(target))
            {
                AppGlobals.Logger?.LogWarning("Target is null or empty");
                continue;
            }

            var ct = new CopyTask
            {
                SourceFileName = sourcePath,
                TargetFileName = target
            };

            if (!string.IsNullOrEmpty(includePath))
            {
                ct.IncludeSourceFileName = includePath;
                ct.IncludeTargetFileName = Path.Combine(Path.GetDirectoryName(ct.TargetFileName) ?? string.Empty,
                    $"{Path.GetFileNameWithoutExtension(ct.TargetFileName)}.inc");
            }

            result.Add(ct);
        }

        return result;
    }
}
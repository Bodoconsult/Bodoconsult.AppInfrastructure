// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Data;
using System.Diagnostics;
using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Helpers;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.ReportHandling;

[SupportedOSPlatform("windows")]
public class WebReportHandler: BaseObjectHandler, IWebReportHandler
{
    private readonly List<Task> _exportTasks = new();
    private readonly IWebReportExporterFactory _webReportExporterFactory;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="webReportExporterFactory">Current factory for WebReportExporter</param>
    /// <param name="appGlobals">Current app globals</param>
    public WebReportHandler(IDataHandler dataHandler, IWebReportExporterFactory webReportExporterFactory, IAppGlobals appGlobals) : base(dataHandler, appGlobals)
    {
        _webReportExporterFactory = webReportExporterFactory;
    }

    /// <summary>
    /// The kind of reporting to be created
    /// </summary>
    public ReportingTargets? Target { get; set; }

    /// <summary>
    /// Start the export tasks for all files
    /// </summary>
    /// <returns>Number of reports created</returns>
    public int StartExport()
    {

        //try
        //{
        var rfiles = DataHandler.GetReportingFiles(AppGlobals.TargetPath);

        var i = 0;

        foreach (var rfile in rfiles)
        {
            StartExportReportFile(rfile, i);
            i++;
        }

        return i;
        //}
        //catch (Exception ex)
        //{
        //    _dataHandler.ErrorHandling("BodoWebReporting:ReportingHandler:StartExport", $"{ex.Message}");
        //    return 0;
        //}
    }

    /// <summary>
    /// Start a single report export to a file task
    /// </summary>
    public void StartExportReportFile(ReportingFile file, int id)
    {
        //try
        //{

        SetStatus($"Export report '{file.FileName}'");

        var wr = _webReportExporterFactory.CreateInstance(file, id);
        wr.Landscape = file.Landscape;
        wr.AddPageSetup();
        wr.StyleSet.ColumnWidth = wr.StyleSet.TypeAreaWidth;

        wr.NumberOfTableRowsPerPage = file.Landscape ? 35 : 48;

        wr.Title = TextHelper.Parser(file.Shortname) ?? "Title";
        wr.StyleSet.DocumentMetaData.Title = wr.Title;
        wr.StyleSet.DocumentMetaData.Authors = file.Author;

        //var x = StyleHandler.StyleSet;

        wr.StartDocument();

        var left = 0;
        var gewicht = 0;
        var start = true;
        var backImage = "";
        //var logoImage = "";
        var sql = "";
        var itemCount = file.ReportingFileParts.Count;

        var data = new DataTable();
        foreach (var rfp in file.ReportingFileParts)
        {
            if (start)
            {
                //wr.Landscape = rfp.Landscape;

                //Steps = rfp.Landscape ? 35 : 42;

                //wr.Steps = Steps;

                if (!string.IsNullOrEmpty(rfp.BackgroundImagePath) &&  backImage != rfp.BackgroundImagePath && File.Exists(rfp.BackgroundImagePath))
                {
                    wr.BackgroundImagePath = rfp.BackgroundImagePath;
                }
                else
                {
                    wr.BackgroundImagePath = null;
                    if (!string.IsNullOrEmpty(rfp.BackgroundImagePath))
                    {
                        Debug.Print($"Not found: {rfp.BackgroundImagePath}");
                    }
                }    

                //if (logoImage != rfp.LogoPath) wr.LogoImagePath = rfp.LogoPath;

                

                // Does a TOC make sense?
                if (itemCount < 2)
                {
                    //wr.AddHeading(r["WR_Title1"].ToString());
                }
                else
                {
                    wr.AddTocSection(rfp.Title1);
                }

                wr.AddContentSection();
            }

            wr.Subject = rfp.Subject ?? string.Empty;

            if (sql != rfp.Sql && (!rfp.Sql?.Equals("XXX", StringComparison.OrdinalIgnoreCase) ?? false ))
            {
                sql = TextHelper.Parser(rfp.Sql);

                if (!string.IsNullOrEmpty(sql))
                {
                    data = DataHandler.GetDataTable(sql);
                }
            }

            if (!string.IsNullOrEmpty(rfp.AddInfoSql))
            {
                wr.AdditionalInfo = FormatAddInfos(DataHandler.GetDataTable(rfp.AddInfoSql));
            }

            var typ = rfp.ReportType?.ToLowerInvariant();

            if (string.IsNullOrEmpty(typ))
            {
                continue;
            }

            if (typ == "smalltable" && data.Rows.Count > wr.NumberOfTableRowsPerPage / 2)
            {
                typ = "table";
            }

            switch (typ)
            {
                case "table":
                    gewicht += 2;
                    break;
                case "chart":
                    gewicht += 1;
                    break;
                case "smalltable":
                    gewicht += 1;
                    break;
                //case "heading":
                //default:
                //    break;
            }

            // Neue Seite?
            if (gewicht > 4)
            {
                gewicht = 0;
                //SetStatus("Neue Seite");

                switch (typ)
                {
                    case "heading":

                        break;
                    case "table":
                        gewicht += 2;
                        break;
                    case "chart":
                        gewicht += 1;
                        break;
                    case "smalltable":
                        gewicht += 1;
                        break;
                }

                wr.EndCharting();
                if (backImage != rfp.BackgroundImagePath)
                {
                    wr.BackgroundImagePath = rfp.BackgroundImagePath;
                }
                //if (logoImage != rfp.LogoPath) wr.LogoImagePath = rfp.LogoPath;
                //wr.NewContentSection();
            }


            switch (typ)
            {
                case "heading":
                    AddHeading(rfp, wr, backImage);
                    break;
                case "table":
                    AddTable(rfp, gewicht, wr, data);
                    break;
                case "smalltable":
                    AddSmallTable(rfp, gewicht, wr, data);
                    break;
                case "chart":
                    AddChart(rfp, gewicht, data, left, wr);
                    break;
            }

            backImage = rfp.BackgroundImagePath;
            //logoImage = rfp.LogoPath;
            start = false;
        }

        wr.EndCharting();

        SetStatus($"Create PDF file {file.FileName}");


        var task = new TaskFactory().StartNew(() =>
            {
                wr.ExportReport();
                wr.Dispose();
            }
        );

        _exportTasks.Add(task);

        //}
        //catch (Exception ex)
        //{
        //    _dataHandler.ErrorHandling("BodoWebReporting:ReportingHandler:StartExportReportFile", string.Format("{1}:{0}", ex.Message, file.FileName));
        //}
    }

    private void AddChart(ReportingFileParts rfp, int gewicht, DataTable data, int left, WebReportExporter wr)
    {
        SetStatus($"Exporting chart {rfp.Name}...{gewicht}");

        // Entferne ggfs. "Gesamt" für Charts
        var s = $"[{data.Columns[0].ColumnName}]='Gesamt'";

        if (data.Columns[0].DataType == typeof(string))
        {
            if (data.Select(s).Any())
            {
                data.Rows.RemoveAt(data.Rows.Count - 1);
            }
        }

        var chartdata = new ChartData
        {
            Data = data.Copy(),
            Title = rfp.Title ?? "Title missing (rfp.Title)",
            Columns = rfp.ChartColumns ?? string.Empty,
            ColumnsDataTable = rfp.ChartColumnsDataTable ?? string.Empty,
            YLabel = rfp.ChartYUnit ?? string.Empty,
            LegendLength = rfp.ChartLegendLength,
            TypeOfChart = rfp.ChartType,
            Left = left,
        };

        wr.AddChart(chartdata);
    }

    private void AddSmallTable(ReportingFileParts rfp, int gewicht, WebReportExporter wr, DataTable data)
    {
        SetStatus($"Exporting small table {rfp.Name}...{gewicht}");
        wr.AddSmallTable(rfp.Title ?? "Title missing (rfp.Title)", data);
    }

    private void AddTable(ReportingFileParts rfp, int gewicht, WebReportExporter wr, DataTable data)
    {
        SetStatus($"Exporting table {rfp.Name}...{gewicht}");
        wr.EndCharting();
        wr.AddTable(rfp.Title ?? "Title missing (rfp.Title)", data, rfp.ShowInToc, gewicht > 2);
    }

    private void AddHeading(ReportingFileParts rfp, WebReportExporter wr, string? backImage)
    {
        SetStatus($"Exporting heading {rfp.Name}...");

        wr.EndCharting();
        if (backImage != rfp.BackgroundImagePath)
        {
            wr.BackgroundImagePath = rfp.BackgroundImagePath;
        }

        if (wr.NumberOfContentElements > 1)
        {
            wr.AddPageBreak();
        }

        //if (logoImage != rfp.LogoPath) wr.LogoImagePath = rfp.LogoPath;
        wr.AddHeading(rfp.Name ?? "Heading missing (rfp.Name)");
    }

    /// <summary>
    /// Wait for all export tasks
    /// </summary>
    public void WaitForExportsDone()
    {
        Task.WaitAll(_exportTasks.ToArray());

        SetStatus("Exporting reporting is done...");
    }

    private static string FormatAddInfos(DataTable dataTable)
    {
        return dataTable.Rows.Cast<DataRow>().Aggregate("", (current, r) => $"{current}{r[0]}:  {r[1]}\t\t");
    }


    /// <summary>
    /// Copy the reports to their final targets
    /// </summary>
    public void CopyToTarget()
    {
        var ct = DataHandler.GetReportingTargets(AppGlobals.TargetPath);

        var errors = CopyTaskHelper.RunAllTasks(ct);

        if (!errors.Any())
        {
            return;
        }

        var msg = errors.Aggregate("", (current, error) => current + ("<br/>+" + error));

        DataHandler.ErrorHandling($"Error:Reportinghandler:MoveToTarget", $"<p>{msg}</p>");

    }
}
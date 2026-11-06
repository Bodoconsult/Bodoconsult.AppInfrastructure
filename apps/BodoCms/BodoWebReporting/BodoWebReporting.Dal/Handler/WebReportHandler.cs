// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Helpers;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;
using System.Data;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace BodoWebReporting.Dal.Handler;

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
        Schritt = 21;
    }

    public int Schritt;

    public ReportingTargets? Target { get; set; }

    public FileOptions? FileOption { get; set; }

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

        Task.WaitAll(_exportTasks.ToArray());

        SetStatus("Exporting reporting is done...");

        return i;
        //}
        //catch (Exception ex)
        //{
        //    _dataHandler.ErrorHandling("BodoWebReporting:ReportingHandler:StartExport", $"{ex.Message}");
        //    return 0;
        //}
    }


    /// <summary>
    /// Export aller Berichte einer Reportdatei
    /// </summary>
    public void StartExportReportFile(ReportingFile file, int id)
    {

        //try
        //{

        SetStatus($"Export report '{file.FileName}'");

        StyleHandler.RunCode();

        var wr = _webReportExporterFactory.CreateInstance(file, id);
        wr.Schritt = Schritt;
        wr.BackColor = StyleHandler.BackColor;

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

                wr.Landscape = rfp.Landscape;

                Schritt = rfp.Landscape ? 35 : 42;

                wr.Schritt = Schritt;

                if (!string.IsNullOrEmpty(rfp.BackgroundImagePath) &&  backImage != rfp.BackgroundImagePath && File.Exists(rfp.BackgroundImagePath))
                {
                    wr.BackgroundImagePath = rfp.BackgroundImagePath;
                }
                else
                {
                    wr.BackgroundImagePath = null;
                    if (!string.IsNullOrEmpty(rfp.BackgroundImagePath)) Debug.Print($"Not found: {rfp.BackgroundImagePath}");
                }    

                //if (logoImage != rfp.LogoPath) wr.LogoImagePath = rfp.LogoPath;

                wr.AddPageSetup();

                // Inhaltsverzeichnis sinnvoll?
                if (itemCount < 2)
                {
                    wr.NewContentSection();
                    //wr.AddHeading(r["WR_Title1"].ToString());
                }
                else
                {
                    wr.TocSection(rfp.Title1);
                    wr.NewContentSection();
                }
            }

            wr.Subject = rfp.Subject ?? string.Empty;

            if (sql != rfp.Sql && rfp.Sql?.ToUpperInvariant() != "XXX")
            {
                sql = TextHelper.Parser(rfp.Sql);

                if (!string.IsNullOrEmpty(sql))
                {
                    data = DataHandler.GetDataTable(sql);
                }
            }

            if (!string.IsNullOrEmpty(rfp.AddInfoSql))
            {
                wr.AddInfos = FormatAddInfos(DataHandler.GetDataTable(rfp.AddInfoSql));
            }

            var typ = rfp.ReportType;

            if (typ == "smalltable" && data.Rows.Count > Schritt / 2)
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
                default:
                    break;
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
                    wr.BackgroundImagePath = rfp.BackgroundImagePath;
                //if (logoImage != rfp.LogoPath) wr.LogoImagePath = rfp.LogoPath;
                wr.NewContentSection();
            }


            switch (typ)
            {
                case "heading":
                    SetStatus($"Exporting heading {rfp.Name}...");

                    gewicht = 0;
                    wr.EndCharting();
                    if (backImage != rfp.BackgroundImagePath)
                        wr.BackgroundImagePath = rfp.BackgroundImagePath;
                    //if (logoImage != rfp.LogoPath) wr.LogoImagePath = rfp.LogoPath;
                    if (!start) wr.NewContentSection();
                    wr.AddHeading(rfp.Name ?? "Heading missing (rfp.Name)");
                    break;
                case "table":
                    SetStatus($"Exporting table {rfp.Name}...{gewicht}");
                    left = 0;
                    wr.EndCharting();
                    wr.AddTable(rfp.Title ?? "Title missing (rfp.Title)", data, rfp.ShowInToc, gewicht > 2);
                    break;
                case "smalltable":
                    SetStatus($"Exporting small table {rfp.Name}...{gewicht}");
                    left = 0;
                    wr.AddSmallTable(rfp.Title ?? "Title missing (rfp.Title)", data);
                    break;
                case "chart":
                    SetStatus($"Exporting chart {rfp.Name}...{gewicht}");

                    // Entferne ggfs. "Gesamt" für Charts
                    var s = $"[{data.Columns[0].ColumnName}]='Gesamt'";

                    if (data.Columns[0].DataType == typeof(string))
                    {

                        //try
                        //{
                        if (data.Select(s).Any()) data.Rows.RemoveAt(data.Rows.Count - 1);
                        //}
                        //// ReSharper disable EmptyGeneralCatchClause
                        //catch
                        //// ReSharper restore EmptyGeneralCatchClause
                        //{

                    }


                    //}

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

                    left = left == 0 ? 24 : 0;

                    wr.AddChart(chartdata);

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

    private static string FormatAddInfos(DataTable dataTable)
    {
        return dataTable.Rows.Cast<DataRow>().Aggregate("", (current, r) => $"{current}{r[0]}:  {r[1]}\t\t");
    }



    public void MoveToTarget()
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
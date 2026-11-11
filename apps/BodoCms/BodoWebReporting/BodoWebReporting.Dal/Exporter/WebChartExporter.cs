// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Charting;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Charting.Util;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using Bodoconsult.Web.Html.Html;
using BodoWebReporting.Dal.Helpers;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;
using System.Data;
using System.Globalization;
using System.Runtime.Versioning;
using Bodoconsult.App.Helpers;

namespace BodoWebReporting.Dal.Exporter;

[SupportedOSPlatform("windows")]
public class WebChartExporter
{
    private bool _exportDataTable;
    private string? _inc;
    private readonly IDataHandler _dataHandler;
    private DataTable? _dt;
    private readonly IBitmapServiceFactory _bitmapServiceFactory;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="bitmapServiceFactory">Current bitmap service factory</param>
    /// <param name="wc">WebChart data</param>
    public WebChartExporter(IDataHandler dataHandler, IBitmapServiceFactory bitmapServiceFactory, WebChart wc)
    {
        _bitmapServiceFactory = bitmapServiceFactory;
        _dataHandler = dataHandler;
        WebChart = wc;
        Typography = ReportingHelper.DefaultTypography();
        BaseCtor();
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="bitmapServiceFactory">Current bitmap service factory</param>
    /// <param name="dt">Current data table with chart data</param>
    /// <param name="wc">WebChart data</param>
    public WebChartExporter(IDataHandler dataHandler, IBitmapServiceFactory bitmapServiceFactory, DataTable dt, WebChart wc)
    {
        _bitmapServiceFactory = bitmapServiceFactory;
        _dataHandler = dataHandler;
        WebChart = wc;
        Typography = ReportingHelper.DefaultTypography();
        BaseCtor();
        _dt = dt;
    }

    private void BaseCtor()
    {
        WebChart.Width = 750;
        WebChart.Height = 433;
    }

    /// <summary>
    /// Typography to use for the chart
    /// </summary>
    public ITypography Typography { get; set; }

    /// <summary>
    /// WebChart data
    /// </summary>
    public WebChart WebChart { get; }

    /// <summary>
    /// Export the chart
    /// </summary>
    public void Export()
    {

        var basePath = _dataHandler.AppGlobals.TargetPath;
        FileSystemHelper.CreateDirectory(basePath);

        var xmlPath = Path.Combine(basePath, "Xml");
        FileSystemHelper.CreateDirectory(xmlPath);

        var path = Path.Combine(basePath, "Xml", $"chart{WebChart.Id}.xml");

        try
        {
            WebChart.Sql = TextHelper.Parser(WebChart.Sql);

            WebChart.Title = TextHelper.Parser(WebChart.Title);

            WebChart.Copyright = TextHelper.Parser(WebChart.Copyright);

            if (_dt is null)
            {
                if (string.IsNullOrEmpty(WebChart.Sql))
                {
                    return;
                }

                _dt = _dataHandler.GetDataTable(WebChart.Sql);

                if (_dt is null)
                {
                    return;
                }
            }

#if DEBUG
            XmlHelper.SaveDataTableAsXml(path, _dt);
#endif

            //Typography.ChartStyle.FontSize = 9;
            //Typography.ChartStyle.Width = Width;
            //Typography.ChartStyle.Height = Height;

            Bodoconsult.Charting.Base.Interfaces.ChartData chartData;

            //#if DEBUG

            //                var path = Path.Combine(@"D:\temp\BodoWebReportingCore\Xml", $"chart{WebChart.Id}.xml");

            //                XmlHelper.SaveDataTableAsXml(path, _dt);

            //                path = Path.Combine(@"D:\temp\BodoWebReportingCore\Xml", $"column{WebChart.Id}.txt");

            //                var s = $"{WebChart.ChartType} {WebChart.Columns}";

            //                if (File.Exists(path)) File.Delete(path);
            //                File.WriteAllText(path, s);




            //#endif


            switch (WebChart.ChartType)
            {
                case ChartType.BarChart:
                case ChartType.ColumnChart:
                    chartData = new Bodoconsult.Charting.Base.Interfaces.ChartData
                    {
                        ChartStyle = Typography.ChartStyle,
                        Title = WebChart.Title,
                        Copyright = $"© {WebChart.Copyright}",
                        YLabelText = WebChart.YUnit,
                        FileName = WebChart.FileName,
                        ChartType = WebChart.ChartType
                    };

                    ChartUtility.DataTableToChartItemData(_dt.Copy(), WebChart.Columns, chartData);

                    break;
                case ChartType.StackedBarChart:
                case ChartType.StackedColumn100Chart:
                case ChartType.StackedColumnChart:
                case ChartType.LineChart:
                    chartData = new Bodoconsult.Charting.Base.Interfaces.ChartData
                    {
                        ChartStyle = Typography.ChartStyle,
                        Title = WebChart.Title,
                        Copyright = $"© {WebChart.Copyright}",
                        YLabelText = WebChart.YUnit,
                        FileName = WebChart.FileName,
                        ChartType = WebChart.ChartType
                    };

                    ChartUtility.DataTableToChartItemData(_dt.Copy(), WebChart.Columns, chartData);

                    break;
                case ChartType.PieChart:
                    chartData = new Bodoconsult.Charting.Base.Interfaces.ChartData
                    {
                        ChartStyle = Typography.ChartStyle,
                        Title = WebChart.Title,
                        Copyright = $"© {WebChart.Copyright}",
                        YLabelText = WebChart.YUnit,
                        FileName = WebChart.FileName,
                        ChartType = WebChart.ChartType
                    };

                    ChartUtility.DataTableToPieChartItemData(_dt.Copy(), WebChart.Columns, chartData);

                    break;
                case ChartType.PointChart:
                    chartData = new Bodoconsult.Charting.Base.Interfaces.ChartData
                    {
                        ChartStyle = Typography.ChartStyle,
                        Title = WebChart.Title,
                        Copyright = $"© {WebChart.Copyright}",
                        YLabelText = WebChart.YUnit,
                        FileName = WebChart.FileName,
                        ChartType = WebChart.ChartType
                    };

                    ChartUtility.DataTableToPointChartItemData(_dt.Copy(), WebChart.Columns, chartData);

                    break;

                case ChartType.StockChart:
                    throw new NotImplementedException();
                case ChartType.Histogram:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var x = new ChartHandler(chartData, _bitmapServiceFactory);
            x.Export();
        }
        catch (Exception ex)
        {

#if DEBUG
            if (_dt is null)
            {
                return;
            }

            XmlHelper.SaveDataTableAsXml(path, _dt);

            path = Path.Combine(xmlPath, $"column{WebChart.Id}.txt");

            var s = $"{WebChart.ChartType} {WebChart.Columns}";

            FileSystemHelper.DeleteFile(path);
            File.WriteAllText(path, s);
#endif

            OnErrorHandlerFired($"WebChart:Export:Id={WebChart.Id}", ex.ToString());
        }
    }

    public void CopyToTarget()
    {
        if (string.IsNullOrEmpty(WebChart.FileName))
        {
            return;
        }

        var ct = _dataHandler.GetWebChartTargets(WebChart.FileName, _exportDataTable ? _inc ?? string.Empty : string.Empty, WebChart.Id);

        var errors = CopyTaskHelper.RunAllTasks(ct);

        if (!errors.Any())
        {
            return;
        }

        var msg = errors.Aggregate("", (current, error) => $"{current}<br/>+{error}");

        _dataHandler.ErrorHandling("Error:WebChartExporter:MoveToTarget", $"<p>{msg}</p>");
    }

    public void ExportDataTable()
    {
        if (_dt is null)
        {
            return;
        }

        try
        {
            _exportDataTable = true;

            if (string.IsNullOrEmpty(WebChart.ColumnsDataTable))
            {
                WebChart.ColumnsDataTable = WebChart.Columns;
            }

            var ex = new DataTableExport
            {
                CssTable = "wr_table",
                CssTableCell = "wr_cell",
                CssAlternatingRows = "_alt",
                CssTableHeader = "wr_header",
                Data = PrepareDataTable(_dt.Copy())
            };

            ex.CreateTable();

            // ReSharper disable AssignNullToNotNullAttribute
            _inc = Path.Combine(Path.GetDirectoryName(WebChart.FileName) ?? string.Empty,
                $"{Path.GetFileNameWithoutExtension(WebChart.FileName)}.inc");
            // ReSharper restore AssignNullToNotNullAttribute

            ex.SaveAsFile(_inc);
        }
        catch (Exception ex)
        {
            OnErrorHandlerFired($"WebChart:ExportDataTable:Id={WebChart.Id}", ex.Message);
        }

    }

    private DataTable? PrepareDataTable(DataTable dt)
    {
        try
        {
            if (string.IsNullOrEmpty(WebChart.ColumnsDataTable))
            {
                return dt;
            }

            var s = WebChart.ColumnsDataTable.Split(';');

            var count = dt.Columns.Count;

            for (var i = count; i > 1; i--)
            {
                var del = true;

                foreach (var z in s)
                {
                    if (z == i.ToString(CultureInfo.InvariantCulture))
                    {
                        del = false;
                    }
                }

                if (del) dt.Columns.RemoveAt(i - 1);
            }

            return dt;
        }
        catch (Exception ex)
        {
            OnErrorHandlerFired($"WebChart:PrepareDataTable:Id={WebChart.Id}", ex.Message);
            return null;
        }
    }

    protected void OnErrorHandlerFired(string subject, string msg)
    {
        _dataHandler.ErrorHandling(subject, msg);
    }
}
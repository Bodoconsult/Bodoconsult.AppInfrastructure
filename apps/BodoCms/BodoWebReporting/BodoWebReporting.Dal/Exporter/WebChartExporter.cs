// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Data;
using System.Globalization;
using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Charting;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Charting.Util;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using Bodoconsult.Web.Html.Html;
using BodoWebReporting.Dal.Handler;
using BodoWebReporting.Dal.Helpers;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Exporter;

[SupportedOSPlatform("windows")]
public class WebChartExporter
{
    private bool _exportDataTable;
    private string? _inc;
    private readonly IDataHandler _dataHandler;
    private DataTable? _dt;
    private IBitmapServiceFactory _bitmapServiceFactory;

    public WebChartExporter(IDataHandler dataHandler, WebChart wc)
    {
        _dataHandler = dataHandler;
        WebChart = wc;
        BaseCtor();
    }

    public WebChartExporter(IDataHandler dataHandler, DataTable dt, WebChart webChart)
    {
        _dataHandler = dataHandler;
        WebChart = webChart;
        BaseCtor();
        _dt = dt;
    }

    private void BaseCtor()
    {
        WebChart.Width = 750;
        WebChart.Height = 433;
        Typography = ReportingHelper.DefaultTypography();
    }

    /// <summary>
    /// Typography to use for the chart
    /// </summary>
    public ITypography Typography { get; set; }

    public WebChart WebChart { get; }


    public void Export()
    {
        try
        {

            WebChart.Sql = TextHelper.Parser(WebChart.Sql);

            WebChart.Title = TextHelper.Parser(WebChart.Title);

            WebChart.Copyright = TextHelper.Parser(WebChart.Copyright);

            if (_dt == null)
            {
                if (string.IsNullOrEmpty(WebChart.Sql))
                {
                    return;
                }

                _dt = _dataHandler.GetDataTable(WebChart.Sql);
            }

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


            ChartHandler x;

            switch (WebChart.ChartType)
            {
                case ChartType.BarChart:
                case ChartType.ColumnChart:
                    chartData = new Bodoconsult.Charting.Base.Interfaces.ChartData
                    {
                        ChartStyle = Typography.ChartStyle,
                        Title = WebChart.Title,
                        Copyright = "© " + WebChart.Copyright,
                        YLabelText = WebChart.YUnit,
                        FileName = WebChart.FileName,
                        ChartType = WebChart.ChartType
                    };

                    ChartUtility.DataTableToChartItemData(_dt.Copy(), WebChart.Columns, chartData);

                    x = new ChartHandler(chartData, _bitmapServiceFactory);
                    x.Export();
                    break;
                case ChartType.StackedBarChart:
                case ChartType.StackedColumn100Chart:
                case ChartType.StackedColumnChart:
                case ChartType.LineChart:
                    chartData = new Bodoconsult.Charting.Base.Interfaces.ChartData
                    {
                        ChartStyle = Typography.ChartStyle,
                        Title = WebChart.Title,
                        Copyright = "© " + WebChart.Copyright,
                        YLabelText = WebChart.YUnit,
                        FileName = WebChart.FileName,
                        ChartType = WebChart.ChartType
                    };

                    ChartUtility.DataTableToChartItemData(_dt.Copy(), WebChart.Columns, chartData);

                    x = new ChartHandler(chartData, _bitmapServiceFactory);
                    x.Export();
                    break;
                case ChartType.PieChart:
                    chartData = new Bodoconsult.Charting.Base.Interfaces.ChartData
                    {
                        ChartStyle = Typography.ChartStyle,
                        Title = WebChart.Title,
                        Copyright = "© "+WebChart.Copyright,
                        YLabelText = WebChart.YUnit,
                        FileName = WebChart.FileName,
                        ChartType = WebChart.ChartType
                    };

                    ChartUtility.DataTableToPieChartItemData(_dt.Copy(), WebChart.Columns, chartData);

                    x = new ChartHandler(chartData, _bitmapServiceFactory);
                    x.Export();
                    break;
                case ChartType.PointChart:
                    chartData = new Bodoconsult.Charting.Base.Interfaces.ChartData
                    {
                        ChartStyle = Typography.ChartStyle,
                        Title = WebChart.Title,
                        Copyright = "© " + WebChart.Copyright,
                        YLabelText = WebChart.YUnit,
                        FileName = WebChart.FileName,
                        ChartType = WebChart.ChartType
                    };

                    ChartUtility.DataTableToPointChartItemData(_dt.Copy(), WebChart.Columns, chartData);

                    x = new ChartHandler(chartData, _bitmapServiceFactory);

                    x.Export();
                    break;

                case ChartType.StockChart:
                    throw new NotImplementedException();
                case ChartType.Histogram:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
        catch (Exception ex)
        {

#if DEBUG

                var path = Path.Combine(@"D:\temp\BodoWebReportingCore\Xml", $"chart{WebChart.Id}.xml");

                XmlHelper.SaveDataTableAsXml(path, _dt);

                path = Path.Combine(@"D:\temp\BodoWebReportingCore\Xml", $"column{WebChart.Id}.txt");

                var s = $"{WebChart.ChartType} {WebChart.Columns}";

                if (File.Exists(path)) File.Delete(path);
                File.WriteAllText(path, s);




#endif

            OnErrorHandlerFired($"WebChart:Export:Id={WebChart.Id}", ex.Message);
        }


    }

    public void CopyToTarget()
    {

        var ct = _dataHandler.GetWebChartTargets(WebChart.FileName, _exportDataTable ? _inc : null, WebChart.Id);

        var errors = CopyTaskHelper.RunAllTasks(ct);

        if (!errors.Any()) return;

        var msg = errors.Aggregate("", (current, error) => current + "<br/>+" + error);

        _dataHandler.ErrorHandling("Error:WebChartExporter:MoveToTarget", $"<p>{msg}</p>");
    }

    public void ExportDataTable()
    {
        try
        {
            _exportDataTable = true;

            if (string.IsNullOrEmpty(WebChart.ColumnsDataTable)) WebChart.ColumnsDataTable = WebChart.Columns;

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
            _inc = Path.Combine(Path.GetDirectoryName(WebChart.FileName), Path.GetFileNameWithoutExtension(WebChart.FileName) + ".inc");
            // ReSharper restore AssignNullToNotNullAttribute

            ex.SaveAsFile(_inc);
        }
        catch (Exception ex)
        {
            OnErrorHandlerFired($"WebChart:ExportDataTable:Id={WebChart.Id}", ex.Message);
        }

    }

    private DataTable PrepareDataTable(DataTable dt)
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
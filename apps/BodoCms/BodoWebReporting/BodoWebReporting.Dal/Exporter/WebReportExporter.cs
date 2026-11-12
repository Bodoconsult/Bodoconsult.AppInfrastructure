// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Drawing;
using Bodoconsult.Pdf.Interfaces;
using Bodoconsult.Pdf.PdfSharp;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;
using BodoWebReporting.Dal.Report;
using MigraDoc.DocumentObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;

// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoWebReporting.Dal.Exporter;

[SupportedOSPlatform("windows")]
public class WebReportExporter : IDisposable
{
    private string _backgroundImagePath = string.Empty;
    private readonly IDataHandler _dataHandler;
    private int _objectCounter;
    private readonly List<BaseReportItem> _reportItems = [];
    private readonly ReportingFile _file;
    private readonly int _id;
    private readonly IWebChartExporterFactory _webChartExporterFactory;

    private int _headingCounter;

    /// <summary>
    /// Divider column width in cm
    /// </summary>
    private readonly double _dividerWidth = 1;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="webChartExporterFactory">Current factory for <see cref="WebChartExporter"/> instances</param>
    /// <param name="styleSetFactory">Current factoy for <see cref="IStyleSet"/> instances</param>
    /// <param name="file">Reporting file data</param>
    /// <param name="id">Running ID</param>
    public WebReportExporter(IDataHandler dataHandler, IWebChartExporterFactory webChartExporterFactory, IStyleSetFactory styleSetFactory, ReportingFile file, int id = 1)
    {
        _webChartExporterFactory = webChartExporterFactory;
        _dataHandler = dataHandler;
        _file = file;
        _id = id;

        // ToDo: move to DI
        StyleSet = styleSetFactory.CreateInstance(file.Landscape);

        ChartWidth = (StyleSet.TypeAreaWidth.Centimeter - _dividerWidth) / 2;
        ChartHeight = ChartWidth / 1.618;   // Goldener Schnitt

        Pdf = new PdfCreator(StyleSet);
#if DEBUG
        Pdf.CreateDdlFile = true;
#endif

    }

    /// <summary>
    /// Background image path or null
    /// </summary>
    public string? BackgroundImagePath
    {
        get => _backgroundImagePath;
        set => _backgroundImagePath = CheckImage(value);
    }

    private static string CheckImage(string? fileName)
    {
        try
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                return string.Empty;
            }

            var thumb = Path.GetTempFileName();

            //if (File.Exists(thumb)) File.Delete(thumb);

            var image = new BitmapService();
            image.LoadBitmap(fileName);

            image.ResizeImage(800, 800);

            image.AdjustSaturation(-0.8F);
            image.AdjustBcg(1.45F, 0.9F, 1F);


            //image.AdjustBcg(1F, -04F, 1F);
            //image.AdjustBcg(0.6F, 1F, 1F);

            image.SaveAsJpeg(thumb);

            return thumb;
        }
        catch (Exception e)
        {
            Debug.Print($"Error: {fileName}: {e.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Steps
    /// </summary>
    public int NumberOfTableRowsPerPage { get; set; }

    /// <summary>
    /// Landscape?
    /// </summary>
    public bool Landscape { get; set; } = true;

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Additional info
    /// </summary>
    public string AdditionalInfo { get; set; } = string.Empty;

    /// <summary>
    /// Subject
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Current styleset
    /// </summary>
    public IStyleSet StyleSet { get; set; }

    /// <summary>
    /// Current number of content elements
    /// </summary>
    public int NumberOfContentElements => Pdf.NumberOfContentElements;

    /// <summary>
    /// Chart width in cm
    /// </summary>
    public double ChartWidth { get; }

    /// <summary>
    /// Chart height in cm
    /// </summary>
    public double ChartHeight { get; }

    /// <summary>
    /// <see cref="PdfCreator"/> instance
    /// </summary>
    public readonly PdfCreator Pdf;

    /// <summary>
    /// Start document generation
    /// </summary>
    public void StartDocument()
    {
        Pdf.NumberOfTableRowsPerPage = NumberOfTableRowsPerPage;

        var meta = StyleSet.DocumentMetaData;

        meta.TocHeading = "Inhaltsverzeichnis";
        meta.HeaderText = Title;
        meta.PageNumberPrefix = "Seite";
        meta.CultureInfo = CultureInfo.CurrentCulture;
        meta.BackgroundImagePath = BackgroundImagePath;

        Pdf.SetDocInfo(Title, Subject, _file.Author);

        Pdf.SetHeader(); Pdf.SetFooter();

    }

    ///// <summary>
    ///// Add a TOC section without custom heading text
    ///// </summary>
    //public void AddTocSection()
    //{
    //    AddTocSection(null);
    //}

    /// <summary>
    /// Create a TOC section with a heading
    /// </summary>
    /// <param name="heading"></param>
    public void AddTocSection(string? heading)
    {
        Pdf.NumberOfTableRowsPerPage = NumberOfTableRowsPerPage;
        Pdf.CreateTocSection(heading);
    }


    /// <summary>
    /// Add a new content section
    /// </summary>
    public void AddContentSection()
    {
        Pdf.NumberOfTableRowsPerPage = NumberOfTableRowsPerPage;
        Pdf.CreateContentSection();
    }


    public void AddTable(string tableTitle, DataTable data)
    {
        Pdf.NumberOfTableRowsPerPage = NumberOfTableRowsPerPage;
        Pdf.AddTableFrame(data, tableTitle, "Heading1", AdditionalInfo, "Details");
    }

    public void AddTable(string tableTitle, DataTable data, bool showInToc, bool addPageBreakIfNecessary)
    {
        Pdf.NumberOfTableRowsPerPage = NumberOfTableRowsPerPage;
        Pdf.AddPageBreakIfNecessary = addPageBreakIfNecessary;

        Pdf.AddTableFrame(data, tableTitle, showInToc ? "Heading1" : "NoHeading1", AdditionalInfo, "Details", Pdf.Width, true);
    }

    /// <summary>
    /// Add a heading to the report
    /// </summary>
    /// <param name="headingTitle">Heading title</param>
    public void AddHeading(string headingTitle)
    {
        var tag = $"H{_headingCounter}";
        Pdf.AddHeading1(headingTitle, tag);
        Pdf.AddToc1Entry(headingTitle, tag);

        _headingCounter++;
    }

    /// <summary>
    /// Add a chart to the report
    /// </summary>
    /// <param name="data">Chart data</param>
    public void AddChart(ChartData data)
    {
        _objectCounter++;

        var fileName = Path.Combine(Path.GetTempPath(), $"tmpR{_id}O{_objectCounter}.png");

        var rc = new ChartReportItem();

        var wc = new WebChart
        {
            Id = _id * 10000 + _objectCounter,
            Width = 900,
            Height = 556,
            Title = data.Title,
            Copyright = _file.Shortname,
            YUnit = data.YLabel,
            FileName = fileName,
            Columns = data.Columns,
            ColumnsDataTable = data.ColumnsDataTable,
            ChartType = data.TypeOfChart
        };

        var x = _webChartExporterFactory.CreateInstance(data.Data, wc);
        rc.Data = x;

        _reportItems.Add(rc);
    }

    /// <summary>
    /// Export the PDF file
    /// </summary>
    public void ExportReport()
    {
        Pdf.RenderToPdf(_file.FileName, false);
    }

    //public int GetCurrentHeightUnits()
    //{
    //    if (_reportItems.Count == 0)
    //    {
    //        return -1;
    //    }

    //    var xy = _reportItems.Count % 2;

    //    return xy;
    //}


    /// <summary>
    /// Ends multicolumn charting
    /// </summary>
    public void EndCharting()
    {
        // No chart at all
        if (_reportItems.Count == 0)
        {
            return;
        }

        // Only a single chart to add
        if (_reportItems.Count == 1)
        {
            AddSingleChart();
            _reportItems.Clear();
            return;
        }

        // Add multiple charts in a 2-column design
        AddMultipleCharts();
        _reportItems.Clear();
    }

    /// <summary>
    /// Add multiple charts to the report in a 2-column design
    /// </summary>
    private void AddMultipleCharts()
    {
        Pdf.TableStart();
        Pdf.TableAddColumn(ParagraphAlignment.Left, ChartWidth);
        Pdf.TableAddColumn(ParagraphAlignment.Left, Pdf.Width - 2 * ChartWidth);
        Pdf.TableAddColumn(ParagraphAlignment.Right, ChartWidth);
        
        var i = 0;
        var zeile = -1;
        foreach (var obj in _reportItems)
        {
            if (i == 0)
            {
                Pdf.TableAddRow();
                zeile++;
            }

            switch (obj.TypeOfObject)
            {
                case ReportItemType.Chart:
                    var chart = (ChartReportItem)obj;
                    chart.Export();
                    if (chart.Data is not null)
                    {
                        Pdf.TableSetContent(i, zeile, chart.Data.WebChart.FileName, ChartWidth, ChartHeight);
                    }

                    break;
                case ReportItemType.SmallTable:
                    var t = (SmallTableReportItem)obj;
                    var height = t.Data?.Rows.Count ?? 0 * 0.8F;
                    if (height < 6F)
                    {
                        height = 6F;
                    }

                    Pdf.TableSetContentSmallTable(i, zeile, t.Data, t.Heading, 12.5, height);
                    break;

            }
            i = i == 2 ? 0 : 2;
        }
    }

    /// <summary>
    /// Only a single chart to add to the report
    /// </summary>
    private void AddSingleChart()
    {
        switch (_reportItems[0].TypeOfObject)
        {
            case ReportItemType.Chart:
                Pdf.AddEmpty();
                Pdf.AddEmpty();
                Pdf.AddEmpty();

                var chart = (ChartReportItem)_reportItems[0];
                chart.Export();

                if (chart.Data is not null)
                {
                    Pdf.AddImage(chart.Data.WebChart.FileName, ChartWidth, ChartHeight);
                }

                break;
            case ReportItemType.SmallTable:
                var t = (SmallTableReportItem)_reportItems[0];
                Pdf.AddTableFrame(t.Data, t.Heading, "NoHeading1");
                //Pdf.AddParagraph("Hallo");
                break;
        }
    }

    //public virtual void StartCharting()
    //{
    //    _reportItems.Clear();
    //}

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Pdf.Dispose();
        if (File.Exists(BackgroundImagePath) && BackgroundImagePath.Contains("Temp"))
        {
            File.Delete(BackgroundImagePath);
        }
    }

    /// <summary>
    /// Add a small table
    /// </summary>
    /// <param name="caption">Table caption</param>
    /// <param name="data">Table data</param>
    public void AddSmallTable(string caption, DataTable data)
    {
        var r = new SmallTableReportItem
        {
            Data = data,
            Heading = caption
        };

        _reportItems.Add(r);
    }

    /// <summary>
    /// Add the page setup
    /// </summary>
    public void AddPageSetup()
    {
        // ToDo: Page settings
        //Pdf.SetPage();

        var ps = StyleSet.PageSetup;

        ps.LeftMargin = Unit.FromCentimeter(1.5);
        ps.RightMargin = Unit.FromCentimeter(1.5);
        ps.TopMargin = Unit.FromCentimeter(1.3);
        ps.BottomMargin = Unit.FromCentimeter(0.5);

        ps.HeaderDistance = Unit.FromCentimeter(0.4);
        ps.FooterDistance = Unit.FromCentimeter(0.5);

        ps.SectionStart = BreakType.BreakNextPage;

        StyleSet.InitializeStyles();
    }

    /// <summary>
    /// Add a page break
    /// </summary>
    public void AddPageBreak()
    {
        Pdf.AddPageBreak();
    }
}
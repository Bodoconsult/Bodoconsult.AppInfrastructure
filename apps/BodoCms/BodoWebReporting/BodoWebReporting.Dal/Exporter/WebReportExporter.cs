// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Drawing;
using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Pdf.Stylesets;
using BodoWebReporting.Dal.Helpers;
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
    private readonly List<BaseReportItem> _reportItems = new();
    private readonly ReportingFile _file;
    private readonly int _id;
    private readonly IWebChartExporterFactory _webChartExporterFactory;

    private int _headingCounter;

    public WebReportExporter(IDataHandler dataHandler, IWebChartExporterFactory webChartExporterFactory, ReportingFile file, int id = 1)
    {
        _webChartExporterFactory = webChartExporterFactory;

        StyleSet = StyleHelper.StyleSet;


        Pdf = new PdfCreator(StyleSet);
#if DEBUG
        Pdf.CreateDdlFile = true;
#endif
        
        //BackColor = new Color(211, 223, 240);
        //TableBorderColor = new Color(217, 230, 245);
        //HeadLineColor = Colors.Black;
        //DetailsColor = Colors.Black;
        //Landscape = true;
        _dataHandler = dataHandler;
        _file = file;
        _id = id;
    }

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
    public int Steps { get; set; }

    public bool Landscape { get; set; } = true;

    public Color BackColor { get; set; } = new(211, 223, 240);
    public Color TableBorderColor { get; set; } = new(217, 230, 245);
    public Color HeadLineColor { get; set; } = Colors.Black;
    public Color DetailsColor { get; set; } = Colors.Black;

    public Color AlternateBackColor { get; set; } = new(217, 230, 245);

    public string Title { get; set; } = string.Empty;

    public string AddInfos { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public IStyleSet StyleSet { get; set; }
    public int NumberOfContentElements => Pdf.NumberOfContentElements;

    public readonly PdfCreator Pdf;


    public void StartDocument()
    {
        BackColor = StyleHelper.BackColor;
        TableBorderColor = StyleHelper.TableBorderColor;
        AlternateBackColor = StyleHelper.AlternateBackColor;

        Pdf.Increment = Steps;

        var meta = StyleSet.DocumentMetaData;

        meta.TocHeading = "Inhaltsverzeichnis";
        meta.HeaderText = Title;
        meta.PageNumberPrefix = "Seite";
        meta.CultureInfo = CultureInfo.CurrentCulture; 

        // ToDo:
        //StyleSet.BackColor = BackColor;
        //Pdf.TableBorderColor = TableBorderColor;
        //Pdf.AlternateBackColor = AlternateBackColor;
        //Pdf.ShadingH1Color = Colors.LightGray;
        //Pdf.ShadingH2Color = TableBorderColor;
        //Pdf.ShadingH3Color = Color.FromRgb(44, 160, 44);
        //Pdf.ShadingRisk1Color = Color.FromRgb(255, 99, 71);
        //Pdf.ShadingRisk2Color = Color.FromRgb(255, 127, 80);

        Pdf.SetDocInfo(Title, Subject, _file.Author);

        // ToDo:
        // Pdf.SetHeader(_file.Author, "Header", _file.LogoPath);
        Pdf.SetHeader();
        Pdf.SetFooter();

    }

    public void TocSection()
    {
        TocSection(null);
    }

    public void TocSection(string? heading)
    {
        //if (File.Exists(BackgroundImagePath))
        //{

        // ToDo:
        //Pdf.BackgroundImagePath = BackgroundImagePath;
        //}
        //else
        //{
        //    Pdf.BackgroundImagePath = null;
        //    if (!string.IsNullOrEmpty(BackgroundImagePath))  Debug.Print($"Not found: {BackgroundImagePath}");
        //}
        Pdf.Increment = Steps;
        Pdf.CreateTocSection(heading);
    }



    public void NewContentSection()
    {
        Pdf.Increment = Steps;
        //if (File.Exists(BackgroundImagePath))
        //{

        //// ToDo:
        //Pdf.BackgroundImagePath = BackgroundImagePath;
        //}
        //else
        //{
        //    Pdf.BackgroundImagePath = null;
        //    Debug.Print($"Not found: {BackgroundImagePath}");
        //}
        Pdf.CreateContentSection();

        //Pdf.SetHeader(_file.Author, "Header", LogoImagePath);
        //Pdf.SetFooter($"{DateTime.Now:dd.MM.yyyy}\t<<page>>", "Footer");
    }


    public void AddTable(string tableTitle, DataTable data)
    {
        Pdf.Increment = Steps;
        Pdf.AddTableFrame(data, tableTitle, "Heading1", AddInfos, "Details");
    }

    public void AddTable(string tableTitle, DataTable data, bool showInToc, bool addPageBreakIfNecessary)
    {
        Pdf.Increment = Steps;
        Pdf.AddPageBreakIfNecessary = addPageBreakIfNecessary;

        Pdf.AddTableFrame(data, tableTitle, showInToc ? "Heading1" : "NoHeading1", AddInfos, "Details", Pdf.Width);
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

    public void AddChart(ChartData data)
    {
        _objectCounter++;

        var fileName = Path.Combine(Path.GetTempPath(), $"tmpR{_id}O{_objectCounter}.png");

        var rc = new ChartReportItem();

        var wc = new WebChart
        {
            Id = _id*10000+_objectCounter,
            Width = 750,
            Height = 454,
            Title = data.Title,
            Copyright = _file.Shortname,
            YUnit = data.YLabel,
            FileName = fileName,
            Columns = data.Columns,
            ColumnsDataTable = data.ColumnsDataTable,
            ChartType = data.TypeOfChart
        };

        var x = _webChartExporterFactory.CreateInstance(data.Data, wc);

        //{
        //    //BackColor = (System.Drawing.Color.FromArgb((int)BackColor.R, (int)BackColor.G, (int)BackColor.B)),
        //    //TableBorderColor = (System.Drawing.Color.FromArgb((int)TableBorderColor.R, (int)TableBorderColor.G, (int)TableBorderColor.B)),
        //};

        rc.Data = x;

        _reportItems.Add(rc);
    }

    public void PageBreak()
    {
        Pdf.NewPage();
    }

    public void ExportReport()
    {
        Pdf.RenderToPdf(_file.FileName, false);
    }

    public int GetCurrentHeightUnits()
    {
        if (_reportItems.Count==0)
        {
            return -1;
        }

        var xy = _reportItems.Count % 2;

        return xy;
    }

    public void EndCharting()
    {
        if (_reportItems.Count == 0)
        {
            return;
        }

        if (_reportItems.Count == 1)
        {
            switch (_reportItems[0].TypeOfObject)
            {
                case ReportItemType.Chart:
                    Pdf.AddParagraph(string.Empty);
                    Pdf.AddParagraph(string.Empty);
                    Pdf.AddParagraph(string.Empty);
                    //Pdf.AddParagraph("Hallo");
                    //Pdf.AddParagraph("Hallo");

                    var chart = (ChartReportItem)_reportItems[0];
                    chart.Export();

                    if (chart.Data is not null)
                    {
                        Pdf.AddImage(chart.Data.WebChart.FileName, 12.5, 7.7);
                    }

                    break;
                case ReportItemType.SmallTable:
                    var t = (SmallTableReportItem)_reportItems[0];
                    Pdf.AddTableFrame(t.Data, t.Heading, "NoHeading1");
                    //Pdf.AddParagraph("Hallo");
                    break;
                //default:
                //    break;

            }

            _reportItems.Clear();

            return;
        }


        Pdf.TableStart();
        Pdf.TableAddColumn(ParagraphAlignment.Left, 13);
        Pdf.TableAddColumn(ParagraphAlignment.Left, 0.8);
        Pdf.TableAddColumn(ParagraphAlignment.Right, 13);
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
                        Pdf.TableSetContent(i, zeile, chart.Data.WebChart.FileName, 12.5, 7.7);
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

        _reportItems.Clear();
    }

    public virtual void StartCharting()
    {
        _reportItems.Clear();
    }

    public void Dispose()
    {
        Pdf.Dispose();
        if (File.Exists(BackgroundImagePath) && BackgroundImagePath.Contains("Temp"))
        {
            File.Delete(BackgroundImagePath);
        }
    }

    public void AddSmallTable(string caption, DataTable data)
    {
        var r = new SmallTableReportItem
        {
            Data = data,
            Heading = caption
        };

        _reportItems.Add(r);
    }

    public void AddPageSetup()
    {
        // ToDo: Page settings
        //Pdf.SetPage();

        var ps = StyleSet.PageSetup;
        ps.Orientation = Landscape ? Orientation.Landscape : Orientation.Portrait;
        var w = ps.PageWidth;
        var h = ps.PageHeight;
        ps.PageHeight = w;
        ps.PageWidth = h;
        ps.LeftMargin = Unit.FromCentimeter(1.5);
        ps.RightMargin = Unit.FromCentimeter(1.5);
        ps.TopMargin = Unit.FromCentimeter(1.3);
        ps.BottomMargin = Unit.FromCentimeter(0.5);

        ps.HeaderDistance = Unit.FromCentimeter(0.4);
        ps.FooterDistance = Unit.FromCentimeter(0.3);

        ps.SectionStart = BreakType.BreakNextPage;

        StyleSet.CalculateMeasures();
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
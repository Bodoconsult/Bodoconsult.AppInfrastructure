// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Data;
using System.Diagnostics;
using System.Runtime.Versioning;
using Bodoconsult.Drawing;
using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Pdf.Stylesets;
using BodoWebReporting.Dal.Handler;
using BodoWebReporting.Dal.Model;
using BodoWebReporting.Dal.Report;
using MigraDoc.DocumentObjectModel;
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoWebReporting.Dal.Exporter;

[SupportedOSPlatform("windows")]
public class WebReportExporter : IDisposable
{

    private int _objectCounter;
    private List<BaseReportItem> _reportItems;

    public int Schritt { get; set; }

    public bool Landscape { get; set; } = true;

    public Color BackColor { get; set; } = new(211, 223, 240);
    public Color TableBorderColor { get; set; } = new (217, 230, 245);
    public Color HeadLineColor { get; set; } = Colors.Black;
    public Color DetailsColor { get; set; } = Colors.Black;

    public Color AlternateBackColor { get; set; } = new(217, 230, 245);

    //public string LogoImagePath { get; set; }

    //public string TargetPath; // { get; set; }

    //public string FileName; // { get; set; }

    public string Title { get; set; } // { get; set; }

    public string AddInfos { get; set; } // { get; set; }

    public string Subject { get; set; } // { get; set; }

    //public string Author; // { get; set; }

    //public string AuthorShort; // { get; set; }


    public IStyleSet StyleSet { get; set; }

    private readonly ReportingFile _file;

    private readonly int _id;


    public string BackgroundImagePath
    {
        get => _backgroundImagePath;
        set => _backgroundImagePath = CheckImage(value);
    }

    private static string CheckImage(string fileName)
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


    //public string Target => _file.FileName;

    public readonly PdfCreator Pdf;


    private string _backgroundImagePath;

    private readonly DataHandler _dataHandler;


    public WebReportExporter(DataHandler dataHandler, ReportingFile file, int id=1)
    {

        StyleSet = StyleHandler.StyleSet;
        

        Pdf = new PdfCreator(StyleSet);
        //BackColor = new Color(211, 223, 240);
        //TableBorderColor = new Color(217, 230, 245);
        //HeadLineColor = Colors.Black;
        //DetailsColor = Colors.Black;
        //Landscape = true;
        _dataHandler = dataHandler;
        _file = file;
        _id = id;
    }


    //public Style AddStyle(string styleName)
    //{
    //    return Pdf.AddStyle(styleName);
    //}


    public void StartDocument()
    {
        BackColor = StyleHandler.BackColor;
        TableBorderColor = StyleHandler.TableBorderColor;
        AlternateBackColor = StyleHandler.AlternateBackColor;

        Pdf.Increment = Schritt;

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


    public void TocSection(string heading)
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
        Pdf.Increment = Schritt;
        Pdf.CreateTocSection(heading);
    }



    public void NewContentSection()
    {
        Pdf.Increment = Schritt;
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
        Pdf.Increment = Schritt;
        Pdf.AddTableFrame(data, tableTitle, "Heading1", AddInfos, "Details");
    }

    public void AddTable(string tableTitle, DataTable data, bool showInToc, bool addPageBreakIfNecessary)
    {
        Pdf.Increment = Schritt;
        Pdf.AddPageBreakIfNecessary = addPageBreakIfNecessary;

        Pdf.AddTableFrame(data, tableTitle, showInToc ? "Heading1" : "NoHeading1", AddInfos, "Details", Pdf.Width);
    }

    public void AddHeading(string headingTitle)
    {
        Pdf.AddParagraph(headingTitle, "Heading1");
    }

    public void AddChart(ChartData data)
    {

        _objectCounter++;

        var fileName = Path.Combine(Path.GetTempPath(), $"tmpR{_id}O{_objectCounter}.png");

        var rc = new ChartReportItem();

        var wc = new WebChart
        {
            Id = _id*10000+_objectCounter,
            Width = 600,
            Height = 500,
            Title = data.Title,
            Copyright = _file.Shortname,
            YUnit = data.YLabel,
            FileName = fileName,
            Columns = data.Columns,
            ColumnsDataTable = data.ColumnsDataTable,
            ChartType = data.TypeOfChart
        };


        var x = new WebChartExporter(_dataHandler, data.Data, wc);


        //{
        //    //BackColor = (System.Drawing.Color.FromArgb((int)BackColor.R, (int)BackColor.G, (int)BackColor.B)),
        //    //TableBorderColor = (System.Drawing.Color.FromArgb((int)TableBorderColor.R, (int)TableBorderColor.G, (int)TableBorderColor.B)),
        //};

        rc.Data = x;

        _reportItems ??= new List<BaseReportItem>();
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
        if (_reportItems == null) return -1;

        var xy = _reportItems.Count % 2;

        return xy;
    }

    public void EndCharting()
    {
        if (_reportItems == null) return;

        if (_reportItems.Count == 1)
        {
            switch (_reportItems[0].TypeOfObject)
            {
                case ReportItemType.Chart:
                    Pdf.AddParagraph("");
                    Pdf.AddParagraph("");
                    Pdf.AddParagraph("");
                    //Pdf.AddParagraph("Hallo");
                    //Pdf.AddParagraph("Hallo");

                    var chart = (ChartReportItem)_reportItems[0];
                    chart.Export();
                    Pdf.AddImage(chart.Data.WebChart.FileName, 12.5, 7.7);

                    break;
                case ReportItemType.SmallTable:
                    var t = (SmallTableReportItem)_reportItems[0];
                    Pdf.AddTableFrame(t.Data, t.Heading, "NoHeading1");
                    //Pdf.AddParagraph("Hallo");
                    break;
                //default:
                //    break;

            }

            _reportItems = null;

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
                    Pdf.TableSetContent(i, zeile, chart.Data.WebChart.FileName, 12.5, 7.7);

                    break;
                case ReportItemType.SmallTable:
                    var t = (SmallTableReportItem)obj;
                    var height = t.Data.Rows.Count * 0.8F;
                    if (height < 6F) height = 6F;

                    Pdf.TableSetContentSmallTable(i, zeile, t.Data, t.Heading, 12.5, height);
                    break;

            }
            i = i == 2 ? 0 : 2;
        }

        _reportItems = null;
    }

    public virtual void StartCharting()
    {
        _reportItems = new List<BaseReportItem>();
    }

    public void Dispose()
    {
        Pdf.Dispose();
        if (File.Exists(BackgroundImagePath) && BackgroundImagePath.Contains("Temp")) File.Delete(BackgroundImagePath);
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
    }
}
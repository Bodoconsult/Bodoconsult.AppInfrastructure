// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Charting.Test.Helpers;
using Bodoconsult.Charting.Util;
using Bodoconsult.Drawing.SkiaSharp.Factories;
using NUnit.Framework;

namespace Bodoconsult.Charting.Test.ChartHandlerTests;

public abstract class BaseChartHandlerTests
{

    protected string FileNameExt = string.Empty;

    protected bool HighResolution;

    protected bool UseDatabase;
    
    [Test]
    public void StackedBarChart_AsPng()
    {

        // const string sql = "EXEC Vermoegen_Db.[dbo].SetFinDBUser 'bodoprivate' exec Vermoegen_Db.[dbo].[GetAnteilswerte] 120, 1";
        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_StackedBarChart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Anlageklassen",
            YLabelText = "Anteilswert",
            FileName = fileName,
            ChartType = ChartType.StackedBarChart,

        };

        TestHelper.LoadDefaultChartStyle(data);

        var dt = TestHelper.GetDataTable("StackedBarChart.xml");
        ChartUtility.DataTableToChartItemData(dt, "", data);

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void StackedColumnChart_AsPng()
    {

        //const string sql = "EXEC Vermoegen_Db.[dbo].SetFinDBUser 'bodoprivate' exec Vermoegen_Db.[dbo].[GetAnteilswerte] 120, 1";

        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_StackedColumnChart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Anlageklassen",
            YLabelText = "Anteilswert",
            FileName = fileName,
            ChartType = ChartType.StackedColumnChart,

        };

        TestHelper.LoadDefaultChartStyle(data);

        var dt = TestHelper.GetDataTable("StackedColumnChart.xml");
        ChartUtility.DataTableToChartItemData(dt, "", data);

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void StackedColumn100Chart_AsPng()
    {
        // const string sql = "EXEC Vermoegen_Db.[dbo].SetFinDBUser 'bodoprivate' exec Vermoegen_Db.[dbo].[GetAnteilswerte] 120, 1";

        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_StackedColumn100Chart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Anlageklassen",
            YLabelText = "Anteilswert",
            FileName = fileName,
            ChartType = ChartType.StackedColumn100Chart,
        };

        TestHelper.LoadDefaultChartStyle(data);

        var dt = TestHelper.GetDataTable("StackedColumnChart.xml");
        ChartUtility.DataTableToChartItemData(dt, "", data);

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void PieChart_AsPng()
    {
        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_PieChart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            YLabelText = "Anteil in %",
            FileName = fileName,
            ChartType = ChartType.PieChart,
        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);

        TestDataHelper.PieChartSample(UseDatabase, data);

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void PointChart_AsPng()
    {
        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_PointChart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Risiko in %",
            YLabelText = "Rendite in %",
            FileName = fileName,
            ChartType = ChartType.PointChart,
        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);
        TestDataHelper.PointChartSample(UseDatabase, data);

        data.ChartStyle.XAxisNumberformat = "0";
        data.ChartStyle.YAxisNumberformat = "0";

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void PointChartPercent_AsPng()
    {
        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_PointChart_Percent.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Risiko",
            YLabelText = "Rendite",
            FileName = fileName,
            ChartType = ChartType.PointChart,

        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);

        TestDataHelper.PointChartSamplePercent(UseDatabase, data);

        data.ChartStyle.XAxisNumberformat = "P0";
        data.ChartStyle.YAxisNumberformat = "P0";

        data.PropertiesToUseForChart.Add("XValue");
        data.PropertiesToUseForChart.Add("YValue");

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void BarChart_AsPng()
    {
        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_BarChart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Anlageklassen",
            YLabelText = "Anteil in %",

            FileName = fileName,
            ChartType = ChartType.BarChart,

        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);

        TestDataHelper.BarChartSample(UseDatabase, data);

        data.ChartStyle.XAxisNumberformat = "0%";

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void ColumnChart_AsPng()
    {
        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_ColumnChart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {

            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Anlageklassen",
            YLabelText = "Anteil in %",


            FileName = fileName,
            ChartType = ChartType.ColumnChart,


        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);
        TestDataHelper.BarChartSample(UseDatabase, data);

        data.ChartStyle.YAxisNumberformat = "0.00";

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }

    [Test]
    public void LineChart_AsPng()
    {
        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_LineChart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Anlageklassen",
            YLabelText = "Anteilwert",
            FileName = fileName,
            ChartType = ChartType.LineChart,
            //PaperColor = Color.Red
        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);

        TestDataHelper.ChartSample(UseDatabase, data);

        data.ChartStyle.XAxisNumberformat = "dd.MM.yyyy";

        data.PropertiesToUseForChart.Add("XValue");
        data.PropertiesToUseForChart.Add("YValue1");
        data.PropertiesToUseForChart.Add("YValue2");
        data.PropertiesToUseForChart.Add("YValue3");

        data.LabelsForSeries.Add("Aktien");
        data.LabelsForSeries.Add("Renten");
        data.LabelsForSeries.Add("Liquidität");

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }



    [Test]
    public void LineChart_SmallValues_AsPng()
    {

        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_LineChart_SmallValues.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Anlageklassen",
            YLabelText = "Anteil in %",
            FileName = fileName,
            ChartType = ChartType.LineChart,

        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);
        TestDataHelper.ChartSampleSmallValues(UseDatabase, data);

        data.ChartStyle.XAxisNumberformat = "0.0";
        data.ChartStyle.YAxisNumberformat = "0.0";

        data.PropertiesToUseForChart.Add("XValue");
        data.PropertiesToUseForChart.Add("YValue1");
        data.PropertiesToUseForChart.Add("YValue2");
        data.PropertiesToUseForChart.Add("YValue3");

        data.LabelsForSeries.Add("Aktien");
        data.LabelsForSeries.Add("Renten");
        data.LabelsForSeries.Add("Liquidität");

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void LineChart_Histogram_AsPng()
    {

        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_LineChart_Histogram.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Histogram",
            Copyright = "Testfirma",
            XLabelText = "Klasse",
            YLabelText = "Anzahl",

            FileName = fileName,
            ChartType = ChartType.Histogram
        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);
        TestDataHelper.ChartSampleHistogram(UseDatabase, data);

        data.ChartStyle.XAxisNumberformat = "0.0%";
        data.ChartStyle.YAxisNumberformat = "0.0%";

        data.PropertiesToUseForChart.Add("XValue");
        data.PropertiesToUseForChart.Add("YValue1");

        //data.LabelsForSeries.Add("Kumulierter Anteil");


        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    [Test]
    public void LineChart_Percentages_AsPng()
    {

        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_LineChart_Percentages.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {
            Title = "Test portfolio",
            Copyright = "Testfirma",
            XLabelText = "Anlageklassen",
            YLabelText = "Anteil in %",
            FileName = fileName,
            ChartType = ChartType.LineChart
        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);
        TestDataHelper.ChartSampleSmallValues(UseDatabase, data);

        data.ChartStyle.XAxisNumberformat = "0,0%";
        data.ChartStyle.YAxisNumberformat = "0,0%";

        data.PropertiesToUseForChart.Add("XValue");
        data.PropertiesToUseForChart.Add("YValue1");
        data.PropertiesToUseForChart.Add("YValue2");
        data.PropertiesToUseForChart.Add("YValue3");

        data.LabelsForSeries.Add("Aktien");
        data.LabelsForSeries.Add("Renten");
        data.LabelsForSeries.Add("Liquidität");

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);
    }


    //[Test]
    //public void LineChart_XAxisDouble_AsPng()
    //{

    //    var fileName = Path.Combine(TestHelper.TestResultPath, "ScottPlott_LineChart.png";

    //    if (File.Exists(fileName)) File.Delete(fileName);

    //    var data = new ChartData<ChartItemData1>
    //    {
    //        Width = 750,
    //        Height = 550,
    //        Title = "Test portfolio",
    //        DataSource = TestData.ChartSampleDouble(),
    //        Copyright = "Testfirma",
    //        XLabelText = "Anlageklassen",
    //        YLabelText = "Anteil in %",
    //        FileName = fileName,
    //        ChartType = ChartType.LineChart,

    //    };

    //    data.PropertiesToUseForChart.Add("XValue");
    //    data.PropertiesToUseForChart.Add("YValue1");
    //    data.PropertiesToUseForChart.Add("YValue2");
    //    data.PropertiesToUseForChart.Add("YValue3");

    //    data.LabelsForSeries.Add("Aktien");
    //    data.LabelsForSeries.Add("Renten");
    //    data.LabelsForSeries.Add("Liquidität");

    //            var bitmapServiceFactory = new BitmapServiceFactory();

    // var x = new ChartHandler(data, bitmapServiceFactory);

    //    x.Export();

    //    Assert.That(File.Exists(fileName));

    //}



    //[Test]
    //public void StackedColumnChart_AsPng()
    //{

    //    var fileName = Path.Combine(TestHelper.TestResultPath, "ScottPlott_StackedColumnChart.png";

    //    if (File.Exists(fileName)) File.Delete(fileName);

    //    var data = new ChartData
    //    {

    //        Title = "Test portfolio",
    //        Copyright = "Testfirma",
    //        XLabelText = "Anlageklassen",
    //        YLabelText = "Anteil in %",
    //        DataSource = TestDataHelper.ChartSample(),

    //        FileName = fileName,
    //        ChartType = ChartType.StackedColumnChart,

    //    };

    //    TestHelper.LoadDefaultChartStyle(data, HighResolution);

    //    data.PropertiesToUseForChart.Add("XValue");
    //    data.PropertiesToUseForChart.Add("YValue1");
    //    data.PropertiesToUseForChart.Add("YValue2");
    //    data.PropertiesToUseForChart.Add("YValue3");

    //    data.LabelsForSeries.Add("Aktien");
    //    data.LabelsForSeries.Add("Renten");
    //    data.LabelsForSeries.Add("Liquidität");

    //                    var bitmapServiceFactory = new BitmapServiceFactory();

    // var x = new ChartHandler(data, bitmapServiceFactory);

    //    x.Export();

    //    TestHelper.StartFile(fileName);
    //}


    //[Test]
    //public void StackedBarChart_AsPng()
    //{

    //    var fileName = Path.Combine(TestHelper.TestResultPath, "ScottPlott_StackedBarChart.png";

    //    if (File.Exists(fileName)) File.Delete(fileName);

    //    var data = new ChartData
    //    {

    //        Title = "Test portfolio",
    //        Copyright = "Testfirma",
    //        XLabelText = "Anlageklassen",
    //        YLabelText = "Anteil in %",
    //        DataSource = TestDataHelper.ChartSample(),

    //        FileName = fileName,
    //        ChartType = ChartType.StackedBarChart,

    //    };

    //    TestHelper.LoadDefaultChartStyle(data, HighResolution);

    //    data.PropertiesToUseForChart.Add("XValue");
    //    data.PropertiesToUseForChart.Add("YValue1");
    //    data.PropertiesToUseForChart.Add("YValue2");
    //    data.PropertiesToUseForChart.Add("YValue3");

    //    data.LabelsForSeries.Add("Aktien");
    //    data.LabelsForSeries.Add("Renten");
    //    data.LabelsForSeries.Add("Liquidität");

    //                    var bitmapServiceFactory = new BitmapServiceFactory();

    // var x = new ChartHandler(data, bitmapServiceFactory);

    //    x.Export();

    //    TestHelper.StartFile(fileName);
    //}



    //[Test]
    //public void StackedColumn100Chart_AsPng()
    //{

    //    var fileName = Path.Combine(TestHelper.TestResultPath, "ScottPlott_StackedColumn100Chart.png";

    //    if (File.Exists(fileName)) File.Delete(fileName);

    //    var data = new ChartData
    //    {


    //        Title = "Test portfolio",
    //        Copyright = "Testfirma",
    //        XLabelText = "Anlageklassen",
    //        YLabelText = "Anteil in %",
    //        DataSource = TestDataHelper.ChartSample(),

    //        FileName = fileName,
    //        ChartType = ChartType.StackedColumn100Chart,

    //    };

    //    TestHelper.LoadDefaultChartStyle(data, HighResolution);

    //    data.PropertiesToUseForChart.Add("XValue");
    //    data.PropertiesToUseForChart.Add("YValue1");
    //    data.PropertiesToUseForChart.Add("YValue2");
    //    data.PropertiesToUseForChart.Add("YValue3");

    //    data.LabelsForSeries.Add("Aktien");
    //    data.LabelsForSeries.Add("Renten");
    //    data.LabelsForSeries.Add("Liquidität");

    //                    var bitmapServiceFactory = new BitmapServiceFactory();

    // var x = new ChartHandler(data, bitmapServiceFactory);

    //    x.Export();

    //    TestHelper.StartFile(fileName);
    //}


    [Test]
    public void StockChart_AsPng()
    {

        var fileName = Path.Combine(TestHelper.TestResultPath, $"ScottPlott{FileNameExt}_StockChart.png");

        if (File.Exists(fileName)) File.Delete(fileName);

        var data = new ChartData
        {

            Title = "Return",
            Copyright = "Testfirma",
            XLabelText = "Periods",
            YLabelText = "Return in %",

            DataSource = TestDataHelper.ChartSampleStockChart(),

            FileName = fileName,
            ChartType = ChartType.StockChart,
            //YAxisNumberformat = "0"
        };

        TestHelper.LoadDefaultChartStyle(data, HighResolution);

        data.PropertiesToUseForChart.Add("XValue");
        data.PropertiesToUseForChart.Add("YValue1");
        data.PropertiesToUseForChart.Add("YValue2");
        data.PropertiesToUseForChart.Add("YValue3");

        //data.LabelsForSeries.Add("Aktien");
        //data.LabelsForSeries.Add("Renten");
        //data.LabelsForSeries.Add("Liquidität");

        var bitmapServiceFactory = new BitmapServiceFactory();

        var x = new ChartHandler(data, bitmapServiceFactory);

        x.Export();

        TestHelper.StartFile(fileName);

    }

}
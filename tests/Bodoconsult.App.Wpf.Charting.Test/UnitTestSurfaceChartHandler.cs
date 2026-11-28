//using System.Diagnostics;
//using System.IO;
//using Bodoconsult.App.Wpf.Charting.Models;
//using Bodoconsult.App.Wpf.Charting.Services;
//using Bodoconsult.App.Wpf.Charting.Test.Utilities;

//// ReSharper disable InconsistentNaming

//namespace Bodoconsult.App.Wpf.Charting.Test;

//[TestFixture]
//public class SurfaceChartHandlerTests
//{
//    [Test]
//    public void TestDrawChartToPng_Peak3D()
//    {
//{

//    [Test]
//    public void TestDrawChartToPng_Peak3D()
//    {
//        const string fileName = @"C:\temp\chart3D_Export.Png";

//        var chart = new SurfaceChartHandler(GetModel());

//        //chart.ChartStyle.Elevation = -30;
//        //chart.ChartStyle.Azimuth = -80;
//        //chart.ChartStyle.DataAreaBackground = Brushes.Red;

//        //Debug.Print(chart.ChartStyle.Elevation.ToString(CultureInfo.InvariantCulture));
//        //Debug.Print(chart.ChartStyle.Azimuth.ToString(CultureInfo.InvariantCulture));

//        // Act
//        chart.DrawChartToPng(fileName);

//        // Assert
//        Assert.That(File.Exists(fileName));
//        Process.Start(fileName);
//    }

//    [Test]
//    public void TestDrawChartToPng_Histogram()
//    {

//        const string fileName = @"C:\temp\chart3D_Export_Histogram.png";


//        var chart = new SurfaceChartHandler(GetModel());
//        chart.UseInterpolation = false;

//        //chart.ChartStyle.Elevation = -30;
//        //chart.ChartStyle.Azimuth = -80;
//        //chart.ChartStyle.DataAreaBackground = Brushes.Red;

//        //Debug.Print(chart.ChartStyle.Elevation.ToString(CultureInfo.InvariantCulture));
//        //Debug.Print(chart.ChartStyle.Azimuth.ToString(CultureInfo.InvariantCulture));

//        // Act
//        chart.DrawChartToPng(fileName);

//        // Assert
//        Assert.That(File.Exists(fileName));
//        Process.Start(fileName);
//    }


//    public ChartStyle GetModel()
//    {
//        ChartStyle? model = null;

//        System.Windows.Application.Current.Dispatcher.Invoke(() =>
//        {

//            model = new ChartStyle
//            {
//                GridlinePattern = DrawSurfaceChart.GridlinePatternEnum.Solid,
//                Elevation = 30,
//                Azimuth = -37,
//                Title = "Risikoanalyse Projekt Infrastruktur XY",
//                IsColorBar = true,
//                XLabel = "Absatz",
//                YLabel = "Betriebskosten",
//                ZLabel = "Wahrscheinlichkeit"
//            };

//            DataUtility.Peak3D(model);

//        });

//        return model;

//    }

//    public ChartStyle GetModelHisto()
//    {

//        ChartStyle model = null;


//        System.Windows.Application.Current.Dispatcher.Invoke(() =>
//        {
//            model = new ChartStyle
//            {
//                GridlinePattern = DrawSurfaceChart.GridlinePatternEnum.Solid,
//                Elevation = 30,
//                Azimuth = -37,
//                Title = "Risikoanalyse Projekt Infrastruktur XY",
//                IsColorBar = true,
//                XLabel = "Absatz",
//                YLabel = "Betriebskosten",
//                ZLabel = "Wahrscheinlichkeit",
//                Xmin = 2017,
//                Xmax = 2026,
//                XTick = 1,
//                Ymin = 0,
//                Ymax = 1.5,
//                YTick = 0.1,
//                Zmin = 0,
//                Zmax = 45,
//                ZTick = 5,
//                NumberFormatZ = "0",
//                NumberFormatX = "0",
//                NumberFormatY = "P1"
//            };

//            DataUtility.Histogram(model);
//        });
//        return model;
//    }

//}
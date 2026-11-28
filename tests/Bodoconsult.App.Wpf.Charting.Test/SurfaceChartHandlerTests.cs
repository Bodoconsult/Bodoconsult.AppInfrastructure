// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Helpers;
using Bodoconsult.App.Wpf.Charting.Models;
using Bodoconsult.App.Wpf.Charting.Services;
using Bodoconsult.App.Wpf.Charting.Test.Utilities;
using Bodoconsult.App.Wpf.Services;
using System.Diagnostics;
using System.IO;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.Wpf.Charting.Test;

[Apartment(ApartmentState.STA)]
[TestFixture]
public class SurfaceChartHandlerTests
{
   [Test]
    public void DrawChartToPng_Peak3D_ChartExported()
    {
        const string fileName = @"C:\temp\chart3D_Export.Png";

        var chart = new SurfaceChartHandler(GetModel());

        //chart.ChartStyle.Elevation = -30;
        //chart.ChartStyle.Azimuth = -80;
        //chart.ChartStyle.DataAreaBackground = Brushes.Red;

        //Debug.Print(chart.ChartStyle.Elevation.ToString(CultureInfo.InvariantCulture));
        //Debug.Print(chart.ChartStyle.Azimuth.ToString(CultureInfo.InvariantCulture));

        // Act
        chart.DrawChartToPng(fileName);

        // Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }

    [Test]
    public void DrawChartToPng_Histogram_ChartExported()
    {
        const string fileName = @"C:\temp\chart3D_Export_Histogram.png";

        var chart = new SurfaceChartHandler(GetModelHisto())
        {
            UseInterpolation = false
        };

        //chart.ChartStyle.Elevation = -30;
        //chart.ChartStyle.Azimuth = -80;
        //chart.ChartStyle.DataAreaBackground = Brushes.Red;

        //Debug.Print(chart.ChartStyle.Elevation.ToString(CultureInfo.InvariantCulture));
        //Debug.Print(chart.ChartStyle.Azimuth.ToString(CultureInfo.InvariantCulture));

        // Act
        chart.DrawChartToPng(fileName);

        // Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }


    public ChartStyle GetModel()
    {
        var model = new ChartStyle
        {
            GridlinePattern = DrawSurfaceChart.GridlinePatternEnum.Solid,
            Elevation = 30,
            Azimuth = -37,
            Title = "Risikoanalyse Projekt Infrastruktur XY",
            IsColorBar = true,
            XLabel = "Absatz",
            YLabel = "Betriebskosten",
            ZLabel = "Wahrscheinlichkeit"
        };

        DataUtility.Peak3D(model);

        return model;
    }

    public ChartStyle GetModelHisto()
    {
        var model = new ChartStyle
        {
            GridlinePattern = DrawSurfaceChart.GridlinePatternEnum.Solid,
            Elevation = 30,
            Azimuth = -37,
            Title = "Risikoanalyse Projekt Infrastruktur XY",
            IsColorBar = true,
            XLabel = "Absatz",
            YLabel = "Betriebskosten",
            ZLabel = "Wahrscheinlichkeit",
            Xmin = 2017,
            Xmax= 2026,
            XTick = 1,
            Ymin = 0,
            Ymax = 1.5,
            YTick = 0.1,
            Zmin = 0,
            Zmax = 45,
            ZTick = 5,
            NumberFormatZ = "0",
            NumberFormatX = "0",
            NumberFormatY = "P1"
        };

        DataUtility.Histogram(model);

        return model;
    }
}
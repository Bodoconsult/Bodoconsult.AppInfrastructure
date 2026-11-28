// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Helpers;
using Bodoconsult.App.Wpf.Charting.Models;
using Bodoconsult.App.Wpf.Charting.Test.Data;
using System.Diagnostics;
using System.IO;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.Wpf.Charting.Test;

[TestFixture]
public class McpsSurfaceChartTests
{
    [Apartment(ApartmentState.STA)]
    [Test]
    public void ExportChartAsPng_ValidChart_ChartExported()
    {
        //Arrange
        const string fileName = @"C:\temp\chart3D_MonteCarloTimePathSurfaceChart_Export.Png";

        var input = new McpsSurfaceChartInputData
        {
            ConstantValue = 5,
            Xmin = 9,
            Xmax = 12,
            Ymin = 1.4,
            Ymax = 1.6,
            TimePeriods = 10,
            Xspacing = 0.1,
            Yspacing = 0.1,
            NumberFormat = "#,##0.0",
            NumberFormatZ = "#,##0"
        };

        var c = new McpsSurfaceChart(input);

        DataHelper.SetModel(c.ChartStyle);

        c.LoadValues();
        c.CalculateValues();

        // Act
        c.ExportChartAsPng(fileName);

        //Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }

    [Explicit]
    [Test]
    public void TestExportChartAsXps()
    {
        //Arrange
        const string fileName = @"C:\temp\chart3D_MonteCarloTimePathSurfaceChart_Export.xps";

        var input = new McpsSurfaceChartInputData
        {
            ConstantValue = 5,
            Xmin = 9,
            Xmax = 12,
            Ymin = 1.4,
            Ymax = 1.6,
            TimePeriods = 10,
            Xspacing = 0.1,
            Yspacing = 0.1,
            NumberFormat = "#,##0.0",
            NumberFormatZ = "#,##0"
        };

        var c = new McpsSurfaceChart(input);

        c.LoadValues();
        c.CalculateValues();


        //var r = new ReportBase();
        //r.AddHeader("Surface chart", 1);

        //r.AddFigure(c.ExportChartAsStream(), "3D surface chart");


        //// Act
        //r.BuildReport();
        //r.SaveAsXps(fileName);

        ////Assert
        //Assert.That(File.Exists(fileName));
        //Process.Start(fileName);

    }
}
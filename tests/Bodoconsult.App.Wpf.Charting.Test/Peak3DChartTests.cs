// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.IO;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Wpf.Charting.Models;
using Bodoconsult.App.Wpf.Charting.Test.Data;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.Wpf.Charting.Test;

[Apartment(ApartmentState.STA)]
[TestFixture]
public class Peak3DChartTests
{
    [Test]
    public void TestExportChartAsPng()
    {
        //Arrange
        const string fileName = @"C:\temp\chart3D_Peak3DChart_Export.Png";

        var c = new Peak3DChart();

        DataHelper.SetModel(c.ChartStyle);

        c.LoadValues();
        c.CalculateValues();

        // Act
        c.ExportChartAsPng(fileName);
            
        //Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }

    [Test]
    public void TestExportChartAsPng_ShowPercentages()
    {
        //Arrange
        const string fileName = @"C:\temp\chart3D_Peak3DChart_ExportPercentage.Png";

        var c = new Peak3DChart();

        DataHelper.SetModel(c.ChartStyle);

        c.ChartStyle.NumberFormatX = "#,##0%";
        c.LoadValues();
        c.CalculateValues();

        // Act
        c.ExportChartAsPng(fileName);

        //Assert
        Assert.That(File.Exists(fileName));
        FileSystemHelper.RunInDebugMode(fileName);
    }
}
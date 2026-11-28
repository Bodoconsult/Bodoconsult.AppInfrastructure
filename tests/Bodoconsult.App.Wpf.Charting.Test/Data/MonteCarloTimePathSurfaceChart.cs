// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Windows.Media.Media3D;
using Bodoconsult.App.Wpf.Charting.Models;

namespace Bodoconsult.App.Wpf.Charting.Test.Data;

public class MonteCarloTimePathSurfaceChart : Base3DChart
{

    public MonteCarloTimePathSurfaceChart()
    {
        Xspacing = 0.001;
        Yspacing = 0.001;
        StartValue = 1;
        XPositive = true;
        YPositive = false;
    }


    #region Input data

    public double StartValue { get; set; }

    public double ConstantValue { get; set; }

    public bool XPositive { get; set; }

    public double Xmin { get; set; }

    public double Xmax { get; set; }

    public double Xspacing { get; set; }

    public bool YPositive { get; set; }

    public double Ymin { get; set; }

    public double Ymax { get; set; }

    public double Yspacing { get; set; }


    public int TimePeriods { get; set; }



    #endregion

    /// <summary>
    /// Load values general data needed to draw the chart
    /// </summary>
    public override void LoadValues()
    {
        var ds = ChartStyle.DataSeries;

        ChartStyle.NumberFormatX = "#,##0.0";
        ChartStyle.Xmin = Xmin;
        ChartStyle.Xmax = Xmax;
        ChartStyle.Ymin = Ymin;
        ChartStyle.Ymax = Ymax;
        ChartStyle.Zmin = 110;
        ChartStyle.Zmax = 150;
        ChartStyle.XTick = 0.5;
        ChartStyle.YTick = 0.1;
        ChartStyle.ZTick = 10;

        ds.XSpacing = Xspacing;
        ds.YSpacing = Yspacing;

        base.LoadValues();
    }

    /// <summary>
    /// Calculate the z-axis values for the 3D chart. May be overwritten for implementing own formulas
    /// </summary>
    public override void CalculateValues()
    {
        var xFactor = XPositive ? 1.0 : -1.0;
        var yFactor = YPositive ? 1.0 : -1.0;

        var min=double.PositiveInfinity;
        var max=double.NegativeInfinity;

        var ds = ChartStyle.DataSeries;
        var pts = new Point3D[ds.XNumber, ds.YNumber];
        for (var i = 0; i < ds.XNumber; i++)
        {
            for (var j = 0; j < ds.YNumber; j++)
            {
                var x = ds.XLimitMin + i * ds.XSpacing;
                var y = ds.YLimitMin + j * ds.YSpacing;

                var z = StartValue;

                for (var period = 1; period < TimePeriods; period++)
                {
                    z += xFactor * x + yFactor * y + ConstantValue;
                }

                max = Math.Max(max, z);
                min = Math.Min(min, z);
                ;

                pts[i, j] = new Point3D(x, y, z);
            }
        }
        ds.PointArray = pts;
    }
}
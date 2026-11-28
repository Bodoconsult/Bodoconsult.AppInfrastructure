// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Windows.Media.Media3D;
using Bodoconsult.App.Wpf.Charting.Models;

namespace Bodoconsult.App.Wpf.Charting.Test.Data;

public class Peak3DChart: Base3DChart
{

    public Peak3DChart()
    { }

    /// <summary>
    /// Load values general data needed to draw the chart
    /// </summary>
    public override void LoadValues()
    {
        var ds = ChartStyle.DataSeries;

        ChartStyle.Xmin = -3;
        ChartStyle.Xmax = 3;
        ChartStyle.Ymin = -3;
        ChartStyle.Ymax = 3;
        ChartStyle.Zmin = -8;
        ChartStyle.Zmax = 8;
        ChartStyle.XTick = 1;
        ChartStyle.YTick = 1;
        ChartStyle.ZTick = 4;


        ds.XSpacing = 0.2;
        ds.YSpacing = 0.2;
            
        base.LoadValues();
    }

    /// <summary>
    /// Calculate the z-axis values for the 3D chart. May be overwritten for implementing own formulas
    /// </summary>
    public override void CalculateValues()
    {
        var ds = ChartStyle.DataSeries;
        var pts = new Point3D[ds.XNumber, ds.YNumber];
        for (var i = 0; i < ds.XNumber; i++)
        {
            for (var j = 0; j < ds.YNumber; j++)
            {
                var x = ds.XLimitMin + i * ds.XSpacing;
                var y = ds.YLimitMin + j * ds.YSpacing;
                var z = 3 * Math.Pow(1 - x, 2) * Math.Exp(-x * x - (y + 1) * (y + 1)) - 10 *
                    // ReSharper disable once PossibleLossOfFraction
                    (0.2 * x - Math.Pow(x, 3) - Math.Pow(y, 5)) * Math.Exp(-x * x - y * y) - 1 / 3 *
                    Math.Exp(-(x + 1) * (x + 1) - y * y);
                pts[i, j] = new Point3D(x, y, z);
            }
        }
        ds.PointArray = pts;
    }
}
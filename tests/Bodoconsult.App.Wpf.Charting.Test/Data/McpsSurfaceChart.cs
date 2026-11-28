// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Windows.Media.Media3D;
using Bodoconsult.App.Wpf.Charting.Helpers;
using Bodoconsult.App.Wpf.Charting.Models;

namespace Bodoconsult.App.Wpf.Charting.Test.Data;

public class McpsSurfaceChart : Base3DChart
{

    public McpsSurfaceChart(McpsSurfaceChartInputData inputData) 
    {
        InputData = inputData;
    }


    #region Input data

    public McpsSurfaceChartInputData InputData { get;  }


    #endregion         

    /// <summary>
    /// Load values general data needed to draw the chart
    /// </summary>
    public override void LoadValues()
    {
        var ds = ChartStyle.DataSeries;

        ChartStyle.NumberFormatX = InputData.NumberFormat;
        ChartStyle.NumberFormatZ = InputData.NumberFormatZ;
        ChartStyle.Xmin = InputData.Xmin;
        ChartStyle.Xmax = InputData.Xmax;
        ChartStyle.Ymin = InputData.Ymin;
        ChartStyle.Ymax = InputData.Ymax;
        ds.XSpacing = InputData.Xspacing;
        ds.YSpacing = InputData.Yspacing;

        base.LoadValues();
    }

    /// <summary>
    /// Calculate the z-axis values for the 3D chart. May be overwritten for implementing own formulas
    /// </summary>
    public override void CalculateValues()
    {
        var xFactor = InputData.XPositive ? 1.0 : -1.0;
        var yFactor = InputData.YPositive ? 1.0 : -1.0;

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

                var z = InputData.StartValue;

                for (var period = 1; period < InputData.TimePeriods; period++)
                {
                    z += xFactor * x + yFactor * y + InputData.ConstantValue;
                }

                max = Math.Max(max, z);
                min = Math.Min(min, z);

                pts[i, j] = new Point3D(x, y, z);
            }
        }
        ds.PointArray = pts;

        if (double.IsNaN(InputData.Xtick))
        {
            InputData.Xtick = ChartHelper.GetScalingForAxis(InputData.Xmin, InputData.Xmax);
        }

        if (double.IsNaN(InputData.Ytick))
        {
            InputData.Ytick = ChartHelper.GetScalingForAxis(InputData.Ymin, InputData.Ymax);
        }



        var scaling = ChartHelper.GetMinMaxForAxis(ref min, ref max);

        ChartStyle.Zmin = min;
        ChartStyle.Zmax = max;

        ChartStyle.XTick = InputData.Xtick;
        ChartStyle.YTick = InputData.Ytick;
        ChartStyle.ZTick = scaling;
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Windows.Media.Media3D;

namespace Bodoconsult.App.Wpf.Charting.Models;

/// <summary>
/// Data series for surface charts
/// </summary>
public class DataSeriesSurface : DataSeriesLine3D
{
    /// <summary>
    /// default ctor
    /// </summary>
    public DataSeriesSurface()
    {
        ZNumber = 10;
        YNumber = 10;
        XNumber = 10;
        ZSpacing = 1;
        YSpacing = 1;
        XSpacing = 1;
        ZLimitMin = -5;
        YLimitMin = 5;
        XLimitMin = -5;
    }

    /// <summary>
    /// Data points for chart surface
    /// </summary>
    public Point3D[,] PointArray { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double XLimitMin { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public double YLimitMin { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public double ZLimitMin { get; set; }

    /// <summary>
    /// Spacing of the x axis
    /// </summary>
    public double XSpacing { get; set; }
    /// <summary>
    /// Spacing of the y axis
    /// </summary>
    public double YSpacing { get; set; }

    /// <summary>
    /// Spacing of the z-axis
    /// </summary>
    public double ZSpacing { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int XNumber { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int YNumber { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ZNumber { get; set; }

    /// <summary>
    /// Get mimimum value for z-axis
    /// </summary>
    /// <returns></returns>
    public double ZDataMin()
    {
        var zmin = double.PositiveInfinity;
        for (var i = 0; i < PointArray.GetLength(0); i++)
        {
            for (var j = 0; j < PointArray.GetLength(1); j++)
            {
                zmin = Math.Min(zmin, PointArray[i, j].Z);
            }
        }
        return zmin;
    }

    /// <summary>
    /// Get maximum value for z axis
    /// </summary>
    /// <returns></returns>
    public double ZDataMax()
    {
        var zmax = double.NegativeInfinity;
        for (var i = 0; i < PointArray.GetLength(0); i++)
        {
            for (var j = 0; j < PointArray.GetLength(1); j++)
            {
                zmax = Math.Max(zmax, PointArray[i, j].Z);
            }
        }
        return zmax;
    }
}
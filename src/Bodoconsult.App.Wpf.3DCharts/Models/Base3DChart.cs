// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.IO;
using System.Windows.Controls;
using Bodoconsult.App.Wpf.Charting.Services;

namespace Bodoconsult.App.Wpf.Charting.Models;

/// <summary>
/// Base class for 3D surface charts
/// </summary>
public abstract class Base3DChart
{
    /// <summary>
    /// Use interpolation for chart drawing
    /// </summary>
    public bool UseInterpolation { get; set; }

    /// <summary>
    /// Number of interpolation steps between to values. Default 2
    /// </summary>
    public int NumberOfInterpolations { get; set; }

    /// <summary>
    /// Default ctor
    /// </summary>
    protected Base3DChart()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            ChartStyle = new ChartStyle();
        });
        UseInterpolation = true;
        NumberOfInterpolations = 2;
    }

    /// <summary>
    /// Keeps the current <see cref="ChartStyle"/> object for the chart
    /// </summary>
    public ChartStyle ChartStyle { get; protected set; }

    /// <summary>
    /// Load values general data needed to draw the chart. 
    /// Overrides must call base.LoadValues(); as last instruction in the overridden method
    /// </summary>
    public virtual void LoadValues()
    {
        var ds = ChartStyle.DataSeries;
        ds.XLimitMin = ChartStyle.Xmin;
        ds.YLimitMin = ChartStyle.Ymin;
        ds.XNumber = Convert.ToInt16(Math.Abs((ChartStyle.Xmax - ChartStyle.Xmin) / ds.XSpacing) + 1);
        ds.YNumber = Convert.ToInt16(Math.Abs((ChartStyle.Ymax - ChartStyle.Ymin) / ds.YSpacing) + 1);
    }

    /// <summary>
    /// Calculate the z-axis values for the 3D chart. May be overwritten for implementing own formulas
    /// </summary>
    public virtual void CalculateValues()
    {

    }

    /// <summary>
    /// Export the chart as PNG file
    /// </summary>
    /// <param name="path"></param>
    /// <param name="width">width in pixels</param>
    /// <param name="height">height in pixels</param>
    public virtual void ExportChartAsPng(string path, double width=1024, double height=768)
    {
        ChartStyle.Width = width;
        ChartStyle.Height = height;

        var chart = new SurfaceChartHandler(ChartStyle)
        {
            UseInterpolation = UseInterpolation,
            NumberOfInterpolations = NumberOfInterpolations
        };
        
        chart.DrawChartToPng(path);
    }

    /// <summary>
    /// Export the chart as PNG file
    /// </summary>
    /// <param name="width">width in pixels</param>
    /// <param name="height">height in pixels</param>
    public virtual MemoryStream? ExportChartAsStream(double width=1024, double height=768)
    {
        ChartStyle.Width = width;
        ChartStyle.Height = height;

        var chart = new SurfaceChartHandler(ChartStyle)
        {
            UseInterpolation = UseInterpolation,
            NumberOfInterpolations = NumberOfInterpolations
        };
        
        return chart.DrawChartToStream();
    }

    /// <summary>
    /// Draw chart in a canvas
    /// </summary>
    /// <returns></returns>
    public Canvas? ExportAsCanvas(double width=1024, double height=768)
    {
        ChartStyle.Width = width;
        ChartStyle.Height = height;

        var chart = new SurfaceChartHandler(ChartStyle)
        {
            UseInterpolation = UseInterpolation,
            NumberOfInterpolations = NumberOfInterpolations
        };

        return chart.DrawChart();
    }


    /// <summary>
    /// Draw surface chart directly into a canvas to show in WPF UI
    /// </summary>
    /// <param name="canvas">Canvas to draw the chart in</param>
    public void DrawToCanvas(Canvas canvas)
    {
        ChartStyle.Width = canvas.ActualWidth;
        ChartStyle.Height = canvas.ActualHeight;

        var chart = new SurfaceChartHandler(ChartStyle)
        {
            UseInterpolation = UseInterpolation,
            NumberOfInterpolations = NumberOfInterpolations
        };

        chart.DrawChart(canvas);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Bodoconsult.App.Wpf.Charting.Models;

/// <summary>
/// Data series for 3D line charts
/// </summary>
public class DataSeriesLine3D 
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DataSeriesLine3D()
    {
        LineThickness = 1;
        LineSeries = new Polyline();
        Point3DList = [];
    }

    /// <summary>
    /// Data for the chart series
    /// </summary>
    public List<Point3D> Point3DList { get; set; }

    /// <summary>
    /// Line color to use for series
    /// </summary>
    public Brush LineColor { get; set; }

    /// <summary>
    /// Line usde for series
    /// </summary>
    public Polyline LineSeries { get; set; }

    /// <summary>
    /// Line width for series
    /// </summary>
    public double LineThickness { get; set; }

    /// <summary>
    /// Line pattern to use for series
    /// </summary>
    public LinePatternEnum LinePattern { get; set; }

    /// <summary>
    /// Add the line pattern
    /// </summary>
    public void AddLinePattern()
    {
        LineSeries.Stroke = LineColor;
        LineSeries.StrokeThickness = LineThickness;

        switch (LinePattern)
        {
            case LinePatternEnum.Dash:
                LineSeries.StrokeDashArray = [4, 3];
                break;
            case LinePatternEnum.Dot:
                LineSeries.StrokeDashArray = [1, 2];
                break;
            case LinePatternEnum.DashDot:
                LineSeries.StrokeDashArray = [4, 2, 1, 2];
                break;
            case LinePatternEnum.None:
                LineSeries.Stroke = Brushes.Transparent;
                break;
            case LinePatternEnum.Solid:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Type of line pattern
    /// </summary>
    public enum LinePatternEnum
    {
#pragma warning disable 1591
        Solid = 1,
        Dash = 2,
        Dot = 3,
        DashDot = 4,
        None = 5
#pragma warning restore 1591
    }
}
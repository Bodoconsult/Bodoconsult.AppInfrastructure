// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Windows;
using System.Windows.Media;
using Bodoconsult.App.Wpf.Charting.Services;

namespace Bodoconsult.App.Wpf.Charting.Models;

/// <summary>
/// 
/// </summary>
public class ChartStyle
{

    #region Ctor

    /// <summary>
    /// default ctor
    /// </summary>
    public ChartStyle()
    {
        ZTick = 3;
        YTick = 1;
        XTick = 1;
        Zmin = -6;
        Zmax = 6;
        Ymin = -3;
        Ymax = 3;
        Xmin = -5;
        Xmax = 5;
        Azimuth = -37.5;
        Elevation = 30;
        ZLabel = "Z Axis";
        YLabel = "Y Axis";
        XLabel = "X Axis";
        Title = "My 3D Chart";
        IsZGrid = true;
        IsYGrid = true;
        IsXGrid = true;
        TickFontSize = (double)new FontSizeConverter().ConvertFrom("11pt");
        TickColor = Brushes.Black;
        TickFont = new FontFamily("Calibri");
        TitleFontSize = (double)new FontSizeConverter().ConvertFrom("15pt");
        TitleColor = Brushes.Black;
        TitleFont = new FontFamily("Cambria");
        TitleShadow = true;
        LabelFontSize = (double)new FontSizeConverter().ConvertFrom("13pt");
        LabelColor = Brushes.Black;
        LabelFont = new FontFamily("Calibri");
        GridlineThickness = 2;
        GridlinePattern = DrawSurfaceChart.GridlinePatternEnum.Dash;
        GridlineColor = Brushes.LightGray;
        AxisThickness = 2;
        AxisPattern = DrawSurfaceChart.AxisPatternEnum.Solid;
        AxisColor = Brushes.Black;
        DataSeries = new DataSeriesSurface();


        var brush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1)
        };

        brush.GradientStops.Add(new GradientStop(Color.FromRgb(208, 223, 255), 0.0));
        brush.GradientStops.Add(new GradientStop(Colors.White, 1.0));


        Background = brush;
        Copyright = "Bodoconsult GmbH";
        CopyrightFont = new FontFamily("Calibri");
        CopyrightFontSize = (double)new FontSizeConverter().ConvertFrom("9pt");
        CopyrightColor = Brushes.Black;
        ChartBorderColor = Color.FromRgb(208, 223, 255);
        ChartBorderWidth = 2;
        PaperColor = Colors.Transparent;
        DataAreaBackground = Brushes.White;
        ChartAreaBackground = new SolidColorBrush(Colors.Transparent);
        Width = 1024;
        Height = 768;
        ChartShadow = 0.0175;
        ChartShadowColor = Colors.DimGray;
    }

        

    #endregion



    #region Public properties:  

    /// <summary>
    /// Show a shadow under the title
    /// </summary>
    public bool TitleShadow { get; set; }

    /// <summary>
    /// Color for the shadow of the chart
    /// </summary>
    public Color ChartShadowColor { get; set; }

    /// <summary>
    /// Delta in % of the chart width for the chart shadow: if 0 then there is no shadow
    /// </summary>
    public double ChartShadow { get; set; }

    /// <summary>
    /// Data series to print in the surface chart
    /// </summary>
    public DataSeriesSurface DataSeries { get; set; }

    /// <summary>
    /// Background for the chart
    /// </summary>
    public Brush Background { get; set; }

    /// <summary>
    /// Numberformat to use for x-axis labels
    /// </summary>
    public string NumberFormatX { get; set; } = "#,##0.00";

    /// <summary>
    /// Numberformat to use for y-axis labels
    /// </summary>
    public string NumberFormatY { get; set; } = "#,##0.00";

    /// <summary>
    /// Numberformat to use for z-axis labels only: default is value of <see cref="NumberFormatX"/>
    /// </summary>
    public string NumberFormatZ { get; set; } = "#,##0.00";


    /// <summary>
    /// Background for the chart area
    /// </summary>
    public Brush ChartAreaBackground { get; set; }


    /// <summary>
    /// Background for the chart area
    /// </summary>
    public Brush DataAreaBackground { get; set; }

    /// <summary>
    /// Show color bar
    /// </summary>
    public bool IsColorBar { get; set; }

    /// <summary>
    /// Axis color
    /// </summary>
    public Brush AxisColor { get; set; }

    /// <summary>
    /// Axis pattern
    /// </summary>
    public DrawSurfaceChart.AxisPatternEnum AxisPattern { get; set; }

    /// <summary>
    /// Axis width
    /// </summary>
    public double AxisThickness { get; set; }
        
    /// <summary>
    /// Gridline color
    /// </summary>
    public Brush GridlineColor { get; set; }

    /// <summary>
    /// Gridline pattern
    /// </summary>
    public DrawSurfaceChart.GridlinePatternEnum GridlinePattern { get; set; }

    /// <summary>
    /// Gridline width
    /// </summary>
    public double GridlineThickness { get; set; }

    /// <summary>
    /// Color of the paper the chart is printed on 
    /// </summary>
    public Color PaperColor { get; set; }


    /// <summary>
    /// Font for labels
    /// </summary>
    public FontFamily LabelFont { get; set; }

    /// <summary>
    /// Brush used for labels
    /// </summary>
    public Brush LabelColor { get; set; }

    /// <summary>
    /// Label font size
    /// </summary>
    public double LabelFontSize { get; set; }

    /// <summary>
    /// Font used for chart title
    /// </summary>
    public FontFamily TitleFont { get; set; }
    /// <summary>
    /// Brush used for chart title
    /// </summary>
    public Brush TitleColor { get; set; }

    /// <summary>
    /// Font size used for chart title
    /// </summary>
    public double TitleFontSize { get; set; }

    /// <summary>
    /// Font used for chart tick
    /// </summary>
    public FontFamily TickFont { get; set; }

    /// <summary>
    /// Brush used for chart tick
    /// </summary>
    public Brush TickColor { get; set; }

    /// <summary>
    /// Font size used for chart tick
    /// </summary>
    public double TickFontSize { get; set; }


    /// <summary>
    /// Copyright to print on the chart
    /// </summary>
    public string Copyright { get; set; }

    /// <summary>
    /// Font to use for copyright
    /// </summary>
    public FontFamily CopyrightFont { get; set; }

    /// <summary>
    /// Brush to use for Copyright
    /// </summary>
    public Brush CopyrightColor { get; set; }

    /// <summary>
    /// Font size to use for copyright
    /// </summary>
    public double CopyrightFontSize { get; set; }


    /// <summary>
    /// Width of the surrounding border of the chart
    /// </summary>
    public int ChartBorderWidth { get; set; }

    /// <summary>
    /// Color of the surrounding border of the chart
    /// </summary>
    public Color ChartBorderColor { get; set; }

    /// <summary>
    /// Width of the chart
    /// </summary>
    public double Width { get; set; }


    /// <summary>
    /// Height of the chart
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// X axis grid visible?
    /// </summary>
    public bool IsXGrid { get; set; }

    /// <summary>
    /// Y axis grid visible
    /// </summary>
    public bool IsYGrid { get; set; }

    /// <summary>
    /// Z axis grid visible
    /// </summary>
    public bool IsZGrid { get; set; }

    /// <summary>
    /// Chart title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// X axis label
    /// </summary>
    public string XLabel { get; set; }

    /// <summary>
    /// Y axis label
    /// </summary>
    public string YLabel { get; set; }

    /// <summary>
    /// Z axis label
    /// </summary>
    public string ZLabel { get; set; }

    /// <summary>
    /// Elevation used for the chart
    /// </summary>
    public double Elevation { get; set; }

    /// <summary>
    /// Azimuth used for the chart
    /// </summary>
    public double Azimuth { get; set; }

    /// <summary>
    /// Max value for x axis
    /// </summary>
    public double Xmax { get; set; }
    /// <summary>
    /// Min value for x axis
    /// </summary>
    public double Xmin { get; set; }

    /// <summary>
    /// Max value for y axis
    /// </summary>
    public double Ymax { get; set; }

    /// <summary>
    /// Min value for y axis
    /// </summary>
    public double Ymin { get; set; }

    /// <summary>
    /// Max value for z axis
    /// </summary>
    public double Zmax { get; set; }
    /// <summary>
    /// Min value for z axis
    /// </summary>
    public double Zmin { get; set; }

    /// <summary>
    /// Tick for x axis
    /// </summary>
    public double XTick { get; set; }

    /// <summary>
    /// Tick for y axis
    /// </summary>
    public double YTick { get; set; }

    /// <summary>
    /// Tick for z axis
    /// </summary>
    public double ZTick { get; set; }

    #endregion


}
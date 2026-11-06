// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Globalization;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Charting.Extensions;
using ScottPlot;

//using GradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle;

namespace Bodoconsult.Charting;

/// <summary>
/// Contains basic functionality for all types of charts in the current library
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseChart<T> : IChart where T : IChartItemData
{
    /// <summary>
    /// Maximum length of a label for one axis
    /// </summary>
    protected int MaxLength = 4;

    /// <summary>
    /// Current X axis offset
    /// </summary>
    protected int OffsetX;

    /// <summary>
    /// Current ticks for an axis
    /// </summary>
    protected Tick[] Ticks;

    /// <summary>
    /// Current tick generator instance for an axis
    /// </summary>
    protected ITickGenerator TickGen;

    /// <summary>
    /// MS-Chart object: maybe used for advanced customizing
    /// </summary>
    public Plot Chart;

    /// <summary>
    /// Contains all data to use in the Chart
    /// </summary>
    public IChartData ChartData { get; set; }

    /// <summary>
    /// Initialize the chart
    /// </summary>
    public void InitChart()
    {
        Chart = new Plot();
        //Chart.AntiAlias(true, true, true);
        Chart.Grid.IsVisible = false;
    }

    /// <summary>
    /// Creates the chart
    /// </summary>
    public virtual void CreateChart()
    {
        AddTitle();
    }

    /// <returns>Byte array</returns>
    public byte[] RenderImagePng(int width, int height)
    {
        var bytes = Chart.GetImageBytes(width, height, ImageFormat.Png);
        return bytes;
    }

    /// <summary>
    /// Add a title to the chart
    /// </summary>
    public virtual void AddTitle()
    {
        Chart.Title(ChartData.Title);
        //Chart.Axes.Title.FullFigureCenter = true;

        var title = Chart.Axes.Title.Label;
        title.Bold = true;
        title.Alignment = Alignment.UpperCenter;
        title.FontName = ChartData.ChartStyle.TitleFontName;
        title.FontSize = (float)Math.Round((double)ChartData.ChartStyle.FontSize * ChartData.ChartStyle.TitleFontSizeDelta, 0);
        title.ForeColor = ChartData.ChartStyle.TitleColor.ToScottPlotColor();

        title.OffsetY = -2 * ChartData.ChartStyle.FontSize;
    }

    /// <summary>
    /// Base formatting for the chart
    /// </summary>
    public virtual void Formatting()
    {
        var style = ChartData.ChartStyle;
        Chart.Axes.AutoScale();

        // Basic grid settings
        Chart.Axes.Margins(bottom: 0, top: 0, left: 0, right: 0);
        Chart.Axes.AntiAlias(true);

        Chart.Axes.Left.FrameLineStyle.Width = 1;
        Chart.Axes.Bottom.FrameLineStyle.Width = 1;

        Chart.Axes.Right.FrameLineStyle.Width = 0;
        Chart.Axes.Top.FrameLineStyle.Width = 0;

        Chart.FigureBackground.Color = TypoColors.Transparent.ToScottPlotColor();
        Chart.DataBackground.Color = ChartData.ChartStyle.BackgroundColor.ToScottPlotColor();
        Chart.Grid.LineColor = TypoColors.Transparent.ToScottPlotColor();
        Chart.Grid.LineWidth = 1;
        Chart.Grid.LinePattern = LinePattern.Dashed;
        Chart.Axes.Title.Label.ForeColor = ChartData.ChartStyle.TitleColor.ToScottPlotColor();

        // Titels for axis

        // X axis
        var label = string.IsNullOrEmpty(ChartData.XLabelText)
            ? ChartData.PropertiesToUseForChart[0]
            : ChartData.XLabelText;

        var labelStyleX = Chart.Axes.Bottom.Label;
        labelStyleX.Text = label;
        labelStyleX.FontName = style.FontName;
        labelStyleX.Bold = true;
        labelStyleX.FontSize = style.FontSize * style.AxisTitleFontSizeDelta;

        // Y axis
        if (string.IsNullOrEmpty(ChartData.YLabelText))
        {
            label = ChartData.PropertiesToUseForChart.Count > 1 ? ChartData.PropertiesToUseForChart[1] : string.Empty;
        }
        else
        {
            label = ChartData.YLabelText;
        }

        if (string.IsNullOrEmpty(label))
        {
            return;
        }

        var labelStyleY = Chart.Axes.Left.Label;
        labelStyleY.Text = label;
        labelStyleY.FontName = style.FontName;
        labelStyleY.Bold = true;
        labelStyleY.FontSize = style.FontSize * style.AxisTitleFontSizeDelta;
    }

    /// <summary>
    /// Get the label for a Chart series
    /// </summary>
    /// <param name="i">index of the series</param>
    /// <returns></returns>
    internal string GetLabelForSeries(int i)
    {
        try
        {
            return ChartData.LabelsForSeries[i];
        }
        catch
        {
            try
            {
                return ChartData.PropertiesToUseForChart[i + 1];
            }
            catch
            {
                return null;
            }

        }
    }

    /// <summary>
    /// Adjust labels to show on x axis
    /// </summary>
    protected void AdjustLabels()
    {
        //var c = Chart.ChartAreas["Default"];

        //var labelStyle = c.AxisX.LabelStyle;
        //labelStyle.IntervalOffset = 1;
        //labelStyle.Angle = 30;
        //labelStyle.Interval = Math.Floor((ChartData.DataSource.Count-1)/15.0);
        //if (labelStyle.Interval < 1) labelStyle.Interval = 1;
        //labelStyle.IsEndLabelVisible = true;


        ////c.AxisX.Crossing = 0;
        //c.AxisX.MajorGrid.Interval = labelStyle.Interval;
        //c.AxisX.MajorGrid.IntervalOffset = labelStyle.IntervalOffset;
        //c.AxisX.MajorGrid.Enabled = true;
        //c.AxisX.MajorTickMark.Interval = labelStyle.Interval;
        //c.AxisX.MajorTickMark.IntervalOffset = labelStyle.IntervalOffset;


        //c.AxisX.IsStartedFromZero = true;
    }

    /// <summary>
    /// Get a color for a series
    /// </summary>
    /// <param name="seriesNumber"></param>
    /// <returns></returns>
    protected static Color GetColor(int seriesNumber)
    {
        return seriesNumber switch
        {
            0 => Colors.OrangeRed,
            1 => Colors.RoyalBlue,
            2 => Colors.Gold,
            3 => Colors.Orange,
            4 => Colors.CadetBlue,
            5 => Colors.Coral,
            6 => Colors.Lime,
            7 => Colors.DarkOrange,
            8 => Colors.Red,
            9 => Colors.Yellow,
            10 => Colors.Blue,
            _ => Color.RandomHue()
        };
    }
}
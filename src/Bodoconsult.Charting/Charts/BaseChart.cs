// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

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

    ///// <summary>
    ///// Get names of all properties to be used as series data
    ///// </summary>
    ///// <returns></returns>
    //public IList<string> GetSeriesNames()
    //{
    //    return ChartData.PropertiesToUseForChart.Skip(1).ToList();
    //}

    ///// <summary>
    ///// Default constructor
    ///// </summary>
    //public BaseChart()
    //{

    //    ////@"Templates\

    //    ////Template = "Default.xml";
    //    //Chart.RenderType = RenderType.BinaryStreaming;
    //    //// Set Antialiasing mode
    //    //Chart.AntiAliasing = AntiAliasingStyles.All;
    //    //Chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;

    //    //// Chart Area
    //    //var c = new ChartArea("Default")
    //    //{
    //    //    AxisY = { IsMarginVisible = false },
    //    //    AxisX = { IsMarginVisible = false },
    //    //    Area3DStyle = { Rotation = 0 },
    //    //};


    //    //Chart.ChartAreas.Add(c);

    //}

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

        var labelStyle = Chart.Axes.Bottom.Label;
        labelStyle.Text = label;
        labelStyle.FontName = style.FontName;
        labelStyle.Bold = true;
        labelStyle.FontSize = style.FontSize * style.AxisTitleFontSizeDelta;

        // Y axis
        label = string.IsNullOrEmpty(ChartData.YLabelText)
            ? ChartData.PropertiesToUseForChart[1]
            : ChartData.YLabelText;

        labelStyle = Chart.Axes.Left.Label;
        labelStyle.Text = label;
        labelStyle.FontName = style.FontName;
        labelStyle.Bold = true;
        labelStyle.FontSize = style.FontSize * style.AxisTitleFontSizeDelta;

        //Chart.BorderWidth = new Unit(ChartData.ChartStyle.ChartBorderWidth);
        //Chart.BorderColor = ChartData.ChartStyle.ChartBorderColor;
        //Chart.BackGradientStyle = (GradientStyle)ChartData.ChartStyle.ChartBackgroundGradientStyle;
        //Chart.BackColor = ChartData.ChartStyle.ChartBackgroundColor;
        //Chart.BackSecondaryColor = ChartData.ChartStyle.ChartBackgroundSecondColor;

        //if (ChartData.ChartStyle.TitleShadow)
        //{
        //    titel.ShadowColor = Color.Gray;
        //    titel.ShadowOffset = 3;
        //}

        //var area = Chart.ChartAreas[0];

        //area.BackColor = ChartData.ChartStyle.BackgroundColor;
        //area.BackSecondaryColor = ChartData.ChartStyle.BackgroundSecondColor;
        //area.BackGradientStyle = (GradientStyle)ChartData.ChartStyle.BackGradientStyle;
        //area.AxisY.IsMarginVisible = false;
        //area.AxisX.IsMarginVisible = false;
        //area.AxisX.LabelStyle.Font = new Font(ChartData.ChartStyle.FontName, ChartData.ChartStyle.FontSize * ChartData.ChartStyle.AxisLabelFontSizeDelta, FontStyle.Regular);
        //area.AxisY.LabelStyle.Font = new Font(ChartData.ChartStyle.FontName, ChartData.ChartStyle.FontSize * ChartData.ChartStyle.AxisLabelFontSizeDelta, FontStyle.Regular);
        //area.AxisX.TitleFont = new Font(ChartData.ChartStyle.FontName, ChartData.ChartStyle.FontSize * ChartData.ChartStyle.AxisTitleFontSizeDelta, FontStyle.Bold);
        //area.AxisY.TitleFont = new Font(ChartData.ChartStyle.FontName, ChartData.ChartStyle.FontSize * ChartData.ChartStyle.AxisTitleFontSizeDelta, FontStyle.Bold);
        //area.AxisX.TitleForeColor = ChartData.ChartStyle.FontColor;
        //area.AxisY.TitleForeColor = ChartData.ChartStyle.FontColor;
        //area.BorderColor = ChartData.ChartStyle.ChartBorderColor;

        //area.AxisY.MajorGrid.LineColor = ChartData.ChartStyle.GridLineColor;
        //area.AxisX.MajorGrid.LineColor = ChartData.ChartStyle.GridLineColor;

        //area.AxisX.LineColor = ChartData.ChartStyle.AxisLineColor;
        //area.AxisY.LineColor = ChartData.ChartStyle.AxisLineColor;
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
        //var dColor = System.Drawing.Color.DeepSkyBlue;

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


    //private string GetFormattedValue(object pointValue, ChartValueType chartValueType)
    //{

    //    if (pointValue is string) return pointValue.ToString();

    //    if (pointValue is double)
    //    {
    //        var value = (double)pointValue;

    //        switch (chartValueType)
    //        {
    //            case ChartValueType.Auto:
    //                break;
    //            case ChartValueType.Double:
    //                break;
    //            case ChartValueType.Single:
    //                break;
    //            case ChartValueType.Int32:
    //                break;
    //            case ChartValueType.Int64:
    //                break;
    //            case ChartValueType.UInt32:
    //                break;
    //            case ChartValueType.UInt64:
    //                break;
    //            case ChartValueType.String:
    //                break;
    //            case ChartValueType.DateTime:
    //            case ChartValueType.Date:
    //                return DateTime.FromOADate((int)value).ToString("dd.MM.yyyy");
    //            case ChartValueType.Time:
    //                return value.ToString("hh:mm");
    //            case ChartValueType.DateTimeOffset:
    //                break;
    //            default:
    //                throw new ArgumentOutOfRangeException("chartValueType", chartValueType, null);
    //        }


    //    }

    //    return pointValue.ToString();
    //}
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Contains all properties for formatting a chart
/// </summary>
public class ChartStyle: ICloneable
{

    /// <summary>
    /// Default constructor
    /// </summary>
    public ChartStyle()
    {

        const int defaultLineWidth = 1;

        Width = 750;
        Height = 464;   // Goldener Schnitt: Width / 1,618 
        Template = "default.xml";
        TitleFontSizeDelta = 1.2F;
        AxisLabelFontSizeDelta = 1.0F;
        AxisTitleFontSizeDelta = 1.1F;
        LegendFontSizeDelta = 0.9F;
        BorderLineWidth = defaultLineWidth;
        IntervalXLineWidth = defaultLineWidth;
        IntervalYLineWidth = defaultLineWidth;
        SeriesLineWidth = defaultLineWidth;
        TitleFontName = "Cambria";
        ChartBorderCornerRadius = 25;
        CorrectiveFactor = 5;
        ChartBorderWidth = defaultLineWidth;
        ChartBorderColor = TypoColor.FromArgb(178, 204, 255);
        ChartBackgroundColor = TypoColor.FromArgb(178, 204, 255);
        ChartBackgroundSecondColor = TypoColors.White;
        ChartBackgroundGradientStyle = GradientStyle.TopBottom;
        BackgroundColor = TypoColor.FromArgb(208, 223, 255);
        BackgroundSecondColor = TypoColor.FromArgb(208, 223, 255);
        BackGradientStyle = GradientStyle.None;

        TitleColor = TypoColor.FromArgb(26, 59, 124);
        TitleShadow = false;
        CopyrightColor = TypoColors.Black;
        PaperColor = TypoColors.Transparent;
        ChartShadow = 0.0175;
        ChartShadowColor = TypoColors.DimGray;
        BorderColor = TypoColors.DarkBlue;
        GridLineColor = TypoColors.CornflowerBlue;
        AxisLineColor = TypoColors.Black;
    }

    /// <summary>
    /// Color for axis lines
    /// </summary>
    public TypoColor AxisLineColor { get; set; }

    /// <summary>
    /// Color of the grid lines
    /// </summary>
    public TypoColor GridLineColor { get; set; }

    /// <summary>
    /// Color of the chart area border
    /// </summary>
    public TypoColor BorderColor { get; set; }

    /// <summary>
    /// Show a shadow under the title
    /// </summary>
    public bool TitleShadow { get; set; }

    /// <summary>
    /// Color for the shadow of the chart
    /// </summary>
    public TypoColor ChartShadowColor { get; set; }

    /// <summary>
    /// Delta in % of the chart width for the chart shadow: if 0 then there is no shadow
    /// </summary>
    public double ChartShadow { get; set; }

    /// <summary>
    /// Color of the paper in the background of the chart: Standard is <see cref="TypoColors.Transparent"/>
    /// </summary>
    public TypoColor PaperColor { get; set; }


    /// <summary>
    /// Gradient style for the chart background
    /// </summary>
    public GradientStyle ChartBackgroundGradientStyle { get; set; }

    /// <summary>
    /// Background color of the chart
    /// </summary>
    public TypoColor ChartBackgroundColor { get; set; }


    /// <summary>
    /// Second background color of the chart used for gradients. See <see cref="ChartBackgroundGradientStyle"/>
    /// </summary>
    public TypoColor ChartBackgroundSecondColor { get; set; }

    /// <summary>
    /// Factor to higher or lower font size for copyright relative to <see cref="FontSize"/>
    /// </summary>
    public float CopyrightFontSizeDelta { get; set; } = 0.6F;

    /// <summary>
    /// Factor to higher or lower font size for title relative to <see cref="FontSize"/>
    /// </summary>
    public float TitleFontSizeDelta { get; set; }

    /// <summary>
    /// Factor to higher or lower font size for legend relative to <see cref="FontSize"/>
    /// </summary>
    public float LegendFontSizeDelta { get; set; }

    /// <summary>
    /// Factor to higher or lower font size for axis labels relative to <see cref="FontSize"/>
    /// </summary>
    public float AxisLabelFontSizeDelta { get; set; }

    /// <summary>
    /// Factor to higher or lower font size for axis titles relative to <see cref="FontSize"/>
    /// </summary>
    public float AxisTitleFontSizeDelta { get; set; }

    /// <summary>
    /// Font name used for the chart title
    /// </summary>
    public string TitleFontName { get; set; }

    /// <summary>
    /// Font name used for all text labels in a chart except the title
    /// </summary>
    public string FontName { get; set; } = "Calibri";


    /// <summary>
    /// Width of the chart areas borderline
    /// </summary>
    public int BorderLineWidth { get; set; }

    /// <summary>
    /// Line width for X-axis interval
    /// </summary>
    public int IntervalXLineWidth { get; set; }

    /// <summary>
    /// Line width for Y-axis interval
    /// </summary>
    public int IntervalYLineWidth { get; set; }


    /// <summary>
    /// Line width for series lines
    /// </summary>
    public int SeriesLineWidth { get; set; }


    ///// <summary>
    ///// Export charts in high quality
    ///// </summary>
    //public bool HighQuality { get; set; }

    /// <summary>
    /// Base font size used for the charts. Use <see cref="TitleFontSizeDelta"/>, <see cref="CopyrightFontSizeDelta"/>, <see cref="AxisLabelFontSizeDelta"/>, <see cref="LegendFontSizeDelta"/>
    /// and <see cref="AxisTitleFontSizeDelta"/> to adjusted font sizes for certain elements relative to base font size
    /// </summary>
    public float FontSize { get; set; } = 12;

    ///// <summary>
    ///// Names of the columns in the data source to be used for the chart
    ///// </summary>
    //public string Columns;

    ///// <summary>
    ///// Names of the columns in the data source to be used for the chart
    ///// </summary>
    //public string ColumnsDataSource;

    /// <summary>
    /// Left position
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// Maximum length for a legend entry
    /// </summary>
    public int LegendLength { get; set; }

    /// <summary>
    /// Height in pixels
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Width in pixels
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Template file name without path (template must be placed in subfolder \Template)
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// Numberformat used to format values of YAxis
    /// </summary>
    public string YAxisNumberformat { get; set; } = "0,000";

    /// <summary>
    /// Numberformat used to format values of XAxis
    /// </summary>
    public string XAxisNumberformat { get; set; } = "0,000";

    /// <summary>
    /// Radius in pixels for the border corners. If no rounded corners requested for the chart, set BorderCornerRadius=0
    /// </summary>
    public int ChartBorderCornerRadius { get; set; }

    /// <summary>
    /// MS Chart have unrequested margin bottom. With CorrectiveFactor measured in pixels you can remove this margin. Margin is size dependent.
    /// </summary>
    public int CorrectiveFactor { get; set; }

    /// <summary>
    /// Width of the surrounding border of the chart
    /// </summary>
    public int ChartBorderWidth { get; set; }

    /// <summary>
    /// Color of the surrounding border of the chart
    /// </summary>
    public TypoColor ChartBorderColor { get; set; }

    /// <summary>
    /// Background color of the chart area
    /// </summary>
    public TypoColor BackgroundColor { get; set; }

    /// <summary>
    /// Second background color of the chart area used for gradients. See <see cref="BackGradientStyle"/>
    /// </summary>
    public TypoColor BackgroundSecondColor { get; set; }

    /// <summary>
    /// Chart areas style
    /// </summary>
    public GradientStyle BackGradientStyle { get; set; }

    /// <summary>
    /// Font color of the title
    /// </summary>
    public TypoColor TitleColor { get; set; }

    /// <summary>
    /// Font color of all texts except title and copyright
    /// </summary>
    public TypoColor FontColor { get; set; } = TypoColors.Black;

    /// <summary>
    /// Font color of the copyright
    /// </summary>
    public TypoColor CopyrightColor { get; set; }

    /// <summary>
    /// Create a shallow clone of the object
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        return MemberwiseClone();
    }
}
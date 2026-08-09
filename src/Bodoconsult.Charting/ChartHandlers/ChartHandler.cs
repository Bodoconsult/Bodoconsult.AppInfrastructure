// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Charting.Extensions;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using Bodoconsult.Drawing.SkiaSharp.Services;
using SkiaSharp;

namespace Bodoconsult.Charting;

// https://stackoverflow.com/questions/61044246/how-to-draw-complex-round-border-with-skiasharp

// https://swharden.com/csdv/skiasharp/skiasharp/

/// <summary>
/// Handles the creation process of a chart
/// </summary>

public class ChartHandler : IChartHandler
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="chartData">Current <see cref="ChartData"/> instance to use for chart drawing containing all data to use in the chart</param>
    /// <param name="bitmapServiceFactory">Current <see cref="IBitmapService"/> instance to use for chart creation</param>
    public ChartHandler(IChartData chartData, IBitmapServiceFactory bitmapServiceFactory)
    {
        ChartData = chartData;
        BitmapServiceFactory = bitmapServiceFactory;
    }

    /// <summary>
    /// Current <see cref="ChartData"/> instance to use for chart drawing containing all data to use in the chart
    /// </summary>
    public IChartData ChartData { get; }

    /// <summary>
    /// Current <see cref="IBitmapServiceFactory"/> instance to create <see cref="IBitmapService"/> instances
    /// </summary>
    public IBitmapServiceFactory BitmapServiceFactory { get; }

    ///// <summary>
    ///// Set chart data to high resolution: 4500 x 3000 pixels
    ///// </summary>
    //public void SetChartToHighResolution()
    //{
    //    ChartData.ChartStyle.Width = 4500;
    //    ChartData.ChartStyle.Height = 3300;
    //    ChartData.ChartStyle.FontSize = 70;
    //    ChartData.ChartStyle.HighQuality = true;
    //    ChartData.ChartStyle.BorderLineWidth = 2;
    //    ChartData.ChartStyle.IntervalXLineWidth = 2;
    //    ChartData.ChartStyle.IntervalYLineWidth = 2;
    //    ChartData.ChartStyle.ChartBorderCornerRadius = 100;
    //    ChartData.ChartStyle.CorrectiveFactor = 30;
    //    ChartData.ChartStyle.ChartBorderWidth = 10;

    //    switch (ChartData.ChartType)
    //    {
    //        case ChartType.StackedBarChart:
    //        case ChartType.StackedColumn100Chart:
    //        case ChartType.StackedColumnChart:
    //            ChartData.ChartStyle.SeriesLineWidth = 0;
    //            break;
    //        default:
    //            ChartData.ChartStyle.SeriesLineWidth = 15;
    //            break;
    //    }
    //}


    /// <summary>
    /// Export chart as PNG file or JPEG if file extension of target file (ChartData.FileName) is .jpg or .jpeg
    /// </summary>
    public void Export()
    {


        if (File.Exists(ChartData.FileName))
        {
            File.Delete(ChartData.FileName);
        }

        var fi = new FileInfo(ChartData.FileName);

        var ext = fi.Extension.ToLowerInvariant();

        var bs = CreateBitmap();

        switch (ext)
        {
            case ".jpg":
            case ".jpeg":

                // ToDo: add JPEG quality to ChartData
                bs.SaveAsJpeg(ChartData.FileName, ChartData.Quality);
                break;
            default:
                bs.SaveAsPng(ChartData.FileName);
                break;
        }
        
        bs.Dispose();
    }

    /// <summary>
    /// Export chart as PNG file memory stream
    /// </summary>
    public MemoryStream ExportMemoryStream()
    {
        using var bs = CreateBitmap();
        return bs.SaveAsMemoryStream();
    }

    /// <summary>
    /// Create a <see cref="BitmapService"/> instance with the chart included. Use this method instead of export if you want to enhance your charts with custom drawing (use BitmapService.CurrentCanvas)
    /// </summary>
    /// <returns><see cref="BitmapService"/> instance with the chart included</returns>
    public IBitmapService CreateBitmap()
    {
        // New bitmap
        var bs = BitmapServiceFactory.CreateInstance();
        bs.NewBitmap(ChartData.ChartStyle.Width, ChartData.ChartStyle.Height);

        // get chart measures
        var deltaX = (int)(ChartData.ChartStyle.Width * ChartData.ChartStyle.ChartShadow);
        var deltaY = (int)(ChartData.ChartStyle.Width * ChartData.ChartStyle.ChartShadow);

        var rectWidth = ChartData.ChartStyle.Width - deltaX;
        var rectHeight = ChartData.ChartStyle.Height - deltaY;

        var effLeft = ChartData.ChartStyle.ChartBorderCornerRadius;
        var effTop = ChartData.ChartStyle.ChartBorderCornerRadius;
        var effWidth = ChartData.ChartStyle.Width - deltaX - 2 * ChartData.ChartStyle.ChartBorderCornerRadius;
        var effHeight = ChartData.ChartStyle.Height - deltaY - 2 * ChartData.ChartStyle.ChartBorderCornerRadius;

        var color1 = ChartData.ChartStyle.ChartBackgroundColor?.ToSkiaColor() ?? SKColors.White;
        var color2 = ChartData.ChartStyle.ChartBackgroundSecondColor?.ToSkiaColor() ?? SKColors.White;
        var borderColor = ChartData.ChartStyle.BorderColor?.ToSkiaColor() ?? SKColors.Black;
        var shadowColor = ChartData.ChartStyle.ChartShadowColor?.ToSkiaColor() ?? SKColors.DarkGray;

        // Draw shadow and chart background: fill with gradients
        if (ChartData.ChartStyle.BackGradientStyle == GradientStyle.TopBottom)
        {
            if (ChartData.ChartStyle.ChartBorderCornerRadius > 0)
            {
                bs.DrawRoundedRectangle(deltaX, deltaY, deltaX + rectWidth, deltaY + rectHeight, shadowColor,
                    shadowColor,
                    ChartData.ChartStyle.ChartBorderCornerRadius, ChartData.ChartStyle.ChartBorderWidth);
                bs.DrawRoundedRectangleWithVerticalGradient(0, 0, rectWidth, rectHeight, color1, color2, borderColor,
                    ChartData.ChartStyle.ChartBorderCornerRadius, ChartData.ChartStyle.ChartBorderWidth);
            }
            else
            {
                bs.DrawRectangle(deltaX, deltaY, deltaX + rectWidth, deltaY + rectHeight, shadowColor, shadowColor,
                    ChartData.ChartStyle.ChartBorderWidth);
                bs.DrawRectangle(0, 0, rectWidth, rectHeight, color1, borderColor,
                    ChartData.ChartStyle.ChartBorderWidth);
            }
        }
        else // Draw shadow and chart background: fill with no gradients
        {
            if (ChartData.ChartStyle.ChartBorderCornerRadius > 0)
            {
                bs.DrawRoundedRectangle(deltaX, deltaY, deltaX + rectWidth, deltaY + rectHeight, shadowColor,
                    shadowColor,
                    ChartData.ChartStyle.ChartBorderCornerRadius, ChartData.ChartStyle.ChartBorderWidth);
                bs.DrawRoundedRectangleWithVerticalGradient(0, 0, rectWidth, rectHeight, color1, color2, borderColor,
                    ChartData.ChartStyle.ChartBorderCornerRadius, ChartData.ChartStyle.ChartBorderWidth);
            }
            else
            {
                bs.DrawRectangle(deltaX, deltaY, deltaX + rectWidth, deltaY + rectHeight, shadowColor, shadowColor,
                    ChartData.ChartStyle.ChartBorderWidth);
                bs.DrawRectangle(0, 0, rectWidth, rectHeight, color1, borderColor,
                    ChartData.ChartStyle.ChartBorderWidth);
            }
        }

        if (ChartData.DataSource is null || ChartData.DataSource.Count == 0)
        {
            const string msg = "No data available! Keine Daten verfügbar!";

            bs.DrawText(msg, rectWidth / 2, rectHeight / 2, ChartData.ChartStyle.FontName,
                ChartData.ChartStyle.FontSize,
                SKColors.Black, SKTextAlign.Center);
        }
        else
        {
            var chart = CreateBaseChart();
            var bytes = chart.RenderImagePng(effWidth, effHeight);
            bs.DrawPng(bytes, effLeft, effTop, effLeft + effWidth, effTop + effHeight);

            // Copyright
            if (!string.IsNullOrEmpty(ChartData.Copyright))
            {
                bs.DrawText(ChartData.Copyright, effLeft + effWidth - ChartData.ChartStyle.ChartBorderCornerRadius - 5,
                    effTop + effHeight - 5, ChartData.ChartStyle.FontName,
                    ChartData.ChartStyle.CopyrightFontSizeDelta * ChartData.ChartStyle.FontSize,
                    ChartData.ChartStyle.CopyrightColor.ToSkiaColor(), SKTextAlign.Right);
            }
        }

        return bs;
    }

    /// <summary>
    /// Create the chart
    /// </summary>
    /// <returns></returns>
    private IChart CreateBaseChart()
    {

        AdjustChartDataToRequestedSize();

        if (ChartData.DataSource is null)
        {
            throw new Exception("ChartHandler.Export: DataSource is empty");
        }

        if (ChartData.PropertiesToUseForChart.Count == 0)
        {
            throw new Exception("ChartHandler.Export: PropertiesToUseForChart is empty");
        }


        if (string.IsNullOrEmpty(ChartData.XName))
        {
            ChartData.XName = ChartData.PropertiesToUseForChart[0];
        }


        if (string.IsNullOrEmpty(ChartData.XLabelText))
        {
            ChartData.XLabelText = ChartData.XName;
        }

        if (ChartData.LabelsForSeries.Count == 0)
        {
            ChartData.LabelsForSeries = ChartData.PropertiesToUseForChart.Skip(1).ToList();
        }

        IChart chart = ChartData.ChartType switch
        {
            ChartType.ColumnChart => new ColumnChart<ChartItemData>(),
            ChartType.BarChart => new BarChart<ChartItemData>(),
            ChartType.StackedBarChart => new StackedBarChart<ChartItemData>(),
            ChartType.PointChart => new PointChart<PointChartItemData>(),
            ChartType.StackedColumnChart => new StackedColumnChart<ChartItemData>(),
            ChartType.StackedColumn100Chart => new StackedColumn100Chart<ChartItemData>(),
            ChartType.PieChart => new PieChart<PieChartItemData>(),
            ChartType.LineChart => new LineChart<ChartItemData>(),
            ChartType.Histogram => new LineChartHistogram<ChartItemData>(),
            ChartType.StockChart => new StockChart<ChartItemData>(),
            _ => throw new NotImplementedException($"No such chart type: {ChartData.ChartType}")
        };

        chart.ChartData = (ChartData)ChartData.Clone();
        chart.InitChart();
        chart.CreateChart();
        chart.Formatting();

        return chart;
    }

    private void AdjustChartDataToRequestedSize()
    {

        var ratio = (float)(1.0 + (ChartData.ChartStyle.Width / 750F - 1F) * 0.92);

        if (ratio < 1)
        {
            ratio = 1;
        }

        ChartData.ChartStyle.FontSize = ChartData.ChartStyle.FontSize * ratio * 1.3f;
        ChartData.ChartStyle.BorderLineWidth = (int)(ChartData.ChartStyle.BorderLineWidth * ratio);
        ChartData.ChartStyle.IntervalXLineWidth = (int)(ChartData.ChartStyle.IntervalXLineWidth * ratio);
        ChartData.ChartStyle.IntervalYLineWidth = (int)(ChartData.ChartStyle.IntervalYLineWidth * ratio);
        ChartData.ChartStyle.ChartBorderCornerRadius = (int)(ChartData.ChartStyle.ChartBorderCornerRadius * ratio);
        //if (ChartData.ChartStyle.Width>750)  ChartData.ChartStyle.CorrectiveFactor = 30;
        ChartData.ChartStyle.ChartBorderWidth = (int)(ChartData.ChartStyle.ChartBorderWidth * ratio);
        ChartData.ChartStyle.IntervalYLineWidth = (int)(ChartData.ChartStyle.IntervalYLineWidth * ratio);
        ChartData.ChartStyle.IntervalXLineWidth = (int)(ChartData.ChartStyle.IntervalXLineWidth * ratio);

        ChartData.ChartStyle.SeriesLineWidth = ChartData.ChartType switch
        {
            ChartType.StackedBarChart or ChartType.StackedColumn100Chart or ChartType.StackedColumnChart => 0,
            _ => (int)(ChartData.ChartStyle.SeriesLineWidth * ratio * 1.2)
        };
    }
}
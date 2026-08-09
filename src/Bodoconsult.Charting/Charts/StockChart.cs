// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Charting.Base.Interfaces;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace Bodoconsult.Charting;

/// <summary>
/// Creates a stock chart
/// </summary>
/// <typeparam name="T"></typeparam>
public class StockChart<T> : BaseChart<T> where T: IChartItemData
{
    private readonly List<ChartItemData> _data = [];

    /// <summary>
    /// Creates the chart
    /// </summary>
    public override void CreateChart()
    {

        foreach (var item in ChartData.DataSource)
        {
            if (item is ChartItemData cid)
            {
                _data.Add(cid);
            }
        }

        if (_data.Count == 0)
        {
            return;
        }

        var minX = double.MaxValue;
        var maxX = double.MinValue;

        var color = Colors.Red;

        foreach (var item in _data)
        {
            if (item.XValue < minX)
            {
                minX = item.XValue;
            }

            if (item.XValue > maxX)
            {
                maxX = item.XValue;
            }

            var box = Chart.Add.Scatter(item.XValue, item.YValue1);

            box.MarkerColor = color;
            box.MarkerShape = MarkerShape.FilledSquare;
            box.MarkerSize = 1.5F * box.MarkerSize;

            var line1 = Chart.Add.Line(new Coordinates(item.XValue, item.YValue1), new Coordinates(item.XValue, item.YValue2));
            line1.Color = color;

            var line2 = Chart.Add.Line(new Coordinates(item.XValue, item.YValue1), new Coordinates(item.XValue, item.YValue3));
            line2.Color = color;

            var c1 = Chart.Add.Circle(item.XValue, item.YValue2, .1);
            var c2 = Chart.Add.Circle(item.XValue, item.YValue3, .1);

            c1.LineColor = color;
            c1.FillStyle.Color = color;
            c2.LineColor = color;
            c2.FillStyle.Color = color;

        }

        Chart.Axes.Bottom.Min = minX;
        Chart.Axes.Bottom.Max = maxX;

        // create a custom tick generator using your custom label formatter
        var myTickGenerator = new NumericFixedInterval
        {
            LabelFormatter = CustomFormatter
        };

        Chart.Axes.Bottom.TickGenerator = myTickGenerator;

        var limits = Chart.Axes.GetLimits();

        Chart.Axes.SetLimits(minX, maxX, limits.Bottom, limits.Top);

        base.CreateChart();
        return;

        // create a static function containing the string formatting logic
        string CustomFormatter(double position)
        {
            var item = _data.FirstOrDefault(x => Math.Abs(x.XValue - position) < 0.0000000000000001);

            return item is null ? string.Empty : item.Label;
        }
    }

    /// <summary>
    /// Formatting the chart
    /// </summary>
    public override void Formatting()
    {

        base.Formatting();

        Chart.Axes.Bottom.MinorTickStyle.Width = 0;
    }
}
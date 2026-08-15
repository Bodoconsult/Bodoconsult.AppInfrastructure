// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.Globalization;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Charting.Extensions;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace Bodoconsult.Charting;

/// <summary>
/// Creates a stacked column chart with columns adding always to 100%
/// </summary>
/// <typeparam name="T"></typeparam>

public class StackedColumn100Chart<T> : BaseChart<T> where T : IChartItemData
{
    private bool _isDate;

    private ChartStyle _style;

    private readonly Dictionary<double, string> _labels = new();

    private readonly List<double> _existingsTicks = [];

    private readonly List<LegendItem> _legends = [];

    /// <summary>
    /// Creates the chart
    /// </summary>
    public override void CreateChart()
    {

        if (!ChartData.DataSource.Any())
        {
            return;
        }

        _style = ChartData.ChartStyle;
        var count = ChartData.DataSource.Count;


        _isDate = ((ChartItemData)ChartData.DataSource[0]).IsDate;

        var countCol = 0;

        for (var index = 0; index < count; index++)
        {
            var item = ChartData.DataSource[index];
            var d = (ChartItemData)item;

            if (d.YValue1 > 0.0000001) countCol = 1;
            if (d.YValue2 > 0.0000001) countCol = 2;
            if (d.YValue3 > 0.0000001) countCol = 3;
            if (d.YValue4 > 0.0000001) countCol = 4;
            if (d.YValue5 > 0.0000001) countCol = 5;
            if (d.YValue6 > 0.0000001) countCol = 6;
            if (d.YValue7 > 0.0000001) countCol = 7;
            if (d.YValue8 > 0.0000001) countCol = 8;
            if (d.YValue9 > 0.0000001) countCol = 9;
            if (d.YValue10 > 0.0000001) countCol = 10;
        }


        List<Bar> barData = [];
        for (var index = 0; index < count; index++)
        {
            var item = ChartData.DataSource[index];
            var d = (ChartItemData)item;

            PlottBar(barData, d, countCol);
        }

        _existingsTicks.AddRange(_labels.Keys.OrderBy(x => x).ToArray());

        var bars = Chart.Add.Bars(barData);
        //bars.ValueLabelStyle.Bold = true;
        //bars.ValueLabelStyle.FontSize = ChartData.ChartStyle.FontSize;

        var pos = (bars.Bars[countCol + 1].Position - bars.Bars[0].Position) + 5;

        foreach (var bar in bars.Bars)
        {
            bar.Size = pos;
            bar.LineWidth = 0;
            bar.LabelOnTop = false;
            bar.CenterLabel = true;
        }

        // create a custom tick generator using your custom label formatter
        NumericAutomatic myTickGenerator = new()
        {
            LabelFormatter = CustomFormatter,
            //MaxTickCount = 10
        };

        Chart.Axes.Bottom.TickGenerator = myTickGenerator;

        // Add legend 
        PlottLegend(barData, countCol);

        base.CreateChart();
        return;


        // create a static function containing the string formatting logic
        string CustomFormatter(double position)
        {
            var position1 = _existingsTicks.FirstOrDefault(x => x >= position);

            if (position1 < 0.00001)
            {
                position1 = _existingsTicks[^1];
            }

            Debug.Print($"{position.ToString(CultureInfo.InvariantCulture)} {position1.ToString(CultureInfo.InvariantCulture)}");

            return !_labels.TryGetValue(position1, out var value) ? string.Empty : value;
        }
    }

    private void PlottBar(List<Bar> barData, ChartItemData item, int colCount)
    {
        var format = "";
        string label;

        if (string.IsNullOrEmpty(item.Label))
        {
            if (_isDate)
            {
                if (string.IsNullOrEmpty(format))
                {
                    format = string.IsNullOrEmpty(_style.XAxisNumberformat) ? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern : _style.XAxisNumberformat;
                }
                label = DateTime.FromOADate(item.XValue).ToString(format);
            }
            else
            {
                if (string.IsNullOrEmpty(format))
                {
                    format = string.IsNullOrEmpty(_style.XAxisNumberformat) ? "0" : _style.XAxisNumberformat;
                }
                label = Convert.ToDouble(item.XValue).ToString(format);
            }
        }
        else
        {
            label = item.Label;
        }

        _labels.Add(item.XValue, label);

        var total = item.YValue1 + item.YValue2 + item.YValue3 + item.YValue4 + item.YValue5 + item.YValue6 +
                    item.YValue7 + item.YValue8 + item.YValue9 + item.YValue10;

        // Plot value 1
        double valueBase = 0;

        var plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue1 / total,
            FillColor = GetColor(1)
        };

        barData.Add(plot);

        // Plot value 2
        valueBase += item.YValue1 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue2 / total,
            FillColor = GetColor(2)
        };

        barData.Add(plot);

        if (colCount < 3)
        {
            return;
        }

        // Plot value 3
        valueBase += item.YValue2 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue3 / total,
            FillColor = GetColor(3)
        };

        barData.Add(plot);

        if (colCount < 4)
        {
            return;
        }

        // Plot value 4
        valueBase += item.YValue3 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue4 / total,
            FillColor = GetColor(4)
        };

        barData.Add(plot);

        if (colCount < 5)
        {
            return;
        }

        // Plot value 5
        valueBase += item.YValue4 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue5 / total,
            FillColor = GetColor(5)
        };

        barData.Add(plot);

        if (colCount < 6)
        {
            return;
        }

        // Plot value 6
        valueBase += item.YValue5 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue6 / total,
            FillColor = GetColor(6)
        };

        barData.Add(plot);

        if (colCount < 7)
        {
            return;
        }

        // Plot value 7
        valueBase += item.YValue6 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue7 / total,
            FillColor = GetColor(7)
        };

        barData.Add(plot);

        if (colCount < 8)
        {
            return;
        }

        // Plot value 8
        valueBase += item.YValue7 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue8 / total,
            FillColor = GetColor(8)
        };

        barData.Add(plot);

        if (colCount < 9)
        {
            return;
        }

        // Plot value 9
        valueBase += item.YValue8 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue9 / total,
            FillColor = GetColor(9)
        };

        barData.Add(plot);

        if (colCount < 10)
        {
            return;
        }

        // Plot value 10
        valueBase += item.YValue9 / total;

        plot = new Bar
        {
            Position = item.XValue,
            ValueBase = valueBase,
            Value = valueBase + item.YValue10 / total,
            FillColor = GetColor(10)
        };

        barData.Add(plot);
    }

    private void PlottLegend(List<Bar> barData, int colCount)
    {
        // Plot value 1
        var plot = barData[0];

        LegendItem item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[0]
        };

        _legends.Add(item1);

        if (colCount < 2)
        {
            return;
        }

        // Plot value 2
        plot = barData[1];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[1]
        };

        _legends.Add(item1);

        if (colCount < 3)
        {
            return;
        }

        // Plot value 3
        plot = barData[2];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[2]
        };

        _legends.Add(item1);

        if (colCount < 4)
        {
            return;
        }

        // Plot value 4
        plot = barData[3];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[3]
        };

        _legends.Add(item1);

        if (colCount < 5)
        {
            return;
        }

        // Plot value 5
        plot = barData[4];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[4]
        };

        _legends.Add(item1);

        if (colCount < 6)
        {
            return;
        }

        // Plot value 6
        plot = barData[5];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[5]
        };

        _legends.Add(item1);

        if (colCount < 7)
        {
            return;
        }

        // Plot value 7
        plot = barData[6];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[6]
        };

        _legends.Add(item1);

        if (colCount < 8)
        {
            return;
        }

        // Plot value 8
        plot = barData[7];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[7]
        };

        _legends.Add(item1);

        if (colCount < 9)
        {
            return;
        }

        // Plot value 9
        plot = barData[8];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[8]
        };

        _legends.Add(item1);

        if (colCount < 10)
        {
            return;
        }

        // Plot value 10
        plot = barData[9];

        item1 = new()
        {
            LineColor = plot.FillColor,
            MarkerFillColor = plot.FillColor,
            MarkerLineColor = plot.FillColor,
            MarkerShape = MarkerShape.Cross,
            LineWidth = 2,
            LabelText = ChartData.LabelsForSeries[9]
        };

        _legends.Add(item1);
    }

    /// <summary>
    /// Formatting the chart
    /// </summary>
    public override void Formatting()
    {
        base.Formatting();

        var style = ChartData.ChartStyle;

        Chart.Legend.IsVisible = true;
        Chart.ShowLegend(_legends);

        var legend = Chart.Legend;
        legend.IsVisible = true;
        legend.BackgroundColor = Colors.WhiteSmoke;
        legend.OutlineColor = Colors.Transparent;
        legend.FontSize = style.FontSize * style.LegendFontSizeDelta;
        legend.FontColor = style.FontColor.ToScottPlotColor();
        legend.Orientation = Orientation.Horizontal;

        Chart.Grid.IsVisible = true;
        Chart.Grid.IsBeneathPlottables = false;

        Chart.Grid.YAxisStyle.MajorLineStyle.IsVisible = false;

        Chart.Grid.YAxisStyle.MajorLineStyle.IsVisible = true;
        Chart.Grid.YAxisStyle.MajorLineStyle.Width = 1;
        Chart.Grid.YAxisStyle.MajorLineStyle.Color = style.GridLineColor.ToScottPlotColor();
        Chart.Grid.YAxisStyle.MajorLineStyle.Pattern = LinePattern.Dashed;
    }
}
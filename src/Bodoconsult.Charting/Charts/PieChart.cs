// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Charting.Base.Interfaces;
using ScottPlot;

namespace Bodoconsult.Charting;

/// <summary>
/// Creates a pie chart
/// </summary>
/// <typeparam name="T">input data type</typeparam>

public class PieChart<T> : BaseChart<T> where T : PieChartItemData
{
    /// <summary>
    /// Creates the chart
    /// </summary>
    public override void CreateChart()
    {

        //var style = ChartData.ChartStyle;

        List<PieSlice> slices = [];

        var total = 0.0;
        for (var index = 0; index < ChartData.DataSource.Count; index++)
        {
            var data = (PieChartItemData)ChartData.DataSource[index];
            total += data.YValue;
        }

        int i = 0;
        for (var index = 0; index < ChartData.DataSource.Count; index++)
        {
            var data = (PieChartItemData)ChartData.DataSource[index];

            var slice = new PieSlice
            {
                Value = Convert.ToDouble(data.YValue), 
                Label = $"{data.XValue}\r\n{data.YValue / total * 100:0.0}%", 
                FillColor = GetColor(i),
            };
            slices.Add(slice);

            i++;
        }

        var pie = Chart.Add.Pie(slices);
        pie.SliceLabelDistance = 1.4;

        //Chart.ShowLegend();

        ////var x = pie.donutSize;
        ////Chart.Legend(enableLegend: true, style.FontName, style.FontSize, fontColor: style.FontColor, 
        ////    frameColor: Color.Transparent, location: legendLocation.lowerRight, backColor: Color.Transparent);

        //var legend = Chart.Legend();
        //legend.IsVisible = true;
        //legend.Location = Alignment.LowerRight;
        //legend.FillColor = Color.Transparent;
        //legend.OutlineColor = Color.Transparent;
        //legend.FontSize = style.FontSize;
        //legend.FontColor = style.FontColor;
        //legend.FontBold = false;
        //legend.FontName = style.FontName;


        //Chart.HideGrid();
        //Chart.Frame(false);
        //Chart.Ticks(false, false);

        //Chart.Layout(yScaleWidth: 80, titleHeight: 50, xLabelHeight: 20, y2LabelWidth: 20);

        base.CreateChart();
    }

    /// <summary>
    /// Base formatting for the chart
    /// </summary>
    public override void Formatting()
    {
        base.Formatting();

        Chart.HideGrid();
        Chart.Axes.Frame(false);
        Chart.Axes.Bottom.IsVisible = false;
        Chart.Axes.Left.IsVisible = false;

        Chart.Axes.Margins(bottom: 0, top: 0, left: 0, right: 0);
    }
}
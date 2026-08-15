// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Data;
using System.Linq;
using Bodoconsult.Pdf.Extensions;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// Simple chart creation with MigraDoc
/// </summary>
public class PdfChart
{
    private readonly Chart _chart = new();


    /// <summary>
    /// Default ctor
    /// </summary>
    public PdfChart()
    {
        HasDataLabel = false;
        Width = 13;
        Height = 8.5;
        Left = 0;
    }

    /// <summary>
    /// Left posiiton in cm
    /// </summary>
    public double Left { get; set; }

    /// <summary>
    /// Width in cm
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Height in cm
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Chart type
    /// </summary>
    public ChartType TypeOfChart { get; set; }

    /// <summary>
    /// Has a data lable
    /// </summary>
    public bool HasDataLabel { get; set; }

    /// <summary>
    /// XAxis lable
    /// </summary>
    public string XAxisLabel { get; set; }

    /// <summary>
    /// YAxis lable
    /// </summary>
    public string YAxisLabel { get; set; }

    /// <summary>
    /// Length of the legend
    /// </summary>
    public int LegendLength { get; set; }

    private int _xDimension;

    /// <summary>
    /// Chart title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Draw the chart
    /// </summary>
    /// <returns>Chart</returns>
    public Chart Draw()
    {
        if (_chart.SeriesCollection.Count==0)
        {
            throw new ArgumentException("At least the data of one series are required.");
        }

        _chart.Width = Unit.FromCentimeter(Width);
        _chart.Height = Unit.FromCentimeter(Height);

        _chart.XAxis.MajorTickMark = TickMarkType.Outside;
        if (!string.IsNullOrEmpty(XAxisLabel)) _chart.XAxis.Title.Caption = XAxisLabel;

        _chart.YAxis.MajorTickMark = TickMarkType.Outside;
        _chart.YAxis.HasMajorGridlines = true;

        _chart.PlotArea.LineFormat.Color = Colors.SteelBlue;
        _chart.PlotArea.LineFormat.Width = 1;
        _chart.PlotArea.BottomPadding = Unit.FromCentimeter(0.2);
        _chart.PlotArea.LeftPadding = Unit.FromCentimeter(0.2);

        _chart.Format.Font.Size = 8;
        _chart.HeaderArea.Style = "ChartTitle";
        _chart.HeaderArea.AddParagraph(Title);
        _chart.FillFormat.Color = Colors.LightSteelBlue;
        _chart.PlotArea.FillFormat.Color = Colors.White;
        _chart.LineFormat.Color = Colors.SteelBlue;

        if (_chart.SeriesCollection.Count > 1)
        {
            if (_chart.SeriesCollection[0] is null || _chart.SeriesCollection[1]==null)
            {
                throw new ArgumentException("Two data series are required.");
            }

            _chart.SeriesCollection[0].MarkerBackgroundColor = Colors.YellowGreen;
            _chart.SeriesCollection[0].MarkerSize = 2;
            _chart.SeriesCollection[0].MarkerStyle = MarkerStyle.Plus;
            _chart.SeriesCollection[0].FillFormat.Color = Colors.YellowGreen;
            _chart.SeriesCollection[0].LineFormat.Color = Colors.YellowGreen;
            _chart.SeriesCollection[0].LineFormat.Width = 2;

            _chart.SeriesCollection[1].FillFormat.Color = Colors.Orange;
            _chart.SeriesCollection[1].MarkerBackgroundColor = Colors.Orange;
            _chart.SeriesCollection[1].MarkerSize = 2;
            _chart.SeriesCollection[1].MarkerStyle = MarkerStyle.Plus;
            _chart.SeriesCollection[1].LineFormat.Color = Colors.Orange;
            _chart.SeriesCollection[1].LineFormat.Width = 2;
        }
        else
        {
            if (_chart.SeriesCollection[0] is null)
            {
                throw new ArgumentException("At least the data of one series are required.");
            }
            _chart.SeriesCollection[0].FillFormat.Color = Colors.Orange;
            _chart.SeriesCollection[0].MarkerBackgroundColor = Colors.Orange;
            _chart.SeriesCollection[0].MarkerSize = 2;
            _chart.SeriesCollection[0].MarkerStyle = MarkerStyle.Plus;
            _chart.SeriesCollection[0].LineFormat.Color = Colors.Orange;
            _chart.SeriesCollection[0].LineFormat.Width = 2;
        }

        if (!string.IsNullOrEmpty(YAxisLabel))
        {
            _chart.LeftArea.Style = "ChartYLabel";
            _chart.LeftArea.AddParagraph(YAxisLabel);
            _chart.LeftArea.LeftPadding = Unit.FromCentimeter(0.2);
        }

        _chart.RightArea.AddLegend();

        return _chart;

    }

    /// <summary>
    /// Add a data series to the chart
    /// </summary>
    /// <param name="data">Data array</param>
    /// <param name="name">Name of the series</param>
    public void AddSeries(double[] data, string name)
    {
        var series = _chart.SeriesCollection.AddSeries();
        series.ChartType = TypeOfChart;
        series.Add(data);
        series.Name = name;
        series.MarkerSize = Unit.FromCentimeter(0.1);
        series.HasDataLabel = HasDataLabel;
    }

    /// <summary>
    /// Add a series taken from a <see cref="DataTable"/>
    /// </summary>
    /// <param name="dataTable">Current data</param>
    /// <param name="columnId">Column number starting with 0</param>
    /// <param name="name">Name of the series</param>
    public void AddSeries(DataTable dataTable, int columnId, string name)
    {

        var x = new double[_xDimension];

        for (var i = 0; i < _xDimension; i++)
        {
            var v = dataTable.Rows[i][columnId].ToString();

            if (string.IsNullOrEmpty(v)) continue;
            x[i] = Convert.ToDouble(v);
        }

        //var query = from mycolumn in dataTable.AsEnumerable()
        //            where mycolumn.Field<decimal>(columnId) != null 
        //            select mycolumn;

        //var x = query.Select(i => i.Field<Double>(columnId)).ToArray(); 

        AddSeries(x, name);

    }

    /// <summary>
    /// Add x-axis values for series
    /// </summary>
    /// <param name="s">Array with names</param>
    public void XSeries(params string[] s)
    {
        _xDimension = s.Length;
        var xseries = _chart.XValues.AddXSeries();
        xseries.Add(s);
    }

    /// <summary>
    /// Add x-axis values for series
    /// </summary>
    /// <param name="dataTable"><see cref="DataTable"/>> with names</param>
    /// <param name="columnId">Column number starting with 0</param>
    public void XSeries(DataTable dataTable, int columnId)
    {
        if (LegendLength > 15) LegendLength = 15;

        var rows = dataTable.EnumerateRows().Where(r => r.ItemArray[columnId]?.ToString() != "Gesamt").ToArray();

        _xDimension = rows.Length;

        var x = new string[_xDimension];

        for (var index = 0; index < _xDimension; index++)
        {
            var row = rows[index];

            var s = row[columnId].ToString();

            if (string.IsNullOrEmpty(s))
            {
                continue;
            }

            x[index] = s[..LegendLength];
        }

        //var query = from mycolumn in dataTable.AsEnumerable()
        //            where mycolumn.Field<string>(columnId) != "Gesamt"
        //            select mycolumn;

        //var x = query.Select(i => i.Field<string>(columnId).Substring(0, LegendLength)).ToArray();

        //_xDimension = x.Length;

        var xseries = _chart.XValues.AddXSeries();
        xseries.Add(x);
    }
}
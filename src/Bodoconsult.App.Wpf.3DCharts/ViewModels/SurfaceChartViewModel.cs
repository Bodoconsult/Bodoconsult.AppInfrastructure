// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Bodoconsult.App.Wpf.Charting.Models;
using Bodoconsult.App.Wpf.Models;
using Bodoconsult.App.Wpf.ViewModels;

namespace Bodoconsult.App.Wpf.Charting.ViewModels;

/// <summary>
/// MVVM view model
/// </summary>
public class SurfaceChartViewModel : ViewModelBase
{

    private Base3DChart? _chart;

    //private readonly IViewManager _viewManager;

    /// <summary>
    /// 
    /// </summary>
    public SurfaceChartViewModel()
    {

    }


    //private ICommand _sizeChangedCommand;

    ///// <summary>
    ///// Size of the window has changed
    ///// </summary>
    //public ICommand SizeChangedCommand
    //{
    //    get
    //    {
    //        return _sizeChangedCommand ??
    //               (_sizeChangedCommand = new DelegateCommand(SizeChanged, () => true));
    //    }
    //}

    private void SizeChanged()
    {
        AddChart();
    }

    private void SizeChanged(bool notUsed)
    {
        AddChart();
    }

    /// <summary>
    /// Viisbility changed
    /// </summary>
    public Visibility Changed { get; set; }

    /// <summary>
    /// The image source for the _chart
    /// </summary>
    public ImageSource ChartImageSource { get; set; }

    /// <summary>
    /// The canvas the surface _chart is drawn in
    /// </summary>
    public Canvas ChartCanvas { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public void AddChart()
    {

        if (_chart == null)
        {
            return;
        }

        _chart.DrawToCanvas(ChartCanvas);

        ChartCanvas.UpdateLayout();

        //_eventAggregator.GetEvent<UserInterfaceChangedEvent>().Publish(true);
    }

    /// <summary>
    /// Load data to the viemodel and draw _chart. Calls <see cref="AddChart">AddChart</see> internally./>
    /// Used for first init of the _chart
    /// </summary>
    /// <param name="data"></param>
    public void DataChanged(Base3DChart data)
    {
        _chart = data;
        _chart.LoadValues();
        _chart.CalculateValues();

        AddChart();
    }

    /// <summary>
    /// Print _chart
    /// </summary>
    public void PrintChart(string message)
    {
        //WpfUtility.PrintVisual(ChartCanvas, message);
    }

    /// <summary>
    /// Export _chart to a file
    /// </summary>
    /// <param name="data"></param>
    public void ExportChart(VisualExportData data)
    {
        _chart?.ExportChartAsPng(data.Path, data.Width, data.Height);
    }
}
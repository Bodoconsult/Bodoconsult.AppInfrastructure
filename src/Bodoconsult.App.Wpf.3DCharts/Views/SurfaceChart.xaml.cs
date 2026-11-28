using System.Windows;
using Bodoconsult.App.Wpf.Charting.ViewModels;

namespace Bodoconsult.App.Wpf.Charting.Views;

/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:Bodoconsult.Wpf.Charts3D"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:Bodoconsult.Wpf.Charts3D;assembly=Bodoconsult.Wpf.Charts3D"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
/// </summary>
/// 
/// <summary>
/// Interaction logic for SurfaceChart.xaml
/// </summary>
public partial class SurfaceChart
{

    private readonly SurfaceChartViewModel _model;

    /// <summary>
    /// default ctor
    /// </summary>
    /// <param name="model"></param>
    public SurfaceChart(SurfaceChartViewModel model)
    {
        _model = model;
        DataContext = model;
        InitializeComponent();

        //ChartCanvas.Width = ChartGrid.ActualWidth;
        //ChartCanvas.Height = ChartGrid.ActualHeight;
        model.ChartCanvas = ChartCanvas;      
            
    }

    private void ChartGridSizeChanged(object sender, SizeChangedEventArgs e)
    {
        _model.AddChart();
    }

}
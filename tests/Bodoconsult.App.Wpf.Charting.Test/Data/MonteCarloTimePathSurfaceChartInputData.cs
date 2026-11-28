namespace Bodoconsult.App.Wpf.Charting.Test.Data;

public class McpsSurfaceChartInputData
{

    public McpsSurfaceChartInputData()
    {
        Xspacing = 0.001;
        Yspacing = 0.001;
        StartValue = 1;
        XPositive = true;
        YPositive = false;
        Xtick = double.NaN;
        Ytick = double.NaN;
    }


    #region Input data

    public double StartValue { get; set; }

    public double ConstantValue { get; set; }

    public bool XPositive { get; set; }

    public double Xmin { get; set; }

    public double Xmax { get; set; }

    public double Xspacing { get; set; }

    public bool YPositive { get; set; }

    public double Xtick { get; set; }

    public double Ymin { get; set; }

    public double Ymax { get; set; }

    public double Yspacing { get; set; }

    public double Ytick { get; set; }

    public int TimePeriods { get; set; }

    /// <summary>
    /// Numberformat to use for all axis labels
    /// </summary>
    public string NumberFormat { get; set; }

    /// <summary>
    /// Numberformat to use for z-axis labels only: default is value of <see cref="NumberFormat"/>
    /// </summary>
    public string NumberFormatZ { get; set; }


    #endregion

}
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Bodoconsult.App.Wpf.Charting.Helpers;
using Bodoconsult.App.Wpf.Charting.Models;
using Bodoconsult.App.Wpf.Charting.Styling;

namespace Bodoconsult.App.Wpf.Charting.Services;

/// <summary>
/// Creates a surface chart in a WPF canvas
/// </summary>
public class DrawSurfaceChart
{

    internal enum CornersOfChartArea
    {
        XmaxYminZmin,
        XminYminZmin,
        XminYmaxZmin,
        XminYmaxZmax,
        XminYminZmax,
        XmaxYminZmax,
        XmaxYmaxZmax,
        XmaxYmaxZmin
    }

    private SolidColorBrush[]? _colormapBrushes;

    private Line _gridline = new Line();
    private Line _axisLine = new Line();
    private Matrix3D _transformationMatrix;

    /// <summary>
    /// default ctor
    /// </summary>
    public DrawSurfaceChart()
    {
        NumberInterp = 2;
        ChartCanvas = new Canvas();
    }



    /// <summary>
    /// Brush used for color map
    /// </summary>
    public ColormapBrush Colormap { get; set; } = new ColormapBrush();

    /// <summary>
    /// Type of the surface chart
    /// </summary>
    public SurfaceChartTypeEnum SurfaceChartType { get; set; } = SurfaceChartTypeEnum.Mesh;

    /// <summary>
    /// Show color map: true/false
    /// </summary>
    public bool IsColormap { get; set; } = true;

    /// <summary>
    /// Line hidden?
    /// </summary>
    public bool IsHiddenLine { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsInterp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int NumberInterp { get; set; }

    /// <summary>
    /// Chart types available
    /// </summary>
    public enum SurfaceChartTypeEnum
    {

#pragma warning disable 1591
        Surface = 1,
        Mesh = 2,
        MeshZ = 3,
        Waterfall = 4
#pragma warning restore 1591
    }

    private ChartStyle _chartStyle;

    /// <summary>
    /// The current canvas to draw in
    /// </summary>
    public Canvas ChartCanvas;

    private Point3D[,] _pointArray;

    /// <summary>
    /// Create the surface chart
    /// </summary>
    /// <param name="cs"></param>
    public void CreateChart(ChartStyle cs)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {

            _chartStyle = cs;

            AdjustChartDataToRequestedSize();

            _pointArray = (Point3D[,])_chartStyle.DataSeries.PointArray.Clone();
            ChartCanvas.Children.Clear();

            if (double.IsNaN(ChartCanvas.Width))
            {
                ChartCanvas.Width = cs.Width;
            }

            if (double.IsNaN(ChartCanvas.Height))
            {
                ChartCanvas.Height = cs.Height;
            }

            if (!string.IsNullOrEmpty(cs.NumberFormatX) && string.IsNullOrEmpty(cs.NumberFormatZ))
            {
                cs.NumberFormatZ = cs.NumberFormatX;
            }


            //if (double.IsNaN(Width) || Width < 1) Width = Canvas.Width;
            //if (double.IsNaN(Width) || Width < 1) Width = 1024;

            //if (double.IsNaN(Height) || Height < 1) Height = Canvas.Height;
            //if (double.IsNaN(Height) || Height < 1) Height = 768;

            //if (Canvas.Width < 1) 
            //if (Canvas.Height < 1)


            //Canvas.Width = Width;
            //Canvas.Height = Height;

            ChartCanvas.Background = _chartStyle.Background;

            if (!Equals(_chartStyle.Background, Brushes.Transparent))
            {
                var rect = new Rectangle
                {
                    Width = _chartStyle.Width,
                    Height = _chartStyle.Height,
                    Fill = _chartStyle.Background,
                    Stroke = _chartStyle.Background,
                };

                ChartCanvas.Children.Add(rect);

                Canvas.SetLeft(rect, 0);
                Canvas.SetTop(rect, 0);
            }

            // Get the transformation matrix for 3D to 2D
            _transformationMatrix = ChartHelper.AzimuthElevation(_chartStyle.Elevation, _chartStyle.Azimuth);

            AddChartArea();

            AddTicks();
            AddGridlines();
            AddAxes();
            AddLabels();

            switch (SurfaceChartType)
            {
                case SurfaceChartTypeEnum.Mesh:
                    AddMesh();
                    break;
                case SurfaceChartTypeEnum.MeshZ:
                    AddMeshZ();
                    break;
                case SurfaceChartTypeEnum.Waterfall:
                    AddWaterfall();
                    break;
                case SurfaceChartTypeEnum.Surface:
                    AddSurface();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
    }



    private void AdjustChartDataToRequestedSize()
    {

        var ratio = (float)(1.0 + (_chartStyle.Width / 1024 - 1) * 0.92);

        _chartStyle.LabelFontSize *= ratio;
        _chartStyle.TitleFontSize *= ratio;
        _chartStyle.TickFontSize *= ratio;
        _chartStyle.CopyrightFontSize *= ratio;
        _chartStyle.ChartBorderWidth = (int)(_chartStyle.ChartBorderWidth * ratio);
        _chartStyle.GridlineThickness *= ratio;
        _chartStyle.AxisThickness *= ratio;

    }

    private void AddChartArea()
    {
        var marginX = 0.04;

        // Add Title
        var tb = new TextBlock
        {
            Text = _chartStyle.Title,
            Foreground = _chartStyle.TitleColor,
            FontSize = _chartStyle.TitleFontSize,
            FontFamily = _chartStyle.TitleFont,
            FontWeight = FontWeights.Bold
        };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var titleSize = tb.DesiredSize;

        var height = _chartStyle.Height / 28;

        if (tb.Text != "No Title")
        {

            if (_chartStyle.TitleShadow)
            {
                var shadow = new TextBlock
                {
                    Text = _chartStyle.Title,
                    Foreground = new SolidColorBrush(Colors.Gray),
                    FontSize = _chartStyle.TitleFontSize,
                    FontFamily = _chartStyle.TitleFont,
                    FontWeight = FontWeights.Bold
                };
                shadow.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                ChartCanvas.Children.Add(shadow);
                Canvas.SetLeft(shadow, _chartStyle.Width / 2 - titleSize.Width / 2 + 0.1 * _chartStyle.TitleFontSize);
                Canvas.SetTop(shadow, height + 0.1 * _chartStyle.TitleFontSize);
            }


            ChartCanvas.Children.Add(tb);
            Canvas.SetLeft(tb, _chartStyle.Width / 2 - titleSize.Width / 2);
            Canvas.SetTop(tb, height);
        }

        var top = height + titleSize.Height + 10;

        var rect = new Rectangle
        {
            Width = _chartStyle.Width * (1 - 2 * marginX),
            Height = _chartStyle.Height - top - 0.025 * _chartStyle.Height,
            Fill = _chartStyle.ChartAreaBackground,
            Stroke = _chartStyle.ChartAreaBackground
        };

        ChartCanvas.Children.Add(rect);

        Canvas.SetLeft(rect, (_chartStyle.Width  - rect.Width) / 2);
        Canvas.SetTop(rect, top);

        // Copyright
        if (string.IsNullOrEmpty(_chartStyle.Copyright)) return;

        var copyright = new TextBlock
        {
            Text = _chartStyle.Copyright,
            Foreground = _chartStyle.TitleColor,
            FontSize = _chartStyle.CopyrightFontSize,
            FontFamily = _chartStyle.CopyrightFont
        };
        copyright.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var copyrightSize = copyright.DesiredSize;

        ChartCanvas.Children.Add(copyright);
        Canvas.SetLeft(copyright, _chartStyle.Width / 2 + rect.Width / 2 - copyrightSize.Width);
        Canvas.SetTop(copyright, top + rect.Height - copyrightSize.Height);
    }

    /// <summary>
    /// Add a color bar to chart
    /// </summary>
    /// <param name="zmin"></param>
    /// <param name="zmax"></param>
    public void AddColorBar(double zmin, double zmax)
    {
        var tb = new TextBlock
        {
            Text = "A",
            FontFamily = _chartStyle.TickFont,
            FontSize = _chartStyle.TickFontSize
        };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var tickSize = tb.DesiredSize;

        var x = 6 * _chartStyle.Width / 7;
        var y = ChartCanvas.Height / 10;
        var width = _chartStyle.Width / 25;
        var height = 8 * _chartStyle.Height / 10;
        var pts = new Point3D[64];
        var dz = (zmax - zmin) / 63;

        // Create the color bar:
        for (var i = 0; i < 64; i++)
        {
            pts[i] = new Point3D(x, y, zmin + i * dz);
        }
        for (var i = 0; i < 63; i++)
        {
            var brush = GetBrush(pts[i].Z, zmin, zmax);
            var y1 = y + height - (pts[i].Z - zmin) * height / (zmax - zmin);
            var y2 = y + height - (pts[i + 1].Z - zmin) * height / (zmax - zmin);
            var plg = new Polygon();
            plg.Points.Add(new Point(x, y2));
            plg.Points.Add(new Point(x + width, y2));
            plg.Points.Add(new Point(x + width, y1));
            plg.Points.Add(new Point(x, y1));
            plg.Fill = brush;
            plg.Stroke = brush;
            ChartCanvas.Children.Add(plg);
        }
        var rect = new Rectangle
        {
            Width = width + 2,
            Height = height + 2,
            Stroke = Brushes.Black
        };
        Canvas.SetLeft(rect, x - 1);
        Canvas.SetTop(rect, y - 1);
        ChartCanvas.Children.Add(rect);

        // Add ticks and labels to the color bar:
        var tickLength = 0.15 * width;
        for (var z = zmin; z <= zmax; z += (zmax - zmin) / 6)
        {
            var yy = y + height - (z - zmin) * height / (zmax - zmin);
            AddTickLine(new Point(x, yy), new Point(x + tickLength, yy));
            AddTickLine(new Point(x + width, yy), new Point(x + width - tickLength, yy));
            tb = new TextBlock
            {
                Text = Math.Round(z, 2).ToString(CultureInfo.InvariantCulture),
                FontFamily = _chartStyle.TickFont,
                FontSize = _chartStyle.TickFontSize
            };
            ChartCanvas.Children.Add(tb);
            Canvas.SetLeft(tb, x + width + 5);
            Canvas.SetTop(tb, yy - tickSize.Height / 2);
        }
    }

    /// <summary>
    /// Add a tick line to 
    /// the axis
    /// </summary>
    /// <param name="pt1"></param>
    /// <param name="pt2"></param>
    public void AddTickLine(Point pt1, Point pt2)
    {
        var line = new Line
        {
            X1 = pt1.X,
            Y1 = pt1.Y,
            X2 = pt2.X,
            Y2 = pt2.Y,
            Stroke = Brushes.Black
        };
        ChartCanvas.Children.Add(line);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="z"></param>
    /// <param name="zmin"></param>
    /// <param name="zmax"></param>
    /// <returns></returns>
    public SolidColorBrush GetBrush(double z, double zmin, double zmax)
    {

        Colormap.Ydivisions = (int)((zmax - zmin) / (Colormap.ColormapLength - 1));
        Colormap.Ymin = zmin;
        Colormap.Ymax = zmax;
        Colormap.Ydivisions = 64;

        if (_colormapBrushes == null)
        {
            _colormapBrushes = Colormap.ColormapBrushes();
        }

        var colorIndex = (int)(((Colormap.ColormapLength - 1) * (z - zmin) + zmax - z) / (zmax - zmin));
        if (colorIndex < 0)
            colorIndex = 0;
        if (colorIndex >= Colormap.ColormapLength)
            colorIndex = Colormap.ColormapLength - 1;
        var brush = _colormapBrushes[colorIndex];
        return brush;
    }

    /// <summary>
    /// Adds a mesh to the chart
    /// </summary>
    public void AddMesh()
    {
        var ds = _chartStyle.DataSeries;

        var pts = _pointArray;
        var zValues = new double[pts.GetLength(0), pts.GetLength(1)];
        var zmin = ds.ZDataMin();
        var zmax = ds.ZDataMax();

        for (var i = 0; i < pts.GetLength(0); i++)
        {
            for (var j = 0; j < pts.GetLength(1); j++)
            {
                zValues[i, j] = pts[i, j].Z;
                pts[i, j] = Normalize3D(pts[i, j]);
            }
        }

        // Draw mesh chart:
        for (var i = 0; i < pts.GetLength(0) - 1; i++)
        {
            int ii;
            if (_chartStyle.Elevation >= 0)
            {
                ii = i;
                if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                {
                    ii = pts.GetLength(0) - 2 - i;
                }
            }
            else
            {
                ii = pts.GetLength(0) - 2 - i;
                if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                {
                    ii = i;
                }
            }
            for (var j = 0; j < pts.GetLength(1) - 1; j++)
            {
                var jj = j;
                if (_chartStyle.Elevation < 0)
                {
                    jj = pts.GetLength(1) - 2 - j;
                }
                var plg = new Polygon();
                plg.Points.Add(new Point(pts[ii, jj].X, pts[ii, jj].Y));
                plg.Points.Add(new Point(pts[ii, jj + 1].X, pts[ii, jj + 1].Y));
                plg.Points.Add(new Point(pts[ii + 1, jj + 1].X, pts[ii + 1, jj + 1].Y));
                plg.Points.Add(new Point(pts[ii + 1, jj].X, pts[ii + 1, jj].Y));

                plg.Stroke = Brushes.Black;
                plg.StrokeThickness = ds.LineThickness;
                plg.Fill = Brushes.White;
                if (!IsHiddenLine)
                {
                    plg.Fill = Brushes.Transparent;
                }
                if (IsColormap)
                {
                    plg.Stroke = GetBrush(zValues[ii, jj], zmin, zmax);
                }
                ChartCanvas.Children.Add(plg);
            }
        }
        if (_chartStyle.IsColorBar && IsColormap)
        {
            AddColorBar(zmin, zmax);
        }
    }

    private void AddMeshZ()
    {
        var ds = _chartStyle.DataSeries;
        Polygon plg;
        var pts = _pointArray;
        var pts1 = new Point3D[pts.GetLength(0), pts.GetLength(1)];
        var zValues = new double[pts.GetLength(0), pts.GetLength(1)];
        var zmin = ds.ZDataMin();
        var zmax = ds.ZDataMax();

        for (var i = 0; i < pts.GetLength(0); i++)
        {
            for (var j = 0; j < pts.GetLength(1); j++)
            {
                zValues[i, j] = pts[i, j].Z;
                pts1[i, j] = new Point3D(pts[i, j].X, pts[i, j].Y, pts[i, j].Z);
                pts[i, j] = Normalize3D(pts[i, j]);
            }
        }

        // Draw mesh using the z-order method:
        for (var i = 0; i < pts.GetLength(0) - 1; i++)
        {
            int ii;
            if (_chartStyle.Elevation >= 0)
            {
                ii = i;
                if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                {
                    ii = pts.GetLength(0) - 2 - i;
                }
            }
            else
            {
                ii = pts.GetLength(0) - 2 - i;
                if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                {
                    ii = i;
                }
            }
            for (var j = 0; j < pts.GetLength(1) - 1; j++)
            {
                var jj = j;
                if (_chartStyle.Elevation < 0)
                {
                    jj = pts.GetLength(1) - 2 - j;
                }
                plg = new Polygon();
                plg.Points.Add(new Point(pts[ii, jj].X, pts[ii, jj].Y));
                plg.Points.Add(new Point(pts[ii, jj + 1].X, pts[ii, jj + 1].Y));
                plg.Points.Add(new Point(pts[ii + 1, jj + 1].X, pts[ii + 1, jj + 1].Y));
                plg.Points.Add(new Point(pts[ii + 1, jj].X, pts[ii + 1, jj].Y));

                plg.Stroke = Brushes.Black;
                plg.StrokeThickness = ds.LineThickness;
                plg.Fill = Brushes.White;
                if (!IsHiddenLine)
                {
                    plg.Fill = Brushes.Transparent;
                }
                if (IsColormap)
                {
                    plg.Stroke = GetBrush(zValues[ii, jj], zmin, zmax);
                }
                ChartCanvas.Children.Add(plg);
            }
        }

        //Draw curtain lines:
        var pta = new Point3D[4];
        for (var i = 0; i < pts1.GetLength(0); i++)
        {
            var jj = pts1.GetLength(0) - 1;
            if (_chartStyle.Elevation >= 0)
            {
                if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth <= 90)
                    jj = 0;
            }
            else if (_chartStyle.Elevation < 0)
            {
                jj = 0;
                if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth <= 90)
                    jj = pts1.GetLength(0) - 1;
            }

            if (i < pts1.GetLength(0) - 1)
            {
                pta[0] = new Point3D(pts1[i, jj].X, pts1[i, jj].Y, pts1[i, jj].Z);
                pta[1] = new Point3D(pts1[i + 1, jj].X, pts1[i + 1, jj].Y, pts1[i + 1, jj].Z);
                pta[2] = new Point3D(pts1[i + 1, jj].X, pts1[i + 1, jj].Y, _chartStyle.Zmin);
                pta[3] = new Point3D(pts1[i, jj].X, pts1[i, jj].Y, _chartStyle.Zmin);
                for (var k = 0; k < 4; k++)
                {
                    pta[k] = Normalize3D(pta[k]);
                }
                plg = new Polygon
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = ds.LineThickness,
                    Fill = Brushes.White
                };
                plg.Points.Add(new Point(pta[0].X, pta[0].Y));
                plg.Points.Add(new Point(pta[1].X, pta[1].Y));
                plg.Points.Add(new Point(pta[2].X, pta[2].Y));
                plg.Points.Add(new Point(pta[3].X, pta[3].Y));
                if (!IsHiddenLine)
                {
                    plg.Fill = Brushes.Transparent;
                }
                if (IsColormap)
                {
                    plg.Stroke = GetBrush(pts1[i, jj].Z, zmin, zmax);
                }
                ChartCanvas.Children.Add(plg);
            }
        }

        for (var j = 0; j < pts1.GetLength(1); j++)
        {
            var ii = 0;
            if (_chartStyle.Elevation >= 0)
            {
                if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth <= 180)
                {
                    ii = pts1.GetLength(1) - 1;
                }
            }
            else if (_chartStyle.Elevation < 0)
            {
                if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth <= 0)
                    ii = pts1.GetLength(1) - 1;
            }
            if (j < pts1.GetLength(1) - 1)
            {
                pta[0] = new Point3D(pts1[ii, j].X, pts1[ii, j].Y, pts1[ii, j].Z);
                pta[1] = new Point3D(pts1[ii, j + 1].X, pts1[ii, j + 1].Y, pts1[ii, j + 1].Z);
                pta[2] = new Point3D(pts1[ii, j + 1].X, pts1[ii, j + 1].Y, _chartStyle.Zmin);
                pta[3] = new Point3D(pts1[ii, j].X, pts1[ii, j].Y, _chartStyle.Zmin);
                for (var k = 0; k < 4; k++)
                    pta[k] = Normalize3D(pta[k]);
                plg = new Polygon
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = ds.LineThickness,
                    Fill = Brushes.White
                };
                plg.Points.Add(new Point(pta[0].X, pta[0].Y));
                plg.Points.Add(new Point(pta[1].X, pta[1].Y));
                plg.Points.Add(new Point(pta[2].X, pta[2].Y));
                plg.Points.Add(new Point(pta[3].X, pta[3].Y));
                if (!IsHiddenLine)
                {
                    plg.Fill = Brushes.Transparent;
                }
                if (IsColormap)
                {
                    plg.Stroke = GetBrush(pts1[ii, j].Z, zmin, zmax);
                }
                ChartCanvas.Children.Add(plg);
            }
        }

        if (_chartStyle.IsColorBar && IsColormap)
        {
            AddColorBar(zmin, zmax);
        }
    }

    private void AddWaterfall()
    {
        var ds = _chartStyle.DataSeries;
        var pts = _pointArray;
        var pt3 = new Point3D[pts.GetLength(0) + 2];
        var zValues = new double[pts.Length];
        var pta = new Point[pts.GetLength(0) + 2];
        var zmin = ds.ZDataMin();
        var zmax = ds.ZDataMax();

        for (var j = 0; j < pts.GetLength(1); j++)
        {
            var jj = j;
            if (_chartStyle.Elevation >= 0)
            {
                if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 90)
                {
                    jj = pts.GetLength(1) - 1 - j;
                }
            }
            else if (_chartStyle.Elevation < 0)
            {
                jj = pts.GetLength(1) - 1 - j;
                if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 90)
                    jj = j;
            }
            for (var i = 0; i < pts.GetLength(0); i++)
            {
                pt3[i + 1] = pts[i, jj];

                if (i == 0)
                {
                    pt3[0] = new Point3D(pt3[i + 1].X, pt3[i + 1].Y, _chartStyle.Zmin);
                }
                if (i == pts.GetLength(0) - 1)
                {
                    pt3[pts.GetLength(0) + 1] = new Point3D(pt3[i + 1].X, pt3[i + 1].Y, _chartStyle.Zmin);
                }
            }
            var plg = new Polygon();
            for (var i = 0; i < pt3.Length; i++)
            {
                zValues[i] = pt3[i].Z;
                pt3[i] = Normalize3D(pt3[i]);
                pta[i] = new Point(pt3[i].X, pt3[i].Y);
                plg.Points.Add(new Point(pt3[i].X, pt3[i].Y));
            }
            plg.Stroke = Brushes.Transparent;
            plg.StrokeThickness = ds.LineThickness;
            plg.Fill = Brushes.White;
            ChartCanvas.Children.Add(plg);

            for (var i = 1; i < pt3.Length; i++)
            {
                var line = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = ds.LineThickness
                };
                if (IsColormap)
                {
                    line.Stroke = GetBrush(i < pt3.Length - 1 ? zValues[i] : zValues[i - 1], zmin, zmax);
                }
                line.X1 = pta[i - 1].X;
                line.Y1 = pta[i - 1].Y;
                line.X2 = pta[i].X;
                line.Y2 = pta[i].Y;
                ChartCanvas.Children.Add(line);
            }
        }

        if (_chartStyle.IsColorBar && IsColormap)
        {
            AddColorBar(zmin, zmax);
        }
    }

    /// <summary>
    /// Adds the surface to the chart
    /// </summary>
    public void AddSurface()
    {
        var ds = _chartStyle.DataSeries;

        Polygon plg;



        var pts = (Point3D[,])_pointArray.Clone();
        var length0 = pts.GetLength(0);
        var length1 = pts.GetLength(1);

        var pts1 = new Point3D[length0, length1];
        //double[,] zValues = new double[pts.GetLength(0), pts.GetLength(1)];
        var zmin = ds.ZDataMin();
        var zmax = ds.ZDataMax();



        for (var i = 0; i < length0; i++)
        {
            for (var j = 0; j < length1; j++)
            {
                //zValues[i, j] = pts[i, j].Z;
                pts1[i, j] = pts[i, j];
                pts[i, j] = Normalize3D(pts[i, j]);
            }
        }

        // Draw surface chart:
        if (!IsInterp)
        {
            for (var i = 0; i < length0 - 1; i++)
            {
                int ii;
                if (_chartStyle.Elevation >= 0)
                {
                    ii = i;
                    if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                    {
                        ii = length0 - 2 - i;
                    }
                }
                else
                {
                    ii = length0 - 2 - i;
                    if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                    {
                        ii = i;
                    }
                }
                for (var j = 0; j < length1 - 1; j++)
                {
                    var jj = j;
                    if (_chartStyle.Elevation < 0)
                    {
                        jj = length1 - 2 - j;
                    }
                    plg = new Polygon();
                    plg.Points.Add(new Point(pts[ii, jj].X, pts[ii, jj].Y));
                    plg.Points.Add(new Point(pts[ii, jj + 1].X, pts[ii, jj + 1].Y));
                    plg.Points.Add(new Point(pts[ii + 1, jj + 1].X, pts[ii + 1, jj + 1].Y));
                    plg.Points.Add(new Point(pts[ii + 1, jj].X, pts[ii + 1, jj].Y));

                    plg.StrokeThickness = ds.LineThickness;
                    plg.Stroke = ds.LineColor;
                    plg.Fill = GetBrush(pts1[ii, jj].Z, zmin, zmax);
                    ChartCanvas.Children.Add(plg);
                }
            }
            if (_chartStyle.IsColorBar && IsColormap)
            {
                AddColorBar(zmin, zmax);
            }
        }
        else if (IsInterp)
        {
            for (var i = 0; i < length0 - 1; i++)
            {
                int ii;
                if (_chartStyle.Elevation >= 0)
                {
                    ii = i;
                    if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                    {
                        ii = length0 - 2 - i;
                    }
                }
                else
                {
                    ii = length0 - 2 - i;
                    if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                    {
                        ii = i;
                    }
                }
                for (var j = 0; j < length1 - 1; j++)
                {
                    //var jj = j;
                    //if (_chartStyle.Elevation < 0)
                    //{
                    //    jj = pts.GetLength(1) - 2 - j;
                    //}

                    var points = new Point3D[4];
                    points[0] = pts1[ii, j];
                    points[1] = pts1[ii, j + 1];
                    points[2] = pts1[ii + 1, j + 1];
                    points[3] = pts1[ii + 1, j];
                    Interp(points, zmin, zmax);
                    plg = new Polygon { Stroke = ds.LineColor };
                    plg.Points.Add(new Point(pts[ii, j].X, pts[ii, j].Y));
                    plg.Points.Add(new Point(pts[ii, j + 1].X, pts[ii, j + 1].Y));
                    plg.Points.Add(new Point(pts[ii + 1, j + 1].X, pts[ii + 1, j + 1].Y));
                    plg.Points.Add(new Point(pts[ii + 1, j].X, pts[ii + 1, j].Y));
                    ChartCanvas.Children.Add(plg);
                }
            }
        }
        if (_chartStyle.IsColorBar && IsColormap)
        {
            AddColorBar(zmin, zmax);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pta"></param>
    /// <param name="zmin"></param>
    /// <param name="zmax"></param>
    public void Interp(Point3D[] pta, double zmin, double zmax)
    {
        var npoints = NumberInterp;
        var pts = new Point3D[npoints + 1, npoints + 1];
        var pts1 = new Point3D[npoints + 1, npoints + 1];
        var x0 = pta[0].X;
        var y0 = pta[0].Y;
        var x1 = pta[2].X;
        var y1 = pta[2].Y;
        var dx = (x1 - x0) / npoints;
        var dy = (y1 - y0) / npoints;
        var c00 = pta[0].Z;
        var c10 = pta[3].Z;
        var c11 = pta[2].Z;
        var c01 = pta[1].Z;

        for (var i = 0; i <= npoints; i++)
        {
            var x = x0 + i * dx;
            for (var j = 0; j <= npoints; j++)
            {
                var y = y0 + j * dy;
                var c = (y1 - y) * ((x1 - x) * c00 + (x - x0) * c10) / (x1 - x0) / (y1 - y0) + (y - y0) * ((x1 - x) * c01 + (x - x0) * c11) / (x1 - x0) / (y1 - y0);
                pts[i, j] = new Point3D(x, y, c);
                pts1[i, j] = new Point3D(x, y, c);
                pts[i, j] = Normalize3D(pts[i, j]);
            }
        }

        for (var i = 0; i < npoints; i++)
        {
            for (var j = 0; j < npoints; j++)
            {
                var plg = new Polygon();
                Brush brush = GetBrush(pts1[i, j].Z, zmin, zmax);
                plg.Fill = brush;
                plg.StrokeThickness = 0.1;
                plg.Stroke = brush;
                plg.Points.Add(new Point(pts[i, j].X, pts[i, j].Y));
                plg.Points.Add(new Point(pts[i + 1, j].X, pts[i + 1, j].Y));
                plg.Points.Add(new Point(pts[i + 1, j + 1].X, pts[i + 1, j + 1].Y));
                plg.Points.Add(new Point(pts[i, j + 1].X, pts[i, j + 1].Y));
                ChartCanvas.Children.Add(plg);
            }
        }
    }

    #region Methods:

    /// <summary>
    /// Add the pattern to a gridline
    /// </summary>
    public void AddGridlinePattern()
    {
        _gridline.Stroke = _chartStyle.GridlineColor;
        _gridline.StrokeThickness = _chartStyle.GridlineThickness;

        switch (_chartStyle.GridlinePattern)
        {
            case GridlinePatternEnum.Dash:
                _gridline.StrokeDashArray = [4, 3];
                break;
            case GridlinePatternEnum.Dot:
                _gridline.StrokeDashArray = [1, 2];
                break;
            case GridlinePatternEnum.DashDot:
                _gridline.StrokeDashArray = [4, 2, 1, 2];
                break;
            case GridlinePatternEnum.Solid:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Adds the pattern to a axis
    /// </summary>
    public void AddAxisPattern()
    {
        _axisLine.Stroke = _chartStyle.AxisColor;
        _axisLine.StrokeThickness = _chartStyle.AxisThickness;

        switch (_chartStyle.AxisPattern)
        {
            case AxisPatternEnum.Dash:
                _axisLine.StrokeDashArray = [4, 3];
                break;
            case AxisPatternEnum.Dot:
                _axisLine.StrokeDashArray = [1, 2];
                break;
            case AxisPatternEnum.DashDot:
                _axisLine.StrokeDashArray = [4, 2, 1, 2];
                break;
            case AxisPatternEnum.Solid:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    /// <summary>
    /// Gridline patterns available
    /// </summary>
    public enum GridlinePatternEnum
    {
#pragma warning disable 1591
        Solid = 1,
        Dash = 2,
        Dot = 3,
        DashDot = 4
#pragma warning restore 1591
    }

    /// <summary>
    /// Axis patterns available
    /// </summary>
    public enum AxisPatternEnum
    {
#pragma warning disable 1591
        Solid = 1,
        Dash = 2,
        Dot = 3,
        DashDot = 4
#pragma warning restore 1591
    }


    /// <summary>
    /// Get 2D x-y-coordinates from 3D x-y-z coordinates
    /// </summary>
    /// <param name="pt"></param>
    /// <returns></returns>
    public Point3D Normalize3D(Point3D pt)
    {
        var result = new Point3D();

        // Normalize the point:
        var x1 = (pt.X - _chartStyle.Xmin) / (_chartStyle.Xmax - _chartStyle.Xmin) - 0.5;
        var y1 = (pt.Y - _chartStyle.Ymin) / (_chartStyle.Ymax - _chartStyle.Ymin) - 0.5;
        var z1 = (pt.Z - _chartStyle.Zmin) / (_chartStyle.Zmax - _chartStyle.Zmin) - 0.5;

        // Perform transformation on the point using matrix m:
        result.X = _transformationMatrix.Transform(new Point3D(x1, y1, z1)).X;
        result.Y = _transformationMatrix.Transform(new Point3D(x1, y1, z1)).Y;

        // Coordinate transformation from World to Device system:
        var xShift = 1.05;
        double xScale = 1;
        var yShift = 1.05;
        var yScale = 0.9;
        if (_chartStyle.Title == "No Title")
        {
            yShift = 0.95;
            yScale = 1;
        }
        if (_chartStyle.IsColorBar)
        {
            xShift = 0.95;
            xScale = 0.9;
        }
        result.X = (xShift + xScale * result.X) * _chartStyle.Width / 2 + _chartStyle.Width * 0.03;
        result.Y = (yShift - yScale * result.Y) * _chartStyle.Height / 2 - _chartStyle.Height * 0.03;

        //Debug.Print("{0}/{1}/{2}  ---> {3}/{4}/{5}", pt.X, pt.Y, pt.Z, result.X, result.Y, result.Z);


        return result;
    }

    private Point3D[] GetGridPoints()
    {
        var pta = new Point3D[8];
        pta[0] = new Point3D(_chartStyle.Xmax, _chartStyle.Ymin, _chartStyle.Zmin); // 
        pta[1] = new Point3D(_chartStyle.Xmin, _chartStyle.Ymin, _chartStyle.Zmin);
        pta[2] = new Point3D(_chartStyle.Xmin, _chartStyle.Ymax, _chartStyle.Zmin);
        pta[3] = new Point3D(_chartStyle.Xmin, _chartStyle.Ymax, _chartStyle.Zmax);
        pta[4] = new Point3D(_chartStyle.Xmin, _chartStyle.Ymin, _chartStyle.Zmax);
        pta[5] = new Point3D(_chartStyle.Xmax, _chartStyle.Ymin, _chartStyle.Zmax);
        pta[6] = new Point3D(_chartStyle.Xmax, _chartStyle.Ymax, _chartStyle.Zmax);
        pta[7] = new Point3D(_chartStyle.Xmax, _chartStyle.Ymax, _chartStyle.Zmin);
        return pta;
    }


    private Point3D[] CoordinatesOfChartBox()
    {
        var pta = GetGridPoints();

        var pts = new Point3D[4];
        int[] npts = [0, 1, 2, 3];

        if (_chartStyle.Elevation >= 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90)
                npts = [1, 2, 7, 6];
            else if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 0)
                npts = [0, 1, 2, 3];
            else if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90)
                npts = [7, 0, 1, 4];
            else if (_chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
                npts = [2, 7, 0, 5];
        }
        else if (_chartStyle.Elevation < 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90)
                npts = [1, 0, 7, 6];
            else if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 0)
                npts = [0, 7, 2, 3];
            else if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90)
                npts = [7, 2, 1, 4];
            else if (_chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
                npts = [2, 1, 0, 5];
        }

        for (var i = 0; i < 4; i++)
            pts[i] = pta[npts[i]];
        return pts;
    }

    #endregion

    #region Add Axes:

    private void AddAxes()
    {
        var pts = CoordinatesOfChartBox();
        for (var i = 0; i < pts.Length; i++)
        {
            pts[i] = Normalize3D(pts[i]);
        }
        DrawAxisLine(pts[0], pts[1]);
        DrawAxisLine(pts[1], pts[2]);
        DrawAxisLine(pts[2], pts[3]);
    }

    private void DrawAxisLine(Point3D pt1, Point3D pt2)
    {
        _axisLine = new Line
        {
            Stroke = _chartStyle.AxisColor,
            StrokeThickness = _chartStyle.AxisThickness
        };
        AddAxisPattern();
        _axisLine.X1 = pt1.X;
        _axisLine.Y1 = pt1.Y;
        _axisLine.X2 = pt2.X;
        _axisLine.Y2 = pt2.Y;
        ChartCanvas.Children.Add(_axisLine);
    }

    #endregion

    #region Add Ticks:

    private void AddTicks()
    {
        var pta = new Point3D[2];
        var pts = CoordinatesOfChartBox();

        // Add x ticks:
        var offset = (_chartStyle.Ymax - _chartStyle.Ymin) * 0.05;
        var ticklength = offset;

        if (_chartStyle.Elevation >= 0)
        {
            if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 90)
                ticklength = -offset;
        }
        else if (_chartStyle.Elevation < 0)
        {
            if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || _chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
                ticklength = -(_chartStyle.Ymax - _chartStyle.Ymin) / 30;
        }

        for (var x = _chartStyle.Xmin; x <= _chartStyle.Xmax; x += _chartStyle.XTick)
        {

            pta[0] = new Point3D(x, pts[1].Y + ticklength, pts[1].Z);
            pta[1] = new Point3D(x, pts[1].Y, pts[1].Z);
            for (var i = 0; i < pta.Length; i++)
            {
                pta[i] = Normalize3D(pta[i]);
            }
            AddTickLine(pta[0], pta[1]);
        }

        // Add y ticks:
        offset = (_chartStyle.Xmax - _chartStyle.Xmin) * 0.05;
        ticklength = offset;

        if (_chartStyle.Elevation >= 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                ticklength = -offset;
        }
        else if (_chartStyle.Elevation < 0)
        {
            if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 180)
                ticklength = -offset;
        }

        for (var y = _chartStyle.Ymin; y <= _chartStyle.Ymax; y += _chartStyle.YTick)
        {


            pta[0] = new Point3D(pts[1].X + ticklength, y, pts[1].Z);
            pta[1] = new Point3D(pts[1].X, y, pts[1].Z);
            for (var i = 0; i < pta.Length; i++)
            {
                pta[i] = Normalize3D(pta[i]);
            }
            AddTickLine(pta[0], pta[1]);
        }

        // Add z ticks:
        var xoffset = (_chartStyle.Xmax - _chartStyle.Xmin) * 0.05;
        var yoffset = (_chartStyle.Ymax - _chartStyle.Ymin) * 0.05;

        //xoffset = 0.1 * _chartStyle.XTick;
        //yoffset = 0.1 * _chartStyle.YTick;


        var xticklength = xoffset;
        var yticklength = yoffset;

        if (_chartStyle.Elevation >= 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90)
            {
                xticklength = 0;
                yticklength = yoffset;
            }
            else if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 0)
            {
                xticklength = xoffset;
                yticklength = 0;
            }
            else if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90)
            {
                xticklength = 0;
                yticklength = -yoffset;
            }
            else if (_chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
            {
                xticklength = xoffset;
                yticklength = 0;
            }
        }
        else if (_chartStyle.Elevation < 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90)
            {
                yticklength = 0;
                xticklength = xoffset;
            }
            else if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 0)
            {
                yticklength = -yoffset;
                xticklength = 0;
            }
            else if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90)
            {
                yticklength = 0;
                xticklength = -xoffset;
            }
            else if (_chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
            {
                yticklength = yoffset;
                xticklength = 0;
            }
        }

        for (var z = _chartStyle.Zmin; z <= _chartStyle.Zmax; z += _chartStyle.ZTick)
        {
            pta[0] = new Point3D(pts[2].X, pts[2].Y, z);
            pta[1] = new Point3D(pts[2].X - xticklength, pts[2].Y - yticklength, z);
            for (var i = 0; i < pta.Length; i++)
            {
                pta[i] = Normalize3D(pta[i]);
            }
            AddTickLine(pta[0], pta[1]);
        }
    }

    private void AddTickLine(Point3D pt1, Point3D pt2)
    {
        var line = new Line
        {
            Stroke = _chartStyle.AxisColor,
            X1 = pt1.X,
            Y1 = pt1.Y,
            X2 = pt2.X,
            Y2 = pt2.Y
        };
        ChartCanvas.Children.Add(line);
    }

    #endregion

    #region Add Gridlines:

    private void AddGridlines()
    {

        var pta = new Point3D[4];
        var pts = CoordinatesOfChartBox();

        double yBack;
        if (_chartStyle.Elevation >= 0)
        {
            if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
            {
                yBack = _chartStyle.Ymin;
                pta[0] = new Point3D(pts[1].X, yBack, pts[1].Z);
                pta[1] = new Point3D(pts[2].X, yBack, pts[1].Z);
                pta[2] = new Point3D(pts[2].X, yBack, pts[3].Z);
                pta[3] = new Point3D(pts[0].X, yBack, pts[3].Z);
            }
            else
            {
                yBack = _chartStyle.Ymax;
                pta[0] = new Point3D(pts[1].X, yBack, pts[1].Z);
                pta[1] = new Point3D(pts[0].X, yBack, pts[1].Z);
                pta[2] = new Point3D(pts[0].X, yBack, pts[3].Z);
                pta[3] = new Point3D(pts[2].X, yBack, pts[3].Z);
            }
        }
        if (_chartStyle.Elevation < 0)
        {
            if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
            {
                yBack = _chartStyle.Ymax;
                pta[0] = new Point3D(pts[1].X, yBack, pts[1].Z);
                pta[1] = new Point3D(pts[0].X, yBack, pts[1].Z);
                pta[2] = new Point3D(pts[0].X, yBack, pts[3].Z);
                pta[3] = new Point3D(pts[2].X, yBack, pts[3].Z);
            }
            else
            {
                yBack = _chartStyle.Ymin;
                pta[0] = new Point3D(pts[1].X, yBack, pts[1].Z);
                pta[1] = new Point3D(pts[2].X, yBack, pts[1].Z);
                pta[2] = new Point3D(pts[2].X, yBack, pts[3].Z);
                pta[3] = new Point3D(pts[0].X, yBack, pts[3].Z);
            }
        }
        for (var i = 0; i < pta.Length; i++)
        {
            pta[i] = Normalize3D(pta[i]);
        }

        DrawBackgroundForLines(pta[0], pta[1], pta[2], pta[3]);


        double xBack;


        if (_chartStyle.Elevation >= 0)
        {
            if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
            {
                xBack = _chartStyle.Xmax;
                pta[0] = new Point3D(xBack, pts[1].Y, pts[1].Z);
                pta[1] = new Point3D(xBack, pts[0].Y, pts[1].Z);
                pta[2] = new Point3D(xBack, pts[0].Y, pts[3].Z);
                pta[3] = new Point3D(xBack, pts[2].Y, pts[3].Z);
            }
            else
            {
                xBack = _chartStyle.Xmax;
                pta[0] = new Point3D(xBack, pts[1].Y, pts[1].Z);
                pta[1] = new Point3D(xBack, pts[2].Y, pts[1].Z);
                pta[2] = new Point3D(xBack, pts[2].Y, pts[3].Z);
                pta[3] = new Point3D(xBack, pts[0].Y, pts[3].Z);
            }
        }
        else if (_chartStyle.Elevation < 0)
        {
            if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
            {
                xBack = _chartStyle.Xmin;
                pta[0] = new Point3D(xBack, pts[1].Y, pts[1].Z);
                pta[1] = new Point3D(xBack, pts[2].Y, pts[1].Z);
                pta[2] = new Point3D(xBack, pts[2].Y, pts[3].Z);
                pta[3] = new Point3D(xBack, pts[0].Y, pts[3].Z);
            }
            else
            {
                xBack = _chartStyle.Xmin;
                pta[0] = new Point3D(xBack, pts[1].Y, pts[1].Z);
                pta[1] = new Point3D(xBack, pts[0].Y, pts[1].Z);
                pta[2] = new Point3D(xBack, pts[0].Y, pts[3].Z);
                pta[3] = new Point3D(xBack, pts[2].Y, pts[3].Z);
            }
        }
        for (var i = 0; i < pta.Length; i++)
        {
            pta[i] = Normalize3D(pta[i]);
        }

        DrawBackgroundForLines(pta[0], pta[1], pta[2], pta[3]);

        double zBack;
        if (_chartStyle.Elevation >= 0)
        {
            if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
            {
                zBack = _chartStyle.Zmin;
                pta[0] = new Point3D(pts[2].X, pts[2].Y, zBack);
                pta[1] = new Point3D(pts[2].X, pts[0].Y, zBack);
                pta[2] = new Point3D(pts[0].X, pts[0].Y, zBack);
                pta[3] = new Point3D(pts[1].X, pts[3].Y, zBack);
            }
            else
            {
                zBack = _chartStyle.Zmin;
                pta[0] = new Point3D(pts[2].X, pts[2].Y, zBack);
                pta[1] = new Point3D(pts[0].X, pts[2].Y, zBack);
                pta[2] = new Point3D(pts[0].X, pts[1].Y, zBack);
                pta[3] = new Point3D(pts[3].X, pts[1].Y, zBack);
            }
        }
        if (_chartStyle.Elevation < 0)
        {
            if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
            {
                zBack = _chartStyle.Zmin;
                pta[0] = new Point3D(pts[2].X, pts[2].Y, zBack);
                pta[1] = new Point3D(pts[0].X, pts[2].Y, zBack);
                pta[2] = new Point3D(pts[0].X, pts[0].Y, zBack);
                pta[3] = new Point3D(pts[1].X, pts[0].Y, zBack);
            }
            else
            {
                zBack = _chartStyle.Zmin;
                pta[0] = new Point3D(pts[2].X, pts[2].Y, zBack);
                pta[1] = new Point3D(pts[2].X, pts[0].Y, zBack);
                pta[2] = new Point3D(pts[0].X, pts[0].Y, zBack);
                pta[3] = new Point3D(pts[1].X, pts[3].Y, zBack);
            }
        }
        for (var i = 0; i < pta.Length; i++)
        {
            pta[i] = Normalize3D(pta[i]);
        }

        DrawBackgroundForLines(pta[0], pta[1], pta[2], pta[3]);

        // Draw x gridlines:
        if (_chartStyle.IsXGrid)
        {





            for (var x = _chartStyle.Xmin; x <= _chartStyle.Xmax; x += _chartStyle.XTick)
            {
                //pts = CoordinatesOfChartBox();
                pta[0] = new Point3D(x, pts[1].Y, pts[1].Z);
                if (_chartStyle.Elevation >= 0)
                {
                    if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
                    {
                        pta[1] = new Point3D(x, pts[0].Y, pts[1].Z);
                        pta[2] = new Point3D(x, pts[0].Y, pts[3].Z);
                        pta[3] = new Point3D(x, pts[2].Y, pts[3].Z);
                    }
                    else
                    {
                        pta[1] = new Point3D(x, pts[2].Y, pts[1].Z);
                        pta[2] = new Point3D(x, pts[2].Y, pts[3].Z);
                        pta[3] = new Point3D(x, pts[0].Y, pts[3].Z);
                    }
                }
                else if (_chartStyle.Elevation < 0)
                {
                    if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
                    {
                        pta[1] = new Point3D(x, pts[2].Y, pts[1].Z);
                        pta[2] = new Point3D(x, pts[2].Y, pts[3].Z);
                        pta[3] = new Point3D(x, pts[0].Y, pts[3].Z);
                    }
                    else
                    {
                        pta[1] = new Point3D(x, pts[0].Y, pts[1].Z);
                        pta[2] = new Point3D(x, pts[0].Y, pts[3].Z);
                        pta[3] = new Point3D(x, pts[2].Y, pts[3].Z);
                    }
                }
                for (var i = 0; i < pta.Length; i++)
                {
                    pta[i] = Normalize3D(pta[i]);
                }

                //_chartStyle.GridlineColor = Brushes.Red;
                DrawGridline(pta[0], pta[1]);
                //_chartStyle.GridlineColor = Brushes.Black;
                DrawGridline(pta[1], pta[2]);
            }




            // Draw y gridlines:
            if (_chartStyle.IsYGrid)
            {
                for (var y = _chartStyle.Ymin; y <= _chartStyle.Ymax; y += _chartStyle.YTick)
                {
                    //pts = CoordinatesOfChartBox();
                    pta[0] = new Point3D(pts[1].X, y, pts[1].Z);
                    if (_chartStyle.Elevation >= 0)
                    {
                        if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
                        {
                            pta[1] = new Point3D(pts[2].X, y, pts[1].Z);
                            pta[2] = new Point3D(pts[2].X, y, pts[3].Z);
                        }
                        else
                        {
                            pta[1] = new Point3D(pts[0].X, y, pts[1].Z);
                            pta[2] = new Point3D(pts[0].X, y, pts[3].Z);
                        }
                    }
                    if (_chartStyle.Elevation < 0)
                    {
                        if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
                        {
                            pta[1] = new Point3D(pts[0].X, y, pts[1].Z);
                            pta[2] = new Point3D(pts[0].X, y, pts[3].Z);
                        }
                        else
                        {
                            pta[1] = new Point3D(pts[2].X, y, pts[1].Z);
                            pta[2] = new Point3D(pts[2].X, y, pts[3].Z);
                        }
                    }
                    for (var i = 0; i < pta.Length; i++)
                    {
                        pta[i] = Normalize3D(pta[i]);
                    }
                    DrawGridline(pta[0], pta[1]);
                    DrawGridline(pta[1], pta[2]);
                }
            }



            // Draw Z gridlines:
            if (_chartStyle.IsZGrid)
            {
                for (var z = _chartStyle.Zmin; z <= _chartStyle.Zmax; z += _chartStyle.ZTick)
                {
                    //pts = CoordinatesOfChartBox();
                    pta[0] = new Point3D(pts[2].X, pts[2].Y, z);
                    if (_chartStyle.Elevation >= 0)
                    {
                        if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
                        {
                            pta[1] = new Point3D(pts[2].X, pts[0].Y, z);
                            pta[2] = new Point3D(pts[0].X, pts[0].Y, z);
                        }
                        else
                        {
                            pta[1] = new Point3D(pts[0].X, pts[2].Y, z);
                            pta[2] = new Point3D(pts[0].X, pts[1].Y, z);
                        }
                    }
                    if (_chartStyle.Elevation < 0)
                    {
                        if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90))
                        {
                            pta[1] = new Point3D(pts[0].X, pts[2].Y, z);
                            pta[2] = new Point3D(pts[0].X, pts[0].Y, z);
                        }
                        else
                        {
                            pta[1] = new Point3D(pts[2].X, pts[0].Y, z);
                            pta[2] = new Point3D(pts[0].X, pts[0].Y, z);
                        }
                    }
                    for (var i = 0; i < pta.Length; i++)
                    {
                        pta[i] = Normalize3D(pta[i]);
                    }
                    DrawGridline(pta[0], pta[1]);
                    DrawGridline(pta[1], pta[2]);
                }
            }




        }
    }

    private void DrawBackgroundForLines(Point3D point1, Point3D point2, Point3D point3
        , Point3D point4
    )
    {

        //_chartStyle.ChartAreaBackground
        var polygon = new Polygon
        {
            Fill = _chartStyle.DataAreaBackground
        };
        polygon.Points.Add(new Point(point1.X, point1.Y));
        polygon.Points.Add(new Point(point2.X, point2.Y));
        polygon.Points.Add(new Point(point3.X, point3.Y));
        polygon.Points.Add(new Point(point4.X, point4.Y));
        ChartCanvas.Children.Add(polygon);

        //Debug.Print("{0}, {1}", point1.X, point1.Y);
        //Debug.Print("{0}, {1}", point2.X, point2.Y);
        //Debug.Print("{0}, {1}", point3.X, point3.Y);
        //Debug.Print("{0}, {1}", point4.X, point4.Y);
    }

    private void DrawGridline(Point3D pt1, Point3D pt2)
    {
        _gridline = new Line
        {
            Stroke = _chartStyle.GridlineColor,
            StrokeThickness = _chartStyle.GridlineThickness
        };
        AddGridlinePattern();
        _gridline.X1 = pt1.X;
        _gridline.Y1 = pt1.Y;
        _gridline.X2 = pt2.X;
        _gridline.Y2 = pt2.Y;
        ChartCanvas.Children.Add(_gridline);
    }

    #endregion

    #region Add Labels:

    private void AddLabels()
    {
        Point3D pt;
        var pts = CoordinatesOfChartBox();
        TextBlock tb;

        // Add x tick labels:
        var offset = (_chartStyle.Ymax - _chartStyle.Ymin) * 0.1;
        var labelSpace = offset;


        for (var x = _chartStyle.Xmin; x <= _chartStyle.Xmax; x += _chartStyle.XTick)
        {
            if (_chartStyle.Elevation >= 0)
            {
                if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 90)
                    labelSpace = -offset;
            }
            else if (_chartStyle.Elevation < 0)
            {
                if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || _chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
                    labelSpace = -offset;
            }
            pt = new Point3D(x, pts[1].Y + labelSpace, pts[1].Z);
            pt = Normalize3D(pt);

            var labelText = !string.IsNullOrEmpty(_chartStyle.NumberFormatX)
                ? x.ToString(_chartStyle.NumberFormatX)
                : x.ToString(CultureInfo.InvariantCulture);

            tb = new TextBlock
            {
                Text = labelText,
                Foreground = _chartStyle.TickColor,
                FontFamily = _chartStyle.TickFont,
                FontSize = _chartStyle.TickFontSize,
                TextAlignment = TextAlignment.Center
            };

            ChartCanvas.Children.Add(tb);
            Canvas.SetLeft(tb, pt.X);
            Canvas.SetTop(tb, pt.Y);
        }

        // Add y tick labels:
        offset = (_chartStyle.Xmax - _chartStyle.Xmin) * 0.15;
        labelSpace = offset;
        for (var y = _chartStyle.Ymin; y <= _chartStyle.Ymax; y += _chartStyle.YTick)
        {

            if (_chartStyle.Elevation >= 0)
            {
                if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                    labelSpace = -offset;
            }
            else if (_chartStyle.Elevation < 0)
            {
                if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 180)
                    labelSpace = -offset;
            }
            pt = new Point3D(pts[1].X + labelSpace, y, pts[1].Z);
            pt = Normalize3D(pt);

            var labelText = !string.IsNullOrEmpty(_chartStyle.NumberFormatY)
                ? y.ToString(_chartStyle.NumberFormatY)
                : y.ToString(CultureInfo.InvariantCulture);

            tb = new TextBlock
            {
                Text = labelText,
                Foreground = _chartStyle.TickColor,
                FontFamily = _chartStyle.TickFont,
                FontSize = _chartStyle.TickFontSize,
                TextAlignment = TextAlignment.Center
            };
            tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var ytickSize = tb.DesiredSize;
            ChartCanvas.Children.Add(tb);
            Canvas.SetLeft(tb, pt.X - ytickSize.Width / 2);
            Canvas.SetTop(tb, pt.Y);
        }

        // Add z tick labels:
        var xoffset = (_chartStyle.Xmax - _chartStyle.Xmin) * 0.1;
        var yoffset = (_chartStyle.Ymax - _chartStyle.Ymin) * 0.1;
        var xlabelSpace = xoffset;
        var ylabelSpace = yoffset;
        tb = new TextBlock { Text = "A" };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

        if (_chartStyle.Elevation >= 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90)
            {
                xlabelSpace = 0;
                ylabelSpace = yoffset;
            }
            else if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 0)
            {
                xlabelSpace = xoffset;
                ylabelSpace = 0;
            }
            else if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90)
            {
                xlabelSpace = 0;
                ylabelSpace = -yoffset;
            }
            else if (_chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
            {
                xlabelSpace = -xoffset;
                ylabelSpace = 0;
            }
        }
        else if (_chartStyle.Elevation < 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90)
            {
                ylabelSpace = 0;
                xlabelSpace = xoffset;
            }
            else if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 0)
            {
                ylabelSpace = -yoffset;
                xlabelSpace = 0;
            }
            else if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90)
            {
                ylabelSpace = 0;
                xlabelSpace = -xoffset;
            }
            else if (_chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
            {
                ylabelSpace = yoffset;
                xlabelSpace = 0;
            }
        }

        for (var z = _chartStyle.Zmin; z <= _chartStyle.Zmax; z += _chartStyle.ZTick)
        {

            pt = new Point3D(pts[2].X - xlabelSpace, pts[2].Y - ylabelSpace, z);
            pt = Normalize3D(pt);

            var labelText = !string.IsNullOrEmpty(_chartStyle.NumberFormatZ)
                ? z.ToString(_chartStyle.NumberFormatZ)
                : z.ToString(CultureInfo.InvariantCulture);

            tb = new TextBlock
            {
                Text = labelText,
                Foreground = _chartStyle.TickColor,
                FontFamily = _chartStyle.TickFont,
                FontSize = _chartStyle.TickFontSize,
                TextAlignment = TextAlignment.Center
            };
            tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var ztickSize = tb.DesiredSize;
            ChartCanvas.Children.Add(tb);
            Canvas.SetLeft(tb, pt.X - ztickSize.Width * 2 / 3 - 1);
            Canvas.SetTop(tb, pt.Y - ztickSize.Height * 0.8);
        }







        // Add x axis label:
        offset = (_chartStyle.Ymax - _chartStyle.Ymin) * 0.4;
        labelSpace = offset;
        var offset1 = (_chartStyle.Xmax - _chartStyle.Xmin) * 0.2;
        var xc = offset1;
        if (_chartStyle.Elevation >= 0)
        {
            if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 90)
                labelSpace = -offset;
            if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth <= 180)
                xc = -offset1;
        }
        else if (_chartStyle.Elevation < 0)
        {
            if ((_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90) || _chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
                labelSpace = -offset;
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth <= 0)
                xc = -offset1;
        }
        var pta = new Point3D[2];
        pta[0] = new Point3D(_chartStyle.Xmin, pts[1].Y + labelSpace, pts[1].Z);
        pta[1] = new Point3D((_chartStyle.Xmin + _chartStyle.Xmax) / 2 - xc, pts[1].Y + labelSpace, pts[1].Z);
        pta[0] = Normalize3D(pta[0]);
        pta[1] = Normalize3D(pta[1]);
        var theta = Math.Atan((pta[1].Y - pta[0].Y) / (pta[1].X - pta[0].X));
        theta = theta * 180 / Math.PI;
        tb = new TextBlock
        {
            Text = _chartStyle.XLabel,
            Foreground = _chartStyle.LabelColor,
            FontFamily = _chartStyle.LabelFont,
            FontSize = _chartStyle.LabelFontSize,
            FontWeight = FontWeights.Bold
        };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var xLabelSize = tb.DesiredSize;

        var tg = new TransformGroup();
        var rt = new RotateTransform(theta, 0.5, 0.5);
        var tt = new TranslateTransform(pta[1].X - xLabelSize.Width * Math.Cos(theta), pta[1].Y - xLabelSize.Height * Math.Sin(theta));
        tg.Children.Add(rt);
        tg.Children.Add(tt);
        tb.RenderTransform = tg;
        ChartCanvas.Children.Add(tb);

        // Add y axis label:
        offset = (_chartStyle.Xmax - _chartStyle.Xmin) * 0.25;
        offset1 = (_chartStyle.Ymax - _chartStyle.Ymin) * 0.15;
        labelSpace = offset;
        var yc = _chartStyle.YTick;
        if (_chartStyle.Elevation >= 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < 0)
                labelSpace = -offset;
            if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth <= 90)
                yc = -offset1;
        }
        else if (_chartStyle.Elevation < 0)
        {
            yc = -offset1;
            if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 180)
                labelSpace = -offset;
            if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth <= 90)
                yc = offset1;
        }
        pta[0] = new Point3D(pts[1].X + labelSpace, _chartStyle.Ymin, pts[1].Z);

        // Y:  - yc
        pta[1] = new Point3D(pts[1].X + labelSpace, _chartStyle.Ymin + (_chartStyle.Ymax - _chartStyle.Ymin) / 2, pts[1].Z);
        pta[0] = Normalize3D(pta[0]);
        pta[1] = Normalize3D(pta[1]);

        theta = Math.Atan((pta[1].Y - pta[0].Y) / (pta[1].X - pta[0].X));
        theta = theta * 180 / Math.PI;
        tb = new TextBlock
        {
            Text = _chartStyle.YLabel,
            Foreground = _chartStyle.LabelColor,
            FontFamily = _chartStyle.LabelFont,
            FontSize = _chartStyle.LabelFontSize,
            FontWeight = FontWeights.Bold
        };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var yLabelSize = tb.DesiredSize;

        tg = new TransformGroup();
        tt = new TranslateTransform(pta[1].X - yLabelSize.Width * Math.Sin(theta), pta[1].Y - yLabelSize.Height * Math.Cos(theta));
        rt = new RotateTransform(theta, 0.5, 0.5);
        tg.Children.Add(rt);
        tg.Children.Add(tt);
        tb.RenderTransform = tg;
        ChartCanvas.Children.Add(tb);

        // Add z axis labels:
        double zticklength = 10;

        offset = (_chartStyle.Xmax - _chartStyle.Xmin) * 0.4;

        labelSpace = 1.3f * offset;
        offset1 = (_chartStyle.Zmax - _chartStyle.Zmin) * 0.3;
        var zc = -offset1;
        for (var z = _chartStyle.Zmin; z < _chartStyle.Zmax; z += _chartStyle.ZTick)
        {
            tb = new TextBlock { Text = z.ToString(CultureInfo.InvariantCulture) };
            tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var size1 = tb.DesiredSize;
            if (zticklength < size1.Width)
                zticklength = size1.Width;
        }

        var zlength = -zticklength;
        if (_chartStyle.Elevation >= 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90)
            {
                zlength = -zticklength;
                labelSpace = -1.3f * offset;
                zc = -offset1;
            }
            else if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 0)
            {
                zlength = zticklength;
                labelSpace = 2 * offset / 3f;
                zc = offset1;
            }
            else if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90)
            {
                zlength = zticklength;
                labelSpace = 2 * offset / 3f;
                zc = -offset1;
            }
            else if (_chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
            {
                zlength = -zticklength;
                labelSpace = -1.3f * offset;
                zc = offset1;
            }
        }
        else if (_chartStyle.Elevation < 0)
        {
            if (_chartStyle.Azimuth >= -180 && _chartStyle.Azimuth < -90)
            {
                zlength = -zticklength;
                labelSpace = -1.3f * offset;
                zc = offset1;
            }
            else if (_chartStyle.Azimuth >= -90 && _chartStyle.Azimuth < 0)
            {
                zlength = zticklength;
                labelSpace = 2 * offset / 3f;
                zc = -offset1;
            }
            else if (_chartStyle.Azimuth >= 0 && _chartStyle.Azimuth < 90)
            {
                zlength = zticklength;
                labelSpace = 2 * offset / 3f;
                zc = offset1;
            }
            else if (_chartStyle.Azimuth >= 90 && _chartStyle.Azimuth <= 180)
            {
                zlength = -zticklength;
                labelSpace = -1.3f * offset;
                zc = -offset1;
            }
        }
        pta[0] = new Point3D(pts[2].X - labelSpace, pts[2].Y, (_chartStyle.Zmin + _chartStyle.Zmax) / 2 + zc);

        pta[0] = Normalize3D(pta[0]);
        tb = new TextBlock
        {
            Text = _chartStyle.ZLabel,
            Foreground = _chartStyle.LabelColor,
            FontFamily = _chartStyle.LabelFont,
            FontSize = _chartStyle.LabelFontSize,
            FontWeight = FontWeights.Bold
        };

        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var zLabelSize = tb.DesiredSize;

        tg = new TransformGroup();
        tt = new TranslateTransform(pta[0].X - zlength * 2F, pta[0].Y + zLabelSize.Width);
        rt = new RotateTransform(270, 0.5, 0.5);
        tg.Children.Add(rt);
        tg.Children.Add(tt);
        tb.RenderTransform = tg;
        ChartCanvas.Children.Add(tb);
    }

    #endregion
}
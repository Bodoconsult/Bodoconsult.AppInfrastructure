using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Wpf.Charting.Models;
using Bodoconsult.App.Wpf.Charting.Styling;
using Bodoconsult.App.Wpf.Helpers;
using Color = System.Windows.Media.Color;

namespace Bodoconsult.App.Wpf.Charting.Services;

/// <summary>
/// Handles the creation of a 3D surface chart
/// </summary>
public class SurfaceChartHandler
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public SurfaceChartHandler(ChartStyle chartStyle)
    {
        UseInterpolation = true;
        NumberOfInterpolations = 2;
        ChartStyle = chartStyle;
    }

    /// <summary>
    /// Use interpolation for chart drawing
    /// </summary>
    public bool UseInterpolation { get; set; }

    /// <summary>
    /// Number of interpolation steps between to values. Default 2
    /// </summary>
    public int NumberOfInterpolations { get; set; }

    /// <summary>
    /// Styling information and series data for the surface chart
    /// </summary>
    public ChartStyle ChartStyle { get;  }

    /// <summary>
    /// Draws the chart in a new canvas
    /// </summary>
    /// <returns></returns>
    public Canvas? DrawChart()
    {
        Canvas? canvas = null;

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            canvas = new Canvas();
        });

        return DrawChart(canvas);
    }

    /// <summary>
    /// Draws a chart in an existing canvas
    /// </summary>
    /// <param name="canvas">Canvas to draw the chart on</param>
    /// <returns>Canvas with the drawn chart</returns>
    public Canvas? DrawChart(Canvas? canvas)
    {
        // No data points
        if (ChartStyle.DataSeries.PointArray.Length==0)
        {
            return null;
        }
        
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {

            //var e1 = ChartCanvas.Children.Count;

            //ChartStyle.Background = Background;


            //if (SecondBackgroundColor == Colors.Transparent)
            //{
            //    ChartStyle.Background = new SolidColorBrush(BackgroundColor);
            //}
            //else
            //{
            //    var brush = new LinearGradientBrush
            //    {
            //        StartPoint = new Point(0, 0),
            //        EndPoint = new Point(0, 1)
            //    };

            //    brush.GradientStops.Add(new GradientStop(BackgroundColor, 0.0));
            //    brush.GradientStops.Add(new GradientStop(SecondBackgroundColor, 1.0));

            //    ChartStyle.Background = brush;
            //}

            //// Background ChartArea
            //if (ChartAreaSecondBackgroundColor == Colors.Transparent)
            //{
            //    ChartStyle.ChartAreaBackground = new SolidColorBrush(ChartAreaBackgroundColor);
            //}
            //else
            //{
            //    var brush = new LinearGradientBrush
            //    {
            //        StartPoint = new Point(0, 0),
            //        EndPoint = new Point(0, 1)
            //    };

            //    brush.GradientStops.Add(new GradientStop(ChartAreaBackgroundColor, 0.0));
            //    brush.GradientStops.Add(new GradientStop(ChartAreaSecondBackgroundColor, 1.0));

            //    ChartStyle.ChartAreaBackground = brush;
            //}



            var dsc = new DrawSurfaceChart
            {
                ChartCanvas = canvas,
                SurfaceChartType = DrawSurfaceChart.SurfaceChartTypeEnum.Surface,
                IsColormap = false,
                IsHiddenLine = true,
                IsInterp = UseInterpolation,
                NumberInterp = NumberOfInterpolations,
                Colormap = { ColormapBrushType = ColormapBrush.ColormapBrushEnum.Jet }
            };
            dsc.CreateChart(ChartStyle);


        });

        return canvas;
    }

    /// <summary>
    /// Draw chart to PNG file
    /// </summary>
    /// <param name="path">File path</param>
    public void DrawChartToPng(string path)
    {
        //var canvas = DrawChart();
        //WpfUtility.RenderCanvasToFile(canvas, (int)Width, (int)Height, path, WpfUtility.ImageFormat.Png);

        var stream = DrawChartToStream();

        if (stream is null)
        {
            return;
        }

        FileSystemHelper.DeleteFile(path);

        using (var fstream = File.OpenWrite(path))
        {
            stream.WriteTo(fstream);
            fstream.Flush();
            fstream.Close();
        }
    }

    /// <summary>
    /// Draw chart to a MemoryStream
    /// </summary>
    /// <returns></returns>
    public MemoryStream? DrawChartToStream()
    {
        MemoryStream? erg = null;

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            var canvas = DrawChart();
            erg = WpfHelper.RenderCanvasToImageStream(canvas,
                (int) ChartStyle.Width,
                (int) ChartStyle.Height,
                WpfHelper.ImageFormat.Png);

            var image = RoundCorners(System.Drawing.Image.FromStream(erg), 50);
            erg.Close();

            erg = new MemoryStream();
            image.Save(erg, ImageFormat.Png);
            erg.Position = 0;
        });

        return erg;
    }

    private System.Drawing.Image RoundCorners(System.Drawing.Image image, int cornerRadius)
    {
        cornerRadius *= 2;
        var roundedImage = new Bitmap(image.Width, image.Height);

        var gp = new GraphicsPath();
        gp.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
        gp.AddArc(0 + roundedImage.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
        gp.AddArc(0 + roundedImage.Width - cornerRadius, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
        gp.AddArc(0, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
        gp.CloseFigure();

        using (var g = Graphics.FromImage(roundedImage))
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            // Fill Background
            if (ChartStyle.PaperColor != Colors.Transparent)
            {
                g.FillRectangle(new SolidBrush(ConvertToDrawingColor(ChartStyle.PaperColor)), 0, 0, image.Width, image.Height);
            }

            // Rounded corners
            g.SetClip(gp);
            g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            if (ChartStyle.ChartBorderWidth > 0)
            {

                var gp1 = new GraphicsPath();
                gp1.AddArc(1, 1, cornerRadius, cornerRadius, 180, 90);
                gp1.AddArc(1 + roundedImage.Width - cornerRadius - 2, 1, cornerRadius, cornerRadius, 270, 90);
                gp1.AddArc(1 + roundedImage.Width - cornerRadius - 2, 1 + roundedImage.Height - cornerRadius - 2, cornerRadius, cornerRadius, 0, 90);
                gp1.AddArc(1, 1 + roundedImage.Height - cornerRadius - 2, cornerRadius, cornerRadius, 90, 90);
                gp1.CloseFigure();

                g.DrawPath(new System.Drawing.Pen(ConvertToDrawingColor(ChartStyle.ChartBorderColor), ChartStyle.ChartBorderWidth), gp1);
            }
        }

        //var roundedImageEnd = new Bitmap(image.Width, image.Height - ChartData.CorrectiveFactor);
        //using (var g = Graphics.FromImage(roundedImageEnd))
        //{
        //    g.SmoothingMode = SmoothingMode.HighQuality;
        //    g.DrawImage(roundedImage, new Rectangle(0, 0, image.Width, image.Height - ChartData.CorrectiveFactor), new Rectangle(0, 0, image.Width, image.Height - ChartData.CorrectiveFactor), GraphicsUnit.Pixel);
        //}


        if (ChartStyle.ChartShadow < 0.0001)
        {
            return roundedImage;
        }

        var shadowedImageEnd = new Bitmap((int)(image.Width * (1 + ChartStyle.ChartShadow)),
            (int)(image.Height * (1 + 1.5 * ChartStyle.ChartShadow)));
        using (var g = Graphics.FromImage(shadowedImageEnd))
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            // Fill Background
            if (ChartStyle.PaperColor != Colors.Transparent)
            {
                g.FillRectangle(new SolidBrush(ConvertToDrawingColor( ChartStyle.PaperColor)), 0, 0, shadowedImageEnd.Width, shadowedImageEnd.Height);
            }

            var left = (int)(image.Width * ChartStyle.ChartShadow);
            var top = (int)(1.5 * image.Height * ChartStyle.ChartShadow);

            var gp1 = new GraphicsPath();
            gp1.AddArc(left, top, cornerRadius, cornerRadius, 180, 90);
            gp1.AddArc(left + roundedImage.Width - cornerRadius, top, cornerRadius, cornerRadius, 270, 90);
            gp1.AddArc(left + roundedImage.Width - cornerRadius, top + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            gp1.AddArc(left, top + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            gp1.CloseFigure();


            //var brush = new PathGradientBrush(gp1)
            //{
            //    CenterPoint = new PointF(left + roundedImage.Width/2, top+roundedImage.Height/2),
            //    CenterColor = Color.Black,
            //    SurroundColors = new[] {Color.White}
            //};


            //g.FillPath(brush, gp1);
            //g.SetClip(gp);



            using (var brush = new PathGradientBrush(gp1))
            {
                brush.WrapMode = WrapMode.Clamp;

                var colorBlend = new ColorBlend(3)
                {
                    Colors =
                    [
                        System.Drawing.Color.Transparent,
                        System.Drawing.Color.FromArgb(180, ChartStyle.ChartShadowColor.R, ChartStyle.ChartShadowColor.G, ChartStyle.ChartShadowColor.B),
                        System.Drawing.Color.FromArgb(180, ChartStyle.ChartShadowColor.R, ChartStyle.ChartShadowColor.G, ChartStyle.ChartShadowColor.B)
                    ],
                    Positions = [0f, .1f, 1f]
                };

                brush.InterpolationColors = colorBlend;

                g.FillPath(brush, gp1);
            }

            g.SetClip(gp);
            g.DrawImage(roundedImage, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
        }

        return shadowedImageEnd;
    }

    /// <summary>
    /// Save 3D-Chart as XAML file
    /// </summary>
    /// <param name="path">File path</param>
    public void DrawChartToXamlFile(string path)
    {
        var canvas = DrawChart();
        WpfHelper.SaveElementAsXamlFile(canvas, path);
    }

    /// <summary>
    /// Convert a WPF color to System.Drawing.Color
    /// </summary>
    /// <param name="wpfColor">WPF color</param>
    /// <returns>System.Drawing.Color</returns>
    private static System.Drawing.Color ConvertToDrawingColor(Color wpfColor)
    {
        return System.Drawing.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
    }

    ///// <summary>
    ///// Convert a WPF color to System.Drawing.Color
    ///// </summary>
    ///// <param name="wpfColor"></param>
    ///// <returns></returns>
    //private Color ConvertToWpfColor(System.Drawing.Color wpfColor)
    //{
    //    return Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
    //}
}
using System.Windows.Media;

namespace Bodoconsult.App.Wpf.Charting.Styling;

/// <summary>
/// Brush used for color map
/// </summary>
public class ColormapBrush
{
    private int _colormapLength = 64;
    private byte _alphaValue = 255;
    private double _ymax = 10;
    private int _ydivisions = 10;
    private ColormapBrushEnum _colormapBrushType = ColormapBrushEnum.Jet;

    /// <summary>
    /// Type of color map
    /// </summary>
    public ColormapBrushEnum ColormapBrushType
    {
        get { return _colormapBrushType; }
        set { _colormapBrushType = value; }
    }

    /// <summary>
    /// Length of color map
    /// </summary>
    public int ColormapLength
    {
        get { return _colormapLength; }
        set { _colormapLength = value; }
    }

    /// <summary>
    /// Alpha value
    /// </summary>
    public byte AlphaValue
    {
        get { return _alphaValue; }
        set { _alphaValue = value; }
    }

    /// <summary>
    /// Minimum to show on y axis
    /// </summary>
    public double Ymin { get; set; }

    /// <summary>
    /// Maximum to show on y axis
    /// </summary>
    public double Ymax
    {
        get { return _ymax; }
        set { _ymax = value; }
    }
    /// <summary>
    /// 
    /// </summary>
    public int Ydivisions
    {
        get { return _ydivisions; }
        set { _ydivisions = value; }
    }

    /// <summary>
    /// default ctor
    /// </summary>
    /// <returns></returns>
    public SolidColorBrush[] ColormapBrushes()
    {
        var cmap = new byte[ColormapLength, 4];
        var array = new double[ColormapLength];

        switch (ColormapBrushType)
        {
            case ColormapBrushEnum.Spring:
                for (var i = 0; i < ColormapLength; i++)
                {
                    array[i] = 1.0 * i / (ColormapLength - 1);
                    cmap[i, 0] = AlphaValue;
                    cmap[i, 1] = 255;
                    cmap[i, 2] = (byte)(255 * array[i]);
                    cmap[i, 3] = (byte)(255 - cmap[i, 2]);
                }
                break;

            case ColormapBrushEnum.Summer:
                for (var i = 0; i < ColormapLength; i++)
                {
                    array[i] = 1.0 * i / (ColormapLength - 1);
                    cmap[i, 0] = AlphaValue;
                    cmap[i, 1] = (byte)(255 * array[i]);
                    cmap[i, 2] = (byte)(255 * 0.5 * (1 + array[i]));
                    cmap[i, 3] = (byte)(255 * 0.4);
                }
                break;

            case ColormapBrushEnum.Autumn:
                for (var i = 0; i < ColormapLength; i++)
                {
                    array[i] = 1.0 * i / (ColormapLength - 1);
                    cmap[i, 0] = AlphaValue;
                    cmap[i, 1] = 255;
                    cmap[i, 2] = (byte)(255 * array[i]);
                    cmap[i, 3] = 0;
                }
                break;

            case ColormapBrushEnum.Winter:
                for (var i = 0; i < ColormapLength; i++)
                {
                    array[i] = 1.0 * i / (ColormapLength - 1);
                    cmap[i, 0] = AlphaValue;
                    cmap[i, 1] = 0;
                    cmap[i, 2] = (byte)(255 * array[i]);
                    cmap[i, 3] = (byte)(255 * (1.0 - 0.5 * array[i]));
                }
                break;

            case ColormapBrushEnum.Gray:
                for (var i = 0; i < ColormapLength; i++)
                {
                    array[i] = 1.0 * i / (ColormapLength - 1);
                    cmap[i, 0] = AlphaValue;
                    cmap[i, 1] = (byte)(255 * array[i]);
                    cmap[i, 2] = (byte)(255 * array[i]);
                    cmap[i, 3] = (byte)(255 * array[i]);
                }
                break;

            case ColormapBrushEnum.Jet:
                var n = (int)Math.Ceiling(ColormapLength / 4.0);
                var cMatrix = new double[ColormapLength, 3];
                var nMod = 0;
                var array1 = new double[3 * n - 1];
                var red = new int[array1.Length];
                var green = new int[array1.Length];
                var blue = new int[array1.Length];

                if (ColormapLength % 4 == 1)
                    nMod = 1;

                for (var i = 0; i < array1.Length; i++)
                {
                    if (i < n)
                        array1[i] = (i + 1.0) / n;
                    else if (i >= n && i < 2 * n - 1)
                        array1[i] = 1.0;
                    else if (i >= 2 * n - 1)
                        array1[i] = (3.0 * n - 1.0 - i) / n;
                    green[i] = (int)Math.Ceiling(n / 2.0) - nMod + i;
                    red[i] = green[i] + n;
                    blue[i] = green[i] - n;
                }

                var nb = 0;
                for (var i = 0; i < blue.Length; i++)
                {
                    if (blue[i] > 0)
                        nb++;
                }

                for (var i = 0; i < ColormapLength; i++)
                {
                    for (var j = 0; j < red.Length; j++)
                    {
                        if (i == red[j] && red[j] < ColormapLength)
                            cMatrix[i, 0] = array1[i - red[0]];
                    }
                    for (var j = 0; j < green.Length; j++)
                    {
                        if (i == green[j] && green[j] < ColormapLength)
                            cMatrix[i, 1] = array1[i - green[0]];
                    }
                    for (var j = 0; j < blue.Length; j++)
                    {
                        if (i == blue[j] && blue[j] >= 0)
                            cMatrix[i, 2] = array1[array1.Length - 1 - nb + i];
                    }
                }

                for (var i = 0; i < ColormapLength; i++)
                {
                    cmap[i, 0] = AlphaValue;
                    for (var j = 0; j < 3; j++)
                        cmap[i, j + 1] = (byte)(cMatrix[i, j] * 255);
                }
                break;

            case ColormapBrushEnum.Hot:
                var n1 = 3 * ColormapLength / 8;
                var red1 = new double[ColormapLength];
                var green1 = new double[ColormapLength];
                var blue1 = new double[ColormapLength];
                for (var i = 0; i < ColormapLength; i++)
                {
                    if (i < n1)
                        red1[i] = 1.0 * (i + 1.0) / n1;
                    else
                        red1[i] = 1.0;
                    if (i < n1)
                        green1[i] = 0.0;
                    else if (i >= n1 && i < 2 * n1)
                        green1[i] = 1.0 * (i + 1 - n1) / n1;
                    else
                        green1[i] = 1.0;
                    if (i < 2 * n1)
                        blue1[i] = 0.0;
                    else
                        blue1[i] = 1.0 * (i + 1 - 2 * n1) / (ColormapLength - 2 * n1);

                    cmap[i, 0] = AlphaValue;
                    cmap[i, 1] = (byte)(255 * red1[i]);
                    cmap[i, 2] = (byte)(255 * green1[i]);
                    cmap[i, 3] = (byte)(255 * blue1[i]);
                }                   
                break;

            case ColormapBrushEnum.Cool:
                for (var i = 0; i < ColormapLength; i++)
                {
                    array[i] = 1.0 * i / (ColormapLength - 1);
                    cmap[i, 0] = AlphaValue;
                    cmap[i, 1] = (byte)(255 * array[i]);
                    cmap[i, 2] = (byte)(255 * (1 - array[i]));
                    cmap[i, 3] = 255;
                }
                break;
        }
        return SetBrush(cmap);
    }

    /// <summary>
    /// Type of color map
    /// </summary>
    public enum ColormapBrushEnum
    {
#pragma warning disable 1591
        Spring = 0,
        Summer = 1,
        Autumn = 2,
        Winter = 3,
        Gray = 4,
        Jet = 5,
        Hot = 6,
        Cool = 7
#pragma warning restore 1591
    }

    private SolidColorBrush[] SetBrush(byte[,] cmap)
    {
        var brushes = new SolidColorBrush[Ydivisions];
        var dy = (Ymax - Ymin) / (Ydivisions - 1);
        for (var i = 0; i < Ydivisions; i++)
        {
            var colorIndex = (int)((ColormapLength - 1) * i * dy / (Ymax - Ymin));
            if (colorIndex < 0)
                colorIndex = 0;
            if (colorIndex >= ColormapLength)
                colorIndex = ColormapLength - 1;
            brushes[i] = new SolidColorBrush(Color.FromArgb(cmap[colorIndex, 0], 
                cmap[colorIndex, 1], 
                cmap[colorIndex, 2], 
                cmap[colorIndex, 3]));
        }
        return brushes;
    }
}
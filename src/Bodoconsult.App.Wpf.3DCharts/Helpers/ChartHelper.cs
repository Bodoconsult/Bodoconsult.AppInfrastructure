// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Windows.Media.Media3D;

namespace Bodoconsult.App.Wpf.Charting.Helpers;

/// <summary>
/// 
/// </summary>
public static class ChartHelper
{
    /// <summary>
    /// Get the minimum and maximum for y-axis of a line chart
    /// </summary>
    /// <param name="minimum">minimum value found in a chart series for the line chart</param>
    /// <param name="maximum">maximum value found in a chart series for the line chart</param>
    /// <returns>Interval for grid on y-axis</returns>
    public static double GetMinMaxForAxis(ref double minimum, ref double maximum)
    {
        var delta = Math.Abs(maximum - minimum);

        var length = 0;

        if (delta < 1)
        {
            var deltaString = delta.ToString("0.0000000000000000");

            for (var i = 2; i < deltaString.Length; i++)
            {
                if (deltaString[i] == '0')
                {
                    continue;
                }

                length = i;
                break;
            }

            length -= 1;
        }
        else
        {
            length = ((int)delta).ToString("0").Length - 2;
        }

        var p = Math.Pow(10, length);

        var count = (int)(delta / p);

        if (count > 10)
        {
            p *= 5;

            count = (int)(delta / p);

            if (count > 10)
            {
                p *= 2;
            }
        }

        p = delta < 1 ? 1 / p : p;

        var min = Math.Floor(minimum / p) * p;
        var max = Math.Ceiling(maximum / p) * p;

        //if (max - maximum > 0.5 * x) max = max - 0.5 * (1 / x);
        //if (min - minimum > 0.5 * x) min = min + 0.5 * (1 / x);


        minimum = min;
        maximum = max;
        return p;

    }

    /// <summary>
    /// Get the scaling for y-axis of a line Chart
    /// </summary>
    /// <param name="minimum">minimum value found in a Chart series for the line Chart</param>
    /// <param name="maximum">maximum value found in a Chart series for the line Chart</param>
    /// <returns>Interval for grid on y-axis</returns>
    public static double GetScalingForAxis(double minimum, double maximum)
    {
        return GetMinMaxForAxis(ref minimum, ref maximum);
    }

    /// <summary>
    /// Elevate azimuth
    /// </summary>
    /// <param name="elevation">Elevation</param>
    /// <param name="azimuth">Azimuth</param>
    /// <returns>A 3D matrix</returns>
    public static Matrix3D AzimuthElevation(double elevation, double azimuth)
    {
        if (elevation > 90.0)
        {
            elevation = 90.0;
        }
        else if (elevation < -90.0)
        {
            elevation = -90.0;
        }

        if (azimuth > 180.0)
        {
            azimuth = 180.0;
        }
        else if (azimuth < -180.0)
        {
            azimuth = -180.0;
        }

        elevation = elevation * Math.PI / 180.0;
        azimuth = azimuth * Math.PI / 180.0;
        double m33 = Math.Sin(elevation);
        double m32 = Math.Cos(elevation);
        double m21 = Math.Sin(azimuth);
        double m11 = Math.Cos(azimuth);
        return new Matrix3D(m11, -m33 * m21, m32 * m21, 0.0, m21, m33 * m11, -m32 * m11, 0.0, 0.0, m32, m33, 0.0, 0.0, 0.0, 0.0, 1.0);
    }

    /// <summary>
    /// Get a normal vectore from 3 points
    /// </summary>
    /// <param name="pt1">Point 1</param>
    /// <param name="pt2">Point 2</param>
    /// <param name="pt3">Point 3</param>
    /// <returns>Normal vector</returns>
    public static Vector3D NormalVector(Point3D pt1, Point3D pt2, Point3D pt3)
    {
        Vector3D vector1 = new();
        Vector3D vector2 = new();
        vector1.X = pt2.X - pt1.X;
        vector1.Y = pt2.Y - pt1.Y;
        vector1.Z = pt2.Z - pt1.Z;
        vector2.X = pt3.X - pt2.X;
        vector2.Y = pt3.Y - pt2.Y;
        vector2.Z = pt3.Z - pt1.Z;
        return Vector3D.CrossProduct(vector1, vector2);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

/*
 *
 *
 * System.Windows.Media is part of the WPF environment by Microsoft.

 * System.Windows.Media is released as open source under the MIT license

 * License-Url: https://github.com/dotnet/wpf/blob/main/LICENSE.TXT

 * Project-Url: https://github.com/dotnet/wpf

 * The MIT License (MIT)

 * Copyright (c) .NET Foundation and Contributors

 * All rights reserved.

 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Diagnostics;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Color defined in ARGB mode
/// </summary>
[DebuggerDisplay("R = {R} G = {G} B = {B} A = {A} ({Html, nq})")]
public class TypoColor: IEquatable<TypoColor>
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public TypoColor()
    { }

    /// <summary>
    /// Ctor providing ARGB color parts
    /// </summary>
    /// <param name="a">A</param>
    /// <param name="r">R</param>
    /// <param name="g">G</param>
    /// <param name="b">B</param>
    public TypoColor(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }

    /// <summary>
    /// Ctor providing an HTML color code like #000000
    /// </summary>
    /// <param name="htmlColor">HTML color code like #000000</param>
    /// <exception cref="ArgumentException">HTML color does not have a length of seven chars</exception>
    public TypoColor(string htmlColor)
    {
        if (htmlColor.Length == 7)
        {
            R = Convert.ToByte(htmlColor.Substring(1, 2), 16);
            G = Convert.ToByte(htmlColor.Substring(3, 2), 16);
            B = Convert.ToByte(htmlColor.Substring(5, 2), 16);
        }
        else
        {
            throw new ArgumentException("Color must length 7. Example: #000000");
        }
    }

    /// <summary>
    /// Alpha
    /// </summary>
    public byte A { get; set; } = byte.MaxValue;

    /// <summary>
    /// Red
    /// </summary>
    public byte R { get; set; }

    /// <summary>
    /// Green
    /// </summary>
    public byte G { get; set; }

    /// <summary>
    /// Blue
    /// </summary>
    public byte B { get; set; }

    /// <summary>
    /// Used for displaying basic color info in debugger
    /// </summary>
    public string Html => $"#{R:X2}{G:X2}{B:X2}";

    ///<summary>
    /// Color - sRgb legacy interface, assumes Rgb values are sRgb
    /// Source: System.Windows.Media by Microsoft
    ///</summary>
    public static TypoColor FromUInt32(uint argb)// internal legacy sRGB interface
    {
        var c1 = new TypoColor
        {
            A = (byte)((argb & 0xff000000) >> 24),
            R = (byte)((argb & 0x00ff0000) >> 16),
            G = (byte)((argb & 0x0000ff00) >> 8),
            B = (byte)(argb & 0x000000ff)
        };
        return c1;
    }

    /// <summary>
    /// Get a color from HTML color string with 7 chars length like #000000
    /// </summary>
    /// <param name="htmlColor">HTML color string with 7 chars length</param>
    /// <returns>Color or null</returns>
    public static TypoColor? FromHtml(string htmlColor)
    {
        var color = new TypoColor();

        if (htmlColor.Length == 7)
        {
            color.R = Convert.ToByte(htmlColor.Substring(1, 2), 16);
            color.G = Convert.ToByte(htmlColor.Substring(3, 2), 16);
            color.B = Convert.ToByte(htmlColor.Substring(5, 2), 16);
            return color;
        }
        //string r = char.ToString(htmlColor[1]);
        //string g = char.ToString(htmlColor[2]);
        //string b = char.ToString(htmlColor[3]);

        //c = System.Drawing.Color.FromArgb(Convert.ToInt32(r + r, 16),
        //    Convert.ToInt32(g + g, 16),
        //    Convert.ToInt32(b + b, 16));
        return null;

    }

    /// <summary>
    /// Get a color from full ARGB
    /// </summary>
    /// <param name="alpha">Alpha</param>
    /// <param name="red">Red</param>
    /// <param name="green">Green</param>
    /// <param name="blue">Blue</param>
    /// <returns>ARGB color</returns>
    public static TypoColor FromArgb(byte alpha, byte red, byte green, byte blue)
    {
        return new TypoColor(alpha, red, green, blue);
    }

    /// <summary>
    /// Get a color from RGB
    /// </summary>
    /// <param name="red">Red</param>
    /// <param name="green">Green</param>
    /// <param name="blue">Blue</param>
    /// <returns>ARGB color</returns>
    public static TypoColor FromArgb(byte red, byte green, byte blue) => FromArgb(byte.MaxValue, red, green, blue);

    /// <summary>
    /// Color equals other color?
    /// </summary>
    /// <param name="other">Other color</param>
    /// <returns></returns>
    public bool Equals(TypoColor? other)
    {
        if (other is null)
        {
            return false;
        }
        return A == other.A && R == other.R && G == other.G && B == other.B;
    }

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }
        return Equals((TypoColor)obj);
    }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(A, R, G, B);
    }
}
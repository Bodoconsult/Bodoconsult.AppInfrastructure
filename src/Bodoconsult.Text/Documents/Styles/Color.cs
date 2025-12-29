// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Color defined in ARGB mode
/// </summary>
public class Color: TypoColor, IPropertyAsAttributeElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public Color()
    { }

    /// <summary>
    /// Ctor providing ARGB color parts
    /// </summary>
    /// <param name="a">A</param>
    /// <param name="r">R</param>
    /// <param name="g">G</param>
    /// <param name="b">B</param>
    public Color(byte a, byte r, byte g, byte b)
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
    public Color(string htmlColor)
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
    /// Ctor for constructing from base class <see cref="TypoColor"/> instance
    /// </summary>
    /// <param name="color"></param>
    public Color(TypoColor color)
    {
        R = color.R;
        G = color.G;
        B = color.B;
        A = color.A;
    }

    /// <summary>
    /// Current indenttation for LDML creation
    /// </summary>
    [DoNotSerialize]
    public string Indentation { get; set; } = "    ";

    /// <summary>
    /// Parent element
    /// </summary>
    [DoNotSerialize]
    public DocumentElement Parent { get; set; }

    /// <summary>
    /// Add the current element to a document defined in LDML (Logical document markup language)
    /// </summary>
    /// <param name="document">StringBuilder instance to create the LDML in</param>
    /// <param name="indent">Current indent</param>
    public void ToLdmlString(StringBuilder document, string indent)
    {
        document.Append(this.ToHtml());
    }

    /// <summary>
    /// Get the element data as formatted property value for an LDML attribute
    /// </summary>
    public string ToPropertyValue()
    {
        return this.ToHtml();
    }

    ///<summary>
    /// Color - sRgb legacy interface, assumes Rgb values are sRgb
    /// Source: System.Windows.Media by Microsoft
    ///</summary>
    public new static Color FromUInt32(uint argb)// internal legacy sRGB interface
    {
        var c1 = new Color
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
    public new static Color FromHtml(string htmlColor)
    {
        var color = new Color();

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
    public new static Color FromArgb(byte alpha, byte red, byte green, byte blue)
    {
        return new Color(alpha, red, green, blue);
    }

    /// <summary>
    /// Get a color from RGB
    /// </summary>
    /// <param name="red">Red</param>
    /// <param name="green">Green</param>
    /// <param name="blue">Blue</param>
    /// <returns>ARGB color</returns>
    public new static Color FromArgb(byte red, byte green, byte blue) => FromArgb(byte.MaxValue, red, green, blue);
}
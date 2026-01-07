//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System.ComponentModel;
//using System.ComponentModel.Design.Serialization;
//using System.Diagnostics;
//using System.Drawing;
//using System.Globalization;
//using System.Reflection;
//using System;
//using System.Diagnostics;
//using System.ComponentModel;
//using System.Windows.Markup;
//using System.Runtime.InteropServices;
//using System.Windows.Navigation;

//namespace Bodoconsult.App.Abstractions.Interfaces;

///// <summary>
///// ColorConverter parses a TypoColor.
///// </summary>
//public sealed class TypoColorConverter : TypeConverter
//{
//    /// <summary>
//    /// CanConvertFrom
//    /// </summary>
//    public override bool CanConvertFrom(ITypeDescriptorContext td, Type t)
//    {
//        if (t == typeof(string))
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    /// <summary>
//    /// TypeConverter method override.
//    /// </summary>
//    /// <param name="context">ITypeDescriptorContext</param>
//    /// <param name="destinationType">Type to convert to</param>
//    /// <returns>true if conversion is possible</returns>
//    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
//    {
//        if (destinationType == typeof(InstanceDescriptor))
//        {
//            return true;
//        }

//        return base.CanConvertTo(context, destinationType);
//    }

//    ///<summary>
//    /// ConvertFromString
//    ///</summary>
//    public static new object ConvertFromString(string value)
//    {
//        if (null == value)
//        {
//            return null;
//        }

//        return Parsers.ParseColor(value, null);
//    }

//    /// <summary>
//    /// ConvertFrom - attempt to convert to a Color from the given object
//    /// </summary>
//    /// <exception cref="NotSupportedException">
//    /// A NotSupportedException is thrown if the example object is null or is not a valid type
//    /// which can be converted to a Color.
//    /// </exception>
//    public override object ConvertFrom(ITypeDescriptorContext td, System.Globalization.CultureInfo ci, object value)
//    {
//        if (null == value)
//        {
//            throw GetConvertFromException(value);
//        }

//        var s = value as string;

//        if (null == s)
//        {
//            throw new ArgumentException("", "value");
//        }

//        return Parsers.ParseColor(value as string, ci, td);
//    }

//    /// <summary>
//    /// TypeConverter method implementation.
//    /// </summary>
//    /// <exception cref="NotSupportedException">
//    /// An NotSupportedException is thrown if the example object is null or is not a Color,
//    /// or if the destinationType isn't one of the valid destination types.
//    /// </exception>
//    /// <param name="context">ITypeDescriptorContext</param>
//    /// <param name="culture">current culture (see CLR specs)</param>
//    /// <param name="value">value to convert from</param>
//    /// <param name="destinationType">Type to convert to</param>
//    /// <returns>converted value</returns>
//    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
//    {
//        if (destinationType != null && value is Color)
//        {
//            if (destinationType == typeof(InstanceDescriptor))
//            {
//                MethodInfo mi = typeof(Color).GetMethod("FromArgb", new Type[] { typeof(byte), typeof(byte), typeof(byte), typeof(byte) });
//                Color c = (Color)value;
//                return new InstanceDescriptor(mi, new object[] { c.A, c.R, c.G, c.B });
//            }

//            if (destinationType == typeof(string))
//            {
//                TypoColor c = (TypoColor)value;
//                return c.ToString(culture);
//            }
//        }

//        // Pass unhandled cases to base class (which will throw exceptions for null value or destinationType.)
//        return base.ConvertTo(context, culture, value, destinationType);
//    }

//    ///// <summary>
//    ///// ParseColor
//    ///// <param name="color"> string with color description </param>
//    ///// <param name="formatProvider">IFormatProvider for processing string</param>
//    ///// </summary>
//    //internal static TypoColor ParseColor(string color, IFormatProvider formatProvider)
//    //{
//    //    return ParseColor(color, formatProvider, null);
//    //}

//    ///// <summary>
//    ///// ParseColor
//    ///// <param name="color"> string with color description </param>
//    ///// <param name="formatProvider">IFormatProvider for processing string</param>
//    ///// <param name="context">ITypeDescriptorContext</param>
//    ///// </summary>
//    //internal static TypoColor ParseColor(string color, IFormatProvider formatProvider, ITypeDescriptorContext context)
//    //{
//    //    bool isPossibleKnowColor;
//    //    bool isNumericColor;
//    //    bool isScRgbColor;
//    //    bool isContextColor;
//    //    string trimmedColor = KnownColors.MatchColor(color, out isPossibleKnowColor, out isNumericColor, out isContextColor, out isScRgbColor);

//    //    if ((isPossibleKnowColor == false) &&
//    //        (isNumericColor == false) &&
//    //        (isScRgbColor == false) &&
//    //        (isContextColor == false))
//    //    {
//    //        throw new FormatException("Illegal token");
//    //    }

//    //    //Is it a number?
//    //    if (isNumericColor)
//    //    {
//    //        return ParseHexColor(trimmedColor);
//    //    }
//    //    else if (isContextColor)
//    //    {
//    //        return ParseContextColor(trimmedColor, formatProvider, context);
//    //    }
//    //    else if (isScRgbColor)
//    //    {
//    //        return ParseScRgbColor(trimmedColor, formatProvider);
//    //    }
//    //    else
//    //    {
//    //        KnownColor kc = KnownColors.ColorStringToKnownColor(trimmedColor);

//    //        if (kc == KnownColor.UnknownColor)
//    //        {
//    //            throw new FormatException(SR.Parsers_IllegalToken);
//    //        }

//    //        return TypoColor.FromUInt32((uint)kc);
//    //    }
//    //}
//}

//internal static partial class Parsers
//{
//    private const int s_zeroChar = (int)'0';
//    private const int s_aLower = (int)'a';
//    private const int s_aUpper = (int)'A';

//    static private int ParseHexChar(char c)
//    {
//        int intChar = (int)c;

//        if ((intChar >= s_zeroChar) && (intChar <= (s_zeroChar + 9)))
//        {
//            return (intChar - s_zeroChar);
//        }

//        if ((intChar >= s_aLower) && (intChar <= (s_aLower + 5)))
//        {
//            return (intChar - s_aLower + 10);
//        }

//        if ((intChar >= s_aUpper) && (intChar <= (s_aUpper + 5)))
//        {
//            return (intChar - s_aUpper + 10);
//        }
//        throw new FormatException("Illegel token");
//    }

//    static private Color ParseHexColor(string trimmedColor)
//    {
//        int a, r, g, b;
//        a = 255;

//        if (trimmedColor.Length > 7)
//        {
//            a = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
//            r = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
//            g = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
//            b = ParseHexChar(trimmedColor[7]) * 16 + ParseHexChar(trimmedColor[8]);
//        }
//        else if (trimmedColor.Length > 5)
//        {
//            r = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
//            g = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
//            b = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
//        }
//        else if (trimmedColor.Length > 4)
//        {
//            a = ParseHexChar(trimmedColor[1]);
//            a = a + a * 16;
//            r = ParseHexChar(trimmedColor[2]);
//            r = r + r * 16;
//            g = ParseHexChar(trimmedColor[3]);
//            g = g + g * 16;
//            b = ParseHexChar(trimmedColor[4]);
//            b = b + b * 16;
//        }
//        else
//        {
//            r = ParseHexChar(trimmedColor[1]);
//            r = r + r * 16;
//            g = ParseHexChar(trimmedColor[2]);
//            g = g + g * 16;
//            b = ParseHexChar(trimmedColor[3]);
//            b = b + b * 16;
//        }

//        return (Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b));
//    }

//    internal const string s_ContextColor = "ContextColor ";
//    internal const string s_ContextColorNoSpace = "ContextColor";

//    static private Color ParseContextColor(string trimmedColor, IFormatProvider formatProvider, ITypeDescriptorContext context)
//    {
//        if (!trimmedColor.StartsWith(s_ContextColor, StringComparison.OrdinalIgnoreCase))
//        {
//            throw new FormatException("Illegal token");
//        }

//        string tokens = trimmedColor.Substring(s_ContextColor.Length);
//        tokens = tokens.Trim();
//        string[] preSplit = tokens.Split(' ');
//        if (preSplit.GetLength(0) < 2)
//        {
//            throw new FormatException("Illegal token");
//        }

//        tokens = tokens.Substring(preSplit[0].Length);

//        TokenizerHelper th = new TokenizerHelper(tokens, formatProvider);
//        string[] split = tokens.Split(new Char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
//        int numTokens = split.GetLength(0);

//        float alpha = Convert.ToSingle(th.NextTokenRequired(), formatProvider);

//        float[] values = new float[numTokens - 1];

//        for (int i = 0; i < numTokens - 1; i++)
//        {
//            values[i] = Convert.ToSingle(th.NextTokenRequired(), formatProvider);
//        }

//        string profileString = preSplit[0];

//        UriHolder uriHolder = TypeConverterHelper.GetUriFromUriContext(context, profileString);

//        Uri profileUri;

//        if (uriHolder.BaseUri != null)
//        {
//            profileUri = new Uri(uriHolder.BaseUri, uriHolder.OriginalUri);
//        }
//        else
//        {
//            profileUri = uriHolder.OriginalUri;
//        }

//        Color result = Color.FromAValues(alpha, values, profileUri);

//        // If the number of color values found does not match the number of channels in the profile, we must throw
//        if (result.ColorContext.NumChannels != values.Length)
//        {
//            throw new FormatException(SR.Parsers_IllegalToken);
//        }

//        return result;
//    }

//    static private Color ParseScRgbColor(string trimmedColor, IFormatProvider formatProvider)
//    {
//        if (!trimmedColor.StartsWith("sc#", StringComparison.Ordinal))
//        {
//            throw new FormatException(SR.Parsers_IllegalToken);
//        }

//        string tokens = trimmedColor.Substring(3, trimmedColor.Length - 3);

//        // The tokenizer helper will tokenize a list based on the IFormatProvider.
//        TokenizerHelper th = new TokenizerHelper(tokens, formatProvider);
//        float[] values = new float[4];

//        for (int i = 0; i < 3; i++)
//        {
//            values[i] = Convert.ToSingle(th.NextTokenRequired(), formatProvider);
//        }

//        if (th.NextToken())
//        {
//            values[3] = Convert.ToSingle(th.GetCurrentToken(), formatProvider);

//            // We should be out of tokens at this point
//            if (th.NextToken())
//            {
//                throw new FormatException(SR.Parsers_IllegalToken);
//            }

//            return Color.FromScRgb(values[0], values[1], values[2], values[3]);
//        }
//        else
//        {
//            return Color.FromScRgb(1.0f, values[0], values[1], values[2]);
//        }
//    }

//    /// <summary>
//    /// ParseColor
//    /// <param name="color"> string with color description </param>
//    /// <param name="formatProvider">IFormatProvider for processing string</param>
//    /// </summary>
//    internal static Color ParseColor(string color, IFormatProvider formatProvider)
//    {
//        return ParseColor(color, formatProvider, null);
//    }

//    /// <summary>
//    /// ParseColor
//    /// <param name="color"> string with color description </param>
//    /// <param name="formatProvider">IFormatProvider for processing string</param>
//    /// <param name="context">ITypeDescriptorContext</param>
//    /// </summary>
//    internal static Color ParseColor(string color, IFormatProvider formatProvider, ITypeDescriptorContext context)
//    {
//        bool isPossibleKnowColor;
//        bool isNumericColor;
//        bool isScRgbColor;
//        bool isContextColor;
//        string trimmedColor = KnownColors.MatchColor(color, out isPossibleKnowColor, out isNumericColor, out isContextColor, out isScRgbColor);

//        if ((isPossibleKnowColor == false) &&
//            (isNumericColor == false) &&
//            (isScRgbColor == false) &&
//            (isContextColor == false))
//        {
//            throw new FormatException(SR.Parsers_IllegalToken);
//        }

//        //Is it a number?
//        if (isNumericColor)
//        {
//            return ParseHexColor(trimmedColor);
//        }
//        else if (isContextColor)
//        {
//            return ParseContextColor(trimmedColor, formatProvider, context);
//        }
//        else if (isScRgbColor)
//        {
//            return ParseScRgbColor(trimmedColor, formatProvider);
//        }
//        else
//        {
//            KnownColor kc = KnownColors.ColorStringToKnownColor(trimmedColor);

//            if (kc == KnownColor.UnknownColor)
//            {
//                throw new FormatException(SR.Parsers_IllegalToken);
//            }

//            return Color.FromUInt32((uint)kc);
//        }
//    }

//    /// <summary>
//    /// ParseBrush
//    /// <param name="brush"> string with brush description </param>
//    /// <param name="formatProvider">IFormatProvider for processing string</param>
//    /// <param name="context">ITypeDescriptorContext</param>
//    /// </summary>
//    internal static Brush ParseBrush(string brush, IFormatProvider formatProvider, ITypeDescriptorContext context)
//    {
//        bool isPossibleKnownColor;
//        bool isNumericColor;
//        bool isScRgbColor;
//        bool isContextColor;
//        string trimmedColor = KnownColors.MatchColor(brush, out isPossibleKnownColor, out isNumericColor, out isContextColor, out isScRgbColor);

//        if (trimmedColor.Length == 0)
//        {
//            throw new FormatException(SR.Parser_Empty);
//        }

//        // Note that because trimmedColor is exactly brush.Trim() we don't have to worry about
//        // extra tokens as we do with TokenizerHelper.  If we return one of the solid color
//        // brushes then the ParseColor routine (or ColorStringToKnownColor) matched the entire
//        // input.
//        if (isNumericColor)
//        {
//            return (new SolidColorBrush(ParseHexColor(trimmedColor)));
//        }

//        if (isContextColor)
//        {
//            return (new SolidColorBrush(ParseContextColor(trimmedColor, formatProvider, context)));
//        }

//        if (isScRgbColor)
//        {
//            return (new SolidColorBrush(ParseScRgbColor(trimmedColor, formatProvider)));
//        }

//        if (isPossibleKnownColor)
//        {
//            SolidColorBrush scp = KnownColors.ColorStringToKnownBrush(trimmedColor);

//            if (scp != null)
//            {
//                return scp;
//            }
//        }

//        // If it's not a color, so the content is illegal.
//        throw new FormatException(SR.Parsers_IllegalToken);
//    }


//    /// <summary>
//    /// ParseTransform - parse a Transform from a string
//    /// </summary>
//    internal static Transform ParseTransform(
//        string transformString,
//        IFormatProvider formatProvider)
//    {
//        Matrix matrix = Matrix.Parse(transformString);

//        return new MatrixTransform(matrix);
//    }

//    /// <summary>
//    /// Parse a PathFigureCollection string.
//    /// </summary>
//    internal static PathFigureCollection ParsePathFigureCollection(
//        string pathString,
//        IFormatProvider formatProvider)
//    {
//        PathStreamGeometryContext context = new PathStreamGeometryContext();

//        AbbreviatedGeometryParser parser = new AbbreviatedGeometryParser();

//        parser.ParseToGeometryContext(context, pathString, 0 /* curIndex */);

//        PathGeometry pathGeometry = context.GetPathGeometry();

//        return pathGeometry.Figures;
//    }
//}

///// <summary>
/////     This helper method is used primarily by type converters to resolve their uri
///// </summary>
///// <remarks>
/////     There are three scenarios that can happen:
/////
/////     1) inputString is an absolute uri -- we return it as the resolvedUri
/////     2) inputString is not absolute:
/////         i) the relativeBaseUri (obtained from IUriContext) has the following values:
/////                 a) is an absolute uri, we use relativeBaseUri as base uri and resolve
/////                 the inputString against it
/////
/////                 b) is a relative uri, we use Application's base uri (obtained from
/////                 BindUriHelperCore.BaseUri) as the base and resolve the relativeBaseUri
/////                 against it; furthermore, we resolve the inputString against with uri
/////                 obtained from the application base resolution.
/////
/////                 c) is "", we resolve inputString against the Application's base uri
///// </remarks>
//internal static class TypeConverterHelper
//{
//    internal static UriHolder GetUriFromUriContext(ITypeDescriptorContext context, object inputString)
//    {
//        UriHolder uriHolder = new UriHolder();

//        if (inputString is string)
//        {
//            uriHolder.OriginalUri = new Uri((string)inputString, UriKind.RelativeOrAbsolute);
//        }
//        else
//        {
//            Debug.Assert(inputString is Uri);
//            uriHolder.OriginalUri = (Uri)inputString;
//        }

//        if (uriHolder.OriginalUri.IsAbsoluteUri == false)
//        {
//            //Debug.Assert (context != null, "Context should not be null");
//            if (context != null)
//            {
//                IUriContext iuc = (IUriContext)context.GetService(typeof(IUriContext));

//                //Debug.Assert (iuc != null, "IUriContext should not be null here");
//                if (iuc != null)
//                {
//                    // the base uri is NOT ""
//                    if (iuc.BaseUri != null)
//                    {

//                        uriHolder.BaseUri = iuc.BaseUri;

//                        if (!uriHolder.BaseUri.IsAbsoluteUri)
//                        {
//                            uriHolder.BaseUri = new Uri(BaseUriHelper.BaseUri, uriHolder.BaseUri);
//                        }
//                    } // uriHolder.BaseUriString != ""
//                    else
//                    {
//                        // if we reach here, the base uri we got from IUriContext is ""
//                        // and the inputString is a relative uri.  Here we resolve it to
//                        // application's base
//                        uriHolder.BaseUri = BaseUriHelper.BaseUri;
//                    }
//                } // iuc != null
//            } // context!= null
//        } // uriHolder.OriginalUri.IsAbsoluteUri == false

//        return uriHolder;
//    }
//}
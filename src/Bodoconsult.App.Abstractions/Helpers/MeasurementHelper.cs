// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// https://stackoverflow.com/questions/20194403/openxml-distance-size-units

namespace Bodoconsult.App.Abstractions.Helpers;

/// <summary>
/// Helepr class for measurement conversions using diverse units like cm, Twips, inch etc.
/// </summary>
public static class MeasurementHelper
{
    /// <summary>
    /// Size of a typograhic Point pt in cm
    /// </summary>
    public const double CentimeterPerPoint = 0.0352775;

    /// <summary>
    /// Twips per inch for typographic points
    /// </summary>
    public const double TwipsPerInchTypographicPoint = 1445.1;

    /// <summary>
    /// Twips per inch for PostScript(R) points
    /// </summary>
    public const double TwipsPerInchPostscriptPoint = 1440;

    /// <summary>
    /// Convert cm into Twips
    /// </summary>
    /// <param name="i">cm</param>
    /// <returns>Twips</returns>
    public static int GetTwipsFromCm(float i)
    {
        var result = i / 2.54f * TwipsPerInchTypographicPoint;
        return (int)Math.Ceiling(result);
    }

    /// <summary>
    /// Convert mm into Twips
    /// </summary>
    /// <param name="i">mm</param>
    /// <returns>Twips</returns>
    public static int GetTwipsFromMm(float i)
    {
        var result = i / 25.4f * TwipsPerInchTypographicPoint;
        return (int)Math.Ceiling(result);
    }

    /// <summary>
    /// Convert cm into Twips
    /// </summary>
    /// <param name="i">cm</param>
    /// <returns>Twips</returns>
    public static int GetTwipsFromCm(double i)
    {
        var result = i / 2.54f * TwipsPerInchTypographicPoint;
        return (int)Math.Ceiling(result);
    }

    /// <summary>
    /// Convert mm into Twips
    /// </summary>
    /// <param name="i">mm</param>
    /// <returns>Twips</returns>
    public static int GetTwipsFromMm(double i)
    {
        var result = i / 25.4f * TwipsPerInchTypographicPoint;
        return (int)Math.Ceiling(result);
    }

    /// <summary>
    /// Get Twips from typographic point pt
    /// </summary>
    /// <param name="pt">pt</param>
    /// <returns>Twips</returns>
    public static int GetTwipsFromPt(double pt)
    {
        return GetTwipsFromCm(pt * CentimeterPerPoint);
    }

    /// <summary>
    /// Get Twips from pixels px
    /// </summary>
    /// <param name="px">px</param>
    /// <returns>Twips</returns>
    public static int GetTwipsFromPx(int px)
    {
        return px * 15;
    }

    /// <summary>
    /// Get typographic points from cm
    /// </summary>
    /// <param name="cm">cm</param>
    /// <returns>typographic points pt</returns>
    public static double GetPtFromCm(double cm)
    {
        var value = cm / CentimeterPerPoint;
        return Math.Round(value, 1);
    }

    /// <summary>
    /// Get typographic points from mm
    /// </summary>
    /// <param name="mm">mm</param>
    /// <returns>typographic points pt</returns>
    public static double GetPtFromMm(double mm)
    {
        return Math.Round(mm / 10.0 / CentimeterPerPoint, 2);
    }

    /// <summary>
    /// Get pixel px from Twips
    /// </summary>
    /// <param name="twips">Twips</param>
    /// <returns>px</returns>
    public static int GetPxFromTwips(int twips)
    {
        return twips / 15;
    }

    /// <summary>
    /// Get cm from typographic points pt
    /// </summary>
    /// <param name="pt">Typographic points pt</param>
    /// <returns>cm</returns>
    public static double GetCmFromPt(double pt)
    {
        return Math.Round(pt * CentimeterPerPoint, 3);
    }

    /// <summary>
    /// Get picels px from typographic points
    /// </summary>
    /// <param name="pt">Typographic points</param>
    /// <returns>Pixels</returns>
    public static int GetPxFromPt(double pt)
    {
        return GetPxFromTwips(GetTwipsFromPt(pt))  ;
    }

    /// <summary>
    /// Get cm from twips
    /// </summary>
    /// <param name="twips">Twips</param>
    /// <returns>cm</returns>
    public static double GetCmFromTwips(int twips)
    {
        return twips / TwipsPerInchTypographicPoint * 2.54f;
    }

    /// <summary>
    /// Factor to convert DIU (px) to cm. Use cm = px/CMFactor.
    /// </summary>
    public const double CmFactor = 37.7952755905512;

    /// <summary>
    /// Convert DIU pixels to centimeters
    /// </summary>
    /// <param name="diu">DIU pixels</param>
    /// <returns>cm</returns>
    public static double GetCmFromDiu(double diu)
    {
        return diu / CmFactor;
    }

    /// <summary>
    /// Convert centimeters to DIU pixels
    /// </summary>
    /// <param name="cm">cm</param>
    /// <returns>DIU pixels</returns>
    public static double GetDiuFromCm(double cm)
    {
        return cm * CmFactor;
    }

    /// <summary>
    /// Convert typographic points to DIU
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static double GetDiuFromPoint(double point)
    {
        // 0.0352775 points per DIU
        return GetDiuFromCm(point * 0.0352775);
    }

    /// <summary>
    /// Get DIU from twips
    /// </summary>
    /// <param name="twips">Twips</param>
    /// <returns>DIU</returns>
    public static double GetDiuFromTwips(int twips)
    {
        var result = twips / TwipsPerInchTypographicPoint * 2.54f;
        return result * CmFactor;
    }

    /// <summary>
    /// Get EMU from pixel
    /// </summary>
    /// <param name="pixels">Pixels</param>
    /// <returns>EMU value</returns>
    public static long? GetEmuFromPx(int pixels)
    {
        return pixels * 9525;
    }

    /// <summary>
    /// Get EMU from cm
    /// </summary>
    /// <param name="cm">cm</param>
    /// <returns>EMU</returns>
    public static int GetEmuFromCm(double cm)
    {
        return (int)(cm * 360000);
    }

    /// <summary>
    /// Get EMU from pt
    /// </summary>
    /// <param name="pt">pt</param>
    /// <returns>EMU</returns>
    public static int GetEmuFromPt(double pt)
    {
        return (int)(pt * 12.700);
    }

    /// <summary>
    /// Get DXA (twentieths of a point) from cm (72dpi)
    /// </summary>
    /// <param name="cm"></param>
    /// <returns>DXA (twentieths of a point)</returns>
    public static uint GetDxaFromCm(double cm)
    {
        return (uint)(1 / 2.54F * 72F * 20F * cm);
    }

    /// <summary>
    /// Get DXA (twentieths of a point) from inch (72dpi)
    /// </summary>
    /// <param name="cm"></param>
    /// <returns>DXA (twentieths of a point)</returns>
    public static uint GetDxaFromInch(double cm)
    {
        return (uint)(72F * 20F * cm);
    }

    /// <summary>
    /// Get cm from pixels px
    /// </summary>
    /// <param name="px">Pixels px</param>
    /// <returns>cm</returns>
    public static double GetCmFromPx(double px)
    {
        var result = px / 2.54f * TwipsPerInchTypographicPoint;
        return Math.Round(result / 15f, 2);
    }

    /// <summary>
    /// Get pixels px from cm
    /// </summary>
    /// <param name="cm">cm</param>
    /// <returns>px</returns>
    public static int GetPxFromCm(double cm)
    {
        var result = cm / 2.54f * TwipsPerInchTypographicPoint / 15f;
        return (int)result;
    }
}
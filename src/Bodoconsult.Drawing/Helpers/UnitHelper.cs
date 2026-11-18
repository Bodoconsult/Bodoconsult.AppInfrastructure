// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Drawing.Helpers;

/// <summary>
/// Helper class for converting common units like ch, mm, or inch to pixels depending on the requested dots per inch (dpi)
/// </summary>
public static class UnitHelper
{
    /// <summary>
    /// Requested dots per inch. Default: 300
    /// </summary>
    public static double DotsPerInch { get; set; } = 300;

    /// <summary>
    /// Get pixels from cm
    /// </summary>
    /// <param name="cm">Centimeter</param>
    /// <returns>Pixels</returns>
    public static int GetPixelsFromCm(double cm)
    {
        return (int)(DotsPerInch / 2.54 * cm);
    }

    /// <summary>
    /// Get pixels from inch
    /// </summary>
    /// <param name="inch">Inch</param>
    /// <returns>Pixels</returns>
    public static int GetPixelsFromInch(double inch)
    {
        return (int)(DotsPerInch * inch);
    }
}
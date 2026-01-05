// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Style for <see cref="Info"/> instances
/// </summary>
public class InfoStyle : ParagraphStyleBase
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public InfoStyle()
    {
        TagToUse = "InfoStyle";
        Name = TagToUse;
        BorderBrush = new SolidColorBrush(Styleset.DefaultColor);
        BorderThickness.Bottom = 0.5 * Styleset.DefaultBorderWidth;
        BorderThickness.Left =   0.5 * Styleset.DefaultBorderWidth;
        BorderThickness.Right = 0.5 * Styleset.DefaultBorderWidth;
        BorderThickness.Top = 0.5 * Styleset.DefaultBorderWidth;
        Paddings.Left = 0.5 * Styleset.DefaultPaddingWidth;
        Paddings.Right = 0.5 * Styleset.DefaultPaddingWidth;
        Paddings.Top = 0.5 * Styleset.DefaultPaddingWidth;
        Paddings.Bottom = 0.5 * Styleset.DefaultPaddingWidth;
        Margins.Top = MeasurementHelper.GetCmFromPt(1 * Styleset.DefaultFontSize);
        Margins.Bottom = MeasurementHelper.GetCmFromPt(1 * Styleset.DefaultFontSize);
        KeepTogether = true;
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Bodoconsult.App.Avalonia.Delegates.Converters;

/// <summary>
///  Convert a color to a solid brush
/// </summary>
public class ColorToBrushConverter : IValueConverter
{
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var brush = value is null ? new SolidColorBrush(Colors.Black) : new SolidColorBrush((Color)value);
        return brush;
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return Colors.Black;
        }

        return (value as SolidColorBrush)?.Color ?? Colors.Black;
    }
}
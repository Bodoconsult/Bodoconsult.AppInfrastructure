// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Extensions;

namespace Bodoconsult.App.Wpf.Converters;

/// <summary>
///  Convert a <see cref="TypoColor"/>> to a solid brush
/// </summary>
public class TypoColorToBrushConverter : IValueConverter
{
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TypoColor color)
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentException("Value is NOT a Bodoconsult.App.Abstractions.Interfaces.TypoColor as expected!");
        }

        var brush = new SolidColorBrush(color.ToColor());
        return brush;
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush brush)
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentException("Value is NOT a System.Window.Media.SolidColorBrush as expected!");
        }

        return brush.Color.ToTypoColor();
    }
}
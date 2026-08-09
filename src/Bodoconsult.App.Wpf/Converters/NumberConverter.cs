// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Globalization;
using System.Windows.Data;

namespace Bodoconsult.App.Wpf.Converters;

/// <summary>
/// Convert numbers for data binding: Format N2 (#,##0.00)
/// </summary>
public class NumberConverter : BaseConverter, IValueConverter
{
    //E.g. DB 0.042367 --> UI "4.24 %"

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        var format = parameter is null ? "N2" : (string)parameter;

        //var fraction = double.Parse(value.ToString());
        //return fraction.ToString("N2");

        return string.Format(culture, $"{{0:{format}}}", value);
    }

    //E.g. UI "4.2367 %" --> DB 0.042367
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return double.Parse(value.ToString(), culture.NumberFormat);
    }
}
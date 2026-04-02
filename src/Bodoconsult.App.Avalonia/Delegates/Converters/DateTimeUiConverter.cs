// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Globalization;
using Avalonia.Data.Converters;

namespace Bodoconsult.App.Avalonia.Delegates.Converters;

/// <summary>
/// Converter for DateTime values to UI string
/// Uses CurrentUICulture with the user interfaces settings
/// </summary>
//[ValueConversion(typeof(DateTime), typeof(string))]
public class DateTimeUiConverter : BaseConverter, IValueConverter
{
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return string.Empty;
        }
        var date = (DateTime)value;
        return date.ToString("d", Thread.CurrentThread.CurrentCulture);
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || value.Equals(string.Empty))
        {
            return DateTime.MinValue;
        }
        var strValue = value.ToString();
        return DateTime.TryParse(strValue, Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out var resultDateTime) ? resultDateTime : value;
    }
}
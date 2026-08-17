// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Globalization;
using Avalonia.Data.Converters;

namespace Bodoconsult.App.Avalonia.Delegates.Converters;

/// <summary>
/// Convert percentages (format P2) for data binding 
/// </summary>
public class PercentageConverter : BaseConverter, IValueConverter
{
    //E.g. DB 0.042367 --> UI "4.24 %"
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var fraction = double.Parse(value?.ToString() ?? "0");
        return fraction.ToString("P2");
    }

    //E.g. UI "4.2367 %" --> DB 0.042367
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        //Trim any trailing percentage symbol that the user MAY have included
        var valueWithoutPercentage = value?.ToString()?.TrimEnd(' ', '%') ?? "0";
        return double.Parse(valueWithoutPercentage) / 100;
    }
}
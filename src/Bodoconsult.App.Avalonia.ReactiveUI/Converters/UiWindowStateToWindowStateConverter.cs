// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Bodoconsult.App.Avalonia.Delegates.Converters;
using Bodoconsult.App.Avalonia.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Ui;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Converters;

/// <summary>
/// Converts an UIWindowState to a WindowState and back
/// </summary>
//[ValueConversion(typeof(UiWindowState), typeof(WindowState))]
public class UiWindowStateToWindowStateConverter : BaseConverter, IValueConverter
{

    #region IValueConverter Members

    /// <summary>
    /// Converts from IList string as content to a Avalonia FlowDocument.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not UiWindowState data)
        {
            throw new ArgumentException("Value is not of type UiWindowState");
        }

        return data.ToWindowState();
    }

    /// <summary>
    /// Converts the content of a Avalonia FlowDocument to a XAML markup string.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not WindowState data)
        {
            throw new ArgumentException("Value is not of type WindowState");
        }

        return data.ToUiWindowState();
    }

    #endregion
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.Wpf.Converters;
using Bodoconsult.App.Wpf.ReactiveUI.Extensions;

namespace Bodoconsult.App.Wpf.ReactiveUI.Converters;

/// <summary>
/// Converts an UIWindowState to a WindowState and back
/// </summary>
[ValueConversion(typeof(UiWindowState), typeof(WindowState))]
public class UiWindowStateToWindowStateConverter : BaseConverter, IValueConverter
{

    #region IValueConverter Members

    /// <summary>
    /// Converts from IList string as content to a WPF FlowDocument.
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
    /// Converts the content of a WPF FlowDocument to a XAML markup string.
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
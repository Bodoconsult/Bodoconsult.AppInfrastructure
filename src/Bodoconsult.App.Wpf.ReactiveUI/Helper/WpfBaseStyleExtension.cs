// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Windows.Markup;
using Bodoconsult.App.Wpf.Services;

namespace Bodoconsult.App.Wpf.ReactiveUI.Helper;

/// <summary>
/// Get styles from Bodoconsult.Wpf.Base assembly
/// </summary>
public class WpfBaseResourceExtension : MarkupExtension
{
    /// <summary>
    /// Resource key which we want to extract
    /// </summary>
    public string ResourceKey { get; set; } = string.Empty;
    /// <summary>
    /// Overriding base function which will return key from RD
    /// </summary>
    /// <param name="serviceProvider">Not used</param>
    /// <returns>Object from RD</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return ResourceFinderService.FindResource("pack://application:,,,/Bodoconsult.App.Wpf.ReactiveUI;component/Resources/Styling/LookAndFeel.xaml", ResourceKey);
    }
}
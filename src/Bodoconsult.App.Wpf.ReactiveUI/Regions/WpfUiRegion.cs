// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using System.Windows.Forms;
using Windows.Foundation;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Regions;

/// <summary>
/// <see cref="UiRegion"/> implementation for WPF
/// </summary>
public class WpfUiRegion : UiRegion
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="uiWindow">Current UI window</param>
    /// <param name="routedViewHost">Routed view host instance to register by its name as region name</param>
    public WpfUiRegion(UiWindow uiWindow, RoutedViewHost routedViewHost) : base(uiWindow, routedViewHost.Name)
    { }
}
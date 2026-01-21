// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Interfaces;

/// <summary>
/// <see cref="UiRegion"/> implementation for WPF
/// </summary>
public class WpfUiRegion : UiRegion
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="routedViewHost">Routed view host instance to register by its name as region name</param>
    /// <param name="regionManager">Current region manager</param>
    public WpfUiRegion(RoutedViewHost routedViewHost, IRegionManager? regionManager) : base(routedViewHost.Name, regionManager)
    { }
}
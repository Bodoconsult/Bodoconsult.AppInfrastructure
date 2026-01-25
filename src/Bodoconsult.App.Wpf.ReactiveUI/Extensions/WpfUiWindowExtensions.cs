// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.Wpf.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IRegionManager"/>
/// </summary>
public static class WpfUiWindowExtensions
{
    /// <summary>
    /// Create a <see cref="WpfUiRegion"/>
    /// </summary>
    /// <param name="uiWindow">Current UI window</param>
    /// <param name="routedViewHost">Current <see cref="RoutedViewHost"/> instance to build the region. The name of the control is used as app-wide unique region name!</param>
    /// <returns><see cref="WpfUiRegion"/> created and registered to region manager</returns>
    public static WpfUiRegion CreateWpfUiRegion(this UiWindow uiWindow, RoutedViewHost routedViewHost)
    {
        if (uiWindow == null)
        {
            throw new ArgumentNullException(nameof(uiWindow));
        }

        var region = new WpfUiRegion(uiWindow, routedViewHost);
        uiWindow.RegionManager.RegisterRegion(region);

        return region;
    }

}
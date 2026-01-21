// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;
using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IRegionManager"/>
/// </summary>
public static class RegionManagerExtensions
{
    /// <summary>
    /// Create a <see cref="WpfUiRegion"/>
    /// </summary>
    /// <param name="regionManager">Current region manager instance</param>
    /// <param name="routedViewHost">Current <see cref="RoutedViewHost"/> instance to build the region. The name of the control is used as app-wide unique region name!</param>
    /// <returns><see cref="WpfUiRegion"/> created and registered to region manager</returns>
    public static WpfUiRegion CreateWpfUiRegion(this IRegionManager? regionManager, RoutedViewHost routedViewHost)
    {
        if (regionManager == null)
        {
            throw new ArgumentNullException(nameof(regionManager));
        }

        var region = new WpfUiRegion(routedViewHost, regionManager);
        regionManager.RegisterRegion(region);
        return region;
    }
}
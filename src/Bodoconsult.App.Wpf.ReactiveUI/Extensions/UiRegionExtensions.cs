// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;
using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IRegionManager"/>
/// </summary>
public static class UiRegionExtensions
{
    /// <summary>
    /// Navigate to a view in the given region
    /// </summary>
    /// <typeparam name="T">Type of viewmodel</typeparam>
    /// <param name="region">Current region</param>
    /// <param name="viewModel">Current viewmodel</param>
    public static void Navigate<T>(this UiRegion region, T viewModel) where T : class, IRoutableViewModel
    {
        region.RegionManager?.Navigate(region.RegionName, viewModel);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.ReactiveUI.Extensions;

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
        if (region.UiWindow.RegionManager is null)
        {
            throw new ArgumentNullException(nameof(region.UiWindow.RegionManager));
        }

        region.UiWindow.RegionManager.Navigate(region, viewModel);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using ReactiveUI;

namespace Bodoconsult.App.ReactiveUI.Regions;

/// <summary>
/// Current <see cref="IRegionManager"/> implementation to handle navigation to different regions in a window
/// </summary>
public class RegionManager : IRegionManager
{
    /// <summary>
    /// Current regions loaded
    /// </summary>
    public Dictionary<string, UiRegion> Regions { get; } = new();

    /// <summary>
    /// Register a region
    /// </summary>
    /// <param name="region">Region to register</param>
    public void RegisterRegion(UiRegion region)
    {
        Regions.Add(region.RegionName, region);
    }

    /// <summary>
    /// Navigate in a region by its name
    /// </summary>
    /// <typeparam name="T">Viewmodel type</typeparam>
    /// <param name="regionName">Region name</param>
    /// <param name="viewModel">Viewmodel</param>
    public void Navigate<T>(string regionName, T viewModel) where T : class, IRoutableViewModel
    {
        if (viewModel == null)
        {
            throw new ArgumentNullException(nameof(viewModel));
        }

        if (!Regions.TryGetValue(regionName, out var region))
        {
            return;
        }

        region.Router.Navigate.Execute(viewModel);
    }

    /// <summary>
    /// Navigate in a region by its name
    /// </summary>
    /// <typeparam name="T">Viewmodel type</typeparam>
    /// <param name="region">Region instance to navigate in</param>
    /// <param name="viewModel">Viewmodel</param>
    public void Navigate<T>(UiRegion region, T viewModel) where T : class, IRoutableViewModel
    {
        if (region == null)
        {
            throw new ArgumentNullException(nameof(region));
        }

        if (viewModel == null)
        {
            throw new ArgumentNullException(nameof(viewModel));
        }
        
        region.Router.Navigate.Execute(viewModel);
    }
}
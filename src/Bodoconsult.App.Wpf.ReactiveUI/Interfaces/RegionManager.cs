// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Interfaces;

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
}
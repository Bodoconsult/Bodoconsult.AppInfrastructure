// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IUiWindow"/>
/// </summary>
public static class IUiWindowExtensions
{
    /// <summary>
    /// Create a region
    /// </summary>
    /// <param name="window">Current UI window</param>
    /// <param name="regionName">Name of the region</param>
    /// <returns>Newly created <see cref="UiRegion"/> instance</returns>
    public static UiRegion? CreateUiRegion(this IUiWindow window, string regionName)
    {

        ArgumentNullException.ThrowIfNull(window.RegionManager);
        //if (window.RegionManager == null)
        //{
        //    throw new ArgumentNullException(nameof(window.RegionManager));
        //}

        if (window.UiRegions.Any(x => x.RegionName == $"{window.InstanceName}.{regionName}"))
        {
            return null;
        }
        
        var region = new UiRegion(window, regionName);
        window.RegionManager.RegisterRegion(region);
        window.UiRegions.Add(region);
        return region;
    }

    /// <summary>
    /// Find a region by name
    /// </summary>
    /// <param name="window">Current UI window</param>
    /// <param name="regionName">Region name to search for</param>
    /// <returns>Region or null if no region with the requested name was found</returns>
    public static UiRegion? FindRegion(this IUiWindow window, string regionName)
    {
        return window.UiRegions.FirstOrDefault(x => x.RegionName == $"{window.InstanceName}.{regionName}");
    }
}
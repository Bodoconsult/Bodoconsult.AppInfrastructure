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
    /// Current UI regions loaded
    /// </summary>
    public Dictionary<string, UiRegion> Regions { get; } = new();

    /// <summary>
    /// Current UI windows loaded
    /// </summary>
    public Dictionary<string, UiWindow> Windows { get; } = new();

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

    public UiWindow RegisterWindow(UiWindow window)
    {
        if (!Windows.TryAdd(window.WindowName, window))
        {
            throw new ArgumentException($"Window name {window.WindowName} already exists!");
        }

        return window;
    }

    /// <summary>
    /// Dispose the UI window and its regions
    /// </summary>
    /// <param name="uiWindow"></param>
    public void Dispose(UiWindow uiWindow)
    {

        var regionsToDelete = Regions.Where(x => x.Value.UiWindow == uiWindow).ToList();

        foreach (var region in regionsToDelete)
        {
            if (Regions.ContainsKey(region.Key) && !Regions.Remove(region.Key))
            {
                throw new ArgumentException($"Region {region.Key} could NOT be deleted!");
            }
        }

        if (Windows.ContainsKey(uiWindow.WindowName) && !Windows.Remove(uiWindow.WindowName))
        {
            throw new ArgumentException($"Window {uiWindow.WindowName} could NOT be deleted!");
        }
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for region managers to handle navigation to different regions in a window
/// </summary>
public interface IRegionManager
{
    /// <summary>
    /// Current regions loaded
    /// </summary>
    Dictionary<string, UiRegion> Regions { get; }

    /// <summary>
    /// Register a region
    /// </summary>
    /// <param name="region">Region to register</param>
    void RegisterRegion(UiRegion region);

    /// <summary>
    /// Navigate in a region by its name
    /// </summary>
    /// <typeparam name="T">Viewmodel type</typeparam>
    /// <param name="regionName">Region name</param>
    /// <param name="viewModel">Viewmodel</param>
    void Navigate<T>(string regionName, T viewModel) where T : class, IRoutableViewModel;

    /// <summary>
    /// Navigate in a region by its name
    /// </summary>
    /// <typeparam name="T">Viewmodel type</typeparam>
    /// <param name="region">Region instance to navigate in</param>
    /// <param name="viewModel">Viewmodel</param>
    void Navigate<T>(UiRegion region, T viewModel) where T : class, IRoutableViewModel;
}
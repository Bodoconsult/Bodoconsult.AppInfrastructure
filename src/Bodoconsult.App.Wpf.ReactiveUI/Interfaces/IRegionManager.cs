// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Interfaces;

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

    void Navigate<T>(string regionName, T viewModel) where T : class, IRoutableViewModel;
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Regions;

/// <summary>
/// Represents a UI window
/// </summary>
public class UiWindow
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="windowName">Name of the window to register</param>
    /// <param name="regionManager">Current region manager</param>
    public UiWindow(string windowName, IRegionManager regionManager)
    {
        WindowName = windowName;
        RegionManager = regionManager;
    }

    /// <summary>
    /// Window name
    /// </summary>
    public string WindowName { get; }

    /// <summary>
    /// Current region manager
    /// </summary>
    public IRegionManager RegionManager { get; }

    /// <summary>
    /// Region in the current window
    /// </summary>
    public List<UiRegion> UiRegions { get; } = new();
}
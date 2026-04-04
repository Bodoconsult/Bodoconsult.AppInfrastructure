// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for ReactiveUI based windows providing regions
/// </summary>
public interface IUiWindow
{
    /// <summary>
    /// Window instance name
    /// </summary>
    string? InstanceName { get; set; }

    /// <summary>
    /// Current region manager
    /// </summary>
    IRegionManager? RegionManager { get; }

    /// <summary>
    /// Region in the current window
    /// </summary>
    List<UiRegion> UiRegions { get; }

    /// <summary>
    /// Dispose this window from region manager
    /// </summary>
    /// <param name="sender">Do not use</param>
    /// <param name="e">Do not use</param>
    void Dispose(object? sender, EventArgs e);

    /// <summary>
    /// Load the region manager
    /// </summary>
    /// <param name="regionManager">Current region manager instance</param>
    void LoadRegionManager(IRegionManager regionManager);
}
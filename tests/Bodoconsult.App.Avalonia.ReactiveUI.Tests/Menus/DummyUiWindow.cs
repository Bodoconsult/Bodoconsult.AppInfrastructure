// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.ReactiveUI.Tests.Menus;

/// <summary>
/// Dummy implementation of <see cref="IUiWindow"/>
/// </summary>
public class DummyUiWindow : IUiWindow
{
    /// <summary>
    /// Window name
    /// </summary>
    public string? Name { get; set; } = "Test";

    /// <summary>
    /// Window instance name
    /// </summary>
    public string? InstanceName { get; set; } = "Test";

    /// <summary>
    /// Current region manager
    /// </summary>
    public IRegionManager? RegionManager { get; set; }

    /// <summary>
    /// Region in the current window
    /// </summary>
    public List<UiRegion> UiRegions { get; set; } = [];

    /// <summary>
    /// Dispose this window from region manager
    /// </summary>
    /// <param name="sender">Do not use</param>
    /// <param name="e">Do not use</param>
    public void Dispose(object? sender, EventArgs e)
    {
        // Do nothing
    }

    /// <summary>
    /// Load the region manager
    /// </summary>
    /// <param name="regionManager">Current region manager instance</param>
    public void LoadRegionManager(IRegionManager regionManager)
    {
        RegionManager = regionManager;
    }
}
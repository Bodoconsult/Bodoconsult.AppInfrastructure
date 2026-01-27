// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.ReactiveUI.Tests;

/// <summary>
/// Dummy implementation of <see cref="IUiWindow"/>
/// </summary>
public class DummyUiWindow : IUiWindow
{
    /// <summary>
    /// Window name
    /// </summary>
    public string Name { get; set; } = "Test";

    /// <summary>
    /// Current region manager
    /// </summary>
    public IRegionManager? RegionManager { get; set; }

    /// <summary>
    /// Region in the current window
    /// </summary>
    public List<UiRegion> UiRegions { get; set; } = new List<UiRegion>();

    /// <summary>
    /// Dispose this window from region manager
    /// </summary>
    /// <param name="sender">Do not use</param>
    /// <param name="e">Do not use</param>
    public void Dispose(object? sender, EventArgs e)
    {
        // Do nothing
    }
}
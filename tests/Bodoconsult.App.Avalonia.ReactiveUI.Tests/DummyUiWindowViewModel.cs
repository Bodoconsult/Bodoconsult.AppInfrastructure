// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.ReactiveUI.Tests;

/// <summary>
/// Dummy implementation of <see cref="IUiWindowViewModel"/>
/// </summary>
public class DummyUiWindowViewModel : IUiWindowViewModel
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="regionManager">Current region manager</param>
    public DummyUiWindowViewModel(IRegionManager regionManager)
    {
        RegionManager = regionManager;
    }

    /// <summary>
    /// Instance name of the window. If null or string.Empty the window instance name is derived from the window type name (loading the window as a singleton instance)
    /// </summary>
    public string? InstanceName { get; set; }

    /// <summary>
    /// Current region manager
    /// </summary>
    public IRegionManager RegionManager { get; set; }

    /// <summary>
    /// Region 1
    /// </summary>
    public UiRegion? Region1 { get; set; }

    /// <summary>
    /// Region 2
    /// </summary>
    public UiRegion? Region2 { get; set; }

    /// <summary>
    /// Region 3
    /// </summary>
    public UiRegion? Region3 { get; set; }
}
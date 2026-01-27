// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Basic features for navigatable windows with regions
/// </summary>
public interface IUiWindowViewModel
{
    /// <summary>
    /// Instance name of the window. If null or string.Empty the window instance name is derived from the window type name (loading the window as a singleton instance)
    /// </summary>
    string? InstanceName { get; set; }

    /// <summary>
    /// Current region manager
    /// </summary>
    IRegionManager RegionManager { get; }

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
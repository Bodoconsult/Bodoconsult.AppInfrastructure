// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.Wpf.Interfaces;

namespace Bodoconsult.App.Wpf.ReactiveUI.Interfaces;

/// <summary>
/// Basic features for navigatable windows with regions
/// </summary>
public interface IReactiveUiWindowViewModel
{
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
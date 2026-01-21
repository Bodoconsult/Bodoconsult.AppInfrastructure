// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.Interfaces;

namespace Bodoconsult.App.Wpf.ReactiveUI.Interfaces;

/// <summary>
/// <see cref="IMainWindowViewModel"/> with enhancements for ReactiveUI usage
/// </summary>
public interface IReactiveUiMainWindowViewModel : IMainWindowViewModel
{
    /// <summary>
    /// Current region manager
    /// </summary>
    IRegionManager? RegionManager { get; }

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
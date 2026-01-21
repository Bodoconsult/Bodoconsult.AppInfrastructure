// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Interfaces;

/// <summary>
/// Region of the UI
/// </summary>
public class UiRegion : ReactiveObject, IScreen
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="regionName">Name of the region to register</param>
    /// <param name="regionManager">Current region manager</param>
    public UiRegion(string regionName, IRegionManager? regionManager)
    {
        RegionName = regionName;
        Router = new RoutingState();
        RegionManager = regionManager;
    }

    public string RegionName { get; }

    /// <summary>Gets the Router associated with this Screen.</summary>
    public RoutingState Router { get; }

    /// <summary>
    /// Current region manager
    /// </summary>
    public IRegionManager? RegionManager { get; }
}
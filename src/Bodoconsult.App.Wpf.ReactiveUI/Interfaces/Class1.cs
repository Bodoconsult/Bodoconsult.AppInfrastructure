// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Interfaces;

/// <summary>
/// Region of the UI
/// </summary>
public class UiRegion
{
    public UiRegion(string regionName)
    {
        RegionName = regionName;
        Router = new RoutingState();
    }


    public string RegionName { get;  }


    public RoutingState Router { get; }

}

public class WpfUiRegion : UiRegion
{
    public WpfUiRegion(RoutedViewHost routedViewHost) : base(routedViewHost.Name)
    {
        routedViewHost.Router = Router;
    }
}


public interface IRegionManager
{

    Dictionary<string, UiRegion> Regions { get; }

    void RegisterRegion(UiRegion region);

    void Navigate<T>(string regionName, T viewModel) where T : class, IRoutableViewModel;
}

public class RegionManager: IRegionManager
{

    public Dictionary<string, UiRegion> Regions { get; } = new();

    public void RegisterRegion(UiRegion region)
    {
        Regions.Add(region.RegionName ,region);
    }

    public void Navigate<T>(string regionName, T viewModel) where T : class, IRoutableViewModel
    {
        if (viewModel == null)
        {
            throw new ArgumentNullException(nameof(viewModel));
        }

        if (!Regions.TryGetValue(regionName, out var region))
        {
            return;
        }

        region.Router.Navigate.Execute(viewModel);
    }
}
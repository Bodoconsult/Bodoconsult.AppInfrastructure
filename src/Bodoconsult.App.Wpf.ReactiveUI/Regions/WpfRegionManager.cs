// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.Wpf.Helpers;
using ReactiveUI;
using System.Windows;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Disposables;

namespace Bodoconsult.App.Wpf.ReactiveUI.Regions;

/// <summary>
/// WPF implementation of <see cref="IRegionManager"/>
/// </summary>
public class WpfRegionManager : RegionManagerBase
{
    /// <summary>
    /// Register a WPF window inheriting IUiWindow to the region manager
    /// </summary>
    /// <typeparam name="T">Window type</typeparam>
    /// <typeparam name="TViewModel">Viewmodel type for the window implementing <see cref="IUiWindowViewModel"/></typeparam>
    /// <param name="window">Current window instance</param>
    /// <param name="disposables">Disposables</param>
    /// <returns><see cref="IUiWindow"/> instance</returns>
    /// <exception cref="ArgumentNullException">Thrown if there are no RoutedViewHost controls in the window or a region name is not defined in the window</exception>
    public IUiWindow RegisterInstances<T, TViewModel>(T window, MultipleDisposable disposables) where T : ReactiveWindow<TViewModel>, IUiWindow where TViewModel : class, IUiWindowViewModel
    {
        var type = typeof(T);

        var wwd = InternalWindows.FirstOrDefault(x => x.WindowType == type);

        if (wwd.WindowType == null)
        {
            throw new ArgumentNullException(nameof(wwd), $"No window definition found for {type.Name}");
        }

        // Set the instance name for the window now. Must be unique in the RegionManagerBase.Windows dictionary
        var instanceName = string.IsNullOrEmpty(window.ViewModel?.InstanceName) ? window.GetType().Name : window.ViewModel?.InstanceName ?? window.GetType().Name;

        // Now register the window
        var uiWindow = RegisterWindowInstances(window, instanceName);
        window.Closed += uiWindow.Dispose;

        // Find the regions now
        FindRegions(uiWindow, wwd);

        return uiWindow;
    }

    /// <summary>
    /// Navigate to a new or already opened window
    /// </summary>
    /// <typeparam name="TWindowViewModel">Type of the window view model</typeparam>
    /// <typeparam name="TViewModel">View model of the view to load</typeparam>
    /// <param name="windowViewModel">Current window viewmodel instance</param>
    /// <param name="viewModel">Current view viewmodel instance</param>
    /// <param name="regionName">Region name to load the view in</param>
    /// <returns><see cref="IUiWindow"/> instance the view is loaded in</returns>
    public override IUiWindow Navigate<TWindowViewModel, TViewModel>(TWindowViewModel windowViewModel, TViewModel viewModel, string regionName)
    {
        var uiWindow = base.Navigate(windowViewModel, viewModel, regionName);

        if (uiWindow is not ReactiveWindow<TWindowViewModel> reactiveWindow)
        {
            throw new ArgumentException($"View {uiWindow.GetType().Name} is not a ReactiveWindow instance as expected");
        }

        reactiveWindow.ViewModel = windowViewModel;
        reactiveWindow.Focus();
        reactiveWindow.Show();

        // Activate navigation to target region now
        SubscribeExtensions.Subscribe(
            reactiveWindow.WhenAnyValue(x => x.IsLoaded).ObserveOn(RxSchedulers.MainThreadScheduler), x =>
            {
                var region = uiWindow.FindRegion(regionName);

                ArgumentNullException.ThrowIfNull(region, $"Region {regionName} not found");

                if (viewModel is not IUiRegionViewModel uvm)
                {
                    throw new ArgumentException(
                        $"Viewmodel {viewModel.GetType().Name} does not implement IUiRegionViewModel as expected");
                }

                uvm.InjectScreen(region);

                region.Router.Navigate.Execute(viewModel);
            });

        return uiWindow;
    }

    /// <summary>
    /// Find the regions for an existing window instance
    /// </summary>
    /// <param name="window">Window</param>
    /// <param name="wwd">UI window definition</param>
    public override void FindRegions(IUiWindow window, UiWindowDefinition wwd)
    {
        if (window is not Window w)
        {
            throw new ArgumentException($"window must be of type Window but was {window.GetType().Name}");
        }

        var childs = WpfHelper.FindLogicalChildren<RoutedViewHost>(w).ToList();

        ArgumentNullException.ThrowIfNull(childs, $"No region childs defined for {w.GetType().Name}");

        if (window.UiRegions.Count != 0)
        {
            return;
        }

        // Now register the regions for the window
        foreach (var regionName in wwd.Regions)
        {
            var regionContainer = childs.FirstOrDefault(x => x.Name == regionName);

            ArgumentNullException.ThrowIfNull(regionContainer, $"Region {regionName} not found");

            // Register region if not registered already
            ArgumentNullException.ThrowIfNull(regionContainer.Name);
            window.CreateUiRegion(regionContainer.Name);

        }
    }
}
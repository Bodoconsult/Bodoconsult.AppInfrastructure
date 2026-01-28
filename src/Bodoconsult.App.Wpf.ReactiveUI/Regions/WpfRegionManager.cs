// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.Wpf.Helpers;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;

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
    public IUiWindow RegisterInstances<T, TViewModel>(T window, CompositeDisposable disposables) where T : ReactiveWindow<TViewModel>, IUiWindow where TViewModel : class, IUiWindowViewModel
    {
        var type = typeof(T);

        var wwd = InternalWindows.FirstOrDefault(x => x.WindowType == type);

        if (wwd.WindowType == null)
        {
            throw new ArgumentNullException(nameof(wwd), $"No window definition found for {type.Name}");
        }

        // Set the instance name for the window now. Must be unique in the RegionManagerBase.Windows dictionary
        if (string.IsNullOrEmpty(window.ViewModel?.InstanceName))
        {
            if (string.IsNullOrEmpty(window.Name))
            {
                window.Name = window.GetType().Name;
            }
        }
        else
        {
            window.Name = window.ViewModel?.InstanceName;
        }

        // Find regions in the window
        var childs = WpfHelper.FindVisualChildren<RoutedViewHost>(window).ToList();

        if (childs == null || childs.Count == 0)
        {
            // ReSharper disable once LocalizableElement
            throw new ArgumentNullException(nameof(childs), $"No region childs defined for {type.Name}");
        }

        // Now register the window
        var uiWindow = RegisterWindowInstances(window);
        window.Closed += uiWindow.Dispose;

        if (window.UiRegions.Count != 0)
        {
            return uiWindow;
        }


        // Now register the regions for the window
        foreach (var regionName in wwd.Regions)
        {
            var regionContainer = childs.FirstOrDefault(x => x.Name == regionName);

            if (regionContainer == null)
            {
                throw new ArgumentNullException(nameof(regionName), regionName);
            }

            // Register region if not registered already
            uiWindow.CreateUiRegion(regionContainer.Name);
            
        }

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
        reactiveWindow.WhenAnyValue(x => x.IsLoaded).ObserveOn(RxSchedulers.MainThreadScheduler).Subscribe(x =>
        {
            var region = uiWindow.FindRegion(regionName);

            if (region == null)
            {
                throw new ArgumentNullException(nameof(region), $"Region {regionName} not found");
            }

            if (viewModel is not IUiRegionViewModel uvm)
            {
                throw new ArgumentException($"Viewmodel {viewModel.GetType().Name} does not implement IUiRegionViewModel as expected");
            }

            uvm.InjectScreen(region);

            region.Router.Navigate.Execute(viewModel);
        });

        return uiWindow;
    }
}
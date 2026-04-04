// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using ReactiveUI;
using System.Collections.Concurrent;

namespace Bodoconsult.App.ReactiveUI.Regions;

/// <summary>
/// Current <see cref="IRegionManager"/> base implementation to handle navigation to different regions in a window
/// </summary>
public abstract class RegionManagerBase : IRegionManager
{
    /// <summary>
    /// ViewModel-View-Binding
    /// </summary>
    protected readonly ConcurrentDictionary<Type, Type> InternalViewModelBindings = new();

    /// <summary>
    /// Registered window definitions
    /// </summary>
    protected readonly List<UiWindowDefinition> InternalWindows = [];

    /// <summary>
    /// Readonly list of all registered viewmodel-view-bindings
    /// </summary>
    public List<KeyValuePair<Type, Type>> ViewModelBindings => InternalViewModelBindings.ToList();

    /// <summary>
    /// Readonly list of all registered window type definitions
    /// </summary>
    public List<UiWindowDefinition> WindowDefinitions => InternalWindows.ToList();

    /// <summary>
    /// Current UI regions loaded
    /// </summary>
    public Dictionary<string, UiRegion> Regions { get; } = new();

    /// <summary>
    /// Current UI windows loaded
    /// </summary>
    public Dictionary<string, IUiWindow> Windows { get; } = new();

    /// <summary>
    /// Register a window type
    /// </summary>
    /// <typeparam name="T">Window type implementing <see cref="IUiWindow"/></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <param name="regions"></param>
    /// <param name="factory"></param>
    public void RegisterWindow<T, TViewModel>(List<string> regions, Func<IUiWindow>? factory)
        where T : class, IUiWindow
        where TViewModel : IUiWindowViewModel
    {
        var windowType = typeof(T);

        if (WindowDefinitions.Any(x => x.WindowType == windowType))
        {
            throw new ArgumentException($"Window type {windowType.Name} is already registered");
        }

        var wwd = new UiWindowDefinition(windowType, regions, factory);
        InternalWindows.Add(wwd);

        InternalViewModelBindings[typeof(TViewModel)] = typeof(T);
    }

    /// <summary>
    /// Register a region
    /// </summary>
    /// <param name="region">Region to register</param>
    public void RegisterRegion(UiRegion region)
    {
        if (!Regions.TryAdd(region.RegionName, region))
        {
            throw new ArgumentException($"Window type {region.RegionName} is already registered");
        }
    }

    ///// <summary>
    ///// Navigate in a region by its name
    ///// </summary>
    ///// <typeparam name="T">Viewmodel type</typeparam>
    ///// <param name="regionName">Region name</param>
    ///// <param name="viewModel">Viewmodel</param>
    //public void Navigate<T>(string regionName, T viewModel) where T : class, IRoutableViewModel
    //{
    //    if (viewModel == null)
    //    {
    //        throw new ArgumentNullException(nameof(viewModel));
    //    }

    //    if (!Regions.TryGetValue(regionName, out var region))
    //    {
    //        return;
    //    }

    //    region.Router.Navigate.Execute(viewModel);
    //}

    /// <summary>
    /// Navigate in a region by its name
    /// </summary>
    /// <typeparam name="T">Viewmodel type</typeparam>
    /// <param name="region">Region instance to navigate in</param>
    /// <param name="viewModel">Viewmodel</param>
    public void Navigate<T>(UiRegion region, T viewModel) where T : class, IRoutableViewModel
    {
        ArgumentNullException.ThrowIfNull(region);
        ArgumentNullException.ThrowIfNull(viewModel);

        region.Router.Navigate.Execute(viewModel);
    }


    /// <summary>
    /// Register a window instance
    /// </summary>
    /// <param name="window">Window to register</param>
    /// <param name="instanceName">The name of the window instance</param>
    /// <returns>The registered window</returns>
    public IUiWindow RegisterWindowInstances(IUiWindow window, string instanceName)
    {
        window.InstanceName = instanceName;

        if (Windows.ContainsKey(instanceName) || Windows.TryAdd(instanceName, window))
        {
            return window;
        }

        throw new ArgumentException($"Window name {instanceName} already exists!");
    }

    /// <summary>
    /// Dispose the UI window and its regions
    /// </summary>
    /// <param name="uiWindow"></param>
    public void Dispose(IUiWindow uiWindow)
    {
        var regionsToDelete = Regions.Where(x => x.Value.UiWindow == uiWindow).ToList();

        foreach (var region in regionsToDelete)
        {
            if (Regions.ContainsKey(region.Key) && !Regions.Remove(region.Key))
            {
                throw new ArgumentException($"Region {region.Key} could NOT be deleted!");
            }
        }

        var instance = Windows.ToList().FirstOrDefault(x => x.Value == uiWindow);

        if (Windows.ContainsValue(uiWindow) && !Windows.Remove(instance.Key))
        {
            throw new ArgumentException($"Window {uiWindow.InstanceName} could NOT be deleted!");
        }
    }

    /// <summary>
    /// Get a UI window by name
    /// </summary>
    /// <param name="windowName">Window name</param>
    /// <returns><see cref="IUiWindow"/> instance or null if no window with the request name was found</returns>
    public IUiWindow? GetUiWindow(string windowName)
    {
        return Windows.GetValueOrDefault(windowName);
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
    public virtual IUiWindow Navigate<TWindowViewModel, TViewModel>(TWindowViewModel windowViewModel, TViewModel viewModel, string regionName) where TWindowViewModel : class, IUiWindowViewModel where TViewModel : IUiRegionViewModel
    {
        var vmType = typeof(TWindowViewModel);

        // Find the window type for the view
        if (!InternalViewModelBindings.TryGetValue(vmType, out var windowType))
        {
            throw new ArgumentException($"Viewmodel {vmType.Name} not registered with view");
        }

        // Find the factory for the window type
        var wwd = InternalWindows.FirstOrDefault(x => x.WindowType == windowType);
        if (wwd.WindowType == null)
        {
            throw new ArgumentException($"Window {windowType.Name} has no window definition registered");
        }

        IUiWindow? uiWindow;

        if (wwd.Factory == null)
        {
            // Try to use existing instance
            if (!Windows.TryGetValue(windowType.Name, out uiWindow))
            {
                ArgumentNullException.ThrowIfNull(wwd.Factory);
            }
        }
        else
        {
            var instanceName = string.IsNullOrEmpty(windowViewModel.InstanceName) ? windowType.Name : windowViewModel.InstanceName;

            if (!Windows.TryGetValue(instanceName, out uiWindow))
            {
                // Create the window now
                uiWindow = wwd.Factory.Invoke();
                uiWindow.InstanceName = instanceName;
            }
        }

        ArgumentNullException.ThrowIfNull(uiWindow);

        uiWindow.LoadRegionManager(this);

        // Now find the regions
        FindRegions(uiWindow, wwd);

        return uiWindow;
    }


    /// <summary>
    /// Find the regions for an existing window instance
    /// </summary>
    /// <param name="window">Window</param>
    /// <param name="wwd">UI window definition</param>
    public virtual void FindRegions(IUiWindow window, UiWindowDefinition wwd)
    {
        throw new NotSupportedException();
    }
}
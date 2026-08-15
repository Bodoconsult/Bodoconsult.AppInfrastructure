// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for region managers to handle navigation to different regions in a window
/// </summary>
public interface IRegionManager
{
    /// <summary>
    /// Readonly list of all viewmodel-view-bindings
    /// </summary>
    IReadOnlyList<KeyValuePair<Type, Type>> ViewModelBindings { get; }

    /// <summary>
    /// Readonly list of all registered window type definitions
    /// </summary>
    IReadOnlyList<UiWindowDefinition> WindowDefinitions { get; }

    /// <summary>
    /// Current UI regions loaded
    /// </summary>
    Dictionary<string, UiRegion> Regions { get; }

    /// <summary>
    ///  Current UI window loaded
    /// </summary>
    Dictionary<string, IUiWindow> Windows { get; }

    /// <summary>
    /// Register a window type
    /// </summary>
    /// <typeparam name="T">Window type implementing <see cref="IUiWindow"/></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <param name="regions"></param>
    /// <param name="factory"></param>
    void RegisterWindow<T, TViewModel>(List<string> regions, Func<IUiWindow>? factory)
        where T : class, IUiWindow
        where TViewModel : IUiWindowViewModel;

    /// <summary>
    /// Register a region
    /// </summary>
    /// <param name="region">Region to register</param>
    void RegisterRegion(UiRegion region);

    ///// <summary>
    ///// Navigate in a region by its name
    ///// </summary>
    ///// <typeparam name="T">Viewmodel type</typeparam>
    ///// <param name="regionName">Region name</param>
    ///// <param name="viewModel">Viewmodel</param>
    //void Navigate<T>(string regionName, T viewModel) where T : class, IRoutableViewModel;

    /// <summary>
    /// Navigate in a region by its name
    /// </summary>
    /// <typeparam name="T">Viewmodel type</typeparam>
    /// <param name="region">Region instance to navigate in</param>
    /// <param name="viewModel">Viewmodel</param>
    void Navigate<T>(UiRegion region, T viewModel) where T : class, IRoutableViewModel;

    /// <summary>
    /// Register a window instance
    /// </summary>
    /// <param name="window">Window to register</param>
    /// <param name="instanceName">The name of the window instance</param>
    /// <returns>The registered window</returns>
    IUiWindow RegisterWindowInstances(IUiWindow window, string instanceName);

    /// <summary>
    /// Dispose the UI window and its regions
    /// </summary>
    /// <param name="uiWindow"></param>
    void Dispose(IUiWindow uiWindow);

    /// <summary>
    /// Get a UI window by name
    /// </summary>
    /// <param name="windowName">Window name</param>
    /// <returns><see cref="IUiWindow"/> instance or null if no window with the request name was found</returns>
    IUiWindow? GetUiWindow(string windowName);

    /// <summary>
    /// Navigate to a new or already opened window
    /// </summary>
    /// <typeparam name="TWindowViewModel">Type of the window view model</typeparam>
    /// <typeparam name="TViewModel">View model of the view to load</typeparam>
    /// <param name="windowViewModel">Current window viewmodel instance</param>
    /// <param name="viewModel">Current view viewmodel instance</param>
    /// <param name="regionName">Region name to load the view in</param>
    /// <returns><see cref="IUiWindow"/> instance the view is loaded in</returns>
    IUiWindow Navigate<TWindowViewModel, TViewModel>(TWindowViewModel windowViewModel, TViewModel viewModel, string regionName) where TWindowViewModel : class, IUiWindowViewModel where TViewModel: IUiRegionViewModel;

    /// <summary>
    /// Find the regions for an existing window instance
    /// </summary>
    /// <param name="window">Window</param>
    /// <param name="wwd">UI window definition</param>
    void FindRegions(IUiWindow window, UiWindowDefinition wwd);
}
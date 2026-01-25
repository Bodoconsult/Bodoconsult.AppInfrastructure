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
    /// Current UI regions loaded
    /// </summary>
    Dictionary<string, UiRegion> Regions { get; }

    /// <summary>
    ///  Current UI window loaded
    /// </summary>
    Dictionary<string, UiWindow> Windows { get; }

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
    /// Register a window
    /// </summary>
    /// <param name="window">Window to register</param>
    /// <returns>The registered window</returns>
    UiWindow RegisterWindow(UiWindow window);

    /// <summary>
    /// Dispose the UI window and its regions
    /// </summary>
    /// <param name="uiWindow"></param>
    void Dispose(UiWindow uiWindow);

    /// <summary>
    /// Get a UI window by name
    /// </summary>
    /// <param name="windowName">Window name</param>
    /// <returns><see cref="UiWindow"/> instance or null if no window with the request name was found</returns>
    UiWindow? GetUiWindow(string windowName);

    void Navigate<T>(T viewModel, string regionName) where T : class;
}
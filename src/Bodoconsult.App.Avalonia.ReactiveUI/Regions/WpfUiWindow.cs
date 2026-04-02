// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System.Windows;
//using Bodoconsult.App.ReactiveUI.Interfaces;
//using Bodoconsult.App.ReactiveUI.Regions;
//using ReactiveUI;

//namespace Bodoconsult.App.Avalonia.ReactiveUI.Regions;

///// <summary>
///// <see cref="UiRegion"/> implementation for Avalonia
///// </summary>
//public class AvaloniaUiWindow : UiWindow
//{
//    /// <summary>
//    /// Default ctor. Registers the window with its Name property value
//    /// </summary>
//    /// <param name="window">Current window</param>
//    /// <param name="regionManager">Current region manager</param>
//    public AvaloniaUiWindow(Window window, IRegionManager regionManager) : base(window.Name, regionManager)
//    {
//        Window = window;
//    }

//    /// <summary>
//    /// Default ctor. Registers the window with a given name that must be unique in the <see cref="UiWindow"/>> collection
//    /// </summary>
//    /// <param name="window">Current window</param>
//    /// <param name="name">Name to register the window with</param>
//    /// <param name="regionManager">Current region manager</param>
//    public AvaloniaUiWindow(Window window, string name, IRegionManager regionManager) : base(name, regionManager)
//    {
//        Window = window;
//    }

//    /// <summary>
//    /// Current window instance
//    /// </summary>
//    public Window Window { get; }

//    /// <summary>
//    /// Dispose this window from region manager
//    /// </summary>
//    /// <param name="sender">Do not use</param>
//    /// <param name="e">Do not use</param>
//    public void Dispose(object? sender, EventArgs e)
//    {
//        RegionManager.Dispose(this);

//        // Clean the event to avoid memory leaking
//        try
//        {
//            Window.Closed -= Dispose;
//        }
//        catch
//        {
//            // Do nothing
//        }
//    }

//    /// <summary>
//    /// Find a region by name
//    /// </summary>
//    /// <param name="regionHost">Region name to search for</param>
//    /// <returns>Region or null if no region with the requested name was found</returns>
//    public UiRegion? FindRegion(RoutedViewHost regionHost)
//    {
//        return FindRegion($"{Name}.{regionHost.Name}");
//    }
//}
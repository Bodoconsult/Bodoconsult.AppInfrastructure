//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System.Runtime.CompilerServices;
//using System.Windows;
//using Bodoconsult.App.ReactiveUI.Interfaces;
//using Bodoconsult.App.Avalonia.ReactiveUI.Regions;
//using ReactiveUI;

//namespace Bodoconsult.App.Avalonia.ReactiveUI.Extensions;

///// <summary>
///// Extension methods for <see cref="IRegionManager"/>
///// </summary>
//public static class RegionManagerExtensions
//{
//    ///// <summary>
//    ///// Create a <see cref="AvaloniaUiRegion"/>
//    ///// </summary>
//    ///// <param name="regionManager">Current region manager instance</param>
//    ///// <param name="routedViewHost">Current <see cref="RoutedViewHost"/> instance to build the region. The name of the control is used as app-wide unique region name!</param>
//    ///// <returns><see cref="AvaloniaUiRegion"/> created and registered to region manager</returns>
//    //public static AvaloniaUiRegion CreateAvaloniaUiRegion(this IRegionManager? regionManager, RoutedViewHost routedViewHost)
//    //{
//    //    if (regionManager == null)
//    //    {
//    //        throw new ArgumentNullException(nameof(regionManager));
//    //    }

//    //    var region = new AvaloniaUiRegion(routedViewHost, regionManager);
//    //    regionManager.RegisterRegion(region);
//    //    return region;
//    //}

//    /// <summary>
//    /// Create a UI window
//    /// </summary>
//    /// <param name="regionManager">Current region manager</param>
//    /// <param name="window">Current window</param>
//    /// <returns></returns>
//    /// <exception cref="ArgumentNullException"></exception>
//    public static IUiWindow CreateUiWindow<T, TViewmodel>(this IRegionManager? regionManager, IUiWindow uiWindow)
//    {
//        if (regionManager == null)
//        {
//            throw new ArgumentNullException(nameof(regionManager));
//        }

//        if (string.IsNullOrEmpty(window.Name))
//        {
//            window.Name = window.GetType().Name;
//        }

//        regionManager.RegisterWindow(uiWindow);
//        uiWindow.Closed += uiWindow.Dispose; 
//        return uiWindow;
//    }
//}
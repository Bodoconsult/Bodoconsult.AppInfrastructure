// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.Services;

namespace Bodoconsult.App.Wpf.ReactiveUI.Helper;

/// <summary>
/// Delivers resources from Bodoconsult.Wpf.Base LookAndFeel.xaml
/// </summary>
public class ResourceFinder
{
    /// <summary>
    /// Find a resource in LookAndFeel.xaml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resourceKey"></param>
    /// <returns></returns>
    public static T FindResource<T>(string resourceKey)
    {
        return ResourceFinderService.FindResource<T>("pack://application:,,,/Bodoconsult.App.Wpf.ReactiveUI;component/Resources/Styling/LookAndFeel.xaml", resourceKey);
    }
}
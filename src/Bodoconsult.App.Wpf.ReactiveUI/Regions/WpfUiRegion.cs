// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using System.Windows.Forms;
using Windows.Foundation;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Regions;

/// <summary>
/// <see cref="UiRegion"/> implementation for WPF
/// </summary>
public class WpfUiRegion : UiRegion
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="uiWindow">Current UI window</param>
    /// <param name="routedViewHost">Routed view host instance to register by its name as region name</param>
    public WpfUiRegion(IUiWindow uiWindow, RoutedViewHost routedViewHost) : base(uiWindow, routedViewHost.Name)
    { }
}

///// <summary>
///// Current interface impl for <see cref=""/>
///// </summary>
//public interface IWpfUiWindow: IUiWindow
//{
//    /// <summary>
//    /// Current window instance
//    /// </summary>
//    public Window Window { get; }
//}
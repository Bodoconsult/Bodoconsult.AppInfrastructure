// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.ReactiveUI;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Regions;

/// <summary>
/// <see cref="UiRegion"/> implementation for Avalonia
/// </summary>
public class AvaloniaUiRegion : UiRegion
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="uiWindow">Current UI window</param>
    /// <param name="routedViewHost">Routed view host instance to register by its name as region name</param>
    public AvaloniaUiRegion(IUiWindow uiWindow, RoutedViewHost routedViewHost) : base(uiWindow, routedViewHost.Name ?? throw new ArgumentNullException(routedViewHost.Name))
    { }
}

///// <summary>
///// Current interface impl for <see cref=""/>
///// </summary>
//public interface IAvaloniaUiWindow: IUiWindow
//{
//    /// <summary>
//    /// Current window instance
//    /// </summary>
//    public Window Window { get; }
//}
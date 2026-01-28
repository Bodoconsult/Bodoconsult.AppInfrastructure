// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.ObjectModel;
using System.Windows.Media;
using Bodoconsult.App.Wpf.ReactiveUI.Models;
using DynamicData;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

/// <summary>
/// Viewmodel for MenuControl
/// </summary>
public partial class MenuControlViewModel :  ReactiveObject
{
    /// <summary>
    ///  Menu items
    /// </summary>
    public ObservableCollection<WpfUiMenuItem> MenuItems { get; } = new();

    [Reactive] public partial Brush BackgroundBrush { get; set; }

    /// <summary>
    /// Add menu items
    /// </summary>
    /// <param name="menuItems">Menu items to add</param>
    public void AddMenuItems(IList<WpfUiMenuItem> menuItems)
    {
        MenuItems.AddRange(menuItems);
    }

    /// <summary>
    /// Clear the menu
    /// </summary>
    public void ClearMenu()
    {
        MenuItems.Clear();
    }
}
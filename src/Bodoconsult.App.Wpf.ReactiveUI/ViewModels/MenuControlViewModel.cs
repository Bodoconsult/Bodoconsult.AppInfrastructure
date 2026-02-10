// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.Menus;
using Bodoconsult.App.Wpf.ReactiveUI.Models;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

/// <summary>
/// Viewmodel for MenuControl
/// </summary>
public partial class MenuControlViewModel :  ReactiveObject
{
    private readonly WpfUiMenuBuilder _wpfUiMenuBuilder;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="wpfUiMenuBuilder"></param>
    public MenuControlViewModel(WpfUiMenuBuilder wpfUiMenuBuilder)
    {
        BackgroundBrush = new SolidColorBrush(Colors.LightGray);

        _wpfUiMenuBuilder = wpfUiMenuBuilder;

        // Use the ToObservableChangeSet operator to convert
        // the observable collection to IObservable<IChangeSet<T>>
        // which describes the changes. Then, use any DD operators
        // to transform the collection. 
        _wpfUiMenuBuilder.MenuItemsSource.ToObservableChangeSet()
            .Transform(value => value)
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _menuItems)
            .Subscribe();
    }

    /// <summary>
    ///  Menu items
    /// </summary>
    private readonly ReadOnlyObservableCollection<WpfUiMenuItem> _menuItems;
    
    /// <summary>
    /// Current menu items
    /// </summary>
    public ReadOnlyObservableCollection<WpfUiMenuItem> MenuItems => _menuItems;

    /// <summary>
    /// Background brush
    /// </summary>
    [Reactive]
    public partial Brush BackgroundBrush { get; set; } 
}
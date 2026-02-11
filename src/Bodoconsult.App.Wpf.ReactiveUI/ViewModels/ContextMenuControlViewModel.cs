// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.ObjectModel;
using System.Windows.Media;
using Bodoconsult.App.Wpf.ReactiveUI.Menus;
using Bodoconsult.App.Wpf.ReactiveUI.Models;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

/// <summary>
/// Viewmodel for ContextMenuControl
/// </summary>
public partial class ContextMenuControlViewModel : ReactiveObject
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public ContextMenuControlViewModel()
    {
        BackgroundBrush = new SolidColorBrush(Colors.LightGray);
    }

    /// <summary>
    /// Load the menu builder to use for this control
    /// </summary>
    /// <param name="wpfUiMenuBuilder">Current menu builder</param>
    public void LoadMenuBuilder(WpfUiMenuBuilder wpfUiMenuBuilder)
    {
        var wpfUiMenuBuilder1 = wpfUiMenuBuilder;

        // Use the ToObservableChangeSet operator to convert
        // the observable collection to IObservable<IChangeSet<T>>
        // which describes the changes. Then, use any DD operators
        // to transform the collection. 
        wpfUiMenuBuilder1.MenuItemsSource.ToObservableChangeSet()
            .Transform(value => value)
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _menuItems)
            .Subscribe();
    }


    /// <summary>
    ///  Menu items
    /// </summary>
    private ReadOnlyObservableCollection<WpfUiMenuItem>? _menuItems;

    /// <summary>
    /// Current menu items
    /// </summary>
    public ReadOnlyObservableCollection<WpfUiMenuItem> MenuItems => _menuItems ?? new ReadOnlyObservableCollection<WpfUiMenuItem>(new ObservableCollectionExtended<WpfUiMenuItem>());

    /// <summary>
    /// Background brush
    /// </summary>
    [Reactive]
    public partial Brush BackgroundBrush { get; set; }
}
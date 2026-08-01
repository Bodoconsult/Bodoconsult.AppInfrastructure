// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Delegates;
using Bodoconsult.App.Wpf.ReactiveUI.Menus;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;
using System.Windows.Media;
using DynamicData;
using DynamicData.Binding;

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
    /// Menu is ready built delegate
    /// </summary>
    public MenuBuiltDelegate? MenuBuiltDelegate { get; set; }

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
    private ReadOnlyObservableCollection<object>? _menuItems;

    /// <summary>
    /// Current menu items
    /// </summary>
    public ReadOnlyObservableCollection<object>? MenuItems => _menuItems;

    /// <summary>
    /// Background brush
    /// </summary>
    [Reactive]
    public partial Brush BackgroundBrush { get; set; }
}
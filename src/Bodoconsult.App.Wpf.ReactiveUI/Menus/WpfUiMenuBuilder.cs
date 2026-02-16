// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;
using DynamicData;
using DynamicData.Binding;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace Bodoconsult.App.Wpf.ReactiveUI.Menus;

/// <summary>
/// <see cref="IUiMenuBuilder"/> implementation for WPF menus using default <see cref="Menu"/> as base control
/// </summary>
public class WpfUiMenuBuilder : UiMenuBuilderBase
{
    private readonly ReadOnlyObservableCollection<object> _menuItems;

    private ObservableCollectionExtended<object> MenuItemsInternal { get; } = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="translationService">Current translation service</param>
    public WpfUiMenuBuilder(II18N translationService) : base(translationService)
    {
        // Use the ToObservableChangeSet operator to convert
        // the observable collection to IObservable<IChangeSet<T>>
        // which describes the changes. Then, use any DD operators
        // to transform the collection. 
        MenuItemsInternal.ToObservableChangeSet()
            .Transform(value => value)
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _menuItems)
            .Subscribe();
    }

    /// <summary>
    /// Source of <see cref="object"/> elements for binding to menu controls etc.
    /// </summary>
    public ReadOnlyObservableCollection<object> MenuItemsSource => _menuItems;

    /// <summary>
    /// Build the final object for a <see cref="CommandUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public override void BuildCommandUiMenuItem(CommandUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        var header = TranslationService.Translate(item.Name);

        var menuItem = new MenuItem
        {
            Header = header,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
        };

        if (item.CommandDefinition != null)
        {
            menuItem.Command = ReactiveCommand.CreateFromObservable(() => item.CommandDefinition.ExecuteMethod, item.CommandDefinition.CanExecuteMethod);
        }

        MenuItemsInternal.Add(menuItem);

        if (parentItem is not { ParentObject: not null })
        {
            return;
        }
        var parent = (MenuItem)parentItem.ParentObject;
        parent.Items.Add(menuItem);
    }

    /// <summary>
    /// Build the final object for a <see cref="GroupUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public override void BuildGroupUiMenuItem(GroupUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        var header = TranslationService.Translate(item.Name);

        var menuItem = new MenuItem
        {
            Header = header,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
        };
        item.ParentObject =  menuItem;

        MenuItemsInternal.Add(menuItem);

        if (parentItem is not { ParentObject: not null })
        {
            return;
        }
        var parent = (MenuItem)parentItem.ParentObject;
        parent.Items.Add(menuItem);
    }

    /// <summary>
    /// Build the final object for a <see cref="SeparatorUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public override void BuildSeparatorUiMenuItem(SeparatorUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        var menuItem = new Separator()
        {
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
        };

        MenuItemsInternal.Add(menuItem);

        if (parentItem is not { ParentObject: not null })
        {
            return;
        }
        var parent = (MenuItem)parentItem.ParentObject;
        parent.Items.Add(menuItem);
    }
}
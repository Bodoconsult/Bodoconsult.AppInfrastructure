// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Bodoconsult.App.Wpf.ReactiveUI.Controls;

/// <summary>
/// WPF default menu user control
/// </summary>
public partial class ContextMenuControl: UserControl
{
    private readonly ContextMenuControlViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="Menu"/> class.
    /// Sets menu alignment on initialization.
    /// </summary>
    public ContextMenuControl()
    {
        InitializeComponent();

        _viewModel = new ContextMenuControlViewModel
        {
            MenuBuiltDelegate = MenuBuiltDelegate
        };
        DataContext = _viewModel;
    }

    private void MenuBuiltDelegate()
    {
        Dispatcher.Invoke(() =>
        {
            LocalMenu.Items.Clear();

            if (_viewModel.MenuItems is null)
            {
                return;
            }

            foreach (var item in _viewModel.MenuItems)
            {
                LocalMenu.Items.Add(item);
            }

            LocalMenu.Visibility = Visibility.Visible;
        });
    }

    //private static void Initialize()
    //{
    //    if (!SystemParameters.MenuDropAlignment)
    //    {
    //        return;
    //    }

    //    var fieldInfo = typeof(SystemParameters).GetField(
    //        "_menuDropAlignment",
    //        BindingFlags.NonPublic | BindingFlags.Static);
    //    fieldInfo?.SetValue(null, false);
    //}
}
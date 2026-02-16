// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Bodoconsult.App.Wpf.Helpers;

namespace Bodoconsult.App.Wpf.ReactiveUI.Controls;

/// <summary>
/// WPF default menu user control
/// </summary>
public partial class MenuControl : UserControl
{
    private readonly MenuControlViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="Menu"/> class.
    /// Sets menu alignment on initialization.
    /// </summary>
    public MenuControl()
    {
        InitializeComponent();

        _viewModel = new MenuControlViewModel
        {
            MenuBuiltDelegate = MenuBuiltDelegate
        };
        DataContext = _viewModel;

        //Initialize();
    }

    private void MenuBuiltDelegate()
    {
        //Menu1.Items.Clear()

        Dispatcher.Invoke(() =>
        {
            LocalMenu.Items.Clear();

            if (_viewModel.MenuItems != null)
            {
                foreach (var item in _viewModel.MenuItems)
                {
                    LocalMenu.Items.Add(item);
                }

                LocalMenu.Visibility = Visibility.Visible;
            }
        });



        //var x = this.FindName("LocalMenu");

        //if (x is Menu menu)
        //{

        //}
    }

    //private void Initialize()
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
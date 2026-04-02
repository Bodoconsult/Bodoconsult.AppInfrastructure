// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Avalonia.Threading;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Controls;

/// <summary>
/// Menu control
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
        var cts = new CancellationTokenSource(5000);

        Dispatcher.UIThread.Invoke(() =>
        {
            LocalMenu.Items.Clear();

            if (_viewModel.MenuItems == null)
            {
                return;
            }

            foreach (var item in _viewModel.MenuItems)
            {
                LocalMenu.Items.Add(item);
            }

            LocalMenu.IsVisible = true;
        }, DispatcherPriority.Normal, cts.Token);
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
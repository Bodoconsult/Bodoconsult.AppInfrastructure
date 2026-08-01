// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.ReactiveUI.ViewModels;
using ReactiveUI;
using ReactiveUI.Primitives;
using System.Windows;

namespace WpfReactiveUiDemoApp.ViewModels;

/// <summary>
/// ViewModel for MainWindow window
/// </summary>
public partial class WpfReactiveUiDemoAppMainWindowViewModel : MainWindowViewModel
{

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="listener">Current app event listener</param>
    /// <param name="translationService">Translation service</param>
    /// <param name="regionManager">Region manager</param>
    public WpfReactiveUiDemoAppMainWindowViewModel(IAppEventListener listener, II18N translationService,
        IRegionManager regionManager) : base(listener, translationService, regionManager)
    {
        GoToWindow1Command = ReactiveCommand.CreateFromTask(GoToWindow1);
        GoToFirstViewCommand = ReactiveCommand.CreateFromTask(GoToFirstView);
        GoToWindow1Instance2Command = ReactiveCommand.CreateFromTask(GoToWindow1Instance2);
    }

    public ReactiveCommand<RxVoid, RxVoid> GoToWindow1Command { get; set; }

    public ReactiveCommand<RxVoid, RxVoid> GoToFirstViewCommand { get; set; }

    public ReactiveCommand<RxVoid, RxVoid> GoToWindow1Instance2Command { get; set; }

    /// <summary>
    /// Create the main form of the application
    /// </summary>
    /// <returns></returns>
    public override Window CreateWindow()
    {
        var w = new MainWindow
        {
            ViewModel = this,
            Visibility = Visibility.Visible,
        };

        WindowState = UiWindowState.Maximized;
        return w;
    }

    /// <summary>
    /// Define the menu items to be stored in <see cref="IUiMenuWindow.MenuItems"/>
    /// </summary>
    public override void DefineMenuItems()
    {
        var groupItem = new GroupUiMenuItem("File");
        MenuItems.Add(groupItem);

        var command1 = new CommandUiMenuItem("Go to first view")
        {
            CommandDefinition = new UiCommandDefinition(GoToFirstView, null)
        };

        groupItem.AddChild(command1);

        var command2 = new CommandUiMenuItem("Go to new window")
        {
            CommandDefinition = new UiCommandDefinition(GoToWindow1, null)
        };

        groupItem.AddChild(command2);

        var command3 = new CommandUiMenuItem("Go to new window instance 2")
        {
            CommandDefinition = new UiCommandDefinition(GoToWindow1Instance2, null)
        };

        groupItem.AddChild(command3);
    }

    // Sync command 

    //[ReactiveCommand]
    //public void GoToFirstView()
    //{
    //    try
    //    {
    //        Region1?.Navigate(new FirstViewModel(Region1));
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e);
    //        throw;
    //    }
    //}

    /// <summary>
    /// Async version of the command
    /// </summary>
    /// <returns></returns>

    public Task<RxVoid> GoToFirstView()
    {
        return new Task<RxVoid>(() =>
        {
            try
            {
                Region1?.Navigate(new FirstViewModel(Region1));
                return RxVoid.Default;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

    public Task<RxVoid> GoToWindow1()
    {
        return new Task<RxVoid>(() =>
        {
            try
            {

                var windowViewModel = new Window1ViewModel(RegionManager);

                var vm = new FirstViewModel();

                RegionManager.Navigate(windowViewModel, vm, "DocumentRegion");
                return RxVoid.Default;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

    public Task<RxVoid> GoToWindow1Instance2()
    {
        return new Task<RxVoid>(() =>
        {
            try
            {
                Region1?.Navigate(new FirstViewModel(Region1));

                var windowViewModel = new Window1ViewModel(RegionManager)
                {
                    InstanceName = "Window1Instance2"
                };

                var vm = new FirstViewModel();

                RegionManager.Navigate(windowViewModel, vm, "DocumentRegion");
                return RxVoid.Default;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}
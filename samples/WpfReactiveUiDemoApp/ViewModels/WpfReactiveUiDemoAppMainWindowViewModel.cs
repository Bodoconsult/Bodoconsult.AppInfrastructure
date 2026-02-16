// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.ReactiveUI.ViewModels;
using ReactiveUI.SourceGenerators;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using Bodoconsult.App.ReactiveUI.Menus;

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
    { }

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
            CommandDefinition = new UiCommandDefinition(GoToFirstView(), null)
        };

        groupItem.AddChild(command1);
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
    [ReactiveCommand]
    public IObservable<Unit> GoToFirstView()
    {
        try
        {
            Region1?.Navigate(new FirstViewModel(Region1));
            return Observable.Return(Unit.Default);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [ReactiveCommand]
    public IObservable<Unit> GoToWindow1()
    {
        try
        {
            Region1?.Navigate(new FirstViewModel(Region1));

            var windowViewModel = new Window1ViewModel(RegionManager);

            var vm = new FirstViewModel();

            RegionManager.Navigate(windowViewModel, vm, "DocumentRegion");
            return Observable.Return(Unit.Default);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [ReactiveCommand]
    public IObservable<Unit> GoToWindow1Instance2()
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
            return Observable.Return(Unit.Default);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
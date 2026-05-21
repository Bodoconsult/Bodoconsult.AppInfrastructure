// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Avalonia.Controls;
using AvaloniaReactiveUiDemoApp.AppData;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.Helpers;
using Bodoconsult.App.Avalonia.ReactiveUI.Views;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.ReactiveUI.ViewModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Channels;

namespace AvaloniaReactiveUiDemoApp.ViewModels;

/// <summary>
/// ViewModel for MainWindow window
/// </summary>
public partial class AvaloniaReactiveUiDemoAppMainWindowViewModel : MainWindowViewModel
{
    private readonly Interaction<string, bool> _confirm;

    public Interaction<string, bool> Confirm => this._confirm;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="listener">Current app event listener</param>
    /// <param name="translationService">Translation service</param>
    /// <param name="regionManager">Region manager</param>
    public AvaloniaReactiveUiDemoAppMainWindowViewModel(IAppEventListener listener, II18N translationService,
        IRegionManager regionManager) : base(listener, translationService, regionManager)
    {
        _confirm = new Interaction<string, bool>();
    }

    /// <summary>
    /// Create the main form of the application
    /// </summary>
    /// <returns></returns>
    public override Window CreateWindow()
    {
        var w = new MainWindow
        {
            ViewModel = this,
            IsVisible = true,
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

        // Group Help
        var helpGroupItem = new GroupUiMenuItem("Help");
        MenuItems.Add(helpGroupItem);

        var command4 = new CommandUiMenuItem("Copyright")
        {
            CommandDefinition = new UiCommandDefinition(GoToCopyright, null)
        };

        helpGroupItem.AddChild(command4);

        var command5 = new CommandUiMenuItem("Show info dialog")
        {
            CommandDefinition = new UiCommandDefinition(GoToInfoDialog, null)
        };

        helpGroupItem.AddChild(command5);
    }

    private IObservable<Unit> GoToInfoDialog()
    {
        return Observable.StartAsync(async () =>
        {
            // this will throw an exception if nothing handles the interaction
            _ = await _confirm.Handle("Hello user!");
        });
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
        //try
        //{
            //Region1?.Navigate(new FirstViewModel(Region1));

            var windowViewModel = new Window1ViewModel(RegionManager);

            var vm = new FirstViewModel();

            RegionManager.Navigate(windowViewModel, vm, "DocumentRegion");
            return Observable.Return(Unit.Default);
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e);
        //    throw;
        //}
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

    [ReactiveCommand]
    public IObservable<Unit> GoToCopyright()
    {
        return Observable.Start(() =>
        {
            var vm = Globals.Instance.DiContainer.Get<CopyrightViewModel>();
            vm.LoadLicenseInfo();
            vm.LoadToolInfo();

            var window = new CopyrightWindow
            {
                DataContext = vm,
                WindowState = Avalonia.Controls.WindowState.Normal
            };
            window.Show();
        }, RxSchedulers.MainThreadScheduler);
    }
}
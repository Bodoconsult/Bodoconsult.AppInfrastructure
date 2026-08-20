// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Avalonia.Controls;
using AvaloniaReactiveUiDemoApp.AppData;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.Services;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using Bodoconsult.App.Avalonia.ReactiveUI.Views;
using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.ReactiveUI.ViewModels;
using ReactiveUI;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
using System.Reactive.Linq;

namespace AvaloniaReactiveUiDemoApp.ViewModels;

/// <summary>
/// ViewModel for MainWindow window
/// </summary>
public partial class AvaloniaReactiveUiDemoAppMainWindowViewModel : MainWindowViewModel
{
    private readonly IAppGlobals _appGlobals;
    private readonly Interaction<string, bool> _confirm;
    private readonly IUiFileDialogService _fileDialogService;

    private readonly IAvaloniaUiClipboardService _clipboardService;

    public Interaction<string, bool> Confirm => _confirm;

    public ReactiveCommand<RxVoid, RxVoid> GoToWindow1Command { get; set; }

    public ReactiveCommand<RxVoid, RxVoid> GoToFirstViewCommand { get; set; }

    public ReactiveCommand<RxVoid, RxVoid> GoToWindow1Instance2Command { get; set; }

    public ReactiveCommand<RxVoid, RxVoid> GoToImageControlCommand { get; set; }


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="listener">Current app event listener</param>
    /// <param name="translationService">Translation service</param>
    /// <param name="regionManager">Region manager</param>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="fileDialogService">Current file dialog service</param>
    /// <param name="clipboardService">Current clipboard service</param>
    public AvaloniaReactiveUiDemoAppMainWindowViewModel(IAppEventListener listener, II18N translationService,
        IRegionManager regionManager, IAppGlobals appGlobals, IUiFileDialogService fileDialogService, IAvaloniaUiClipboardService clipboardService) : base(listener, translationService, regionManager)
    {
        _appGlobals = appGlobals;
        _confirm = new Interaction<string, bool>();
        _fileDialogService = fileDialogService;
        _clipboardService = clipboardService;

        GoToWindow1Command = ReactiveCommand.CreateFromTask(GoToWindow1, null, AvaloniaScheduler.Instance);
        GoToFirstViewCommand = ReactiveCommand.CreateFromTask(GoToFirstView, null, AvaloniaScheduler.Instance);
        GoToWindow1Instance2Command = ReactiveCommand.CreateFromTask(GoToWindow1Instance2, null, AvaloniaScheduler.Instance);
        GoToImageControlCommand = ReactiveCommand.CreateFromTask(GoToImageControl, null, AvaloniaScheduler.Instance);
    }

    /// <summary>
    /// Navigate to start user controls for the regions. Loads <see cref="LogoViewModel"/> based control
    /// </summary>
    public override void NavigateToStart()
    {
        var vm = _appGlobals.DiContainer.Get<LogoViewModel>();
        Region1?.Navigate(vm);

        ArgumentNullException.ThrowIfNull(Region2);

        var vm2 = new SecondViewModel(Region2);
        Region2.Navigate(vm2);
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

        ((FileDialogService)_fileDialogService).LoadTopLevel(w);
        ((ClipboardService)_clipboardService).LoadClipboard(w);

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

        var command6 = new CommandUiMenuItem("Go to image control")
        {
            CommandDefinition = new UiCommandDefinition(GoToImageControl, null)
        };

        groupItem.AddChild(command6);

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

    private async Task<RxVoid> GoToInfoDialog()
    {
        // this will throw an exception if nothing handles the interaction
        _ = await _confirm.Handle("Hello user!");
        return RxVoid.Default;
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
        Region1?.Navigate(new FirstViewModel(Region1));
        return Task.FromResult(RxVoid.Default);
    }

    public Task<RxVoid> GoToImageControl()
    {
        var vm = _appGlobals.DiContainer.Get<ImageViewModel>();

        var fileName = Path.Combine(_appGlobals.AppStartParameter.AppPath ?? "", "fft.jpg");
        vm.LoadBitmapFromFile(fileName);

        Region1?.Navigate(vm);
        return Task.FromResult(RxVoid.Default);
    }

    public Task<RxVoid> GoToWindow1()
    {
        var windowViewModel = new Window1ViewModel(RegionManager);

        var vm = new FirstViewModel();

        RegionManager.Navigate(windowViewModel, vm, "DocumentRegion");
        return Task.FromResult(RxVoid.Default);
    }

    public Task<RxVoid> GoToWindow1Instance2()
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
            return Task.FromResult(RxVoid.Default);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<RxVoid> GoToCopyright()
    {
        try
        {
            var vm = Globals.Instance.DiContainer.Get<CopyrightViewModel>();

            var window = new CopyrightWindow
            {
                DataContext = vm,
                WindowState = Avalonia.Controls.WindowState.Normal
            };
            window.Show();

            return Task.FromResult(RxVoid.Default);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
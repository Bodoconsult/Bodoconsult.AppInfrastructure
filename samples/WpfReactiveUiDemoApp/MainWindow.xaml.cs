// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.Wpf.ReactiveUI.Menus;
using Bodoconsult.App.Wpf.ReactiveUI.Regions;
using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;
using ReactiveUI;
using Bodoconsult.App.Wpf.ReactiveUI.Converters;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Disposables;
using WpfReactiveUiDemoApp.ViewModels;

namespace WpfReactiveUiDemoApp;

public partial class MainWindow : IUiWindow
{

    private WpfUiMenuBuilder? _menuBuilder;
    private MenuControlViewModel? _menuControlViewModel;

    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            //SubscribeExtensions.Subscribe(this.WhenAnyValue(x => x.ViewModel).ObserveOn(RxSchedulers.MainThreadScheduler),  x =>
            //{
            //    if (x == null)
            //    {
            //        return;
            //    }

            //    RegisterAllRouterBindings(x, disposables);
            //});

            RegisterAllRouterBindings(ViewModel, disposables);
        });
    }

    public void RegisterAllRouterBindings(WpfReactiveUiDemoAppMainWindowViewModel? viewModel, MultipleDisposable disposables)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        // Bind WindowState
        this.Bind(viewModel, vm => vm.WindowState,
                view => view.WindowState,
                InlineConverterMethods.FromUiWindowStateToWindowState,
                InlineConverterMethods.FromWindowStateToUiWindowState)
            .DisposeWith(disposables);

        // Get the viemodel of the menu control
        _menuControlViewModel = (MenuControlViewModel)MainMenu.DataContext;

        // Now build the menu
        viewModel.DefineMenuItems();
        
        _menuBuilder =  new WpfUiMenuBuilder(viewModel.TranslationService);
        viewModel.MenuBuilder = _menuBuilder;
        _menuControlViewModel.LoadMenuBuilder(_menuBuilder);
        viewModel.BuildIt();

        // Now set the regions for routing
        RegionManager = viewModel.RegionManager;

        var rm = (WpfRegionManager)viewModel.RegionManager;
        var window = rm.RegisterInstances<MainWindow, WpfReactiveUiDemoAppMainWindowViewModel>(this, disposables);

        viewModel.Region1=window.FindRegion(DocumentRegion.Name);
        viewModel.Region2=window.FindRegion(MenuRegion.Name);

        ArgumentNullException.ThrowIfNull(viewModel.Region1);
        ArgumentNullException.ThrowIfNull(viewModel.Region2);

        // Bind regions
        this.OneWayBind(viewModel, p => p.Region1!.Router, xy => xy.DocumentRegion.Router)
            .DisposeWith(disposables);

        this.OneWayBind(viewModel, p => p.Region2!.Router, xy => xy.MenuRegion.Router)
            .DisposeWith(disposables);

        // Bind commands to buttons
        this.BindCommand(viewModel, x => x.GoToFirstViewCommand, x => x.GoNextButton)
            .DisposeWith(disposables);

        this.BindCommand(viewModel, x => x.GoToWindow1Command, x => x.GoNewWindowButton)
            .DisposeWith(disposables);

        this.BindCommand(viewModel, x => x.GoToWindow1Instance2Command, x => x.GoNewWindowInstance2Button)
            .DisposeWith(disposables);

        this.BindCommand(viewModel, x => x.Region1!.GoBack, x => x.GoBackButton)
            .DisposeWith(disposables);

        var vm2 = new SecondViewModel(viewModel.Region2);

        viewModel.Region2.Navigate(vm2);
    }

    /// <summary>
    /// Window instance name
    /// </summary>
    public string? InstanceName { get; set; }

    /// <summary>
    /// Current region manager
    /// </summary>
    public IRegionManager? RegionManager { get; private set; }

    /// <summary>
    /// Region in the current window
    /// </summary>
    public List<UiRegion> UiRegions { get; } = [];

    /// <summary>
    /// Dispose this window from region manager
    /// </summary>
    /// <param name="sender">Do not use</param>
    /// <param name="e">Do not use</param>
    public void Dispose(object? sender, EventArgs e)
    {
        RegionManager?.Dispose(this);

        // Clean the event to avoid memory leaking
        try
        {
            Closed -= Dispose;
        }
        catch
        {
            // Do nothing
        }
    }

    /// <summary>
    /// Load the region manager
    /// </summary>
    /// <param name="regionManager">Current region manager instance</param>
    public void LoadRegionManager(IRegionManager regionManager)
    {
        RegionManager = regionManager;
    }

    /// <summary>
    /// Show an info dialog
    /// </summary>
    /// <param name="message">Message to show</param>
    /// <returns>True</returns>
    public Task<bool?> ShowInfoDialog(string message)
    {
        throw new NotImplementedException();
    }

    ///// <summary>
    ///// Allows the ViewModel to be used on the XAML via a dependency property
    ///// </summary>
    //public static readonly DependencyProperty ViewModelProperty =
    //        DependencyProperty.Register("ViewModel", typeof(MainViewModel), typeof(MainWindow),
    //                                    new PropertyMetadata(default(MainViewModel)));
}

// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.Wpf.ReactiveUI.Regions;
using ReactiveUI;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Disposables;
using WpfReactiveUiDemoApp.ViewModels;

namespace WpfReactiveUiDemoApp.Views;

/// <summary>
/// Interaktionslogik für Window1.xaml
/// </summary>
public partial class Window1 : IUiWindow
{
    public Window1()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            //SubscribeExtensions.Subscribe(this.WhenAnyValue(x => x.ViewModel).ObserveOn(RxSchedulers.MainThreadScheduler), x =>
            //{
            //    if (x is null)
            //    {
            //        return;
            //    }

            //    RegisterAllRouterBindings(x, disposables);
            //});

            RegisterAllRouterBindings(ViewModel, disposables);
        });
    }

    public void RegisterAllRouterBindings(Window1ViewModel? viewModel, MultipleDisposable disposables)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        var rm = (WpfRegionManager)viewModel.RegionManager;
        var window = rm.RegisterInstances<Window1, Window1ViewModel>(this, disposables);

        viewModel.Region1 = window.FindRegion(DocumentRegion.Name);
        viewModel.Region2 = window.FindRegion(MenuRegion.Name);

        ArgumentNullException.ThrowIfNull(viewModel.Region1);
        ArgumentNullException.ThrowIfNull(viewModel.Region2);
        
        this.OneWayBind(viewModel, p => p.Region1!.Router, xy => xy.DocumentRegion.Router)
            .DisposeWith(disposables);

        this.OneWayBind(viewModel, p => p.Region2!.Router, xy => xy.MenuRegion.Router)
            .DisposeWith(disposables);

        this.BindCommand(viewModel, x => x.GoToSecondViewCommand, x => x.GoNextButton)
            .DisposeWith(disposables);

        //this.BindCommand(viewModel, x => x.GoToWindow1Command, x => x.GoNewWindowButton)
        //    .DisposeWith(disposables);

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
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.Wpf.ReactiveUI.Regions;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Bodoconsult.App.ReactiveUI.Regions;
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
            this.WhenAnyValue(x => x.ViewModel).ObserveOn(RxSchedulers.MainThreadScheduler).Subscribe(x =>
            {
                if (x == null)
                {
                    return;
                }

                RegisterAllRouterBindings(x, disposables);
            });
        });
    }

    public void RegisterAllRouterBindings(Window1ViewModel viewModel, CompositeDisposable disposables)
    {
        //if (viewModel == null)
        //{
        //    return;
        //}

        var rm = (WpfRegionManager)viewModel.RegionManager;
        var window = rm.RegisterInstances<Window1, Window1ViewModel>(this, disposables);

        viewModel.Region1 = window.FindRegion(DocumentRegion.Name);
        viewModel.Region2 = window.FindRegion(MenuRegion.Name);

        if (viewModel.Region1 == null)
        {
            throw new ArgumentNullException(nameof(viewModel.Region1));
        }

        if (viewModel.Region2 == null)
        {
            throw new ArgumentNullException(nameof(viewModel.Region2));
        }

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
    /// Current region manager
    /// </summary>
    public IRegionManager? RegionManager => ViewModel?.RegionManager;

    /// <summary>
    /// Region in the current window
    /// </summary>
    public List<UiRegion> UiRegions { get; } = new();

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
}
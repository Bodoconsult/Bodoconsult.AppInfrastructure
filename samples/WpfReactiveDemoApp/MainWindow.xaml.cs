// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Disposables;
using WpfReactiveDemoApp.ViewModels;

namespace WpfReactiveDemoApp;

// We use ReactiveWindow here for WPF, but could actually use
// ReactiveUserControl or a custom IViewFor implementation. For
// Xamarin.Forms, use ReactiveMasterDetailPage.
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainViewModel();
        this.WhenActivated(RegisterAllRouterBindings);
    }

    public void RegisterAllRouterBindings(MultipleDisposable disposables)
    {
        // Bind the view model router to RoutedViewHost.Router property.
        this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
            .DisposeWith(disposables);
        this.BindCommand(ViewModel, x => x.GoNextCommand, x => x.GoNextButton)
            .DisposeWith(disposables);
        this.BindCommand(ViewModel, x => x.GoBackCommand, x => x.GoBackButton)
            .DisposeWith(disposables);
    }
}
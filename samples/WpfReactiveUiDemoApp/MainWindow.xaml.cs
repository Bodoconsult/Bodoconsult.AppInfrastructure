using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.Wpf.Helpers;
using Bodoconsult.App.Wpf.ReactiveUI.Extensions;
using Bodoconsult.App.Wpf.ReactiveUI.Regions;
using WpfReactiveUiDemoApp.AppData;
using WpfReactiveUiDemoApp.ViewModels;

namespace WpfReactiveUiDemoApp;

// We use ReactiveWindow here for WPF, but could actually use
// ReactiveUserControl or a custom IViewFor implementation. For
// Xamarin.Forms, use ReactiveMasterDetailPage.
public partial class MainWindow
{
    public MainWindow()
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

    public void RegisterAllRouterBindings(WpfReactiveUiDemoAppMainWindowViewModel viewModel, CompositeDisposable disposables)
    {
        var rm = (WpfRegionManager)viewModel.RegionManager;
        var window = rm.RegisterInstances<MainWindow, WpfReactiveUiDemoAppMainWindowViewModel>(this, disposables);

        viewModel.Region1=window.FindRegion(DocumentRegion);
        viewModel.Region2=window.FindRegion(MenuRegion);

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

        this.BindCommand(viewModel, x => x.GoToFirstViewCommand, x => x.GoNextButton)
            .DisposeWith(disposables);

        this.BindCommand(viewModel, x => x.GoToWindow1Command, x => x.GoNewWindowButton)
            .DisposeWith(disposables);

        this.BindCommand(viewModel, x => x.Region1!.GoBack, x => x.GoBackButton)
            .DisposeWith(disposables);

        var vm2 = new SecondViewModel(viewModel.Region2);

        viewModel.Region2.Navigate(vm2);
    }
}
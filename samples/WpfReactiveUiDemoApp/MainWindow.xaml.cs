using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;
using ReactiveUI;
using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows.Forms;
using Bodoconsult.App.Wpf.ReactiveUI.Extensions;
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

                RegisterRouterBinding(x, disposables);
            });
        });


        //this.WhenActivated(disposables =>
        //{


        //    //// Bind the view model router to RoutedViewHost.Router property.
        //    //this.OneWayBind(ViewModel, x => region1.Router, x => x.DocumentRegion.Router)
        //    //    .DisposeWith(disposables);
        //    //this.BindCommand(ViewModel, x => x.GoNext, x => x.GoNextButton)
        //    //    .DisposeWith(disposables);
        //    //this.BindCommand(ViewModel, x => x.GoBack, x => x.GoBackButton)
        //    //    .DisposeWith(disposables);
        //});
    }

    public void RegisterRouterBinding(WpfReactiveUiDemoAppMainWindowViewModel viewModel, CompositeDisposable disposables)
    {
        if (viewModel == null)
        {
            return;
        }

        viewModel.Region1 = viewModel.RegionManager.CreateWpfUiRegion(this.DocumentRegion);

        this.OneWayBind(viewModel, p => p.Region1.Router, xy => xy.DocumentRegion.Router)
            .DisposeWith(disposables);

        this.BindCommand(viewModel, x => x.SaveCommand, x => x.GoNextButton)
            .DisposeWith(disposables);
    }
}
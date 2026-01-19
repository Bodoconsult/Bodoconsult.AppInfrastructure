using System.Reactive.Disposables.Fluent;
using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;
using ReactiveUI;
using WpfReactiveUiDemoApp.ViewModels;

namespace WpfReactiveUiDemoApp
{
    // We use ReactiveWindow here for WPF, but could actually use
    // ReactiveUserControl or a custom IViewFor implementation. For
    // Xamarin.Forms, use ReactiveMasterDetailPage.
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //ViewModel = new MainViewModel();



            var region1 = new WpfUiRegion(this.DocumentRegion);

            ViewModel.RegionManager.RegisterRegion(region1);

            this.WhenActivated(disposables =>
            {
                // Bind the view model router to RoutedViewHost.Router property.
                this.OneWayBind(ViewModel, x => region1.Router, x => x.DocumentRegion.Router)
                    .DisposeWith(disposables);
                //this.BindCommand(ViewModel, x => x.GoNext, x => x.GoNextButton)
                //    .DisposeWith(disposables);
                //this.BindCommand(ViewModel, x => x.GoBack, x => x.GoBackButton)
                //    .DisposeWith(disposables);
            });
        }
    }

}
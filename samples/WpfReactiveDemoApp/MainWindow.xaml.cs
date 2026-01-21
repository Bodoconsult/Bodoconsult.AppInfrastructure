using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        this.WhenActivated(disposables =>
        {
            // Bind the view model router to RoutedViewHost.Router property.
            this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, x => x.GoNext, x => x.GoNextButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, x => x.GoBack, x => x.GoBackButton)
                .DisposeWith(disposables);
        });
    }
}
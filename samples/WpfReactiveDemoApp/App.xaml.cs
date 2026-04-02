using ReactiveUI.Builder;
using System.Windows;

namespace WpfReactiveDemoApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var rxuiInstance = RxAppBuilder.CreateReactiveUIBuilder()
            .WithWpf() // Register WPF platform services
            .WithViewsFromAssembly(typeof(App).Assembly) // Register views and view models
            .BuildApp();

        var mainUiThreadScheduler = rxuiInstance.MainThreadScheduler;
        var taskpoolScheduler = rxuiInstance.TaskpoolScheduler;

        //var test = rxuiInstance.Current.GetService<MainViewModel>();
    }

        
}
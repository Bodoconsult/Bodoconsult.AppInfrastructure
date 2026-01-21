using ReactiveUI;
using ReactiveUI.Builder;
using Splat;
using System.Configuration;
using System.Data;
using System.Windows;
using WpfReactiveDemoApp.ViewModels;

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

        var mainUIThreadScheduler = rxuiInstance.MainThreadScheduler;
        var taskpoolScheduler = rxuiInstance.TaskpoolScheduler;

        //var test = rxuiInstance.Current.GetService<MainViewModel>();
    }

        
}
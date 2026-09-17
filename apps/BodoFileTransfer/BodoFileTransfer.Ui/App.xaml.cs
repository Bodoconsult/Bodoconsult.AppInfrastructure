using System.Windows;
using log4net;

namespace BodoFileTransferCore.Ui;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{


    private ILog _logger;


    private void Application_Startup(object sender, StartupEventArgs e)
    {

        _logger = LogManager.GetLogger(nameof(App));

        _logger.Info("BodoFileTransfer starts...");

        //GlobalValues.LoadAppSettings();

        //GlobalValues.DiContainer.AddSingleton((IAppSettings)GlobalValues.CurrentAppSettings);
        //GlobalValues.DiContainer.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
        //GlobalValues.DiContainer.AddSingleton(typeof(MainWindow));
        //GlobalValues.DiContainer.BuildServiceProvider();

        ////var x = GlobalValues.DiContainer.Get<IMainWindowViewModel>();

        //var window = GlobalValues.DiContainer.Get<MainWindow>();
        //{
 
        //};
        //window?.Show();
    }

}
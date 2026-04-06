// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using WpfReactiveUiDemoApp.AppData;

namespace WpfReactiveUiDemoApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        //var rxuiInstance = RxAppBuilder.CreateReactiveUIBuilder()
        //    .WithWpf() // Register WPF platform services
        //               //.WithViewsFromAssembly(typeof(App).Assembly) // Register views and view models
        //    //.RegisterView<MainWindow, MainViewModel>()
        //    //.RegisterView<FirstView, FirstViewModel>()
        //    .BuildApp();

        //var mainUIThreadScheduler = rxuiInstance.MainThreadScheduler;
        //var taskpoolScheduler = rxuiInstance.TaskpoolScheduler;

        var type = typeof(App);

        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForBackgroundServiceApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.AppName = "WpfReactiveUiDemoApp: Demo app";
        param.SoftwareTeam = "Robert Leisner";
        param.LogoRessourcePath = "WpfReactiveUiDemoApp.Resources.logo.jpg";
        param.LogoAssembly = type.Assembly;
        param.AppFolderName = "WpfReactiveUiDemoApp";

        //const string performanceToken = "--PERF";

        //if (args.Contains(performanceToken))
        //{
        //    param.IsPerformanceLoggingActivated = true;
        //}

        // Now start app buiding process
        var builder = new WpfReactiveUiDemoAppAppBuilder(globals);
#if !DEBUG
                    AppDomain.CurrentDomain.UnhandledException += builder.CurrentDomainOnUnhandledException;
#endif

        // Load basic app metadata
        builder.LoadBasicSettings(typeof(App));

        // Process the config file
        builder.ProcessConfiguration();

        // Now load the globally needed settings
        builder.LoadGlobalSettings();

        if (Globals.Instance.Logger == null)
        {
            throw new ArgumentNullException(nameof(Globals.Instance.Logger));
        }

        // Write first log entry with default logger
        Globals.Instance.Logger.LogInformation($"{param.AppName} {param.AppVersion} starts...");
        Console.WriteLine("Logging started...");

        // App is ready now for doing something
        Console.WriteLine($"Connection string loaded: {param.DefaultConnectionString}");

        Console.WriteLine("");
        Console.WriteLine("");

        Console.WriteLine($"App name loaded: {param.AppName}");
        Console.WriteLine($"App version loaded: {param.AppVersion}");
        Console.WriteLine($"App path loaded: {param.AppPath}");

        Console.WriteLine("");
        Console.WriteLine("");

        Console.WriteLine($"Logging config: {ObjectHelper.GetObjectPropertiesAsString(Globals.Instance.LoggingConfig)}");

        // Prepare the DI container package
        builder.LoadDiContainerServiceProviderPackage();
        builder.RegisterDiServices();
        // builder.FinalizeDiContainerSetup(); Do not run here

        // Now finally start the app and wait
        builder.StartApplication(null);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AvaloniaApp1.AppData;
using Bodoconsult.App.Avalonia.Interfaces;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;

// ReSharper disable LocalizableElement

namespace AvaloniaApp1;

public class App : Application
{
    public IMainWindowViewModel MainWindowViewModel { get; set; }

    /// <summary>Initializes the application by loading XAML etc.</summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        string[] args = null;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            args = desktop.Args;

            //// Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            //// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            //DisableAvaloniaDataAnnotationValidation();
            //desktop.MainWindow = new MainWindow
            //{
            //    DataContext = new MainWindowViewModel(),
            //};
        }

        base.OnFrameworkInitializationCompleted();

        var type = typeof(App);

        Debug.Print("Hello, World!");

        Console.WriteLine("AvaloniaApp1 initiation starts...");

        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.AppName = "AvaloniaApp1: Demo app";
        param.SoftwareTeam = "Robert Leisner";
        param.LogoRessourcePath = "AvaloniaApp1.Resources.logo.jpg";
        param.AppFolderName = "AvaloniaApp1";

        const string performanceToken = "--PERF";

        if (args!=null && args.Contains(performanceToken))
        {
            param.IsPerformanceLoggingActivated = true;
        }

        // Now start app buiding process
        var builder = new AvaloniaApp1AppBuilder(globals);
#if !DEBUG
        AppDomain.CurrentDomain.UnhandledException += builder.CurrentDomainOnUnhandledException;
#endif

        // Load basic app metadata
        builder.LoadBasicSettings();

        // Process the config file
        builder.ProcessConfiguration();

        // Now load the globally needed settings
        builder.LoadGlobalSettings();

        // Write first log entry with default logger
        Globals.Instance.Logger.LogInformation($"{param.AppName} {param.AppVersion} starts...");
        Console.WriteLine("Logging started...");

        // App is ready now for doing something
        Console.WriteLine($"Connection string loaded: {param.DefaultConnectionString}");

        Console.WriteLine(string.Empty);
        Console.WriteLine(string.Empty);

        Console.WriteLine($"App name loaded: {param.AppName}");
        Console.WriteLine($"App version loaded: {param.AppVersion}");
        Console.WriteLine($"App path loaded: {param.AppPath}");

        Console.WriteLine(string.Empty);
        Console.WriteLine(string.Empty);

        Console.WriteLine($"Logging config: {ObjectHelper.GetObjectPropertiesAsString(Globals.Instance.LoggingConfig)}");

        // Prepare the DI container package
        builder.LoadDiContainerServiceProviderPackage();
        builder.RegisterDiServices();
        builder.FinalizeDiContainerSetup();

        // Create the viewmodel now
        MainWindowViewModel = Globals.Instance.DiContainer.Get<IMainWindowViewModel>();
        MainWindowViewModel.HeaderBackColor = Colors.DarkBlue;
        MainWindowViewModel.BodyBackColor = Colors.Beige;
        MainWindowViewModel.AppExe = param.AppExe;

        //var eventLevel = EventLevel.Warning;
        //var listener = new AppEventListener(eventLevel);



        //MainWindowViewModel = new AvaloniaApp1MainWindowViewModel(listener)
        //{
        //    HeaderBackColor = Colors.DarkBlue,
        //    BodyBackColor = Colors.Beige,
        //    AppExe = param.AppExe
        //};



        DataContext = MainWindowViewModel;

        // Load the logo now
        MainWindowViewModel.LoadLogo(type.Assembly, param.LogoRessourcePath);

        // Set the view model 
        builder.MainWindowViewModel = MainWindowViewModel;

        // Now finally start the app and wait
        builder.StartApplication();

        //base.OnStartup(e);
    }
}
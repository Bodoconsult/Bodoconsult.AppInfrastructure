// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.AppStarter;
using Bodoconsult.App.ReactiveUI.DependecyResolvers;
using Bodoconsult.App.ReactiveUI.Interfaces;
using ReactiveUI;
using ReactiveUI.Avalonia;
using ReactiveUI.Builder;
using Splat;

namespace Bodoconsult.App.Avalonia.ReactiveUI.App;

/// <summary>
/// Base class for <see cref="IAppBuilder"/> implementations running a background service but not using GRPC
/// </summary>
public class BaseAvaloniaReactiveUiAppBuilder : BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="args">Current app args from command line</param>
    public BaseAvaloniaReactiveUiAppBuilder(IAppGlobals appGlobals, string[]? args = null) : base(appGlobals)
    {
        AppGlobals.DiContainer = new DiContainer();
    }

    /// <summary>
    /// Register DI container services
    /// </summary>
    public override void RegisterDiServices()
    {
        DiContainerServiceProviderPackage.AddServices(AppGlobals.DiContainer);

        AppGlobals.DiContainer.AddSingleton<IAppBuilder>(this);
    }

    /// <summary>
    /// Start the application
    /// </summary>
    public override void StartApplication()
    {
        var dpr = new MicrosoftDependencyResolver(AppGlobals.DiContainer.ServiceCollection);

        var appB = dpr.CreateReactiveUIBuilder(); // Register Avalonia platform services
        appB.WithAvalonia();

        // View location
        appB.ConfigureViewLocator(LoadViewLocation);
        var h = appB.BuildApp();

        if (dpr.ServiceProvider == null)
        {
            throw new ArgumentNullException(nameof(dpr.ServiceProvider));
        }

        if (AppLocator.Current is MicrosoftDependencyResolver resolver)
        {
            resolver.UpdateContainer(dpr.ServiceProvider);
        }
        else
        {
            // Will be disposed with the InternalLocator
            AppLocator.SetLocator(dpr);
        }

        if (AppGlobals is IReactiveUiAppGlobals uiAppGlobals)
        {
            uiAppGlobals.ReactiveUiInstance = h;

            uiAppGlobals.MainUiThreadScheduler = h.MainThreadScheduler;
            uiAppGlobals.TaskpoolScheduler = h.TaskpoolScheduler;
        }

        AppGlobals.DiContainer.LoadServiceProvider(dpr.ServiceProvider);

        DiContainerServiceProviderPackage.LateBindObjects(AppGlobals.DiContainer);

        //// For checking if DI container works
        //var service = AppLocator.Current.GetService<IAppLoggerProxy>();
        //var service2 = AppGlobals.DiContainer.Get<AvaloniaReactiveUiDemoAppMainWindowViewModel>();

        // Logger
        AddLogger();

        // Create the viewmodel now
        var viewModel = CreateViewModel();

        // Inject it to UI
        var appStarter = new AvaloniaStarterUi(this, viewModel);
        AppStarter = appStarter;

        // Run as singleton app
        if (AppGlobals.AppStartParameter.IsSingletonApp && appStarter.IsAnotherInstance)
        {
            Console.WriteLine($"Another instance of {AppGlobals.AppStartParameter.AppName} is already running! Press any key to proceed!");
            Console.ReadLine();
            Environment.Exit(0);
            return;
        }

        appStarter.Start();

        appStarter.Wait();

        StartApplicationService();
    }

    /// <summary>
    /// Add logging
    /// </summary>
    public virtual void AddLogger()
    {
        //var logger = AppGlobals.DiContainer.Get<IAppLoggerProxy>();
        //var loggerFactory = logger.LoggerFactory;
    }

    /// <summary>
    /// Load view location
    /// </summary>
    /// <param name="locator">The locator to use for the app instance</param>
    public virtual void LoadViewLocation(DefaultViewLocator locator)
    {
        // Do nothing
    }
    
    /// <summary>
    /// Stops the application
    /// </summary>
    public override void StopApplication()
    {
        AppGlobals.EventWaitHandle?.Reset();
        ApplicationServer?.StopApplication();
    }

    /// <summary>
    /// Create the view model for the main window
    /// </summary>
    public virtual IRxMainWindowViewModel CreateViewModel()
    {
        throw new NotSupportedException();
    }
}
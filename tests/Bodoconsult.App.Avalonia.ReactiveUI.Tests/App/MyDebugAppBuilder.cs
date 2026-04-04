// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.DependecyResolvers;
using Bodoconsult.App.ReactiveUI.Interfaces;
using ReactiveUI;
using ReactiveUI.Avalonia;
using ReactiveUI.Builder;
using Splat;

namespace Bodoconsult.App.ReactiveUI.Tests.App;

public class MyDebugAppBuilder : BaseDebugAppBuilder
{
    public MyDebugAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    {
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

        var appB = dpr.CreateReactiveUIBuilder()
            .WithAvalonia(); // Register WPF platform services

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
    }

    /// <summary>
    /// Load view location
    /// </summary>
    /// <param name="locator">The locator to use for the app instance</param>
    public virtual void LoadViewLocation(DefaultViewLocator locator)
    {
        // Do nothing
    }
}
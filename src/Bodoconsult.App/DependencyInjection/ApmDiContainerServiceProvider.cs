// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.PerformanceLogging;

namespace Bodoconsult.App.DependencyInjection;

/// <summary>
/// Loads all APM specific services to DI container. Intended mainly for production
/// </summary>
public class ApmDiContainerServiceProvider : IDiContainerServiceProvider
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appStartParameter">Current app start parameter</param>
    /// <param name="statusMessageDelegate">Status message delegate sending a status message to UI</param>
    public ApmDiContainerServiceProvider(IAppStartParameter appStartParameter, StatusMessageDelegate? statusMessageDelegate)
    {
        ArgumentNullException.ThrowIfNull(statusMessageDelegate);
        AppStartParameter = appStartParameter;
        StatusMessageDelegate = statusMessageDelegate;
    }

    /// <summary>
    /// Current app start parameter
    /// </summary>
    public IAppStartParameter AppStartParameter { get; }

    /// <summary>
    /// Current status message delegate to be called from the logging method
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        if (AppStartParameter.IsPerformanceLoggingActivated)
        {
            diContainer.AddSingleton<IPerformanceLogger, PerformanceLogger>();
            diContainer.AddSingleton<IPerformanceLoggerManager, PerformanceLoggerManager>();
        }
        else
        {
            diContainer.AddSingleton<IPerformanceLogger, FakePerformanceLogger>();
            diContainer.AddSingleton<IPerformanceLoggerManager, FakePerformanceLoggerManager>();
        }
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer"></param>
    public void LateBindObjects(DiContainer diContainer)
    {
        // Activate performance logging
        var perflog = diContainer.Get<IPerformanceLoggerManager>();

        perflog.StatusMessageDelegate = StatusMessageDelegate;

        perflog.StartLogging();
    }
}
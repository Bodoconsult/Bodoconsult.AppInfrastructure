// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using System.Threading;

namespace Bodoconsult.App.BackgroundService.AppStarter;

// https://www.codegenes.net/blog/graceful-shutdown-with-generic-host-in-net-core-2-1/

/// <summary>
/// <see cref="IAppStarter"/> implementation for a background service NOT using GRPC
/// </summary>
public class BackgroundServiceAppStarter : Microsoft.Extensions.Hosting.BackgroundService, IAppStarter
{
    private readonly IAppLoggerProxy _logger;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="logger">Current app logger</param>
    /// <param name="appBilder">Current app builder</param>
    public BackgroundServiceAppStarter(IAppLoggerProxy logger, IAppBuilder appBilder)
    {
        _logger = logger;
        AppBuilder = appBilder;
        AppBuilder.LoadAppStarterUi( this);
    }

    /// <summary>
    /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
    /// the lifetime of the longrunning operation(s) being performed.
    /// </summary>
    /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the longrunning operations.</returns>
    /// <remarks>See <see href="https://learn.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.</remarks>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var task = Task.Run(()=>Start(stoppingToken), stoppingToken);

        await Task.Run(() =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var isStopped = stoppingToken.IsCancellationRequested
                                    || task.IsCompleted
                                    || task.IsCanceled ||
                                    task.IsFaulted;
                    if (isStopped)
                    {
                        break;
                    }
                    Task.Delay(100, stoppingToken);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Start failed", e);
                return false;
            }

        });

        AppBuilder.StopApplication();
    }

    /// <summary>
    /// The current app start process handler
    /// </summary>
    public IAppBuilder AppBuilder { get; }

    /// <summary>
    /// Start the app
    /// </summary>
    /// <param name="stoppingToken"></param>
    public void Start(CancellationToken stoppingToken)
    {
        try
        {
            AppBuilder.StartApplicationService(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Start failed", e);
        }
    }

    /// <summary>
    /// Start the app
    /// </summary>
    public void Start()
    {
        // Do nothing
    }

    /// <summary>
    /// Show a message and then terminate the app
    /// </summary>
    /// <param name="message">Message to show before app termination</param>
    /// <param name="appTitle">App title to set</param>
    public void TerminateAppWithMessage(string message, string appTitle)
    {
           // Do nothing 
    }

    /// <summary>
    /// Handle an exception raised
    /// </summary>
    /// <param name="ex">Exception raised</param>
    public void HandleException(Exception ex)
    {
        throw new NotImplementedException();
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;

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
        _logger.LogInformation("Service initialized");
        AppBuilder = appBilder;
        AppBuilder.LoadAppStarterUi(this);
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
        var error = false;

        try
        {
            _logger.LogInformation("Service starts...");

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(() => Start(stoppingToken), stoppingToken);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            while (!stoppingToken.IsCancellationRequested)
            {
                var isStopped = stoppingToken.IsCancellationRequested;

                if (isStopped)
                {
                    Debug.Print("Service was stopped");
                    _logger.LogInformation("Service was stopped");
                    break;
                }
                // ReSharper disable once MethodSupportsCancellation
                await Task.Delay(100);
                Debug.Print("Service is running");
            }
        }
        catch (OperationCanceledException)
        {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        }
        catch (Exception e)
        {
            _logger.LogError("Running service failed", e);
            error = true;
        }

        try
        {
            AppBuilder.StopApplication();
            _logger.LogInformation("Service stopped");
        }
        catch (Exception e)
        {
            _logger.LogError("Stopping service failed", e);
            error = true;
        }

        // Stop logging now
        try
        {
            if (_logger != null)
            {
                _logger.StopLogging();
                _logger.Dispose();
            }
        }
        catch
        {
            // Do nothing
        }

        if (error)
        {
            Environment.Exit(1);
        }
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
        _logger.LogError("Handle exception", ex);
    }
}
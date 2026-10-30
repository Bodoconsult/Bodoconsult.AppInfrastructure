// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using OpenSourceConnectorBodoCms.Business.Services;
using System.Diagnostics;
using System.Runtime.Versioning;
using OpenSourceConnectorBodoCms.Business.Interfaces;

namespace OpenSourceConnectorBodoCms.Business.App;

/// <summary>
/// Current implementation of <see cref="IApplicationService"/>
/// </summary>
[SupportedOSPlatform("windows")]
public class OpenSourceConnectorBodoCmsService : IApplicationService
{
    private bool _isStopped;
    private bool _isStarting;
    private readonly IAppLoggerProxy _appLogger;

    /// <summary>
    /// Default ctor
    /// </summary>
    public OpenSourceConnectorBodoCmsService(IAppLoggerProxy appLogger,
        IAppGlobals appGlobals)
    {
        _appLogger = appLogger;
        AppGlobals = appGlobals;
        Offline = false;
    }

    /// <summary>
    /// Request application stop delegate
    /// </summary>
    public RequestApplicationStopDelegate? RequestApplicationStopDelegate { get; set; }

    /// <summary>
    /// Current app globals
    /// </summary>
    public IAppGlobals AppGlobals { get; }

    /// <summary>
    /// Application status offline true / false
    /// </summary>
    public bool Offline { get; set; }

    /// <summary>
    /// Register required services like GRPC clients etc.
    /// </summary>
    public void RegisterServices()
    {
        // Do nothing in this demo
    }

    /// <summary>
    /// Start the application
    /// </summary>
    /// <param name="cancellationToken"></param>
    public void StartApplication(CancellationToken? cancellationToken)
    {
        _isStarting = true;

        if (_isStopped)
        {
            return;
        }

        _isStarting = false;

        // Do start your workload here

        _appLogger.LogInformation("BodoCms starts creating website");

        ArgumentException.ThrowIfNullOrEmpty(AppGlobals.AppStartParameter.DefaultConnectionString);
        ArgumentException.ThrowIfNullOrEmpty(AppGlobals.AppStartParameter.ErrorMailAddress);

        var status = AppGlobals.StatusMessageDelegate;
        ArgumentNullException.ThrowIfNull(status);

        status.Invoke("OpenSourceConnectorToBodoCms: started...");

        var main = AppGlobals.DiContainer.Get<IOpenSourcePackagesHandler>();
        main.GetAllProjectFolders();
        main.ExportPackageInfoToBodoCms();
        
        _appLogger.LogInformation("OpenSourceConnectorToBodoCms has finished creating website");

        // Stop app now
        if (RequestApplicationStopDelegate is null)
        {
            return;
        }

        // Fire app stop now if workload is done
        AsyncHelper.FireAndForget(RequestApplicationStopDelegate.Invoke);
    }

    /// <summary>
    /// Suspend the app
    /// </summary>
    public void SuspendApplication()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Restart the app if it is in suspend state
    /// </summary>
    public void RestartApplication()
    {
        throw new NotSupportedException();
    }

    //private void OnLicenseMissingEvent(object sender, LicenseMissingArgs e)
    //{
    //    var msg = $"License is missing: {e.ErrorMessage}. Application will stop";
    //    _appLogger.LogError(msg);
    //    StopApplication();

    //    LicenseMissingDelegate?.Invoke(msg);
    //}

    /// <summary>
    /// Stop the application
    /// </summary>
    public void StopApplication()
    {
        _appLogger.LogWarning("Stopping application starts...");

        // Start process running? If yes leave here
        if (_isStarting)
        {
            return;
        }

        // Do not stop more than one time
        if (_isStopped)
        {
            return;
        }

        _isStopped = true;



        // Do all needed to stop youe app correctly
        var di = Globals.Instance.DiContainer;

        try
        {
            // Stop performance logging
            var perflog = di.Get<IPerformanceLoggerManager>();
            perflog?.StopLogging();
        }
        catch (Exception e)
        {
            Debug.Print(e.Message);
            //_appLogger.LogError($"Performance logging could not be stopped", new object[]{e});
        }


        //var gms = di.Get<IGeneralAppManagementManager>();
        //var request = new EmptyBusinessTransactionRequestData();

        //DefaultBusinessTransactionReply result;

        //// Create log dump on app stop
        //try
        //{

        //    // ToDo: fill request with useful information for logging
        //    result = gms.CreateLogDump(request);

        //    //if (result != null)
        //    //{
        //        _appLogger?.LogWarning($"CreateLogDump: error code {result.ErrorCode}: {result.Message}");
        //    //}
        //}
        //catch
        //{
        //    // Do nothing
        //}

        // Stop logging now
        try
        {
            if (_appLogger != null)
            {
                _appLogger.StopLogging();
                _appLogger.Dispose();
            }

        }
        catch
        {
            // Do nothing
        }

    }

    /// <summary>
    /// Current <see cref="IApplicationService.LicenseMissingDelegate"/>
    /// </summary>
    public LicenseMissingDelegate? LicenseMissingDelegate { get; set; }
}
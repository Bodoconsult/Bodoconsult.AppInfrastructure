// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

#region GRPC copyright notice and license

// Copyright 2019 The gRPC Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Diagnostics;
using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.Interfaces;

namespace GrpcServerApp.Grpc.App;

/// <summary>
/// Current implementation of <see cref="IApplicationService"/>
/// </summary>
public class GrpcServerAppService : IApplicationService
{
    private bool _isStopped;
    private bool _isStarting;

    private readonly IAppLoggerProxy _appLogger;

    /// <summary>
    /// Default ctor
    /// </summary>
    public GrpcServerAppService(IAppLoggerProxy appLogger,
        IAppGlobals appGlobals)
    {
        _appLogger = appLogger;
        AppGlobals = appGlobals;
        Offline = false;
    }

    /// <summary>
    /// Request application stop delegate
    /// </summary>
    public RequestApplicationStopDelegate RequestApplicationStopDelegate { get; set; }

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
        // Do nothing
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

        //// Do start your workload here
        //var i = 0;

        //Debug.Print("");
        //while (i < 15)
        //{
        //    Debug.Print("Processing workload...");
        //    Console.WriteLine("Processing workload...");
        //    AsyncHelper.Delay(1000);
        //    i++;
        //}



        //if (RequestApplicationStopDelegate == null)
        //{
        //    return;
        //}

        //// Fire app stop now if workload is done
        //AsyncHelper.FireAndForget(RequestApplicationStopDelegate.Invoke);
    }

    /// <summary>
    /// Suspend the app
    /// </summary>
    public void SuspendApplication()
    {

        if (_isStopped)
        {
            return;
        }

        // Clear DI container: more sophisticated implementation for single instances in the container may be required
        var di = Globals.Instance.DiContainer;
        di.ClearAll();
    }

    /// <summary>
    /// Restart the app if it is in suspend state
    /// </summary>
    public void RestartApplication()
    {
        //if (_isStopped)
        //{
        //    return;
        //}

        //if (_isStarting)
        //{
        //    return;
        //}

        // ToDo: RL: Restart app
    }

    private void OnLicenseMissingEvent(object sender, LicenseMissingArgs e)
    {
        var msg = $"License is missing: {e.ErrorMessage}. Application will stop";
        _appLogger.LogError(msg);
        StopApplication();

        LicenseMissingDelegate?.Invoke(msg);
    }

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


        var gms = di.Get<IGeneralAppManagementManager>();
        var request = new EmptyBusinessTransactionRequestData();

        DefaultBusinessTransactionReply result;

        // Create log dump on app stop
        try
        {

            // ToDo: fill request with useful information for logging
            result = gms.CreateLogDump(request);

            if (result != null)
            {
                _appLogger?.LogWarning($"CreateLogDump: error code {result.ErrorCode}: {result.Message}");
            }
        }
        catch
        {
            // Do nothing
        }

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
    public LicenseMissingDelegate LicenseMissingDelegate { get; set; }

}
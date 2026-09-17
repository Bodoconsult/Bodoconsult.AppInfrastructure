// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Interfaces;
using Bodoconsult.Web.Mail.Helpers;
using BodoFileTransfer.Business.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;

namespace BodoFileTransfer.Business.App;

/// <summary>
/// Current implementation of <see cref="IApplicationService"/>
/// </summary>
public class BodoFileTransferService : IApplicationService
{
    private bool _isStopped;
    private bool _isStarting;
    private readonly IAppLoggerProxy _appLogger;

    /// <summary>
    /// Default ctor
    /// </summary>
    public BodoFileTransferService(IAppLoggerProxy appLogger,
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
        BasicConfig();
        Start();

        // Stop app now
        if (RequestApplicationStopDelegate is null)
        {
            return;
        }

        // Fire app stop now if workload is done
        AsyncHelper.FireAndForget(RequestApplicationStopDelegate.Invoke);
    }

    private static void BasicConfig()
    {
        PasswordHandler.Key1 = "Mail2020";
        PasswordHandler.Key2 = "2020Mail";
        PasswordHandler.Key3 = "20Mail20";
        PasswordHandler.Salt = [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76];
    }

    private void Start()
    {
        int modus;
        var accountId = 0;

        var args = AppGlobals.AppStartParameter.Args ?? [];

        if (args.Length == 0)
        {
            Status("Mode 0");
            modus = 0;
        }
        else
        {
            switch (args[0])
            {
                case "2":
                    Status("Mode 2");
                    modus = 2;
                    try
                    {
                        accountId = Convert.ToInt32(args[1]);
                    }
                    catch
                    {
                        modus = 0;
                    }
                    break;

                case "1":
                    Status("Mode 1");
                    modus = 1;
                    break;
                default:
                    Status("Mode 0");
                    modus = 0;
                    break;
            }
        }


//#if DEBUG
//        modus = 1;
//#endif

        Status("Init BodoFileTransfer done!");

        var fh = AppGlobals.DiContainer.Get<IFolderHandler>();
        fh.LoadAccounts();

        //Status($"SentDateFilter: {fh.SentDateFilter}");

        Status($"Run operations for current mode {modus}");

        fh.ProcessO365Accounts();
        fh.ProcessImapAccounts();
        fh.ProcessAccounts();

        switch (modus)
        {
            case 2: // Manual start for all manually sending accounts
                fh.ManuallySendOutboundMails(accountId);
                break;
            case 1: // Full job for all not manually sending accounts
                fh.SendOutboundMails();
                break;
            default:    // Collect only files job for all not manually sending accounts
                break;

        }

        Status("BodoFileTransfer done!");
    }

    public void Status(string msg)
    {
        if (msg.Contains("Error"))
        {
            _appLogger.LogError(msg);
        }
        else
        {
            _appLogger.LogInformation(msg);
        }
        AppGlobals.StatusMessageDelegate?.Invoke(msg);
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


        var gms = di.Get<IGeneralAppManagementManager>();
        var request = new EmptyBusinessTransactionRequestData();

        DefaultBusinessTransactionReply result;

        // Create log dump on app stop
        try
        {

            // ToDo: fill request with useful information for logging
            result = gms.CreateLogDump(request);

            //if (result != null)
            //{
            _appLogger?.LogWarning($"CreateLogDump: error code {result.ErrorCode}: {result.Message}");
            //}
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
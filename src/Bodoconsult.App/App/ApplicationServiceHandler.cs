//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System.Diagnostics;
//using Bodoconsult.App.Exceptions;
//using Bodoconsult.App.Interfaces;

//namespace Bodoconsult.App;

///// <summary>
///// Current implementation of <see cref="IApplicationServiceHandler"/> for console apps
///// </summary>
//public class ApplicationServiceHandler : IApplicationServiceHandler
//{

//    /// <summary>
//    /// Default ctor
//    /// </summary>
//    public ApplicationServiceHandler(IAppGlobals appGlobals)
//    {
//        AppGlobals = appGlobals;
//        AppGlobals.LicenseMissingDelegate = TerminateIfLicenseMissing;
//        AppGlobals.StatusMessageDelegate = StatusMessageDelegate;
//    }

//    /// <summary>
//    /// Current app server
//    /// </summary>
//    public IApplicationService ApplicationServer { get; private set; }


//    /// <summary>
//    /// App globals
//    /// </summary>
//    public IAppGlobals AppGlobals { get; }

//    /// <summary>
//    /// Current <see cref="IAppStarterUi"/> instance
//    /// </summary>
//    public IAppStarter AppStarterUi { get; private set; }

//    /// <summary>
//    /// Load the current app starter in the start process
//    /// </summary>
//    /// <param name="appStarter">Current <see cref="IAppStarterUi"/> instance</param>
//    public void SetAppStarterUi(IAppStarter appStarter)
//    {
//        AppStarterUi = appStarter;
//    }


//    /// <summary>
//    /// Starts the application in one step. Calls CheckStorageConnection() => RegisterGlobalAndDatabaseServices() => FinalizeSetupForGlobalAndDatabaseServices() => StartApplicationService()
//    /// </summary>
//    public void StartApplication()
//    {
       
//        RegisterGlobalAndDatabaseServices();

//        FinalizeSetupForGlobalAndDatabaseServices();

//        StartApplicationService();
//    }

//    /// <summary>
//    /// Start the application service
//    /// </summary>
//    public void StartApplicationService()
//    {
//        ApplicationServer = AppGlobals.DiContainer.Get<IApplicationService>();
//        ApplicationServer.RequestApplicationStopDelegate = RequestApplicationStop;
//        ApplicationServer.RegisterServices();
//        ApplicationServer.LicenseMissingDelegate = TerminateIfLicenseMissing;
//        ApplicationServer.StartApplication();

//        AppGlobals.Logger.LogWarning($"{AppGlobals.AppStartParameter.AppName} app is started!");
//    }

//    /// <summary>
//    /// Register all DI container services required for app start. Do NOT use directly. Only public for unit testing
//    /// </summary>
//    public void RegisterGlobalAndDatabaseServices()
//    {
//        try
//        {
//            //_package.AddServices(AppGlobals.DiContainer);
//        }
//        catch (Exception e)
//        {
//            Debug.Print($"{e}");
//            AppGlobals.Logger?.LogError("Setting up DI container failed", e);
//            ApplicationServer?.StopApplication();
//        }
//    }

//    /// <summary>
//    /// Finalized the setup of the loaded DI services
//    /// </summary>
//    public void FinalizeSetupForGlobalAndDatabaseServices()
//    {

//        //_package.LateBindObjects(AppGlobals.DiContainer);

//        AppGlobals.Logger.LogDebug($"{AppGlobals.AppStartParameter.AppName} app global and database services successfully registered!");

//        //// Di tests to find circular refernces in DI container
//        //var a = Globals.Instance.DiContainer.Get<ITTraceManagementDelegate>();
//        //var b = Globals.Instance.DiContainer.Get<ITTowerMagazineSlotService>();
//        //var c = Globals.Instance.DiContainer.Get<ServerUiRequestsImplementationFactory>().Invoke();
//    }

//    /// <summary>
//    /// Suspend the app
//    /// </summary>
//    public void SuspendApplication()
//    {
//        AppGlobals.Logger.LogWarning($"{AppGlobals.AppStartParameter.AppName} app is going to suspend mode...");
//        ApplicationServer.SuspendApplication();
//    }

//    /// <summary>
//    /// Restart the app if it is in suspend state
//    /// </summary>
//    public void RestartApplication()
//    {
//        if (AppGlobals.Logger is null)
//        {
//            throw new ArgumentException("Logger is null");
//        }

//        AppGlobals.Logger.LogWarning($"{AppGlobals.AppStartParameter.AppName} app is recovering from suspend mode...");

//        RegisterGlobalAndDatabaseServices();

//        FinalizeSetupForGlobalAndDatabaseServices();

//        AppGlobals.Logger = AppGlobals.DiContainer.Get<IAppLoggerProxy>();
//        AppGlobals.Logger.LogInformation("Global and database services successfully registered!");

//        AppGlobals.Logger.LogInformation($"{AppGlobals.AppStartParameter.AppName} app restarts...");
//        AppGlobals.Logger.LogInformation(AppGlobals.AppStartParameter.AppVersion);
//        Console.WriteLine(AppGlobals.AppStartParameter.AppVersion);

//        StartApplicationService();
//    }

//    private void RequestApplicationStop()
//    {
//        StopApplication();

//        AppStarterUi.TerminateAppWithMessage("App shutdown requested", AppGlobals.AppStartParameter.AppName);
        
//    }

//    /// <summary>
//    /// Stops the application
//    /// </summary>
//    public void StopApplication()
//    {
//        AppGlobals.EventWaitHandle?.Reset();
//        ApplicationServer?.StopApplication();
//    }





//    private void StatusMessageDelegate(string message)
//    {
//        AppGlobals.Logger?.LogInformation(message);
//    }

//    protected void TerminateIfLicenseMissing(string message)
//    {
//        AppGlobals.Logger?.LogError("License not found");

//        AppStarterUi?.TerminateAppWithMessage(UiMessages.MsgLicenseNotFoundNowTerminate, AppGlobals.AppStartParameter.AppName);
//    }
//}

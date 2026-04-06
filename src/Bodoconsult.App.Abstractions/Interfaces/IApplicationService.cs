// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Top level interface for application handling
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// Request application stop delegate
    /// </summary>
    RequestApplicationStopDelegate RequestApplicationStopDelegate { get; set; }

    /// <summary>
    /// Current app globals
    /// </summary>
    public IAppGlobals AppGlobals { get; }

    /// <summary>
    /// Application status offline true / false
    /// </summary>
    bool Offline { get; set; }

    /// <summary>
    /// Start the application
    /// </summary>
    /// <param name="cancellationToken">Current cancellation token or null</param>
    void StartApplication(CancellationToken? cancellationToken);


    /// <summary>
    /// Suspend the app
    /// </summary>
    void SuspendApplication();

    /// <summary>
    /// Restart the app if it is in suspend state
    /// </summary>
    void RestartApplication();


    /// <summary>
    /// Stop the application
    /// </summary>
    void StopApplication();

    /// <summary>
    /// Current <see cref="LicenseMissingDelegate"/>
    /// </summary>
    public LicenseMissingDelegate LicenseMissingDelegate { get; set; }

    //void ServerShutdown();

    /// <summary>
    /// Register required services like GRPC clients etc.
    /// </summary>
    void RegisterServices();


}
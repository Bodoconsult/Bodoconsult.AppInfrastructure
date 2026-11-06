// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Interfaces;

namespace BodoWebReporting.Dal.Handler;

/// <summary>
/// Base class for reporting object handlers
/// </summary>
public abstract class BaseObjectHandler
{
    protected readonly IDataHandler DataHandler;

    protected readonly IBodoWebReportingAppGlobals AppGlobals;


    protected StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="appGlobals">Current app globals</param>
    protected BaseObjectHandler(IDataHandler dataHandler, IAppGlobals appGlobals)
    {
        DataHandler = dataHandler;

        if (appGlobals is not IBodoWebReportingAppGlobals globals)
        {
            throw new ArgumentException("appGlobals is not IBodoWebReportingAppGlobals");
        }

        AppGlobals = globals;

        StatusMessageDelegate = appGlobals.StatusMessageDelegate ?? Console.WriteLine;
    }


    /// <summary>
    /// Statusanzeige
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    protected void SetStatus(string msg)
    {
        StatusMessageDelegate.Invoke(msg);
    }
}
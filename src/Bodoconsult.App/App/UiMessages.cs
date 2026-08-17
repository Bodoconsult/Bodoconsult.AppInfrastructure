// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Exceptions;

namespace Bodoconsult.App;

/// <summary>
/// Messages for WinForms UI and console
/// </summary>
public static class UiMessages
{
    /// <summary>
    /// Message for app is started and ready for processing workload
    /// </summary>
    public static string MsgAppIsReady => "App is started and ready for processing workload!";

    /// <summary>
    /// Message for app is listening on certain port
    /// </summary>
    public static string MsgServerIsListeningOnPort => "App is listening on port ";

    /// <summary>
    /// Message showing how to stop the app
    /// </summary>
    public static string MsgHowToShutdownServer => "Press CTRL+C (STRG+C) to stop the app...";

    /// <summary>
    /// Message for is app stop requested
    /// </summary>
    public static string Exit => "Do you want to stop the app?";

    /// <summary>
    /// Message for being sure the app stop should be requested
    /// </summary>
    public static string ExitForced => "Do you really want to stop the app?";

    /// <summary>
    /// Stop the app message
    /// </summary>
    public static string ExitHeader => "Stop the app";
    
    /// <summary>
    /// Message for no license found and the app will be terminating in a certain amount of time
    /// </summary>
    public static string MsgLicenseNotFoundTerminateTime => "License not found. Application will terminate in 5s";
    
    /// <summary>
    /// Message shown if there is no license and the app will be terminated therefore
    /// </summary>
    public static string MsgLicenseNotFoundNowTerminate => "License not found. Application will terminate!";

    /// <summary>
    /// Current process ID message
    /// </summary>
    public static string MsgServerProcessId => "App main process ID: ";

    /// <summary>
    /// Handles an exception and returns a string to use in UI
    /// </summary>
    /// <param name="e">Raised exception</param>
    /// <returns>String with information for the raised exception</returns>
    public static string HandleException(Exception? e)
    {
        if (e is null)
        {
            return string.Empty;
        }

        // RL: Check for licence exception here too: it may be the inner exception
        var ex = ContainsLicenseMissingException(e);
        if (ex != null)
        {
            return $"{MsgLicenseNotFoundTerminateTime}:{ex.Message}";
        }

        const string msg = "An error occurs when trying to start application server";

        return $"{msg} {e.InnerException ?? e}: {e.StackTrace}";
    }

    /// <summary>
    /// Check all inner exceptions for <see cref="LicenseMissingException"/>
    /// </summary>
    /// <param name="e">Current exception</param>
    /// <returns>A <see cref="LicenseMissingException"/> contained in the exception</returns>
    public static LicenseMissingException? ContainsLicenseMissingException(Exception e)
    {
        while (true)
        {
            switch (e)
            {
                case null:
                    return null;
                case LicenseMissingException ex:
                    return ex;
                default:
                    if (e.InnerException is null)
                    {
                        break;
                    }
                    e = e.InnerException;
                    break;
            }
        }
    }
}
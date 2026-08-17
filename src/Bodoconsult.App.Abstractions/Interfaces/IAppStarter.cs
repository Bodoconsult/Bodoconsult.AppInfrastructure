// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for application starter classes
/// </summary>
public interface IAppStarter : IDisposable
{
    /// <summary>
    /// The current app start process handler
    /// </summary>
    IAppBuilder AppBuilder { get; }

    /// <summary>
    /// Start the app
    /// </summary>
    void Start();

    /// <summary>
    /// Show a message and then terminate the app
    /// </summary>
    /// <param name="message">Message to show before app termination</param>
    /// <param name="appTitle">App title to set</param>
    void TerminateAppWithMessage(string message, string appTitle);

    /// <summary>
    /// Handle an exception raised
    /// </summary>
    /// <param name="ex">Exception raised</param>
    void HandleException(Exception? ex);
}
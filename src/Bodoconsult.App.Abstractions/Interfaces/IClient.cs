// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for notification client implementations
/// </summary>
public interface IClient
{
    /// <summary>
    /// Current client manager instance
    /// </summary>
    IClientManager ClientManager { get; }

    /// <summary>
    /// information about connected client
    /// </summary>
    IClientLoginData ClientData { get; }

    /// <summary>
    /// Connection state
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// All allowed notifications
    /// </summary>
    public IList<string> AllowedNotifications { get; }

    /// <summary>
    /// Wait until connection result is set
    /// </summary>
    /// <returns>true if connected</returns>
    Task<bool> WaitForConnectionResult();

    /// <summary>
    /// Start the connection with the client
    /// </summary>
    void StartConnection();

    /// <summary>
    /// Check if the notification is allowed to be sent to the client
    /// </summary>
    /// <param name="notification">Notification to check</param>
    /// <returns>True if the notification should be sent to the client else false</returns>
    bool CheckNotification(IClientNotification notification);
    
    /// <summary>
    /// Do notify the client
    /// </summary>
    /// <param name="notification">Current notification to send to the client instance</param>
    void DoNotifyClient(IClientNotification notification);

    /// <summary>
    /// Load the current client manager instance
    /// </summary>
    /// <param name="clientManager">Current client manager instance</param>
    void LoadClientManager(IClientManager clientManager);

}
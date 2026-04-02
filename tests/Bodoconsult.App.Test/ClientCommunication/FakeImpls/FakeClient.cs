// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Test.ClientCommunication.FakeImpls;

/// <summary>
/// Fake implementation of <see cref="IClient"/>
/// </summary>
public class FakeClient : IClient
{
    /// <summary>
    /// Is the current notification to send to the client valid?
    /// </summary>
    public bool NotificationIsValid { get; set; } = true;

    /// <summary>
    /// Was <see cref="DoNotifyClient"/> fired?
    /// </summary>
    public bool WasFired { get; set; }

    /// <summary>
    /// Current client manager instance
    /// </summary>
    public IClientManager ClientManager { get; private set; }

    /// <summary>
    /// information about connected client
    /// </summary>
    public IClientLoginData ClientData { get; set; }

    /// <summary>
    /// Connection state
    /// </summary>
    public bool IsConnected { get; set; }

    /// <summary>
    /// All allowed notifications
    /// </summary>
    public IList<string> AllowedNotifications { get; set; }

    /// <summary>
    /// Wait until connection result is set
    /// </summary>
    /// <returns>true if connected</returns>
    public Task<bool> WaitForConnectionResult()
    {
        return new Task<bool>(() => true);
    }

    public void StartConnection()
    {
        // Do nothing
    }

    /// <summary>
    /// Check if the notification is allowed to be sent to the client
    /// </summary>
    /// <param name="notification">Notification to check</param>
    /// <returns>True if the notification should be sent to the client else false</returns>
    public bool CheckNotification(IClientNotification notification)
    {
        return NotificationIsValid;
    }

    /// <summary>
    /// Do notify the client
    /// </summary>
    /// <param name="notification">Current notification to send to the client instance</param>
    public void DoNotifyClient(IClientNotification notification)
    {
        WasFired = true;
    }

    /// <summary>
    /// Load the current client manager instance
    /// </summary>
    /// <param name="clientManager">Current client manager instance</param>
    public void LoadClientManager(IClientManager clientManager)
    {
        ClientManager = clientManager;
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Concurrent;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for client connection manager implementations
/// </summary>
public interface IClientManager: IDisposable
{
    /// <summary>
    /// Current license manager
    /// </summary>
    IClientNotificationLicenseManager LicenseManager { get; }        
        
    /// <summary>
    /// Current logger
    /// </summary>
    IAppLoggerProxy AppLogger { get; }

    /// <summary>
    /// Current instance of <see cref="IClientMessagingService"/> to convert notifications to final format
    /// </summary>

    IClientMessagingService ClientMessagingService { get; }


    /// <summary>
    /// Currently connected clients
    /// </summary>
    ConcurrentDictionary<Guid, IClient> AllConnectedClients { get; }

    /// <summary>
    /// Add a connected client to the connected clients
    /// </summary>
    /// <param name="client">Current client to add to the connected clients</param>
    void AddClient(IClient client);

    /// <summary>
    /// Remove a client from the connected clients list
    /// </summary>
    /// <param name="client">Current client to remove</param>
    void RemoveClient(IClient client);


    /// <summary>
    /// Send a notification to all clients
    /// </summary>
    /// <param name="notification">Notification to send to the clients</param>
    void DoNotifyAllClients(IClientNotification notification);

}
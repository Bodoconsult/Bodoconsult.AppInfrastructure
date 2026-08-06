// Copyright (c) Royotech. All rights reserved.

using System.Collections.Concurrent;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.ClientNotifications;

/// <summary>
/// Current implementation of <see cref="IClientManager"/>
/// </summary>
public class ClientManager : IClientManager
{
    private readonly ProducerConsumerQueue<IClientNotification> _waitingQueue = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="licenseManager">Current license manager</param>
    /// <param name="appLogger">Current app logger</param>
    /// <param name="clientMessagingService">Current client messaging service</param>
    public ClientManager(IClientNotificationLicenseManager licenseManager, IAppLoggerProxy appLogger, IClientMessagingService clientMessagingService)
    {
        LicenseManager = licenseManager;
        AppLogger = appLogger;
        ClientMessagingService = clientMessagingService;
        _waitingQueue.ConsumerTaskDelegate = ConsumerTaskDelegate;
        _waitingQueue.StartConsumer();
    }

    private void ConsumerTaskDelegate(IClientNotification notification)
    {
        // Check if there are missing objects to send
        if (notification.NotificationObjectToSend == null)
        {
            // Create the transport level object and store it in the notification
            var message = ClientMessagingService.Convert(notification);
            notification.NotificationObjectToSend = message;
        }

        foreach (var connectedClient in AllConnectedClients.Values)
        {
            if (!connectedClient.CheckNotification(notification))
            {
                continue;
            }
            connectedClient.DoNotifyClient(notification);
        }
    }

    /// <summary>
    /// Current license manager
    /// </summary>
    public IClientNotificationLicenseManager LicenseManager { get; }

    /// <summary>
    /// Current logger
    /// </summary>
    public IAppLoggerProxy AppLogger { get; }

    /// <summary>
    /// Current instance of <see cref="IClientMessagingService"/> to convert notifications to final format
    /// </summary>
    public IClientMessagingService ClientMessagingService { get; }

    /// <summary>
    /// Currently connected clients
    /// </summary>
    public ConcurrentDictionary<Guid, IClient> AllConnectedClients { get; } = new();


    /// <summary>
    /// Add a connected client to the connected clients
    /// </summary>
    /// <param name="client">Current client to add to the connected clients</param>
    public void AddClient(IClient client)
    {

        if (!LicenseManager.CheckClientForReceivingNotifications(client))
        {
            return;
        }

        client.LoadClientManager(this);
        AllConnectedClients.GetOrAdd(client.ClientData.Id, client);
        AppLogger.LogInformation($"Client added {client.ClientData.Type} {client.ClientData.Id} Count: {AllConnectedClients.Count}");

    }

    /// <summary>
    /// Remove a client from the connected clients list
    /// </summary>
    /// <param name="client">Current client to remove</param>
    public void RemoveClient(IClient client)
    {
        if (AllConnectedClients.TryRemove(client.ClientData.Id, out _))
        {
            AppLogger.LogInformation($"Client removed {client.ClientData.Type} {client.ClientData.Id} Count: {AllConnectedClients.Count}");
        }
    }

    /// <summary>
    /// Send a notification to all clients
    /// </summary>
    /// <param name="notification">Notification to send to the clients</param>
    public void DoNotifyAllClients(IClientNotification notification)
    {
        _waitingQueue.Enqueue(notification);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _waitingQueue.StopConsumer();
        _waitingQueue?.Dispose();
    }
}
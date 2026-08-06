// Copyright (c) Royotech. All rights reserved.

using System.Collections.Concurrent;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.ClientNotifications;

/// <summary>
/// Current implementation of <see cref="IClientMessagingBusinessDelegate"/>
/// </summary>
public class ClientMessagingBusinessDelegate : IClientMessagingBusinessDelegate
{
    private readonly ConcurrentQueue<IClientNotification> _notifications = new();

    private IWatchDog? _watchDog;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="clientMessagingService"></param>
    /// <param name="clientManager"></param>
    public ClientMessagingBusinessDelegate(IClientMessagingService clientMessagingService, IClientManager clientManager)
    {
        ClientMessagingService = clientMessagingService;
        ClientManager = clientManager;
    }

    /// <summary>
    /// Event method to bind to current business logic class instance for messaging. Calls <see cref="IClientMessagingBusinessDelegate.DoNotifyClient"/>. Use this method to decouple messaging business logic from <see cref="IClientMessagingBusinessDelegate"/> impls.
    /// </summary>
    /// <param name="sender">Source (not used)</param>
    /// <param name="notification">Current notification to send to the clients</param>
    public void MessagingBusinessManagerOnNotifyClient(object sender, IClientNotification notification)
    {
        DoNotifyClient(notification);
    }

    /// <summary>
    /// Current client messaging service
    /// </summary>
    public IClientMessagingService ClientMessagingService { get; }

    /// <summary>
    /// Current client manager to use
    /// </summary>
    public IClientManager ClientManager { get; }

    /// <summary>
    /// Notify  a client
    /// </summary>
    /// <param name="notification">Notification to send to the client</param>
    public void DoNotifyClient(IClientNotification notification)
    {
        _notifications.Enqueue(notification);
    }

    /// <summary>
    /// Any notifications in the queue?
    /// </summary>
    public bool HasNotifications => _notifications.Any();

    /// <summary>
    /// Send the queued messages to the client. To be called internally by <see cref="IWatchDog"/> implementation. Public only for testing purposes
    /// </summary>
    public void Runner()
    {

        if (_notifications.IsEmpty)
        {
            return;
        }

        // Do NOT move inside the loop due to performance and gc issues
        // ReSharper disable once TooWideLocalVariableScope
        // ReSharper disable once InlineOutVariableDeclaration
        IClientNotification? notification;

        while (!_notifications.IsEmpty)
        {

            // Get the next notification from the queue
            var success = _notifications.TryDequeue(out notification);

            if (!success || notification == null)
            {
                Thread.Sleep(5);
                continue;
            }

            // Create the transport level object and store it in the notification
            var message = ClientMessagingService.Convert(notification);
            notification.NotificationObjectToSend = message;

            // Send the notification to all registered clients
            ClientManager.DoNotifyAllClients(notification);
        }
    }

    /// <summary>
    /// Start messaging to the client
    /// </summary>
    public void StartClientMessaging()
    {
        _watchDog ??= new WatchDog(Runner, 5);
        _watchDog.StartWatchDog();
    }

    /// <summary>
    /// Stop messaging to the client
    /// </summary>
    public void StopClientMessaging()
    {
        _watchDog?.StopWatchDog();
    }
}
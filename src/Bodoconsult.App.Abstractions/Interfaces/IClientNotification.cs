// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for implementing client notifications
/// </summary>
public interface IClientNotification
{
    /// <summary>
    /// The notification object to send via GRPC etc to the client
    /// </summary>
    object? NotificationObjectToSend { get; set; }

}
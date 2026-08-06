// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for client messaging services
/// </summary>
public interface IClientMessagingService
{

    /// <summary>
    /// All conversion rules for notifications event args to client transport level target object
    /// </summary>
    Dictionary<string, NotificationToTargetTransferObjectDelegate> ConversionRules { get; }

    /// <summary>
    /// Convert a notification into a client transport level target object
    /// </summary>
    /// <param name="notification">Current notification to send</param>
    /// <returns>Object to transfer to the client on transport level</returns>
    object? Convert(IClientNotification notification);

}
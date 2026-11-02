// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Sending a toast message to the operating system
/// </summary>
public interface IToastMessagingService : IDisposable
{
    /// <summary>
    /// Send a simple toast notification to the operating system
    /// </summary>
    /// <param name="notificationRequest">Notification request</param>
    void SendSimpleToastMessage(NotifyRequestRecord notificationRequest);
}
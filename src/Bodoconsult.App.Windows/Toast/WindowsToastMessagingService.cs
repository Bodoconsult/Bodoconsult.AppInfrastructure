// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Bodoconsult.App.Windows.Toast;

/// <summary>
/// Windows implementation for <see cref="IToastMessagingService"/>
/// </summary>
public class WindowsToastMessagingService : IToastMessagingService
{
    /// <summary>
    /// Send a simple toast notification to the operating system
    /// </summary>
    /// <param name="notificationRequest">Notification request</param>
    public void SendSimpleToastMessage(NotifyRequestRecord notificationRequest)
    {
        new ToastContentBuilder()
            .AddText(notificationRequest.Title)
            .AddText(notificationRequest.Text)
            .Show();
    }
}
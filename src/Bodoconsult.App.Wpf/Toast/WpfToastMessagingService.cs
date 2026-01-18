// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Bodoconsult.App.Wpf.Toast;

/// <summary>
/// WPF implementation for <see cref="IToastMessagingService"/>
/// </summary>
public class WpfToastMessagingService : IToastMessagingService
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
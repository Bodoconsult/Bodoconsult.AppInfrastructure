// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

//using Microsoft.Windows.AppNotifications;
//using Microsoft.Windows.AppNotifications.Builder;

namespace Bodoconsult.App.Windows.Toast;

// https://learn.microsoft.com/de-de/windows/apps/develop/notifications/app-notifications/app-notifications-console

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
        //new ToastContentBuilder()
        //    .AddText(notificationRequest.Title)
        //    .AddText(notificationRequest.Text)
        //    .Show();

        var notification = new AppNotificationBuilder()
            //.AddArgument("action", "viewItem")
            .AddText(notificationRequest.Title)
            .AddText(notificationRequest.Text)
            //.AddButton(new AppNotificationButton("Acknowledge")
            //    .AddArgument("action", "acknowledge"))
            .BuildNotification();

        AppNotificationManager.Default.Show(notification);

        //throw new NotImplementedException("Currently not implemented due to NET 10 issue with NUGET package");
    }
}
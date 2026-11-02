// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Windows.ApplicationModel.DynamicDependency;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

// https://learn.microsoft.com/de-de/windows/apps/develop/notifications/app-notifications/app-notifications-console

namespace Bodoconsult.App.WinForms.Toast;

/// <summary>
/// WinFormsimplementation for <see cref="IToastMessagingService"/>
/// </summary>
public class WinFormsToastMessagingService : IToastMessagingService
{
    private readonly bool _isError;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WinFormsToastMessagingService()
    {
        Bootstrap.TryInitialize(0x00020005, out var result);
        if (result != 0)
        {
            _isError=true;
            return;
        }

        AppNotificationManager.Default.Register();
    }

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

        if (_isError)
        {
            return;
        }

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

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        AppNotificationManager.Default.Unregister();
    }
}
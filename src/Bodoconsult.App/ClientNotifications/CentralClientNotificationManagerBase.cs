// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.ClientNotifications;

/// <summary>
/// Current implementation of <see cref="ICentralClientNotificationManager"/> for central event notification handling
/// </summary>
public abstract class CentralClientNotificationManagerBase : ICentralClientNotificationManager
{
    #region Delegate definitions

    /// <summary>
    /// Delegate for sending a notification to the client
    /// </summary>
    public TransferToClientDelegate NotifyClient { get; set; }

    #endregion

    /// <summary>
    /// Send a progress notification
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="currentProgressType">Current progress type. Define your own types in an enum</param>
    /// <param name="percentage">Current percentage</param>
    /// <param name="complete">Is completed?</param>
    public void DoNotifyProgressEvent(object sender, int currentProgressType, int percentage, bool complete)
    {
        var notification = new ProgressNotification
        {
            Completed = complete,
            Type = currentProgressType,
            Progress = percentage
        };

        NotifyClient?.Invoke(sender, notification);
    }

    /// <summary>
    /// Send an exception notification
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Exception to report</param>
    public void DoNotifyException(object sender, Exception e)
    {
        var notification = new ExceptionNotification
        {
            Exception = e
        };

        NotifyClient?.Invoke(sender, notification);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.ClientNotifications;

/// <summary>
/// Fake implementation of <see cref="ICentralClientNotificationManager"/> doing nothing
/// </summary>
public class FakeICentralClientNotificationManager : ICentralClientNotificationManager
{
    /// <summary>
    /// Delegate for sending a notification to the client
    /// </summary>
    public TransferToClientDelegate NotifyClient { get; set; } = DummyNotifyClient;

    private static void DummyNotifyClient(object source, IClientNotification notification)
    {
        // Do nothing
    }

    /// <summary>
    /// Send progress notification
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="currentProgressType">Current progress type. Define your own types in an enum</param>
    /// <param name="percentage">Current percentage</param>
    /// <param name="complete">Is completed?</param>
    public void DoNotifyProgressEvent(object sender, int currentProgressType, int percentage, bool complete)
    {
        // Do nothing
    }

    /// <summary>
    /// Send an exception notification
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Exception to report</param>
    public void DoNotifyException(object sender, Exception e)
    {
        // Do nothing
    }
}
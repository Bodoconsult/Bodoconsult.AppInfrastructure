// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ClientNotifications;

/// <summary>
/// Notification data for [exception has been thrown] event
/// </summary>
public class ExceptionNotification : BaseClientNotification
{
    /// <summary>
    /// Exception to report
    /// </summary>
    public Exception? Exception { get; set; }
}
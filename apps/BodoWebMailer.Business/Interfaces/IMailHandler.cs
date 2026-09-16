// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace BodoWebMailer.Business.Interfaces;

/// <summary>
/// Interface for mail handling instances
/// </summary>
public interface IMailHandler
{
    /// <summary>
    /// Delegate for handling status messages for console
    /// </summary>
    public StatusMessageDelegate StatusChanged { get; }

    /// <summary>
    /// Admin mail address
    /// </summary>
    public string AdminMailAddress { get; set; }

    /// <summary>
    /// Start mailing
    /// </summary>
    void StartMailing();

    /// <summary>
    /// Send a status message to console
    /// </summary>
    /// <param name="message">Message to send to console</param>
    void Status(string message);
}
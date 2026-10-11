// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Basic interface for mail accounts
/// </summary>
public interface IMailAccount
{
    /// <summary>
    /// Mail address to use for sending 
    /// </summary>
    public string? MailAddressSender { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoWebMailer.Business.Interfaces;

/// <summary>
/// App globals enhancements for BodoWebMailer
/// </summary>
public interface IBodoWebMailerAppGlobals: IAppGlobals
{
    /// <summary>
    /// Use SMTP for mailing. If false O365 MS Graph is used. Default: false meaning I365 is used
    /// </summary>
    bool UseSmtp { get; set; }

    /// <summary>
    /// Current data protection service
    /// </summary>
    IDataProtectionService DataProtectionService { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoFileTransfer.Business.Enums;

/// <summary>
/// Enum for types of email transport to use for sending emails to external receivers
/// </summary>
public enum SendEmailsType
{
    None = 0,
    Smtp = 1,
    Office365 = 2
}
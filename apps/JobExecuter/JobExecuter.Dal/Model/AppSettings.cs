// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace JobExecuter.Dal.Model;

/// <summary>
/// Application settings
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    /// Job salt
    /// </summary>
    public string JobSalt { get; set; }

    /// <summary>
    /// Job password
    /// </summary>
    public string JobPassword { get; set; }
}
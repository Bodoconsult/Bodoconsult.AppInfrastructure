// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using JobExecuter.Base.Interfaces;

namespace JobExecuter.Dal.App;

/// <summary>
/// App globals adjusted for JobExecuter
/// </summary>
public interface IJobExecuterAppGlobals: IAppGlobals
{
    /// <summary>
    /// Current job to handle
    /// </summary>
    IJob CurrentJob { get; set; }

    /// <summary>
    /// Filename of the crypted JSON file
    /// </summary>
    string CjsonFileName { get; set; }

    /// <summary>
    /// Name of the logfile
    /// </summary>
    string LogFileName { get; set; }

    /// <summary>
    /// The current SMTP mail account or null if no SMTP mailing is used
    /// </summary>
    SmtpMailAccount SmtpMailAccount { get; set; }
}
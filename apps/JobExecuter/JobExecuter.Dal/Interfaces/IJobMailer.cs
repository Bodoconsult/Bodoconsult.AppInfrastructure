// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Interfaces;

namespace JobExecuter.Dal.Interfaces;

/// <summary>
/// Interface for mailing service
/// </summary>
public interface IJobMailer
{
    /// <summary>
    /// Send a mail 
    /// </summary>
    /// <param name="mailTo">receiver of the mail</param>
    /// <param name="subject">subject of the mail</param>
    /// <param name="body">mail body</param>
    /// <param name="priority">true=high priority</param>
    /// <returns>true on success</returns>
    bool SendMail(string mailTo, string subject, IStructuredText body, bool priority);
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.Text.Interfaces;

namespace JobExecuter.Base.Interfaces;

/// <summary>
/// Runs a single task
/// </summary>
public interface ITaskExecuter
{
    /// <summary>
    /// Status changed event
    /// </summary>
    StatusMessageDelegate StatusChanged { get; set; }

    /// <summary>
    /// Mail sent event
    /// </summary>
    JobHandlerMailDelegate MailSend { get; set; }

    /// <summary>
    /// Save HTML as delegate
    /// </summary>
    SaveAsHtmlDelegate SaveAsHtml { get; set; }

    /// <summary>
    /// Save HTML as delegate
    /// </summary>
    SaveAsPdfDelegate SaveAsPdf { get; set; }

    /// <summary>
    /// Save text as JSON
    /// </summary>
    SaveAsJsonDelegate<IStructuredText> SaveAsJson { get; set; }

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    bool Run();

    /// <summary>
    /// Kill task when timed out
    /// </summary>
    /// <returns>true on success</returns>
    bool Kill();

    /// <summary>
    /// Contains error messages resulting from errors in single task steps for example in batch processes
    /// </summary>
    IList<Exception> Errors { get; }

    /// <summary>
    /// Prefix used for logging
    /// </summary>
    string LogPrefix { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.Text.Interfaces;
using JobExecuter.Base.Interfaces;

namespace JobExecuter.Base;

/// <inheritdoc />
/// <summary>
/// Base class for <see cref="T:ITaskExecuter" /> implementations
/// </summary>
public abstract class BaseExecuter: ITaskExecuter
{
    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    protected BaseExecuter(IJobTask jobTask)
    {
        Errors= new List<Exception>();
    }

    /// <summary>
    /// Status changed event
    /// </summary>
    public StatusMessageDelegate StatusChanged { get; set; }

    /// <summary>
    /// Mail sent event
    /// </summary>
    public JobHandlerMailDelegate MailSend { get; set; }

    /// <summary>
    /// Save HTML as delegate
    /// </summary>
    public SaveAsHtmlDelegate SaveAsHtml { get; set; }

    /// <summary>
    /// Save HTML as delegate
    /// </summary>
    public SaveAsPdfDelegate SaveAsPdf { get; set; }

    /// <summary>
    /// Save text as JSON
    /// </summary>
    public SaveAsJsonDelegate<IStructuredText> SaveAsJson { get; set; }

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public virtual bool Run()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Kill task when timed out
    /// </summary>
    /// <returns>true on success</returns>
    public virtual bool Kill()
    {
        return true;
    }

    /// <summary>
    /// Contains error messages resulting from errors in single task steps for example in batch processes
    /// </summary>
    public IList<Exception> Errors { get; private set; }

    /// <summary>
    /// Prefix used for logging
    /// </summary>
    public string LogPrefix { get; set; }

    /// <summary>
    /// Send status changed message
    /// </summary>
    /// <param name="message">message to show</param>
    public void SetStatus(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }
        var x = StatusChanged;
        x?.Invoke($"{LogPrefix}: {message}");
    }

    /// <summary>
    /// Send status changed message from an external app
    /// </summary>
    /// <param name="message">message to show</param>
    public void SetStatusApp(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }
        var x = StatusChanged;
        x?.Invoke($"{LogPrefix}:ExtApp: {message}");
    }

    /// <summary>
    /// Send mail sent message
    /// </summary>
    /// <param name="mailTo">Mail receiver</param>
    /// <param name="subject">Subject of the mail</param>
    /// <param name="body">Body of the mail</param>
    public void SendMail(string mailTo, string subject, IStructuredText body)
    {
        if (body==null || body.TextItems.Count==0)
        {
            return;
        }
        var x = MailSend;
        x?.Invoke(mailTo, subject, body);
    }

    /// <summary>
    /// Send status changed message
    /// </summary>
    /// <param name="message">message to show</param>
    /// <param name="data">data parameters to show in message</param>
    public void SetStatus(string message, params object[] data)
    {
        var x = StatusChanged;
        x?.Invoke(string.Format(message, data));
    }
}
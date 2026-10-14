// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using Bodoconsult.Text.Model;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Interfaces;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Base executer class for documentation job tasks
/// </summary>
internal class DocuBaseExecuter : BaseExecuter
{
    protected readonly StructuredText Result = new();
    private readonly IDocuBaseJobTask _jobTask;

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public DocuBaseExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (IDocuBaseJobTask)jobTask;
    }

    /// <summary>
    /// Save result as JSON / HTML or send mail
    /// </summary>
    protected void Save(string title = null)
    {
        if (string.IsNullOrEmpty(_jobTask.Title))
        {
            _jobTask.Title = title;
        }

        if (!string.IsNullOrEmpty(_jobTask.JsonTargetFileName))
        {
            SaveAsJson?.Invoke(_jobTask.JsonTargetFileName, Result);
        }

        // Add meta data only to HTML and 
        AddMetaData();

        if (!string.IsNullOrEmpty(_jobTask.HtmlTargetFileName))
        {
            SaveAsHtml?.Invoke(_jobTask.HtmlTargetFileName, Result, title);
        }


        if (!string.IsNullOrEmpty(_jobTask.PdfTargetFileName))
        {
            SaveAsPdf?.Invoke(_jobTask.PdfTargetFileName, Result, title);
        }


        if (!string.IsNullOrEmpty(_jobTask.MailTo))
        {
            SendMail(_jobTask.MailTo, title, Result);
        }
    }

    /// <summary>
    /// Add meta data like date, user, computername tec. to the text
    /// </summary>
    protected void AddMetaData()
    {
        Result.AddHeader1("Document meta data");
        Result.AddDefinitionListLine("Date: ", $"{DateTime.Now:g}");
        Result.AddDefinitionListLine("User", $"{Environment.UserDomainName}\\{Environment.UserName}");
        Result.AddDefinitionListLine("Host", Environment.MachineName);
        Result.AddDefinitionListLine("IP address:", NetworkHelper.GetLocalIpAddress());
        Result.AddDefinitionListLine("OS:", Environment.OSVersion.VersionString);
        Result.AddDefinitionListLine("64-bit:", Environment.Is64BitOperatingSystem.ToString());
    }
}
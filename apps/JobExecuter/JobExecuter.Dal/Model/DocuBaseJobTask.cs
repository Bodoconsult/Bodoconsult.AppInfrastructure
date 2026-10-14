// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Model;
using JobExecuter.Base;
using JobExecuter.Dal.Interfaces;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Base class for documentation job tasks
/// </summary>
public class DocuBaseJobTask : BaseJobTask, IDocuBaseJobTask
{
    /// <summary>
    /// Title for documentation
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner
    /// </summary>
    public string MailTo { get; set; }

    /// <summary>
    /// Export documentation to HTML
    /// </summary>
    public string HtmlTargetFileName { get; set; }

    /// <summary>
    /// Export documentation to Pdf
    /// </summary>
    public string PdfTargetFileName { get; set; }


    /// <summary>
    /// Export documentation as <see cref="StructuredText"/> object to JSON.
    /// If empty, the file will be placed in app subfolder docu with the name "job_docu.json" 
    /// </summary>
    public string JsonTargetFileName { get; set; }

    ///// <summary>
    ///// Get the factory for the task's executer class
    ///// </summary>
    ///// <returns></returns>
    //public new virtual ITaskExecuterFactory GetFactory()
    //{
    //    throw new System.NotImplementedException();
    //}

}
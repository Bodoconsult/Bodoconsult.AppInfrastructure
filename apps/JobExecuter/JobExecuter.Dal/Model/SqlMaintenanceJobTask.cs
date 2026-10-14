// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Model;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;
using JobExecuter.Dal.Interfaces;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Job to perfrom maintenance on a Microsoft® SqlServer instance
/// </summary>
/// <remarks>Needs sysadmin permissions to run successfully on the instance of SqlServer</remarks>
/// <example>
/// <include file='..\Samples\SqlMaintenanceJobTask.xml' path='*'/>
///  </example>
public sealed class SqlMaintenanceJobTask : SqlBaseJobTask, IDocuBaseJobTask
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public SqlMaintenanceJobTask()
    {
        FragmentationLevel1 = 5;
        FragmentationLevel2 = 30;
    }

    /// <summary>
    /// Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner
    /// </summary>
    public string MailTo { get; set; }

    /// <summary>
    /// Title for documentation
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Export documentation to HTML
    /// </summary>
    public string HtmlTargetFileName { get; set; }


    /// <summary>
    /// Export documentation to PDF
    /// </summary>
    public string PdfTargetFileName { get; set; }


    /// <summary>
    /// Export documentation as <see cref="StructuredText"/> object to JSON.
    /// If empty, the file will be placed in app subfolder docu with the name "job_docu.json" 
    /// </summary>
    public string JsonTargetFileName { get; set; }


    /// <summary>
    /// Fragmentation level 1 (low ->medium) in percent. 50 means 50 percent. Default: 5
    /// </summary>
    public int FragmentationLevel1 { get; set; }


    /// <summary>
    /// Fragmentation level 2 (medium -> high) in percent. 50 means 50 percent. Default: 30
    /// </summary>
    public int FragmentationLevel2 { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new SqlMaintenanceExecuterFactory();
    }

}
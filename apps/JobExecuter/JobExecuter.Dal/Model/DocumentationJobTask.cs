// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using Bodoconsult.Text.Interfaces;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Creates a HTML documentation from diverse JSON files as input.
/// May be used i.e. for combining database compliance reports from different database servers to one compliance report HTML file.
/// </summary>
/// <example>
/// <remarks>JSON files must contain an object implementing the <see cref="IStructuredText"/> interface.
/// Use in conjunction with
///   - <see cref="JobDocuJobTask"/>,
///   - <see cref="ActiveDirectoryJobTask" />
///   - <see cref="DnsJobTask"/>
///   - <see cref="DhcpJobTask"/>
///   - <see cref="FolderComplianceJobTask"/>
///   - <see cref="SqlComplianceAllDatabasesJobTask"/>,
///   - <see cref="SqlComplianceSingleDatabaseJobTask"/>,
///   - <see cref="SqlComplianceAllDatabasesJobTask"/>,
///   - <see cref="SqlComplianceSingleDatabaseJobTask"/>,
///   - <see cref="SqlMaintenanceJobTask"/>
///   - <see cref="TaskSchedulerDocuJobTask"/>.
/// Let all this JobTasks create JSON files as output and combine them with <see cref="DocumentationJobTask"/> to HTML reports as required.
/// </remarks>
/// <include file='..\Samples\DocumentationJobTask.xml' path='*'/>
///  </example>
public sealed class DocumentationJobTask : BaseJobTask
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DocumentationJobTask()
    {
        JsonFiles = new List<string>();
        HtmlTargetFiles = new List<string>();
        Title = "Documentation";
    }

    /// <summary>
    /// message to deliver for testing purposes
    /// </summary>
    public IList<string> JsonFiles { get; private set; }

    /// <summary>
    /// Filenames for HTML documentation as result of the job
    /// </summary>
    public IList<string> HtmlTargetFiles { get; set; }

    /// <summary>
    /// Title for documentation
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Append the JobExecuter help file to documentation
    /// </summary>
    public bool AppendHelpFile { get; set; }

    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new DocumentationExecuterFactory();
    }
}
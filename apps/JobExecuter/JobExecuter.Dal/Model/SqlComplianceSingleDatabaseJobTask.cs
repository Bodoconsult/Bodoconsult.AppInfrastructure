// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to collect compliance relevante information for a single SQL database on a Microsoft® SqlServer 
/// </summary>
/// <example>
/// <include file='..\Samples\SqlComplianceSingleDatabaseJobTask.xml' path='*'/>
///  </example>
public sealed class SqlComplianceSingleDatabaseJobTask : DocuBaseJobTask
{
    /// <summary>
    /// server name or server and instance name of a database server
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// Name of the database to backup
    /// </summary>
    public string DatabaseName { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new SqlComplianceSingleDatabaseExecuterFactory();
    }

}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to collect compliance relevante information for all SQL databases on a Microsoft® SqlServer 
/// </summary>
/// <remarks>Databases Master, Msdb, Model and TempDb are excluded by this task. Requires the job running user to be member of the sysadmin or securityadmin server roles.</remarks>
/// <example>
/// <include file='..\Samples\SqlComplianceAllDatabasesJobTask.xml' path='*'/>
///  </example>
public sealed class SqlComplianceAllDatabasesJobTask : DocuBaseJobTask
{
    /// <summary>
    /// server name or server and instance name of a database server
    /// </summary>
    public string Server { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new SqlComplianceAllDatabasesExecuterFactory();
    }


}
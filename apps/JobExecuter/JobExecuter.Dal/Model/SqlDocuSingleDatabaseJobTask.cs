// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to take a documentation for a single SQL database on a Microsoft® SqlServer.
/// </summary>
/// <remarks><para>A database may contain stored procs under the schema dbo and names beginning with spDbDocu_ foradding database specific content to the docmentation.</para>
/// <para>For parts of the documentation the user running the job needs certain permissions.
/// To give least possible permissions, this user should be part of a user defined server role on the SqlServer with the permissions VIEW ANY DEFINITION and GRANT VIEW SERVER STATE.
/// You should avoid making the user a sysadmin for security reasons. User must have access to each database with least possible permissions!</para></remarks>
/// <code>
/// CREATE SERVER ROLE NameOfNewServerRole
///
///GO
///
///GRANT VIEW ANY DEFINITION TO NameOfNewServerRole
///
///GO
///
///GRANT VIEW SERVER STATE TO NameOfNewServerRole
///
///GO
///
///EXEC sp_addsrvrolemember 'domain\SG-SqlKeyUsers', 'NameOfNewServerRole'
///
/// </code>
/// <example>
/// <include file='..\Samples\SqlDocuSingleDatabaseJobTask.xml' path='*'/>
///  </example>
public sealed class SqlDocuSingleDatabaseJobTask : DocuBaseJobTask
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
        return new SqlDocuSingleDatabaseExecuterFactory();
    }

}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to run a SQL batch from a file or from a string against a Microsoft® SqlServer 
/// </summary>
/// <remarks>SQL statements must be separated by GO. The SQL statements must address database correctly, i.e EXEC testdb.dbo.TestProc.
/// All SQL statements are running on the same connection. This makes it possible to use temporary tables in SQL statements.</remarks>
/// <example>
/// <include file='..\Samples\SqlBatchJobTask.xml' path='*'/>
///  </example>
public sealed class SqlBatchJobTask : SqlBaseJobTask
{


    /// <summary>
    /// Name of the database to run the batch file in
    /// </summary>
    public string DatabaseName { get; set; }

    /// <summary>
    /// Path to the SQL batch file to run
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Direct batch SQL to run
    /// </summary>
    public string BatchContent { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new SqlBatchExecuterFactory();
    }

}
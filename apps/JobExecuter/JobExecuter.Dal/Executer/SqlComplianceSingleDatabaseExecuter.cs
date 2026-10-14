// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for sending a mail with suer access information of a Sql database on Ms SqlServer
/// </summary>
internal class SqlComplianceSingleDatabaseExecuter : DocuBaseExecuter
{

    private readonly SqlComplianceSingleDatabaseJobTask _jobTask;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        var result = true;

        var dbName = $"{_jobTask.Server}\\{_jobTask.DatabaseName}";

        SetStatus("Start mailing user access for database'{0}'...", dbName);

        try
        {

            Result.AddHeader1($"Database '{dbName}' on server '{_jobTask.Server}'");

            var docu = new SqlDocuDatabaseHelper(Result, _jobTask.DatabaseName, _jobTask.Server, Errors, SetStatus);

            Result.AddParagraph($"ADO.NET connection string: {docu.ConnectionString}");

            docu.AddServerInfo();

            docu.AddGeneralInfo();
            docu.AddSecurityRoles();
            docu.AddSecurityLogins();
            docu.AddSecurityLoginsRoles();

            Save("Compliance reporting for database servers");

            SetStatus("Mailing user access for database '{0}' done!", dbName);

        }
        catch (Exception ex)
        {
            var msg = $"SqlMailUserAccessSingleDatabaseExecuter:Run:{dbName}";
            result = false;
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;
    }



    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public SqlComplianceSingleDatabaseExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (SqlComplianceSingleDatabaseJobTask)jobTask;
    }
}
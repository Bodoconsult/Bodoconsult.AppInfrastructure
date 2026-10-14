// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for taking a documentation of a Sql database on Ms SqlServer
/// </summary>
internal class SqlDocuSingleDatabaseExecuter : DocuBaseExecuter
{
    private readonly SqlDocuSingleDatabaseJobTask _jobTask;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        var result = true;

        try
        {
            var dbName = $"{_jobTask.Server}\\{_jobTask.DatabaseName}";

            SetStatus("Start documentation of database'{0}'...", dbName);

            try
            {
                var docu = new SqlDocuDatabaseHelper(Result, _jobTask.DatabaseName, _jobTask.Server, Errors, SetStatus);
                Result.AddHeader1("General server information '{0}'", null, _jobTask.Server);
                docu.AddServerInfo();

                Result.AddHeader1("Documentation of database '{0}'", null, dbName);                    
                docu.AddGeneralDatabaseInfos();
                docu.AddSecurityRoles();
                docu.AddSecurityLogins();
                docu.AddSecurityLoginsRoles();
                docu.AddDatabaseInfo();
                docu.AddDocumentation();

                Save($"Documentation of database '{dbName}'");

                SetStatus("Documentation of database '{0}' done!", dbName);
            }
            catch (Exception ex)
            {
                result = false;
                var msg = $"SqlDocuSingleDatabaseExecuter:Run:{_jobTask.Server}\\{_jobTask.DatabaseName}";
                Errors.Add(new Exception(msg, ex));
                SetStatus(msg);
            }
        }
        catch (Exception ex)
        {
            result = false;
            var msg = $"SqlDocuSingleDatabaseExecuter:Run:{_jobTask.Server}\\{_jobTask.DatabaseName}";
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public SqlDocuSingleDatabaseExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (SqlDocuSingleDatabaseJobTask)jobTask;
    }
}
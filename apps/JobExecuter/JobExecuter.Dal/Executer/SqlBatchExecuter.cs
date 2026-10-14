// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Run a SQL batch from a file or from a string
/// </summary>
internal class SqlBatchExecuter : BaseExecuter
{
    //private const string ConnectionString = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI;TrustServerCertificate=True";

    private readonly SqlBatchJobTask _jobTask;


    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        var result = true;
        try
        {

            SetStatus("Start batch run for file {1} on server '{0}'...", _jobTask.Server, _jobTask.FileName);

            var sConn = string.Format(ExecuterConstants.SqlServerConnectionString, _jobTask.Server, _jobTask.DatabaseName);

            var db = new SqlHelper(sConn, SetStatus);


            var content = (string.IsNullOrEmpty(_jobTask.BatchContent)? File.ReadAllText(_jobTask.FileName): _jobTask.BatchContent)
                .Replace("\r\ngo\r\n", "\r\nGO\r\n")
                .Replace("\r\nGo\r\n", "\r\nGO\r\n")
                .Replace("\r\ngO\r\n", "\r\nGO\r\n");

            var sqlArray = content.Split(["\r\nGO\r\n"], StringSplitOptions.RemoveEmptyEntries);

            foreach (var sql in sqlArray)
            {
                try
                {
                    db.Exec(sql);
                }
                catch (Exception ex)
                {
                    result = false;
                    var msg = $"SqlBatchExecuter:run:'{_jobTask.Server}\\{_jobTask.DatabaseName}: {sql}'!";
                    Errors.Add(new Exception(msg, ex));
                    SetStatus(msg);
                }
            }

            SetStatus("Batch run for file {1} on server '{0}' done with {2} error(s)!", _jobTask.Server, _jobTask.FileName, Errors.Count);
                
        }
        catch (Exception ex)
        {
            result = false;
            var msg = $"SqlBatchExecuter:run:'{_jobTask.Server}\\{_jobTask.DatabaseName}'!";
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public SqlBatchExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (SqlBatchJobTask)jobTask;
    }
}
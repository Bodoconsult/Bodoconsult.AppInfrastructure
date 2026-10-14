// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for running a maintenance job on Ms SqlServer
/// </summary>
/// <remarks>Should run only on daily, weekly or monthly base dedending on the system work load. The heavier the work load the more often it should run.</remarks>
internal class SqlMaintenanceExecuter : DocuBaseExecuter
{
    private readonly SqlMaintenanceJobTask _jobTask;
    private readonly SqlHelper _db;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        var result = true;

        if (string.IsNullOrEmpty(_jobTask.Server))
        {
            return false;
        }

        SetStatus($"Starting SQLServer maintenance '{_jobTask.Server}'...");

        try
        {
            Result.AddHeader1($"SQLServer maintenance '{_jobTask.Server}'");

            // ***********
            SetStatus("Check admin tools...");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlMaintenanceAdminToolsExist.sql");
            var erg = _db.ExecWithResult(sql);

            bool x;
            if (erg == "0")
            {
                SetStatus("Install admin tools...");
                sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlMaintenanceAdminTools.sql")
                    .Replace("\r\ngo\r\n", "\r\nGO\r\n")
                    .Replace("\r\nGo\r\n", "\r\nGO\r\n")
                    .Replace("\r\ngO\r\n", "\r\nGO\r\n");

                x = RunSql(sql);
                if (!x)
                {
                    result = false;
                }

                Status("Admin tools installed!");
            }

            // ***********
            SetStatus("Optimize indexes...");
            Result.AddHeader2("Optimizing indexes");

            sql = string.Format(FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlMaintenanceIndexOptimize.sql")
                , _jobTask.FragmentationLevel1
                , _jobTask.FragmentationLevel2);

            x = RunSql(sql);
            if (result && !x)
            {
                result = false;
            }

            // ***********
            SetStatus("Update statistics...");
            Result.AddHeader2("Updating statistics");

            sql = string.Format(FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlMaintenanceUpdateStatistics.sql")
                , _jobTask.FragmentationLevel1
                , _jobTask.FragmentationLevel2);

            x = RunSql(sql);
            if (result && !x)
            {
                result = false;
            }

            // ***********
            Status("Check database integrity...");
            Result.AddHeader2("Database integrity checks");
            sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlMaintenanceIntegrityCheck.sql");
            x = RunSql(sql);
            if (result && !x)
            {
                result = false;
            }

            SetStatus($"SQLServer maintenance '{_jobTask.Server}' done!");

            _db.Close();

            Save("SqlServer maintenance report");

        }
        catch (Exception ex)
        {
            var msg = $"Error:SqlMaintenanceExecuter:Run:'{_jobTask.Server}':";
            result = false;
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg + ex.Message);
        }

        return result;
    }

    private bool RunSql(string sql)
    {
        var sqlArray = sql.Split(["\r\nGO\r\n"], StringSplitOptions.RemoveEmptyEntries);

        foreach (var sql1 in sqlArray)
        {
            try
            {
                _db.Exec(sql1);
            }
            catch (Exception ex)
            {
                Result.AddParagraph(sql1);
                Result.AddError(ex.Message);

                //result = false;
                //var msg = $"SqlMaintenanceExecuter:run:'{_jobTask.Server}: {sql}'!";
                //Errors.Add(new Exception(msg, ex));
                //SetStatus(msg);
            }
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    private void Status(string message)
    {
        Result.AddParagraph(message);

        SetStatus(message);
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public SqlMaintenanceExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (SqlMaintenanceJobTask)jobTask;

        var connectionString = string.Format(ExecuterConstants.SqlServerConnectionString, _jobTask.Server, "Master");

        _db = new SqlHelper(connectionString, Status);
    }
}
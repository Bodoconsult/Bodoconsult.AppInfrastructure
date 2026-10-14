// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using Bodoconsult.Database.SqlClient;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for taking a full backup of a SQL database on Ms SqlServer. Uses the following SQL statement:
/// BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N'{0} full backup'
/// </summary>
internal class SqlBackupSingleDatabaseExecuter : BaseExecuter
{
        
    //private const string ConnectionString = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI;TrustServerCertificate=True;";

    private const string SqlBackup = "BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N'{0} full backup'";

    private readonly SqlBackupSingleDatabaseJobTask _jobTask;


    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        var localFile = NamingHelper.ClearPath(Path.Combine(_jobTask.LocalTarget, _jobTask.FileName), _jobTask.Server, _jobTask.DatabaseName);
        var localPath = NamingHelper.ClearPath(_jobTask.LocalTarget, _jobTask.Server, _jobTask.DatabaseName);

        var sConn = string.Format(ExecuterConstants.SqlServerConnectionString, _jobTask.Server, _jobTask.DatabaseName);
        var sql = string.Format(SqlBackup, _jobTask.DatabaseName, localFile);

        SetStatus("Start backup of database'{1}' on server '{0}'...",_jobTask.Server, _jobTask.DatabaseName);

        try
        {

            if (!Directory.Exists(localPath)) Directory.CreateDirectory(localPath);

            var db = SqlClientConnManager.GetConnManager(sConn);
            db.SetCommandTimeout(_jobTask.Timeout);
            db.SendStatus += SetStatus;
            db.Exec(sql, false);

            SetStatus("Successful local backup: {0}", sql);

            if (string.IsNullOrEmpty(_jobTask.NetworkTarget) || !File.Exists(localFile)) return true;

            var networkPath = NamingHelper.ClearPath(_jobTask.NetworkTarget, _jobTask.Server, _jobTask.DatabaseName);
            if (!Directory.Exists(networkPath)) Directory.CreateDirectory(networkPath);

            var networkFile = NamingHelper.ClearPath( Path.Combine(_jobTask.NetworkTarget, _jobTask.FileName), _jobTask.Server, _jobTask.DatabaseName);
            if (File.Exists(networkFile)) File.Delete(networkFile);

            if (_jobTask.RemoveLocalBackup)
            {
                File.Move(localFile, networkFile);
                SetStatus("Successful moved local backup to network path: {0}", networkFile);
            }
            else
            {
                File.Copy(localFile, networkFile);
                SetStatus("Successful copied local backup to network path: {0}", networkFile);
            }

            SetStatus("Backup of database'{1}' on server '{0}' done!", _jobTask.Server, _jobTask.DatabaseName);
            return true;
        }
        catch (Exception ex)
        {
            var msg = $"Conn: {sConn}\r\nSQL: {sql}";
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
            return false;
        }
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public SqlBackupSingleDatabaseExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (SqlBackupSingleDatabaseJobTask)jobTask;
    }
}
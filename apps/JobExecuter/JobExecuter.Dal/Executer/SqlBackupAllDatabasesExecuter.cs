// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Data;
using System.IO;
using Bodoconsult.Database.SqlClient;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for taking a full backup of all SQL database on Ms SqlServer with exception of TempDB. Uses the following SQL statement:
/// BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N'{0} full backup'
/// </summary>
internal class SqlBackupAllDatabasesExecuter : BaseExecuter
{

    //private const string ConnectionString = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI;TrustServerCertificate=True";

    private const string SqlBackup =
        "BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N'{0} full backup'";

    private readonly SqlBackupAllDatabasesJobTask _jobTask;


    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        SetStatus("Start backup off all databases on server '{0}'", _jobTask.Server);

        try
        {
            var sConn = string.Format(ExecuterConstants.SqlServerConnectionString, _jobTask.Server, "master");


            var db = SqlClientConnManager.GetConnManager(sConn);
            db.SetCommandTimeout(_jobTask.Timeout);
            db.SendStatus += SetStatus;

            var result = true;

            var dt = db.GetDataTable("select name from sys.databases where database_id<>2");
            foreach (DataRow r in dt.Rows)
            {
                var dbName = r["name"].ToString();

                SetStatus("Start backup of database '{0}'...", dbName);

                var localPath = NamingHelper.ClearPath(_jobTask.LocalTarget, _jobTask.Server, dbName);


                var localFile = NamingHelper.ClearPath(Path.Combine(_jobTask.LocalTarget, _jobTask.FileName),
                    _jobTask.Server, dbName);

                var sql = string.Format(SqlBackup, dbName, localFile);

                try
                {

                    if (!Directory.Exists(localPath)) Directory.CreateDirectory(localPath);

                    db.Exec(sql, false);

                    SetStatus("Successful local backup: {0}", sql);

                    if (!(string.IsNullOrEmpty(_jobTask.NetworkTarget) || !File.Exists(localFile)))
                    {
                        var networkPath = NamingHelper.ClearPath(_jobTask.NetworkTarget, _jobTask.Server, dbName);
                        if (!Directory.Exists(networkPath)) Directory.CreateDirectory(networkPath);

                        var networkFile = NamingHelper.ClearPath(Path.Combine(_jobTask.NetworkTarget, _jobTask.FileName),
                            _jobTask.Server, dbName);
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
                    }

                        

                }
                catch (Exception ex)
                {
                    result = false;
                    var msg = string.Format("Error at backup of database '{3}'\r\nConn: {0}\r\nSQL: {1}\r\nLocalFile:{2}", sConn, sql, localFile, dbName);
                    Errors.Add(new Exception(msg, ex));
                    SetStatus(msg);
                }
            }

            SetStatus("Backup of all databases on server '{0}' done with {1} error(s)!", _jobTask.Server, Errors.Count);
            return result;
        }
        catch (Exception ex)
        {

            throw new Exception($"Error at backup for all databases on server {_jobTask.Server}", ex);
        }
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public SqlBackupAllDatabasesExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (SqlBackupAllDatabasesJobTask)jobTask;
    }
}
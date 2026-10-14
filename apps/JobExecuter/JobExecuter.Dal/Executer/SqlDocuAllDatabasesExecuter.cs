// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Data;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using Bodoconsult.Text.Enums;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <summary>
/// Executer for taking a documentation of a all Sql databases on Ms SqlServer with exception of databases Master, Msdb, Model and TempDb
/// </summary>
internal class SqlDocuAllDatabasesExecuter : DocuBaseExecuter
{
    private const string ConnectionString = "Data Source={0};Initial Catalog=Master;Integrated Security=SSPI;TrustServerCertificate=True;";
    private readonly SqlDocuAllDatabasesJobTask _jobTask;
    private IConnManager _db;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        var result = true;
        try
        {
            var sConn = string.Format(ConnectionString, _jobTask.Server);

            _db = SqlClientConnManager.GetConnManager(sConn);
            _db.SendStatus += SetStatus;

            Result.AddHeader1($"SqlServer server documentation {_jobTask.Server}");

            AddServerInfo();
            AddControlServerPermissions();
            AddUserDatabaseAccess();
            AddOrphanedLogins();
            AddOtherDatabaseInfo();

            var dt = _db.GetDataTable("select name from sys.databases where database_id>4 order by name");
            foreach (DataRow r in dt.Rows)
            {
                var dbName = $"{_jobTask.Server}\\{r["name"]}";

                try
                {
                    SetStatus("Start documentation of database '{0}'...", dbName);

                    Result.AddHeader1("Documentation of database '{0}'", null, dbName);

                    var docu = new SqlDocuDatabaseHelper(Result, r["name"].ToString(), _jobTask.Server, Errors,
                        SetStatus)
                    {
                        Header1Type = TextItemType.H1,
                        Header2Type = TextItemType.H2
                    };
                    docu.AddGeneralDatabaseInfos();
                    docu.AddSecurityRoles();
                    docu.AddSecurityLogins();
                    docu.AddSecurityLoginsRoles();
                    docu.AddDatabaseInfo();
                    docu.AddDocumentation();

                    SetStatus("Documentation of database '{0}' done!", dbName);
                }
                catch (Exception ex)
                {
                    result = false;
                    var msg = $"SqlDocuAllDatabasesExecuter:Run:{dbName}";
                    Errors.Add(new Exception(msg, ex));
                }
            }

            Save($"Documentation of all databases on SqlServer '{_jobTask.Server}'");

        }
        catch (Exception ex)
        {
            result = false;
            var msg = $"Server documentation for '{_jobTask.Server}' failed!";
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;
    }

    /// <summary>
    /// Add some general important infos of databases like read-only mode, single-user mode
    /// </summary>
    private void AddOtherDatabaseInfo()
    {
        try
        {
            // Databases not in online mode
            AddTable("Databases not in online mode", "JobExecuter.Dal.Resources.DatabasesNotOnline.sql");

            // Databases not in multiuser mode
            AddTable("Databases not in multiuser mode", "JobExecuter.Dal.Resources.DatabasesNotMultiuser.sql");

            //// Databases not in multiuser mode
            //AddTable("Databases not in multiuser mode", "JobExecuter.Dal.Resources.DatabasesNotMultiuser.sql");

            // Databases not in multiuser mode
            AddTable("Databases in read-only mode", "JobExecuter.Dal.Resources.DatabasesReadOnly.sql");

            // Databases with compatibility level below server level
            AddTable("Databases with compatibility level below server level", "JobExecuter.Dal.Resources.DatabasesCompatibility.sql");

        }
        catch
        {
            // ignored
        }
    }

    private void AddTable(string headLine, string resourceId)
    {
        Result.AddHeader2(headLine);

        var sql = FileHelper.GetTextResource(resourceId);

        var dt = _db.GetDataTable(sql);

        if (dt.Rows.Count > 0)
        {
            Result.AddTable(string.Empty, dt);
        }
        else
        {
            Result.AddParagraph("- No items found -");
        }
    }


    /// <summary>
    /// Add info on users and groups with server level permissions
    /// </summary>
    private void AddServerInfo()
    {
        try
        {
            AddTable("General server info", "JobExecuter.Dal.Resources.SqlServer1.sql");
        }
        catch
        {
            // ignored
        }
    }


    /// <summary>
    /// Add info on users and groups with server level permissions
    /// </summary>
    private void AddControlServerPermissions()
    {
        try
        {
            AddTable("Control server permissions", "JobExecuter.Dal.Resources.ControlServerPermission.sql");
        }
        catch
        {
            // ignored
        }
    }

    private void AddUserDatabaseAccess()
    {
        try
        {
            AddTable("User database access", "JobExecuter.Dal.Resources.UserDatabaseAccess.sql");
        }
        catch
        {
            // ignored
        }
    }

    private void AddOrphanedLogins()
    {
        try
        {
            Result.AddHeader2("Orphaned logins and their objects");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.OrphanedLogins.sql");

            var dt = _db.GetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                Result.AddTable(string.Empty, dt);
                Result.AddParagraph("The orphaned logins own the following objects:");

                sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.OrphanedLoginsObjects.sql");

                dt = _db.GetDataTable(sql);

                Result.AddTable(string.Empty, dt);
            }
            else
            {
                Result.AddParagraph("No items found!");
            }
        }
        catch (Exception ex)
        {
            // ignored
            Result.AddParagraph($"Missing permissions for that report! {ex.Message}");
        }
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public SqlDocuAllDatabasesExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (SqlDocuAllDatabasesJobTask)jobTask;
    }
}
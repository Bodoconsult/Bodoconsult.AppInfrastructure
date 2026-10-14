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

/// <inheritdoc />
/// <summary>
/// Executer for sending a mail with suer access information of a all Sql databases on Ms SqlServer with exception of databases Master, Msdb, Model and TempDb
/// </summary>
internal class SqlComplianceAllDatabasesExecuter : DocuBaseExecuter
{
    private const string ConnectionString = "Data Source={0};Initial Catalog=Master;Integrated Security=SSPI;TrustServerCertificate=True;";

    private readonly SqlComplianceAllDatabasesJobTask _jobTask;


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

            Result.AddHeader1($"Server { _jobTask.Server}");

            Result.AddHeader2("General information");

            AddServerInfo();

            AddGeneralInfo();

            Result.AddHeader2("Server user access");


            AddControlServerPermissions();
            AddUserDatabaseAccess();
            AddOrphanedLogins();

            var dt = _db.GetDataTable("select name from sys.databases where database_id>4 order by name");
            foreach (DataRow r in dt.Rows)
            {
                var dbName = $"{_jobTask.Server}\\{r["name"]}";

                try
                {

                    SetStatus("Mailing user access for  database '{0}'...", dbName);

                    Result.AddHeader2("User access for  database '{0}'", null, dbName);

                    var docu = new SqlDocuDatabaseHelper(Result, r["name"].ToString(), _jobTask.Server, Errors,
                        SetStatus)
                    {
                        Header1Type = TextItemType.H2,
                        Header2Type = TextItemType.H3
                    };

                    Result.AddParagraph($"ADO.NET connection string: {docu.ConnectionString}");

                    docu.AddSecurityRoles();
                    docu.AddSecurityLogins();
                    docu.AddSecurityLoginsRoles();

                    SetStatus("Mailing user access for database '{0}' done!", dbName);

                }
                catch (Exception ex)
                {
                    result = false;
                    var msg = $"SqlMailUserAccessAllDatabasesExecuter:Run:{dbName}";
                    Errors.Add(new Exception(msg, ex));
                    SetStatus(msg);
                }
            }

            Save("Compliance reporting database servers");

        }
        catch (Exception ex)
        {
            result = false;
            const string msg = "SqlMailUserAccessAllDatabasesExecuter:Run";
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;


    }

    /// <summary>
    /// Add info on users and groups with server level permissions
    /// </summary>
    private void AddServerInfo()
    {

        try
        {
            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlServer1.sql");

            var dt = _db.GetDataTable(sql);

            Result.AddParagraph($"Timestamp: {DateTime.Now:G}");
            Result.AddParagraph($"User: {Environment.UserDomainName}\\{Environment.UserName}");
            Result.AddParagraph("Data source: ");
            Result.AddParagraph($"Number of rows returned: --{dt.Rows.Count}--");
            Result.AddParagraph(" ");

            Result.AddTable(string.Empty, dt);

        }
        catch
        {
            // ignored
        }
    }

    private void AddGeneralInfo()
    {
        var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlInstance.sql");

        var dt = _db.GetDataTable(sql);

        Result.AddTable(string.Empty, dt);
    }

    private void AddControlServerPermissions()
    {

        try
        {
            Result.AddHeader3("Control server permissions");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.ControlServerPermission.sql");

            var dt = _db.GetDataTable(sql);

            Result.AddTable(string.Empty, dt);
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

            Result.AddHeader3("User database access");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.UserDatabaseAccess.sql");

            var dt = _db.GetDataTable(sql);

            Result.AddTable(string.Empty, dt);
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
            Result.AddHeader3("Orphaned logins and their objects");

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
                Result.AddParagraph("No orphaned logins found!");
            }
        }
        catch //(Exception ex)
        {
            Result.AddParagraph("Error: please check if the job running user is member of sysadmin or securityadmin server role!");
        }
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public SqlComplianceAllDatabasesExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (SqlComplianceAllDatabasesJobTask)jobTask;
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using Bodoconsult.Text.Enums;
using Bodoconsult.Text.Model;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Get docu text for a single SQLServer database
/// </summary>
public class SqlDocuDatabaseHelper
{
    private const string ConConnectionString = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI;TrustServerCertificate=True;";

    private readonly StructuredText _structuredText;

    private readonly IConnManager _db;

    private readonly IList<Exception> _errors;

    private readonly string _databaseName;

    private readonly string _server;

    /// <summary>
    /// Get the current connection string used
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Type of header level 1 text. Default is <see cref="TextItemType.H1"/>
    /// </summary>
    public TextItemType Header1Type { get; set; }

    /// <summary>
    /// Type of header level 2 text. Default is <see cref="TextItemType.H2"/>
    /// </summary>
    public TextItemType Header2Type { get; set; }


    /// <summary>
    /// Type of header level 3 text. Default is <see cref="TextItemType.H2"/>
    /// </summary>
    public TextItemType Header3Type { get; set; }


    /// <summary>
    /// default ctor
    /// </summary>
    /// <param name="structuredText">text to documentation in</param>
    /// <param name="server"></param>
    /// <param name="errors">list of errors</param>
    /// <param name="status">delegate for status messages</param>
    /// <param name="databaseName">name of database (and server)</param>
    public SqlDocuDatabaseHelper(StructuredText structuredText, string databaseName, string server, IList<Exception> errors, SqlStatus status)
    {
        _structuredText = structuredText;

        ConnectionString = string.Format(ConConnectionString, server, databaseName);
            
        Console.WriteLine(ConnectionString);

        _db = new SqlClientConnManager(ConnectionString);
        _db.SendStatus += status;
        _errors = errors;
        _databaseName = databaseName;
        _server = server;

        Header1Type = TextItemType.H1;
        Header2Type = TextItemType.H2;
    }

    /// <summary>
    /// Add general info on the server
    /// </summary>
    public void AddGeneralInfo()
    {
        var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlInstance.sql");

        try
        {
            var dt = _db.GetDataTable(sql);

            _structuredText.AddParagraph($"Timestamp: {DateTime.Now:G}");
            _structuredText.AddParagraph($"User: {Environment.UserDomainName}\\{Environment.UserName}");
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);
            _structuredText.AddParagraph($"Number of rows returned: --{dt.Rows.Count}--");
            _structuredText.AddParagraph(" ");

            _structuredText.AddTable(string.Empty, dt);
        }
        catch (Exception e)
        {
            Console.WriteLine($"{ConnectionString}{e}");
            throw;
        }

    }

    /// <summary>
    /// Add general database infos to the documentation
    /// </summary>
    public void AddGeneralDatabaseInfos()
    {
        try
        {

            //            SELECT  
            //SERVERPROPERTY('productversion') as 'Product Version', 
            //SERVERPROPERTY('productlevel') as 'Product Level',  
            //SERVERPROPERTY('edition') as 'Product Edition',
            //SERVERPROPERTY('buildclrversion') as 'CLR Version',
            //SERVERPROPERTY('collation') as 'Default Collation',
            //SERVERPROPERTY('instancename') as 'Instance',
            //SERVERPROPERTY('lcid') as 'LCID',
            //SERVERPROPERTY('servername') as 'Server Name'

                

            _structuredText.AddParagraph($"Timestamp: {DateTime.Now:G}");
            _structuredText.AddParagraph($"User: {Environment.UserDomainName}\\{Environment.UserName}");
            _structuredText.AddParagraph(" ");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlVersion.sql");


            var version = _db.ExecWithResult(sql);

            _structuredText.AddDefinitionListLine("Version:", version);
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);

            sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlServerLevel.sql");
            var serverLevel = _db.ExecWithResult(sql);
            _structuredText.AddDefinitionListLine("Server compatibility level:", serverLevel);
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);

            AddHeader2("General database information");


            sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlDatabaseOverview.sql");

            //sql =
            //    "select name, COMPATIBILITY_LEVEL, collation_name, state_desc, user_access_desc, is_auto_shrink_on, recovery_model_desc" +
            //    $" from sys.databases WHERE name='{_databaseName}'";



            var dt = _db.GetDataTable(sql);

            foreach (DataRow r in dt.Rows)
            {
                _structuredText.AddDefinitionListLine("Name:", r["name"].ToString());
                _structuredText.AddDefinitionListLine("Compatibility level:", r["COMPATIBILITY_LEVEL"].ToString());
                _structuredText.AddDefinitionListLine("Collation:", r["collation_name"].ToString());
                _structuredText.AddDefinitionListLine("State:", r["state_desc"].ToString());
                _structuredText.AddDefinitionListLine("User access:", r["user_access_desc"].ToString());
                _structuredText.AddDefinitionListLine("Auto shrink:", r["is_auto_shrink_on"].ToString());
                _structuredText.AddDefinitionListLine("Recovery model:", r["recovery_model_desc"].ToString());
            }

            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);

            // Database files
            AddHeader2("Database files");

            sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.Files.sql");

            dt = _db.GetDataTable(string.Format(sql, _databaseName));

            _structuredText.AddTable(string.Empty, dt);
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);

            // Database files
            AddHeader2("Database files with state other than online");

            sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.FilesNotOnline.sql");

            dt = _db.GetDataTable(string.Format(sql, _databaseName));

            _structuredText.AddTable(string.Empty, dt);
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);


        }
        catch (Exception ex)
        {
            var msg = $"SqlDocuDatabaseHelper:AddGeneralDatabaseInfos '{_server}\\{_databaseName}'";
            _errors.Add(new Exception(msg, ex));
        }

    }



    /// <summary>
    /// Add database roles to documentation
    /// </summary>
    public void AddSecurityRoles()
    {
        try
        {
            AddHeader2("Roles in the database");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.Roles.sql");

            var dt = _db.GetDataTable(sql);

            _structuredText.AddParagraph($"Timestamp: {DateTime.Now:G}");
            _structuredText.AddParagraph($"User: {Environment.UserDomainName}\\{Environment.UserName}");
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);
            _structuredText.AddParagraph($"Number of rows returned: --{dt.Rows.Count}--");
            _structuredText.AddParagraph(" ");

            _structuredText.AddTable(string.Empty, dt);


            //foreach (DataRow r in dt.Rows)
            //{
            //    _structuredText.AddDefinitionListLine(r["name"].ToString(), r["type_desc"].ToString());
            //}
        }
        catch (Exception ex)
        {
            var msg = $"SqlDocuDatabaseHelper:AddSecurityRoles '{_server}\\{_databaseName}'";
            _errors.Add(new Exception(msg, ex));
        }
    }


    /// <summary>
    /// Add database logins to documentation
    /// </summary>
    public void AddSecurityLogins()
    {
        try
        {
            AddHeader2("Logins in the database");

            // 
            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.Logins.sql");

            var dt = _db.GetDataTable(sql);

            _structuredText.AddParagraph($"Timestamp: {DateTime.Now:G}");
            _structuredText.AddParagraph($"User: {Environment.UserDomainName}\\{Environment.UserName}");
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);
            _structuredText.AddParagraph($"Number of rows returned: --{dt.Rows.Count}--");
            _structuredText.AddParagraph(" ");

            dt.Columns.Add("UserName1");

            foreach (DataRow r in dt.Rows)
            {
                if (r["login type"].ToString() == "WINDOWS_USER")
                {
                    try
                    {
                        r["UserName1"] = ActiveDirectoryHelper.GetAdUser(r["login type"].ToString());
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }


            _structuredText.AddTable(string.Empty, dt);

            //foreach (DataRow r in dt.Rows)
            //{
            //    _structuredText.AddDefinitionListLine(r["name"].ToString(), r["type_desc"].ToString());
            //}
        }
        catch (Exception ex)
        {
            var msg = $"SqlDocuDatabaseHelper:AddSecurityLogins '{_server}\\{_databaseName}'";
            _errors.Add(new Exception(msg, ex));
        }

    }


    /// <summary>
    /// Add roles and related logins to documentation
    /// </summary>
    public void AddSecurityLoginsRoles()
    {
        try
        {
            AddHeader2("Direct role members");

            _structuredText.AddParagraph("Roles not listed here do not have any members!");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.RoleMembers.sql");

            var dt = _db.GetDataTable(sql);

            _structuredText.AddParagraph($"Timestamp: {DateTime.Now:G}");
            _structuredText.AddParagraph($"User: {Environment.UserDomainName}\\{Environment.UserName}");
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(sql);
            _structuredText.AddParagraph($"Number of rows returned: --{dt.Rows.Count}--");
            _structuredText.AddParagraph(" ");

            _structuredText.AddTable(string.Empty, dt);


            //foreach (DataRow r in dt.Rows)
            //{
            //    _structuredText.AddDefinitionListLine(r["DatabaseRoleName"].ToString(), 
            //        string.Format("{0} ({1})", r["DatabaseUserName"], r["type_desc"]));
            //}


            AddHeader2("Indirect role members");

            sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.RoleMembersIndirect.sql");

            dt = _db.GetDataTable(sql);

            _structuredText.AddParagraph($"Timestamp: {DateTime.Now:G}");
            _structuredText.AddParagraph($"User: {Environment.UserDomainName}\\{Environment.UserName}");
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddCode(
                $"{sql}\r\n\r\nResolving AD groups: .NET System.DirectoryServices.AccountManagement");
            _structuredText.AddParagraph($"Number of rows returned: --{dt.Rows.Count}--");
            _structuredText.AddParagraph(" ");


            var result = new DataTable();
            result.Clear();
            result.Columns.Add("Role");
            result.Columns.Add("User");
            result.Columns.Add("viaAdGroup");
            dt.AcceptChanges();

            foreach (DataRow r in dt.Rows)
            {

                //AddHeader3("Indirect role members for role " + r["Role"]);

                var group = r["UserName"].ToString();

                Debug.Print(group);

                var users = ActiveDirectoryHelper.GetUsernames(group);

                if (users == null) continue;

                if (users.Count == 0)
                {
                    var row = result.NewRow();
                    row["Role"] = r["Role"].ToString();
                    row["User"] = "No users found or AD not accessible";
                    row["viaAdGroup"] = group;
                    result.Rows.Add(row);
                }
                else
                {
                    foreach (var user in users)
                    {
                        var row = result.NewRow();
                        row["Role"] = r["Role"].ToString();
                        row["User"] = user;
                        row["viaAdGroup"] = group;
                        result.Rows.Add(row);
                    }
                }

                //_structuredText.AddDefinitionListLine(r["DatabaseRoleName"].ToString(),
                //    string.Format("{0} ({1})", r["DatabaseUserName"], r["type_desc"]));
            }

            _structuredText.AddTable(string.Empty, result);
       
        }
        catch (Exception ex)
        {
            var msg = $"SqlDocuDatabaseHelper:AddSecurityLoginsRoles '{_server}\\{_databaseName}'";
            _errors.Add(new Exception(msg, ex));
        }
    }



    /// <summary>
    /// Add info about the server
    /// </summary>
    public void AddServerInfo()
    {
        try
        {
            AddHeader2("General server info");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.SqlServer1.sql");

            var dt = _db.GetDataTable(sql);

            _structuredText.AddParagraph($"Timestamp: {DateTime.Now:G}");
            _structuredText.AddParagraph($"User: {Environment.UserDomainName}\\{Environment.UserName}");
            _structuredText.AddParagraph("Data source: ");
            _structuredText.AddParagraph($"Number of rows returned: --{dt.Rows.Count}--");
            _structuredText.AddParagraph(" ");

            _structuredText.AddTable(string.Empty, dt);
        }
        catch (Exception ex)
        {
            var msg = $"SqlDocuDatabaseHelper:AddDatabaseInfo '{_server}\\{_databaseName}'";
            _errors.Add(new Exception(msg, ex));
        }
    }

    /// <summary>
    /// Add info about database content
    /// </summary>
    public void AddDatabaseInfo()
    {
        try
        {
            AddHeader2("Info about tables");

            var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.TableInfo.sql");

            var dt = _db.GetDataTable(sql);

                

            _structuredText.AddTable(string.Empty, dt);
        }
        catch (Exception ex)
        {
            var msg = $"SqlDocuDatabaseHelper:AddDatabaseInfo '{_server}\\{_databaseName}'";
            _errors.Add(new Exception(msg, ex));
        }
    }

    /// <summary>
    /// Search for stored procs beginning with spDbDocu_ and call them to add their data to the documentation
    /// </summary>
    public void AddDocumentation()
    {



        var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.DatabasesDocuStoredProcs.sql");
        var dt = _db.GetDataTable(sql);

        foreach (DataRow r in dt.Rows)
        {

            var title = r["Name"].ToString().Replace("EXEC dbo.spDbDocu_", string.Empty).Replace("_", " ");
            AddHeader2(title);

            try
            {
                var docuSql = r["Name"].ToString();

                var docu = _db.GetDataTable(docuSql);
                _structuredText.AddTable(string.Empty, docu);
            }
            catch (Exception e)
            {
                _structuredText.AddParagraph($"Error:Sql:{r["Name"]}: {e}");
            }
        }
    }


    #region Helpers

    private void AddHeader1(string p, params object[] data)
    {
        switch (Header1Type)
        {
            case TextItemType.H2:
                _structuredText.AddHeader2(p, null, data);
                break;
            default:
                _structuredText.AddHeader1(p, null, data);
                break;
        }
    }


    //private void AddHeader2(string p, params object[] data)
    //{
    //    switch (Header2Type)
    //    {
    //        case TextItemType.H3:
    //            _structuredText.AddHeader2(p, data);
    //            break;
    //        default:
    //            _structuredText.AddHeader1(p, data);
    //            break;
    //    }
    //}

    private void AddHeader2(string p)
    {
        switch (Header2Type)
        {
            case TextItemType.H3:
                _structuredText.AddHeader3(p);
                break;
            default:
                _structuredText.AddHeader2(p);
                break;
        }
    }

    private void AddHeader3(string p)
    {
        switch (Header3Type)
        {
            case TextItemType.H4:
                _structuredText.AddHeader4(p);
                break;
            default:
                _structuredText.AddHeader3(p);
                break;
        }
    }

    #endregion

}
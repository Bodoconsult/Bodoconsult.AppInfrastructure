using System;
using System.Data;
using Bodoconsult.Core.Database;
using Bodoconsult.Core.Database.SqlClient;
using Bodoconsult.Core.Text.Enums;
using Bodoconsult.Core.Text.Model;
using JobExecuterCore.Base;
using JobExecuterCore.Dal.Helpers;
using JobExecuterCore.Dal.Model;

namespace JobExecuterCore.Dal.Executer
{
    /// <summary>
    /// Executer for sending a mail with suer access information of a all Sql databases on Ms SqlServer with exception of databases Master, Msdb, Model and TempDb
    /// </summary>
    internal class SqlMailUserAccessAllDatabasesExecuter : BaseExecuter
    {
        private const string ConnectionString = "Data Source={0};Initial Catalog=Master;Integrated Security=SSPI;";

        private readonly SqlMailUserAccessAllDatabasesJobTask _jobTask;

        private readonly StructuredText _result = new StructuredText();

        private IConnManager _db;

        /// <summary>
        /// Run job task
        /// </summary>
        /// <returns>true in case of success</returns>
        public override bool Run()
        {

            var sConn = string.Format(ConnectionString, _jobTask.Server);

            _db = SqlClientConnManager.GetConnManager(sConn);
            _db.SendStatus += SetStatus;

            _result.AddHeader1("Server user acess");

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

                    _result.AddHeader1("User access for  database '{0}'", dbName);

                    var docu = new SqlDocuDatabaseHelper(_result, r["name"].ToString(), _jobTask.Server, Errors,
                        SetStatus)
                    {
                        Header1Type = TextItemType.H1,
                        Header2Type = TextItemType.H2
                    };
                    
                    docu.AddSecurityRoles();
                    docu.AddSecurityLogins();
                    docu.AddSecurityLoginsRoles();
                   
                    SetStatus("Mailing user access for database '{0}' done!", dbName);

                }
                catch (Exception ex)
                {
                    var msg = $"SqlMailUserAccessAllDatabasesExecuter:Run:{dbName}";
                    Errors.Add(new Exception(msg, ex));
                }
            }

            SendMail(_jobTask.MailTo, string.Format("User access for SqlServer '{0}'", _jobTask.Server), _result);

            return true;
        }

        private void AddControlServerPermissions()
        {

            try
            {
                _result.AddHeader2("Control server permissions");

                var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.ControlServerPermission.sql");

                var dt = _db.GetDataTable(sql);

                _result.AddTable("", dt);
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

                _result.AddHeader2("User database access");

                var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.UserDatabaseAccess.sql");

                var dt = _db.GetDataTable(sql);

                _result.AddTable("", dt);
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
                _result.AddHeader2("Orphaned logins and their objects");

                var sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.OrphanedLogins.sql");

                var dt = _db.GetDataTable(sql);

                if (dt.Rows.Count > 0)
                {
                    _result.AddTable("", dt);
                    _result.AddParagraph("The orphaned logins own the following objects:");

                    sql = FileHelper.GetTextResource("JobExecuter.Dal.Resources.OrphanedLoginsObjects.sql");

                    dt = _db.GetDataTable(sql);

                    _result.AddTable("", dt);
                }
                else
                {
                    _result.AddParagraph("No orphaned logins found!");
                }
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="jobTask"></param>
        public SqlMailUserAccessAllDatabasesExecuter(IJobTask jobTask)
            : base(jobTask)
        {
            _jobTask = (SqlMailUserAccessAllDatabasesJobTask)jobTask;
        }
    }
}
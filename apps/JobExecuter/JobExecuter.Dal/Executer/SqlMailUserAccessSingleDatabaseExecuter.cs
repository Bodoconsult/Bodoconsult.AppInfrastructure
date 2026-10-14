using System;
using Bodoconsult.Core.Text.Model;
using JobExecuterCore.Base;
using JobExecuterCore.Dal.Helpers;
using JobExecuterCore.Dal.Model;

namespace JobExecuterCore.Dal.Executer
{
    /// <summary>
    /// Executer for sending a mail with suer access information of a Sql database on Ms SqlServer
    /// </summary>
    internal class SqlMailUserAccessSingleDatabaseExecuter : BaseExecuter
    {
        
        private readonly SqlMailUserAccessSingleDatabaseJobTask _jobTask;

        private readonly StructuredText _result = new StructuredText();

        /// <summary>
        /// Run job task
        /// </summary>
        /// <returns>true in case of success</returns>
        public override bool Run()
        {

            var dbName = $"{_jobTask.Server}\\{_jobTask.DatabaseName}";

            SetStatus("Start mailing user access for database'{0}'...", dbName);

            try
            {
                var docu = new SqlDocuDatabaseHelper(_result, _jobTask.DatabaseName, _jobTask.Server, Errors, SetStatus);
                docu.AddSecurityRoles();
                docu.AddSecurityLogins();
                docu.AddSecurityLoginsRoles();

                SendMail(_jobTask.MailTo, $"User access for SqlServer database '{dbName}'", _result);

                SetStatus("Mailing user access for database '{0}' done!", dbName);
                return true;
            }
            catch (Exception ex)
            {
                var msg = $"SqlMailUserAccessSingleDatabaseExecuter:Run:{dbName}";
                throw new Exception(msg, ex);
            }
        }


        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="jobTask"></param>
        public SqlMailUserAccessSingleDatabaseExecuter(IJobTask jobTask)
            : base(jobTask)
        {
            _jobTask = (SqlMailUserAccessSingleDatabaseJobTask)jobTask;
        }
    }
}
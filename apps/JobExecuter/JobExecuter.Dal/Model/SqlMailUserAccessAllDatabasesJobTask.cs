namespace JobExecuterCore.Dal.Model
{
    /// <summary>
    /// Task to send user access information via email to a responsible person for all SQL databases on a MSSqlServer with exception of databases Master, Msdb, Model and TempDb
    /// </summary>
    /// <example>
    /// <include file='..\Samples\SqlMailUserAccessAllDatabasesJobTask.xml' path='*'/>
    ///  </example>
    public class SqlMailUserAccessAllDatabasesJobTask : JobTaskBase
    {
        /// <summary>
        /// server name or server and instance name of a database server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Mail receiver(s) separated by semicolon
        /// </summary>
        public string MailTo { get; set; }

    }
}
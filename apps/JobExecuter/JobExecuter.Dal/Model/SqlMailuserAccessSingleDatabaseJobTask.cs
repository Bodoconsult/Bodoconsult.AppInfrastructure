namespace JobExecuterCore.Dal.Model
{
    /// <summary>
    /// Task to send user access information via email to a responsible person for a single SQL database on MSSqlServer
    /// </summary>
    /// <example>
    /// <include file='..\Samples\SqlMailUserAccessSingleDatabaseJobTask.xml' path='*'/>
    ///  </example>
    public class SqlMailUserAccessSingleDatabaseJobTask : JobTaskBase
    {
        /// <summary>
        /// server name or server and instance name of a database server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Name of the database to backup
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Mail receiver(s) separated by semicolon
        /// </summary>
        public string MailTo { get; set; }

    }
}
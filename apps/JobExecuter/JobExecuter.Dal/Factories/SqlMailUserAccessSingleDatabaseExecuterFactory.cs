using JobExecuterCore.Base;
using JobExecuterCore.Dal.Executer;

namespace JobExecuterCore.Dal.Factories
{
    /// <summary>
    /// Factory for <see cref="SqlMailUserAccessSingleDatabaseExecuter"/>
    /// </summary>
    internal class SqlMailUserAccessSingleDatabaseExecuterFactory : ITaskExecuterFactory
    {
        /// <summary>
        /// Create new executer
        /// </summary>
        /// <returns>New job task executer</returns>
        public ITaskExecuter New(IJobTask jobTask)
        {
            var e = new SqlMailUserAccessSingleDatabaseExecuter(jobTask);

            return e;
        }
    }
}
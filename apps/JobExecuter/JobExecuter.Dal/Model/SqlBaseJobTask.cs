using JobExecuter.Base;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Base class for Microsoft® SqlServer related job tasks
/// </summary>
public class SqlBaseJobTask : BaseJobTask
{
    /// <summary>
    /// server name or server and instance name of a database server
    /// </summary>
    public string Server { get; set; }
}
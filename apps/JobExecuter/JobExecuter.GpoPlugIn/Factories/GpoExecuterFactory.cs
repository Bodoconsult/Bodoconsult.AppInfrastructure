using JobExecuter.Base.Interfaces;
using JobExecuter.GpoPlugIn.Executer;

namespace JobExecuter.GpoPlugIn.Factories;

/// <summary>
/// Factory for <see cref="GpoExecuter"/>
/// </summary>
internal class GpoExecuterFactory : ITaskExecuterFactory
{
    /// <summary>
    /// Create new executer
    /// </summary>
    /// <returns>New job task executer</returns>
    public ITaskExecuter New(IJobTask jobTask)
    {
        var e = new GpoExecuter(jobTask);

        return e;
    }
}
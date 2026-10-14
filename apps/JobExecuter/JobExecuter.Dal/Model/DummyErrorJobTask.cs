using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Dummy task for error testing purposes
/// </summary>
public sealed class DummyErrorJobTask : BaseJobTask
{
    /// <summary>
    /// message to deliver for testing purposes
    /// </summary>
    public string Message { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new DummyErrorExecuterFactory();
    }

}
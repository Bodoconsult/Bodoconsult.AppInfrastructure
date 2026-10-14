using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.GpoPlugIn.Factories;

namespace JobExecuter.GpoPlugIn.Model;

/// <summary>
/// Task to create a report for each GPO and backup the GPO as XML for all GPOs of a Windows domain
/// </summary>
/// <remarks>Test on Windows 2016 servers and Windows 10 R1803 only. Windows Feature "GPO management" (deutsch: "Gruppenrichtlinienverwaltung") must be installed on the server the task is running on.</remarks>
public class GpoJobTask : BaseJobTask
{

    /// <summary>
    /// Directory to save the GPO reports and backups in
    /// </summary>
    public string TargetDirectory { get; set; }

    /// <summary>
    /// Domain to get the GPOs from. If empty, then the current user's domain is used
    /// </summary>
    public string Domain { get; set; }


    /// <summary>
    /// Clear old backup. Default: true;
    /// </summary>
    public bool ClearOldBackups { get; set; } = true;


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new GpoExecuterFactory();
    }

}
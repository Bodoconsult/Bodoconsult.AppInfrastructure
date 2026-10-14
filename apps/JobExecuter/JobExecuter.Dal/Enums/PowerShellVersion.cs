using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Enums;

/// <summary>
/// The version of PowerShell to run the <see cref="PsBatchJobTask"/> on
/// </summary>
public enum PowerShellVersion
{
    /// <summary>
    /// Runs on PowerShell version 01 or 02
    /// </summary>
    V01,
    /// <summary>
    /// Runs on PowerShell version 03
    /// </summary>
    V03
}
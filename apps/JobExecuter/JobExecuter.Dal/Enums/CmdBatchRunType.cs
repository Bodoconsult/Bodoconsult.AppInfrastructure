namespace JobExecuter.Dal.Enums;

/// <summary>
/// Type of how to run a cmd batch from a file or a string
/// </summary>
public enum CmdBatchRunType
{
    /// <summary>
    /// Run the complete batch file as one process (like calling it at the command prompt)
    /// </summary>
    WholeFile,
    /// <summary>
    /// Run the batch file or the batch string line per line as separated processes (lines must be independent from each other)
    /// </summary>
    LineByLine
}
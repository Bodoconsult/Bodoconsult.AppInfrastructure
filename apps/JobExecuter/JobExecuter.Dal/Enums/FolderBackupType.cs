namespace JobExecuter.Dal.Enums;

/// <summary>
/// Type of the folder backup based on robocopy.exe by Microsoft
/// </summary>
public enum FolderBackupType
{
    /// <summary>
    /// Copy source files to target with overwriting existing files
    /// </summary>
    PlainCopy,
    /// <summary>
    /// Copy source files to target but only if the source file is newer or the target file does not exist
    /// </summary>
    CopyOnlyNewerFiles,
    /// <summary>
    /// Mirror source to target. Files at target not existing in the source will be removed!
    /// </summary>
    Mirror
}
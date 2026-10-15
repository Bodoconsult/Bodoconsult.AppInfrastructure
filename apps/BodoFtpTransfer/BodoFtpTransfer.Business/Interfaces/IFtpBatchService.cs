// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace BodoFtpTransfer.Business.Interfaces;

/// <summary>
/// Interface for a FTP batch transfer service
/// </summary>
public interface IFtpBatchService
{
    /// <summary>
    /// Current status message delegate
    /// </summary>
    StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Directory names to exclude from transfer
    /// </summary>
    string ExcludeDirs { get; set; }

    /// <summary>
    /// File names to exclude from transfer
    /// </summary>
    string ExcludedFiles { get; set; }

    /// <summary>
    /// Relative path of the remote base directory to transfer to
    /// </summary>
    string RemoteDirectory { get; set; }

    /// <summary>
    /// Path for the batch file
    /// </summary>
    string FileName { get; set; }

    /// <summary>
    /// Local base directory to transfer
    /// </summary>
    string BaseDir { get; set; }

    /// <summary>
    /// Start command collecting for saving db connections
    /// </summary>
    void StartCommandCollecting();

    /// <summary>
    /// Run all the collected commands on one connection
    /// </summary>
    void RunCommandsFromCollection();

    /// <summary>
    /// Check if a file was tranferred already
    /// </summary>
    /// <param name="fo">File to check</param>
    void CheckFile(FileInfo fo);

    /// <summary>
    /// Create a directory if necessary
    /// </summary>
    void CheckDirectory(string localPath);

    /// <summary>
    /// Quit the connection to the server
    /// </summary>
    void Quit();

    /// <summary>
    /// Check if remote file is existing by size in bytes
    /// </summary>
    /// <param name="remotePath">Remote file path</param>
    /// <param name="size">Current size in bytes</param>
    void CheckRemoteFile(string remotePath, long size);

    /// <summary>
    /// Check if a remote directory is existing
    /// </summary>
    /// <param name="remotePath">Remote directory path</param>
    void CheckRemoteDir(string remotePath);

    /// <summary>
    /// Get the hash codes
    /// </summary>
    void GetHashCodes();

    /// <summary>
    /// Set the hash codes for remote files
    /// </summary>
    void SetzeHashcodesForRemoteFiles();

    /// <summary>
    /// Create a directory on the remote server
    /// </summary>
    void CreateRemoteDirectory();

    /// <summary>
    /// Remove a directory on the remote server
    /// </summary>
    void RemoveRemoteDirectory();

    /// <summary>
    /// Remove a files on the remote server not needed any more
    /// </summary>
    void RemoveRemoteFiles();

    /// <summary>
    /// Copy local files to FTP server
    /// </summary>
    void CopyLocalFiles();

    /// <summary>
    /// Open the connection to the server
    /// </summary>
    void Open();

    /// <summary>
    /// Get the remote files
    /// </summary>
    /// <param name="remotePath">Remote directory path</param>
    void GetRemoteData(string remotePath);

    /// <summary>
    /// Remove a remote directory
    /// </summary>
    /// <param name="remotePath">Remote directory path</param>
    void RmDir(string remotePath);

    /// <summary>
    /// Put a local file to the remote server
    /// </summary>
    /// <param name="localPath">Local file path</param>
    /// <param name="remotePath">Remote file path</param>
    /// <returns>True on success else false</returns>
    bool Put(string localPath, string remotePath);

    /// <summary>
    /// Make a remote directory
    /// </summary>
    /// <param name="remotePath">Remote directory path</param>
    void MkDir(string remotePath);

    /// <summary>
    /// Delete a remote file
    /// </summary>
    /// <param name="remotePath">Remote file path</param>
    void Del(string remotePath);

    /// <summary>
    /// Get the local inventory of files and directories
    /// </summary>
    /// <param name="d">Directory to check</param>
    void GetLocalData(DirectoryInfo d);

    /// <summary>
    /// Load all file paths from database
    /// </summary>
    void LoadAllFilePaths();
}
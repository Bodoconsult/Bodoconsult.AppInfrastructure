// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
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
    /// Pfad zur Batch-Datei
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
    /// Prüfe, ob Datei bereits übertragen wurde oder nicht
    /// </summary>
    /// <param name="fo"> </param>
    void CheckFile(FileInfo fo);

    /// <summary>
    /// Verzeichnis anlegen
    /// </summary>
    void CheckDirectory(string localPath);

    void Quit();
    void CheckRemoteFile(string remotePath, long size);
    void CheckRemoteDir(string remotePath);
    void GetHashCodes();
    void SetzeHashcodesFürRemoteFiles();
    void CreateRemoteDirectory();
    void RemoveRemoteDirectory();
    void RemoveRemoteFiles();

    /// <summary>
    /// Copy local files to FTP server
    /// </summary>
    void CopyLocalFiles();

    void Open();
    void GetRemoteData(string remotePath);
    void RmDir(string remotePath);
    bool Put(string localPath, string remotePath);
    void MkDir(string remotePath);
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
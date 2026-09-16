// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.Web.Ftp.Models;
using System.Collections.Generic;

namespace Bodoconsult.Web.Ftp.Interfaces;

/// <summary>
/// Interface for remote server file handler
/// </summary>
public interface IRemoteServerHandler
{
    /// <summary>
    /// Connect to the SFTP-Server
    /// </summary>
    void Connect();

    /// <summary>
    /// Is SFTP server connected?
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Disconnect from SFTP-Server
    /// </summary>
    void Disconnect();

    /// <summary>
    /// Delete a remote file on the SFTP server
    /// </summary>
    /// <param name="remotePath"></param>
    void RemoveFile(string remotePath);

    /// <summary>
    /// Upload a local file to the remote SFTP server
    /// </summary>
    /// <param name="localPath"></param>
    /// <param name="remotePath"></param>
    void Put(string localPath, string remotePath);

    /// <summary>
    /// Exists path on SFTP server
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool Exists(string path);

    /// <summary>
    /// Create a remote directory on the SFTP server
    /// </summary>
    /// <param name="remotePath"></param>
    void CreateDirectory(string remotePath);

    /// <summary>
    /// Remove a remote directory
    /// </summary>
    /// <param name="remotePath">Remte directory path</param>
    void RemoveDirectory(string remotePath);

    /// <summary>
    /// Get items in a directory on the remote server
    /// </summary>
    /// <param name="remotePath">Remote path to check</param>
    /// <returns>List of items in the remote path </returns>
    IEnumerable<SftpFileItem> GetDirectoryItems(string remotePath);

    /// <summary>
    /// Download a file from the remote FTP server
    /// </summary>
    /// <param name="remotePath">Remote path on the FTP server</param>
    /// <param name="localPath">Local path to store the downloaded file</param>
    void DownloadFile(string remotePath, string localPath);
}
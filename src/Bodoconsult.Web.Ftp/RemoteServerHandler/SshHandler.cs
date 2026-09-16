// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using System.IO;
using Bodoconsult.Web.Ftp.Interfaces;
using Bodoconsult.Web.Ftp.Models;
using Renci.SshNet;

namespace Bodoconsult.Web.Ftp.RemoteServerHandler;

/// <summary>
/// Handles SSH access to FTP server
/// </summary>
public class SshHandler : IRemoteServerHandler
{
    private SftpClient _sftp;

    private readonly SshCredentials _credentials;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="credentials">FTP server credentials</param>
    public SshHandler(SshCredentials credentials)
    {
        _credentials = credentials;
    }

    /// <summary>
    /// Connect to the SFTP-Server
    /// </summary>
    public void Connect()
    {
        _sftp = new SftpClient(_credentials.Url, _credentials.Username, _credentials.Password);
        _sftp.Connect();
    }

    /// <summary>
    /// Is SFTP server connected?
    /// </summary>
    public bool IsConnected => _sftp is { IsConnected: true };


    /// <summary>
    /// Disconnect from SFTP-Server
    /// </summary>
    public void Disconnect()
    {
        _sftp.Disconnect();
        _sftp.Dispose();
    }

    /// <summary>
    /// Delete a remote file on the SFTP server
    /// </summary>
    /// <param name="remotePath"></param>
    public void RemoveFile(string remotePath)
    {
        try
        {
            if (_sftp.Exists(remotePath))
            {
                _sftp.DeleteFile(remotePath);
            }
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Upload a local file to the remote SFTP server
    /// </summary>
    /// <param name="localPath"></param>
    /// <param name="remotePath"></param>
    public void Put(string localPath, string remotePath)
    {
        var path = Path.GetDirectoryName(remotePath)?.Replace("\\", "/");
        var fileName = Path.GetFileName(remotePath);

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        _sftp.ChangeDirectory(path);

        using var fileStream = new FileStream(localPath, FileMode.Open);
        _sftp.BufferSize = 4 * 1024; // bypass Payload error large files 
        _sftp.UploadFile(fileStream, fileName, true);
    }

    /// <summary>
    /// Exists path on SFTP server
    /// </summary>
    /// <param name="path">Remote path</param>
    /// <returns>True if the remote path exists else false</returns>
    public bool Exists(string path)
    {
        return _sftp.Exists(path);
    }

    /// <summary>
    /// Create a remote directory on the SFTP server
    /// </summary>
    /// <param name="remotePath"></param>
    public void CreateDirectory(string remotePath)
    {
        _sftp.CreateDirectory(remotePath);
    }

    /// <summary>
    /// Remove a remote directory
    /// </summary>
    /// <param name="remotePath">Remte directory path</param>
    public void RemoveDirectory(string remotePath)
    {
        try
        {
            if (_sftp.Exists(remotePath))
            {
                _sftp.DeleteDirectory(remotePath);
            }
        }
        catch
        {
            // Ignored
        }
    }

    /// <summary>
    /// Get file items in the remote path
    /// </summary>
    /// <param name="remotePath">Remote path to search items located in</param>
    /// <returns>List of files located in the remote path</returns>
    public IEnumerable<Renci.SshNet.Sftp.ISftpFile> GetDirectoryItemsRaw(string remotePath)
    {
        return _sftp.ListDirectory(remotePath);

        //string remoteFileName = file.Name;
        //if ((!file.Name.StartsWith(".")) && ((file.LastWriteTime.Date == DateTime.Today))

        //    using (Stream file1 = File.OpenWrite(localDirectory + remoteFileName))
        //    { 
        //        sftp.DownloadFile(remoteDirectory + remoteFileName, file1);
        //    }
    }

    /// <summary>
    /// Get file items in the remote path
    /// </summary>
    /// <param name="remotePath">Remote path to search items located in</param>
    /// <returns>List of files located in the remote path</returns>
    public IEnumerable<SftpFileItem> GetDirectoryItems(string remotePath)
    {
        var result = new List<SftpFileItem>();
        var ftp = _sftp.ListDirectory(remotePath);

        foreach (var item in ftp)
        {
            var i = new SftpFileItem
            {
                Name = item.Name,
                FullName = item.FullName,
                IsDirectory = item.IsDirectory,
                LastAccessTime = item.LastAccessTime,
                LastAccessTimeUtc = item.LastAccessTimeUtc,
                LastWriteTime = item.LastWriteTime,
                LastWriteTimeUtc = item.LastWriteTimeUtc,
                Length = item.Length
            };

            result.Add(i);
        }

        //string remoteFileName = file.Name;
        //if ((!file.Name.StartsWith(".")) && ((file.LastWriteTime.Date == DateTime.Today))

        //    using (Stream file1 = File.OpenWrite(localDirectory + remoteFileName))
        //    { 
        //        sftp.DownloadFile(remoteDirectory + remoteFileName, file1);
        //    }

        return result;
    }

    /// <summary>
    /// Download a file from the remote FTP server
    /// </summary>
    /// <param name="remotePath">Remote path on the FTP server</param>
    /// <param name="localPath">Local path to store the downloaded file</param>
    public void DownloadFile(string remotePath, string localPath)
    {
        using Stream file1 = File.OpenWrite(localPath);
        _sftp.DownloadFile(remotePath, file1);
    }
}
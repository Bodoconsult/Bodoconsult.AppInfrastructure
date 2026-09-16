// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Web.Ftp.Models;

/// <summary>
/// Credentials for SFTP login
/// </summary>
public class SshCredentials
{
    /// <summary>
    /// FTP server address 
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Username for the FTP server
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Password for the FTP server
    /// </summary>
    public string Password { get; set; }
}
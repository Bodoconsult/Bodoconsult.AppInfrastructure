// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Information about a local share
/// </summary>
public class Share
{
    #region Private data

    private string _server;
    private string _netName;
    private string _path;
    private ShareType _shareType;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="server">Server name or IP address</param>
    /// <param name="netName">Net name</param>
    /// <param name="path">Local path</param>
    /// <param name="shareType">Share type</param>
    /// <param name="remark">Bemerkung</param>
    public Share(string server, string netName, string path, ShareType shareType, string remark)
    {
        if (ShareType.Special == shareType && "IPC$" == netName)
        {
            shareType |= ShareType.IPC;
        }

        _server = server;
        _netName = netName;
        _path = path;
        _shareType = shareType;
        Remark = remark;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The name of the computer that this share belongs to
    /// </summary>
    public string Server => _server;

    /// <summary>
    /// Share name
    /// </summary>
    public string NetName => _netName;

    /// <summary>
    /// Local path
    /// </summary>
    public string Path => _path;

    /// <summary>
    /// Share type
    /// </summary>
    public ShareType ShareType => _shareType;

    /// <summary>
    /// Comment
    /// </summary>
    public string Remark { get; }

    /// <summary>
    /// Returns true if this is a file system share
    /// </summary>
    public bool IsFileSystem
    {
        get
        {
            // Shared device
            if (0 != (_shareType & ShareType.Device)) return false;
            // IPC share
            if (0 != (_shareType & ShareType.IPC)) return false;
            // Shared printer
            if (0 != (_shareType & ShareType.Printer)) return false;

            // Standard disk share
            if (0 == (_shareType & ShareType.Special)) return true;

            // Special disk share (e.g. C$)
            return ShareType.Special == _shareType && !string.IsNullOrEmpty(_netName);
        }
    }

    /// <summary>
    /// Get the root of a disk-based share
    /// </summary>
    public DirectoryInfo Root
    {
        get
        {
            if (!IsFileSystem) return null;
            if (string.IsNullOrEmpty(_server))
                return string.IsNullOrEmpty(_path) ? new DirectoryInfo(ToString()) : new DirectoryInfo(_path);
            return new DirectoryInfo(ToString());

        }
    }

    #endregion

    /// <summary>
    /// Returns the path to this share
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.IsNullOrEmpty(_server) ? $@"\\{Environment.MachineName}\{_netName}" : $@"\\{_server}\{_netName}";
    }

    /// <summary>
    /// Returns true if this share matches the local path
    /// </summary>
    /// <param name="path">Local path</param>
    /// <returns></returns>
    public bool MatchesPath(string path)
    {
        if (!IsFileSystem) return false;
        if (string.IsNullOrEmpty(path)) return true;

        return path.ToLower().StartsWith(_path.ToLower());
    }
}
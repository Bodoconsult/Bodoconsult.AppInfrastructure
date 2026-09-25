// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace BodoFtpTransfer.Business.Interfaces;

/// <summary>
/// Interface for main handling of FTP transfer
/// </summary>
public interface IFtpTransferService
{
    /// <summary>
    /// Current status message delegate
    /// </summary>
    StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Transfer mode
    /// 0 = transfer all
    /// 1 = mark all files and directories as transferred already
    /// </summary>
    int Modus { get; set; }

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
    /// Local base directory to transfer
    /// </summary>
    string BaseDirectory { get; set; }

    /// <summary>
    /// Create batch file for FTP transfer
    /// </summary>
    void CreateBatch();
}
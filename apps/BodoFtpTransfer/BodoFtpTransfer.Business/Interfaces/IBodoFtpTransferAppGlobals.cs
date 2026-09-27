// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataProtection.Interfaces;

namespace BodoFtpTransfer.Business.Interfaces;

public interface IBodoFtpTransferAppGlobals: IBasicCredentialsAppGlobals
{
    /// <summary>
    /// Directory names to exclude from transfer
    /// </summary>
    string ExcludeDirs { get; set; }

    /// <summary>
    /// File names to exclude from transfer
    /// </summary>
    string ExcludeFiles { get; set; }

    /// <summary>
    /// Relative path of the remote base directory to transfer to
    /// </summary>
    string RemoteDirectory { get; set; }

    /// <summary>
    /// Local base directory to transfer
    /// </summary>
    string BaseDirectory { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for factories creating <see cref="IBackupTarget"/> instances
/// </summary>
public interface IBackupTargetFactory
{
    /// <summary>
    /// Create an instance of <see cref="IBackupTarget"/>
    /// </summary>
    /// <param name="backupTargetSettings">Settings to use for the backup target</param>
    /// <returns><see cref="IBackupTarget"/> instance</returns>
    IBackupTarget CreateInstance(IBackupTargetSettings backupTargetSettings);
}
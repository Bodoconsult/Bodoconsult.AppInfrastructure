// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Windows.Backup;

/// <summary>
/// Implementation of <see cref="IBackupTargetFactory"/> creating a <see cref="RobocopyBackupTarget"/> instance
/// </summary>
public class RobocopyBackupTargetFactory : IBackupTargetFactory
{
    private readonly IShellProcessManager _shellProcessManager;

    /// <summary>
    /// Defaul ctor
    /// </summary>
    /// <param name="shellProcessManager">Current shell process manager</param>
    public RobocopyBackupTargetFactory(IShellProcessManager shellProcessManager)
    {
        _shellProcessManager = shellProcessManager;
    }

    /// <summary>
    /// Create an instance of <see cref="IBackupTarget"/>
    /// </summary>
    /// <param name="backupTargetSettings">Settings to use for the backup target</param>
    /// <returns><see cref="IBackupTarget"/> instance</returns>
    public IBackupTarget CreateInstance(IBackupTargetSettings backupTargetSettings)
    {
        return new RobocopyBackupTarget(_shellProcessManager, backupTargetSettings);
    }
}
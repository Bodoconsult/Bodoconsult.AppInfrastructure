// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for backup manager implementations
/// </summary>
public interface IBackupManager
{
    /// <summary>
    /// Current list of errors
    /// </summary>
    IList<Exception>? Errors { get; }

    /// <summary>
    /// Status changed event
    /// </summary>
    StatusMessageDelegate? StatusChanged { get; }

    /// <summary>
    /// Current backup config
    /// </summary>
    IBackupConfig BackupConfig { get; }

    /// <summary>
    /// Activate shadowing if necessary
    /// </summary>
    void ActivateShadowing();

    /// <summary>
    /// Deactivate shadowing
    /// </summary>
    void DeactivateShadowing();

    /// <summary>
    /// Create a summary file for the backup process
    /// </summary>
    void CreateSummaryFile();

    /// <summary>
    /// Check if the storage media is available
    /// </summary>
    void CheckStorageMedia();

    /// <summary>
    /// Backup starten mit den vorher aus BodoBackupSettings-Datei eingelesenen Einstellungen
    /// </summary>
    void StartBackup();
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Time intervals for backup 
/// </summary>
public enum BackupModeEnum
{
    /// <summary>
    /// Simple: only one backup is stored
    /// </summary>
    Simple,
    /// <summary>
    /// Backups for  certain number of days are kept
    /// </summary>
    Days,
    /// <summary>
    /// 
    /// </summary>
    DaysDate,
    /// <summary>
    /// A certain number of weeks backups are kept
    /// </summary>
    Week,
    /// <summary>
    /// 
    /// </summary>
    Week1,
    /// <summary>
    /// A certain number of months backups are kept
    /// </summary>
    Month
}